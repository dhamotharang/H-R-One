DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0043' 
BEGIN

	CREATE TABLE Tmp_EmpSpouse
		(
		EmpSpouseID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpSpouseSurname nvarchar(20) NULL,
		EmpSpouseOtherName nvarchar(40) NULL,
		EmpSpouseChineseName nvarchar(50) NULL,
		EmpSpouseHKID nvarchar(12) NULL,
		EmpSpousePassportNo nvarchar(40) NULL,
		EmpSpousePassportIssuedCountry nvarchar(40) NULL
		)  ON [PRIMARY]
	SET IDENTITY_INSERT Tmp_EmpSpouse ON
	IF EXISTS(SELECT * FROM EmpSpouse)
		 EXEC('INSERT INTO Tmp_EmpSpouse (EmpSpouseID, EmpID, EmpSpouseSurname, EmpSpouseOtherName, EmpSpouseChineseName, EmpSpouseHKID, EmpSpousePassportNo, EmpSpousePassportIssuedCountry)
			SELECT EmpSpouseID, CONVERT(int, EmpID), EmpSpouseSurname, EmpSpouseOtherName, EmpSpouseChineseName, EmpSpouseHKID, EmpSpousePassportNo, EmpSpousePassportIssuedCountry FROM EmpSpouse WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_EmpSpouse OFF
	DROP TABLE EmpSpouse
	EXECUTE sp_rename N'Tmp_EmpSpouse', N'EmpSpouse', 'OBJECT' 
	ALTER TABLE EmpSpouse ADD CONSTRAINT
		PK_EmpSpouse PRIMARY KEY CLUSTERED 
		(
		EmpSpouseID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


	CREATE TABLE Tmp_Authorizer
		(
		AuthorizerID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		AuthorizationGroupID int NULL
		)  ON [PRIMARY]
	SET IDENTITY_INSERT Tmp_Authorizer ON
	IF EXISTS(SELECT * FROM Authorizer)
		 EXEC('INSERT INTO Tmp_Authorizer (AuthorizerID, EmpID, AuthorizationGroupID)
			SELECT AuthorizerID, EmpID, AuthorizationGroupID FROM Authorizer WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_Authorizer OFF
	DROP TABLE Authorizer
	EXECUTE sp_rename N'Tmp_Authorizer', N'Authorizer', 'OBJECT' 
	ALTER TABLE Authorizer ADD CONSTRAINT
		PK_Authorizer PRIMARY KEY CLUSTERED 
		(
		AuthorizerID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	INSERT INTO PaymentType
           (PaymentTypeCode
           ,PaymentTypeDesc)
		VALUES
           ('LSPSP','Long Service Payment / Severance Payment')
           
	UPDATE PaymentCode
		SET PaymentTypeID=(Select PaymentTypeID from PaymentType where PaymentTypeCode='LSPSP')           
		WHERE (PaymentCodeDesc like '%Long Service%' OR PaymentCodeDesc like '%Severance%')
		AND PaymentTypeID=(Select PaymentTypeID from PaymentType where PaymentTypeCode='OTHERS')
	
	Update PaymentCode
		Set PaymentCode='LSP'
		WHERE PaymentCodeDesc like '%Long Service%'
		AND  PaymentCode='LONGSERVICE'
		AND PaymentTypeID=(Select PaymentTypeID from PaymentType where PaymentTypeCode='LSPSP')

	Update PaymentCode
		Set PaymentCode='SP'
		WHERE PaymentCodeDesc like '%Severance%'
		AND  PaymentCode='SEVERANCE'
		AND PaymentTypeID=(Select PaymentTypeID from PaymentType where PaymentTypeCode='LSPSP')

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0044'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

