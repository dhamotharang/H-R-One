
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.31a'
BEGIN

	BEGIN TRANSACTION 

	alter table UploadEmpDependant add EmpDependantMedicalSchemeInsured int;
	alter table UploadEmpDependant add EmpDependantMedicalEffectiveDate datetime;
	alter table UploadEmpDependant add EmpDependantExpiryDate datetime;



	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.31b'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.31b';
	
	COMMIT TRANSACTION
END

