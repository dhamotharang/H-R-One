DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0085' 
BEGIN
	
	CREATE TABLE Tmp_YEBPlan
		(
		YEBPlanID int NOT NULL IDENTITY (1, 1),
		YEBPlanCode nvarchar(20) NULL,
		YEBPlanDesc nvarchar(100) NULL,
		YEBPlanPaymentBaseMethod nvarchar(10) NULL,
		YEBPlanRPPaymentCodeID int NULL,
		YEBPlanMultiplier real NULL,
		YEBPlanProrataMethod nvarchar(1) NULL,
		YEBPlanPaymentCodeID int NULL,
		YEBPlanIsEligibleAfterProbation int NULL,
		YEBPlanEligiblePeriod int NULL,
		YEBPlanEligibleUnit nvarchar(1) NULL,
		YEBPlanIsGlobal int NULL
		)  
	SET IDENTITY_INSERT Tmp_YEBPlan ON
	IF EXISTS(SELECT * FROM YEBPlan)
		 EXEC('INSERT INTO Tmp_YEBPlan (YEBPlanID, YEBPlanCode, YEBPlanDesc, YEBPlanPaymentBaseMethod, YEBPlanRPPaymentCodeID, YEBPlanMultiplier, YEBPlanProrataMethod, YEBPlanPaymentCodeID, YEBPlanIsEligibleAfterProbation, YEBPlanEligiblePeriod, YEBPlanEligibleUnit, YEBPlanIsGlobal)
			SELECT YEBPlanID, YEBPlanCode, YEBPlanDesc, YEBPlanPaymentBaseMethod, YEBPlanRPPaymentCodeID, YEBPlanMultiplier, ''M'', YEBPlanPaymentCodeID, YEBPlanIsEligibleAfterProbation, YEBPlanEligiblePeriod, YEBPlanEligibleUnit, YEBPlanIsGlobal FROM YEBPlan WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_YEBPlan OFF
	DROP TABLE YEBPlan
	EXECUTE sp_rename N'Tmp_YEBPlan', N'YEBPlan', 'OBJECT' 
	ALTER TABLE YEBPlan ADD CONSTRAINT
		PK_YEBPlan PRIMARY KEY CLUSTERED 
		(
		YEBPlanID
		) 
		
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0086'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





