

DECLARE @DBVERSION as varchar(100);
SET @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.7' 
Begin
	
	ALTER TABLE CompanyDatabase ADD
		CompanyDBAutopayMPFFileHasHSBCHASE INT NULL,
		CompanyDBAutopayMPFFileHasOthers INT NULL


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.8'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



