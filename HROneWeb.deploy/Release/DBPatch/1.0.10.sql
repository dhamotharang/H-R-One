DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.7' 
BEGIN
	BEGIN TRANSACTION 
	
	CREATE NONCLUSTERED INDEX IX_PayrollPeriod_1
	ON payrollPeriod (PayGroupID ASC);

	CREATE NONCLUSTERED INDEX IX_PayrollPeriod_2
	ON payrollPeriod (PayPeriodFr DESC);
	
	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY', NULL, 'Y');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
	VALUES
		('ESS_FUNCTION_LEAVE_APPLICATION_LIST', NULL, 'Y');

	INSERT INTO MPFParameter
		(MPFParamEffFr, MPFParamMinMonthly, MPFParamMaxMonthly, MPFParamMinDaily, MPFParamMaxDaily, MPFParamEEPercent, MPFParamERPercent)
	VALUES
		('2014-06-01', 7100, 30000, 280, 1000, 5, 5);

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.10'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.10';
	
	COMMIT TRANSACTION
END