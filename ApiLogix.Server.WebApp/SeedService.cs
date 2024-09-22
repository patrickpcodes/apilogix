namespace ApiLogix.Server.WebApp;

using System;
using System.Linq;
using System.Threading.Tasks;

public class SeedService
{
    private readonly IApiLogRepository _apiLogRepository;
    private readonly IApiUrlRepository _apiUrlRepository;

    private static readonly string[] BaseUrls = new[]
    {
        "https://api.example.com",
        "https://api.another.com",
        "https://api.thirdservice.com",
        "https://api.fourthservice.com",
        "https://api.fifthservice.com",
    };

    private static readonly string[] Paths = new[]
    {
        "/v1/resource",
        "/v2/resource",
        "/v1/data",
        "/v2/data",
        "/v1/info"
    };

    public SeedService(IApiLogRepository apiLogRepository, IApiUrlRepository apiUrlRepository)
    {
        _apiLogRepository = apiLogRepository;
        _apiUrlRepository = apiUrlRepository;
    }

    public async Task SeedDatabaseWithApiLogs(Guid userId)
    {
        var random = new Random();
        var numOfApiUrls = 5;
        var apiGuids = new Guid[numOfApiUrls];
        for (int i = 0; i < numOfApiUrls; i++)
        {
            var baseUrl = BaseUrls.ElementAt(random.Next(0, BaseUrls.Length));
            var apiName = $"Api Name For Testing {i:D2}";
            var urlId = await _apiUrlRepository.InsertApiUrlAsync(userId, baseUrl, apiName);
            apiGuids[i] = urlId;
        }

        var validCodes = new List<int>(){200, 201, 400, 404};
        for (int i = 0; i < 1000; i++)
        {
            var apiUrl = apiGuids[random.Next(numOfApiUrls)];
            var apiPath = Paths[random.Next(Paths.Length)];
            var httpMethod = random.Next(0, 2) == 0 ? "GET" : "POST";
            var statusCode = validCodes[random.Next(validCodes.Count)];
            var responseTimeMs = random.Next(50, 1000);


            // Insert into ApiLogs
            await _apiLogRepository.InsertApiLogAsync(
                userId,
                apiUrl,
                httpMethod,
                apiPath,
                statusCode,
                responseTimeMs);
        }
    }
}