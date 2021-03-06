
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.13.0364' 
BEGIN


	Create Table SystemFunctionEmailAlert
	(
		SystemFunctionEmailAlertID INT NOT NULL IDENTITY (1, 1),
		FunctionID INT NULL,
		SystemFunctionEmailAlertInsert INT NULL,
		SystemFunctionEmailAlertUpdate INT NULL,
		SystemFunctionEmailAlertDelete INT NULL,
		CONSTRAINT PK_SystemFunctionEmailAlert PRIMARY KEY CLUSTERED 
		(
			SystemFunctionEmailAlertID
		) 
	)
	
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.14.368'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





