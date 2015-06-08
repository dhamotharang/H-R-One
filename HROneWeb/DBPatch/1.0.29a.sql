
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.29'
BEGIN

	BEGIN TRANSACTION 

	-- Insert version of Database --
	INSERT INTO SystemFunction VALUES('CUSTOM004', 'Payment Summary List (WaiJi)', 'Payroll & MPF Reports', -1);	

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.29a'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.29a';
	
	COMMIT TRANSACTION
END

