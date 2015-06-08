
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.17e'
BEGIN
	BEGIN TRANSACTION 

	UPDATE TaxPayment set TaxPayIsShowNature = 'Y' where TaxFormType = 'M' and TaxPayCode = 'Others (d)';
	UPDATE TaxPayment set TaxPayIsShowNature = 'Y' where TaxFormType = 'M' and TaxPayCode = 'Others (e)';


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.17f'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.17f';
	
	COMMIT TRANSACTION
END
