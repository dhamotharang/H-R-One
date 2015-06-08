

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.5' 
Begin
	
	ALTER TABLE CompanyDatabase ADD
		CompanyDBMaxEmployee INT NULL,
		CompanyDBHasEChannel INT NULL,
		CompanyDBHasIMGR INT NULL,
		CompanyDBHasIStaff INT NULL,
		CompanyDBInboxMaxQuotaMB INT NULL


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.6'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



