
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.19d'
BEGIN
	BEGIN TRANSACTION 

	IF (NOT EXISTS(SELECT * FROM SystemFunction WHERE FunctionCode = 'SYS022'))
	BEGIN
		INSERT INTO SystemFunction VALUES('SYS022', 'Place of Birth', 'System', 0);
	END;
	
	IF (NOT EXISTS(SELECT * FROM SystemFunction WHERE FunctionCode = 'SYS023'))
	BEGIN
		INSERT INTO SystemFunction VALUES('SYS023', 'Country of Issue', 'System', 0);
	END;
	
	IF (NOT EXISTS(SELECT * FROM SystemFunction WHERE FunctionCode = 'SYS024'))
	BEGIN
		INSERT INTO SystemFunction VALUES('SYS024', 'Nationality', 'System', 0);
	END;

	ALTER TABLE EmpPersonalInfo ADD
		EmpNationalityID int,
		EmpPassportIssuedCountryID int,
		EmpPlaceOfBirthID int;
		
	ALTER TABLE UploadEmpPersonalInfo ADD
		EmpNationalityID int,
		EmpPassportIssuedCountryID int,
		EmpPlaceOfBirthID int;
	
	CREATE TABLE PlaceOfBirth
	(
		[PlaceOfBirthID] int NOT NULL IDENTITY(1,1),
		[PlaceOfBirthCode] nvarchar(20) NULL,
		[PlaceOfBirthDesc] nvarchar(100) NULL,
		CONSTRAINT PK_PlaceOfBirth PRIMARY KEY CLUSTERED 
		(
			[PlaceOfBirthID] ASC
		) 
	);

	CREATE TABLE Nationality
	(
		NationalityID int NOT NULL IDENTITY(1,1),
		NationalityCode nvarchar(20) NULL,
		NationalityDesc nvarchar(100) NULL,
		CONSTRAINT PK_Nationality PRIMARY KEY CLUSTERED 
		(
			NationalityID ASC
		) 
	);

	CREATE TABLE IssueCountry
	(
		CountryID int NOT NULL IDENTITY(1,1),
		CountryCode nvarchar(20) NULL,
		CountryDesc nvarchar(100) NULL,
		CONSTRAINT PK_IssueCountry PRIMARY KEY CLUSTERED 
		(
			CountryID ASC
		) 
	);

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.20'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.20';
	
	COMMIT TRANSACTION
END

