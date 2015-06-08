
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.22'
BEGIN
	BEGIN TRANSACTION 

	IF (NOT EXISTS(SELECT * FROM SystemFunction WHERE FunctionCode = 'PAY020'))
	BEGIN
		INSERT INTO SystemFunction VALUES('PAY020', 'Bonus Process', 'Payroll', 0);
	END;
	

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.22a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.22a';
	
	COMMIT TRANSACTION
END

