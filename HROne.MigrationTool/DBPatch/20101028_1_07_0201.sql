
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0199' 
BEGIN
	
	Create Table EmpWorkInjuryRecord
	(
		EmpWorkInjuryRecordID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpWorkInjuryRecordAccidentDate datetime NULL,
		EmpWorkInjuryRecordAccidentLocation nvarchar(100) NULL,
		EmpWorkInjuryRecordAccidentReason nvarchar(100) NULL,
		EmpWorkInjuryRecordInjuryType nvarchar(50) NULL,
		EmpWorkInjuryRecordReportedDate datetime NULL,
		EmpWorkInjuryRecordChequeReceivedDate datetime NULL,
		EmpWorkInjuryRecordSettleDate datetime NULL,
		EmpWorkInjuryRecordRemark ntext NULL,
		CONSTRAINT PK_EmpWorkInjuryRecord PRIMARY KEY CLUSTERED 
		(
			EmpWorkInjuryRecordID
		)
	)
	
	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('PER017','Employee Work Injury Record', 'Personnel', 0)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0201'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





