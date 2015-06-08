DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.00.0040' 
BEGIN

	Insert into SystemFunction
       (FunctionCode, Description, FunctionCategory)
 	VALUES
       ('SYS007', 'Year End Bonus Plan Setup','System')

	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0042'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

