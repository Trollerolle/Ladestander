USE El_Booking
GO

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

-- usp_GetUserBy
CREATE OR ALTER PROC usp_GetUserBy
(
	@Parameter NVarChar(50)
)
AS
BEGIN
DECLARE @Date_ DATE = GETDATE();
	SELECT 
		Users.[UserID],
		Users.[FirstName],
		Users.[LastName],
		Users.[Email],
		Users.[PhoneNumber],
		Cars.[CarID],
		B.[BookingID]
	FROM
		[dbo].[Users] 
	LEFT JOIN
		[dbo].[Cars]
	ON
		Users.UserID = Cars.UserID
	LEFT JOIN
		(SELECT CarID, BookingID From [dbo].[Bookings] WHERE Date_ >= @Date_) AS B
	ON 
		Cars.CarID = B.CarID
	WHERE
		[Email] = @Parameter
		OR
		[PhoneNumber] = @Parameter


END;
GO

-- usp_AddUser
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
		'K�re ' + @Firstname + ' ' + @Lastname + '. Tak for din oprettelse i El Booking, Din bruger er nu oprettet og klar til at logge ind brugernavn ' + @Email + '.';

	-- send email
	EXEC msdb.dbo.sp_send_dbmail
		@profile_name = 'ElBookingMail',  
		@recipients = @Email, --'4531441539@sms.inmobile.dk',  
		@body = @BodyMessage,  
		@subject = 'Bruger oprettelse i El Booking';
END;
GO

-- SP til at finde Fuldt reserverede timeslots for en uge.
CREATE OR ALTER PROC usp_GetFullTimeSlotsForWeek
(
	@Date DATE
)
AS
	SELECT 
		*
	FROM 
		[dbo].[ReservedBookings]
	WHERE
		[Date_] >= @Date
		AND
		[Date_] <= (DATEADD(DAY, 5, @Date));
GO

--DECLARE @monday DATE = '2024-11-18';
--EXEC usp_GetPlannedBookings @monday;

-- SP til at få en ledig ladestander for den valgte Timeslot.
CREATE OR ALTER PROC usp_GetAvailableChargingPoint
@Date DATE,
@TimeSlotID INT,
@ChargingPointID INT OUTPUT
AS 
BEGIN

SET @ChargingPointID = (

SELECT TOP 1
			[dbo].[ChargingPoints].[ChargingPointID]
		FROM
			[dbo].[ChargingPoints]
			LEFT JOIN 
				(SELECT [ChargingPointID] AS bcp FROM [dbo].[Bookings] WHERE [Date_] = @Date AND [TimeSlotID] = @TimeSlotID) AS Bookings_
			ON 
				[dbo].[ChargingPoints].[ChargingPointID] = Bookings_.bcp
		WHERE
			bcp IS NULL
		ORDER BY
			[ChargingPointID]%2 DESC, [ChargingPointID] -- Hvis ulige tal ønskes: DESC, [ChargingPointID]
	);

IF @ChargingPointID IS NULL
	PRINT 
	'Ingen ledige ladestandere.';
	Return -1;

END;
GO
--Til at execute usp_GetAvailableChargingPoint.
--DECLARE @Result INT;
--EXECUTE usp_GetAvailableChargingPoint '2024-11-19', 2, @Result OUT;
--PRINT @Result;

-- usp_AddBooking 
CREATE OR ALTER PROC usp_AddBooking
(
	@Date DATE,
	@TimeSlotID INT,
	@CarID INT
)
AS
DECLARE @ChargingPoint INT; 

EXECUTE usp_GetAvailableChargingPoint @Date, @TimeSlotID, @ChargingPoint out;
	
DECLARE @msg NVARCHAR(100) = 'No available Charging Points on Date: ' + FORMAT(@Date, 'yyyy-MM-dd', 'en-US') + ', Time Slot: ' + CAST(@TimeslotID AS NVarChar(5));
IF @ChargingPoint IS NULL
	THROW 50001, @msg, 1;
ELSE

BEGIN TRANSACTION
	INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [CarID]) VALUES
	(@Date, @TimeSlotID, @ChargingPoint, @CarID);

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION 
GO

EXEC sys.sp_addmessage
	@msgnum = 50001,
    @severity = 1,
    @msgtext = N'(%s)',
	@lang = 'us_english',
	@replace = 'REPLACE';
GO

-- usp_GetBooking
CREATE OR ALTER   PROC [dbo].[usp_GetBooking]
(
	@CarID NVarChar(50)
)
AS
--Kommentarer er til at teste for en specifik dag eventuelt i historisk data.
--DECLARE @DATEONLY DATE = '20241119';
SELECT TOP 1
    *
FROM dbo.ActiveBookings
	WHERE
		[CarID] = @CarID
	ORDER BY
		BookingID DESC;

GO

--EXECUTE usp_GetFullTimeSlotsForWeek '2024-10-14';
-- GO

CREATE OR ALTER PROC [dbo].[usp_UpdateUser]
(
	@UserID INT,
	@FirstName NVarChar(50),
	@LastName NVarChar(50),
	@Email NVarChar(50),
	@PhoneNumber NVarChar(50),
	@Password NVarChar(21) = null
)
AS
BEGIN TRANSACTION

	IF @Password is not null
		UPDATE [dbo].[Users]
		SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber, Password = @Password
		WHERE UserID = @UserID;
	ELSE
		UPDATE [dbo].[Users]
		SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber
		WHERE UserID = @UserID;

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO

CREATE OR ALTER PROC usp_GetTimeSlots 
AS
SELECT 
[TimeSlotID],
[TimeSlotStart],
[TimeSlotEnd]
  FROM [El_Booking].[dbo].[TimeSlots]
;
GO


CREATE OR ALTER PROC usp_AddCar 
(
	@LicensePlate NVarChar(10),
	@Brand NVarChar(50),
	@Model NVarChar(50),
	@UserID INT,
	@ScopeCarID int output
)
AS
BEGIN TRANSACTION
	INSERT INTO [dbo].[Cars]	([Brand], [Model], [LicensePlate], [UserID]) 
	VALUES						(@Brand, @Model, @LicensePlate, @UserID);
	set @ScopeCarID = (SELECT SCOPE_IDENTITY());
	
	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO

CREATE OR ALTER PROC usp_GetCarBy
(
	@UserID INT
)
AS
BEGIN
	SELECT 
		[CarID],
		[Brand],
		[Model],
		[LicensePlate]
	FROM
		[dbo].[Cars]
	WHERE
		[UserID] = @UserID
END;
GO

CREATE OR ALTER PROC [dbo].[usp_UpdateCar]
(
	@CarID INT,
	@Brand NVarChar(50),
	@Model NVarChar(50),
	@LicensePlate NVarChar(10)
)
AS
BEGIN TRANSACTION

UPDATE [dbo].[Cars]
		SET Brand = @Brand, Model = @Model, LicensePlate = @LicensePlate 
		WHERE CarID = @CarID;

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO

CREATE OR ALTER PROC [dbo].[usp_DeleteBooking]
(
	@BookingID INT
)
AS 
BEGIN TRANSACTION

DELETE FROM Bookings WHERE BookingID = @BookingID;

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO

-- SQL til at få en random kode, samt usp med insert og mailservice.
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