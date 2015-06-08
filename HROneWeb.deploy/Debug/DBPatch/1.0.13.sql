DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.12'
BEGIN
	BEGIN TRANSACTION 
	
	ALTER TABLE PaymentRecord
		ADD EmpRPIDforBP int;
		
	ALTER TABLE EmpPersonalInfo
		ADD EmpNextSalaryIncrementDate datetime;
		
	ALTER TABLE EmpRecurringPayment
		ADD SchemeCode nvarchar(24) NULL,
			Capacity nvarchar(255) NULL,
			Point decimal(5,2) NULL
			
	ALTER TABLE UploadEmpRecurringPayment
		ADD SchemeCode nvarchar(24) NULL,
			Capacity nvarchar(255) NULL,
			Point decimal(5,2) NULL
			
	ALTER TABLE UploadEmpPersonalInfo
		ADD EmpNextSalaryIncrementDate datetime;
	
		
	CREATE TABLE [dbo].[PayScale](
		[PayScaleID] [int] IDENTITY(1,1) NOT NULL,
		[SchemeCode] [nvarchar](24) NULL,
		[Capacity] [nvarchar](255) NULL,
		[FirstPoint] [decimal](5, 2) NULL,
		[MidPoint] [decimal](5, 2) NULL,
		[LastPoint] [decimal](5, 2) NULL,
		[RecordCreatedDateTime] [datetime] NULL,
		[RecordCreatedBy] [int] NULL,
		[RecordLastModifiedDateTime] [datetime] NULL,
		[RecordLastModifiedBy] [int] NULL,
		[SynID] [nvarchar](255) NULL,
	 CONSTRAINT [PK_PayScale] PRIMARY KEY CLUSTERED 
	(
		[PayScaleID] ASC
	));
	
	CREATE TABLE [dbo].[PayScaleMap](
		[PayScaleMapID] [int] IDENTITY(1,1) NOT NULL,
		[EffectiveDate] [datetime] NULL,
		[ExpiryDate] [datetime] NULL,
		[SchemeCode] [nvarchar](24) NULL,
		[Point] [decimal](5, 2) NULL,
		[Salary] [decimal](15, 4) NULL,
		[RecordCreatedDateTime] [datetime] NULL,
		[RecordCreatedBy] [int] NULL,
		[RecordLastModifiedDateTime] [datetime] NULL,
		[RecordLastModifiedBy] [int] NULL,
		[SynID] [nvarchar](255) NULL,
	 CONSTRAINT [PK_PayScalePoints] PRIMARY KEY CLUSTERED 
	(
		[PayScaleMapID] ASC
	));
	
	CREATE TABLE [dbo].[PS_SalaryIncrementBatch](
		[BatchID] [int] IDENTITY(1,1) NOT NULL,
		[AsAtDate] [datetime] NULL,
		[DeferredBatch] [int] NULL,
		[PaymentCodeID] [int] NULL,
		[PaymentDate] [datetime] NULL,
		[Status] [nvarchar](1) NULL,
		[UploadDateTime] [datetime] NULL,
		[UploadBy] [int] NULL,
		[ConfirmDateTime] [datetime] NULL,
		[ConfirmBy] [int] NULL,
		[RecordCreatedDateTime] [datetime] NULL,
		[RecordCreatedBy] [int] NULL,
		[RecordLastModifiedDateTime] [datetime] NULL,
		[RecordLastModifiedBy] [int] NULL,
		[SynID] [nvarchar](255) NULL,
	CONSTRAINT [PK_PS_SalaryIncrementBatchID] PRIMARY KEY CLUSTERED 
	(
		[BatchID] ASC
	));

	CREATE TABLE [dbo].[PS_SalaryIncrementBatchDetail](
		[DetailID] [int] IDENTITY(1,1) NOT NULL,
		[BatchID] [int] NULL,
		[EmpID] [int] NULL,
		[EmpRPID] [int] NULL,
		[SchemeCode] [nvarchar](24) NULL,
		[Capacity] [nvarchar](255) NULL,
		[CurrentPoint] [decimal](5, 2) NULL,
		[NewPoint] [decimal](5, 2) NULL,
		[RecordCreatedDateTime] [datetime] NULL,
		[RecordCreatedBy] [int] NULL,
		[RecordLastModifiedDateTime] [datetime] NULL,
		[RecordLastModifiedBy] [int] NULL,
		[SynID] [nvarchar](255) NULL, 
	 CONSTRAINT [PK_PS_SalaryIncrementBatchDetail] PRIMARY KEY CLUSTERED 
	(
		[DetailID] ASC
	));
	
	CREATE TABLE [dbo].[PS_BackpayBatchDetail](
		[DetailID] [int] IDENTITY(1,1) NOT NULL,
		[AnnounceDate] [datetime] NULL,
		[EffectiveDate] [datetime] NULL,
		[BackpayDate] [datetime] NULL,
		[SchemeCode] [nvarchar](24) NULL,
		[Point] [decimal](5, 2) NULL,
		[Salary] [decimal](15, 4) NULL,
		[CurrentSalary] [decimal](15, 4) NULL,
		[PaymentCode] [nvarchar](24) NULL,
		[RecordCreatedDateTime] [datetime] NULL,
		[RecordCreatedBy] [int] NULL,
		[RecordLastModifiedDateTime] [datetime] NULL,
		[RecordLastModifiedBy] [int] NULL, 
		[SynID] [nvarchar](255) NULL, 
	 CONSTRAINT [PK_BackpayProcessDetail] PRIMARY KEY CLUSTERED 
	(
		[DetailID] ASC
	));
	
	INSERT INTO SystemFunction
	(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
	Values
	('SYS020', 'PayScale Master', 'System', 0);

	INSERT INTO SystemFunction
	(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
	Values
	('SYS021', 'PayScale Points Master', 'System', 0);

	INSERT INTO SystemFunction
	(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
	Values
	('PAY016', 'Payscale - Salary Increment Batch', 'Payroll', 0);

	INSERT INTO SystemFunction
	(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
	Values
	('PAY017', 'Payscale - Backpay Template Export', 'Payroll', 0);


	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('PASCALE_POINT_SYSTEM', 'Enable/Disable Payscale Point System', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('PAYSCALE_SALARY_INCREMENT_METHOD', 'Payscale Salary Increment Method', '1');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
	('AUTO_GENERATE_EMPNO', 'Enable/Disable Auto-Employee-No function', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
	('AUTO_GENERATE_EMPNO_FORMAT', 'Auto-Employee-No Number Format', '999999');



		-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.13'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.13';
	
	COMMIT TRANSACTION
END