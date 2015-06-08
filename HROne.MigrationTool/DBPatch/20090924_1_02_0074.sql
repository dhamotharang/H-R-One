DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0073' 
BEGIN
	
		update paymentCode
		set 
		paymentCodeDecimalPlace=2,
		paymentCodeRoundingRule='To'
		where paymentCodeRoundingRule is null
      
      	Update ORSOPlan 
      	set
		ORSOPlanEmployerDecimalPlace=2,
		ORSOPlanEmployerRoundingRule='TO',
		ORSOPlanEmployeeDecimalPlace=2,
		ORSOPlanEmployeeRoundingRule='TO'
		where ORSOPlanEmployerRoundingRule is NULL
		and ORSOPlanEmployeeRoundingRule  is NULL
		
	
      	Update MPFPlan 
      	set
		MPFPlanEmployerDecimalPlace=2,
		MPFPlanEmployerRoundingRule='TO',
		MPFPlanEmployeeDecimalPlace=2,
		MPFPlanEmployeeRoundingRule='TO'
		where MPFPlanEmployerRoundingRule is NULL
		and MPFPlanEmployeeRoundingRule  is NULL

      	Update AVCPlan 
      	set
		AVCPlanEmployerDecimalPlace=2,
		AVCPlanEmployerRoundingRule='TO',
		AVCPlanEmployeeDecimalPlace=2,
		AVCPlanEmployeeRoundingRule='TO'
		where AVCPlanEmployerRoundingRule is NULL
		and AVCPlanEmployeeRoundingRule  is NULL

           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0074'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





