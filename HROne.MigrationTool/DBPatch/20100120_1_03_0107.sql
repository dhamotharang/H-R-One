DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0101' 
BEGIN

	CREATE TABLE EmpEmergencyContact
	(
		EmpEmergencyContactID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpEmergencyContactName nvarchar(255) NULL,
		EmpEmergencyContactGender nvarchar(100) NULL,
		EmpEmergencyContactRelationship nvarchar(255) NULL,
		EmpEmergencyContactContactNoDay nvarchar(255) NULL,
		EmpEmergencyContactContactNoNight nvarchar(255) NULL,
		CONSTRAINT PK_EmpEmergencyContact PRIMARY KEY CLUSTERED 
		(
			EmpEmergencyContactID
		)
	)
	
	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('PER015','Emergency Contact','Personnel', 0)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0107'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





