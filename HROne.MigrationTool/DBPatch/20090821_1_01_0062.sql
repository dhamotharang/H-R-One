DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0061' 
BEGIN
	
	Drop Table RosterCode
	
	CREATE TABLE RosterCode
	(
		RosterCodeID int NOT NULL IDENTITY (1, 1),
		RosterCode nvarchar(20) NULL,
		RosterCodeDesc nvarchar(100) NULL,
		RosterCodeType nvarchar(1) NULL,
		RosterCodeInTime datetime NULL,
		RosterCodeOutTime datetime NULL,
		RosterCodeGraceInTime datetime NULL,
		RosterCodeGraceOutTime datetime NULL,
		RosterCodeHasLunch int NULL,
		RosterCodeLunchStartTime datetime NULL,
		RosterCodeLunchEndTime datetime NULL,
		RosterCodeHasOT int NULL,
		RosterCodeOTStartTime datetime NULL,
		RosterCodeOTEndTime datetime NULL,
		RosterCodeIsOTStartFromOutTime int NULL,
		RosterCodeCutOffTime datetime NULL,
		RosterCodeWorkingDayUnit real NULL,
		RosterCodeDailyWorkingHour real NULL,
		RosterCodeLateMinsUnit int NULL,
		RosterCodeEarlyLeaveMinsUnit int NULL,
		RosterCodeOTMinsUnit int NULL,
		CONSTRAINT PK_RosterCode PRIMARY KEY CLUSTERED 
		(
			RosterCodeID
		)
	)
	
	ALTER TABLE EmpPersonalInfo ADD
	EmpTimeCardNo nvarchar(20) NULL
	
	CREATE TABLE AttendanceRecord
	(
		AttendanceRecordID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		AttendanceRecordDate datetime NULL,
		RosterCodeID int NULL,
		RosterTableID int NULL,
		AttendanceRecordWorkStart datetime NULL,
		AttendanceRecordWorkEnd datetime NULL,
		AttendanceRecordLunchOut datetime NULL,
		AttendanceRecordLunchIn datetime NULL,
		AttendanceRecordCalculateLateMins int NULL,
		AttendanceRecordActualLateMins int NULL,
		AttendanceRecordCalculateEarlyLeaveMins int NULL,
		AttendanceRecordActualEarlyLeaveMins int NULL,
		AttendanceRecordCalculateOvertimeMins int NULL,
		AttendanceRecordActualOvertimeMins int NULL,
		AttendanceRecordCalculateWorkingDay real NULL,
		AttendanceRecordActualWorkingDay real NULL,
		AttendanceRecordCalculateWorkingHour real NULL,
		AttendanceRecordActualWorkingHour real NULL,
		AttendanceRecordIsAbsent int NULL,
		AttendanceRecordRemark ntext NULL,
		CONSTRAINT PK_AttendanceRecord PRIMARY KEY CLUSTERED 
		(
			AttendanceRecordID
		)
	)
	
	
	CREATE TABLE Tmp_LeaveApplication
	(
		LeaveAppID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		LeaveCodeID int NULL,
		LeaveAppUnit nvarchar(1) NULL,
		LeaveAppDateFrom datetime NULL,
		LeaveAppDateTo datetime NULL,
		LeaveAppTimeFrom datetime NULL,
		LeaveAppTimeTo datetime NULL,
		LeaveAppDays decimal(15, 4) NULL,
		LeaveAppRemark ntext NULL,
		LeaveAppNoPayProcess int NULL,
		EmpPaymentID int NULL,
		EmpPayrollID int NULL
	)
	
	SET IDENTITY_INSERT Tmp_LeaveApplication ON

	IF EXISTS(SELECT * FROM LeaveApplication)
	 EXEC('INSERT INTO Tmp_LeaveApplication (LeaveAppID, EmpID, LeaveCodeID, LeaveAppUnit, LeaveAppDateFrom, LeaveAppDateTo, LeaveAppDays, LeaveAppRemark, LeaveAppNoPayProcess, EmpPaymentID, EmpPayrollID)
		SELECT LeaveAppID, EmpID, LeaveCodeID, LeaveAppUnit, LeaveAppDateFrom, LeaveAppDateTo, LeaveAppDays, LeaveAppRemark, LeaveAppNoPayProcess, EmpPaymentID, EmpPayrollID FROM LeaveApplication WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT Tmp_LeaveApplication OFF
	
	DROP TABLE dbo.LeaveApplication

	EXECUTE sp_rename N'dbo.Tmp_LeaveApplication', N'LeaveApplication', 'OBJECT' 

	ALTER TABLE LeaveApplication ADD CONSTRAINT
		PK_LeaveApplication PRIMARY KEY CLUSTERED 
	(
		LeaveAppID
	) 

	CREATE TABLE dbo.Tmp_RequestLeaveApplication
	(
		RequestLeaveAppID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		RequestLeaveCodeID int NULL,
		RequestLeaveAppUnit nvarchar(50) NULL,
		RequestLeaveAppDateFrom datetime NULL,
		RequestLeaveAppDateTo datetime NULL,
		RequestLeaveAppTimeFrom datetime NULL,
		RequestLeaveAppTimeTo datetime NULL,
		RequestLeaveDays decimal(15, 4) NULL,
		RequestLeaveAppRemark nvarchar(50) NULL,
		RequestLeaveAppCreateDate datetime NULL
	)  
	
	SET IDENTITY_INSERT dbo.Tmp_RequestLeaveApplication ON

	IF EXISTS(SELECT * FROM dbo.RequestLeaveApplication)
		EXEC('INSERT INTO dbo.Tmp_RequestLeaveApplication (RequestLeaveAppID, EmpID, RequestLeaveCodeID, RequestLeaveAppUnit, RequestLeaveAppDateFrom, RequestLeaveAppDateTo, RequestLeaveDays, RequestLeaveAppRemark, RequestLeaveAppCreateDate)
		SELECT RequestLeaveAppID, EmpID, RequestLeaveCodeID, RequestLeaveAppUnit, RequestLeaveAppDateFrom, RequestLeaveAppDateTo, RequestLeaveDays, RequestLeaveAppRemark, RequestLeaveAppCreateDate FROM dbo.RequestLeaveApplication WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT dbo.Tmp_RequestLeaveApplication OFF

	DROP TABLE dbo.RequestLeaveApplication

	EXECUTE sp_rename N'dbo.Tmp_RequestLeaveApplication', N'RequestLeaveApplication', 'OBJECT' 

	ALTER TABLE dbo.RequestLeaveApplication ADD CONSTRAINT
		PK_RequestLeaveApplication PRIMARY KEY CLUSTERED 
		(
			RequestLeaveAppID
		)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0062'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




