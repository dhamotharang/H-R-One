

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.2' 
Begin

	CREATE TABLE HSBCExchangeProfile 
	(
		HSBCExchangeProfileID int NOT NULL IDENTITY (1, 1),
		CompanyDBID INT NULL,
		HSBCExchangeProfileRemoteProfileID NVARCHAR(255) NULL,
		CompanyDBClientBank NVARCHAR(100) NULL
		CONSTRAINT PK_HSBCExchangeProfile PRIMARY KEY CLUSTERED 
		(
			HSBCExchangeProfileID
		)
	)
	
	ALTER TABLE HSBCBankPaymentCode ADD
		HSBCExchangeProfileID int NULL 

	ALTER TABLE CompanyMPFFile ADD
		HSBCExchangeProfileID int NULL 

	ALTER TABLE CompanyAutopayFile ADD
		HSBCExchangeProfileID int NULL 

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.3'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



