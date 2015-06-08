DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.00.0025' 
BEGIN


	ALTER TABLE dbo.CostCenter
		DROP CONSTRAINT PK_CostCenter
	ALTER TABLE dbo.CostCenter ADD CONSTRAINT
	PK_CostCenter PRIMARY KEY CLUSTERED 
	(
		CostCenterID
	) 
	
		-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.00.0029'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

