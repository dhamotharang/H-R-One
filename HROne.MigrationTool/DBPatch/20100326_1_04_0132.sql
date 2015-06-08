
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.04.0130' 
BEGIN
	ALTER TABLE LeavePlan ADD
		ALProrataRoundingRuleID int NULL,
		LeavePlanResetYearOfService int NULL,
		LeavePlanNoCountFirstIncompleteYearOfService int NULL,
		LeavePlanComparePreviousLeavePlan int NULL

	CREATE TABLE ALProrataRoundingRule
	(
		ALProrataRoundingRuleID int NOT NULL IDENTITY (1, 1),
		ALProrataRoundingRuleCode nvarchar(200) NULL,
		ALProrataRoundingRuleDesc nvarchar(255) NULL
	)
	
	CREATE TABLE ALProrataRoundingRuleDetail
	(
		ALProrataRoundingRuleDetailID int NOT NULL IDENTITY (1, 1),
		ALProrataRoundingRuleID int NOT NULL,
		ALProrataRoundingRuleDetailRangeTo real NOT NULL,
		ALProrataRoundingRuleDetailRoundTo real NOT NULL,
	)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('LEV004','Annual Leave Prorata Rounding Rule Setup','Leave', 0)
           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.04.0132'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





