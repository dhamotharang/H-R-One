DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0049' 
BEGIN

	Alter Table CostAllocationDetail ADD
		CostAllocationDetailIsContribution int Null default 0
		


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0050'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




