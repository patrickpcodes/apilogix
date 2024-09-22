namespace ApiLogix.Server.WebApp.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class ApiController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApiController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetApiKey()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var apiKey = user.ApiKey.ToString();
        return Ok(new { ApiKey = apiKey });
    }
}
