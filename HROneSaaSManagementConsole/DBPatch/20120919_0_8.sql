

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.7' 
Begin

	CREATE TABLE SystemFunction
	(
		FunctionID int NOT NULL IDENTITY (1, 1),
		FunctionCode nvarchar(10) NULL,
		Description nvarchar(100) NULL,
		CONSTRAINT PK_SystemFunction PRIMARY KEY CLUSTERED 
		(
			FunctionID
		)
	)

	CREATE TABLE UserFunction
	(
		UserFunctionID int NOT NULL IDENTITY (1, 1),
		UserID int NULL,
		FunctionID int NULL
		CONSTRAINT PK_UserFunction PRIMARY KEY CLUSTERED 
		(
			UserFunctionID
		) 
	)
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.8'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



