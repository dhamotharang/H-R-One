DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0051' 
BEGIN
		
	CREATE TABLE BankList
	(
		BankCode nvarchar(3) NOT NULL,
		BankName nvarchar(200) NULL,
		CONSTRAINT PK_BankList PRIMARY KEY CLUSTERED 
		(
			BankCode
		)
	) 
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0052'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




