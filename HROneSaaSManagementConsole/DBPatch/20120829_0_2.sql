

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.1' 
Begin

Create TABLE CompanyBankFile 
(
	CompanyBankFileID INT NOT NULL IDENTITY (1, 1),
	CompanyDBID INT NULL,
	CompanyBankFileRelativePath NVARCHAR(255) NULL,
	CompanyBankFileSubmitDateTime DATETIME NULL,
	CompanyBankFileConfirmDateTime DATETIME NULL,
	CompanyBankFileConsolidateDateTime DATETIME NULL,
	CONSTRAINT PK_CompanyBankFile PRIMARY KEY CLUSTERED 
	(
		CompanyBankFileID
	) 
)
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.2'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



