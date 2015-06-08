DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0071' 
BEGIN

	Alter table RosterCode Add 
		RosterCodeCountWorkHourOnly int,
		RosterCodeCountOTAfterWorkHourMin int,
		RosterCodeOTMinsUnit int,
		RosterCodeOTMinsRoundingRule nvarchar(50)
	
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0072'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





