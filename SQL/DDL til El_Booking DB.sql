
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
	--LicensPlate NvarChar(10) NOT NULL UNIQUE,
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
	StartTime TIME NOT NULL,
	Interval INT NOT NULL -- interval i minutter
)
GO

CREATE TABLE Bookings 
(
	BookingID INT IDENTITY(1,1) PRIMARY KEY,
	Date_ DATE NOT NULL,
	TimeSlot INT NOT NULL FOREIGN KEY REFERENCES TimeSlots(TimeSlotID),
	ChargingPoint INT NOT NULL FOREIGN KEY REFERENCES ChargingPoints(ChargingPointID),
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID)
)
GO

-- on cascade bookings_user
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
------------------------------------------------