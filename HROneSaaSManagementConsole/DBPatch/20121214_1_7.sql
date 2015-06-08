

DECLARE @DBVERSION as varchar(100);
SET @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.6' 
Begin
	
	UPDATE CompanyDatabase SET 
	CompanyDBMaxEmployee=0,
	CompanyDBHasEChannel=1,
	CompanyDBHasIMGR=1,
	CompanyDBHasIStaff=0,
	CompanyDBInboxMaxQuotaMB=0

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.7'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



