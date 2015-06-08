DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0068' 
BEGIN

	ALTER TABLE PaymentCode ADD
		PaymentCodeDisplaySeqNo int NULL
	
	CREATE TABLE Tmp_AttendancePlan
	(
		AttendancePlanID int NOT NULL IDENTITY (1, 1),
		AttendancePlanCode nvarchar(20) NULL,
		AttendancePlanDesc nvarchar(100) NULL,
		AttendancePlanOTFormula int NULL,
		AttendancePlanLateFormula int NULL,
		AttendancePlanOTMinsUnit int NULL,
		AttendancePlanLateMinsUnit int NULL,
		AttendancePlanOTPayCodeID int NULL,
		AttendancePlanLatePayCodeID int NULL,
		AttendancePlanOTMinsRoundingRule nvarchar(50) NULL,
		AttendancePlanLateMinsRoundingRule nvarchar(50) NULL,
		AttendancePlanBonusMaxTotalLateCount int NULL,
		AttendancePlanBonusMaxTotalLateMins int NULL,
		AttendancePlanBonusMaxTotalEarlyLeaveCount int NULL,
		AttendancePlanBonusMaxTotalEarlyLeaveMins int NULL,
		AttendancePlanBonusMaxTotalSLWithMedicalCertificate int NULL,
		AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate int NULL,
		AttendancePlanBonusMaxTotalAbsentCount int NULL,
		AttendancePlanBonusMaxTotalNonFullPayCasualLeave int NULL,
		AttendancePlanBonusMaxTotalInjuryLeave int NULL,
		AttendancePlanBonusAmount real NULL,
		AttendancePlanBonusAmountUnit nvarchar(1) NULL,
		AttendancePlanBonusPayCodeID int NULL
	)

	SET IDENTITY_INSERT Tmp_AttendancePlan ON

	IF EXISTS(SELECT * FROM AttendancePlan)
		 EXEC('INSERT INTO Tmp_AttendancePlan (AttendancePlanID, AttendancePlanCode, AttendancePlanDesc, AttendancePlanOTFormula, AttendancePlanLateFormula, AttendancePlanOTMinsUnit, AttendancePlanLateMinsUnit, AttendancePlanOTPayCodeID, AttendancePlanLatePayCodeID, AttendancePlanBonusMaxTotalLateCount, AttendancePlanBonusMaxTotalLateMins, AttendancePlanBonusMaxTotalEarlyLeaveCount, AttendancePlanBonusMaxTotalEarlyLeaveMins, AttendancePlanBonusMaxTotalSLWithMedicalCertificate, AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate, AttendancePlanBonusMaxTotalAbsentCount, AttendancePlanBonusMaxTotalNonFullPayCasualLeave, AttendancePlanBonusMaxTotalInjuryLeave, AttendancePlanBonusAmount, AttendancePlanBonusAmountUnit, AttendancePlanBonusPayCodeID)
			SELECT AttendancePlanID, AttendancePlanCode, AttendancePlanDesc, AttendancePlanOTFormula, AttendancePlanLateFormula, AttendancePlanOTMinsUnit, AttendancePlanLateMinsUnit, AttendancePlanOTPayCodeID, AttendancePlanLatePayCodeID, AttendancePlanBonusMaxTotalLateCount, AttendancePlanBonusMaxTotalLateMins, AttendancePlanBonusMaxTotalEarlyLeaveCount, AttendancePlanBonusMaxTotalEarlyLeaveMins, AttendancePlanBonusMaxTotalSLWithMedicalCertificate, AttendancePlanBonusMaxTotalSLWithoutMedicalCertificate, AttendancePlanBonusMaxTotalAbsentCount, AttendancePlanBonusMaxTotalNonFullPayCasualLeave, AttendancePlanBonusMaxTotalInjuryLeave, AttendancePlanBonusAmount, AttendancePlanBonusAmountUnit, AttendancePlanBonusPayCodeID FROM AttendancePlan WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT Tmp_AttendancePlan OFF

	DROP TABLE AttendancePlan

	EXECUTE sp_rename N'Tmp_AttendancePlan', N'AttendancePlan', 'OBJECT' 

	ALTER TABLE AttendancePlan ADD CONSTRAINT
		PK_AttendancePlan PRIMARY KEY CLUSTERED 
		(
		AttendancePlanID
		) 


		           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0069'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




