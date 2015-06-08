DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.17b'
BEGIN
	BEGIN TRANSACTION 
	
	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('ENABLE_PAYROLL_GROUP_SECURITY', 'Enable Payroll Group Security', 'N');
	
	CREATE TABLE PayrollGroupUsers
	(
		PayGroupUsersID int IDENTITY(1,1) NOT NULL,
		PayGroupID int NULL,
		UserID int NULL,
		RecordCreatedDateTime datetime NULL,
		RecordCreatedBy int NULL,
		RecordLastModifiedDateTime datetime NULL,
		RecordLastModifiedBy int NULL,
		 CONSTRAINT PK_PayrollGroupUsers PRIMARY KEY CLUSTERED 
		(
			PayGroupUsersID
		)
	);
	
	INSERT INTO PayrollGroupUsers (PayGroupID, UserID)
	SELECT PayGroupID, UserID
	FROM PayrollGroup, Users;
	

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.17c'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.17c';
	
	COMMIT TRANSACTION
END

