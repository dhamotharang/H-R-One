
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.2.562' 
BEGIN

	CREATE TABLE LeaveBalanceEntitle
	(
		LeaveBalanceEntitleID INT NOT NULL IDENTITY (1, 1),
		EmpID INT NULL,
		LeaveTypeID INT NULL,
		LeaveBalanceEntitleEffectiveDate DATETIME NULL,
		LeaveBalanceEntitleDateExpiry DATETIME NULL,
		LeaveBalanceEntitleDays REAL NULL,
		CONSTRAINT PK_LeaveBalanceEntitle PRIMARY KEY CLUSTERED 
		(
			LeaveBalanceEntitleID
		) 
	)
	ALTER TABLE LeavePlanBroughtForward ADD 
		LeavePlanBroughtForwardNumOfMonthExpired INT NULL

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.2.563'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





