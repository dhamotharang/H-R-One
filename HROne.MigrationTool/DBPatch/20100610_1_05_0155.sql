
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.05.0154' 
BEGIN
     
	CREATE TABLE UserCompany
	(
		UserCompanyID int IDENTITY(1,1) NOT NULL,
		UserID int NULL,
		CompanyID int NULL,
		CONSTRAINT PK_UserCompany PRIMARY KEY CLUSTERED 
		(
			UserCompanyID
		)
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.05.0155'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





