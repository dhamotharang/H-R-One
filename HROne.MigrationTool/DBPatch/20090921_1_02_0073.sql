DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0072' 
BEGIN

	CREATE TABLE dbo.Tmp_PayrollGroup
	(
		PayGroupID int NOT NULL IDENTITY (1, 1),
		PayGroupCode nvarchar(20) NULL,
		PayGroupDesc nvarchar(100) NULL,
		PayGroupFreq nvarchar(1) NULL,
		PayGroupDefaultStartDay int NULL,
		PayGroupDefaultEndDay int NULL,
		PayGroupNextStartDate datetime NULL,
		PayGroupNextEndDate datetime NULL,
		CurrentPayPeriodID int NULL,
		PayGroupLeaveDeductFormula int NULL,
		PayGroupLeaveAllowFormula int NULL,
		PayGroupNewJoinFormula int NULL,
		PayGroupTerminatedFormula int NULL,
		PayGroupTerminatedALCompensationDailyFormula int NULL,
		PayGroupTerminatedALCompensationPaymentCodeID int NULL,
		PayGroupTerminatedPaymentInLieuMonthlyBaseMethod nvarchar(100) NULL,
		PayGroupTerminatedPaymentInLieuDailyFormula int NULL,
		PayGroupTerminatedPaymentInLieuERPaymentCodeID int NULL,
		PayGroupTerminatedPaymentInLieuEEPaymentCodeID int NULL,
		PayGroupTerminatedLSPSPMonthlyBaseMethod nvarchar(100) NULL,
		PayGroupTerminatedLSPPaymentCodeID int NULL,
		PayGroupTerminatedSPPaymentCodeID int NULL,
		PayGroupStatHolDeductFormula int NULL,
		PayGroupStatHolAllowFormula int NULL,
		PayGroupStatHolAllowPaymentCodeID int NULL,
		PayGroupStatHolDeductPaymentCodeID int NULL,
		PayGroupPayAdvance int NULL,
		PayGroupStatHolEligiblePeriod int NULL,
		PayGroupStatHolEligibleUnit nvarchar(1) NULL,
		PayGroupStatHolEligibleAfterProbation int NULL,
		PayGroupYEBStartPayrollMonth int NULL,
		PayGroupYEBMonthFrom int NULL,
		PayGroupYEBMonthTo int NULL
	)
		
	SET IDENTITY_INSERT dbo.Tmp_PayrollGroup ON
	IF EXISTS(SELECT * FROM dbo.PayrollGroup)
		 EXEC('INSERT INTO dbo.Tmp_PayrollGroup (PayGroupID, PayGroupCode, PayGroupDesc, PayGroupFreq, PayGroupDefaultStartDay, PayGroupDefaultEndDay, PayGroupNextStartDate, PayGroupNextEndDate, CurrentPayPeriodID, PayGroupLeaveDeductFormula, PayGroupLeaveAllowFormula, PayGroupNewJoinFormula, PayGroupTerminatedFormula, PayGroupStatHolDeductFormula, PayGroupStatHolAllowFormula, PayGroupStatHolAllowPaymentCodeID, PayGroupStatHolDeductPaymentCodeID, PayGroupPayAdvance, PayGroupStatHolEligiblePeriod, PayGroupStatHolEligibleUnit, PayGroupStatHolEligibleAfterProbation, PayGroupYEBStartPayrollMonth, PayGroupYEBMonthFrom, PayGroupYEBMonthTo)
			SELECT PayGroupID, PayGroupCode, PayGroupDesc, PayGroupFreq, PayGroupDefaultStartDay, PayGroupDefaultEndDay, PayGroupNextStartDate, PayGroupNextEndDate, CurrentPayPeriodID, PayGroupLeaveDeductFormula, PayGroupLeaveAllowFormula, PayGroupNewJoinFormula, PayGroupTerminatedFormula, PayGroupStatHolDeductFormula, PayGroupStatHolAllowFormula, PayGroupStatHolAllowPaymentCodeID, PayGroupStatHolDeductPaymentCodeID, PayGroupPayAdvance, PayGroupStatHolEligiblePeriod, PayGroupStatHolEligibleUnit, PayGroupStatHolEligibleAfterProbation, PayGroupYEBStartPayrollMonth, PayGroupYEBMonthFrom, PayGroupYEBMonthTo FROM dbo.PayrollGroup WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT dbo.Tmp_PayrollGroup OFF
	DROP TABLE dbo.PayrollGroup
	EXECUTE sp_rename N'dbo.Tmp_PayrollGroup', N'PayrollGroup', 'OBJECT' 
	ALTER TABLE dbo.PayrollGroup ADD CONSTRAINT
		PK_PayrollGroup PRIMARY KEY CLUSTERED 
		(
		PayGroupID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0073'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





