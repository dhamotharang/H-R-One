
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.28c'
BEGIN

	BEGIN TRANSACTION 

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.29'
	WHERE	ParameterCode='DBVERSION';
	

	SELECT  @DBVERSION='1.0.29';
	
	COMMIT TRANSACTION
END

