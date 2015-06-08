DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.13'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE LeaveCode
	Add LeaveAppUnit nvarchar(255) null;

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('WF_SEND_ACCEPT_EMAIL', 'Enable/Disable Send Accept E-mail', 'N');

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.14'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.14';
	
	COMMIT TRANSACTION
END
