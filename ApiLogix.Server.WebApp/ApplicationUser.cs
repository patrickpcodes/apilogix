using Microsoft.AspNetCore.Identity;

namespace ApiLogix.Server.WebApp;

public class ApplicationUser : IdentityUser
{
    // Add ApiKey as an additional property
    public Guid ApiKey { get; set; }
}