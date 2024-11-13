CREATE DATABASE [El_Booking]
GO

-- Create Tables
USE [El_Booking]
GO

CREATE TABLE Users (
UserID INT IDENTITY(1,1) PRIMARY KEY,
FirstName NvarChar(50) NOT NULL,
LastName NvarChar(50)NOT NULL,
Email NvarChar(50) NOT NULL UNIQUE,
PhoneNumber NvarChar(50) NOT NULL UNIQUE,
--LicensPlate NvarChar(10) NOT NULL UNIQUE,
Password NvarChar(21) NOT NULL
)
GO
------------------------------------------------