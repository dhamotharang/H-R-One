
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.05.0161' 
BEGIN

	Alter Table LeavePlan ADD
		LeavePlanLeavePlanCompareRank int NULL
			
	Alter Table PayrollGroup ADD
		PayGroupStatHolNextMonth int NULL
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.05.0162'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





