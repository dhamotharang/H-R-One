DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

--## Start 2013-11-29, Ricky So, Patch newly added LeavePlanBroughtForward
IF @DBVERSION='1.0.0' 
BEGIN

	BEGIN TRANSACTION 
	
	UPDATE	LeavePlanBroughtForward
	SET		LeavePlanBroughtForwardNumOfMonthExpired = 9999
	WHERE	LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly=0


	UPDATE	LeavePlanBroughtForward
	SET		LeavePlanBroughtForwardNumOfMonthExpired = 0
	WHERE	LeavePlanBroughtForwardForfeitLastYearBroughtForwardOnly=1

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.2'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.2';

	COMMIT TRANSACTION
END
