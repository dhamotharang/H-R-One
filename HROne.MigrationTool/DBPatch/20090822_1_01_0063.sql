DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0062' 
BEGIN
	
ALTER TABLE RosterCode
	DROP COLUMN RosterCodeLateMinsUnit, RosterCodeEarlyLeaveMinsUnit, RosterCodeOTMinsUnit
	
	ALTER TABLE AttendancePlan ADD
	AttendancePlanOTMinsUnit int NULL,
	AttendancePlanLateMinsUnit int NULL

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0063'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
	

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




