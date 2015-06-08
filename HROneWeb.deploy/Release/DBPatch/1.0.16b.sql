DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.16a'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE CommissionAchievement
		ADD CAEffDate datetime;
		
	ALTER TABLE UploadCommissionAchievement
		ADD CAEffDate datetime;

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.16b'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.16b';
	
	COMMIT TRANSACTION
END