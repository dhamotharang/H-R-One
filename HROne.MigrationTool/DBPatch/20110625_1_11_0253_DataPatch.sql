	UPDATE LeaveCode
	Set LeaveCodeHideInESS=0
	WHERE LeaveCodeHideInESS is NULL

	UPDATE AttendancePlan
	Set AttendancePlanOTRateMultiplier=1
	WHERE AttendancePlanOTRateMultiplier is NULL
