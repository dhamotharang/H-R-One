

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.3' 
Begin

DROP TABLE CompanyBankFile
DROP TABLE CompanyMPFFile

Create TABLE CompanyMPFFile 
(
	CompanyMPFFileID INT NOT NULL IDENTITY (1, 1),
	CompanyDBID INT NULL,
	CompanyMPFFileTrusteeCode NVARCHAR(255) NULL,
	CompanyMPFFileDataFileRelativePath NTEXT NULL,
	CompanyMPFFileReportFileRelativePath NTEXT NULL,
	CompanyMPFFileSubmitDateTime DATETIME NULL,
	CompanyMPFFileConfirmDateTime DATETIME NULL,
	CompanyMPFFileConsolidateDateTime DATETIME NULL,
	CONSTRAINT PK_CompanyMPFFile PRIMARY KEY CLUSTERED 
	(
		CompanyMPFFileID
	) 
)


Create TABLE CompanyAutopayFile 
(
	CompanyAutopayFileID INT NOT NULL IDENTITY (1, 1),
	CompanyDBID INT NULL,
	CompanyAutopayFileBankCode NVARCHAR(255) NULL,
	CompanyAutopayFileDataFileRelativePath NTEXT NULL,
	CompanyAutopayFileReportFileRelativePath NTEXT NULL,
	CompanyAutopayFileSubmitDateTime DATETIME NULL,
	CompanyAutopayFileConfirmDateTime DATETIME NULL,
	CompanyAutopayFileConsolidateDateTime DATETIME NULL,
	CONSTRAINT PK_CompanyAutopayFile PRIMARY KEY CLUSTERED 
	(
		CompanyAutopayFileID
	) 
)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.4'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



