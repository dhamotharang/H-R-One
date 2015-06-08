
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.19b'
BEGIN
	BEGIN TRANSACTION 

	-- Add Upload Employee Benefit --
	CREATE TABLE [UploadEmpBenefit](
		[UploadEmpBenefitID] [int] IDENTITY(1,1) NOT NULL,
		[UploadEmpID] [int] NULL,
		[EmpBenefitID] [int] NULL,
		[EmpID] [int] NULL,
		[EmpBenefitEffectiveDate] [datetime] NULL,
		[EmpBenefitExpiryDate] [datetime] NULL,
		[EmpBenefitPlanID] [int] NULL,
		[EmpBenefitERPremium] [decimal](15, 4) NULL,
		[EmpBenefitEEPremium] [decimal](15, 4) NULL,
		[EmpBenefitChildPremium] [decimal](15, 4) NULL,
		[EmpBenefitSpousePremium] [decimal](15, 4) NULL,
		[SynID] [nvarchar](255) NULL,
		[SessionID] [nvarchar](200) NOT NULL,
		[TransactionDate] [datetime] NOT NULL,
		[ImportAction] [nvarchar](100) NULL,
		[ModifiedFieldList] [ntext] NULL,
	 CONSTRAINT [PK_UploadEmpBenefit] PRIMARY KEY CLUSTERED 
	(
		[UploadEmpBenefitID] ASC
	)
	) ON [PRIMARY];

	-- Add Upload Employee Beneficiaries --
	CREATE TABLE [UploadEmpBeneficiaries](
		[UploadEmpBeneficiariesID] [int] IDENTITY(1,1) NOT NULL,
		[UploadEmpID] [int] NULL,
		[EmpBeneficiariesID] [int] NULL,
		[EmpID] [int] NULL,
		[EmpBeneficiariesName] [nvarchar](255) NULL,
		[EmpBeneficiariesShare] [decimal](5, 2) NULL,
		[EmpBeneficiariesHKID] [nvarchar](50) NULL,
		[EmpBeneficiariesRelation] [nvarchar](255) NULL,
		[EmpBeneficiariesAddress] [nvarchar](250) NULL,
		[EmpBeneficiariesDistrict] [nvarchar](150) NULL,
		[EmpBeneficiariesArea] [nvarchar](100) NULL,
		[EmpBeneficiariesCountry] [nvarchar](255) NULL,
		[SynID] [nvarchar](255) NULL,
		[SessionID] [nvarchar](200) NOT NULL,
		[TransactionDate] [datetime] NOT NULL,
		[ImportAction] [nvarchar](100) NULL,
		[ModifiedFieldList] [ntext] NULL,
	 CONSTRAINT [PK_UploadEmpBeneficiaries] PRIMARY KEY CLUSTERED 
	(
		[UploadEmpBeneficiariesID] ASC
	)
	) ON [PRIMARY];
	
	IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = 'EmpPersonalInfo' 
			   AND  COLUMN_NAME = 'EmpOriginalHireDate')
	BEGIN
		ALTER TABLE [EmpPersonalInfo] ADD
				[EmpOriginalHireDate] [datetime] NULL;
	END

	IF NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = 'UploadEmpPersonalInfo' 
			   AND  COLUMN_NAME = 'EmpOriginalHireDate')
	BEGIN
		ALTER TABLE [UploadEmpPersonalInfo] ADD
				[EmpOriginalHireDate] [datetime] NULL;
	END


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.19c'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.19c';
	
	COMMIT TRANSACTION
END

