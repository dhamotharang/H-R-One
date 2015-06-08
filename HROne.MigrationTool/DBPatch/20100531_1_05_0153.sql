
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.05.0143' 
BEGIN
	ALTER TABLE UserReminderOption ADD
		UserReminderOptionRemindDaysBefore int NULL,
		UserReminderOptionRemindDaysAfter int NULL
		
	
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.05.0153'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





