using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using ApiLogix.Server.WebApp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity with EF Core
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddTransient<IApiUrlRepository, ApiUrlRepository>();
builder.Services.AddTransient<IApiLogRepository, ApiLogRepository>();
builder.Services.AddTransient<SeedService>();
builder.Services.AddTransient<ApiService>();
builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Apply migrations and seed the database with the user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var configuration = services.GetRequiredService<IConfiguration>();
    
    // Apply any pending migrations
    context.Database.Migrate();

    // Seed the user from appsettings
    await SeedUserAsync(userManager, configuration);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();


async Task SeedUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
{
    var seedUserName = configuration["SeedUser:UserName"];
    var seedPassword = configuration["SeedUser:Password"];

    if (string.IsNullOrEmpty(seedUserName) || string.IsNullOrEmpty(seedPassword))
    {
        throw new Exception("Seed user credentials are not provided in the appsettings.json.");
    }

    var user = await userManager.FindByNameAsync(seedUserName);
    
    if (user == null)
    {
        // Create the seed user if they don't exist
        var seedUser = new ApplicationUser
        {
            UserName = seedUserName,
            Email = $"{seedUserName}@example.com", // Optional
            ApiKey = Guid.NewGuid()
        };

        var result = await userManager.CreateAsync(seedUser, seedPassword);

        if (!result.Succeeded)
        {
            throw new Exception("Failed to create seed user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        Console.WriteLine("Seed user created successfully.");
    }
    else
    {
        Console.WriteLine("Seed user already exists.");
    }
}