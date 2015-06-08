
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.05.0153' 
BEGIN
	ALTER TABLE RosterCode ADD
		LeaveCodeID int NULL
			
	ALTER TABLE RosterTable ADD
		LeaveAppID int NULL
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.05.0154'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





