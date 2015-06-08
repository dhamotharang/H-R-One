DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.14'
BEGIN
	BEGIN TRANSACTION 

	UPDATE LeaveCode
	SET    LeaveAppUnit = 'Day,A.M.,P.M.'
	WHERE  LeaveTypeID NOT IN (SELECT LeaveTypeID FROM LeaveType WHERE LeaveType='COMPENSATION'); 

	UPDATE LeaveCode
	SET    LeaveAppUnit = 'Day,A.M.,P.M.,Hour'
	WHERE  LeaveTypeID IN (SELECT LeaveTypeID FROM LeaveType WHERE LeaveType='COMPENSATION'); 
		
	COMMIT TRANSACTION
END