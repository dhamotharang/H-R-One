INSERT INTO UserGroupFunction (UserGroupID, FunctionID, FunctionAllowRead, FunctionAllowWrite)
SELECT ug.UserGroupID, sf.FunctionID, 1, 1
FROM UserGroup ug, SystemFunction sf
WHERE NOT EXISTS
(	
	SELECT * 
	FROM UserGroupFunction ugf
	WHERE ugf.UserGroupID=ug.UserGroupID
	AND ugf.FunctionID=sf.FunctionID
)
AND NOT EXISTS (SELECT * FROM UserGroup tmpug WHERE tmpug.UserGroupID >1)
AND NOT EXISTS (SELECT * FROM Users tmpusers WHERE tmpusers.UserID >1)