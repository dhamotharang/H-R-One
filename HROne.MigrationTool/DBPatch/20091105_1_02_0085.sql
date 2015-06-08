DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0084' 
BEGIN
	
	CREATE TABLE Tmp_EmpSpouse
		(
		EmpSpouseID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpSpouseSurname nvarchar(20) NULL,
		EmpSpouseOtherName nvarchar(40) NULL,
		EmpSpouseChineseName nvarchar(50) NULL,
		EmpSpouseHKID nvarchar(50) NULL,
		EmpSpousePassportNo nvarchar(100) NULL,
		EmpSpousePassportIssuedCountry nvarchar(40) NULL,
		EmpSpouseDateOfBirth datetime NULL
		)
	SET IDENTITY_INSERT Tmp_EmpSpouse ON
	IF EXISTS(SELECT * FROM EmpSpouse)
		 EXEC('INSERT INTO Tmp_EmpSpouse (EmpSpouseID, EmpID, EmpSpouseSurname, EmpSpouseOtherName, EmpSpouseChineseName, EmpSpouseHKID, EmpSpousePassportNo, EmpSpousePassportIssuedCountry, EmpSpouseDateOfBirth)
			SELECT EmpSpouseID, EmpID, EmpSpouseSurname, EmpSpouseOtherName, EmpSpouseChineseName, EmpSpouseHKID, EmpSpousePassportNo, EmpSpousePassportIssuedCountry, EmpSpouseDateOfBirth FROM EmpSpouse WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_EmpSpouse OFF
	DROP TABLE EmpSpouse
	EXECUTE sp_rename N'Tmp_EmpSpouse', N'EmpSpouse', 'OBJECT' 
	ALTER TABLE EmpSpouse ADD CONSTRAINT
		PK_EmpSpouse PRIMARY KEY CLUSTERED 
		(
		EmpSpouseID
		)

	CREATE TABLE Tmp_EmpDependant
		(
		EmpDependantID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpDependantSurname nvarchar(20) NULL,
		EmpDependantOtherName nvarchar(40) NULL,
		EmpDependantChineseName nvarchar(50) NULL,
		EmpDependantGender nvarchar(1) NULL,
		EmpDependantHKID nvarchar(50) NULL,
		EmpDependantPassportNo nvarchar(100) NULL,
		EmpDependantPassportIssuedCountry nvarchar(40) NULL,
		EmpDependantRelationship nvarchar(100) NULL,
		EmpDependantDateOfBirth datetime NULL
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
		
	CREATE TABLE Tmp_TaxEmp
		(
		TaxEmpID int NOT NULL IDENTITY (1, 1),
		TaxFormID int NULL,
		EmpID int NULL,
		TaxEmpSheetNo int NULL,
		TaxEmpHKID nvarchar(50) NULL,
		TaxEmpStatus nvarchar(1) NULL,
		TaxEmpSurname nvarchar(20) NULL,
		TaxEmpOtherName nvarchar(55) NULL,
		TaxEmpChineseName nvarchar(50) NULL,
		TaxEmpSex nvarchar(1) NULL,
		TaxEmpMartialStatus nvarchar(1) NULL,
		TaxEmpPassportNo nvarchar(100) NULL,
		TaxEmpIssuedCountry nvarchar(40) NULL,
		TaxEmpSpouseName nvarchar(50) NULL,
		TaxEmpSpouseHKID nvarchar(50) NULL,
		TaxEmpSpousePassportNo nvarchar(100) NULL,
		TaxEmpSpouseIssuedCountry nvarchar(40) NULL,
		TaxEmpResAddr nvarchar(90) NULL,
		TaxEmpResAddrAreaCode nvarchar(1) NULL,
		TaxEmpCorAddr nvarchar(60) NULL,
		TaxEmpCapacity nvarchar(40) NULL,
		TaxEmpPartTimeEmployer nvarchar(30) NULL,
		TaxEmpStartDate datetime NULL,
		TaxEmpEndDate datetime NULL,
		TaxEmpCessationReason nvarchar(100) NULL,
		TaxEmpPlaceOfResidenceIndicator int NULL,
		TaxEmpOvearseasIncomeIndicator int NULL,
		TaxEmpOverseasCompanyAmount nvarchar(20) NULL,
		TaxEmpOverseasCompanyName nvarchar(60) NULL,
		TaxEmpOverseasCompanyAddress nvarchar(60) NULL,
		TaxEmpTaxFileNo nvarchar(13) NULL,
		TaxEmpRemark nvarchar(60) NULL,
		TaxEmpNewEmployerNameddress nvarchar(200) NULL,
		TaxEmpFutureCorAddr nvarchar(200) NULL,
		TaxEmpLeaveHKDate datetime NULL,
		TaxEmpIsERBearTax nvarchar(1) NULL,
		TaxEmpIsMoneyHoldByOrdinance nvarchar(1) NULL,
		TaxEmpHoldAmount real NULL,
		TaxEmpReasonForNotHold nvarchar(200) NULL,
		TaxEmpReasonForDepartureReason nvarchar(200) NULL,
		TaxEmpReasonForDepartureOtherReason nvarchar(200) NULL,
		TaxEmpIsEEReturnHK nvarchar(1) NULL,
		TaxEmpEEReturnHKDate datetime NULL,
		TaxEmpIsShareOptionsGrant nvarchar(1) NULL,
		TaxEmpShareOptionsGrantCount nvarchar(200) NULL,
		TaxEmpShareOptionsGrantDate datetime NULL,
		TaxEmpPreviousEmployerNameddress nvarchar(200) NULL
		)  ON [PRIMARY]

	SET IDENTITY_INSERT Tmp_TaxEmp ON
	IF EXISTS(SELECT * FROM TaxEmp)
		 EXEC('INSERT INTO Tmp_TaxEmp (TaxEmpID, TaxFormID, EmpID, TaxEmpSheetNo, TaxEmpHKID, TaxEmpStatus, TaxEmpSurname, TaxEmpOtherName, TaxEmpChineseName, TaxEmpSex, TaxEmpMartialStatus, TaxEmpPassportNo, TaxEmpIssuedCountry, TaxEmpSpouseName, TaxEmpSpouseHKID, TaxEmpSpousePassportNo, TaxEmpSpouseIssuedCountry, TaxEmpResAddr, TaxEmpResAddrAreaCode, TaxEmpCorAddr, TaxEmpCapacity, TaxEmpPartTimeEmployer, TaxEmpStartDate, TaxEmpEndDate, TaxEmpCessationReason, TaxEmpPlaceOfResidenceIndicator, TaxEmpOvearseasIncomeIndicator, TaxEmpOverseasCompanyAmount, TaxEmpOverseasCompanyName, TaxEmpOverseasCompanyAddress, TaxEmpTaxFileNo, TaxEmpRemark, TaxEmpNewEmployerNameddress, TaxEmpFutureCorAddr, TaxEmpLeaveHKDate, TaxEmpIsERBearTax, TaxEmpIsMoneyHoldByOrdinance, TaxEmpHoldAmount, TaxEmpReasonForNotHold, TaxEmpReasonForDepartureReason, TaxEmpReasonForDepartureOtherReason, TaxEmpIsEEReturnHK, TaxEmpEEReturnHKDate, TaxEmpIsShareOptionsGrant, TaxEmpShareOptionsGrantCount, TaxEmpShareOptionsGrantDate, TaxEmpPreviousEmployerNameddress)
			SELECT TaxEmpID, TaxFormID, EmpID, TaxEmpSheetNo, TaxEmpHKID, TaxEmpStatus, TaxEmpSurname, TaxEmpOtherName, TaxEmpChineseName, TaxEmpSex, TaxEmpMartialStatus, TaxEmpPassportNo, TaxEmpIssuedCountry, TaxEmpSpouseName, TaxEmpSpouseHKID, TaxEmpSpousePassportNo, TaxEmpSpouseIssuedCountry, TaxEmpResAddr, TaxEmpResAddrAreaCode, TaxEmpCorAddr, TaxEmpCapacity, TaxEmpPartTimeEmployer, TaxEmpStartDate, TaxEmpEndDate, TaxEmpCessationReason, TaxEmpPlaceOfResidenceIndicator, TaxEmpOvearseasIncomeIndicator, TaxEmpOverseasCompanyAmount, TaxEmpOverseasCompanyName, TaxEmpOverseasCompanyAddress, TaxEmpTaxFileNo, TaxEmpRemark, TaxEmpNewEmployerNameddress, TaxEmpFutureCorAddr, TaxEmpLeaveHKDate, TaxEmpIsERBearTax, TaxEmpIsMoneyHoldByOrdinance, TaxEmpHoldAmount, TaxEmpReasonForNotHold, TaxEmpReasonForDepartureReason, TaxEmpReasonForDepartureOtherReason, TaxEmpIsEEReturnHK, TaxEmpEEReturnHKDate, TaxEmpIsShareOptionsGrant, TaxEmpShareOptionsGrantCount, TaxEmpShareOptionsGrantDate, TaxEmpPreviousEmployerNameddress FROM TaxEmp WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_TaxEmp OFF
	DROP TABLE TaxEmp
	EXECUTE sp_rename N'Tmp_TaxEmp', N'TaxEmp', 'OBJECT' 
	ALTER TABLE TaxEmp ADD CONSTRAINT
		PK_TaxEmp PRIMARY KEY CLUSTERED 
		(
		TaxEmpID
		)
		
		
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0085'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





