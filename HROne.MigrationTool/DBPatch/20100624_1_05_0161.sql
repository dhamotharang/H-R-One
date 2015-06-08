
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.05.0155' 
BEGIN

	Alter Table PaymentCode ADD
		PaymentCodeNotRemoveContributionFromTopUp int NULL
			
	Alter Table AVCPlan ADD
		AVCPlanNotRemoveContributionFromTopUp int NULL

	CREATE TABLE AVCPlanPaymentConsider
	(
		AVCPlanPaymentConsiderID int NOT NULL IDENTITY (1, 1),
		AVCPlanID int NULL,
		PaymentCodeID int NULL,
		AVCPlanPaymentConsiderAfterMPF int NULL
	)
	
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.05.0161'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





