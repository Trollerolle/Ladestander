USE El_Booking
GO

DROP TABLE Users

CREATE TABLE Users (
UserID INT IDENTITY(1,1) PRIMARY KEY,
FirstName NvarChar(50) NOT NULL,
LastName NvarChar(50)NOT NULL,
Email NvarChar(50) NOT NULL UNIQUE,
PhoneNumber NvarChar(50) NOT NULL UNIQUE,
--LicensPlate NvarChar(10) NOT NULL UNIQUE,
Password NvarChar(21) NOT NULL
)
GO

--EXECUTE sp_addlogin 'WPFApp', 'UCL123';
--GO
--EXECUTE sp_adduser 'WPFApp', 'WPFApp';
--GO

-- Forsøg på at lave usp til opsættelse af dynamisk oprettelse i databasen, med tilhørende EXECUTES.
-- Men WPFApp bruger har ikke adgang til at exec på sp_executesql.
--CREATE OR ALTER PROC usp_addLogin
--@Email NVARCHAR(50),
--@Password NVARCHAR(21)
--AS
--BEGIN
--BEGIN TRANSACTION
--DECLARE @LoginName NVARCHAR(150);
--SET @LoginName = N'CREATE LOGIN ' + QUOTENAME(@Email) + N' WITH PASSWORD = ' + QUOTENAME(@Password, '''') + N';';

--EXEC sp_executesql (@LoginName);
--IF @@ERROR <> 0
--		BEGIN
--			ROLLBACK TRANSACTION
--		END
--			ELSE
--		COMMIT TRANSACTION 
--End;
--GO
--EXECUTE usp_addLogin @LoginName, @Password;	
--GO

--CREATE OR ALTER PROC usp_addUser
--@LoginName NVARCHAR(50),
--@UserName NVARCHAR(50)
--AS
--BEGIN 
--DECLARE @UserAccount NVARCHAR(MAX);
--SET @UserAccount = N'CREATE USER ' + QUOTENAME(@UserName) + N' FOR LOGIN ' + QUOTENAME(@LoginName) + N';';
--EXEC sp_executesql @UserAccount;
--IF @@ERROR <> 0
--		BEGIN
--			ROLLBACK TRANSACTION
--		END
--			ELSE
--		COMMIT TRANSACTION 
--End;
--GO
		--EXECUTE usp_addUser @LoginName, @UserName;
--GO

--CREATE OR ALTER PROC usp_addRoleMember 
--@UserName NVARCHAR(50)
--AS
--BEGIN 
--DECLARE @rolename NvarChar(20) = 'BookingUserRole';
--DECLARE @AddRoleMember NVARCHAR(MAX);
--SET @AddRoleMember = N'ALTER ROLE ' + QUOTENAME(@rolename) + N' ADD MEMBER ' + QUOTENAME(@UserName) + N';';
--EXEC sp_executesql @AddRole;
--IF @@ERROR <> 0
--		BEGIN
--			ROLLBACK TRANSACTION
--		END
--			ELSE
--		COMMIT TRANSACTION 
--End;
--GO
		--EXECUTE usp_addRoleMember @UserName;
--GO



CREATE OR ALTER PROCEDURE usp_CreateUserAndLogin
@FirstName NVARCHAR(50),
@LastName NVARCHAR(50),
@Email NVARCHAR(50),
@PhoneNumber NVARCHAR(50),
-- @LicensePlate NVARCHAR(10),
@Password NVARCHAR(21)
AS 
 BEGIN 
	DECLARE @MailExist INT = 0;
	DECLARE @MailQueuedID INT;
	DECLARE @BodyMessage NvarChar(MAX);
	DECLARE @LoginName NVARCHAR(50) = @Email;
	DECLARE @UserName NVARCHAR(101) = @FirstName + ' ' + @LastName;
 -- Kunne ikke bruge TRY... og CATCH.. her fordi systemlagrede procedures ikke tillader at køres inde i en TRANSACTION.


	--BEGIN TRY

			-- Tekst til besked i oprettelses E-mailen.
		SET @BodyMessage = 'Kære ' + @UserName + '. Tak for din oprettelse i El Booking, Din bruger er nu oprettet og klar til at logge ind brugernavn ' + @Email + '.';

			EXEC msdb.dbo.sp_send_dbmail  
		@profile_name = 'ElBookingMail',  
		@recipients = @Email, --'4531441539@sms.inmobile.dk',  
		@body = @BodyMessage,  
		@subject = 'Bruger oprettelse i El Booking' ;
		
		SELECT TOP 1 @MailQueuedID = mailitem_id
		FROM msdb.dbo.sysmail_allitems
		WHERE recipients = @Email
		ORDER BY send_request_date DESC;

		WAITFOR DELAY '00:00:02';

		IF EXISTS (SELECT 1
					FROM msdb.dbo.sysmail_allitems
					WHERE mailitem_id = @MailQueuedID AND sent_status = 'sent')
			BEGIN 
				SET @MailExist = 1;
			END
			ELSE
            BEGIN
                PRINT 'Email Findes ikke';
				RETURN;
            END;

		IF @MailExist = 1
		BEGIN
			BEGIN TRANSACTION;


			EXECUTE El_Booking.dbo.uspAddUserToUsers
			@FirstName, 
			@LastName, 
			@Email,
			@PhoneNumber,
			-- @LicensePlate,
			@Password;

				IF @@ERROR <> 0
					BEGIN
					ROLLBACK TRANSACTION;
					PRINT 'Fejl ved uspAddUserToUsers';
					RETURN;
				END
		
			COMMIT TRANSACTION;
			PRINT 'Bruger oprettet og bekræftelses mail er afsendt';
		END

		 EXECUTE master.dbo.sp_addlogin
		@loginame = @LoginName, 
		@passwd = @Password, 
		@defdb = 'El_Booking';
		;
		
		IF @@ERROR <> 0
            BEGIN
                PRINT 'Fejl ved sp_addlogin'
				RETURN
            END;
		

		EXECUTE El_Booking.dbo.sp_adduser 
		@loginame = @LoginName, 
		@name_in_db = @UserName;
		;
		IF @@ERROR <> 0
            BEGIN
                PRINT 'Fejl ved sp_adduser'
				RETURN
            END;

		EXECUTE El_Booking.dbo.sp_addrolemember 
		@rolename = BookingUserRole, 
		@membername = @UserName;
		;
		IF @@ERROR <> 0
            BEGIN
                PRINT 'Fejl ved sp_addrolemember'
				RETURN
            END;



		
-- Kunne ikke bruge TRY... og CATCH.. her fordi systemlagrede procedures ikke tillader at køres inde i en TRANSACTION
	--END TRY
	--BEGIN CATCH
	--	IF @@TRANCOUNT > 0 
	--		ROLLBACK TRANSACTION;
	--	PRINT 'An error has occurred:';
	--	PRINT ERROR_MESSAGE();
	--END CATCH
END;
GO 

--GRANT EXECUTE ON OBJECT::[dbo].[usp_CreateUserAndLogin]
--    TO WPFApp;
--GO

-- CREATE ROLE BookingUserRole;
-- GRANT EXECUTE ON [dbo].[usp_GetUserBy] TO [BookingUserRole];
-- GRANT (Her skal der være SELECT, UPDATE osv. på de views og SP som rolle brugerne må bruge. Altså deres permissions.)
-- TO BookingUserRole;
-- GO 

CREATE OR ALTER PROCEDURE usp_Login
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

USE msdb;
GO

CREATE USER [WPFApp] FOR LOGIN [WPFApp];
GO
ALTER ROLE DatabaseMailUserRole ADD MEMBER [WPFApp];
GO


USE master;
GO
CREATE USER [WPFApp] FOR LOGIN [WPFApp];
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_Login]
    TO WPFApp;
GO
USE master
GRANT EXECUTE ON OBJECT::[dbo].[sp_addlogin]
    TO WPFApp;
GO

GRANT EXECUTE ON OBJECT::[dbo].[sp_adduser]
    TO WPFApp;
GO

USE master;
GO
GRANT EXECUTE ON OBJECT::[dbo].[sp_adduser] TO [WPFApp];
GRANT EXECUTE ON OBJECT::[dbo].[sp_addrolemember] TO [WPFApp];
GO

USE master;
GO
GRANT ALTER ANY LOGIN TO [WPFApp];
EXEC sp_addsrvrolemember 'WPFApp', 'sysadmin';
GO
USE El_Booking;
GRANT ALTER ANY USER TO [WPFApp];
GRANT ALTER ANY ROLE TO [WPFApp];
GO

--GRANT EXECUTE ON sys.sp_executesql TO [WPFApp];
--GO



--GRANT EXECUTE ON sys.sp_executesql TO [WPFApp];
--GO

-- EXECUTE Test til om WPFApp's permission er sat korrekt. HUSK AT ÆNDRE INPUTNE

EXEC msdb.dbo.sp_send_dbmail  
		@profile_name = 'ElBookingMail',  
		@recipients = 'Rene@Elbooking.dk', --'4531441539@sms.inmobile.dk',  
		@body = 'Voila..!! This email has been sent from SQL Server Express Edition.',  
		@subject = 'Voila..!! This email has been sent from SQL Server Express Edition.' ;


		EXECUTE uspAddUserToUsers
		'Troels', 
		'Trab', 
		'Troelstrab@hotmail.com',
		'12345678',
		-- @LicensePlate,
		'1234';

		EXECUTE sp_addlogin 'Troelstrab@hotmail.com', '1234';	
		-- EXECUTE sp_addlogin
		--@loginame = @LoginName, 
		--@passwd = @Password, 
		--@defdb = 'El_Booking';

		EXECUTE sp_adduser 'Troelstrab@hotmail.com', 'Troels trab';
		--EXECUTE sp_adduser 
		--@loginame = @LoginName, 
		--@name_in_db = @UserName;
		--;

		EXECUTE sp_addrolemember'BookingUserRole', 'Troels trab';
		--EXECUTE sp_addrolemember 
		--@rolename = BookingUserRole, 
		--@membername = @UserName;
		--;

		EXECUTE usp_CreateUserAndLogin 'Rene', 'Hansen', 'rmha63250@edu.ucl.dk', '98765432', '1234';