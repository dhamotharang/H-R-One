
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.20b'
BEGIN
	BEGIN TRANSACTION 

	IF (NOT EXISTS(SELECT * FROM SystemFunction WHERE FunctionCode = 'RPT255'))
		INSERT INTO SystemFunction VALUES('RPT255', 'Payroll Allocation Report - Details', 'Payroll & MPF Reports', 0);
	ELSE
		UPDATE SystemFunction SET Description = 'Payroll Allocation Report - Details' WHERE FunctionCode = 'RPT255';	
	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.20c'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.20c';
	
	COMMIT TRANSACTION
END

