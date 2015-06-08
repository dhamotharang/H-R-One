DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0077' 
BEGIN

	Update SystemFunction
	set FunctionIsHidden=0
	where FunctionIsHidden is null
	
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('PER999','Import Employee','Personnel', 1)
	
	
	
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0078'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





