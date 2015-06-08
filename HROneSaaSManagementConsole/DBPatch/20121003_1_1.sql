

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.0' 
Begin

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
    )
    VALUES
           ('ADM007','Bank Key Management')

	INSERT INTO UserFunction (UserID, FunctionID)
	SELECT usr.UserID, sf.FunctionID
	FROM Users usr, SystemFunction sf
	WHERE NOT EXISTS
	(	
		SELECT * 
		FROM UserFunction ugf
		WHERE ugf.UserID=usr.UserID
		AND ugf.FunctionID=sf.FunctionID
	)
	AND usr.UserID=1
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.1'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



