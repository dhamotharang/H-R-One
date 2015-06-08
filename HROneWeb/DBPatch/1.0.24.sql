
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.23a'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE EmpPositionInfo ADD
		AuthorizationWorkFlowIDLateWaive INT NULL;
	ALTER TABLE UploadEmpPositionInfo ADD
		AuthorizationWorkFlowIDLateWaive INT NULL;

	CREATE TABLE RequestLateWaive
	(
		RequestLateWaiveID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		AttendanceRecordID int NULL,
		RequestLateWaiveReason nvarchar(200) NULL,
		RequestLateWaiveCreateDate datetime NULL,
		CONSTRAINT PK_RequestLateWaive PRIMARY KEY CLUSTERED 
		(
			RequestLateWaiveID
		)
	)

	CREATE TABLE LateWaive
	(
		LateWaiveID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		AttendanceRecordID int NULL,
		LateWaiveReason nvarchar(200) NULL,
		RecordCreatedDateTime datetime NULL,
		RecordCreatedBy int NULL,
		RecordLastModifiedDateTime datetime NULL,
		RecordLastModifiedBy int NULL,
		LateWaiveCancelID int NULL,
		EmpRequestID int NULL,
		RequestLateWaiveID int NULL,
		SynID nvarchar(255) NULL,
		CONSTRAINT PK_LateWaive PRIMARY KEY CLUSTERED 
		(
			LateWaiveID
		)
	)

	CREATE TABLE RequestLateWaiveCancel
	(
		RequestLateWaiveCancelID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		LateWaiveID int NULL,
		RequestLateWaiveCancelCreateDateTime datetime NULL,
		RequestLateWaiveCancelReason ntext NULL,
		CONSTRAINT PK_RequestLateWaiveCancel PRIMARY KEY CLUSTERED 
		(
			RequestLateWaiveCancelID
		)
	)
	
	UPDATE SystemFunction
	SET    Description = 'Monthly Achievement'
	WHERE  FunctionCode = 'PAY018'
	
	UPDATE SystemFunction
	SET    Description = 'Import Compensation Leave Entitlement'
	WHERE  FunctionCode = 'LEV006'


	INSERT INTO SystemParameter
	VALUES ('ENABLE_LATE_WAIVE', 'Enable/Disable Late Waive', 'N');	
		
	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.24'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.24';
	
	COMMIT TRANSACTION
END

