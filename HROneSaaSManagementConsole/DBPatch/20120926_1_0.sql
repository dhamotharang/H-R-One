

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.9' 
Begin

	ALTER TABLE CompanyDatabase ADD
		CompanyDBClientBank NVARCHAR(100) NULL


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.0'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



