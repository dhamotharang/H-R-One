
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.07.0202' 
BEGIN

	UPDATE ATTENDANCEPLAN
		set AttendancePlanBonusOTAmount=0
		where AttendancePlanBonusOTAmount is null
	
	Alter table RosterCode Add 
		RosterCodeUseHalfWorkingDaysHours int NULL,
		RosterCodeUseHalfWorkingDaysHoursMaxWorkingHours real NULL 
		
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.07.0203'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





