DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0092' 
BEGIN

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('ATT012','Roster Client Setup','Attendance', 0)
		
	Create Table MPFSchemeCessationReason
	(
		MPFSchemeCessationReasonID int NOT NULL IDENTITY (1,1),
		MPFSchemeID int Null,
		MPFSchemeCessationReasonCode NVARCHAR(10) NULL,
		MPFSchemeCessationReasonDesc NVARCHAR(100) NULL,
		Constraint PK_MPFSchemeCessationReason Primary Key
		(
			MPFSchemeCessationReasonID
		)
	)

	Create Table MPFSchemeCessationReasonMapping
	(
		MPFSchemeCessationReasonMappingID int NOT NULL IDENTITY (1,1),
		MPFSchemeCessationReasonID int NULL,
		CessationReasonID int NULL,
		Constraint PK_MPFSchemeCessationReasonMapping Primary Key
		(
			MPFSchemeCessationReasonMappingID
		)
	)
	
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
		EmpGender nvarchar(10) NULL,
		EmpMaritalStatus nvarchar(100) NULL,
		EmpDateOfBirth datetime NULL,
		EmpPlaceOfBirth nvarchar(255) NULL,
		EmpNationality nvarchar(255) NULL,
		EmpPassportNo nvarchar(255) NULL,
		EmpPassportIssuedCountry nvarchar(255) NULL,
		EmpPassportExpiryDate datetime NULL,
		EmpResAddr nvarchar(250) NULL,
		EmpResAddrAreaCode nvarchar(10) NULL,
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
	
	
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0093'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





