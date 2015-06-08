DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0081' 
BEGIN

	Update Company
	set CompanyBankHolderName=''
	Where CompanyBankHolderName IS NULL
	
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0082'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





