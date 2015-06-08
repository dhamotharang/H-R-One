DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.10' 
BEGIN
	BEGIN TRANSACTION 
	
	CREATE NONCLUSTERED INDEX IX_CostAllocation1 
	ON CostAllocation (EmpPayrollID ASC);
	
		-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.11'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.11';
	
	COMMIT TRANSACTION
END
