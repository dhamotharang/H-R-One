DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0067' 
BEGIN
		
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT001','Roster Code Setup','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT002','Attendance Formula Setup','Attendance')
	
	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT003','Attendance Plan Setup','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT004','Export Roster Table','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT005','Import Roster Table','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT006','Import Time Card Record','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT007','Generate Attendance Record','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT008','Attendance Record Adjustment','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT009','Export Attendance Records','Attendance')           

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT010','Import Attendance Records','Attendance')

	INSERT INTO SystemFunction
           (FunctionCode
           ,Description
           ,FunctionCategory)
     VALUES
           ('ATT011','Generate Attendance Payment','Attendance')
           

	Alter Table YEBPlan Add
		YEBPlanIsGlobal int Null
		           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0068'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




