DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.16'
BEGIN
	BEGIN TRANSACTION 

	DELETE SystemFunction WHERE FunctionCode = 'PAY018';

	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory)
    VALUES
		('PAY018','F&V Monthly Achievement','Payroll');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('ESS_ENABLE_OTCLAIM', 'Enable/Disable ECL Requisition', 'N');

	ALTER TABLE UploadEmpPositionInfo ADD
		AuthorizationWorkFlowIDOTClaims INT NULL;

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.16a'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.16a';
	
	COMMIT TRANSACTION
END
