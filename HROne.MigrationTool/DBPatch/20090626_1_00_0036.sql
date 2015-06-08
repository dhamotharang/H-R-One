DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.00.0029' 
BEGIN

	--	Add Employment Type
	CREATE TABLE Tmp_EmpPositionInfo
		(
		EmpPosID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpPosEffFr datetime NULL,
		EmpPosEffTo datetime NULL,
		CompanyID int NULL,
		PositionID int NULL,
		RankID int NULL,
		StaffTypeID int NULL,
		EmploymentTypeID int NULL,
		PayGroupID int NULL,
		LeavePlanID int NULL,
		YEBPlanID int NULL,
		EmpFirstAuthorizationGp int NULL,
		EmpSecondAuthorizationGp int NULL
		)

	SET IDENTITY_INSERT Tmp_EmpPositionInfo ON

	IF EXISTS(SELECT * FROM EmpPositionInfo)
		 EXEC('INSERT INTO Tmp_EmpPositionInfo (EmpPosID, EmpID, EmpPosEffFr, EmpPosEffTo, CompanyID, PositionID, RankID, StaffTypeID, PayGroupID, LeavePlanID, EmpFirstAuthorizationGp, EmpSecondAuthorizationGp)
			SELECT EmpPosID, EmpID, EmpPosEffFr, EmpPosEffTo, CompanyID, PositionID, RankID, StaffTypeID, PayGroupID, LeavePlanID, EmpFirstAuthorizationGp, EmpSecondAuthorizationGp FROM EmpPositionInfo WITH (HOLDLOCK TABLOCKX)')

	SET IDENTITY_INSERT Tmp_EmpPositionInfo OFF

	DROP TABLE EmpPositionInfo

	EXECUTE sp_rename N'Tmp_EmpPositionInfo', N'EmpPositionInfo', 'OBJECT' 

	ALTER TABLE EmpPositionInfo ADD CONSTRAINT
		PK_EmpPositionInfo PRIMARY KEY CLUSTERED 
		(
		EmpPosID
		)

	ALTER TABLE EmpDependant ADD
		EmpDependantDateOfBirth datetime NULL
	
	ALTER TABLE EmpSpouse ADD
		EmpSpouseDateOfBirth datetime NULL

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.00.0036'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

