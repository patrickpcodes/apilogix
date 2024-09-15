
namespace ApiLogix.Server.WebApp;

public class Program
{
    public static void Main( string[] args )
    {
        var builder = WebApplication.CreateBuilder( args );

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseHttpsRedirection();


        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI( options =>
        {
            options.SwaggerEndpoint( "/swagger/v1/swagger.json", "My API v1" );
        } );


        app.UseCors( builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader() );

        app.MapControllers();
        app.MapFallbackToFile( "index.html" );
        app.MapControllers();

        app.Run();
    }
}
