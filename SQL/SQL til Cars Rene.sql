CREATE OR ALTER PROC usp_GetUserBy
(
	@Parameter NVarChar(50)
)
AS
BEGIN
	SELECT 
		[UserID],
		[FirstName],
		[LastName],
		[Email],
		[PhoneNumber]
		[CarID]
	FROM
		[dbo].[Users]
	WHERE
		[Email] = @Parameter
		OR
		[PhoneNumber] = @Parameter
END;
GO



CREATE OR ALTER PROC usp_AddCar 
(
	@Brand NVarChar(50),
	@Model NVarChar(50),
	@LicensePlate NVarChar(10)
)
AS
BEGIN TRANSACTION
	INSERT INTO [dbo].[Cars]	([Brand], [Model], [LicensePlate]) 
	VALUES						(@Brand, @Model, @LicensePlate);

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO




CREATE OR ALTER PROC [dbo].[usp_UpdateCar]
(
	@CarID INT,
	@Brand NVarChar(50),
	@Model NVarChar(50),
	@LicensePlate NVarChar(10)
)
AS
BEGIN TRANSACTION

UPDATE [dbo].[Cars]
		SET @Brand = @Brand, @Model = @Model, @LicensePlate = @LicensePlate, 
		WHERE CarID = @CarID;

	IF @@ERROR <> 0
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
GO