
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.22a'
BEGIN
	BEGIN TRANSACTION 

	DROP TABLE BonusProcess;
	
	CREATE TABLE BonusProcess
	(
		BonusProcessID int IDENTITY(1,1) NOT NULL,
		BonusProcessMonth DateTime NULL,
		BonusProcessDesc nvarchar(255) NULL,
		BonusProcessStatus nvarchar(1) NULL,
		BonusProcessPayCodeID int NULL,
		BonusProcessPayDate DateTime NULL,
		BonusProcessSalaryMonth DateTime NULL,
		BonusProcessPeriodFr DateTime NULL,
		BonusProcessPeriodTo DateTime NULL,
		BonusProcessStdRate decimal(5, 4) NULL,
		BonusProcessRank1 decimal(5, 2) NULL,
		BonusProcessRank2 decimal(5, 2) NULL,
		BonusProcessRank3 decimal(5, 2) NULL,
		BonusProcessRank4 decimal(5, 2) NULL,
		BonusProcessRank5 decimal(5, 2) NULL,		
		RecordCreatedDateTime DateTime NULL,
		RecordCreatedBy int NULL,
		RecordLastModifiedDateTime DateTime NULL,
		RecordLastModifiedBy int NULL,
		CONSTRAINT PK_BonusProcess PRIMARY KEY CLUSTERED 
		(
			BonusProcessID ASC
		)
	);
		
	DROP TABLE EmpBonusProcess; 
	
	CREATE TABLE EmpBonusProcess
	(
		EmpBonusProcessID int IDENTITY(1, 1) NOT NULL,
		EmpID int NULL,
		BonusProcessID int NULL,
		EmpBonusProcessType nvarchar(1) NULL,
		EmpBonusProcessRank nvarchar(1) NULL,
		EmpBonusProcessTargetSalary decimal(15,4) NULL,
		EmpBonusProcessBonusProportion decimal(5, 4) NULL,
		EmpBonusProcessBonusAmount decimal(15,4) NULL,
		RecordCreatedDateTime DateTime NULL,
		RecordCreatedBy int NULL,
		RecordLastModifiedDateTime DateTime NULL,
		RecordLastModifiedBy int NULL,
		CONSTRAINT PK_EmpBonusProcess PRIMARY KEY CLUSTERED
		(
			EmpBonusProcessID ASC
		)
	);

	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.22b'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.22b';
	
	COMMIT TRANSACTION
END

