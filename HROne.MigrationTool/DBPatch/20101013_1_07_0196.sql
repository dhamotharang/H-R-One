
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0192' 
BEGIN

	Update ClaimsAndDeductions  
	set ClaimsAndDeductions.EmpPayrollID=pr.EmpPayrollID
	from PaymentRecord pr
	where ClaimsAndDeductions.PayRecID=pr.PayRecID

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0196'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





