DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0056' 
BEGIN

	ALTER TABLE dbo.PayrollPeriod ADD
		PayPeriodIsAutoCreate int NULL
		
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0058'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




