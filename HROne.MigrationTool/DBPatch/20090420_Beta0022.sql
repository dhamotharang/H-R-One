
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.00.0021' 
BEGIN

	CREATE TABLE CostCenter
	(
		CostCenterID int NOT NULL IDENTITY (1, 1),
		CostCenterCode nvarchar(20) NOT NULL,
		CostCenterDesc nvarchar(100) NULL,
		CONSTRAINT PK_CostCenter PRIMARY KEY CLUSTERED 
		(
			CostCenterCode
		)
	)  

	CREATE TABLE EmpCostCenter
	(
		EmpCostCenterID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpCostCenterEffFr datetime NULL,
		EmpCostCenterEffTo datetime NULL,
		CONSTRAINT PK_EmpCostCenter PRIMARY KEY CLUSTERED 
		(
			EmpCostCenterID
		) 
	)

	CREATE TABLE EmpCostCenterDetail
	(
		EmpCostCenterDetailID int NOT NULL IDENTITY (1, 1),
		EmpCostCenterID int NULL,
		CostCenterID int NULL,
		EmpCostCenterPercentage real NULL,
		EmpCostCenterDetailIsDefault int NULL,
		CONSTRAINT PK_EmpCostCenterDetail PRIMARY KEY CLUSTERED 
		(
			EmpCostCenterDetailID
		)
	)

	CREATE TABLE CostAllocation
	(
		CostAllocationID int NOT NULL IDENTITY (1, 1),
		EmpID int NULL,
		EmpPayrollID int NULL,
		CostAllocationStatus nvarchar(1) NULL,
		CostAllocationTrialRunDate datetime NULL,
		CostAllocationTrialRunBy int NULL,
		CostAllocationConfirmDate datetime NULL,
		CostAllocationConfirmBy int NULL,
		CONSTRAINT PK_CostAllocation PRIMARY KEY CLUSTERED 
		(
			CostAllocationID
		) 
	)

	CREATE TABLE CostAllocationDetail
	(
		CostAllocationDetailID int NOT NULL IDENTITY (1, 1),
		CostAllocationID int NULL,
		CompanyID int NULL,
		CostCenterID int NULL,
		PaymentCodeID int NULL,
		CostAllocationDetailAmount decimal(15, 4) NULL,
		PayRecID int NULL,
		CONSTRAINT PK_CostAllocationDetail PRIMARY KEY CLUSTERED 
		(
			CostAllocationDetailID
		)
	)
	Create Table CostAllocationDetailHElement
	(
		CostAllocationDetailHElementID int NOT NULL IDENTITY (1,1),
		CostAllocationDetailID int NULL,
		HElementID int NULL,
		HLevelID int NULL,
		CONSTRAINT PK_CostAllocationDetailHElement PRIMARY KEY CLUSTERED 
		(
			CostAllocationDetailHElementID
		)
	)
	
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.00.0022'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);

