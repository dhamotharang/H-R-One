
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.25b'
BEGIN

	BEGIN TRANSACTION 

	CREATE TABLE HitRateProcess(
		HitRateProcessID int IDENTITY(1,1) NOT NULL,
		EmpID int NULL,
		RpID int NULL,
		payCodeID int NULL,
		HitRate decimal(5, 2) NULL,
		HitRateProcessImportBatchID int NULL,
	 CONSTRAINT PK_HitRateProcess PRIMARY KEY CLUSTERED 
	(
		HitRateProcessID ASC
	))

	CREATE TABLE HitRateProcessImportBatch(
		HitRateProcessImportBatchID int IDENTITY(1,1) NOT NULL,
		HitRateProcessImportBatchDateTime datetime NULL,
		HitRateProcessImportBatchUploadedBy int NULL,
		HitRateProcessImportBatchRemark ntext NULL,
	 CONSTRAINT PK_HitRateProcessImportBatch PRIMARY KEY CLUSTERED 
	(
		HitRateProcessImportBatchID ASC
	))

	CREATE TABLE UploadHitRateProcess(
		UploadHitRateProcessID int IDENTITY(1,1) NOT NULL,
		EmpID int NOT NULL,
		payCodeID int NULL,
		HitRate decimal(5, 2) NOT NULL,
		SessionID nvarchar(200) NOT NULL,
		TransactionDate datetime NOT NULL,
		ImportAction nvarchar(100) NULL,
		ModifiedFieldList ntext NULL,
	 CONSTRAINT PK_UploadHitRateProcess PRIMARY KEY CLUSTERED 
	(
		UploadHitRateProcessID ASC
	))


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.25c'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.25c';
	
	COMMIT TRANSACTION
END

