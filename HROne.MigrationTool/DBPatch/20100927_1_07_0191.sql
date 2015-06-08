
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0190' 
BEGIN

	UPDATE EmpExtraField
		SET EmpExtraFieldControlType='TextBox'
		WHERE EmpExtraFieldControlType IS NULL

	UPDATE PayrollProrataFormula
		SET PayFormRoundingRule='NOROUND'
		WHERE PayFormRoundingRule IS NULL
	
	UPDATE AttendanceFormula
		SET AttendanceFormulaRoundingRule='NOROUND'
		WHERE AttendanceFormulaRoundingRule IS NULL


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0191'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





