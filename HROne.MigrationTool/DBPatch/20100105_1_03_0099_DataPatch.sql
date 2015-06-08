insert into LeavePlanBroughtForward (LeavePlanID,LeaveTypeID,LeavePlanBroughtForwardMax)
select lp.LeavePlanID, lt.LeaveTypeID,lp.LeavePlanALMaxBF
From LeavePlan lp, LeaveType lt
where lt.LeaveType='ANNUAL';

insert into LeavePlanBroughtForward (LeavePlanID,LeaveTypeID,LeavePlanBroughtForwardMax)
select lp.LeavePlanID, lt.LeaveTypeID,lp.LeavePlanSL1MaxBF
From LeavePlan lp, LeaveType lt
where lt.LeaveType='SLCAT1';

insert into LeavePlanBroughtForward (LeavePlanID,LeaveTypeID,LeavePlanBroughtForwardMax)
select lp.LeavePlanID, lt.LeaveTypeID,lp.LeavePlanSL2MaxBF
From LeavePlan lp, LeaveType lt
where lt.LeaveType='SLCAT2';

