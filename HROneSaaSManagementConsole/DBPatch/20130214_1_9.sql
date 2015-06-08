

DECLARE @DBVERSION as varchar(100);
SET @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.8' 
Begin
	
	CREATE TABLE CompanyAutopayFileSignature 
	(
		CompanyAutopayFileSignatureID INT NOT NULL IDENTITY (1, 1),
		CompanyAutopayFileID INT NULL,
		CompanyDBID INT NULL,
		UserID INT NULL,
		CompanyAutopayFileSignatureUserName NVARCHAR(255) NULL,
		CompanyAutopayFileSignatureDateTime DATETIME NULL,
		CONSTRAINT PK_CompanyAutopayFileSignature PRIMARY KEY CLUSTERED 
		(
			CompanyAutopayFileSignatureID
		)
	)

	CREATE TABLE CompanyMPFFileSignature 
	(
		CompanyMPFFileSignatureID INT NOT NULL IDENTITY (1, 1),
		CompanyMPFFileID INT NULL,
		CompanyDBID INT NULL,
		UserID INT NULL,
		CompanyMPFFileSignatureUserName NVARCHAR(255) NULL,
		CompanyMPFFileSignatureDateTime DATETIME NULL,
		CONSTRAINT PK_CompanyMPFFileSignature PRIMARY KEY CLUSTERED 
		(
			CompanyMPFFileSignatureID
		)
	)
	
	ALTER TABLE CompanyAutopayFile ADD
		CompanyAutopayFileValueDate DateTime NULL

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.9'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



