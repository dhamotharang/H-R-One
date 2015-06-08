DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0063' 
BEGIN
	
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
		LeaveAppHasMedicalCertificate int NULL,
		LeaveAppNoPayProcess int NULL,
		EmpPaymentID int NULL,
		EmpPayrollID int NULL
	)  
	
	SET IDENTITY_INSERT dbo.Tmp_LeaveApplication ON

	IF EXISTS(SELECT * FROM LeaveApplication)
		EXEC('INSERT INTO Tmp_LeaveApplication (LeaveAppID, EmpID, LeaveCodeID, LeaveAppUnit, LeaveAppDateFrom, LeaveAppDateTo, LeaveAppTimeFrom, LeaveAppTimeTo, LeaveAppDays, LeaveAppRemark, LeaveAppNoPayProcess, EmpPaymentID, EmpPayrollID)
			SELECT LeaveAppID, EmpID, LeaveCodeID, LeaveAppUnit, LeaveAppDateFrom, LeaveAppDateTo, LeaveAppTimeFrom, LeaveAppTimeTo, LeaveAppDays, LeaveAppRemark, LeaveAppNoPayProcess, EmpPaymentID, EmpPayrollID FROM LeaveApplication WITH (HOLDLOCK TABLOCKX)')
	
	SET IDENTITY_INSERT Tmp_LeaveApplication OFF

	DROP TABLE LeaveApplication
	
	EXECUTE sp_rename N'Tmp_LeaveApplication', N'LeaveApplication', 'OBJECT' 

	ALTER TABLE LeaveApplication ADD CONSTRAINT
		PK_LeaveApplication PRIMARY KEY CLUSTERED 
		(
			LeaveAppID
		)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0065'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
	

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




