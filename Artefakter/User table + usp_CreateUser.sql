USE El_Booking
GO

DROP TABLE Users

CREATE TABLE Users (
UserID INT IDENTITY(1,1) PRIMARY KEY,
FirstName NvarChar(50) NOT NULL,
LastName NvarChar(50)NOT NULL,
Email NvarChar(50) NOT NULL UNIQUE,
PhoneNumber NvarChar(50) NOT NULL UNIQUE,
LicensPlate NvarChar(10) NOT NULL UNIQUE,
Password NvarChar(21) NOT NULL
)
GO

--EXECUTE sp_addlogin 'WPFApp', 'UCL123';
--GO
--EXECUTE sp_adduser 'WPFApp', 'WPFApp';
--GO

CREATE OR ALTER PROCEDURE usp_CreateUserAndLogin
@FirstName NVARCHAR(50),
@LastName NVARCHAR(50),
@Email NVARCHAR(50),
@PhoneNumber NVARCHAR(50),
@LicensePlate NVARCHAR(10),
@Password NVARCHAR(21)
AS 
 BEGIN 

 DECLARE @LoginName NVARCHAR(50) = @Email;
 DECLARE @UserName NVARCHAR(101) = @FirstName + ' ' + @LastName;

	BEGIN TRY
		
		BEGIN TRANSACTION

		EXECUTE sp_addlogin	
		@loginame = @LoginName, 
		@passwd = @Password, 
		@defdb = 'El_Booking';

		EXECUTE sp_adduser 
		@loginame = @LoginName, 
		@name_in_db = @UserName;

		EXECUTE sp_addrolemember 
		@rolename = BookingUserRole, 
		@membername = @UserName;

		EXECUTE uspAddUserToUsers 
		@FirstName, 
		@LastName, 
		@Email,
		@PhoneNumber,
		@LicensePlate,
		@Password;

		COMMIT TRANSACTION 

		EXEC msdb.dbo.sp_send_dbmail  
		@profile_name = 'ElBookingMail',  
		@recipients = '4531441539@sms.inmobile.dk',  
		@body = 'Voila..!! This email has been sent from SQL Server Express Edition.',  
		@subject = 'Voila..!! This email has been sent from SQL Server Express Edition.' ;

	END TRY
	BEGIN CATCH
		PRINT 'An error has occurred:';
		PRINT ERROR_MESSAGE();
	END CATCH
END;
GO 

CREATE ROLE BookingUserRole;
-- GRANT (Her skal der være SELECT, UPDATE osv. på de views og SP som rolle brugerne må bruge. Altså deres permissions.)
-- TO BookingUserRole;
GO 

CREATE OR ALTER PROCEDURE uspLogin
@Email NvarChar(50),
@Password NvarChar(21)
AS
BEGIN  

SELECT * FROM Users 
WHERE Email = @Email and Password = @Password

IF @@ROWCOUNT = 0
			PRINT 'Ugyldig Email eller Password'

END;
GO
