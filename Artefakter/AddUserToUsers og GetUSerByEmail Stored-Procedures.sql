USE El_Booking;
GO

-- GetUserBy
CREATE OR ALTER PROC usp_GetUserBy
(
	@Parameter NVarChar(50)
)
AS
BEGIN
	SELECT * FROM [dbo].[Users] WHERE [Email] = @Parameter OR [PhoneNumber] = @Parameter
END;
GO

DROP PROC [dbo].[uspGetUserByEmail];

GRANT EXECUTE ON OBJECT::[dbo].[usp_GetUserBy]
    TO WPFApp;
GO

EXEC usp_GetUserBy 'sander@gmail.com';
GO

-- AddUser
CREATE OR ALTER PROC uspAddUserToUsers
(
	@FirstName NvarChar(50),
	@LastName NvarChar(50),
	@Email NvarChar(50),
	@PhoneNumber NvarChar(50),
	@Password NvarChar(21)
)
AS
BEGIN
	BEGIN TRANSACTION
		INSERT INTO [dbo].[Users] (FirstName, LastName, Email, PhoneNumber, Password)
		VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Password)

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK TRANSACTION
		END
	ELSE
		COMMIT TRANSACTION
END;
GO

GRANT EXECUTE ON OBJECT::[dbo].[uspAddUserToUsers]
    TO WPFApp;
GO

EXEC [dbo].[uspAddUserToUsers] 'fornavn', 'efternavn', 'my@email.com', '00000000', 'pw123';
GO

DELETE FROM Users WHERE Password = 'pw123';