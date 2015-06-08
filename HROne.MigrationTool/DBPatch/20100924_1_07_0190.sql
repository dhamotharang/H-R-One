
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.06.0186' 
BEGIN

	ALTER TABLE EmpExtraField
		ADD EmpExtraFieldControlType NVARCHAR(100) NULL 

	ALTER TABLE PayrollProrataFormula
		ADD ReferencePayFormID INT NULL, 
		PayFormDecimalPlace INT NULL, 
		PayFormRoundingRule NVARCHAR(50) NULL

	ALTER TABLE AttendanceFormula
		ADD AttendanceFormulaDecimalPlace INT NULL, 
		AttendanceFormulaRoundingRule NVARCHAR(50) NULL

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0190'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





