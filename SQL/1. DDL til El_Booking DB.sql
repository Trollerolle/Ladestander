CREATE DATABASE El_Booking;
GO

-- Create Tables
USE [El_Booking]
GO

	-- DROP Tables
--DROP TABLE Users;
--GO
--DROP TABLE Bookings;
--GO
--DROP TABLE TimeSlots;
--GO
--DROP TABLE ChargingPoints;
--GO

CREATE TABLE Users 
(
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	FirstName NvarChar(50) NOT NULL,
	LastName NvarChar(50)NOT NULL,
	Email NvarChar(50) NOT NULL UNIQUE,
	PhoneNumber NvarChar(50) NOT NULL UNIQUE,
	Password NvarChar(21) NOT NULL
)
GO


--ALTERS...
--ALTER TABLE Cars
--ADD UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID)

--ALTER COLUMN CarID INT DEFAULT NULL UNIQUE FOREIGN KEY REFERENCES Cars(CarID)

--ALTER TABLE Users DROP COLUMN CarID;

--ALTER TABLE Users
--ALTER COLUMN CarID INT NULL;

--ALTER TABLE Users
--ADD CONSTRAINT DF_Users_CarID DEFAULT NULL FOR CarID;

--ALTER TABLE Users
--ADD CONSTRAINT UQ_Users_CarID UNIQUE (CarID);

--ALTER TABLE Users
--ADD CONSTRAINT FK_Users_CarID FOREIGN KEY (CarID)
--REFERENCES Cars(CarID);





CREATE TABLE ChargingPoints 
(
	ChargingPointID INT IDENTITY(1,1) PRIMARY KEY,
	Name_ NVarChar(50) NOT NULL UNIQUE
)
GO

CREATE TABLE TimeSlots 
(
	TimeSlotID INT IDENTITY(1,1) PRIMARY KEY,
	TimeSlotStart TIME(0) NOT NULL,
	TimeSlotEnd Time(0) NOT NULL 
)
GO

CREATE TABLE Cars
(
	CarID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
	Brand NVarChar(50) NOT NULL,
	Model NVarChar(50) NOT NULL,
	LicensePlate NVarChar(10) NOT NULL UNIQUE
)
GO

CREATE TABLE Bookings 
(
	BookingID INT IDENTITY(1,1) PRIMARY KEY,
	Date_ DATE NOT NULL,
	TimeSlotID INT NOT NULL FOREIGN KEY REFERENCES TimeSlots(TimeSlotID),
	ChargingPointID INT NOT NULL FOREIGN KEY REFERENCES ChargingPoints(ChargingPointID),
	CarID INT NOT NULL FOREIGN KEY REFERENCES Cars(CarID)
)
GO

CREATE TABLE ForgotPassword 
(
	FpID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
	Date_ datetime,
	TempPW NvarChar(8)
);
GO



-- on cascade bookings_cars
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Cars] FOREIGN KEY([CarID])
REFERENCES [dbo].[Cars] ([CarID])
ON DELETE CASCADE
GO

-- on cascade cars_users
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FK_Cars_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO

-- on cascade forgotpasswords_users
ALTER TABLE [dbo].[ForgotPassword]  WITH CHECK ADD  CONSTRAINT [FK_forgotpasswords_users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO

CREATE OR ALTER VIEW ReservedBookings AS 
SELECT TimeSlotID, Date_  
FROM Bookings
GROUP BY
	TimeSlotID,
	Date_
HAVING 
	COUNT(*) = (SELECT COUNT(*) FROM ChargingPoints);
GO

CREATE OR ALTER VIEW ActiveBookings AS
SELECT 
    [BookingID],
    [Date_],
    Bookings.[TimeSlotID],
    [ChargingPointID],
    [CarID],
	TimeSlots.TimeSlotEnd
FROM [El_Booking].[dbo].[Bookings]
	Left join dbo.TimeSlots
	on dbo.Bookings.TimeSlotID = TimeSlots.TimeSlotID
	WHERE
		[Date_] >= convert(Date,GETDATE())--@DATEONLY;
		AND
		(
			[Date_] >= convert(Date,GETDATE())
			OR 
			(
			[Date_] >= convert(Date,GETDATE()) 
			AND
			TimeSlots.TimeSlotEnd >= convert(Time(0), dateadd(MINUTE, -15, GETDATE()))
			)
		);

GO
		
------------------------------------------------