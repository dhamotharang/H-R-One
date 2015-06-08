DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0125' 
BEGIN

	Drop Table EmpTrainingEnroll
	
	CREATE TABLE EmpTrainingEnroll
	(
		EmpTrainingEnrollID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		TrainingSeminarID int NULL,
		CONSTRAINT PK_EmpTrainingEnroll PRIMARY KEY CLUSTERED 
		(
			EmpTrainingEnrollID
		) 
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0126'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





