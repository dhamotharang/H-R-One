DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0078' 
BEGIN

	
	CREATE TABLE PermitType
	(
		PermitTypeID int NOT NULL IDENTITY (1, 1),
		PermitTypeCode nvarchar(20) NULL,
		PermitTypeDesc nvarchar(100) NULL,
		CONSTRAINT	PK_PermitType PRIMARY KEY 
		(
			PermitTypeID
		)
	)
	
	Create Table EmpPermit
	(
		EmpPermitID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		PermitTypeID int NULL,
		EmpPermitNo nvarchar(50) null,
		EmpPermitIssueDate DateTime Null,
		EmpPermitExpiryDate DateTime Null,
		EmpPermitRemark nText Null,
		CONSTRAINT	PK_EmpPermit PRIMARY KEY 
		(
			EmpPermitID
		)
	)
	
	
	Create Table DocumentType
	(
		DocumentTypeID int NOT NULL IDENTITY (1, 1),
		DocumentTypeCode nvarchar(20) NULL,
		DocumentTypeDesc nvarchar(100) NULL,
		DocumentTypeIsSystem int NULL,
		CONSTRAINT	PK_DocumentType PRIMARY KEY 
		(
			DocumentTypeID
		)
	)
	
	Create Table EmpDocument
	(
		EmpDocumentID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		DocumentTypeID int NULL,
		EmpDocumentOriginalFileName nvarchar(250) null,
		EmpDocumentDesc nvarchar(250) null,
		EmpDocumentStoredFileName nvarchar(250) null,
		EmpDocumentIsCompressed int NULL,
		EmpDocumentIsProfilePhoto int NULL,
		CONSTRAINT	PK_EmpDocument PRIMARY KEY 
		(
			EmpDocumentID
		)
	)
	
	Alter Table Company ADD
		CompanyBankHolderName nvarchar(100) null

	Alter Table CessationReason ADD
		CessationReasonHasProrataYEB int null
	
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('PER013','Employee Work Permit/License Information','Personnel', 0)
	
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory
           ,FunctionIsHidden)
     VALUES
           ('PER014','Employee Document List','Personnel', 0)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0079'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





