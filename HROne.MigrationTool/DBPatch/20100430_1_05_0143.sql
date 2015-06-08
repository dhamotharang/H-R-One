
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.04.0132' 
BEGIN
	ALTER TABLE RosterClient ADD
		RosterClientMappingSiteCodeToHLevelID int NULL
		
	Alter Table AttendanceRecord ADD
		AttendanceRecordExtendData NTEXT NULL

	CREATE TABLE AVCPlanPaymentCeiling
	(
		AVCPlanPaymentCeilingID int NOT NULL IDENTITY (1, 1),
		AVCPlanID int NULL,
		PaymentCodeID int NULL,
		AVCPlanPaymentCeilingAmount real NULL
	)
	
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.05.0143'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





