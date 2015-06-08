DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0087' 
BEGIN

	Create Table EmpUniform
	(
		EmpUniformID int NOT NULL IDENTITY(1,1),
		EmpID int NULL,
		EmpUniformB nvarchar(10) NULL,
		EmpUniformW nvarchar(10) NULL,
		EmpUniformH nvarchar(10) NULL,
		CONSTRAINT PK_EmpUniform PRIMARY KEY CLUSTERED 
		(
		EmpUniformID
		) 
	)
	
	Create Table EmpWorkExp
	(
		EmpWorkExpID int NOT NULL IDENTITY(1,1),
		EmpID int NULL,
		EmpWorkExpFromYear int NULL,
		EmpWorkExpFromMonth int NULL,
		EmpWorkExpToYear int NULL,
		EmpWorkExpToMonth int NULL,
		EmpWorkExpCompanyName nvarchar(100) NULL,
		EmpWorkExpPosition nvarchar(100) NULL,
		EmpWorkExpRemark NTEXT NULL,
		CONSTRAINT PK_EmpWorkExp PRIMARY KEY CLUSTERED 
		(
			EmpWorkExpID
		) 
	)
	
	Insert into SystemParameter (ParameterCode,ParameterValue)
		Values ('MAX_MONTHLY_LSPSP_AMOUNT', '22500')
	Insert into SystemParameter (ParameterCode,ParameterValue)
		Values ('MAX_TOTAL_LSPSP_AMOUNT', '390000')
 
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0088'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





