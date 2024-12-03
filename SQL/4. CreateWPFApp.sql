
-- opret login til LoginWindow
    -- master
USE [master]
GO

EXECUTE sp_addlogin 'WPFApp', 'UCL123';
GO
    -- El_Booking
USE [El_Booking]
GO
EXECUTE sp_adduser 'WPFApp', 'WPFApp';
GO
    -- msdb
USE [msdb];
GO

CREATE USER [WPFApp] FOR LOGIN [WPFApp];
GO
ALTER ROLE DatabaseMailUserRole ADD MEMBER [WPFApp];
GO

USE [El_Booking]
-- grant permissions
    -- usp_GetUserBy
GRANT EXECUTE ON OBJECT::[dbo].[usp_GetUserBy]
    TO WPFApp;
GO
    -- usp_AddUserToUsers
GRANT EXECUTE ON OBJECT::[dbo].[usp_AddUserToUsers]
    TO WPFApp;
GO

    -- usp_Login
GRANT EXECUTE ON OBJECT::[dbo].[usp_Login]
    TO WPFApp;
GO



-- opret et login til alle brugere til connectionstrig.
    -- master
USE [master]
GO

EXECUTE sp_addlogin 'WPFAccount', 'UCL456';
GO

    -- El_Booking
USE [El_Booking]
GO
EXECUTE sp_adduser 'WPFAccount', 'WPFAccount';
GO
    -- msdb
USE [msdb];
GO

CREATE USER WPFAccount FOR LOGIN [WPFAccount];
GO
ALTER ROLE DatabaseMailUserRole ADD MEMBER [WPFAccount];
GO

-- grant permissions
    -- usp_GetUserBy
USE [El_Booking]

    -- usp_AddBooking 
GRANT EXECUTE ON OBJECT::[dbo].[usp_AddBooking]
    TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_GetCarBy]
    TO WPFAccount;
GO

    -- usp_GetBooking 
GRANT EXECUTE ON OBJECT::[dbo].[usp_GetBooking]
    TO WPFAccount;
GO

GRANT SELECT ON OBJECT::[dbo].[ReservedBookings]
    TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_UpdateUser]
	TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_GetTimeSlots]
	TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_GetFullTimeSlotsForWeek]
	TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_GetAvailableChargingPoint]
	TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_AddCar]
	TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_UpdateCar]
	TO WPFAccount;
GO

GRANT EXECUTE ON OBJECT::[dbo].[usp_DeleteBooking]
	TO WPFAccount;
GO