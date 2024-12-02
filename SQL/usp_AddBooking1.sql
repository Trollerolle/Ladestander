CREATE OR ALTER PROC usp_AddBooking1
(
	@Date DATE,
	@TimeSlotID INT,
	@CarID NVarChar(50)
)
AS
DECLARE @ChargingPoint INT = 
	(
		SELECT TOP 1
			[dbo].[ChargingPoints].[ChargingPointID]
		FROM
			[dbo].[ChargingPoints]
			LEFT JOIN 
				(SELECT [ChargingPointID] AS bcp FROM [dbo].[Bookings] WHERE [Date_] = @Date AND [TimeSlotID] = @TimeSlotID) AS Bookings_
			ON 
				[dbo].[ChargingPoints].[ChargingPointID] = Bookings_.bcp
		WHERE
			InService <> 0
			AND 
			bcp IS NULL
	)

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

EXEC usp_AddBooking1 '2024-12-04', 2, 1

EXEC sys.sp_addmessage
	@msgnum = 50001,
    @severity = 1,
    @msgtext = N'(%s)',
	@lang = 'us_english',
	@replace = 'REPLACE';
GO