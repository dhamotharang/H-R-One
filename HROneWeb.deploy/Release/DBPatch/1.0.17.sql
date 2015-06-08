DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.16b'
BEGIN
	BEGIN TRANSACTION 

	UPDATE SystemFunction
	SET    Description = 'Monthly Achievement Commission',
	       FunctionIsHidden = 0
	WHERE  FunctionCode = 'PAY018';

	UPDATE SystemParameter
	SET	   ParameterCode = 'MONTHLY_ACHIEVEMENT_COMMISSION',
	       ParameterDesc = 'Enable/Disable Monthly Achievement Commission'
	WHERE  ParameterCode = 'FIXED_PERCENTAGE_OF_SALARY';

	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
    VALUES
		('PAY019','Incentive Payment','Payroll', 0);

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES 
		('INCENTIVE_PAYMENT', 'Enable/Disable Incentive Payment', 'Y');
		
	
	CREATE TABLE IncentivePayment(
		IPID int IDENTITY(1,1) NOT NULL,
		EmpID int NULL,
		IPPercent decimal(5, 2) NULL,
		RpID int NULL, 
		IPImportBatchID int NULL,
		IPEffDate datetime
		CONSTRAINT PK_IncentivePayment PRIMARY KEY CLUSTERED 
		(
			IPID
		)
	);
	
	CREATE TABLE IncentivePaymentImportBatch
	(
		IPImportBatchID int IDENTITY(1,1) NOT NULL,
		IPImportBatchDateTime datetime NULL,
		IPImportBatchUploadedBy int NULL,
		IPImportBatchRemark ntext NULL
		 CONSTRAINT PK_IncentivePaymentImportBatch PRIMARY KEY CLUSTERED 
		(
			IPImportBatchID
		)
	);

	CREATE TABLE UploadIncentivePayment
	(
		UploadIPID int IDENTITY(1,1) NOT NULL,
		EmpID int NOT NULL,
		--IPEffDate datetime NOT NULL,
		IPPercent decimal(5, 2) NOT NULL,
		SessionID nvarchar(200) NOT NULL,
		TransactionDate datetime NOT NULL,
		ImportAction nvarchar(100) NULL,
		ModifiedFieldList ntext NULL,
		IPEffDate datetime
		CONSTRAINT PK_UploadIncentivePayment PRIMARY KEY CLUSTERED 
		(
			UploadIPID ASC
		)
	);	
	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.17'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.17';
	
	COMMIT TRANSACTION
END
