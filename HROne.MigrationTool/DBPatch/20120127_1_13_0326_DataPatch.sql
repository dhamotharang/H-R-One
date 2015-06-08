UPDATE LeavePlan
SET LeavePlanUseCommonLeaveYear=1
WHERE LeavePlanUseCommonLeaveYear IS NULL
AND EXISTS 
(
	SELECT * 
	FROM SystemParameter 
	WHERE ParameterCode='LEAVE_ENTITLE_BY_SERVICE_DATE'
	AND ParameterValue='N'
)

UPDATE LeavePlan
SET LeavePlanUseCommonLeaveYear=0
WHERE LeavePlanUseCommonLeaveYear IS NULL

UPDATE LeavePlan
SET LeavePlanCommonLeaveYearStartMonth=(SELECT ParameterValue FROM SystemParameter WHERE ParameterCode='LEAVE_ENTITLE_MONTH')
WHERE LeavePlanCommonLeaveYearStartMonth IS NULL

UPDATE LeavePlan
SET LeavePlanCommonLeaveYearStartDay=(SELECT ParameterValue FROM SystemParameter WHERE ParameterCode='LEAVE_ENTITLE_DAY')
WHERE LeavePlanCommonLeaveYearStartDay IS NULL
