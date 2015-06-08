
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.23'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE RequestEmpPersonalInfo ADD RequestEmpPlaceOfBirth nvarchar(255);
	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.23a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.23a';
	
	COMMIT TRANSACTION
END

