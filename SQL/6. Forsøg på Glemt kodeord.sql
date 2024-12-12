CREATE TABLE ForgotPassword 
(
	FpID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
	Date_ datetime,
	TempPW NvarChar(8)
);
GO

--65 stor A til 90 for stort Z

--97 lille a til 122 for lille z

--48 for 0 og 57 for 9

--SET @charI = FLOOR(RAND()*(90-65+1)+65)
--SET @charI = FLOOR(RAND()*(122-48+1)+48)


-- SELECT @Random [PassWord]
CREATE OR ALTER PROC usp_Randomizer
(
@Random NvarChar(8) OUTPUT
)
AS 
SET @Random = ''
DECLARE @char CHAR = ''
DECLARE @charI INT = 0
DECLARE @len INT = 8-- Length of Password
	WHILE @len > 0
BEGIN
SET @charI = ROUND(RAND()*100,0)
SET @char = CHAR(@charI)
	IF @charI >= 48 AND @charI <= 57 OR @charI >= 65 AND @charI <= 90 OR @charI >= 97 AND @charI <= 122
	BEGIN
		SET @Random += @char
		SET @len = @len - 1
	END
END;
GO

GRANT EXEC ON usp_ForgotPW TO WPFApp;

CREATE OR ALTER PROC usp_ForgotPW
(
@Email NvarChar(50)
)
AS
BEGIN
DECLARE @TempPW Nvarchar(8);
DECLARE @Date_ datetime = GETDATE();
DECLARE @UserID INT;


SET @UserID = (SELECT UserID FROM Users WHERE Email = @Email);

IF @UserID IS NULL
	Begin
		RETURN 2;
	END;

IF EXISTS (SELECT 1 FROM ForgotPassword WHERE UserID = @UserID AND Date_ >= DATEADD(MINUTE, -15, @Date_)) 
	Begin 
		RETURN 2;
	END;

EXECUTE usp_Randomizer @TempPW OUT;

 INSERT INTO dbo.ForgotPassword (UserID, Date_, TempPW)
 VALUES 
 (
 @UserID,
 @Date_,
 @TempPW
 );

 If @@ROWCOUNT <> 1
	BEGIN
		RETURN 1;
	END;

 	-- Tekst til besked i glemt password E-mailen.
	DECLARE @BodyMessage NvarChar(MAX) = 
		'Hej ' + @Email + ', Vi har modtaget d. ' + convert(NVARCHAR, @Date_, 120) + ', en anmodning om at nulstille dit password til El Booking.

		Din midlertidige kode som virker i 15 minutter:

		' + @TempPW + '

		Hvis du ikke har bedt om denne ændring, kontakt vores Kundeservice eller gå ind på help.Elbooking.dk så hurtigt som muligt, for at sikre din profil.

		Tak, El Booking.';

	-- send email
	EXEC msdb.dbo.sp_send_dbmail
		@profile_name = 'ElBookingMail',  
		@recipients = @Email, --'4531441539@sms.inmobile.dk',  
		@body = @BodyMessage,  
		@subject = 'Glemt adgangskode til El Booking';

 END;
GO

-- Til at teste
-- EXECUTE usp_ForgotPW "tjtr63311@edu.ucl.dk";
-- GO


-- tjekker om deres password først er på ForgotPassword table og hvis den finder en som er maks 15 minuter gammel, så setter den UserID.
-- Error handling sker stadig først i usp_Login.
CREATE OR ALTER PROC usp_LoginForgot 
(
@Email NvarChar(50),
@Password NvarChar(21),
@UserID INT OUT
)
AS
BEGIN 
DECLARE @Date_ datetime = dateadd(MINUTE, -15, GETDATE());
SET @UserID = 
	(
	SELECT TOP 1
	F.UserID
	FROM [dbo].[ForgotPassword] as F
	LEFT JOIN 
	dbo.Users as U
	ON
	F.UserID = U.UserID
	WHERE 
		U.Email = @Email AND F.TempPW = @Password AND F.Date_ >= @Date_
	);

	IF @@ROWCOUNT <> 1
	BEGIN
		SET @UserID = NULL;
	END;
END;
GO

-- usp_Login opdateret
CREATE OR ALTER PROCEDURE usp_Login
	@Email NvarChar(50),
	@Password NvarChar(21)
AS
BEGIN  
DECLARE @UserID INT;
		
	SELECT
		[UserID],
		[FirstName],
		[LastName],
		[Email],
		[PhoneNumber]
	FROM 
		[Users]
	WHERE 
		Email = @Email AND Password = @Password;

	IF @@ROWCOUNT = 1
	BEGIN
		RETURN 0; -- Succes
	END;

	EXECUTE usp_LoginForgot @Email, @Password, @UserID OUTPUT;

	IF @UserID IS NULL
	BEGIN
		RETURN 1; -- Fejl
	END;

	IF @UserID IS NOT NULL
	BEGIN 
		RETURN 0; -- Succes
	END;
END;
GO

DECLARE @RESULT INT;
EXECUTE @RESULT = usp_Login 'tjtr63311@edu.ucl.dk', 'GDTcQXS1'; 
PRINT @RESULT;