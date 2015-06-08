CREATE TABLE MPFCalculationOverride
(
	MPFCalculationOverrideID int IDENTITY(1,1) NOT NULL,
	EmpNo nvarchar(20) NOT NULL,
	MPFCalculationOverridePeriodFr datetime NOT NULL,
	MPFCalculationOverridePeriodTo datetime NOT NULL,
	MPFCalculationOverrideMCRI decimal(15, 4) NULL,
	MPFCalculationOverrideMCER decimal(15, 4) NULL,
	MPFCalculationOverrideMCEE decimal(15, 4) NULL,
	MPFCalculationOverrideVCRI decimal(15, 4) NULL,
	MPFCalculationOverrideVCER decimal(15, 4) NULL,
	MPFCalculationOverrideVCEE decimal(15, 4) NULL,
	CONSTRAINT PK_MPFCalculationOverride PRIMARY KEY CLUSTERED 
	(
		MPFCalculationOverrideID ASC
	)
)