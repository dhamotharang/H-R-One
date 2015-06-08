
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.19'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE LeaveType ADD 
		LeaveTypeIsESSAllowableAdvanceBalance decimal(5,2);

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.19a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.19a';
	
	COMMIT TRANSACTION
END

