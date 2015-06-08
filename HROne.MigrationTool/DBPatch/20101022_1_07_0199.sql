
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0198' 
BEGIN
	
	Delete from EmpUniform
		where EmpUniformB='' and EmpUniformW='' and EmpUniformH=''

	IF EXISTS(SELECT * FROM EmpUniform)
	BEGIN
		Insert into EmpExtraField
			(EmpExtraFieldName, EmpExtraFieldControlType, EmpExtraFieldGroupName)
		Values
			('Bust (B)', 'TextBox' , 'Uniform Information')

		Insert into EmpExtraField
			(EmpExtraFieldName, EmpExtraFieldControlType, EmpExtraFieldGroupName)
		Values
			('Waist (W)', 'TextBox' , 'Uniform Information')

		Insert into EmpExtraField
			(EmpExtraFieldName, EmpExtraFieldControlType, EmpExtraFieldGroupName)
		Values
			('Hips (H)', 'TextBox' , 'Uniform Information')

		Insert into EmpExtraFieldValue
			(EmpExtraFieldID, EmpID, EmpExtraFieldValue)
		Select EmpExtraFieldID, EmpID, EmpUniformB
		From EmpUniform, EmpExtraField
		Where EmpExtraFieldName='Bust (B)'
		and EmpUniformB<>''
		
		Insert into EmpExtraFieldValue
			(EmpExtraFieldID, EmpID, EmpExtraFieldValue)
		Select EmpExtraFieldID, EmpID, EmpUniformW
		From EmpUniform, EmpExtraField
		Where EmpExtraFieldName='Waist (W)'
		and EmpUniformW<>''

		Insert into EmpExtraFieldValue
			(EmpExtraFieldID, EmpID, EmpExtraFieldValue)
		Select EmpExtraFieldID, EmpID, EmpUniformH
		From EmpUniform, EmpExtraField
		Where EmpExtraFieldName='Hips (H)'
		and EmpUniformH<>''

	END
	
	CREATE TABLE Tmp_LeavePlanEntitle
	(
		LeavePlanEntitleID int NOT NULL IDENTITY (1, 1),
		LeavePlanID int NULL,
		LeaveTypeID int NULL,
		LeavePlanEntitleYearOfService int NULL,
		LeavePlanEntitleDays real NULL
	)
	SET IDENTITY_INSERT Tmp_LeavePlanEntitle ON
	IF EXISTS(SELECT * FROM LeavePlanEntitle)
		EXEC('INSERT INTO Tmp_LeavePlanEntitle (LeavePlanEntitleID, LeavePlanID, LeaveTypeID, LeavePlanEntitleYearOfService, LeavePlanEntitleDays)
			SELECT LeavePlanEntitleID, LeavePlanID, LeaveTypeID, LeavePlanEntitleYearOfService, CONVERT(real, LeavePlanEntitleDays) FROM LeavePlanEntitle WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_LeavePlanEntitle OFF
	DROP TABLE LeavePlanEntitle
	EXECUTE sp_rename N'Tmp_LeavePlanEntitle', N'LeavePlanEntitle', 'OBJECT' 
	ALTER TABLE dbo.LeavePlanEntitle ADD CONSTRAINT
		PK__LeavePlanEntitle__18EBB532 PRIMARY KEY CLUSTERED 
		(
			LeavePlanEntitleID
		)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0199'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





