SET IDENTITY_INSERT ReminderType On

INSERT INTO [ReminderType] ([ReminderTypeID],[ReminderTypeCode],[ReminderTypeDesc])VALUES(1,'DOB18','Age of 18 Birthday Reminder')
INSERT INTO [ReminderType] ([ReminderTypeID],[ReminderTypeCode],[ReminderTypeDesc])VALUES(2,'DOB','Birthday Reminder')
INSERT INTO [ReminderType] ([ReminderTypeID],[ReminderTypeCode],[ReminderTypeDesc])VALUES(3,'DOB65','Age of 65 Birthday Reminder')
INSERT INTO [ReminderType] ([ReminderTypeID],[ReminderTypeCode],[ReminderTypeDesc])VALUES(4,'PROBATION','Probation Reminder')
INSERT INTO [ReminderType] ([ReminderTypeID],[ReminderTypeCode],[ReminderTypeDesc])VALUES(5,'TERMINATION','Employee Termination Reminder')
INSERT INTO [ReminderType] ([ReminderTypeID],[ReminderTypeCode],[ReminderTypeDesc])VALUES(6,'WORKPERMITEXPIRY','Work Permit Expiry Reminder')

SET IDENTITY_INSERT ReminderType Off