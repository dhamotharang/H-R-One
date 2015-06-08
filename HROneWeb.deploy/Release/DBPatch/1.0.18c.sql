
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.18b'
BEGIN
	BEGIN TRANSACTION 

	-- Add new System Functions --
	INSERT INTO [SystemFunction]
			   ([FunctionCode]
			   ,[Description]
			   ,[FunctionCategory]
			   ,[FunctionIsHidden])
		 VALUES
			   ('SYS022','Benefit Plan Setup','System', 0);

	INSERT INTO [SystemFunction]
			   ([FunctionCode]
			   ,[Description]
			   ,[FunctionCategory]
			   ,[FunctionIsHidden])
		 VALUES
			   ('PER020','Employee Benefit','Personnel', 0);

	INSERT INTO [SystemFunction]
			   ([FunctionCode]
			   ,[Description]
			   ,[FunctionCategory]
			   ,[FunctionIsHidden])
		 VALUES
			   ('PER021','Employee Beneficiaries','Personnel', 0);


	-- Add Benefit Plan Setup --
	CREATE TABLE [BenefitPlan](
		[BenefitPlanID] [int] IDENTITY(1,1) NOT NULL,
		[BenefitPlanCode] [nvarchar](20) NULL,
		[BenefitPlanDesc] [nvarchar](100) NULL,
		[BenefitPlanERPaymentBaseMethod] [nvarchar](10) NULL,
		[BenefitPlanERPaymentCodeID] [int] NULL,
		[BenefitPlanERMultiplier] [nvarchar](50) NULL, 
		[BenefitPlanERAmount] [decimal](15, 4) NULL,
		[BenefitPlanEEPaymentBaseMethod] [nvarchar](10) NULL,
		[BenefitPlanEEPaymentCodeID] [int] NULL,
		[BenefitPlanEEMultiplier] [nvarchar](50) NULL, 
		[BenefitPlanEEAmount] [decimal](15, 4) NULL,
		[BenefitPlanSpousePaymentBaseMethod] [nvarchar](10) NULL,
		[BenefitPlanSpousePaymentCodeID] [int] NULL,
		[BenefitPlanSpouseMultiplier] [nvarchar](50) NULL, 
		[BenefitPlanSpouseAmount] [decimal](15, 4) NULL,
		[BenefitPlanChildPaymentBaseMethod] [nvarchar](10) NULL,
		[BenefitPlanChildPaymentCodeID] [int] NULL,
		[BenefitPlanChildMultiplier] [nvarchar](50) NULL, 
		[BenefitPlanChildAmount] [decimal](15, 4) NULL,
	 CONSTRAINT [PK_BenefitPlan] PRIMARY KEY CLUSTERED 
	(
		[BenefitPlanID] ASC
	)
	) ON [PRIMARY];


	-- Add Employee Benefit --
	CREATE TABLE [EmpBenefit](
		[EmpBenefitID] [int] IDENTITY(1,1) NOT NULL,
		[EmpID] [int] NULL,
		[EmpBenefitEffectiveDate] [datetime] NULL,
		[EmpBenefitExpiryDate] [datetime] NULL,
		[EmpBenefitPlanID] [int] NULL,
		[EmpBenefitERPremium] [decimal](15, 4) NULL,
		[EmpBenefitEEPremium] [decimal](15, 4) NULL,
		[EmpBenefitSpousePremium] [decimal](15, 4) NULL,
		[EmpBenefitChildPremium] [decimal](15, 4) NULL,
		[RecordCreatedDateTime] [datetime] NULL,
		[RecordCreatedBy] [int] NULL,
		[RecordLastModifiedDateTime] [datetime] NULL,
		[RecordLastModifiedBy] [int] NULL,
		[SynID] [nvarchar](255) NULL,
	 CONSTRAINT [PK_EmpBenefit] PRIMARY KEY CLUSTERED 
	(
		[EmpBenefitID] ASC
	)
	) ON [PRIMARY];


	-- Add Employee Beneficiaries --
	CREATE TABLE [EmpBeneficiaries](
		[EmpBeneficiariesID] [int] IDENTITY(1,1) NOT NULL,
		[EmpID] [int] NULL,
		[EmpBeneficiariesName] [nvarchar](255) NULL,
		[EmpBeneficiariesShare] [decimal](5, 2) NULL,
		[EmpBeneficiariesHKID] [nvarchar](50) NULL,
		[EmpBeneficiariesRelation] [nvarchar](255) NULL,
		[EmpBeneficiariesAddress] [nvarchar](250) NULL,
		[EmpBeneficiariesDistrict] [nvarchar](150) NULL,
		[EmpBeneficiariesArea] [nvarchar](100) NULL,
		[EmpBeneficiariesCountry] [nvarchar](255) NULL,
		[RecordCreatedDateTime] [datetime] NULL,
		[RecordCreatedBy] [int] NULL,
		[RecordLastModifiedDateTime] [datetime] NULL,
		[RecordLastModifiedBy] [int] NULL,
		[SynID] [nvarchar](255) NULL,
	 CONSTRAINT [PK_EmpBeneficiaries] PRIMARY KEY CLUSTERED 
	(
		[EmpBeneficiariesID] ASC
	)
	) ON [PRIMARY];



	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.18c'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.18c';
	
	COMMIT TRANSACTION
END

