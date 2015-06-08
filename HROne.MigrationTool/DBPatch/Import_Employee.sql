DROP TABLE UploadEmpPersonalInfo
DROP TABLE UploadEmpPositionInfo
DROP TABLE UploadEmpHierarchy
DROP TABLE UploadEmpRecurringPayment
DROP TABLE UploadEmpBankAccount
DROP TABLE UploadEmpMPFPlan
DROP TABLE UploadEmpAVCPlan
DROP TABLE UploadEmpORSOPlan
DROP TABLE UploadEmpSpouse
DROP TABLE UploadEmpDependant
DROP TABLE UploadEmpQualification
DROP TABLE UploadEmpSkill
DROP TABLE UploadEmpPlaceOfResidence
DROP TABLE UploadEmpContractTerms
DROP TABLE UploadEmpTermination
DROP TABLE UploadLeaveApplication
DROP TABLE UploadEmpFinalPayment
DROP TABLE UploadEmpCostCenter
DROP TABLE UploadEmpCostCenterDetail
DROP TABLE UploadEmpPermit
DROP TABLE UploadClaimsAndDeductions
DROP TABLE UploadRosterTable
DROP TABLE UploadTimeCardRecord
DROP TABLE UploadAttendanceRecord
DROP TABLE UploadEmpExtraFieldValue
DROP TABLE UploadEmpEmergencyContact
DROP TABLE UploadEmpWorkExp
DROP TABLE UploadEmpWorkInjuryRecord
DROP TABLE UploadEmpWorkingSummary
DROP TABLE UploadLeaveBalanceAdjustment
DROP TABLE UploadEmpRosterTableGroup
DROP TABLE UploadCompensationLeaveEntitle

CREATE TABLE UploadEmpPersonalInfo
(
	UploadEmpID int IDENTITY(1,1) NOT NULL,
	EmpID int NULL,
	EmpNo nvarchar(20) NULL,
	EmpStatus nvarchar(3) NULL,
	EmpEngSurname nvarchar(20) NULL,
	EmpEngOtherName nvarchar(55) NULL,
	EmpChiFullName nvarchar(50) NULL,
	EmpAlias nvarchar(100) NULL,
	EmpHKID nvarchar(50) NULL,
	EmpGender nvarchar(1) NULL,
	EmpMaritalStatus nvarchar(10) NULL,
	EmpDateOfBirth datetime NULL,
	EmpPlaceOfBirth nvarchar(100) NULL,
	EmpNationality nvarchar(100) NULL,
	EmpPassportNo nvarchar(100) NULL,
	EmpPassportIssuedCountry nvarchar(40) NULL,
	EmpPassportExpiryDate datetime NULL,
	EmpResAddr nvarchar(200) NULL,
	EmpResAddrAreaCode nvarchar(1) NULL,
	EmpCorAddr nvarchar(200) NULL,
	EmpDateOfJoin datetime NULL,
	EmpServiceDate datetime NULL,
	EmpNoticePeriod int NULL,
	EmpNoticeUnit nvarchar(1) NULL,
	EmpProbaPeriod int NULL,
	EmpProbaUnit nvarchar(1) NULL,
	EmpProbaLastDate datetime NULL,
	EmpEmail nvarchar(100) NULL,
	EmpInternalEmail nvarchar(100) NULL,
	EmpHomePhoneNo nvarchar(100) NULL,
	EmpMobileNo nvarchar(100) NULL,
	EmpOfficePhoneNo nvarchar(100) NULL,
	Remark ntext NULL,
	EmpTimeCardNo nvarchar(255) NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpPersonalInfo PRIMARY KEY CLUSTERED 
	(
		UploadEmpID ASC
	)
)

CREATE TABLE UploadEmpPositionInfo
(
	UploadEmpPosID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpPosID int NULL,
	EmpID int NULL,
	EmpPosEffFr datetime NULL,
	EmpPosEffTo datetime NULL,
	CompanyID int NULL,
	PositionID int NULL,
	RankID int NULL,
	EmploymentTypeID int NULL,
	StaffTypeID int NULL,
	PayGroupID int NULL,
	LeavePlanID int NULL,
	EmpPosIsLeavePlanResetEffectiveDate INT NULL,
	YEBPlanID int NULL,
	AuthorizationWorkFlowIDLeaveApp INT NULL,
	AuthorizationWorkFlowIDEmpInfoModified INT NULL,
	AttendancePlanID int NULL,
	EmpPosDefaultRosterCodeID int NULL,
	WorkHourPatternID int NULL,
	RosterTableGroupID int NULL,
	EmpPosIsRosterTableGroupSupervisor int NULL,
	EmpPosRemark NTEXT NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpPositionInfo PRIMARY KEY CLUSTERED 
	(
		UploadEmpPosID ASC
	)
)

CREATE TABLE UploadEmpHierarchy
(
	UploadEmpHierarchyID int IDENTITY(1,1) NOT NULL,			
	UploadEmpPosID int NULL,
	EmpHierarchyID int NULL,
	EmpID int NULL,
	EmpPosID int NULL,
	HElementID int NULL,
	HLevelID int NULL,
 	CONSTRAINT PK_UploadEmpHierarchy PRIMARY KEY CLUSTERED 
	(
		UploadEmpHierarchyID ASC
	)
)

CREATE TABLE UploadEmpRecurringPayment
(
	UploadEmpRPID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	UploadEmpAccID int NULL,
	EmpRPID int NULL,
	EmpID int NULL,
	EmpRPEffFr datetime NULL,
	EmpRPEffTo datetime NULL,
	PayCodeID int NULL,
	EmpRPAmount decimal(15, 4) NULL,
	CurrencyID nvarchar(3) NULL,
	EmpRPUnit nvarchar(50) NULL,
	EmpRPMethod nvarchar(50) NULL,
	EmpAccID int NULL,
	EmpRPIsNonPayrollItem int NULL,
	CostCenterID int NULL,
	EmpRPRemark ntext NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpRecurringPayment PRIMARY KEY CLUSTERED 
	(
		UploadEmpRPID ASC
	)
)

CREATE TABLE UploadEmpBankAccount
(
	UploadEmpBankAccountID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpBankAccountID int NULL,
	EmpID int NULL,
	EmpBankCode nvarchar(3) NULL,
	EmpBranchCode nvarchar(3) NULL,
	EmpAccountNo nvarchar(9) NULL,
	EmpBankAccountHolderName nvarchar(100) NULL,
	EmpAccDefault int NULL,
	EmpBankAccountRemark NTEXT NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpBankAccount PRIMARY KEY CLUSTERED 
	(
		UploadEmpBankAccountID ASC
	)
)

CREATE TABLE UploadEmpMPFPlan
(
	UploadEmpMPFID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpMPFID int NULL,
	EmpID int NULL,
	EmpMPFEffFr datetime NULL,
	EmpMPFEffTo datetime NULL,
	MPFPlanID int NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpMPFPlan PRIMARY KEY CLUSTERED 
	(
		UploadEmpMPFID ASC
	)
) 

CREATE TABLE UploadEmpAVCPlan
(
	UploadEmpAVCID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpAVCID int NULL,
	EmpID int NULL,
	EmpAVCEffFr datetime NULL,
	EmpAVCEffTo datetime NULL,
	AVCPlanID int NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpAVCPlan PRIMARY KEY CLUSTERED 
	(
		UploadEmpAVCID ASC
	)
) 

CREATE TABLE UploadEmpORSOPlan
(
	UploadEmpORSOID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpORSOID int NULL,
	EmpID int NULL,
	EmpORSOEffFr datetime NULL,
	EmpORSOEffTo datetime NULL,
	ORSOPlanID int NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpORSOPlan PRIMARY KEY CLUSTERED 
	(
		UploadEmpORSOID ASC
	)
)

CREATE TABLE UploadEmpSpouse
(
	UploadEmpSpouseID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpSpouseID int NULL,
	EmpID int NULL,
	EmpSpouseSurname nvarchar(20) NULL,
	EmpSpouseOtherName nvarchar(40) NULL,
	EmpSpouseChineseName nvarchar(50) NULL,
	EmpSpouseDateOfBirth datetime NULL,	
	EmpSpouseHKID nvarchar(12) NULL,
	EmpSpousePassportNo nvarchar(40) NULL,
	EmpSpousePassportIssuedCountry nvarchar(40) NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpSpouse PRIMARY KEY CLUSTERED 
	(
		UploadEmpSpouseID ASC
	)
)

CREATE TABLE UploadEmpDependant
(
	UploadEmpDependantID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpDependantID int NULL,
	EmpID int NULL,
	EmpDependantSurname nvarchar(20) NULL,
	EmpDependantOtherName nvarchar(40) NULL,
	EmpDependantChineseName nvarchar(50) NULL,
	EmpDependantGender nvarchar(1) NULL,
	EmpDependantDateOfBirth datetime NULL,
	EmpDependantHKID nvarchar(12) NULL,
	EmpDependantPassportNo nvarchar(40) NULL,
	EmpDependantPassportIssuedCountry nvarchar(40) NULL,
	EmpDependantRelationship nvarchar(100) NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpDependant PRIMARY KEY CLUSTERED 
	(
		UploadEmpDependantID ASC
	)
)

CREATE TABLE UploadEmpQualification
(
	UploadEmpQualificationID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpQualificationID int NULL,
	EmpID int NULL,
	QualificationID int NULL,
	EmpQualificationFrom datetime NULL,
	EmpQualificationTo datetime NULL,
	EmpQualificationInstitution nvarchar(100) NULL,
	EmpQualificationLearningMethod NVARCHAR(100) NULL,
	EmpQualificationRemark ntext NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpQualification PRIMARY KEY CLUSTERED 
	(
		UploadEmpQualificationID ASC
	)
)

CREATE TABLE UploadEmpSkill
(
	UploadEmpSkillID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpSkillID int NULL,
	EmpID int NULL,
	SkillID int NULL,
	SkillLevelID int NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpSkill PRIMARY KEY CLUSTERED 
	(
		UploadEmpSkillID ASC
	)
)

CREATE TABLE UploadEmpPlaceOfResidence
(
	UploadEmpPoRID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpPoRID int NOT NULL,
	EmpID int NULL,
	EmpPoRFrom datetime NULL,
	EmpPoRTo datetime NULL,
	EmpPoRLandLord nvarchar(100) NULL,
	EmpPoRLandLordAddr nvarchar(110) NULL,
	EmpPoRPropertyAddr nvarchar(110) NULL,
	EmpPoRNature nvarchar(19) NULL,
	EmpPoRPayToLandER numeric(18, 4) NOT NULL,
	EmpPoRPayToLandEE numeric(18, 4) NOT NULL,
	EmpPoRRefundToEE numeric(18, 4) NOT NULL,
	EmpPoRPayToERByEE numeric(18, 4) NOT NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpPlaceOfResidence PRIMARY KEY CLUSTERED 
	(
		UploadEmpPoRID ASC
	)
)

CREATE TABLE UploadEmpContractTerms
(
	UploadEmpContractID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpContractID int NULL,
	EmpID int NULL,
	EmpContractCompanyName nvarchar(100) NULL,
	EmpContractCompanyContactNo nvarchar(100) NULL,
	EmpContractCompanyAddr nvarchar(100) NULL,
	EmpContractEmployedFrom datetime NULL,
	EmpContractEmployedTo datetime NULL,
	EmpContractGratuity decimal(15, 4) NULL,
	CurrencyID nvarchar(3) NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpContractTerms PRIMARY KEY CLUSTERED 
	(
		UploadEmpContractID ASC
	)
)

CREATE TABLE UploadEmpTermination
(
	UploadEmpTermID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpTermID int NULL,
	EmpID int NULL,
	CessationReasonID int NULL,
	EmpTermResignDate datetime NULL,
	EmpTermNoticePeriod int NULL,
	EmpTermNoticeUnit nvarchar(1) NULL,
	EmpTermLastDate datetime NULL,
	EmpTermRemark ntext NULL,
	EmpTermIsTransferCompany int NULL,
	NewEmpID int NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadEmpTermination PRIMARY KEY CLUSTERED 
	(
		UploadEmpTermID ASC
	)
)

CREATE TABLE UploadLeaveApplication
(
	UploadLeaveAppID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	LeaveAppID int NULL,
	EmpID int NULL,
	LeaveCodeID int NULL,
	LeaveAppUnit nvarchar(1) NULL,
	LeaveAppDateFrom datetime NULL,
	LeaveAppDateTo datetime NULL,
	LeaveAppTimeFrom DateTime NULL,
	LeaveAppTimeTo DateTime NULL,
	LeaveAppDays decimal(15, 4) NULL,
	LeaveAppHours decimal(15, 4) NULL,
	LeaveAppRemark ntext NULL,
	LeaveAppNoPayProcess int NULL,
	LeaveAppHasMedicalCertificate int NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
 	CONSTRAINT PK_UploadLeaveApplication PRIMARY KEY CLUSTERED 
	(
		UploadLeaveAppID ASC
	)
)

CREATE TABLE UploadEmpFinalPayment
(
	UploadEmpFinalPayID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	UploadEmpAccID int NULL,
	EmpFinalPayID int NULL,
	EmpID int NULL,
	PayCodeID int NULL,
	EmpFinalPayAmount decimal(15, 4) NULL,
	CurrencyID nvarchar(3) NULL,
	EmpFinalPayMethod nvarchar(50) NULL,
	EmpAccID int NULL,
	EmpFinalPayIsAutoGen int NULL,
	EmpFinalPayNumOfDayAdj real NULL,
	LeaveAppID int NULL,
	PayRecID nchar(10) NULL,
	CostCenterID int NULL,
	EmpFinalPayRemark ntext NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadEmpFinalPayment PRIMARY KEY CLUSTERED 
	(
		UploadEmpFinalPayID ASC
	)
)

CREATE TABLE UploadEmpCostCenter
(
	UploadEmpCostCenterID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpCostCenterID int NULL,
	EmpID int NULL,
	EmpCostCenterEffFr datetime NULL,
	EmpCostCenterEffTo datetime NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadEmpCostCenter PRIMARY KEY CLUSTERED 
	(
	UploadEmpCostCenterID ASC
	)
)
CREATE TABLE UploadEmpCostCenterDetail
(
	UploadEmpCostCenterDetailID int IDENTITY(1,1) NOT NULL,
	UploadEmpCostCenterID int NULL,
	EmpCostCenterDetailID int NULL,
	EmpCostCenterID int NULL,
	CostCenterID int NULL,
	EmpCostCenterPercentage real NULL,
	EmpCostCenterDetailIsDefault int NULL,
	CONSTRAINT PK_UploadEmpCostCenterDetail PRIMARY KEY CLUSTERED 
	(
		UploadEmpCostCenterDetailID ASC
	)
)

Create Table UploadEmpPermit
(
	UploadEmpPermitID int NOT NULL IDENTITY (1, 1),
	UploadEmpID int NULL,
	EmpPermitID int NULL,
	EmpID int NULL,
	PermitTypeID int NULL,
	EmpPermitNo nvarchar(50) null,
	EmpPermitIssueDate DateTime Null,
	EmpPermitExpiryDate DateTime Null,
	EmpPermitRemark nText Null,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT	PK_UploadEmpPermit PRIMARY KEY 
	(
		UploadEmpPermitID
	)
)

Create Table UploadEmpExtraFieldValue
(
	UploadEmpExtraFieldValueID int NOT NULL IDENTITY (1, 1),
	EmpExtraFieldValueID int,
	UploadEmpID int NULL,
	EmpID int NULL,
	EmpExtraFieldID int NULL,
	EmpExtraFieldValue ntext NULL,
	CONSTRAINT PK_UploadEmpExtraFieldValue PRIMARY KEY CLUSTERED 
	(
	UploadEmpExtraFieldValueID
	) 
)

	
CREATE TABLE UploadEmpEmergencyContact
(
	UploadEmpEmergencyContactID int NOT NULL IDENTITY (1, 1),
	UploadEmpID int NULL,
	EmpEmergencyContactID int NULL,
	EmpID int NULL,
	EmpEmergencyContactName nvarchar(255) NULL,
	EmpEmergencyContactGender nvarchar(100) NULL,
	EmpEmergencyContactRelationship nvarchar(255) NULL,
	EmpEmergencyContactContactNoDay nvarchar(255) NULL,
	EmpEmergencyContactContactNoNight nvarchar(255) NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadEmpEmergencyContact PRIMARY KEY CLUSTERED 
	(
		UploadEmpEmergencyContactID
	)
)	

CREATE TABLE UploadEmpWorkExp
(
	UploadEmpWorkExpID int IDENTITY(1,1) NOT NULL,
	UploadEmpID int NULL,
	EmpWorkExpID int NULL,
	EmpID int NULL,
	EmpWorkExpFromYear int NULL,
	EmpWorkExpFromMonth int NULL,
	EmpWorkExpToYear int NULL,
	EmpWorkExpToMonth int NULL,
	EmpWorkExpCompanyName nvarchar(100) NULL,
	EmpWorkExpPosition nvarchar(100) NULL,
	EmpWorkExpEmploymentTypeID INT NULL,
	EmpWorkExpIsRelevantExperience INT NULL,
	EmpWorkExpRemark ntext NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadEmpWorkExp PRIMARY KEY CLUSTERED 
	(
		UploadEmpWorkExpID
	)
)

Create Table UploadEmpWorkInjuryRecord
(
	UploadEmpWorkInjuryRecordID int NOT NULL IDENTITY (1, 1),
	UploadEmpID int NULL,
	EmpWorkInjuryRecordID int NULL,
	EmpID int NULL,
	EmpWorkInjuryRecordAccidentDate datetime NULL,
	EmpWorkInjuryRecordAccidentLocation nvarchar(100) NULL,
	EmpWorkInjuryRecordAccidentReason nvarchar(100) NULL,
	EmpWorkInjuryRecordInjuryNature nvarchar(50) NULL,
	EmpWorkInjuryRecordReportedDate datetime NULL,
	EmpWorkInjuryRecordChequeReceivedDate datetime NULL,
	EmpWorkInjuryRecordSettleDate datetime NULL,
	EmpWorkInjuryRecordRemark ntext NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadEmpWorkInjuryRecord PRIMARY KEY CLUSTERED 
	(
		UploadEmpWorkInjuryRecordID
	)
)

CREATE TABLE UploadClaimsAndDeductions
(
	UploadCNDID int NOT NULL IDENTITY (1, 1),
	EmpID int NOT NULL,
	CNDEffDate datetime NOT NULL,
	PayCodeID int NOT NULL,
	CNDAmount decimal(15, 4) NOT NULL,
	CurrencyID nvarchar(3) NOT NULL,
	CNDPayMethod nvarchar(1) NULL,
	EmpAccID int NULL,
	CNDIsRestDayPayment INT NULL,
	CostCenterID int NULL,
	CNDRemark NTEXT NULL,
	CNDNumOfDayAdj real NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadClaimsAndDeductions PRIMARY KEY CLUSTERED 
	(
		UploadCNDID
	) 
)
	
CREATE TABLE UploadRosterTable
(
	UploadRosterTableID int IDENTITY(1,1) NOT NULL,
	RosterTableID int NULL,
	EmpID int NULL,
	RosterTableDate datetime NULL,
	RosterCodeID int NULL,
	RosterTableOverrideInTime datetime NULL,
	RosterTableOverrideOutTime datetime NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadRosterTable PRIMARY KEY CLUSTERED 
	(
		UploadRosterTableID
	)
)

CREATE TABLE UploadTimeCardRecord
(
	UploadTimeCardRecordID int NOT NULL IDENTITY (1, 1),
	TimeCardRecordID int NULL,
	EmpID int NULL,
	TimeCardRecordCardNo nvarchar(20) NULL,
	TimeCardRecordDateTime datetime NULL,
	TimeCardRecordLocation nvarchar(50) NULL,
	TimeCardRecordInOutIndex int NULL,
	TimeCardRecordOriginalData ntext NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadTimeCardRecord PRIMARY KEY CLUSTERED 
	(
		UploadTimeCardRecordID
	) 
)

	CREATE TABLE UploadAttendanceRecord
	(
		UploadAttendanceRecordID int NOT NULL IDENTITY (1, 1),
		AttendanceRecordID int NULL,
		EmpID int NULL,
		AttendanceRecordDate datetime NULL,
		RosterCodeID int NULL,
		RosterTableID int NULL,
		AttendanceRecordWorkStart datetime NULL,
		AttendanceRecordWorkStartLocation NVARCHAR(255) NULL,
		AttendanceRecordWorkEnd datetime NULL,
		AttendanceRecordWorkEndLocation NVARCHAR(255) NULL,
		AttendanceRecordLunchOut datetime NULL,
		AttendanceRecordLunchOutLocation NVARCHAR(255) NULL,		
		AttendanceRecordLunchIn datetime NULL,
		AttendanceRecordLunchInLocation NVARCHAR(255) NULL,	
		AttendanceRecordCalculateLateMins int NULL,
		AttendanceRecordActualLateMins int NULL,
		AttendanceRecordCalculateEarlyLeaveMins int NULL,
		AttendanceRecordActualEarlyLeaveMins int NULL,
		AttendanceRecordCalculateOvertimeMins int NULL,
		AttendanceRecordActualOvertimeMins int NULL,
		AttendanceRecordActualLunchOvertimeMins INT NULL,
		AttendanceRecordCalculateLunchTimeMins INT NULL,
		AttendanceRecordActualLunchTimeMins INT NULL,
		AttendanceRecordCalculateWorkingDay real NULL,
		AttendanceRecordActualWorkingDay real NULL,
		AttendanceRecordCalculateWorkingHour real NULL,
		AttendanceRecordActualWorkingHour real NULL,
		AttendanceRecordIsAbsent int NULL,
		AttendanceRecordWorkOnRestDay INT NULL,
		AttendanceRecordRemark ntext NULL,
		AttendanceRecordOverrideBonusEntitled int NULL,
		AttendanceRecordHasBonus int NULL,
		AttendanceRecordExtendData NTEXT NULL,
		AttendanceRecordCalculateLunchLateMins INT NULL,
		AttendanceRecordActualLunchLateMins INT NULL,
		AttendanceRecordCalculateLunchEarlyLeaveMins INT NULL,
		AttendanceRecordActualLunchEarlyLeaveMins INT NULL,
		AttendanceRecordRosterCodeInTimeOverride DATETIME NULL,
		AttendanceRecordRosterCodeOutTimeOverride DATETIME NULL,
		AttendanceRecordRosterCodeLunchStartTimeOverride DATETIME NULL,
		AttendanceRecordRosterCodeLunchEndTimeOverride DATETIME NULL,
		SessionID nvarchar(200) NOT NULL,
		TransactionDate datetime NOT NULL,
		ImportAction nvarchar(100) NULL,
		ModifiedFieldList NTEXT NULL,
		CONSTRAINT PK_UploadAttendanceRecord PRIMARY KEY CLUSTERED 
		(
			UploadAttendanceRecordID
		)
	)
	
	CREATE TABLE UploadEmpWorkingSummary
	(
		UploadEmpWorkingSummaryID int NOT NULL IDENTITY (1, 1),
		EmpWorkingSummaryID int NULL,
		EmpID int NULL,
		EmpWorkingSummaryAsOfDate datetime NULL,
		EmpWorkingSummaryRestDayEntitled real NULL,
		EmpWorkingSummaryRestDayTaken real NULL,
		EmpWorkingSummaryTotalWorkingDays real NULL,
		EmpWorkingSummaryTotalWorkingHours real NULL,
		EmpWorkingSummaryTotalLunchTimeHours REAL NULL,
		SessionID nvarchar(200) NOT NULL,
		TransactionDate datetime NOT NULL,
		ImportAction nvarchar(100) NULL,
		ModifiedFieldList NTEXT NULL,
		CONSTRAINT PK_UploadEmpWorkingSummary PRIMARY KEY CLUSTERED 
		(
			UploadEmpWorkingSummaryID
		) 
	)
CREATE TABLE UploadLeaveBalanceAdjustment
(
	UploadLeaveBalAdjID int NOT NULL IDENTITY (1, 1),
	UploadEmpID int NULL,
	LeaveBalAdjID int NULL,
	EmpID int NULL,
	LeaveBalAdjDate datetime NULL,
	LeaveTypeID int NULL,
	LeaveBalAdjType nvarchar(50) NULL,
	LeaveBalAdjValue real NULL,
	LeaveBalAdjRemark ntext NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadLeaveBalanceAdjustment PRIMARY KEY CLUSTERED 
	(
		UploadLeaveBalAdjID
	)
)

CREATE TABLE UploadEmpRosterTableGroup
(
	UploadEmpRosterTableGroupID INT NOT NULL IDENTITY (1, 1),
	UploadEmpID int NULL,
	EmpRosterTableGroupID INT NULL,
	EmpID INT NULL,
	EmpRosterTableGroupEffFr DATETIME NULL,
	EmpRosterTableGroupEffTo DATETIME NULL,
	RosterTableGroupID INT NULL,
	EmpRosterTableGroupIsSupervisor INT NULL,
	SynID nvarchar(255) NULL,
	SessionID nvarchar(200) NOT NULL,
	TransactionDate datetime NOT NULL,
	ImportAction nvarchar(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadEmpRosterTableGroup PRIMARY KEY CLUSTERED 
	(
		UploadEmpRosterTableGroupID
	) 
)

CREATE TABLE UploadCompensationLeaveEntitle
(
	UploadCompensationLeaveEntitleID INT NOT NULL IDENTITY (1, 1),
	UploadEmpID INT NULL,
	CompensationLeaveEntitleID INT NULL,
	EmpID INT NULL,
	CompensationLeaveEntitleEffectiveDate DATETIME NULL,
	CompensationLeaveEntitleClaimPeriodFrom DATETIME NULL,
	CompensationLeaveEntitleClaimPeriodTo DATETIME NULL,
	CompensationLeaveEntitleClaimHourFrom DATETIME NULL,
	CompensationLeaveEntitleClaimHourTo DATETIME NULL,
	CompensationLeaveEntitleHoursClaim REAL NULL,
	CompensationLeaveEntitleDateExpiry DATETIME NULL,
	CompensationLeaveEntitleApprovedBy NVARCHAR(255) NULL,
	CompensationLeaveEntitleIsAutoGenerated INT NULL,
	CompensationLeaveEntitleRemark NTEXT NULL,
	SynID NVARCHAR(255) NULL,
	SessionID NVARCHAR(200) NOT NULL,
	TransactionDate DATETIME NOT NULL,
	ImportAction NVARCHAR(100) NULL,
	ModifiedFieldList NTEXT NULL,
	CONSTRAINT PK_UploadCompensationLeaveEntitle PRIMARY KEY CLUSTERED 
	(
		UploadCompensationLeaveEntitleID
	)
)