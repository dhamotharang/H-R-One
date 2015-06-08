
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.18a'
BEGIN
	BEGIN TRANSACTION 

	-- default all existing paygroup are public 
	UPDATE PayrollGroup
	SET    PayGroupIsPublic = 1;
	
	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
	VALUES
		('RPT254', 'MPF First Contribution Statement', 'Payroll & MPF Reports', 0);

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.18b'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.18b';
	
	COMMIT TRANSACTION
END

