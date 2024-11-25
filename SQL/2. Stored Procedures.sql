USE El_Booking
GO

-- usp_Login
CREATE OR ALTER PROCEDURE usp_Login
	@Email NvarChar(50),
	@Password NvarChar(21)
AS
BEGIN  

	SELECT
		[UserID]
		[FirstName],
		[LastName],
		[Email],
		[PhoneNumber]
	FROM 
		[Users]
	WHERE 
		Email = @Email AND Password = @Password;

	IF @@ROWCOUNT <> 1
	BEGIN
		PRINT 'Ugyldig Email eller Password'
		RETURN 1
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
	SELECT 
		[UserID],
		[FirstName],
		[LastName],
		[Email],
		[PhoneNumber]
	FROM
		[dbo].[Users]
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

-- usp_AddBooking 
CREATE OR ALTER PROC usp_AddBooking 
(
	@Date DATE,
	@TimeSlotID INT,
	@ChargingPointID INT,
	@UserEmail NVarChar(50)
)
AS
BEGIN TRANSACTION
	INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [UserID]) VALUES
	(@Date, @TimeSlotID, @ChargingPointID, (SELECT [UserID] FROM [dbo].[Users] WHERE [Email] = @UserEmail));

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO

-- usp_GetBooking
CREATE OR ALTER   PROC [dbo].[usp_GetBooking]
(
	@Email NVarChar(50)
)
AS
--Kommentarer er til at teste for en specifik dag eventuelt i historisk data.
--DECLARE @DATEONLY DATE = '20241119';
SELECT 
    B.[BookingID],
    B.[Date_],
    T.[TimeSlotStart],
	T.[TimeSlotEnd],
    B.[ChargingPointID],
    B.[UserID]
FROM [El_Booking].[dbo].[Bookings] AS B
	INNER JOIN [El_Booking].[dbo].[TimeSlots] AS T
    ON B.[TimeSlotID] = T.[TimeSlotID]
	WHERE
		[UserID] = (SELECT [UserID] FROM [dbo].[Users] WHERE [Email] = @Email)
		AND
		[Date_] >= GETDATE();--@DATEONLY;
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

	IF @Password <> null
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

-- SP til at få en ledig ladestander for den valgte Timeslot.
CREATE OR ALTER PROC usp_GetAvailableChargingPoint
@Date DATE,
@TimeSlotID INT,
@ChargingPointID INT OUTPUT
AS 
BEGIN

SET @ChargingPointID = (

SELECT TOP 1 ChargingPoints.ChargingPointID
FROM ChargingPoints
LEFT JOIN Bookings
	ON ChargingPoints.ChargingPointID = Bookings.ChargingPointID
	AND Bookings.Date_ = @Date
	AND Bookings.TimeSlotID = @TimeSlotID
	WHERE Bookings.ChargingPointID IS NULL
	);

IF @ChargingPointID IS NULL
	PRINT 
	'Ingen ledige ladestandere.';
	Return -1;

END;

--Til at execute usp_GetAvailableChargingPoint.
--DECLARE @Result INT;
--EXECUTE usp_GetAvailableChargingPoint '2024-11-19', 2, @Result OUT;
--PRINT @Result;