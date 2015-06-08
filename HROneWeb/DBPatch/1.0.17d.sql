DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.17c'
BEGIN
	BEGIN TRANSACTION 
	
	ALTER TABLE TaxEmp ADD
		TaxEmpSumWithheldAmount NVARCHAR(20) NULL, 
		TaxEmpSumWithheldIndicator INT NULL;
		
 	ALTER TABLE EmpPersonalInfo ADD
		EmpOriginalHireDate datetime NULL;

	ALTER TABLE UploadEmpPersonalInfo ADD
		EmpOriginalHireDate datetime NULL;		

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.17d'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.17d';
	
	COMMIT TRANSACTION
END

