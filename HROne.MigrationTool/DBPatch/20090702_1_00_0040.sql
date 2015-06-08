DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.00.0036' 
BEGIN

	CREATE TABLE YEBPlan
	(
		YEBPlanID int NOT NULL IDENTITY (1, 1),
		YEBPlanCode nvarchar(20) Null,
		YEBPlanDesc nvarchar(100) Null,
		YEBPlanPaymentBaseMethod nvarchar(10) Null,
		YEBPlanRPPaymentCodeID int NULL,
		YEBPlanMultiplier real NULL,
		YEBPlanPaymentCodeID int NULL,
		YEBPlanIsEligibleAfterProbation int NULL,
		YEBPlanEligiblePeriod int NULL,
		YEBPlanEligibleUnit nvarchar(1) NULL,
		CONSTRAINT PK_YEBPlan Primary Key CLUSTERED (YEBPlanID)
	)

	ALTER TABLE PayrollGroup ADD
		PayGroupStatHolEligibleAfterProbation int NULL,
		PayGroupYEBStartPayrollMonth int NULL,
		PayGroupYEBMonthFrom int NULL,
		PayGroupYEBMonthTo int NULL
	
	
	CREATE TABLE Tmp_EmpPayroll
		(
		EmpPayrollID int NOT NULL IDENTITY (1, 1),
		PayPeriodID int NULL,
		EmpID int NULL,
		EmpPayStatus nvarchar(1) NULL,
		PayBatchID int NULL,
		EmpPayIsRP nvarchar(1) NULL,
		EmpPayIsCND nvarchar(1) NULL,
		EmpPayIsYEB nvarchar(1) NULL,
		EmpPayIsHistoryAdj nvarchar(1) NULL,
		EmpPayTrialRunDate datetime NULL,
		EmpPayTrialRunBy int NULL,
		EmpPayConfirmDate datetime NULL,
		EmpPayConfirmBy int NULL,
		EmpPayNumOfDayCount real NULL
		)
		
	SET IDENTITY_INSERT Tmp_EmpPayroll ON

	IF EXISTS(SELECT * FROM EmpPayroll)
		 EXEC('INSERT INTO Tmp_EmpPayroll (EmpPayrollID, PayPeriodID, EmpID, EmpPayStatus, PayBatchID, EmpPayIsRP, EmpPayIsCND, EmpPayIsHistoryAdj, EmpPayTrialRunDate, EmpPayTrialRunBy, EmpPayConfirmDate, EmpPayConfirmBy, EmpPayNumOfDayCount)
			SELECT EmpPayrollID, PayPeriodID, EmpID, EmpPayStatus, PayBatchID, EmpPayIsRP, EmpPayIsCND, EmpPayIsHistoryAdj, EmpPayTrialRunDate, EmpPayTrialRunBy, EmpPayConfirmDate, EmpPayConfirmBy, EmpPayNumOfDayCount FROM EmpPayroll WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_EmpPayroll OFF
	DROP TABLE EmpPayroll
	EXECUTE sp_rename N'Tmp_EmpPayroll', N'EmpPayroll', 'OBJECT' 
	ALTER TABLE EmpPayroll ADD CONSTRAINT
		PK_EmpPayroll PRIMARY KEY CLUSTERED 
		(
		EmpPayrollID
		)

	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.00.0040'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

