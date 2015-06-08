DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0099' 
BEGIN

	ALTER TABLE Users ADD
		UserLanguage nvarchar(10) NULL

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0101'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





