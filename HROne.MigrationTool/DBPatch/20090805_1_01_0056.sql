DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0054' 
BEGIN
		
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('PER012','Employee Cost Center','Personnel')
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('CST001','Cost Allocation Trial Run','Cost Center')
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('CST002','Cost Allocation Trial Run Adjustment','Cost Center')
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('CST003','Cost Allocation Confirmation','Cost Center')
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('CST004','Cost Allocation Detail Export','Cost Center')
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('CST005','Cost Allocation Summary Export','Cost Center')

	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0056'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




