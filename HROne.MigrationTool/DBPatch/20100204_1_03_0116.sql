DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0112' 
BEGIN

	UPDATE MPFScheme
	SET MPFSchemeDesc='Taifook MPF Retirement Fund'
	WHERE MPFSchemeCode='MT00121';
	


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0116'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





