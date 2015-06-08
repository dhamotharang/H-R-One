
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.18c'
BEGIN
	BEGIN TRANSACTION 

	TRUNCATE TABLE PayrollGroupUsers;
	
	UPDATE PayrollGroup
	SET		PayGroupIsPublic = 1;


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.19'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.19';
	
	COMMIT TRANSACTION
END

