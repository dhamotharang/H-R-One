DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0066' 
BEGIN

	ALTER TABLE AttendancePlan ADD
		AttendancePlanOTPayCodeID int NULL,
		AttendancePlanLatePayCodeID int NULL,
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

	ALTER TABLE EmpPositionInfo ADD
		AttendancePlanID int NULL
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0067'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
	

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




