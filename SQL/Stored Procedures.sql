USE El_Booking
GO

-- usp_Login
CREATE OR ALTER PROCEDURE usp_Login
	@Email NvarChar(50),
	@Password NvarChar(21)
AS
BEGIN  

	SELECT
		[FirstName],
		[LastName],
		[Email],
		[PhoneNumber],
		[Password]
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

-- usp_GetPlannedBookingsForWeek
CREATE OR ALTER PROC usp_GetPlannedBookingsForWeek
(
	@Date DATE
)
AS
	SELECT 
		[Date_], 
		[TimeSlot],
		[ChargingPoint] 
	FROM 
		[dbo].[Bookings]
	WHERE
		[Date_] >= @Date
		AND
		[Date_] <= (DATEADD(DAY, 5, @Date));
GO

CREATE OR ALTER PROC usp_GetPlannedBookingsForWeek2
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
	@TimeSlot INT,
	@ChargingPoint INT,
	@UserEmail NVarChar(50)
)
AS
BEGIN TRANSACTION
	INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlot], [ChargingPoint], [UserID]) VALUES
	(@Date, @TimeSlot, @ChargingPoint, (SELECT [UserID] FROM [dbo].[Users] WHERE [Email] = @UserEmail));

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO

-- usp_GetBooking
CREATE OR ALTER PROC [dbo].[usp_GetBooking]
(
	@Email NVarChar(50)
)
AS
	SELECT 
		[Date_], 
		[TimeSlot],
		[ChargingPoint] 
	FROM 
		[dbo].[Bookings]
	WHERE
		[UserID] = (SELECT [UserID] FROM [dbo].[Users] WHERE [Email] = @Email)
		AND
		[Date_] >= GETDATE();
GO

CREATE OR ALTER PROC [dbo].[usp_GetFullTimeSlots]
(
	@Date DATE
)
AS
   SELECT
        TimeSlot,
        (DATEPART(WEEKDAY, @DATE) + @@DATEFIRST - 2) % 7 AS DayIndex
    FROM 
        [dbo].[ReservedBookings]
    WHERE 
        [Date_] >= @DATE
        AND
        [Date_] <= (DATEADD(DAY, 5, @DATE));
GO

EXECUTE usp_GetFullTimeSlots '2024-11-18';

EXECUTE usp_GetPlannedBookingsForWeek2 '2024-10-14';
GO

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
