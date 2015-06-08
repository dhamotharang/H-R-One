
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.25a'
BEGIN

	BEGIN TRANSACTION 

	IF EXISTS(SELECT * FROM SystemParameter WHERE ParameterCode = 'COMMISSION_BASE_FACTOR')
		DELETE SystemParameter WHERE ParameterCode = 'COMMISSION_BASE_FACTOR'

	ALTER TABLE EmpRecurringPayment
		DROP COLUMN EmpRPOTCAmount
		
	ALTER TABLE UploadEmpRecurringPayment
		DROP COLUMN EmpRPOTCAmount

	ALTER TABLE PaymentCode 
		ADD PaymentCodeIsHitRateBased int NULL DEFAULT 0,
			PaymentCodeDefaultRateAtMonth1 decimal(15,4) NULL DEFAULT 0,
			PaymentCodeDefaultRateAtMonth2 decimal(15,4) NULL DEFAULT 0,
			PaymentCodeDefaultRateAtMonth3 decimal(15,4) NULL DEFAULT 0

	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
    VALUES
		('PAY022','Hit-Rate Based Payment Process','Payroll', 0);


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.25b'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.25b';
	
	COMMIT TRANSACTION
END

