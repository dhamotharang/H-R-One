

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.1' 
Begin

	ALTER TABLE CompanyDatabase ADD
		CompanyDBResetDefaultUserLoginID NVARCHAR(255) NULL,
		CompanyDBResetDefaultUserPassword NVARCHAR(255) NULL

	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.2'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



