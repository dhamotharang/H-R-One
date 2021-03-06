
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.0.481' 
BEGIN
	
	UPDATE PaymentRecord
		SET PayRecStatus='A'
		WHERE PayRecStatus IS NULL
		OR PayRecStatus='' 
	
	ALTER TABLE PayrollGroup DROP COLUMN
		PayGroupLeaveDefaultFirstCutOffDay
	ALTER TABLE PayrollGroup DROP COLUMN
		PayGroupLeaveDefaultNextCutOffDay
		
	ALTER TABLE PayrollGroup ADD 
		PayGroupDefaultNextStartDay int NULL
	ALTER TABLE PayrollGroup ADD 
		PayGroupLeaveDefaultCutOffDay int NULL
	ALTER TABLE PayrollGroup ADD 
		PayGroupLeaveDefaultNextCutOffDay int NULL
           
		
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.0.483'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





