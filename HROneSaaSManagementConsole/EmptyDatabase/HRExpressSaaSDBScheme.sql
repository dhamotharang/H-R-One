CREATE TABLE DatabaseServer
(
	DBServerID int NOT NULL IDENTITY (1, 1),
	DBServerCode NVARCHAR(255) NULL,
	DBServerDBType NVARCHAR(100) NULL,
	DBServerLocation NVARCHAR(255) NULL,
	DBServerSAUserID NVARCHAR(255) NULL,
	DBServerSAPassword NVARCHAR(255) NULL,
	DBServerUserID NVARCHAR(255) NULL,
	DBServerPassword NVARCHAR(255) NULL,
	CONSTRAINT PK_DatabaseServer PRIMARY KEY CLUSTERED 
	(
		DBServerID
	)
)

CREATE TABLE CompanyDatabase
(
	CompanyDBID int NOT NULL IDENTITY (1, 1),
	CompanyDBClientCode NVARCHAR(255) NULL,
	CompanyDBClientContactPerson NVARCHAR(255) NULL,
	CompanyDBClientName NVARCHAR(255) NULL,
	CompanyDBClientAddress NTEXT NULL,
	DBServerID INT NULL,
	CompanyDBSchemaName NVARCHAR(255) NULL,
	CompanyDBIsActive INT NULL,
	CompanyDBMaxCompany INT NULL,
	CompanyDBMaxUser INT NULL,
	CompanyDBProductKey NVARCHAR(255) NULL,
	CompanyDBTrialKey NVARCHAR(255) NULL,
	CompanyDBAuthorizationCode NVARCHAR(255) NULL,
	CONSTRAINT PK_CompanyDatabase PRIMARY KEY CLUSTERED 
	(
		CompanyDBID
	)
)

CREATE TABLE SystemParameter
(
	ParameterCode NVARCHAR(100) NOT NULL,
	ParameterDesc NVARCHAR(200) NULL,
	ParameterValue NVARCHAR(200) NULL,
	CONSTRAINT PK_SystemParameter PRIMARY KEY CLUSTERED 
	(
		ParameterCode
	) 
)

CREATE TABLE Users
(
	UserID int NOT NULL IDENTITY (1, 1),
	LoginID nvarchar(255) NULL,
	UserName nvarchar(255) NULL,
	UserPassword nvarchar(255) NULL,
	UserAccountStatus nvarchar(1) NULL,
	ExpiryDate datetime NULL,
	UserChangePassword int NULL,
	UserChangePasswordUnit nvarchar(1) NULL,
	UserChangePasswordPeriod int NULL,
	UserChangePasswordDate datetime NULL,
	FailCount int NULL,
	UserLanguage nvarchar(10) NULL,
	UserIsKeepConnected int NULL,
	UsersCannotCreateUsersWithMorePermission int NULL,	
	CONSTRAINT PK_Users PRIMARY KEY CLUSTERED 
	(
		UserID
	) 
)

Insert into SystemParameter
	(ParameterCode, ParameterDesc, ParameterValue)
Values
	('DBVERSION', 'Database Version', '0.1')
	
INSERT INTO Users
    (LoginID,UserName,UserPassword,UserAccountStatus,UserChangePassword,UserChangePasswordPeriod,FailCount)
 VALUES
	('admin','Administrator','RbmoyE0kTJ1SzmImbL+Wew==','A',0,0,0)
	