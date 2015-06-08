DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0094' 
BEGIN

	CREATE TABLE Tmp_EmpPersonalInfo
	(
		EmpID int NOT NULL IDENTITY (1, 1),
		EmpNo nvarchar(255) NULL,
		EmpStatus nvarchar(3) NULL,
		EmpEngSurname nvarchar(255) NULL,
		EmpEngOtherName nvarchar(255) NULL,
		EmpChiFullName nvarchar(255) NULL,
		EmpAlias nvarchar(255) NULL,
		EmpHKID nvarchar(50) NULL,
		EmpGender nvarchar(100) NULL,
		EmpMaritalStatus nvarchar(100) NULL,
		EmpDateOfBirth datetime NULL,
		EmpPlaceOfBirth nvarchar(255) NULL,
		EmpNationality nvarchar(255) NULL,
		EmpPassportNo nvarchar(255) NULL,
		EmpPassportIssuedCountry nvarchar(255) NULL,
		EmpPassportExpiryDate datetime NULL,
		EmpResAddr nvarchar(250) NULL,
		EmpResAddrAreaCode nvarchar(100) NULL,
		EmpCorAddr nvarchar(250) NULL,
		EmpDateOfJoin datetime NULL,
		EmpServiceDate datetime NULL,
		EmpNoticePeriod int NULL,
		EmpNoticeUnit nvarchar(1) NULL,
		EmpProbaPeriod int NULL,
		EmpProbaUnit nvarchar(1) NULL,
		EmpProbaLastDate datetime NULL,
		EmpEmail nvarchar(255) NULL,
		EmpHomePhoneNo nvarchar(255) NULL,
		EmpMobileNo nvarchar(255) NULL,
		EmpOfficePhoneNo nvarchar(255) NULL,
		Remark ntext NULL,
		PreviousEmpID int NULL,
		EmpPW nvarchar(255) NULL,
		EmpInternalEmail nvarchar(255) NULL,
		EmpTimeCardNo nvarchar(255) NULL
	)  
	SET IDENTITY_INSERT Tmp_EmpPersonalInfo ON
	IF EXISTS(SELECT * FROM EmpPersonalInfo)
		 EXEC('INSERT INTO Tmp_EmpPersonalInfo (EmpID, EmpNo, EmpStatus, EmpEngSurname, EmpEngOtherName, EmpChiFullName, EmpAlias, EmpHKID, EmpGender, EmpMaritalStatus, EmpDateOfBirth, EmpPlaceOfBirth, EmpNationality, EmpPassportNo, EmpPassportIssuedCountry, EmpPassportExpiryDate, EmpResAddr, EmpResAddrAreaCode, EmpCorAddr, EmpDateOfJoin, EmpServiceDate, EmpNoticePeriod, EmpNoticeUnit, EmpProbaPeriod, EmpProbaUnit, EmpProbaLastDate, EmpEmail, EmpHomePhoneNo, EmpMobileNo, EmpOfficePhoneNo, Remark, PreviousEmpID, EmpPW, EmpInternalEmail, EmpTimeCardNo)
			SELECT EmpID, EmpNo, EmpStatus, EmpEngSurname, EmpEngOtherName, EmpChiFullName, EmpAlias, EmpHKID, EmpGender, EmpMaritalStatus, EmpDateOfBirth, EmpPlaceOfBirth, EmpNationality, EmpPassportNo, EmpPassportIssuedCountry, EmpPassportExpiryDate, EmpResAddr, EmpResAddrAreaCode, EmpCorAddr, EmpDateOfJoin, EmpServiceDate, EmpNoticePeriod, EmpNoticeUnit, EmpProbaPeriod, EmpProbaUnit, EmpProbaLastDate, EmpEmail, EmpHomePhoneNo, EmpMobileNo, EmpOfficePhoneNo, Remark, PreviousEmpID, EmpPW, EmpInternalEmail, EmpTimeCardNo FROM EmpPersonalInfo WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_EmpPersonalInfo OFF
	DROP TABLE EmpPersonalInfo
	EXECUTE sp_rename N'Tmp_EmpPersonalInfo', N'EmpPersonalInfo', 'OBJECT' 
	ALTER TABLE EmpPersonalInfo ADD CONSTRAINT
		PK__EmpPersonalInfo PRIMARY KEY CLUSTERED 
		(
		EmpID
		)           
 
 

	CREATE TABLE Tmp_EmpDependant
	(
		EmpDependantID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpDependantSurname nvarchar(255) NULL,
		EmpDependantOtherName nvarchar(255) NULL,
		EmpDependantChineseName nvarchar(255) NULL,
		EmpDependantGender nvarchar(100) NULL,
		EmpDependantHKID nvarchar(255) NULL,
		EmpDependantPassportNo nvarchar(255) NULL,
		EmpDependantPassportIssuedCountry nvarchar(255) NULL,
		EmpDependantRelationship nvarchar(255) NULL,
		EmpDependantDateOfBirth datetime NULL,
		SynID nvarchar(255) NULL
	)
	
	SET IDENTITY_INSERT Tmp_EmpDependant ON
	IF EXISTS(SELECT * FROM EmpDependant)
		 EXEC('INSERT INTO Tmp_EmpDependant (EmpDependantID, EmpID, EmpDependantSurname, EmpDependantOtherName, EmpDependantChineseName, EmpDependantGender, EmpDependantHKID, EmpDependantPassportNo, EmpDependantPassportIssuedCountry, EmpDependantRelationship, EmpDependantDateOfBirth)
			SELECT EmpDependantID, EmpID, EmpDependantSurname, EmpDependantOtherName, EmpDependantChineseName, EmpDependantGender, EmpDependantHKID, EmpDependantPassportNo, EmpDependantPassportIssuedCountry, EmpDependantRelationship, EmpDependantDateOfBirth FROM EmpDependant WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_EmpDependant OFF
	DROP TABLE EmpDependant
	EXECUTE sp_rename N'Tmp_EmpDependant', N'EmpDependant', 'OBJECT' 
	ALTER TABLE EmpDependant ADD CONSTRAINT
		PK_EmpDependant PRIMARY KEY CLUSTERED 
		(
			EmpDependantID
		)

	CREATE TABLE Tmp_Company
	(
		CompanyID int NOT NULL IDENTITY (1, 1),
		CompanyCode nvarchar(200) NULL,
		CompanyName nvarchar(255) NULL,
		CompanyAddress nvarchar(255) NULL,
		CompanyContactPerson nvarchar(255) NULL,
		CompanyContactNo nvarchar(255) NULL,
		CompanyFaxNo nvarchar(255) NULL,
		CompanyBRNo nvarchar(255) NULL,
		CompanyBankCode nvarchar(30) NULL,
		CompanyBranchCode nvarchar(30) NULL,
		CompanyBankAccountNo nvarchar(90) NULL,
		CompanyBankHolderName nvarchar(255) NULL
	)
	SET IDENTITY_INSERT Tmp_Company ON
	IF EXISTS(SELECT * FROM Company)
		 EXEC('INSERT INTO Tmp_Company (CompanyID, CompanyCode, CompanyName, CompanyAddress, CompanyContactPerson, CompanyContactNo, CompanyFaxNo, CompanyBRNo, CompanyBankAccountNo, CompanyBankCode, CompanyBranchCode, CompanyBankHolderName)
			SELECT CompanyID, CompanyCode, CompanyName, CompanyAddress, CompanyContactPerson, CompanyContactNo, CompanyFaxNo, CompanyBRNo, CompanyBankAccountNo, CompanyBankCode, CompanyBranchCode, CompanyBankHolderName FROM Company WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_Company OFF
	DROP TABLE Company
	EXECUTE sp_rename N'Tmp_Company', N'Company', 'OBJECT' 
	ALTER TABLE Company ADD CONSTRAINT
		PK_Company_ PRIMARY KEY CLUSTERED 
		(
			CompanyID
		)

	CREATE TABLE Tmp_Position
	(
		PositionID int NOT NULL IDENTITY (1, 1),
		PositionCode nvarchar(200) NULL,
		PositionDesc nvarchar(255) NULL,
		PositionCapacity nvarchar(255) NULL
	)
	SET IDENTITY_INSERT Tmp_Position ON
	IF EXISTS(SELECT * FROM Position)
		 EXEC('INSERT INTO Tmp_Position (PositionID, PositionCode, PositionDesc, PositionCapacity)
			SELECT PositionID, PositionCode, PositionDesc, PositionCapacity FROM Position WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_Position OFF
	DROP TABLE Position
	EXECUTE sp_rename N'Tmp_Position', N'Position', 'OBJECT' 
	ALTER TABLE Position ADD CONSTRAINT
		PK_Position PRIMARY KEY CLUSTERED 
		(
			PositionID
		)
		
	CREATE TABLE Tmp_Rank
	(
		RankID int NOT NULL IDENTITY (1, 1),
		RankCode nvarchar(200) NULL,
		RankDesc nvarchar(255) NULL
	)
	SET IDENTITY_INSERT Tmp_Rank ON
	IF EXISTS(SELECT * FROM Rank)
		 EXEC('INSERT INTO Tmp_Rank (RankID, RankCode, RankDesc)
			SELECT RankID, RankCode, RankDesc FROM Rank WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_Rank OFF
	DROP TABLE Rank
	EXECUTE sp_rename N'Tmp_Rank', N'Rank', 'OBJECT' 
	ALTER TABLE Rank ADD CONSTRAINT
		PK_Rank PRIMARY KEY CLUSTERED 
		(
			RankID
		) 

	CREATE TABLE Tmp_StaffType
	(
		StaffTypeID int NOT NULL IDENTITY (1, 1),
		StaffTypeCode nvarchar(200) NULL,
		StaffTypeDesc nvarchar(255) NULL
	)
	SET IDENTITY_INSERT Tmp_StaffType ON
	IF EXISTS(SELECT * FROM StaffType)
		 EXEC('INSERT INTO Tmp_StaffType (StaffTypeID, StaffTypeCode, StaffTypeDesc)
			SELECT StaffTypeID, StaffTypeCode, StaffTypeDesc FROM StaffType WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_StaffType OFF
	DROP TABLE StaffType
	EXECUTE sp_rename N'Tmp_StaffType', N'StaffType', 'OBJECT' 
	ALTER TABLE StaffType ADD CONSTRAINT
		PK_StaffType PRIMARY KEY CLUSTERED 
		(
			StaffTypeID
		) 
	CREATE TABLE Tmp_PayrollGroup
	(
		PayGroupID int NOT NULL IDENTITY (1, 1),
		PayGroupCode nvarchar(200) NULL,
		PayGroupDesc nvarchar(255) NULL,
		PayGroupFreq nvarchar(1) NULL,
		PayGroupDefaultStartDay int NULL,
		PayGroupDefaultEndDay int NULL,
		PayGroupNextStartDate datetime NULL,
		PayGroupNextEndDate datetime NULL,
		CurrentPayPeriodID int NULL,
		PayGroupLeaveDeductFormula int NULL,
		PayGroupLeaveAllowFormula int NULL,
		PayGroupNewJoinFormula int NULL,
		PayGroupTerminatedFormula int NULL,
		PayGroupTerminatedALCompensationDailyFormula int NULL,
		PayGroupTerminatedALCompensationPaymentCodeID int NULL,
		PayGroupTerminatedPaymentInLieuMonthlyBaseMethod nvarchar(100) NULL,
		PayGroupTerminatedPaymentInLieuDailyFormula int NULL,
		PayGroupTerminatedPaymentInLieuERPaymentCodeID int NULL,
		PayGroupTerminatedPaymentInLieuEEPaymentCodeID int NULL,
		PayGroupTerminatedLSPSPMonthlyBaseMethod nvarchar(100) NULL,
		PayGroupTerminatedLSPPaymentCodeID int NULL,
		PayGroupTerminatedSPPaymentCodeID int NULL,
		PayGroupStatHolDeductFormula int NULL,
		PayGroupStatHolAllowFormula int NULL,
		PayGroupStatHolAllowPaymentCodeID int NULL,
		PayGroupStatHolDeductPaymentCodeID int NULL,
		PayGroupPayAdvance int NULL,
		PayGroupStatHolEligiblePeriod int NULL,
		PayGroupStatHolEligibleUnit nvarchar(1) NULL,
		PayGroupStatHolEligibleAfterProbation int NULL,
		PayGroupYEBStartPayrollMonth int NULL,
		PayGroupYEBMonthFrom int NULL,
		PayGroupYEBMonthTo int NULL
	)  
	SET IDENTITY_INSERT Tmp_PayrollGroup ON
	IF EXISTS(SELECT * FROM PayrollGroup)
		 EXEC('INSERT INTO Tmp_PayrollGroup (PayGroupID, PayGroupCode, PayGroupDesc, PayGroupFreq, PayGroupDefaultStartDay, PayGroupDefaultEndDay, PayGroupNextStartDate, PayGroupNextEndDate, CurrentPayPeriodID, PayGroupLeaveDeductFormula, PayGroupLeaveAllowFormula, PayGroupNewJoinFormula, PayGroupTerminatedFormula, PayGroupTerminatedALCompensationDailyFormula, PayGroupTerminatedALCompensationPaymentCodeID, PayGroupTerminatedPaymentInLieuMonthlyBaseMethod, PayGroupTerminatedPaymentInLieuDailyFormula, PayGroupTerminatedPaymentInLieuERPaymentCodeID, PayGroupTerminatedPaymentInLieuEEPaymentCodeID, PayGroupTerminatedLSPSPMonthlyBaseMethod, PayGroupTerminatedLSPPaymentCodeID, PayGroupTerminatedSPPaymentCodeID, PayGroupStatHolDeductFormula, PayGroupStatHolAllowFormula, PayGroupStatHolAllowPaymentCodeID, PayGroupStatHolDeductPaymentCodeID, PayGroupPayAdvance, PayGroupStatHolEligiblePeriod, PayGroupStatHolEligibleUnit, PayGroupStatHolEligibleAfterProbation, PayGroupYEBStartPayrollMonth, PayGroupYEBMonthFrom, PayGroupYEBMonthTo)
			SELECT PayGroupID, PayGroupCode, PayGroupDesc, PayGroupFreq, PayGroupDefaultStartDay, PayGroupDefaultEndDay, PayGroupNextStartDate, PayGroupNextEndDate, CurrentPayPeriodID, PayGroupLeaveDeductFormula, PayGroupLeaveAllowFormula, PayGroupNewJoinFormula, PayGroupTerminatedFormula, PayGroupTerminatedALCompensationDailyFormula, PayGroupTerminatedALCompensationPaymentCodeID, PayGroupTerminatedPaymentInLieuMonthlyBaseMethod, PayGroupTerminatedPaymentInLieuDailyFormula, PayGroupTerminatedPaymentInLieuERPaymentCodeID, PayGroupTerminatedPaymentInLieuEEPaymentCodeID, PayGroupTerminatedLSPSPMonthlyBaseMethod, PayGroupTerminatedLSPPaymentCodeID, PayGroupTerminatedSPPaymentCodeID, PayGroupStatHolDeductFormula, PayGroupStatHolAllowFormula, PayGroupStatHolAllowPaymentCodeID, PayGroupStatHolDeductPaymentCodeID, PayGroupPayAdvance, PayGroupStatHolEligiblePeriod, PayGroupStatHolEligibleUnit, PayGroupStatHolEligibleAfterProbation, PayGroupYEBStartPayrollMonth, PayGroupYEBMonthFrom, PayGroupYEBMonthTo FROM PayrollGroup WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_PayrollGroup OFF
	DROP TABLE PayrollGroup
	EXECUTE sp_rename N'Tmp_PayrollGroup', N'PayrollGroup', 'OBJECT' 
	ALTER TABLE PayrollGroup ADD CONSTRAINT
		PK_PayrollGroup PRIMARY KEY CLUSTERED 
		(
			PayGroupID
		)

	CREATE TABLE Tmp_LeavePlan
	(
		LeavePlanID int NOT NULL IDENTITY (1, 1),
		LeavePlanCode nvarchar(200) NULL,
		LeavePlanDesc nvarchar(255) NULL,
		LeavePlanALMaxBF int NULL,
		LeavePlanSL1MaxBF int NULL,
		LeavePlanSL2MaxBF int NULL
	)
	SET IDENTITY_INSERT Tmp_LeavePlan ON
	IF EXISTS(SELECT * FROM LeavePlan)
		 EXEC('INSERT INTO Tmp_LeavePlan (LeavePlanID, LeavePlanCode, LeavePlanDesc, LeavePlanALMaxBF, LeavePlanSL1MaxBF, LeavePlanSL2MaxBF)
			SELECT LeavePlanID, LeavePlanCode, LeavePlanDesc, LeavePlanALMaxBF, LeavePlanSL1MaxBF, LeavePlanSL2MaxBF FROM LeavePlan WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_LeavePlan OFF
	DROP TABLE LeavePlan
	EXECUTE sp_rename N'Tmp_LeavePlan', N'LeavePlan', 'OBJECT' 
	ALTER TABLE LeavePlan ADD CONSTRAINT
		PK_LeavePlan PRIMARY KEY CLUSTERED 
		(
			LeavePlanID
		) 
	
	CREATE TABLE Tmp_HierarchyElement
	(
		HElementID int NOT NULL IDENTITY (1, 1),
		CompanyID int NULL,
		HElementCode nvarchar(200) NULL,
		HElementDesc nvarchar(255) NULL,
		HLevelID int NULL
	)
	SET IDENTITY_INSERT Tmp_HierarchyElement ON
	IF EXISTS(SELECT * FROM HierarchyElement)
		 EXEC('INSERT INTO Tmp_HierarchyElement (HElementID, CompanyID, HElementCode, HElementDesc, HLevelID)
			SELECT HElementID, CompanyID, HElementCode, HElementDesc, HLevelID FROM HierarchyElement WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_HierarchyElement OFF
	DROP TABLE HierarchyElement
	EXECUTE sp_rename N'Tmp_HierarchyElement', N'HierarchyElement', 'OBJECT' 
	ALTER TABLE HierarchyElement ADD CONSTRAINT
		PK_HierarchyElement PRIMARY KEY CLUSTERED 
		(
			HElementID
		) 
	
	CREATE TABLE Tmp_CostCenter
	(
		CostCenterID int NOT NULL IDENTITY (1, 1),
		CostCenterCode nvarchar(200) NOT NULL,
		CostCenterDesc nvarchar(255) NULL
	)
	SET IDENTITY_INSERT Tmp_CostCenter ON
	IF EXISTS(SELECT * FROM CostCenter)
		 EXEC('INSERT INTO Tmp_CostCenter (CostCenterID, CostCenterCode, CostCenterDesc)
			SELECT CostCenterID, CostCenterCode, CostCenterDesc FROM CostCenter WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_CostCenter OFF
	DROP TABLE CostCenter
	EXECUTE sp_rename N'Tmp_CostCenter', N'CostCenter', 'OBJECT' 
	ALTER TABLE CostCenter ADD CONSTRAINT
		PK_CostCenter PRIMARY KEY CLUSTERED 
		(
			CostCenterID
		)

	CREATE TABLE Tmp_CessationReason
	(
		CessationReasonID int NOT NULL IDENTITY (1, 1),
		CessationReasonCode nvarchar(200) NULL,
		CessationReasonDesc nvarchar(255) NULL,
		CessationReasonIsSeverancePay int NULL,
		CessationReasonIsLongServicePay int NULL,
		CessationReasonHasProrataYEB int NULL
	)
	SET IDENTITY_INSERT Tmp_CessationReason ON
	IF EXISTS(SELECT * FROM CessationReason)
		 EXEC('INSERT INTO Tmp_CessationReason (CessationReasonID, CessationReasonCode, CessationReasonDesc, CessationReasonIsSeverancePay, CessationReasonIsLongServicePay, CessationReasonHasProrataYEB)
			SELECT CessationReasonID, CessationReasonCode, CessationReasonDesc, CessationReasonIsSeverancePay, CessationReasonIsLongServicePay, CessationReasonHasProrataYEB FROM CessationReason WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_CessationReason OFF
	DROP TABLE CessationReason
	EXECUTE sp_rename N'Tmp_CessationReason', N'CessationReason', 'OBJECT' 
	ALTER TABLE CessationReason ADD CONSTRAINT
		PK_CessationReason PRIMARY KEY CLUSTERED 
		(
			CessationReasonID
		)
		
	CREATE TABLE Tmp_PaymentCode
	(
		PaymentCodeID int NOT NULL IDENTITY (1, 1),
		PaymentCode nvarchar(200) NULL,
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
		 EXEC('INSERT INTO Tmp_PaymentCode (PaymentCodeID, PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata, PaymentCodeIsProrataLeave, PaymentCodeIsMPF, PaymentCodeIsTopUp, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo)
			SELECT PaymentCodeID, PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata, PaymentCodeIsProrataLeave, PaymentCodeIsMPF, PaymentCodeIsTopUp, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo FROM PaymentCode WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_PaymentCode OFF
	DROP TABLE PaymentCode
	EXECUTE sp_rename N'Tmp_PaymentCode', N'PaymentCode', 'OBJECT' 
	ALTER TABLE PaymentCode ADD CONSTRAINT
		PK_PaymentCode PRIMARY KEY CLUSTERED 
		(
			PaymentCodeID
		)
		

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0095'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





