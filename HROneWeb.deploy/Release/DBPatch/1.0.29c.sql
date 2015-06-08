
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.29b'
BEGIN

	BEGIN TRANSACTION 

	DELETE FROM SystemFunction WHERE FunctionCode = 'CUSTOM002';
	
	DELETE FROM SystemFunction WHERE FunctionCode = 'CUSTOM003';

	INSERT INTO SystemFunction
		(FunctionCode, Description, FunctionCategory, FunctionIsHidden)
	VALUES
		('CUSTOM003', 'Winson Customization: Customized Attendance Preparation Process', 'Attendance', -1);
		
	DROP TABLE AttendancePreparationProcess;
	DROP TABLE AttendancePreparationImportBatch;
	DROP TABLE EmpAttendancePreparationProcess;
	DROP TABLE UploadAttendancePreparationProcess;
	
	CREATE TABLE AttendancePreparationProcess
	(
		AttendancePreparationProcessID int IDENTITY(1,1) NOT NULL,
		AttendancePreparationProcessMonth DateTime NULL,
		AttendancePreparationProcessDesc nvarchar(255) NULL,
		AttendancePreparationProcessStatus nvarchar(1) NULL,	
		AttendancePreparationProcessPayDate DateTime NULL,
		AttendancePreparationProcessEmpCount int NULL,	
		AttendancePreparationProcessPeriodFr DateTime NULL,
		AttendancePreparationProcessPeriodTo DateTime NULL,	
		RecordCreatedDateTime DateTime NULL,
		RecordCreatedBy int NULL,
		RecordLastModifiedDateTime DateTime NULL,
		RecordLastModifiedBy int NULL,
		CONSTRAINT PK_AttendancePreparationProcess PRIMARY KEY CLUSTERED 
		(
			AttendancePreparationProcessID ASC
		)
	);

	CREATE TABLE AttendancePreparationImportBatch
	(
		AttendancePreparationImportBatchID int IDENTITY(1,1) NOT NULL,
		AttendancePreparationImportBatchDateTime datetime NULL,
		AttendancePreparationImportBatchUploadedBy int NULL,
		AttendancePreparationImportBatchRemark ntext NULL,
		CONSTRAINT PK_AttendancePreparationImportBatch PRIMARY KEY CLUSTERED 
		(
			AttendancePreparationImportBatchID ASC
		)
	);

	CREATE TABLE EmpAttendancePreparationProcess
	(
		EmpAPPID int IDENTITY(1,1) NOT NULL,
		EmpID int NOT NULL,
		AttendancePreparationProcessID int NULL,
		EmpRPID int NOT NULL,
		Day1 nvarchar(10) NULL,	Day2 nvarchar(10) NULL,	Day3 nvarchar(10) NULL,	Day4 nvarchar(10) NULL,	Day5 nvarchar(10) NULL,
		Day6 nvarchar(10) NULL,	Day7 nvarchar(10) NULL,	Day8 nvarchar(10) NULL,	Day9 nvarchar(10) NULL,	Day10 nvarchar(10) NULL,
		Day11 nvarchar(10) NULL,	Day12 nvarchar(10) NULL,	Day13 nvarchar(10) NULL,	Day14 nvarchar(10) NULL,	Day15 nvarchar(10) NULL,
		Day16 nvarchar(10) NULL,	Day17 nvarchar(10) NULL,	Day18 nvarchar(10) NULL,	Day19 nvarchar(10) NULL,	Day20 nvarchar(10) NULL,
		Day21 nvarchar(10) NULL,	Day22 nvarchar(10) NULL,	Day23 nvarchar(10) NULL,	Day24 nvarchar(10) NULL,	Day25 nvarchar(10) NULL,
		Day26 nvarchar(10) NULL,	Day27 nvarchar(10) NULL,	Day28 nvarchar(10) NULL,	Day29 nvarchar(10) NULL,	Day30 nvarchar(10) NULL,
		Day31 nvarchar(10) NULL,
		TotalHours int NOT NULL,
		Remarks nvarchar(255) NULL,
		ReductionOthers decimal(15, 4) NOT NULL,
		ReductionUniformTimecard decimal(15, 4) NOT NULL,
		BackpayAllowance decimal(15, 4) NOT NULL,
		BackpayOthers decimal(15, 4) NOT NULL,
		BackpayUniformTimecard decimal(15, 4) NOT NULL,
		CNDAmount decimal(15, 4) NULL,
		APPImportBatchID int NULL,
		CONSTRAINT PK_EmpAttendancePreparationProcess PRIMARY KEY CLUSTERED 
		(
			EmpAPPID ASC
		)
	);

	CREATE TABLE UploadAttendancePreparationProcess
	(
		UploadAttendancePreparationProcessID int IDENTITY(1,1) NOT NULL,
		EmpAPPID int NULL,
		EmpID int NOT NULL,
		AttendancePreparationProcessID int NULL,
		EmpRPID int NOT NULL,
		Day1 nvarchar(10) NULL, Day2 nvarchar(10) NULL, Day3 nvarchar(10) NULL, Day4 nvarchar(10) NULL, Day5 nvarchar(10) NULL,
		Day6 nvarchar(10) NULL, Day7 nvarchar(10) NULL,	Day8 nvarchar(10) NULL,	Day9 nvarchar(10) NULL,	Day10 nvarchar(10) NULL,
		Day11 nvarchar(10) NULL,Day12 nvarchar(10) NULL,Day13 nvarchar(10) NULL,Day14 nvarchar(10) NULL,Day15 nvarchar(10) NULL,
		Day16 nvarchar(10) NULL,Day17 nvarchar(10) NULL,Day18 nvarchar(10) NULL,Day19 nvarchar(10) NULL,Day20 nvarchar(10) NULL,
		Day21 nvarchar(10) NULL,Day22 nvarchar(10) NULL,Day23 nvarchar(10) NULL,Day24 nvarchar(10) NULL,Day25 nvarchar(10) NULL,
		Day26 nvarchar(10) NULL,Day27 nvarchar(10) NULL,Day28 nvarchar(10) NULL,Day29 nvarchar(10) NULL,Day30 nvarchar(10) NULL,
		Day31 nvarchar(10) NULL,
		TotalHours int NOT NULL,
		Remarks nvarchar(255) NULL,
		ReductionOthers decimal(15, 4) NOT NULL,
		ReductionUniformTimecard decimal(15, 4) NOT NULL,
		BackpayAllowance decimal(15, 4) NOT NULL,
		BackpayOthers decimal(15, 4) NOT NULL,
		BackpayUniformTimecard decimal(15, 4) NOT NULL,
		SessionID nvarchar(200) NOT NULL,
		TransactionDate datetime NOT NULL,
		ImportAction nvarchar(100) NULL,
		ModifiedFieldList ntext NULL,
		CONSTRAINT PK_UploadAttendancePreparationProcess PRIMARY KEY CLUSTERED 
		(
			UploadAttendancePreparationProcessID ASC
		)
	);

	-- 0000166
	DROP TABLE EmpRPWinson;

	CREATE TABLE EmpRPWinson
	(
		EmpRPWinsonID int NOT NULL IDENTITY (1, 1),
		EmpRPID int NOT NULL,
		EmpRPShiftDutyID int NOT NULL,
		EmpRPPayCalFormulaID int NOT NULL,

		CONSTRAINT PK_EmpRPWinson PRIMARY KEY CLUSTERED 
		(
			EmpRPWinsonID
		) 
	);
	
	alter table EmpDependant add EmpDependantMedicalSchemeInsured int;
	alter table EmpDependant add EmpDependantMedicalEffectiveDate datetime;
	alter table EmpDependant add EmpDependantExpiryDate datetime;


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.29c'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.29c';
	
	COMMIT TRANSACTION
END

