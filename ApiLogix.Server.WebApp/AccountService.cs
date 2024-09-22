namespace ApiLogix.Server.WebApp;

using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

public class AccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterUserAsync(string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            ApiKey = Guid.NewGuid()  // Generate a new API key for the user
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            // Optionally, add roles or send confirmation email here
        }

        return result;
    }

    public async Task<string> GetUserApiKeyAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.ApiKey.ToString();
    }
}
