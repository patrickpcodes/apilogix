CREATE PROCEDURE InsertApiUrl
    @UserId UNIQUEIDENTIFIER,
    @BaseUrl NVARCHAR(500),
    @ApiName NVARCHAR(255)
AS
BEGIN
    DECLARE @UrlId UNIQUEIDENTIFIER;

    -- Check if the UserId and BaseUrl combination already exists
    SELECT @UrlId = UrlId
    FROM ApiUrls
    WHERE UserId = @UserId AND BaseUrl = @BaseUrl;

    -- If not found, insert a new record
    IF @UrlId IS NULL
    BEGIN
        SET @UrlId = NEWID();  -- Generate a new Guid for UrlId
        INSERT INTO ApiUrls (UrlId, UserId, BaseUrl, ApiName)
        VALUES (@UrlId, @UserId, @BaseUrl, @ApiName);
    END

    -- Return the UrlId (whether newly created or existing)
    SELECT @UrlId AS UrlId;
END;
