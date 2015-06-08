

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.4' 
Begin
	
	INSERT INTO HSBCExchangeProfile(CompanyDBID, HSBCExchangeProfileRemoteProfileID, HSBCExchangeProfileBankCode, HSBCExchangeProfileIsLocked)
		SELECT CompanyDBID, CompanyDBClientCode, CompanyDBClientBank, 0
		FROM CompanyDatabase
	
	UPDATE HSBCBankPaymentCode
	SET HSBCExchangeProfileID=hep.HSBCExchangeProfileID
	FROM HSBCExchangeProfile hep
	WHERE hep.CompanyDBID=HSBCBankPaymentCode.CompanyDBID

	UPDATE CompanyMPFFile
	SET HSBCExchangeProfileID=hep.HSBCExchangeProfileID
	FROM HSBCExchangeProfile hep
	WHERE hep.CompanyDBID=CompanyMPFFile.CompanyDBID

	UPDATE CompanyAutopayFile
	SET HSBCExchangeProfileID=hep.HSBCExchangeProfileID
	FROM HSBCExchangeProfile hep
	WHERE hep.CompanyDBID=CompanyAutopayFile.CompanyDBID


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.5'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



