DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0093' 
BEGIN

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('MPF005','MPF Termination Code Setup','MPF', 0)
           
           
           
           
	Insert into ReminderType (ReminderTypeCode,ReminderTypeDesc)
	Values ('PROBATION', 'Probation Reminder')
	Insert into ReminderType (ReminderTypeCode,ReminderTypeDesc)
	Values ('TERMINATION', 'Employee Termination Reminder')
	Insert into ReminderType (ReminderTypeCode,ReminderTypeDesc)
	Values ('WORKPERMITEXPIRY', 'Work Permit Expiry Reminder')
           
	CREATE TABLE Tmp_RequestEmpPersonalInfo
	(
		RequestEmpID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		RequestEmpAlias nvarchar(255) NULL,
		RequestEmpMaritalStatus nvarchar(100) NULL,
		RequestEmpPassportNo nvarchar(255) NULL,
		RequestEmpPassportIssuedCountry nvarchar(255) NULL,
		RequestEmpNationality nvarchar(255) NULL,
		RequestEmpHomePhoneNo nvarchar(255) NULL,
		RequestEmpMobileNo nvarchar(255) NULL,
		RequestEmpOfficePhoneNo nvarchar(255) NULL,
		RequestEmpEmail nvarchar(255) NULL,
		RequestEmpResAddr nvarchar(255) NULL,
		RequestEmpCorAdd nvarchar(255) NULL,
		RequestEmpCreateDate datetime NULL
	) 
	SET IDENTITY_INSERT Tmp_RequestEmpPersonalInfo ON
	IF EXISTS(SELECT * FROM RequestEmpPersonalInfo)
		 EXEC('INSERT INTO Tmp_RequestEmpPersonalInfo (RequestEmpID, EmpID, RequestEmpAlias, RequestEmpMaritalStatus, RequestEmpPassportNo, RequestEmpPassportIssuedCountry, RequestEmpNationality, RequestEmpHomePhoneNo, RequestEmpMobileNo, RequestEmpOfficePhoneNo, RequestEmpEmail, RequestEmpResAddr, RequestEmpCorAdd, RequestEmpCreateDate)
			SELECT RequestEmpID, EmpID, RequestEmpAlias, RequestEmpMaritalStatus, RequestEmpPassportNo, RequestEmpPassportIssuedCountry, RequestEmpNationality, RequestEmpHomePhoneNo, RequestEmpMobileNo, RequestEmpOfficePhoneNo, RequestEmpEmail, RequestEmpResAddr, RequestEmpCorAdd, RequestEmpCreateDate FROM RequestEmpPersonalInfo WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_RequestEmpPersonalInfo OFF
	DROP TABLE RequestEmpPersonalInfo
	EXECUTE sp_rename N'Tmp_RequestEmpPersonalInfo', N'RequestEmpPersonalInfo', 'OBJECT' 
	ALTER TABLE RequestEmpPersonalInfo ADD CONSTRAINT
		PK_RequestEmpPersonalInfo PRIMARY KEY CLUSTERED 
		(
			RequestEmpID
		) 

	CREATE TABLE Tmp_EmpSpouse
	(
		EmpSpouseID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpSpouseSurname nvarchar(255) NULL,
		EmpSpouseOtherName nvarchar(255) NULL,
		EmpSpouseChineseName nvarchar(255) NULL,
		EmpSpouseHKID nvarchar(50) NULL,
		EmpSpousePassportNo nvarchar(255) NULL,
		EmpSpousePassportIssuedCountry nvarchar(255) NULL,
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
		EmpDependantSurname nvarchar(255) NULL,
		EmpDependantOtherName nvarchar(255) NULL,
		EmpDependantChineseName nvarchar(255) NULL,
		EmpDependantGender nvarchar(10) NULL,
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

	CREATE TABLE Tmp_EmpBankAccount
	(
		EmpBankAccountID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpBankCode nvarchar(30) NULL,
		EmpBranchCode nvarchar(30) NULL,
		EmpAccountNo nvarchar(90) NULL,
		EmpBankAccountHolderName nvarchar(255) NULL,
		EmpAccDefault int NULL
	)
	SET IDENTITY_INSERT Tmp_EmpBankAccount ON
	IF EXISTS(SELECT * FROM EmpBankAccount)
		 EXEC('INSERT INTO Tmp_EmpBankAccount (EmpBankAccountID, EmpID, EmpBankCode, EmpBranchCode, EmpAccountNo, EmpBankAccountHolderName, EmpAccDefault)
			SELECT EmpBankAccountID, EmpID, EmpBankCode, EmpBranchCode, EmpAccountNo, EmpBankAccountHolderName, EmpAccDefault FROM EmpBankAccount WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_EmpBankAccount OFF
	DROP TABLE EmpBankAccount
	EXECUTE sp_rename N'Tmp_EmpBankAccount', N'EmpBankAccount', 'OBJECT' 
	ALTER TABLE EmpBankAccount ADD CONSTRAINT
		PK_EmpBankAccount PRIMARY KEY CLUSTERED 
		(
			EmpBankAccountID
		)

	CREATE TABLE dbo.Tmp_EmpPlaceOfResidence
	(
		EmpPoRID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpPoRFrom datetime NULL,
		EmpPoRTo datetime NULL,
		EmpPoRLandLord nvarchar(255) NULL,
		EmpPoRLandLordAddr nvarchar(255) NULL,
		EmpPoRPropertyAddr nvarchar(255) NULL,
		EmpPoRNature nvarchar(190) NULL,
		EmpPoRPayToLandER decimal(15, 4) NOT NULL,
		EmpPoRPayToLandEE decimal(15, 4) NOT NULL,
		EmpPoRRefundToEE decimal(15, 4) NOT NULL,
		EmpPoRPayToERByEE decimal(15, 4) NOT NULL
	) 
	SET IDENTITY_INSERT dbo.Tmp_EmpPlaceOfResidence ON
	IF EXISTS(SELECT * FROM dbo.EmpPlaceOfResidence)
		 EXEC('INSERT INTO dbo.Tmp_EmpPlaceOfResidence (EmpPoRID, EmpID, EmpPoRFrom, EmpPoRTo, EmpPoRLandLord, EmpPoRLandLordAddr, EmpPoRPropertyAddr, EmpPoRNature, EmpPoRPayToLandER, EmpPoRPayToLandEE, EmpPoRRefundToEE, EmpPoRPayToERByEE)
			SELECT EmpPoRID, EmpID, EmpPoRFrom, EmpPoRTo, EmpPoRLandLord, EmpPoRLandLordAddr, EmpPoRPropertyAddr, EmpPoRNature, EmpPoRPayToLandER, EmpPoRPayToLandEE, EmpPoRRefundToEE, EmpPoRPayToERByEE FROM dbo.EmpPlaceOfResidence WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT dbo.Tmp_EmpPlaceOfResidence OFF
	DROP TABLE dbo.EmpPlaceOfResidence
	EXECUTE sp_rename N'dbo.Tmp_EmpPlaceOfResidence', N'EmpPlaceOfResidence', 'OBJECT' 
	ALTER TABLE dbo.EmpPlaceOfResidence ADD CONSTRAINT
		PK_EmpPlaceOfResidence PRIMARY KEY CLUSTERED 
		(
			EmpPoRID
		)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0094'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





