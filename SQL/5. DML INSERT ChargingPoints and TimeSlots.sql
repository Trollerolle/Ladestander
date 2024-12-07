USE [El_Booking];
GO

INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Email], [PhoneNumber], [Password]) VALUES
	('bruger', '1', 'test1@email.com', 12345678, 'S@nder1'),
	('bruger', '2', 'test2@email.com', 23456789, 'S@nder2'),
	('bruger', '3', 'test3@email.com', 34567891, 'S@nder3');
GO

INSERT INTO [dbo].[ChargingPoints] ([Name_]) VALUES
	('1'), ('2'), ('3'), ('4');
GO

INSERT INTO [dbo].[TimeSlots] ([TimeSlotStart], [TimeSlotEnd]) VALUES
	('06:00:00', '09:00:00'),
	('09:00:00', '12:00:00'),
	('12:00:00', '15:00:00'),
	('15:00:00', '18:00:00');
GO

INSERT INTO [dbo].[Cars] ([LicensePlate], [Brand], [Model], [UserID]) VALUES
	('AB12345', 'Tesla', 'Model 3', (SELECT [UserID] FROM [Users] WHERE [LastName] = '1')),
	('CD12345', 'VW', 'E-Golf', (SELECT [UserID] FROM [Users] WHERE [LastName] = '2')),
	('EF12345', 'Skoda', 'Enyaq', (SELECT [UserID] FROM [Users] WHERE [LastName] = '3'));
GO
-- 
INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [CarID]) VALUES
	('2024-12-04', 1, 1, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'AB12345')),
	('2024-12-04', 1, 2, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'CD12345')),
	('2024-12-04', 1, 3, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'EF12345'));
GO

