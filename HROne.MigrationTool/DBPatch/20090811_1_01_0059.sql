DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0058' 
BEGIN

	CREATE INDEX IX_PaymentRecord_EmpPayrollID ON PaymentRecord
	(
		EmpPayrollID
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0059'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




