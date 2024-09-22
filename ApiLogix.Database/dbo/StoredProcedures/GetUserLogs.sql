CREATE PROCEDURE GetUserLogs
    @UserId NVARCHAR(450)  -- Assuming UserId is stored as string (consistent with Identity)
AS
BEGIN
    SELECT 
        ApiUrls.UrlId,
        ApiUrls.BaseUrl,
        ApiUrls.ApiName,
        ApiLogs.LogId,
        ApiLogs.HttpMethod,
        ApiLogs.ApiPath,
        ApiLogs.StatusCode,
        ApiLogs.ResponseTimeMs,
        ApiLogs.Timestamp,
        ApiLogs.Payload,
        ApiLogs.Response,
        ApiLogs.ErrorMessage
    FROM 
        ApiUrls
    LEFT JOIN 
        ApiLogs ON ApiUrls.UrlId = ApiLogs.UrlId
    WHERE 
        ApiUrls.UserId = @UserId
    ORDER BY 
        ApiUrls.BaseUrl, ApiLogs.Timestamp;
END;
