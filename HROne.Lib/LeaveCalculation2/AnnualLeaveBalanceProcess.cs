//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;
//using HROne.Lib.Entities;
//using HROne.DataAccess;
//using HROne.CommonLib;

//namespace HROne.LeaveCalc
//{
//    public class AnnualLeaveBalanceProcess : LeaveBalanceProcess
//    {
//        public AnnualLeaveBalanceProcess(int EmpID)
//            : base(EmpID, 0)
//        {
//            EntitlePeriodUnit = "Y";
//            m_LeaveTypeID = ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID;
//        }

//        protected override double LoadProrata(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {
//            base.LoadProrata(balanceItem, AsOfDate);
//            balanceItem.LeaveBalanceEntitled = ALRounding(EmpID, balanceItem.LeaveBalanceEffectiveEndDate, balanceItem.LeaveBalanceEntitled);
//            return balanceItem.LeaveBalanceEntitled;
//        }
//        protected static double ALRounding(int EmpID, DateTime AsOfDate, double originalProrata)
//        {
//            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, EmpID);
//            if (empPos != null)
//            {
//                ELeavePlan leavePlan = new ELeavePlan();
//                leavePlan.LeavePlanID = empPos.LeavePlanID;
//                if (ELeavePlan.db.select(dbConn, leavePlan))
//                {
//                    EALProrataRoundingRule roundingRule = new EALProrataRoundingRule();
//                    roundingRule.ALProrataRoundingRuleID = leavePlan.ALProrataRoundingRuleID;
//                    if (EALProrataRoundingRule.db.select(dbConn, roundingRule))
//                        return roundingRule.Rounding(dbConn, originalProrata);

//                }
//            }
//            return originalProrata;
//        }
//    }
//}
