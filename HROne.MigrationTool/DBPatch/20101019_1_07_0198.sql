
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0197' 
BEGIN

	Alter Table EmpExtraField
		ADD EmpExtraFieldGroupName nvarchar(50) null 

	Alter Table AttendancePlan
		ADD AttendancePlanBonusOTAmount real null
		
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0198'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





