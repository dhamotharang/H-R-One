DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.14'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE EmpRecurringPayment
		ADD EmpRPBasicSalary decimal(15, 4) NULL,
			EmpRPFPS decimal(15, 4) NULL,
			EmpRPOTCAmount decimal(15, 4) NULL;
			
	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('FIXED_PERCENTAGE_OF_SALARY', 'Monthly Salary based on pre-defined %', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('COMMISSION_BASE_FACTOR', 'Non-basic salary based commission', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT', 'Enable/Disable Monthly Attendance Report in ESS', 'Y');

	/********** EOT *****************/

	ALTER TABLE EmpPositionInfo ADD
			AuthorizationWorkFlowIDOTClaims INT NULL;

	CREATE TABLE RequestOTClaim
	(
		RequestOTClaimID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		RequestOTClaimEffectiveDate datetime NULL,
		RequestOTClaimDateExpiry datetime NULL,
		RequestOTClaimPeriodFrom datetime NULL,
		RequestOTClaimPeriodTo datetime NULL,
		RequestOTClaimHourFrom datetime NULL,
		RequestOTClaimHourTo datetime NULL,
		RequestOTHours float NULL,
		RequestOTClaimRemark nvarchar(50) NULL,
		RequestOTClaimCreateDate datetime NULL,
		CONSTRAINT PK_RequestOTClaim PRIMARY KEY CLUSTERED 
		(
			RequestOTClaimID
		)
	);

	CREATE TABLE OTClaim
	(
		OTClaimID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		OTClaimDateFrom datetime NULL,
		OTClaimDateTo datetime NULL,
		OTClaimTimeFrom datetime NULL,
		OTClaimTimeTo datetime NULL,
		OTClaimHours float NULL,
		OTClaimRemark nvarchar(50) NULL,
		RecordCreatedDateTime datetime NULL,
		RecordCreatedBy int NULL,
		RecordLastModifiedDateTime datetime NULL,
		RecordLastModifiedBy int NULL,
		OTClaimCancelID int NULL,
		EmpRequestID int NULL,
		RequestOTClaimID int NULL,
		SynID nvarchar(255) NULL,
		CONSTRAINT PK_OTClaim PRIMARY KEY CLUSTERED 
		(
			OTClaimID
		)
	);

	CREATE TABLE RequestOTClaimCancel
	(
		RequestOTClaimCancelID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		OTClaimID int NULL,
		RequestOTClaimCancelCreateDateTime datetime NULL,
		RequestOTClaimCancelReason ntext NULL,
		CONSTRAINT PK_RequestOTClaimCancel PRIMARY KEY CLUSTERED 
		(
			RequestOTClaimCancelID
		)
	);
	
	CREATE TABLE CommissionAchievement(
		CAID int IDENTITY(1,1) NOT NULL,
		EmpID int NULL,
		CAPercent decimal(5, 2) NULL,
		RpID int NULL, 
		CAImportBatchID int NULL
		CONSTRAINT PK_CommissionAchievement PRIMARY KEY CLUSTERED 
		(
			CAID
		)
	);
	
	CREATE TABLE CommissionAchievementImportBatch
	(
		CAImportBatchID int IDENTITY(1,1) NOT NULL,
		CAImportBatchDateTime datetime NULL,
		CAImportBatchUploadedBy int NULL,
		CAImportBatchRemark ntext NULL
		 CONSTRAINT PK_CommissionAchievementImportBatch PRIMARY KEY CLUSTERED 
		(
			CAImportBatchID
		)
	);

	CREATE TABLE UploadCommissionAchievement
	(
		UploadCAID int IDENTITY(1,1) NOT NULL,
		EmpID int NOT NULL,
		--CAEffDate datetime NOT NULL,
		CAPercent decimal(5, 2) NOT NULL,
		SessionID nvarchar(200) NOT NULL,
		TransactionDate datetime NOT NULL,
		ImportAction nvarchar(100) NULL,
		ModifiedFieldList ntext NULL,
		CONSTRAINT PK_UploadCommissionAchievement PRIMARY KEY CLUSTERED 
		(
			UploadCAID ASC
		)
	);	

	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory)
    VALUES
		('PAY018','F&V Monthly Achievement','Payroll');

	
	/********************************/


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.16'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.16';
	
	COMMIT TRANSACTION
END
