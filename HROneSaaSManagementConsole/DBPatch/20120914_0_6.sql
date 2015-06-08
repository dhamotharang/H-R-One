

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.5' 
Begin

	CREATE TABLE StatutoryHoliday
	(
		StatutoryHolidayID int NOT NULL IDENTITY (1, 1),
		StatutoryHolidayDate DATETIME NULL,
		StatutoryHolidayDesc NVARCHAR(100) NULL,
		CONSTRAINT PK_StatutoryHoliday PRIMARY KEY CLUSTERED 
		(
			StatutoryHolidayID
		) 
	)

	CREATE TABLE PublicHoliday
	(
		PublicHolidayID int IDENTITY(1,1) NOT NULL,
		PublicHolidayDate DATETIME NULL,
		PublicHolidayDesc NVARCHAR(100) NULL,
		CONSTRAINT PK_PublicHoliday PRIMARY KEY CLUSTERED 
		(
			PublicHolidayID
		) 
	)

	Create Table HSBCBankPaymentCode
	(
		HSBCBankPaymentCodeID int IDENTITY(1,1) NOT NULL,
		CompanyDBID INT NULL,
		HSBCBankPaymentCodeBankAccountNo NVARCHAR(255) NULL,
		HSBCBankPaymentCode NVARCHAR(255) NULL,
		HSBCBankPaymentCodeAutoPayInOutFlag NVARCHAR(1) NULL,
		CONSTRAINT PK_HSBCBankPaymentCode PRIMARY KEY CLUSTERED 
		(
			HSBCBankPaymentCodeID
		) 
	)	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.6'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



