DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0065' 
BEGIN

	ALTER TABLE EmpContractTerms ADD
		PayCodeID int NULL,
		EmpContractGratuityMethod nvarchar(1) NULL,
		EmpAccID int NULL

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0066'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
	

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




