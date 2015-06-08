
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.20'
BEGIN
	BEGIN TRANSACTION 

	IF (NOT EXISTS(SELECT * FROM SystemFunction WHERE FunctionCode = 'RPT255'))
	BEGIN
		INSERT INTO SystemFunction VALUES('RPT255', 'Provident Fund Contribution Summary', 'Payroll & MPF Reports', 0);
	END;

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.20a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.20a';
	
	COMMIT TRANSACTION
END

