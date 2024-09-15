CREATE TABLE [dbo].[Users]
(
    UserId UNIQUEIDENTIFIER PRIMARY KEY,  -- Using Guid
    Name NVARCHAR(100),
    Email NVARCHAR(255),
    ApiKey NVARCHAR(255),  -- API key for user identification
    CreatedAt DATETIME DEFAULT GETDATE()
)
