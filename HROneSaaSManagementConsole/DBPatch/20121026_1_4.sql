

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.3' 
Begin
	
	DROP TABLE HSBCExchangeProfile
	
	CREATE TABLE HSBCExchangeProfile 
	(
		HSBCExchangeProfileID int NOT NULL IDENTITY (1, 1),
		CompanyDBID INT NULL,
		HSBCExchangeProfileRemoteProfileID NVARCHAR(255) NULL,
		HSBCExchangeProfileBankCode NVARCHAR(100) NULL,
		HSBCExchangeProfileIsLocked INT NULL
		CONSTRAINT PK_HSBCExchangeProfile PRIMARY KEY CLUSTERED 
		(
			HSBCExchangeProfileID
		)
	)
	


	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.4'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



