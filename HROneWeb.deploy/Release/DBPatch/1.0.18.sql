
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.17f'
BEGIN
	BEGIN TRANSACTION 

	ALTER TABLE EmpRequest ADD
		EmpRequestFromDate datetime NULL, 
		EmpRequestToDate datetime NULL, 
		EmpRequestDuration nvarchar(50) NULL;

	ALTER TABLE ORSOPlan ADD 
		ORSOPlanEmployerResidual int NULL, 
		ORSOPlanEmployerResidualCAP decimal(15,2),
		ORSOPlanEmployeeResidual int NULL,
		ORSOPlanEmployeeResidualCAP decimal(15,2);

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.18'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.18';
	
	COMMIT TRANSACTION
END

