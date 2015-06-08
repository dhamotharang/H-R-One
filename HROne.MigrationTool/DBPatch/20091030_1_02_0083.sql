DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0082' 
BEGIN
	
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('PAY900','Rollback Payroll Process','Payroll', 0)
	
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('PER007-1','Employee Recurring Payment Information','Personnel', 0)	
           
	Insert into UserGroupFunction
		(UserGroupID,FunctionID,FunctionAllowRead,FunctionAllowWrite)
	Select ugf.UserGroupID,f.FunctionID,ugf.FunctionAllowRead,ugf.FunctionAllowWrite
		From UserGroupFunction ugf, SystemFunction f
		where ugf.FunctionID in (Select FunctionID from SystemFunction where FunctionCode='PER007')
		AND f.FunctionID in (Select FunctionID from SystemFunction where FunctionCode='PER007-1')
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0083'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





