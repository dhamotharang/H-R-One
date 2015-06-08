	UPDATE LeaveBalanceAdjustment 
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

	UPDATE CompensationLeaveEntitle 
	SET SynID=NewID()
	WHERE SynID IS NULL
	OR SynID=''

