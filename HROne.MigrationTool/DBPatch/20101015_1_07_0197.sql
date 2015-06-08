
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0196' 
BEGIN

	CREATE TABLE ClaimsAndDeductionsImportBatch
	(
		CNDImportBatchID int NOT NULL IDENTITY (1, 1),
		CNDImportBatchDateTime datetime NULL,
		CNDImportBatchUploadedBy int NULL,
		CNDImportBatchRemark ntext NULL
		CONSTRAINT PK_ClaimsAndDeductionImportBatch PRIMARY KEY CLUSTERED 
		(
			CNDImportBatchID
		)
	)

	CREATE TABLE Tmp_TimeCardRecord
	(
		TimeCardRecordID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		TimeCardRecordCardNo nvarchar(20) NULL,
		TimeCardRecordDateTime datetime NULL,
		TimeCardRecordLocation nvarchar(50) NULL,
		TimeCardRecordInOutIndex int NULL,
		TimeCardRecordOriginalData nvarchar(450) NULL
	)
	SET IDENTITY_INSERT Tmp_TimeCardRecord ON
	IF EXISTS(SELECT * FROM TimeCardRecord)
		 EXEC('INSERT INTO Tmp_TimeCardRecord (TimeCardRecordID, EmpID, TimeCardRecordCardNo, TimeCardRecordDateTime, TimeCardRecordLocation, TimeCardRecordInOutIndex, TimeCardRecordOriginalData)
			SELECT TimeCardRecordID, EmpID, TimeCardRecordCardNo, TimeCardRecordDateTime, TimeCardRecordLocation, TimeCardRecordInOutIndex, CONVERT(nvarchar(450), TimeCardRecordOriginalData) FROM TimeCardRecord WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_TimeCardRecord OFF
	DROP TABLE TimeCardRecord
	EXECUTE sp_rename N'Tmp_TimeCardRecord', N'TimeCardRecord', 'OBJECT' 

	ALTER TABLE TimeCardRecord ADD CONSTRAINT
		PK_TimeCardRecord PRIMARY KEY CLUSTERED 
	(
		TimeCardRecordID
	)
	CREATE NONCLUSTERED INDEX IX_TimeCardRecord_OriginalData ON TimeCardRecord
	(
		TimeCardRecordOriginalData
	)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0197'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





