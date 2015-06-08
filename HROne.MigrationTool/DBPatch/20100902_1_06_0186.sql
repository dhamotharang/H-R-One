
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.06.0175' 
BEGIN

	CREATE TABLE dbo.Tmp_RequestEmpPersonalInfo
	(
		RequestEmpID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		RequestEmpAlias nvarchar(255) NULL,
		RequestEmpMaritalStatus nvarchar(100) NULL,
		RequestEmpPassportNo nvarchar(255) NULL,
		RequestEmpPassportIssuedCountry nvarchar(255) NULL,
		RequestEmpNationality nvarchar(255) NULL,
		RequestEmpPassportExpiryDate datetime NULL,
		RequestEmpHomePhoneNo nvarchar(255) NULL,
		RequestEmpMobileNo nvarchar(255) NULL,
		RequestEmpOfficePhoneNo nvarchar(255) NULL,
		RequestEmpEmail nvarchar(255) NULL,
		RequestEmpResAddr nvarchar(255) NULL,
		RequestEmpCorAdd nvarchar(255) NULL,
		RequestEmpCreateDate datetime NULL
	)  

	SET IDENTITY_INSERT dbo.Tmp_RequestEmpPersonalInfo ON

	IF EXISTS(SELECT * FROM dbo.RequestEmpPersonalInfo)
		 EXEC('INSERT INTO dbo.Tmp_RequestEmpPersonalInfo (RequestEmpID, EmpID, RequestEmpAlias, RequestEmpMaritalStatus, RequestEmpPassportNo, RequestEmpPassportIssuedCountry, RequestEmpNationality, RequestEmpHomePhoneNo, RequestEmpMobileNo, RequestEmpOfficePhoneNo, RequestEmpEmail, RequestEmpResAddr, RequestEmpCorAdd, RequestEmpCreateDate)
			SELECT RequestEmpID, EmpID, RequestEmpAlias, RequestEmpMaritalStatus, RequestEmpPassportNo, RequestEmpPassportIssuedCountry, RequestEmpNationality, RequestEmpHomePhoneNo, RequestEmpMobileNo, RequestEmpOfficePhoneNo, RequestEmpEmail, RequestEmpResAddr, RequestEmpCorAdd, RequestEmpCreateDate FROM dbo.RequestEmpPersonalInfo WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT dbo.Tmp_RequestEmpPersonalInfo OFF
	DROP TABLE dbo.RequestEmpPersonalInfo
	EXECUTE sp_rename N'dbo.Tmp_RequestEmpPersonalInfo', N'RequestEmpPersonalInfo', 'OBJECT' 
	ALTER TABLE dbo.RequestEmpPersonalInfo ADD CONSTRAINT
		PK_RequestEmpPersonalInfo PRIMARY KEY CLUSTERED 
		(
			RequestEmpID
		) 
		
	ALTER TABLE EmpBankAccount
		ADD EmpBankAccountRemark NTEXT NULL
			
	INSERT INTO MPFScheme(MPFSchemeCode, MPFSchemeDesc)
		VALUES ('MT00156', 'AIA-JF Mandatory Provident Fund Scheme')			
			
	INSERT INTO MPFScheme(MPFSchemeCode, MPFSchemeDesc)
		VALUES ('MT00172', 'AIA-JF Premium MPF Scheme')			

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.06.0186'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





