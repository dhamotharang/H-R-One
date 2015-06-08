DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0070' 
BEGIN

	ALTER TABLE ORSOPlan ADD
		ORSOPlanEmployerDecimalPlace int NULL,
		ORSOPlanEmployerRoundingRule nvarchar(50) NULL,
		ORSOPlanEmployeeDecimalPlace int NULL,
		ORSOPlanEmployeeRoundingRule nvarchar(50) NULL
	
	ALTER TABLE AVCPlan ADD
		AVCPlanEmployerDecimalPlace int NULL,
		AVCPlanEmployerRoundingRule nvarchar(50) NULL,
		AVCPlanEmployeeDecimalPlace int NULL,
		AVCPlanEmployeeRoundingRule nvarchar(50) NULL

	ALTER TABLE MPFPlan ADD
		MPFPlanEmployerDecimalPlace int NULL,
		MPFPlanEmployerRoundingRule nvarchar(50) NULL,
		MPFPlanEmployeeDecimalPlace int NULL,
		MPFPlanEmployeeRoundingRule nvarchar(50) NULL

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('SYS008','Audit Trail','System')
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0071'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





