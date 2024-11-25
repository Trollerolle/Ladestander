CREATE OR ALTER PROC usp_GetAvailebleChargingPoint
@Date DATE,
@TimeSlot INT,
@ChargingPoint INT OUTPUT
AS 
BEGIN

SET @ChargingPoint = (

SELECT TOP 1 ChargingPointID
FROM ChargingPoints
LEFT JOIN Bookings
	ON ChargingPoints.ChargingPointID = Bookings.ChargingPoint
	AND Bookings.Date_ = @Date
	AND Bookings.TimeSlot = @TimeSlot
	WHERE Bookings.ChargingPoint IS NULL
	);

IF @ChargingPoint IS NULL
	PRINT 
	'Ingen ledige ladestandere.';
	Return -1;

END;


DECLARE @Result INT;
EXECUTE usp_GetAvailebleChargingPoint '2024-11-19', 2, @Result OUT;
PRINT @Result;