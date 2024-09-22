using Microsoft.AspNetCore.Identity;

namespace ApiLogix.Server.WebApp;

using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

public interface IApiService
{
    Task<List<ApiUrlWithLogs>> GetUserLogsAsync(string userId);

    Task LogApiRequestAsync(string userName, string baseUrl, string apiName, string httpMethod,
        string apiPath, int statusCode, int responseTimeMs);
}


public class ApiService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApiLogRepository _apiLogRepository;
    private readonly IApiUrlRepository _apiUrlRepository;

    public ApiService(UserManager<ApplicationUser> userManager, IApiLogRepository apiLogRepository, IApiUrlRepository apiUrlRepository)
    {
        _userManager = userManager;
        _apiLogRepository = apiLogRepository;
        _apiUrlRepository = apiUrlRepository;
    }

    // Log an API request for the authenticated user
    public async Task LogApiRequestAsync(string userName, string baseUrl, string apiName, string httpMethod, string apiPath, int statusCode, int responseTimeMs)
    {
        // Retrieve the user from the Identity database
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Convert the string UserId to Guid
        Guid userGuid;
        if (!Guid.TryParse(user.Id, out userGuid))
        {
            throw new Exception("UserId is not a valid GUID");
        }

        // Insert the API URL associated with the user
        var urlId = await _apiUrlRepository.InsertApiUrlAsync(userGuid, baseUrl, apiName);

        // Insert the API log for the user's request
        await _apiLogRepository.InsertApiLogAsync(
            userGuid,
            urlId,
            httpMethod,
            apiPath,
            statusCode,
            responseTimeMs);
    }
    
    public async Task<List<ApiUrlWithLogs>> GetUserLogsAsync(string userId)
    {
        return await _apiLogRepository.GetUserLogsAsync(userId);
    }
}
