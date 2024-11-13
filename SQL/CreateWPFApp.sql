
-- opret login
    -- master
USE [master]
GO

EXECUTE sp_addlogin 'WPFApp', 'UCL123';
GO
EXECUTE sp_adduser 'WPFApp', 'WPFApp';
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

