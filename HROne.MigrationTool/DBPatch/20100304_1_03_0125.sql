DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0118' 
BEGIN

	CREATE TABLE TrainingCourse
	(
		TrainingCourseID int NOT NULL IDENTITY (1, 1),
		TrainingCourseCode nvarchar(20) NULL,
		TrainingCourseName nvarchar(100) NULL,
		CONSTRAINT PK_TrainingCourse PRIMARY KEY CLUSTERED 
		(
			TrainingCourseID
		) 
	)

	CREATE TABLE TrainingSeminar
	(
		TrainingSeminarID int NOT NULL IDENTITY (1, 1),
		TrainingCourseID int NULL,
		TrainingSeminarDateFrom datetime NULL,
		TrainingSeminarDateTo datetime NULL,
		TrainingSeminarDuration real NULL,
		TrainingSeminarDurationUnit nvarchar(1) NULL,
		TrainingSeminarTrainer nvarchar(250) NULL,
		CONSTRAINT PK_TrainingSeminar PRIMARY KEY CLUSTERED 
		(
			TrainingSeminarID
		) 
	)
	
	CREATE TABLE EmpTrainingEnroll
	(
		EmpTrainingEnrollID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		TrainingCourseID int NULL,
		CONSTRAINT PK_EmpTrainingEnroll PRIMARY KEY CLUSTERED 
		(
			EmpTrainingEnrollID
		) 
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0125'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





