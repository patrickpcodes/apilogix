CREATE PROCEDURE InsertApiLog
    @UserId UNIQUEIDENTIFIER,
    @BaseUrl NVARCHAR(500),
    @ApiName NVARCHAR(255),
    @HttpMethod NVARCHAR(10),
    @ApiPath NVARCHAR(500),
    @StatusCode INT,
    @ResponseTimeMs INT,
    @Payload NVARCHAR(MAX) = NULL,  -- Optional parameter
    @Response NVARCHAR(MAX) = NULL,  -- Optional parameter
    @ErrorMessage NVARCHAR(MAX) = NULL  -- Optional parameter
AS
BEGIN
    DECLARE @UrlId UNIQUEIDENTIFIER;

    -- Call the InsertApiUrl procedure to get the UrlId (or create it if it doesn't exist)
    EXEC @UrlId = InsertApiUrl @UserId, @BaseUrl, @ApiName;

    -- Insert the new API log
    INSERT INTO ApiLogs (LogId, UrlId, HttpMethod, ApiPath, StatusCode, ResponseTimeMs, Timestamp, Payload, Response, ErrorMessage)
    VALUES (NEWID(), @UrlId, @HttpMethod, @ApiPath, @StatusCode, @ResponseTimeMs, GETDATE(), @Payload, @Response, @ErrorMessage);
    
    -- Return the newly inserted LogId
    SELECT SCOPE_IDENTITY() AS LogId;
END;
