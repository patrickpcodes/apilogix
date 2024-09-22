using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLogix.Server.WebApp.Controllers;

public class ApiLogRequest
{
    public string BaseUrl { get; set; }
    public string ApiName { get; set; }
    public string HttpMethod { get; set; }
    public string ApiPath { get; set; }
    public int StatusCode { get; set; }
    public int ResponseTimeMs { get; set; }
}

[Route("api/[controller]")]
[ApiController]
[Authorize] // Ensure this endpoint is authenticated
public class ApiLogController : ControllerBase
{
    private readonly ApiService _apiService;

    public ApiLogController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs()
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

        var items = await _apiService.GetUserLogsAsync(userGuid.ToString());
        return Ok(items);
    }


    [HttpPost("log-request")]
    public async Task<IActionResult> LogRequest([FromBody] ApiLogRequest logRequest)
    {
        // Get the authenticated user's username from the JWT token
        var userName = User.Identity.Name;

        if (string.IsNullOrEmpty(userName))
        {
            return Unauthorized("User not found.");
        }

        // Log the API request for the authenticated user
        await _apiService.LogApiRequestAsync(
            userName,
            logRequest.BaseUrl,
            logRequest.ApiName,
            logRequest.HttpMethod,
            logRequest.ApiPath,
            logRequest.StatusCode,
            logRequest.ResponseTimeMs);

        return Ok("API request logged successfully.");
    }
}