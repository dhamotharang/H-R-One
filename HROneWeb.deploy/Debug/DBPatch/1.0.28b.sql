
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.28a'
BEGIN

	BEGIN TRANSACTION 

	if exists (select 1  
				from  sysobjects  
			   where  id = object_id('ReportBuilder'))  
	   drop table ReportBuilder;

	if exists (select 1  
				from  sysobjects  
			   where  id = object_id('ReportBuilderField'))  
	   drop table ReportBuilderField;

	CREATE TABLE ReportBuilder
	(
		ReportBuilderID int IDENTITY(1,1) NOT NULL,
		ReportModule varchar(200) NOT NULL,
		SelectedFieldNames varchar(500) NOT NULL,
		DBFilterExpressions varchar(255) NOT NULL,
		ReportName varchar(255) NOT NULL,
		CONSTRAINT PK_ReportBuilder PRIMARY KEY CLUSTERED 
		(
			ReportBuilderID ASC
		)
	);	



	INSERT INTO SYSTEMFUNCTION
		(FUNCTIONCODE, DESCRIPTION, FunctionCategory, FUNCTIONISHIDDEN)
	VALUES
		('RPT004', 'Report Builder', 'System', -1);


	INSERT INTO SYSTEMFUNCTION
		(FUNCTIONCODE, DESCRIPTION, FunctionCategory, FUNCTIONISHIDDEN)
	VALUES
		('CUSTOM001', 'Kerry Customization: KTP Fund Report', 'Payroll & MPF Reports', -1);

	INSERT INTO SYSTEMFUNCTION
	VALUES
		('CUSTOM002', 'Winson Customization: Customized Attendance Preparation Process', 'Payroll', -1);
		

	UPDATE SystemFunction
	SET FunctionIsHidden=-1
	WHERE FunctionCode = 'SYS026';
	
	UPDATE SystemFunction
	SET FunctionIsHidden=-1
	WHERE FunctionCode = 'SYS025';



	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.28b'
	WHERE	ParameterCode='DBVERSION';
	

	SELECT  @DBVERSION='1.0.28b';
	
	COMMIT TRANSACTION
END

