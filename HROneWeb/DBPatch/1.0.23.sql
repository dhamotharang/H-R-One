
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.22b'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE RequestEmpPersonalInfo ADD RequestEmpNationalityID int NULL;
	ALTER TABLE RequestEmpPersonalInfo ADD RequestEmpPassportIssuedCountryID int NULL;
	ALTER TABLE RequestEmpPersonalInfo ADD RequestEmpPlaceOfBirthID int NULL;
	
	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
    VALUES
		('PAY021','Double Pay Adjustment','Payroll', 0);

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES 
		('ENABLE_DOUBLE_PAY_ADJUSTMENT', 'Enable/Disable Double Pay Adjustment', 'Y');	
	
	CREATE TABLE DoublePayAdjustment(
		DoublePayAdjustID int IDENTITY(1,1) NOT NULL,
		EmpID int NULL,
		RpID int NULL, 
		SalesAchievementRate decimal(5, 2) NULL,
		DoublePayAdjustImportBatchID int NULL,
		DoublePayAdjustEffDate datetime
		CONSTRAINT PK_DoublePayAdjust PRIMARY KEY CLUSTERED 
		(
			DoublePayAdjustID
		)
	);
	
	CREATE TABLE DoublePayAdjustmentImportBatch
	(
		DoublePayAdjustImportBatchID int IDENTITY(1,1) NOT NULL,
		DoublePayAdjustImportBatchDateTime datetime NULL,
		DoublePayAdjustImportBatchUploadedBy int NULL,
		DoublePayAdjustImportBatchRemark ntext NULL
		 CONSTRAINT PK_DoublePayAdjustImportBatch PRIMARY KEY CLUSTERED 
		(
			DoublePayAdjustImportBatchID
		)
	);

	CREATE TABLE UploadDoublePayAdjustment
	(
		UploadDoublePayAdjustID int IDENTITY(1,1) NOT NULL,
		EmpID int NOT NULL,
		--IPEffDate datetime NOT NULL,
		SalesAchievementRate decimal(5, 2) NOT NULL,
		SessionID nvarchar(200) NOT NULL,
		TransactionDate datetime NOT NULL,
		ImportAction nvarchar(100) NULL,
		ModifiedFieldList ntext NULL,
		DoublePayAdjustEffDate datetime
		CONSTRAINT PK_UploadDoublePayAdjustment PRIMARY KEY CLUSTERED 
		(
			UploadDoublePayAdjustID ASC
		)
	);		
	
	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.23'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.23';
	
	COMMIT TRANSACTION
END

