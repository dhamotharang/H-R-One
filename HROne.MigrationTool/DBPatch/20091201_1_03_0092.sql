DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0091' 
BEGIN

	Alter Table RosterClientSite Add 
		RosterClientSitePremisesNature nvarchar(50) NULL,
		RosterClientSiteServiceHours nvarchar(100) NULL,
		RosterClientSiteShift nvarchar(50) NULL
		

		
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0092'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





