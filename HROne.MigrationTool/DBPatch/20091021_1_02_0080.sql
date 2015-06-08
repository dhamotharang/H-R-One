DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0079' 
BEGIN

	
	CREATE TABLE Tmp_PaymentCode
		(
		PaymentCodeID int NOT NULL IDENTITY (1, 1),
		PaymentCode nvarchar(20) NULL,
		PaymentCodeDesc nvarchar(100) NULL,
		PaymentTypeID int NOT NULL,
		PaymentCodeIsProrata int NULL,
		PaymentCodeIsProrataLeave int NULL,
		PaymentCodeIsMPF int NULL,
		PaymentCodeIsTopUp int NULL,
		PaymentCodeIsWages int NULL,
		PaymentCodeIsORSO int NULL,
		PaymentCodeDecimalPlace int NULL,
		PaymentCodeRoundingRule nvarchar(50) NULL,
		PaymentCodeHideInPaySlip int NULL,
		PaymentCodeDisplaySeqNo int NULL
		)

	SET IDENTITY_INSERT Tmp_PaymentCode ON

	IF EXISTS(SELECT * FROM PaymentCode)
		 EXEC('INSERT INTO Tmp_PaymentCode (PaymentCodeID, PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata,PaymentCodeIsProrataLeave, PaymentCodeIsMPF, PaymentCodeIsTopUp, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo)
			SELECT PaymentCodeID, PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata,1, PaymentCodeIsMPF, PaymentCodeIsTopUp, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo FROM PaymentCode WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT Tmp_PaymentCode OFF

	DROP TABLE PaymentCode

	EXECUTE sp_rename N'Tmp_PaymentCode', N'PaymentCode', 'OBJECT' 

	ALTER TABLE PaymentCode ADD CONSTRAINT
	PK_PaymentCode PRIMARY KEY CLUSTERED 
	(
		PaymentCodeID
	) 

	Alter Table EmpFinalPayment Add
		LeaveAppIDList NTEXT NULL
	
	Alter Table PaymentRecord Add
		LeaveAppIDList NTEXT NULL
		
	CREATE TABLE Tmp_RosterCode
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
		RosterCodeLunchIsDeductWorkingHour int NULL,
		RosterCodeLunchDeductWorkingHourMinsUnit int NULL,
		RosterCodeLunchDeductWorkingHourMinsRoundingRule nvarchar(50) NULL,
		RosterCodeHasOT int NULL,
		RosterCodeOTStartTime datetime NULL,
		RosterCodeOTEndTime datetime NULL,
		RosterCodeIsOTStartFromOutTime int NULL,
		RosterCodeCutOffTime datetime NULL,
		RosterCodeWorkingDayUnit real NULL,
		RosterCodeDailyWorkingHour real NULL,
		RosterCodeCountWorkHourOnly int NULL,
		RosterCodeCountOTAfterWorkHourMin int NULL,
		RosterCodeOTMinsUnit int NULL,
		RosterCodeOTMinsRoundingRule nvarchar(50) NULL
	)
	SET IDENTITY_INSERT Tmp_RosterCode ON
	IF EXISTS(SELECT * FROM RosterCode)
		 EXEC('INSERT INTO Tmp_RosterCode (RosterCodeID, RosterCode, RosterCodeDesc, RosterCodeType, RosterCodeInTime, RosterCodeOutTime, RosterCodeGraceInTime, RosterCodeGraceOutTime, RosterCodeHasLunch, RosterCodeLunchStartTime, RosterCodeLunchEndTime, RosterCodeLunchIsDeductWorkingHour, RosterCodeLunchDeductWorkingHourMinsUnit, RosterCodeLunchDeductWorkingHourMinsRoundingRule, RosterCodeHasOT, RosterCodeOTStartTime, RosterCodeOTEndTime, RosterCodeIsOTStartFromOutTime, RosterCodeCutOffTime, RosterCodeWorkingDayUnit, RosterCodeDailyWorkingHour, RosterCodeCountWorkHourOnly, RosterCodeCountOTAfterWorkHourMin, RosterCodeOTMinsUnit, RosterCodeOTMinsRoundingRule)
			SELECT RosterCodeID, RosterCode, RosterCodeDesc, RosterCodeType, RosterCodeInTime, RosterCodeOutTime, RosterCodeGraceInTime, RosterCodeGraceOutTime, RosterCodeHasLunch, RosterCodeLunchStartTime, RosterCodeLunchEndTime, 0, 0, '''', RosterCodeHasOT, RosterCodeOTStartTime, RosterCodeOTEndTime, RosterCodeIsOTStartFromOutTime, RosterCodeCutOffTime, RosterCodeWorkingDayUnit, RosterCodeDailyWorkingHour, RosterCodeCountWorkHourOnly, RosterCodeCountOTAfterWorkHourMin, RosterCodeOTMinsUnit, RosterCodeOTMinsRoundingRule FROM RosterCode WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT Tmp_RosterCode OFF
	DROP TABLE RosterCode
	EXECUTE sp_rename N'Tmp_RosterCode', N'RosterCode', 'OBJECT' 
	ALTER TABLE RosterCode ADD CONSTRAINT
		PK_RosterCode PRIMARY KEY CLUSTERED 
		(
			RosterCodeID
		)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0080'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





