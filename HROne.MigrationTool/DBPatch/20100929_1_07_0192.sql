
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0191' 
BEGIN

	ALTER TABLE ClaimsAndDeductions
		ADD EmpPayrollID int NULL, 
		CNDImportBatchID int NULL 

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0192'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





