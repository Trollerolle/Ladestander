USE [El_Booking];
GO

INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Email], [PhoneNumber], [Password]) VALUES
	('bruger', '1', 'test1@email.com', 1, 'pwd1'),
	('bruger', '2', 'test2@email.com', 2, 'pwd2'),
	('bruger', '3', 'test3@email.com', 3, 'pwd3');
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

-- 
INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [UserID]) VALUES
	('2024-11-19', 1, 1, (SELECT [UserID] FROM [Users] WHERE [LastName] = '1')),
	('2024-11-20', 1, 2, (SELECT [UserID] FROM [Users] WHERE [LastName] = '2'));
GO

INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [UserID]) VALUES
	('2024-11-18', 2, 1, (SELECT [UserID] FROM [Users] WHERE [LastName] = '1')),
	('2024-11-19', 2, 1, (SELECT [UserID] FROM [Users] WHERE [LastName] = '2'));
GO

INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [UserID]) VALUES
	('2024-11-15', 2, 1, (SELECT [UserID] FROM [Users] WHERE [LastName] = '1')),
	('2024-11-15', 2, 1, (SELECT [UserID] FROM [Users] WHERE [LastName] = '2'));
GO