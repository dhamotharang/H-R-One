DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0086' 
BEGIN

	Create Table EmpExtraField
	(
		EmpExtraFieldID int NOT NULL IDENTITY (1, 1),
		EmpExtraFieldName nvarchar(50) NULL,
		CONSTRAINT PK_EmpExtraField PRIMARY KEY CLUSTERED 
		(
		EmpExtraFieldID
		) 
	)
	
	Create Table EmpExtraFieldValue
	(
		EmpExtraFieldValueID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpExtraFieldID int NULL,
		EmpExtraFieldValue ntext NULL,
		CONSTRAINT PK_EmpExtraFieldValue PRIMARY KEY CLUSTERED 
		(
		EmpExtraFieldValueID
		) 
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0087'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





