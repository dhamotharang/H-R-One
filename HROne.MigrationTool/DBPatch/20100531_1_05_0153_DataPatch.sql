update UserReminderOption
Set UserReminderOptionRemindDaysBefore=30,
UserReminderOptionRemindDaysAfter=30
where UserReminderOptionRemindDaysBefore is null
and UserReminderOptionRemindDaysAfter is null
and ReminderTypeID in 
(	Select ReminderTypeID from ReminderType where ReminderTypeCode in ('DOB18','DOB65','DOB','WORKPERMITEXPIRY')	)

update UserReminderOption
Set UserReminderOptionRemindDaysBefore=15,
UserReminderOptionRemindDaysAfter=15
where UserReminderOptionRemindDaysBefore is null
and UserReminderOptionRemindDaysAfter is null
and ReminderTypeID in 
(	Select ReminderTypeID from ReminderType where ReminderTypeCode in ('PROBATION','TERMINATION')	)
