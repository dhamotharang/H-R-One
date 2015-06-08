DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.00.0024' 
BEGIN


	ALTER TABLE dbo.EmpMPFPlan ADD
		EmpMPFPlanExtendData ntext NULL

	Insert into SystemFunction
           (FunctionCode, Description, FunctionCategory)
     	VALUES
           ('PAY011', 'Generate MPF File','Payroll')
	
		-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.00.0025'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);
