
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.20c'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE AVCPlan ADD 
		AVCPlanEmployerResidualCAP decimal(15,2),
		AVCPlanEmployeeResidualCAP decimal(15,2);
	
	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.21'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.21';
	
	COMMIT TRANSACTION
END

