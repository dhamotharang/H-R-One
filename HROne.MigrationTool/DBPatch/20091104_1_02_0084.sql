DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0083' 
BEGIN
	
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('PER900','Export Employee Information','Personnel', 0)
	
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0084'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





