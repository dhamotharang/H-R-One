	UPDATE LeaveApplication
	Set LeaveAppHours=0
	WHERE LeaveAppHours is NULL

	UPDATE RequestLeaveApplication
	Set RequestLeaveAppHours=0
	WHERE RequestLeaveAppHours is NULL
