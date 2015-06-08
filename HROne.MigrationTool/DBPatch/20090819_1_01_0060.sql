DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0059' 
BEGIN

	CREATE TABLE AttendanceFormula
	(
		AttendanceFormulaID int NOT NULL IDENTITY (1, 1),
		AttendanceFormulaCode nvarchar(20) NULL,
		AttendanceFormulaDesc nvarchar(100) NULL,
		AttendanceFormulaType nvarchar(1) NULL,
		AttendanceFormulaPayFormID int NULL,
		AttendanceFormulaWorkHourPerDay real NULL,
		AttendanceFormulaFixedRate decimal(15,2) NULL,
		CONSTRAINT	PK_AttendanceFormula PRIMARY KEY CLUSTERED 
		(
			AttendanceFormulaID
		) 
	)
	
	CREATE TABLE AttendancePlan
	(
		AttendancePlanID int NOT NULL IDENTITY (1, 1),
		AttendancePlanCode nvarchar(20) NULL,
		AttendancePlanDesc nvarchar(100) NULL,
		AttendancePlanOTFormula int NULL,
		AttendancePlanLateFormula int NULL,
		CONSTRAINT PK_AttendancePlan PRIMARY KEY CLUSTERED 
		(
			AttendancePlanID
		)
	)
	
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
		RosterCodeLunch int NULL,
		RosterCodeLunchStartTime datetime NULL,
		RosterCodeLunchEndTime datetime NULL,
		RosterCodeOT int NULL,
		RosterCodeOTStartTime datetime NULL,
		RosterCodeOTEndTime datetime NULL,
		RosterCodeCutOffTime datetime NULL,
		RosterCodeWorkingDayUnit real NULL,
		RosterCodeDailyWorkingHour real NULL,
		CONSTRAINT PK_RosterCode PRIMARY KEY CLUSTERED 
		(
			RosterCodeID
		)
	)
	
	CREATE TABLE RosterCodeDetail
	(
		RosterCodeDetailID int NOT NULL IDENTITY (1, 1),
		RosterCodeID int NULL,
		RosterCodeDetailNoOfHour real NULL,
		RosterCodeDetailRate real NULL,
		CONSTRAINT PK_RosterCodeDetail PRIMARY KEY CLUSTERED 
		(
			RosterCodeDetailID
		)
	)
	
	
	CREATE TABLE RosterTable
	(
		RosterTableID int NOT NULL,
		EmpID int NULL,
		RosterTableDate datetime NULL,
		RosterCodeID int NULL,
		CONSTRAINT PK_RosterTable PRIMARY KEY CLUSTERED 
		(
			RosterTableID
		)
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0060'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




