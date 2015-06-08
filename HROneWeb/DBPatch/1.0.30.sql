
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.29c'
BEGIN

	BEGIN TRANSACTION 

	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
	VALUES
		('CUSTOM005', 'F&V Customization: MPF Remittance Statement', 'Payroll & MPF Reports', -1);

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.30'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.30';
	
	COMMIT TRANSACTION
END

