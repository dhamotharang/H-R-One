DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0107' 
BEGIN

	UPDATE SystemFunction
	SET Description='Employee Emergency Contact'
	WHERE FunctionCode='PER015';
	
	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('PER016','Employee Working Experience','Personnel', 0)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0112'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





