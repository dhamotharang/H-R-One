
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.24a'
BEGIN
	BEGIN TRANSACTION 

	INSERT INTO SystemFunction VALUES('CST007', 'Average Cost Centre Export', 'Cost Center', 0);

	UPDATE SystemFunction
	SET    Description = 'Payroll Allocation Report - Details'
	WHERE  FunctionCode = 'RPT255'


	IF NOT EXISTS(SELECT * FROM systemParameter WHERE ParameterCode = 'WF_SEND_ACCEPT_EMAIL')
	BEGIN
		IF NOT EXISTS(SELECT * FROM systemParameter WHERE ParameterCode = 'WF_SENT_ACCEPT_EMAIL')
			INSERT INTO SystemParameter
				(ParameterCode, ParameterDesc, ParameterValue)
			VALUES
				('WF_SEND_ACCEPT_EMAIL', 'Enable/Disable Send Accept E-mail', 'N');
		ELSE
			UPDATE SystemParameter
			SET ParameterCode = 'WF_SEND_ACCEPT_EMAIL'
			WHERE ParameterCode = 'WF_SENT_ACCEPT_EMAIL'
	END


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.25'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.25';
	
	COMMIT TRANSACTION
END

