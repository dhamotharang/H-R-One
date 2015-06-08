using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.LeaveCalc
{
    public class AnnualLeaveBalanceProcess : LeaveBalanceProcess
    {
        public bool skipALRoundingRule = false;

        public AnnualLeaveBalanceProcess(DatabaseConnection dbConn, int EmpID)
            : base(dbConn, EmpID, 0)
        {
            EntitlePeriodUnit = "Y";
            m_LeaveTypeID = ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID;
        }

        protected override double LoadProrata(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            base.LoadProrata(balanceItem, AsOfDate);
            if (!skipALRoundingRule)
            balanceItem.LeaveBalanceEntitled = ALRounding(EmpID, AsOfDate, balanceItem);
            return balanceItem.LeaveBalanceEntitled;
        }
        protected double ALRounding(int EmpID, DateTime AsOfDate, ELeaveBalance balanceItem)
        {
            double originalProrata = balanceItem.LeaveBalanceEntitled;
            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, balanceItem.LeaveBalanceEffectiveEndDate, EmpID);
            if (empPos != null)
            {
                ELeavePlan leavePlan = new ELeavePlan();
                leavePlan.LeavePlanID = empPos.LeavePlanID;
                if (ELeavePlan.db.select(dbConn, leavePlan))
                {
                    if (!leavePlan.LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly || leavePlan.LeavePlanALRoundingRuleIsApplyFirstYearBroughtForwardOnly && AsOfDate > balanceItem.LeaveBalanceEffectiveEndDate && balanceItem.LeaveBalanceEffectiveDate <= DefaultServiceStartDate)
                    {
                        EALProrataRoundingRule roundingRule = new EALProrataRoundingRule();
                        roundingRule.ALProrataRoundingRuleID = leavePlan.ALProrataRoundingRuleID;
                        if (EALProrataRoundingRule.db.select(dbConn, roundingRule))
                            return roundingRule.Rounding(dbConn, originalProrata);
                    }
                }
            }
            return originalProrata;
        }
        protected override void  submit(ELeaveBalance balanceItem, ELeaveBalanceEntitle balanceEntitle)
        {
            if (!skipALRoundingRule)
            {
                base.submit(balanceItem, balanceEntitle);
            }
        }
    }
}
