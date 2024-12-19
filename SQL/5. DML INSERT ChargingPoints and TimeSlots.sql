USE [El_Booking];
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


-- Insert users 
INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Email], [PhoneNumber], [Password]) VALUES
	('bruger', '1', 'test1@email.com', 12345678, 'S@nder1'),
	('bruger', '2', 'test2@email.com', 23456789, 'S@nder2'),
	('bruger', '3', 'test3@email.com', 34567891, 'S@nder3'),
	('John', 'Doe', 'john.doe@email.com', 12345679, 'S@nder1'),
    ('Jane', 'Smith', 'jane.smith@email.com', 23456783, 'S@nder2'),
    ('Emily', 'Davis', 'emily.davis@email.com', 34567893, 'S@nder3'),
    ('Alice', 'Johnson', 'alice.johnson@email.com', 45678912, 'P@ssword1'),
    ('Bob', 'Brown', 'bob.brown@email.com', 56789123, 'P@ssword2'),
    ('Charlie', 'Taylor', 'charlie.taylor@email.com', 67891234, 'P@ssword3'),
    ('Diana', 'Miller', 'diana.miller@email.com', 78912345, 'P@ssword4'),
    ('Ethan', 'Wilson', 'ethan.wilson@email.com', 89123456, 'P@ssword5'),
    ('Fiona', 'Moore', 'fiona.moore@email.com', 91234567, 'P@ssword6'),
    ('George', 'White', 'george.white@email.com', 98765432, 'P@ssword7'),
    ('Hannah', 'Hall', 'hannah.hall@email.com', 87654321, 'P@ssword8'),
    ('Isaac', 'Lewis', 'isaac.lewis@email.com', 76543210, 'P@ssword9');
GO

-- Insert cars 
INSERT INTO [dbo].[Cars] ([LicensePlate], [Brand], [Model], [UserID]) VALUES
    ('AB12346', 'Tesla', 'Model 3', (SELECT [UserID] FROM [Users] WHERE [LastName] = '1')),
	('CD12346', 'VW', 'E-Golf', (SELECT [UserID] FROM [Users] WHERE [LastName] = '2')),
	('EF12346', 'Skoda', 'Enyaq', (SELECT [UserID] FROM [Users] WHERE [LastName] = '3')),
	('AB12345', 'Tesla', 'Model 3', (SELECT [UserID] FROM [Users] WHERE [Email] = 'john.doe@email.com')),
    ('CD12345', 'VW', 'E-Golf', (SELECT [UserID] FROM [Users] WHERE [Email] = 'jane.smith@email.com')),
    ('EF12345', 'Skoda', 'Enyaq', (SELECT [UserID] FROM [Users] WHERE [Email] = 'emily.davis@email.com')),
    ('GH12345', 'Hyundai', 'Kona Electric', (SELECT [UserID] FROM [Users] WHERE [Email] = 'alice.johnson@email.com')),
    ('IJ12345', 'BMW', 'i3', (SELECT [UserID] FROM [Users] WHERE [Email] = 'bob.brown@email.com')),
    ('KL12345', 'Nissan', 'Leaf', (SELECT [UserID] FROM [Users] WHERE [Email] = 'charlie.taylor@email.com')),
    ('MN12345', 'Audi', 'e-tron', (SELECT [UserID] FROM [Users] WHERE [Email] = 'diana.miller@email.com')),
    ('OP12345', 'Mercedes', 'EQC', (SELECT [UserID] FROM [Users] WHERE [Email] = 'ethan.wilson@email.com')),
    ('QR12345', 'Kia', 'EV6', (SELECT [UserID] FROM [Users] WHERE [Email] = 'fiona.moore@email.com')),
    ('ST12345', 'Ford', 'Mustang Mach-E', (SELECT [UserID] FROM [Users] WHERE [Email] = 'george.white@email.com')),
    ('UV12345', 'Chevrolet', 'Bolt EV', (SELECT [UserID] FROM [Users] WHERE [Email] = 'hannah.hall@email.com')),
    ('WX12345', 'Volvo', 'XC40 Recharge', (SELECT [UserID] FROM [Users] WHERE [Email] = 'isaac.lewis@email.com'));
GO

-- Bookings for 20/01/2025 (Time Slot 4)
INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [CarID]) VALUES
    ('2025-01-20', 3, 1, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'AB12345')),
    ('2025-01-20', 3, 2, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'CD12345')),
    ('2025-01-20', 3, 3, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'EF12345')),
    ('2025-01-20', 3, 4, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'GH12345'));
GO

-- Bookings for 08/01/2025 (Time Slot 4)
INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [CarID]) VALUES
    ('2025-01-08', 2, 1, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'IJ12345')),
    ('2025-01-08', 2, 2, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'KL12345')),
    ('2025-01-08', 2, 3, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'MN12345')),
    ('2025-01-08', 2, 4, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'OP12345'));
GO

-- Bookings for 16/01/2025 (Time Slot 4)
INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [CarID]) VALUES
    ('2025-01-16', 4, 1, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'QR12345')),
    ('2025-01-16', 4, 2, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'ST12345')),
    ('2025-01-16', 4, 3, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'UV12345')),
    ('2025-01-16', 4, 4, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'WX12345'));
GO

-- Bookings for 17/01/2025 (Time Slot 4, only 3 bookings)
INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlotID], [ChargingPointID], [CarID]) VALUES
    ('2025-01-17', 4, 1, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'EF12346')),
    ('2025-01-17', 4, 2, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'CD12346')),
    ('2025-01-17', 4, 3, (SELECT [CarID] FROM [Cars] WHERE [LicensePlate] = 'AB12346'));
GO

