DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0074' 
BEGIN

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('SEC003','Authorization Group Setup','Security')
           
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('SEC004','Assign Authorizer','Security')

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0075'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





