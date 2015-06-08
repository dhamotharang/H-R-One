
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.06.0174' 
BEGIN

	Alter Table LeavePlan
		Add LeavePlanProrataSkipFeb29 int null
			
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.06.0175'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





