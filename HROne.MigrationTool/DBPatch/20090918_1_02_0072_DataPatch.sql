update rostercode
set 
RosterCodeCountWorkHourOnly=0,
RosterCodeCountOTAfterWorkHourMin=0,
RosterCodeOTMinsUnit=1,
RosterCodeOTMinsRoundingRule='DOWN'
where RosterCodeCountWorkHourOnly is null
AND RosterCodeCountOTAfterWorkHourMin is null
AND RosterCodeOTMinsUnit is null
AND RosterCodeOTMinsRoundingRule is null
