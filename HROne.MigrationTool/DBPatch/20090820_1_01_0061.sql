DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0060' 
BEGIN

	DROP TABLE RosterTable

	CREATE TABLE RosterTable
	(
		RosterTableID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		RosterTableDate datetime NULL,
		RosterCodeID int NULL,
		CONSTRAINT PK_RosterTable PRIMARY KEY CLUSTERED 
		(
			RosterTableID
		)
	)
	
	CREATE TABLE TimeCardRecord
	(
		TimeCardRecordID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		TimeCardRecordCardNo nvarchar(20) NULL,
		TimeCardRecordDateTime datetime NULL,
		TimeCardRecordLocation nvarchar(50) NULL,
		TimeCardRecordInOutIndex int NULL,
		TimeCardRecordOriginalData nvarchar(1000) NULL,
		CONSTRAINT PK_TimeCardRecord PRIMARY KEY CLUSTERED 
		(
			TimeCardRecordID
		) 
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0061'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




