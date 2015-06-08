DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.03.0089' 
BEGIN

	Alter Table EmpPositionInfo Add 
		EmpPosDefaultRosterCodeID int NULL
		
		
	
	Create Table RosterClient
	(
		RosterClientID INT NOT NULL IDENTITY(1,1),
		RosterClientCode NVARCHAR(20) NULL,
		RosterClientName NVARCHAR(100) NULL,
		Constraint PK_RosterClient PRIMARY KEY 
		(
			RosterClientID
		)
	)
	
	Create Table RosterClientSite
	(
		RosterClientSiteID INT NOT NULL IDENTITY(1,1),
		RosterClientID INT NULL,		
		RosterClientSiteCode NVARCHAR(20) NULL,
		RosterClientSitePropertyName NVARCHAR(100) NULL,
		RosterClientSiteLocation NVARCHAR(200) NULL,
		RosterClientSiteInCharge NVARCHAR(100) NULL,
		RosterClientSiteInChargeContactNo NVARCHAR(50) NULL,
		Constraint PK_RosterClientSite PRIMARY KEY 
		(
			RosterClientSiteID
		)
	)
	
	Update ReminderType 
	set ReminderTypeDesc='Birthday Reminder'
	where ReminderTypeCode='DOB'

		
	Alter Table RosterCode ADD
		RosterClientID int NULL,
		RosterClientSiteID int NULL,
		RosterCodeIsOverrideHourlyPayment int NULL,
		RosterCodeOverrideHoulyAmount decimal(15,4) NULL
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.03.0091'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





