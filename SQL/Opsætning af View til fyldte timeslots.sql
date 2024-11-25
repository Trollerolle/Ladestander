CREATE OR ALTER VIEW ReservedBookings AS 
SELECT TimeSlot, Date_  
FROM Bookings
GROUP BY
	TimeSlot,
	Date_
HAVING 
	COUNT(*) = (SELECT COUNT(*) FROM ChargingPoints);
GO

	CREATE   PROC [dbo].[usp_GetReservedTimeslotsForWeek]
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
