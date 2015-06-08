DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0042' 
BEGIN

	ALTER TABLE PaymentCode ADD
		PaymentCodeHideInPaySlip int NULL

	Alter Table Position ADD
		PositionCapacity nvarchar(40) NULL
		
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0043'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

