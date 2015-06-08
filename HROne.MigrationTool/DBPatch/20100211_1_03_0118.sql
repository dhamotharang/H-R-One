DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0116' 
BEGIN

	INSERT INTO LEAVETYPE
	(LeaveType,LeaveTypeDesc,LeaveDecimalPlace,LeaveSystemUse)
	Values
	('INJURY','Injury Leave',2,1)	


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0118'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





