	UPDATE EmpPersonalInfo 
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpPositionInfo 
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpRecurringPayment 
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpTermination 
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE LeaveApplication 
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpAVCPlan  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpBankAccount  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpContractTerms  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpDependant  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpEmergencyContact  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpFinalPayment  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpMPFPlan  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpORSOPlan  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpPermit  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpPlaceOfResidence  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpQualification  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpRosterTableGroup  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpCostCenter  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpSkill  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpSpouse  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpWorkExp  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE EmpWorkInjuryRecord  
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''
