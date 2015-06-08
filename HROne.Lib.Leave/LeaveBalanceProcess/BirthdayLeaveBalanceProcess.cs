using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.LeaveCalc
{
    public class BirthdayLeaveBalanceProcess : LeaveBalanceProcess
    {
        public BirthdayLeaveBalanceProcess(DatabaseConnection dbConn, int EmpID)
            : base(dbConn, EmpID, 0)
        {
            EntitlePeriodUnit = "Y";
            m_LeaveTypeID = ELeaveType.BITYHDAY_LEAVE_TYPE(dbConn).LeaveTypeID;
            m_BalanceUnit = ELeaveBalance.LeaveBalanceUnit.Day;
        }

        protected override double CalculateProrata(LeaveProrataEntitle prorataEntitle)
        {
            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            if (!EEmpPersonalInfo.db.select(dbConn, empInfo))
                return 0;

            int addYear = Convert.ToInt32(((TimeSpan)(prorataEntitle.From - empInfo.EmpDateOfBirth)).TotalDays / 366);

            while (empInfo.EmpDateOfBirth.AddYears(addYear) < prorataEntitle.From)
                addYear++;

            DateTime birthday = empInfo.EmpDateOfBirth.AddYears(addYear);
            if (birthday >= prorataEntitle.From && birthday <= prorataEntitle.To)
                return prorataEntitle.LeavePlanEntitle.LeavePlanEntitleDays;

            return 0;
        }

    }
}
