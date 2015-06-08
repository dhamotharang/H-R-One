
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.26'
BEGIN

	BEGIN TRANSACTION 

	ALTER TABLE AttendanceRecord
		ADD AttendanceRecordWaivedLateMins int DEFAULT 0

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.26a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.26a';
	
	COMMIT TRANSACTION
END

