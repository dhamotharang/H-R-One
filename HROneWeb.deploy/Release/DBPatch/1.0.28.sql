
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.27'
BEGIN

	BEGIN TRANSACTION 

	-- 000164
	INSERT INTO SystemFunction values('SYS025', 'Shift Duty Code', 'System', 0);

	CREATE TABLE ShiftDutyCode
	(
		ShiftDutyCodeID int NOT NULL IDENTITY (1, 1),
		ShiftDutyCode nvarchar(255) NOT NULL,
		ShiftDutyCodeDesc nvarchar(255) NULL,	
		ShiftDutyFromTime datetime NOT NULL,
		ShiftDutyToTime datetime NOT NULL,

		CONSTRAINT PK_ShiftDutyCode PRIMARY KEY CLUSTERED 
		(
			ShiftDutyCodeID
		) 
	);

	-- 000165
	INSERT INTO SystemFunction values('SYS026', 'Payment Calculation Formula', 'System', 0);

	CREATE TABLE PaymentCalculationFormula
	(
		PayCalFormulaID int NOT NULL IDENTITY (1, 1),
		PayCalFormulaCode  nvarchar(255) NOT NULL,
		PayCalFormulaCodeDesc  nvarchar(255) NULL,	

		CONSTRAINT PK_PaymentCalculationFormula PRIMARY KEY CLUSTERED 
		(
			PayCalFormulaID
		) 
	);

	-- 000177
	ALTER TABLE ORSOPlan
		ADD ORSOFirstContributionAfterCompleteMonth int NULL;


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.28'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.28';
	
	COMMIT TRANSACTION
END

