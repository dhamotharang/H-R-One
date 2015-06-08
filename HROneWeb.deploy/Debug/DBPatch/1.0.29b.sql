
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.29a'
BEGIN

	BEGIN TRANSACTION 

	-- new minimum wage compliance
	IF (NOT EXISTS(SELECT MinimumWageID FROM MinimumWage WHERE MinimumWageEffectiveDate = '2015-05-01' AND MinimumWageHourlyRate = 32.5)) 
	BEGIN
		INSERT INTO MinimumWage
			(MinimumWageEffectiveDate, MinimumWageHourlyRate)
		VALUES 
			('2015-05-01', 32.5);
	END

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.29b'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.29b';
	
	COMMIT TRANSACTION
END

