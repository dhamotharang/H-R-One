//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;
//using HROne.Lib.Entities;
//using HROne.DataAccess;
//using HROne.CommonLib;

//namespace HROne.LeaveCalc
//{
//    public class CompensationLeaveBalanceProcess : LeaveBalanceProcess
//    {
//        public CompensationLeaveBalanceProcess(int EmpID)
//            : base(EmpID, 0)
//        {
//            EntitlePeriodUnit = "Y";
//            m_LeaveTypeID = ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID;
//            m_BalanceUnit = ELeaveBalance.LeaveBalanceUnit.Hour;
//        }

//        protected override double CalculateProrata(LeaveProrataEntitle prorataEntitle)
//        {
//            DBFilter compensationLeaveFilter = new DBFilter();
//            compensationLeaveFilter.add(new Match("EmpID", EmpID));
//            compensationLeaveFilter.add(new Match("CompensationLeaveEntitleEffectiveDate", ">=", prorataEntitle.From));
//            compensationLeaveFilter.add(new Match("CompensationLeaveEntitleEffectiveDate", "<=", prorataEntitle.To));
//            ArrayList compensationLeaveEntitleList = ECompensationLeaveEntitle.db.select(dbConn, compensationLeaveFilter);

//            double result = 0;
//            foreach (ECompensationLeaveEntitle compEntitle in compensationLeaveEntitleList)
//                result += compEntitle.CompensationLeaveEntitleHoursClaim;

//            return result;
//        }
//    }
//}
