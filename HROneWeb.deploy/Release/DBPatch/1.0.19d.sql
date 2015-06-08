
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.19c'
BEGIN
	BEGIN TRANSACTION 

	UPDATE EmpRequest
	SET    EmpRequest.EmpRequestDuration = CASE WHEN RequestLeaveAppHours > 1 THEN 
							CAST(RequestLeaveAppHours AS nvarchar) + ' hours'
						 ELSE
							CAST(RequestLeaveAppHours AS nvarchar) + ' hour'
						 END 
	FROM   RequestLeaveApplication
	WHERE  RequestLeaveApplication.RequestLeaveAppUnit = 'H' AND 
		   EmpRequest.EmpRequestType='EELEAVEAPP' AND
		   EmpRequest.EmpRequestRecordID = RequestLeaveApplication.RequestLeaveAppID

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.19d'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.19d';
	
	COMMIT TRANSACTION
END

