--CREATE DATABASE El_Booking;
--GO

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
	CarID INT DEFAULT NULL,
	Password NvarChar(21) NOT NULL
)
GO

CREATE TABLE ChargingPoints 
(
	ChargingPointID INT IDENTITY(1,1) PRIMARY KEY,
	Name_ NVarChar(50) NOT NULL UNIQUE,
	InService BIT DEFAULT 1
)
GO

CREATE TABLE TimeSlots 
(
	TimeSlotID INT IDENTITY(1,1) PRIMARY KEY,
	TimeSlotStart TIME(0) NOT NULL,
	TimeSlotEnd Time(0) NOT NULL -- Smut med dine "interval i minutter" og time(7)...
)
GO

CREATE TABLE Bookings 
(
	BookingID INT IDENTITY(1,1) PRIMARY KEY,
	Date_ DATE NOT NULL,
	TimeSlotID INT NOT NULL FOREIGN KEY REFERENCES TimeSlots(TimeSlotID),
	ChargingPointID INT NOT NULL FOREIGN KEY REFERENCES ChargingPoints(ChargingPointID),
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID)
)
GO

CREATE TABLE Cars
(
	CarID INT IDENTITY(1,1) PRIMARY KEY,
	Brand NVarChar(50) NOT NULL,
	Model NVarChar(50) NOT NULL,
	LicensePlate NVarChar(10) NOT NULL UNIQUE
)

-- on cascade bookings_user
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO

-- on cascade users_car
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FK_Users_Cars] FOREIGN KEY([CarID])
REFERENCES [dbo].[Users] ([CarID])
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
------------------------------------------------