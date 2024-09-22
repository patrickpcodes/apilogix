CREATE PROCEDURE InsertApiLog
    @UserId UNIQUEIDENTIFIER,
    @UrlId UNIQUEIDENTIFIER,
    @HttpMethod NVARCHAR(10),
    @ApiPath NVARCHAR(500),
    @StatusCode INT,
    @ResponseTimeMs INT,
    @Payload NVARCHAR(MAX) = NULL,  -- Optional parameter
    @Response NVARCHAR(MAX) = NULL,  -- Optional parameter
    @ErrorMessage NVARCHAR(MAX) = NULL  -- Optional parameter
AS
BEGIN

	DECLARE @LogId UNIQUEIDENTIFIER
    -- Insert the new API log
	SET @LogId = NEWID();
    INSERT INTO ApiLogs (LogId, UrlId, HttpMethod, ApiPath, StatusCode, ResponseTimeMs, Timestamp, Payload, Response, ErrorMessage)
    VALUES (@LogId, @UrlId, @HttpMethod, @ApiPath, @StatusCode, @ResponseTimeMs, GETDATE(), @Payload, @Response, @ErrorMessage);
    
    -- Return the newly inserted LogId
    SELECT @LogId AS LogId;
END;
