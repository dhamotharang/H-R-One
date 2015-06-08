DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.17d'
BEGIN
	BEGIN TRANSACTION 

	INSERT INTO TaxPayment VALUES('M', 'Type 1', 'Subcontracting Fees', '', 'N');
	INSERT INTO TaxPayment VALUES('M', 'Type 2', 'Commission', '', 'N');
	INSERT INTO TaxPayment VALUES('M', 'Type 3', 'Writer’s / Contributor’s Fees', '', 'N');
	INSERT INTO TaxPayment VALUES('M', 'Others (a)', 'Artiste’s Fees', '', 'N');
	INSERT INTO TaxPayment VALUES('M', 'Others (b)', 'Copyright / Royalties', '', 'N');
	INSERT INTO TaxPayment VALUES('M', 'Others (c)', 'Consultancy / Management Fees', '', 'N');
	INSERT INTO TaxPayment VALUES('M', 'Others (d)', '', '', 'N');
	INSERT INTO TaxPayment VALUES('M', 'Others (e)', '', '', 'N');	

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.17e'
	WHERE	ParameterCode='DBVERSION';

	SELECT  @DBVERSION='1.0.17e';
	
	COMMIT TRANSACTION
END

