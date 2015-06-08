
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.24'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE EmpSpouse ADD
		EmpGender nvarchar(100) NULL,
		EmpIsMedicalSchemaInsured INT NULL,
		EmpMedicalEffectiveDate DATETIME NULL,
		EmpMedicalExpiryDate DATETIME NULL

	ALTER TABLE UploadEmpSpouse ADD
		EmpGender nvarchar(100) NULL,
		EmpIsMedicalSchemaInsured INT NULL,
		EmpMedicalEffectiveDate DATETIME NULL,
		EmpMedicalExpiryDate DATETIME NULL	

	IF NOT EXISTS(SELECT * FROM systemParameter WHERE ParameterCode = 'ENABLE_DOUBLE_PAY_ADJUSTMENT')
		INSERT INTO SystemParameter
		VALUES
			('ENABLE_DOUBLE_PAY_ADJUSTMENT', 'Enable/Disable Double Pay Adjustment', 'N'); 
	ELSE
		UPDATE  SystemParameter
		SET		ParameterValue = 'N'
		WHERE   ParameterCode = 'ENABLE_DOUBLE_PAY_ADJUSTMENT'


	IF NOT EXISTS(SELECT * FROM systemParameter WHERE ParameterCode = 'ENABLE_BONUS_PROCESS')
		INSERT INTO SystemParameter
		VALUES
			('ENABLE_BONUS_PROCESS', 'Enable/Disable Bonus Process', 'N'); 

	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.24a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.24a';
	
	COMMIT TRANSACTION
END

