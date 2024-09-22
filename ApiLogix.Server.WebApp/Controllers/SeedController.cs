
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ApiLogix.Server.WebApp.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]  // This ensures that the user must be authenticated to access this route
public class SeedController : ControllerBase
{
    private readonly SeedService _seedService;

    public SeedController(SeedService seedService)
    {
        _seedService = seedService;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedDatabase()
    {
        // Get the authenticated user's UserId from the JWT token
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userId == null)
        {
            return Unauthorized("User ID not found.");
        }

        // Convert string userId to Guid
        if (!Guid.TryParse(userId, out Guid userGuid))
        {
            return BadRequest("Invalid User ID.");
        }

        // Call the SeedService and pass the authenticated user's UserId
        await _seedService.SeedDatabaseWithApiLogs(userGuid);

        return Ok("Database seeded with 1000 random requests.");
    }
}
