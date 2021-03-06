
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.13.0322' 
BEGIN

	ALTER TABLE RosterCode ADD
		CostCenterID int null

	ALTER TABLE RosterClient ADD
		CostCenterID int null

	ALTER TABLE RosterClientSite ADD
		CostCenterID int null

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.13.0324'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





