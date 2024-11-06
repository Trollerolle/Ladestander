CREATE TABLE Users (
UserID INT IDENTITY(1,1) PRIMARY KEY,
FirstName NvarChar(50),
LastName NvarChar(50),
Email NvarChar(50) UNIQUE,
PhoneNumber NvarChar(50) UNIQUE,
Password NvarChar(21)
)

CREATE PROCEDURE usp_CreateUserAndLogin
 @LoginName NvarChar(50),
 @Password NvarChar(50),
 @UserName NvarChar(50)
 AS 
 BEGIN 
	BEGIN TRY

	EXECUTE sp_addlogin @loginame = @LoginName, @passwd = @Password, @defdb = 'El_Booking';

	EXECUTE sp_adduser @loginame = @LoginName, @name_in_db = @UserName;

	EXECUTE sp_addrolemember @rolename = BookingUserRole, @membername = @UserName;

	END TRY
	BEGIN CATCH
		PRINT 'An error has occurred:';
		PRINT ERROR_MESSAGE();
	END CATCH
END;

CREATE ROLE BookingUserRole;