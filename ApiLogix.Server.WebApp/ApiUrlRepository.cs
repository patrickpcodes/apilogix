using System.Data;
using Dapper;

namespace ApiLogix.Server.WebApp;

public interface IApiUrlRepository
{
    Task<Guid> InsertApiUrlAsync(Guid userId, string baseUrl, string apiName);
}

public class ApiUrlRepository : IApiUrlRepository
{
    private readonly IDbConnection _dbConnection;

    public ApiUrlRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Guid> InsertApiUrlAsync(Guid userId, string baseUrl, string apiName)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId);
        parameters.Add("@BaseUrl", baseUrl);
        parameters.Add("@ApiName", apiName);

        // Call the InsertApiUrl stored procedure
        var urlId = await _dbConnection.QuerySingleAsync<Guid>(
            "InsertApiUrl", 
            parameters, 
            commandType: CommandType.StoredProcedure);

        return urlId;
    }
}
