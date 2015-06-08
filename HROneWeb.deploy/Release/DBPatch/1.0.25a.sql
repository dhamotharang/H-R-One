
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.25'
BEGIN

	CREATE NONCLUSTERED INDEX IDX_EmpStatus ON EmpPersonalInfo 
	(
		EmpStatus ASC
	);

	BEGIN TRANSACTION 

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.25a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.25a';
	
	COMMIT TRANSACTION
END

