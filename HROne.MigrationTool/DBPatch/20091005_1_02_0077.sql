DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0075' 
BEGIN

	Update PaymentRecord
	Set PayRecRemark = (Select CNDRemark from ClaimsAndDeductions cnd where cnd.PayRecID=PaymentRecord.PayRecID)
	Where PayRecRemark is null
	
	ALTER TABLE SystemFunction ADD 
		FunctionIsHidden int NULL
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0077'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





