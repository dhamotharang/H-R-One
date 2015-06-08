DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.17a'
BEGIN
	BEGIN TRANSACTION 

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('EMP_LIST_SHOW_COMPANY', 'Show/Hide Company in employee search result list', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('EMP_LIST_SHOW_H1', 'Show/Hide Hierarchy1 in employee search result list', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('EMP_LIST_SHOW_H2', 'Show/Hide Hierarchy2 in employee search result list', 'N');

	INSERT INTO SystemParameter
		(ParameterCode, ParameterDesc, ParameterValue)
		VALUES
		('EMP_LIST_SHOW_H3', 'Show/Hide Hierarchy3 in employee search result list', 'N');

	ALTER TABLE UploadEmpRecurringPayment
		ADD EmpRPBasicSalary decimal(15, 4) NULL,
			EmpRPFPS decimal(15, 4) NULL,
			EmpRPOTCAmount decimal(15, 4) NULL;


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.17b'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.17b';
	
	COMMIT TRANSACTION
END

