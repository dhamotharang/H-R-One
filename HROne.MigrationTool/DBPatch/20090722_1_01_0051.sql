DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.01.0050' 
BEGIN
		
	CREATE TABLE Tmp_ORSOPlan
	(
		ORSOPlanID int NOT NULL IDENTITY (1, 1),
		ORSOPlanCode nvarchar(20) NULL,
		ORSOPlanDesc nvarchar(100) NULL,
		ORSOPlanSchemeNo nvarchar(100) NULL,
		ORSOPlanCompanyName nvarchar(100) NULL,
		ORSOPlanPayCenter nvarchar(20) NULL,
		ORSOPlanMaxEmployerVC decimal(15, 2) NULL,
		ORSOPlanMaxEmployeeVC decimal(15, 2) NULL
	)
	SET IDENTITY_INSERT Tmp_ORSOPlan ON
	IF EXISTS(SELECT * FROM ORSOPlan)
		 EXEC('INSERT INTO Tmp_ORSOPlan (ORSOPlanID, ORSOPlanCode, ORSOPlanDesc, ORSOPlanSchemeNo, ORSOPlanCompanyName, ORSOPlanPayCenter, ORSOPlanMaxEmployerVC, ORSOPlanMaxEmployeeVC)
			SELECT ORSOPlanID, ORSOPlanCode, ORSOPlanDesc, ORSOPlanSchemeNo, ORSOPlanCompanyName, ORSOPlanPayCenter, ORSOPlanMaxEmployerVC, CONVERT(decimal(15, 2), ORSOPlanMaxEmployeeVC) FROM ORSOPlan WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Tmp_ORSOPlan OFF
	DROP TABLE ORSOPlan
	EXECUTE sp_rename N'Tmp_ORSOPlan', N'ORSOPlan', 'OBJECT' 
	ALTER TABLE ORSOPlan ADD CONSTRAINT
		PK_ORSOPlan PRIMARY KEY CLUSTERED 
		(
		ORSOPlanID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]



	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.01.0051'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);




