DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.00.0022' 
BEGIN



	ALTER TABLE dbo.MPFPlan ADD
		MPFPlanExtendData ntext NULL
	
		-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.00.0023'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);
