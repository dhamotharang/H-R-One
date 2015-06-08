
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.05.0169' 
BEGIN

	Update PaymentCode
	Set PaymentCode='MCEE',
	PaymentCodeDesc='Employee Mandatory Contribution'
	Where PaymentCode='MPFE'

	Update PaymentCode
	Set PaymentCode='MCER',
	PaymentCodeDesc='Employer Mandatory Contribution'
	Where PaymentCode='MPFR'

	Update PaymentCode
	Set PaymentCode='MCEEADJ',
	PaymentCodeDesc='Employee Mandatory Contribution Adjustment'
	Where PaymentCode='MPFEADJ'

	Update PaymentCode
	Set PaymentCode='VCEE',
	PaymentCodeDesc='Employee Voluntary Contribution'
	Where PaymentCode='TPE'

	Update PaymentCode
	Set PaymentCode='VCER',
	PaymentCodeDesc='Employer Voluntary Contribution'
	Where PaymentCode='TPR'

	Update PaymentCode
	Set PaymentCode='VCEEADJ',
	PaymentCodeDesc='Employee Voluntary Contribution Adjustment'
	Where PaymentCode='TPEADJ'

	Update PaymentCode
	Set PaymentCode='PFUNDEE',
	PaymentCodeDesc='Employee P-Fund Contribution'
	Where PaymentCode='ORSOEE'

	Update PaymentCode
	Set PaymentCode='PFUNDEEADJ',
	PaymentCodeDesc='Employee P-Fund Contribution Adjustment'
	Where PaymentCode='ORSOEEADJ'

	--	Update Payment Type
	
		Update PaymentType
	Set PaymentTypeCode='MPFMCEE',
	PaymentTypeDesc='Employee Mandatory Contribution'
	Where PaymentTypeCode='MPFEE'

	Update PaymentType
	Set PaymentTypeCode='MPFMCER',
	PaymentTypeDesc='Employer Mandatory Contribution'
	Where PaymentTypeCode='MPFER'

	Update PaymentType
	Set PaymentTypeCode='MPFVCEE',
	PaymentTypeDesc='Employee Voluntary Contribution'
	Where PaymentTypeCode='TOPUPEE'

	Update PaymentType
	Set PaymentTypeCode='MPFVCER',
	PaymentTypeDesc='Employer Voluntary Contribution'
	Where PaymentTypeCode='TOPUPER'
	
	Update PaymentType
	SET PaymentTypeDesc='Employee P-Fund Contribution'
	Where PaymentTypeCode='PFUNDEE'

	Update PaymentType
	SET PaymentTypeDesc='Employer P-Fund Contribution'
	Where PaymentTypeCode='PFUNDER'
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.06.0172'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





