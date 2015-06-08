DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0052' 
BEGIN
		
ALTER TABLE dbo.EmpAVCPlan ADD
	DefaultMPFPlanID int NULL
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0054'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




