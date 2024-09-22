using System.Data;
using Dapper;

namespace ApiLogix.Server.WebApp;

public class ApiUrlWithLogs
{
    public Guid UrlId { get; set; }
    public string BaseUrl { get; set; }
    public string ApiName { get; set; }

    // List of logs associated with this URL
    public List<ApiLog> Logs { get; set; } = new List<ApiLog>();
}

public class ApiLog
{
    public Guid LogId { get; set; }
    public string HttpMethod { get; set; }
    public string ApiPath { get; set; }
    public int StatusCode { get; set; }
    public int ResponseTimeMs { get; set; }
    public DateTime Timestamp { get; set; }
    public string Payload { get; set; }
    public string Response { get; set; }
    public string ErrorMessage { get; set; }
}


public interface IApiLogRepository
{
    Task<Guid> InsertApiLogAsync(Guid userId, Guid urlId, string httpMethod, string apiPath, int statusCode,
        int responseTimeMs, string payload = null, string response = null, string errorMessage = null);

    Task<List<ApiUrlWithLogs>> GetUserLogsAsync(string userId);
}

public class ApiLogRepository : IApiLogRepository
{
    private readonly IDbConnection _dbConnection;

    public ApiLogRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Guid> InsertApiLogAsync(Guid userId, Guid urlId, string httpMethod, string apiPath,
        int statusCode, int responseTimeMs, string payload = null, string response = null, string errorMessage = null)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@UrlId", urlId);
            parameters.Add("@HttpMethod", httpMethod);
            parameters.Add("@ApiPath", apiPath);
            parameters.Add("@StatusCode", statusCode);
            parameters.Add("@ResponseTimeMs", responseTimeMs);
            parameters.Add("@Payload", payload);
            parameters.Add("@Response", response);
            parameters.Add("@ErrorMessage", errorMessage);

            // Call the InsertApiLog stored procedure
            var logId = await _dbConnection.QuerySingleAsync<Guid>(
                "InsertApiLog",
                parameters,
                commandType: CommandType.StoredProcedure);
            return logId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception(ex.Message);
        }
    }
    
    public async Task<List<ApiUrlWithLogs>> GetUserLogsAsync(string userId)
    {
        var urlDictionary = new Dictionary<Guid, ApiUrlWithLogs>();

        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);

        // Execute the stored procedure
        var result = await _dbConnection.QueryAsync<ApiUrlWithLogs, ApiLog, ApiUrlWithLogs>(
            "GetUserLogs",
            (apiUrl, apiLog) =>
            {
                // Check if the URL already exists in the dictionary
                if (!urlDictionary.TryGetValue(apiUrl.UrlId, out var currentApiUrl))
                {
                    currentApiUrl = apiUrl;
                    urlDictionary.Add(apiUrl.UrlId, currentApiUrl);
                }

                // Add the log to the corresponding URL
                if (apiLog != null)
                {
                    currentApiUrl.Logs.Add(apiLog);
                }

                return currentApiUrl;
            },
            parameters,
            splitOn: "LogId",  // This tells Dapper to map ApiUrl and ApiLog based on LogId
            commandType: CommandType.StoredProcedure
        );

        // Return the list of URLs with their associated logs
        return urlDictionary.Values.ToList();
    }
}