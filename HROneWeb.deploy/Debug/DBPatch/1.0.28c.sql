
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.28b'
BEGIN

	BEGIN TRANSACTION 

	update EmpPersonalInfo
	set EmppassportIssuedCountry = '', EmppassportIssuedCountryID = 0
	where EmppassportIssuedCountry is null ;

	update RequestEmpPersonalInfo 
	set RequestEmppassportIssuedCountry = '', RequestEmppassportIssuedCountryID = 0
	where RequestEmppassportIssuedCountry is null ;

	UPDATE EmpPersonalInfo
	SET empNationality = '',
	 empNationalityID = 0
	WHERE empNationality IS NULL;

	UPDATE RequestEmpPersonalInfo
	SET RequestEmpNationality = '',
	 RequestEmpNationalityID = 0
	WHERE RequestEmpNationality IS NULL;

	UPDATE EmpPersonalInfo
	SET EmpPlaceOfBirth = '',
	 EmpPlaceOfBirthid = 0
	WHERE EmpPlaceOfBirth IS NULL;

	UPDATE RequestEmpPersonalInfo
	SET RequestEmpPlaceOfBirth = '',
	 RequestEmpPlaceOfBirthid = 0
	WHERE RequestEmpPlaceOfBirth IS NULL;
	
	
	If exists (select 1  
				from  sysobjects  
			   where  id = object_id('rb_sample'))  
		drop view rb_sample;

	

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.28c'
	WHERE	ParameterCode='DBVERSION';
	

	SELECT  @DBVERSION='1.0.28c';
	
	COMMIT TRANSACTION
END

