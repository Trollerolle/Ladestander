
-- opret login
USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [WPFApp]    Script Date: 11.11.2024 17.33.25 ******/
CREATE LOGIN [WPFApp1] WITH PASSWORD='UCL123', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [WPFApp1] DISABLE
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [WPFApp1]
GO

-- opret user i master
USE [master]
GO

/****** Object:  User [WPFApp1]    Script Date: 11.11.2024 17.47.23 ******/
CREATE USER [WPFApp1] FOR LOGIN [WPFApp1] WITH DEFAULT_SCHEMA=[WPFApp1]
GO

-- grant permissions
GRANT EXECUTE ON OBJECT::[dbo].[usp_GetUserBy]
    TO WPFApp1;
GO

GRANT EXECUTE ON OBJECT::[dbo].[uspAddUserToUsers]
    TO WPFApp1;
GO


