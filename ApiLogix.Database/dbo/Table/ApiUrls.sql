CREATE TABLE [dbo].[ApiUrls]
(
    UrlId UNIQUEIDENTIFIER PRIMARY KEY,  -- Unique Id for the User-URL pair
    UserId UNIQUEIDENTIFIER,  -- References Users.UserId
    BaseUrl NVARCHAR(500),  -- The base URL (e.g., https://api.example.com)
    ApiName NVARCHAR(255),  -- User-defined name for the API
    CONSTRAINT FK_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(UserId)
)
