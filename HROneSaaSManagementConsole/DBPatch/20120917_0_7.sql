

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.6' 
Begin

	CREATE TABLE CompanyInbox
	(
		CompanyInboxID int NOT NULL IDENTITY (1, 1),
		CompanyDBID int NULL,
		CompanyInboxSubject nvarchar(100) NULL,
		CompanyInboxRelatedRecID int NULL,
		CompanyInboxMessage ntext NULL,
		CompanyInboxCreateDate datetime NULL,
		CompanyInboxReadDate datetime NULL,
		CompanyInboxDeleteDate datetime NULL,
		CONSTRAINT	PK_CompanyInbox PRIMARY KEY 
		(
			CompanyInboxID
		)	
	)

	CREATE TABLE CompanyInboxAttachment
	(
		CompanyInboxAttachmentID INT NOT NULL IDENTITY (1, 1),
		CompanyInboxID INT NULL,
		CompanyInboxAttachmentOriginalFileName NVARCHAR(250) NULL,
		CompanyInboxAttachmentStoredFileName NVARCHAR(250) NULL,
		CompanyInboxAttachmentIsCompressed INT NULL,
		CompanyInboxAttachmentSize BIGINT NULL
		CONSTRAINT PK_CompanyInboxAttachment PRIMARY KEY CLUSTERED 
		(
			CompanyInboxAttachmentID
		) 
	)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.7'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



