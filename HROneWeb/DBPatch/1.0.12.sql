DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0.11'
BEGIN
	BEGIN TRANSACTION 
	
	UPDATE MPFScheme
	SET	 MPFSchemeDesc = 'Sun Life Rainbow MPF Scheme'
	WHERE MPFSchemeCode = 'MT00067';
	
		-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.12'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.12';
	
	COMMIT TRANSACTION
END
