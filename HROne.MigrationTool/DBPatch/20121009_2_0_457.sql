
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.0.448' 
BEGIN

	UPDATE MPFScheme 
	SET MPFSchemeTrusteeCode='Others'
	WHERE MPFSchemeTrusteeCode IS NULL
	
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.0.457'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





