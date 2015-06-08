

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.2' 
Begin

Create TABLE CompanyMPFFile 
(
	CompanyMPFFileID INT NOT NULL IDENTITY (1, 1),
	CompanyDBID INT NULL,
	CompanyMPFFileRelativePath NVARCHAR(255) NULL,
	CompanyMPFFileSubmitDateTime DATETIME NULL,
	CompanyMPFFileConfirmDateTime DATETIME NULL,
	CompanyMPFFileConsolidateDateTime DATETIME NULL,
	CONSTRAINT PK_CompanyMPFFile PRIMARY KEY CLUSTERED 
	(
		CompanyMPFFileID
	) 
)
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.3'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



