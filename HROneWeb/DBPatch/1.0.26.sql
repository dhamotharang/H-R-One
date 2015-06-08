
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.25c'
BEGIN

	BEGIN TRANSACTION 

	ALTER TABLE EmpPersonalInfo
		ADD EmpESSLanguage nvarchar(10)

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.26'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.26';
	
	COMMIT TRANSACTION
END

