
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.12.0312' 
BEGIN

	UPDATE YEBPLAN
	SET YEBPlanEligibleUnit=''
	WHERE YEBPlanEligibleUnit IS NULL
	
	INSERT INTO PaymentType
           (PaymentTypeCode
           ,PaymentTypeDesc)
		VALUES
           ('BONUS','Bonus')
		
	INSERT INTO PaymentType
           (PaymentTypeCode
           ,PaymentTypeDesc)
		VALUES
           ('COMMISSION','Commission')

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.12.0313'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





