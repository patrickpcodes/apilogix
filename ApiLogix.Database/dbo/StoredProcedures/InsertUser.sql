CREATE PROCEDURE InsertUser
    @Name NVARCHAR(100),
    @Email NVARCHAR(255),
    @ApiKey UNIQUEIDENTIFIER
AS
BEGIN
    -- Insert the new user into the Users table
    INSERT INTO Users (UserId, Name, Email, ApiKey, CreatedAt)
    VALUES (NEWID(), @Name, @Email, @ApiKey, GETDATE());

    -- Return the UserId of the newly inserted user
    SELECT SCOPE_IDENTITY() AS UserId;
END;
