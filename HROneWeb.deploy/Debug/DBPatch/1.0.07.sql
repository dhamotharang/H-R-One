DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.2' 
BEGIN
	BEGIN TRANSACTION 
	
	ALTER TABLE LeaveType
		ADD LeaveTypeIsESSIgnoreEntitlement int

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.7'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.7';
	
	COMMIT TRANSACTION
END