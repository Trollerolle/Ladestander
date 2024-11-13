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

GRANT EXECUTE ON OBJECT::[dbo].[usp_GetUserBy]
    TO WPFApp;
GO

EXEC usp_GetUserBy 'sander@gmail.com';
GO

-- AddUser
USE El_Booking;
GO
CREATE OR ALTER PROC usp_AddUserToUsers
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

	-- Tekst til besked i oprettelses E-mailen.
	DECLARE @BodyMessage NvarChar(MAX) = 
		'Kære ' + @Firstname + ' ' + @Lastname + '. Tak for din oprettelse i El Booking, Din bruger er nu oprettet og klar til at logge ind brugernavn ' + @Email + '.';

	-- send email
	EXEC msdb.dbo.sp_send_dbmail
		@profile_name = 'ElBookingMail',  
		@recipients = @Email, --'4531441539@sms.inmobile.dk',  
		@body = @BodyMessage,  
		@subject = 'Bruger oprettelse i El Booking'


END;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_AddUserToUsers]
    TO WPFApp;
GO

EXEC [dbo].[usp_AddUserToUsers] 'fornavn', 'efternavn', 'sean62359@edu.ucl.dk', '00000000', 'pw123';
GO

DELETE FROM Users WHERE Password = 'pw123';