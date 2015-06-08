Insert into WorkHourPattern
(
	WorkHourPatternCode, 
	WorkHourPatternDesc, 
	WorkHourPatternWorkDayDetermineMethod,
	WorkHourPatternSunDefaultRosterCodeID,
	WorkHourPatternMonDefaultRosterCodeID,
	WorkHourPatternTueDefaultRosterCodeID,
	WorkHourPatternWedDefaultRosterCodeID,
	WorkHourPatternThuDefaultRosterCodeID,
	WorkHourPatternFriDefaultRosterCodeID,
	WorkHourPatternSatDefaultRosterCodeID
)
Select 
	RosterCode, 
	RosterCodeDesc, 
	'R', 
	RosterCodeID, 
	RosterCodeID, 
	RosterCodeID, 
	RosterCodeID, 
	RosterCodeID, 
	RosterCodeID, 
	RosterCodeID
FROM RosterCode
WHERE RosterCodeID IN (SELECT EmpPosDefaultRosterCodeID FROM EmpPositionInfo)
AND RosterCode NOT IN (SELECT WorkHourPatternCode FROM WorkHourPattern)

Update EmpPositionInfo
Set WorkHourPatternID = (Select WorkHourPatternID from WorkHourPattern where WorkHourPatternCode = (Select RosterCode FROM RosterCode where RosterCodeID=EmpPosDefaultRosterCodeID))
where WorkHourPatternID is NULL
AND EmpPosDefaultRosterCodeID in (Select RosterCodeID from RosterCode)