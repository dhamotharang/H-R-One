DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.17'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE CompensationLeaveEntitle 
		ADD CompensationLeaveEntitleIsOTClaim bit default 0;
	
	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('ENABLE_OTCLAIM', 'Enable/Disable ECL Requisition', 'N');

	DELETE SystemParameter
		WHERE  ParameterCode = 'ESS_ENABLE_OTCLAIM';
	
	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('ENABLE_MONTHLY_ATTENDANCE_REPORT', 'Enable/Disable Customized Monthly Attendance Report', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('ENABLE_ATTENDANCE_TIMEENTRY_LIST', 'Enable/Disable Customized Attendance Time Entry List', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('ENABLE_TIMECARD_RECORD', 'Enable/Disable Customized Customized Timecard Record', 'N');
	
	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST', 'Show Customized Attendance Time Entry List in ESS', 'Y');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('PARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD', 'Show Customized Customized Timecard Record in ESS', 'Y');
	

	INSERT INTO [SystemFunction]
           ([FunctionCode]
           ,[Description]
           ,[FunctionCategory]
           ,[FunctionIsHidden])
		VALUES
           ('PER001-1','Update Employee Number','Personnel', 0);
           
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.17a'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.17a';
	
	COMMIT TRANSACTION
END

 