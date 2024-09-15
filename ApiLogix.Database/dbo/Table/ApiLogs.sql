CREATE TABLE [dbo].[ApiLogs]
(
	LogId UNIQUEIDENTIFIER PRIMARY KEY,  -- Using Guid for LogId
    UrlId UNIQUEIDENTIFIER,  -- References ApiUrls.UrlId
    HttpMethod NVARCHAR(10),
    ApiPath NVARCHAR(500),  -- The path after the base URL (e.g., /v1/resource)
    StatusCode INT,
    ResponseTimeMs INT,
    Timestamp DATETIME DEFAULT GETDATE(),
    Payload NVARCHAR(MAX),
    Response NVARCHAR(MAX),
    ErrorMessage NVARCHAR(MAX),
    CONSTRAINT FK_ApiUrls_UrlId FOREIGN KEY (UrlId) REFERENCES ApiUrls(UrlId)
)
