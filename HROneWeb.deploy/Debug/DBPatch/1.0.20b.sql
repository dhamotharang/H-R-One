
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.20a'
BEGIN
	BEGIN TRANSACTION 

	IF (NOT EXISTS(SELECT * FROM SystemFunction WHERE FunctionCode = 'RPT213'))
	BEGIN
		INSERT INTO SystemFunction VALUES('RPT213','New Join Payment Summary','Payroll & MPF Reports', 0);
	END;

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.20b'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.20b';
	
	COMMIT TRANSACTION
END

