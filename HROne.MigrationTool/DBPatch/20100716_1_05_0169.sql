
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.05.0162' 
BEGIN

	CREATE TABLE Tmp_TrainingSeminar
	(
		TrainingSeminarID int NOT NULL IDENTITY (1, 1),
		TrainingCourseID int NULL,
		TrainingSeminarDesc nvarchar(250) NULL,
		TrainingSeminarDateFrom datetime NULL,
		TrainingSeminarDateTo datetime NULL,
		TrainingSeminarDuration real NULL,
		TrainingSeminarDurationUnit nvarchar(1) NULL,
		TrainingSeminarTrainer nvarchar(250) NULL
	)

	SET IDENTITY_INSERT Tmp_TrainingSeminar ON

	IF EXISTS(SELECT * FROM TrainingSeminar)
		 EXEC('INSERT INTO Tmp_TrainingSeminar (TrainingSeminarID, TrainingCourseID, TrainingSeminarDateFrom, TrainingSeminarDateTo, TrainingSeminarDuration, TrainingSeminarDurationUnit, TrainingSeminarTrainer)
			SELECT TrainingSeminarID, TrainingCourseID, TrainingSeminarDateFrom, TrainingSeminarDateTo, TrainingSeminarDuration, TrainingSeminarDurationUnit, TrainingSeminarTrainer FROM TrainingSeminar WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT Tmp_TrainingSeminar OFF
	DROP TABLE TrainingSeminar
	EXECUTE sp_rename N'Tmp_TrainingSeminar', N'TrainingSeminar', 'OBJECT' 

	ALTER TABLE TrainingSeminar ADD CONSTRAINT
		PK_TrainingSeminar PRIMARY KEY CLUSTERED 
		(
			TrainingSeminarID
		)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.05.0169'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





