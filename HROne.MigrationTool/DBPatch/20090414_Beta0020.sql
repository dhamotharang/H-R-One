

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.00.0019' 
Begin
ALTER TABLE dbo.AuthorizationGroup ADD
	AuthorizationGroupIsApproveEEInfo int NULL,
	AuthorizationGroupIsApproveLeave int NULL,
	AuthorizationGroupIsReceiveOtherGrpAlert int NULL

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.00.0020'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



