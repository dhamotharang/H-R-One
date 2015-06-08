DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0080' 
BEGIN

	Insert into SystemParameter(ParameterCode, ParameterValue)
	VALUES
	('PAY_SLIP_HIDE_LEAVE_BALANCE','Y')

	
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0081'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





