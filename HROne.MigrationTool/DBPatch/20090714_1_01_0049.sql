DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0046' 
BEGIN

	CREATE TABLE Tmp_EmpPersonalInfo
		(
		EmpID int NOT NULL IDENTITY (1, 1),
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
		EmpResAddr nvarchar(250) NULL,
		EmpResAddrAreaCode nvarchar(1) NULL,
		EmpCorAddr nvarchar(250) NULL,
		EmpDateOfJoin datetime NULL,
		EmpServiceDate datetime NULL,
		EmpNoticePeriod int NULL,
		EmpNoticeUnit nvarchar(1) NULL,
		EmpProbaPeriod int NULL,
		EmpProbaUnit nvarchar(1) NULL,
		EmpProbaLastDate datetime NULL,
		EmpEmail nvarchar(100) NULL,
		EmpHomePhoneNo nvarchar(100) NULL,
		EmpMobileNo nvarchar(100) NULL,
		EmpOfficePhoneNo nvarchar(100) NULL,
		Remark ntext NULL,
		PreviousEmpID int NULL,
		EmpPW nvarchar(50) NULL,
		EmpInternalEmail nvarchar(100) NULL
		)  
		
	SET IDENTITY_INSERT Tmp_EmpPersonalInfo ON

	IF EXISTS(SELECT * FROM EmpPersonalInfo)
		 EXEC('INSERT INTO Tmp_EmpPersonalInfo (EmpID, EmpNo, EmpStatus, EmpEngSurname, EmpEngOtherName, EmpChiFullName, EmpAlias, EmpHKID, EmpGender, EmpMaritalStatus, EmpDateOfBirth, EmpPlaceOfBirth, EmpNationality, EmpPassportNo, EmpPassportIssuedCountry, EmpPassportExpiryDate, EmpResAddr, EmpResAddrAreaCode, EmpCorAddr, EmpDateOfJoin, EmpServiceDate, EmpNoticePeriod, EmpNoticeUnit, EmpProbaPeriod, EmpProbaUnit, EmpProbaLastDate, EmpEmail, EmpHomePhoneNo, EmpMobileNo, EmpOfficePhoneNo, Remark, PreviousEmpID, EmpPW, EmpInternalEmail)
			SELECT EmpID, EmpNo, EmpStatus, EmpEngSurname, EmpEngOtherName, EmpChiFullName, EmpAlias, EmpHKID, EmpGender, EmpMaritalStatus, EmpDateOfBirth, EmpPlaceOfBirth, EmpNationality, EmpPassportNo, EmpPassportIssuedCountry, EmpPassportExpiryDate, EmpResAddr, EmpResAddrAreaCode, EmpCorAddr, EmpDateOfJoin, EmpServiceDate, EmpNoticePeriod, EmpNoticeUnit, EmpProbaPeriod, EmpProbaUnit, EmpProbaLastDate, EmpEmail, EmpHomePhoneNo, EmpMobileNo, EmpOfficePhoneNo, Remark, PreviousEmpID, EmpPW, EmpInternalEmail FROM EmpPersonalInfo WITH (HOLDLOCK TABLOCKX)')
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
	set ParameterValue='1.01.0049'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




