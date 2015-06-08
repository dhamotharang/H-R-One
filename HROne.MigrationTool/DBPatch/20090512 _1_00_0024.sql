DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.00.0023' 
BEGIN

CREATE TABLE dbo.Tmp_EmpTermination
(
	EmpTermID int NOT NULL IDENTITY (1, 1),
	EmpID int NULL,
	CessationReasonID int NULL,
	EmpTermResignDate datetime NULL,
	EmpTermNoticePeriod int NULL,
	EmpTermNoticeUnit nvarchar(1) NULL,
	EmpTermLastDate datetime NULL,
	EmpTermRemark ntext NULL,
	EmpTermIsTransferCompany int NULL,
	NewEmpID int NULL
)


SET IDENTITY_INSERT dbo.Tmp_EmpTermination ON
IF EXISTS(SELECT * FROM dbo.EmpTermination)
	 EXEC('INSERT INTO dbo.Tmp_EmpTermination (EmpTermID, EmpID, CessationReasonID, EmpTermResignDate, EmpTermNoticePeriod, EmpTermNoticeUnit, EmpTermLastDate, EmpTermRemark, EmpTermIsTransferCompany, NewEmpID)
		SELECT EmpTermID, EmpID, CessationReasonID, EmpTermResignDate, EmpTermNoticePeriod, EmpTermNoticeUnit, EmpTermLastDate, CONVERT(ntext, EmpTermRemark), EmpTermIsTransferCompany, NewEmpID FROM dbo.EmpTermination WITH (HOLDLOCK TABLOCKX)')
SET IDENTITY_INSERT dbo.Tmp_EmpTermination OFF
DROP TABLE dbo.EmpTermination
EXECUTE sp_rename N'dbo.Tmp_EmpTermination', N'EmpTermination', 'OBJECT' 
ALTER TABLE dbo.EmpTermination ADD CONSTRAINT
	PK__EmpTermination PRIMARY KEY CLUSTERED 
	(
	EmpTermID
	)
		-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.00.0024'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

