DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0127' 
BEGIN

	UPDATE PaymentType
	SET PaymentTypeDesc='Employee MPF Contribution'
	WHERE PaymentTypeCode='MPFEE'
	
	UPDATE PaymentType
	SET PaymentTypeDesc='Employee P-Fund Contribution'
	WHERE PaymentTypeCode='PFUNDEE'
	
	UPDATE PaymentType
	SET PaymentTypeDesc='Employer P-Fund Contribution'
	WHERE PaymentTypeCode='PFUNDER'

	INSERT INTO PaymentCode
	(PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata, PaymentCodeIsProrataLeave, PaymentCodeIsMPF, PaymentCodeIsTopUP, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo)
	Select 'MCEEADJ','Employee Mandatory Contribution Adjustment', PaymentTypeID, 0, 0, 0, 0, 0, 0, 2, 'TO', 0, 97 
	from PaymentType 
	WHERE PaymentTypeCode='OTHERS'
	
	INSERT INTO PaymentCode
	(PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata, PaymentCodeIsProrataLeave, PaymentCodeIsMPF, PaymentCodeIsTopUP, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo)
	Select 'VCEEADJ','Employee Voluntary Contribution Adjustment', PaymentTypeID, 0, 0, 0, 0, 0, 0, 2, 'TO', 0, 98 
	from PaymentType
	WHERE PaymentTypeCode='OTHERS'

	INSERT INTO PaymentCode
	(PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata, PaymentCodeIsProrataLeave, PaymentCodeIsMPF, PaymentCodeIsTopUP, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo)
	Select 'PFUNDADJ','Employee P-Fund Contribution Adjustment', PaymentTypeID, 0, 0, 0, 0, 0, 0, 2, 'TO', 0, 99 
	from PaymentType
	WHERE PaymentTypeCode='OTHERS'

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.04.0130'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





