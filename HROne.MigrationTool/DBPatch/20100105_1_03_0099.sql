DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0095' 
BEGIN

	CREATE TABLE LeavePlanBroughtForward
	(
		LeavePlanBroughtForwardID int NOT NULL IDENTITY(1,1),
		LeavePlanID int NULL,
		LeaveTypeID int NULL,
		LeavePlanBroughtForwardMax int NULL
		CONSTRAINT PK_LeavePlanBroughtForward PRIMARY KEY CLUSTERED 
		(
			LeavePlanBroughtForwardID
		) 
	)
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0099'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





