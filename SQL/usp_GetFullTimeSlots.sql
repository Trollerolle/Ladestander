CREATE OR ALTER PROC usp_GetTimeSlots1
(
	@Monday DATE
)
AS
	SELECT
		TimeSlotID,
		Date_
	FROM 
		[El_Booking].[dbo].[Bookings]
	WHERE
		Date_ BETWEEN @Monday AND DATEADD(DAY, 5, @Monday)
	GROUP BY
		Date_, TimeSlotID
	HAVING
		COUNT(TimeSlotID) = (SELECT COUNT(*) FROM ChargingPoints);
		
GO

EXEC usp_GetTimeSlots1 '2024-12-02'