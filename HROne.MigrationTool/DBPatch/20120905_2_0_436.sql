
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.0.433' 
BEGIN

	INSERT INTO MPFScheme (MPFSchemeCode, MPFSchemeDesc, MPFSchemeTrusteeCode)
	VALUES ('MT00512', 'HSBC Mandatory Provident Fund - SimpleChoice', 'HSBC')
	
	INSERT INTO MPFScheme (MPFSchemeCode, MPFSchemeDesc, MPFSchemeTrusteeCode)
	VALUES ('MT00520', 'Hang Seng Mandatory Provident Fund - SimpleChoice', 'HangSeng')

	INSERT INTO MPFScheme (MPFSchemeCode, MPFSchemeDesc, MPFSchemeTrusteeCode)
	VALUES ('MT00563', 'Hang Seng Mandatory Provident Fund - ValueChoice', 'HangSeng')

	INSERT INTO BankList
           (BankCode
           ,BankName)
     VALUES
           ('014','BANK OF CHINA (HONG KONG) LIMITED')

	INSERT INTO BankList
           (BankCode
           ,BankName)
     VALUES
           ('019','BANK OF CHINA (HONG KONG) LIMITED')

	INSERT INTO BankList
           (BankCode
           ,BankName)
     VALUES
           ('026','BANK OF CHINA (HONG KONG) LIMITED')

	INSERT INTO BankList
           (BankCode
           ,BankName)
     VALUES
           ('030','BANK OF CHINA (HONG KONG) LIMITED')
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.0.436'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





