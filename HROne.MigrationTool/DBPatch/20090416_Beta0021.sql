
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.00.0020' 
Begin
	-- Change field type for Leave Application Remark
	CREATE TABLE dbo.Tmp_LeaveApplication
	(
		LeaveAppID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		LeaveCodeID int NULL,
		LeaveAppUnit nvarchar(1) NULL,
		LeaveAppDateFrom datetime NULL,
		LeaveAppDateTo datetime NULL,
		LeaveAppTimeFrom int NULL,
		LeaveAppTimeTo int NULL,
		LeaveAppDays decimal(15, 4) NULL,
		LeaveAppRemark ntext NULL,
		LeaveAppNoPayProcess int NULL,
		EmpPaymentID int NULL,
		EmpPayrollID int NULL
	)
	SET IDENTITY_INSERT dbo.Tmp_LeaveApplication ON
	IF EXISTS(SELECT * FROM dbo.LeaveApplication)
	 	EXEC('INSERT INTO dbo.Tmp_LeaveApplication (LeaveAppID, EmpID, LeaveCodeID, LeaveAppUnit, LeaveAppDateFrom, LeaveAppDateTo, LeaveAppTimeFrom, LeaveAppTimeTo, LeaveAppDays, LeaveAppRemark, LeaveAppNoPayProcess, EmpPaymentID, EmpPayrollID)
			SELECT LeaveAppID, EmpID, LeaveCodeID, LeaveAppUnit, LeaveAppDateFrom, LeaveAppDateTo, LeaveAppTimeFrom, LeaveAppTimeTo, LeaveAppDays, CONVERT(ntext, LeaveAppRemark), LeaveAppNoPayProcess, EmpPaymentID, EmpPayrollID FROM dbo.LeaveApplication WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT dbo.Tmp_LeaveApplication OFF

	DROP TABLE dbo.LeaveApplication

	EXECUTE sp_rename N'dbo.Tmp_LeaveApplication', N'LeaveApplication', 'OBJECT' 

	ALTER TABLE dbo.LeaveApplication ADD CONSTRAINT
	PK_LeaveApplication PRIMARY KEY CLUSTERED 
	(
		LeaveAppID
	) 

	CREATE TABLE dbo.Tmp_Users
	(
		UserID int NOT NULL IDENTITY (1, 1),
		LoginID nvarchar(20) NULL,
		UserName nvarchar(100) NULL,
		UserPassword nvarchar(255) NULL,
		UserAccountStatus nvarchar(1) NULL,
		ExpiryDate datetime NULL,
		UserChangePassword int NULL,
		UserChangePasswordUnit nvarchar(1) NULL,
		UserChangePasswordPeriod int NULL,
		UserChangePasswordDate datetime NULL,
		FailCount int NULL
	)

	SET IDENTITY_INSERT dbo.Tmp_Users ON

	IF EXISTS(SELECT * FROM dbo.Users)
	 	EXEC('INSERT INTO dbo.Tmp_Users (UserID, LoginID, UserName, UserPassword, UserAccountStatus, ExpiryDate, UserChangePassword, UserChangePasswordUnit, UserChangePasswordPeriod, UserChangePasswordDate, FailCount)
			SELECT UserID, LoginID, UserName, UserPassword, UserAccountStatus, ExpiryDate, UserChangePassword, UserChangePasswordUnit, UserChangePasswordPeriod, UserChangePasswordDate, FailCount FROM dbo.Users WITH (HOLDLOCK TABLOCKX)')
	
	SET IDENTITY_INSERT dbo.Tmp_Users OFF

	DROP TABLE dbo.Users

	EXECUTE sp_rename N'dbo.Tmp_Users', N'Users', 'OBJECT' 

	ALTER TABLE dbo.Users ADD CONSTRAINT
	PK_User PRIMARY KEY CLUSTERED 
	(
		UserID
	)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.00.0021'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

