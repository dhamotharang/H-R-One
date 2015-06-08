using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.CommonLib;
using System.Data;

namespace HROne.LeaveCalc
{
    public class CompensationLeaveEntitleCompareByEffectiveDate : IComparer
    {
        bool m_ascending = true;
        public CompensationLeaveEntitleCompareByEffectiveDate(bool ascending)
        {
            m_ascending = ascending;
        }

        int IComparer.Compare(object x, object y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            ECompensationLeaveEntitle m_X = (ECompensationLeaveEntitle)x;
            ECompensationLeaveEntitle m_Y = (ECompensationLeaveEntitle)y;

            if (m_X.CompensationLeaveEntitleEffectiveDate.Equals(m_Y.CompensationLeaveEntitleEffectiveDate))
            {
                if (m_X.CompensationLeaveEntitleDateExpiry.Equals(m_Y.CompensationLeaveEntitleDateExpiry))
                {
                    return m_X.CompensationLeaveEntitleID.CompareTo(m_Y.CompensationLeaveEntitleID);
                }
                return m_X.CompensationLeaveEntitleDateExpiry.CompareTo(m_Y.CompensationLeaveEntitleDateExpiry);
            }
            return m_X.CompensationLeaveEntitleEffectiveDate.CompareTo(m_Y.CompensationLeaveEntitleEffectiveDate);
        }
    }

    public class CompensationLeaveBalanceProcess : LeaveBalanceProcess
    {
        protected DataTable m_earned;

        public CompensationLeaveBalanceProcess(DatabaseConnection dbConn, int EmpID)
            : base(dbConn, EmpID, 0)
        {
            EntitlePeriodUnit = "Y";
            m_LeaveTypeID = ELeaveType.COMPENSATION_LEAVE_TYPE(dbConn).LeaveTypeID;
            m_BalanceUnit = ELeaveBalance.LeaveBalanceUnit.Hour;

        }

        private ArrayList LoadAllEntitled(DateTime fromDate, DateTime toDate, int empID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("CompensationLeaveEntitleEffectiveDate", ">", fromDate.AddDays(-1)));
            filter.add(new Match("CompensationLeaveEntitleEffectiveDate", "<", toDate.AddDays(1)));
            filter.add(new Match("EmpID", empID));
            //filter.add(new Match("LeaveAppCancelID", 0));
            filter.add("CompensationLeaveEntitleEffectiveDate", true);
            filter.add("CompensationLeaveEntitleDateExpiry", false);

            ArrayList entitlementList = ECompensationLeaveEntitle.db.select(dbConn, filter);

            foreach (ECompensationLeaveEntitle o in entitlementList)
            {
                if (o.CompensationLeaveEntitleDateExpiry.Ticks.Equals(0))
                    o.CompensationLeaveEntitleDateExpiry = new DateTime(2999, 12, 31);

                // if the entitlement is created from ESS, reset the Effective Date and Expiry Date to 00:00:00
                o.CompensationLeaveEntitleEffectiveDate = o.CompensationLeaveEntitleEffectiveDate.Date;
                o.CompensationLeaveEntitleDateExpiry = o.CompensationLeaveEntitleDateExpiry.Date;
            }

            entitlementList.Sort(new CompensationLeaveEntitleCompareByEffectiveDate(true));

            return entitlementList;
        }

        private ArrayList LoadAllTaken(DateTime fromDate, DateTime toDate, int empID, int leaveTypeID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("LeaveAppDateFrom", ">", fromDate.AddDays(-1)));
            filter.add(new Match("LeaveAppDateFrom", "<", toDate.AddDays(1)));
            filter.add(new Match("EmpID", empID));
            filter.add("LeaveAppDateFrom", true);
            filter.add("LeaveAppID", true);

            DBFilter leaveCodeFilter = new DBFilter();
            leaveCodeFilter.add(new Match("LeaveTypeID", leaveTypeID));
            filter.add(new IN("LeaveCodeID", "Select LeaveCodeID from " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));

            return ELeaveApplication.db.select(dbConn, filter);
        }

        private ArrayList LoadAllAdjust(DateTime fromDate, DateTime toDate, int empID, int leaveTypeID)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("LeaveBalAdjDate", ">", fromDate.AddDays(-1)));
            filter.add(new Match("LeaveBalAdjDate", "<", toDate.AddDays(1)));
            filter.add(new Match("EmpID", empID));
            filter.add(new Match("LeaveTypeID", leaveTypeID));
            filter.add("LeaveBalAdjDate", true);
            filter.add("LeaveBalAdjType", false); // (B)alance Reset -> (A)djustment

            return ELeaveBalanceAdjustment.db.select(dbConn, filter);
        }

        protected DataTable BuildEarnedTable(ArrayList entitledList, ArrayList adjustedList)
        {
            DataTable m_table = new DataTable("earned");

            m_table.Columns.Add("EFFECTIVE_DATE", typeof(DateTime));
            m_table.Columns.Add("EXPIRY_DATE", typeof(DateTime));
            m_table.Columns.Add("HOURS", typeof(double));
            m_table.Columns.Add("UNUSED_HOURS", typeof(double));
            m_table.Columns.Add("TYPE", typeof(string));

            DataRow m_row;
            foreach (ECompensationLeaveEntitle o in entitledList)
            {
                m_row = m_table.NewRow();
                m_row["EFFECTIVE_DATE"] = o.CompensationLeaveEntitleEffectiveDate;
                if (!o.CompensationLeaveEntitleDateExpiry.Ticks.Equals(0))
                    m_row["EXPIRY_DATE"] = o.CompensationLeaveEntitleDateExpiry;
                else
                    m_row["EXPIRY_DATE"] = new DateTime(2999, 12, 31);
                m_row["HOURS"] = o.CompensationLeaveEntitleHoursClaim;
                m_row["UNUSED_HOURS"] = o.CompensationLeaveEntitleHoursClaim;
                m_row["TYPE"] = "E";    // entitlement
                m_table.Rows.Add(m_row);
            }

            foreach (ELeaveBalanceAdjustment o in adjustedList)
            {
                if (o.LeaveBalAdjType == ELeaveBalanceAdjustment.ADJUST_TYPE_ADJUSTMENT)
                {
                    m_row = m_table.NewRow();
                    m_row["EFFECTIVE_DATE"] = o.LeaveBalAdjDate;
                    m_row["EXPIRY_DATE"] = new DateTime(2999, 12, 31);
                    m_row["HOURS"] = o.LeaveBalAdjValue;
                    m_row["UNUSED_HOURS"] = o.LeaveBalAdjValue;
                    m_row["TYPE"] = "A";    // adjustment, (Reset Balance is not supported)
                    m_table.Rows.Add(m_row);
                }
            }
            m_table.DefaultView.Sort = "EFFECTIVE_DATE, EXPIRY_DATE, TYPE";
            return m_table.DefaultView.ToTable();
        }

        protected override void BroughtForwardCalculation(DateTime AsOfDate)
        {
            // no need to call this function
        }

        protected double GetEntitledBalance(DateTime fromDate, DateTime toDate)
        {
            double m_balance = 0;
            foreach (DataRow row in m_earned.Rows)
            {
                if (row["TYPE"].ToString() == "E" && 
                    System.Convert.ToDateTime(row["EFFECTIVE_DATE"]) >= fromDate && 
                    System.Convert.ToDateTime(row["EFFECTIVE_DATE"]) <= toDate)
                {
                    m_balance += System.Convert.ToDouble(row["HOURS"]);
                }
            }
            return m_balance;
        }

        protected double GetAdjustedBalance(DateTime fromDate, DateTime toDate)
        {
            double m_balance = 0;
            foreach (DataRow row in m_earned.Rows)
            {
                if (row["TYPE"].ToString() == "A" &&
                    System.Convert.ToDateTime(row["EFFECTIVE_DATE"]) >= fromDate &&
                    System.Convert.ToDateTime(row["EFFECTIVE_DATE"]) <= toDate)
                {
                    m_balance += System.Convert.ToDouble(row["HOURS"]);
                }
            }
            return m_balance;
        }

        protected override void LoadServerData(DateTime asOfDate)
        {
            ELeaveBalance m_balance = new ELeaveBalance();
            ELeaveBalance m_preBalance = new ELeaveBalance();
            ELeaveBalance m_curBalance = new ELeaveBalance();
            ELeaveBalance m_subBalance;

            DateTime m_periodFromDate = new DateTime(1900, 1, 1);
            DateTime m_periodToDate = asOfDate;

            m_balance.EmpID = EmpID;
            m_balance.LeaveTypeID = LeaveTypeID;
            m_balance.BalanceUnit = ELeaveBalance.LeaveBalanceUnit.Hour;
            m_balance.LeaveBalanceEffectiveDate = new DateTime(asOfDate.Year, 1, 1);

            ArrayList entitledList = LoadAllEntitled(m_periodFromDate, asOfDate, m_balance.EmpID);
            ArrayList adjustedList = LoadAllAdjust(m_periodFromDate, asOfDate, EmpID, m_balance.LeaveTypeID);
            ArrayList takenList = LoadAllTaken(m_periodFromDate, m_periodToDate, EmpID, m_balance.LeaveTypeID);
            ArrayList processedList = new ArrayList();
            ArrayList removalList = new ArrayList();

            m_earned = BuildEarnedTable(entitledList, adjustedList);

            m_preBalance.LeaveBalanceEntitled = GetEntitledBalance(m_periodFromDate, m_balance.LeaveBalanceEffectiveDate.AddDays(-1));
            m_preBalance.Adjust = GetAdjustedBalance(m_periodFromDate, m_balance.LeaveBalanceEffectiveDate.AddDays(-1));

            //********************* before effective date ******************/
            foreach (ELeaveApplication o in takenList) // process Taken before effective date
            {
                if (o.LeaveAppDateFrom >= m_balance.LeaveBalanceEffectiveDate)
                    break;

                double m_takenHours = o.LeaveAppHours;

                // check available balance before process leave taken
                m_subBalance = GetAvailableBalance(o, m_balance.LeaveBalanceEffectiveDate.AddDays(-1)); //, entitledList, processedList, adjustedList, o.LeaveAppHours);
                m_preBalance.ExpiryForfeit += m_subBalance.ExpiryForfeit;
                m_preBalance.Taken += m_takenHours;

                removalList.Add(o);
            }

            foreach (ELeaveApplication o in removalList)
            {
                takenList.Remove(o);
            }
            
            m_balance.LeaveBalanceBF = m_preBalance.LeaveBalanceEntitled +
                                       m_preBalance.Adjust -
                                       m_preBalance.Taken -
                                       m_preBalance.ExpiryForfeit;

            m_balance.LeaveBalanceEntitled = GetEntitledBalance(m_balance.LeaveBalanceEffectiveDate, asOfDate);
            m_balance.Adjust = GetAdjustedBalance(m_balance.LeaveBalanceEffectiveDate, asOfDate);
            
            foreach (ELeaveApplication o in takenList) // process Taken after effective date
            {
                if (o.LeaveAppDateFrom > asOfDate)
                    break;

                double m_takenHours = o.LeaveAppHours;

                // check available balance before process leave taken
                m_subBalance = GetAvailableBalance(o, asOfDate);
                m_curBalance.ExpiryForfeit += m_subBalance.ExpiryForfeit;
                m_curBalance.Taken += m_takenHours;

                removalList.Add(o);
            }
            foreach (ELeaveApplication o in removalList)
            {
                takenList.Remove(o);
            }

            m_balance.Taken = m_curBalance.Taken;
            m_balance.ExpiryForfeit = m_curBalance.ExpiryForfeit;

            // let unused entitlement/adjustment expire if there are no further taken.
            m_balance.ExpiryForfeit += GetUnusedExpiredBalance(asOfDate);

            //********************** get next expiry information *************/
            if (m_earned.Rows.Count > 0)
            {

                m_earned.DefaultView.Sort = "EXPIRY_DATE, UNUSED_HOURS desc";
                m_earned = m_earned.DefaultView.ToTable();

                foreach(DataRow row in m_earned.Rows)
                {
                    if (row["EXPIRY_DATE"] != null && System.Convert.ToDateTime(row["EXPIRY_DATE"]).CompareTo(asOfDate) >= 0
                                                   && System.Convert.ToDouble(row["UNUSED_HOURS"]) > 0)
                    {
                        if (m_balance.NextExpiryDate.Ticks.Equals(0))
                        {
                            m_balance.NextExpiryDate = System.Convert.ToDateTime(row["EXPIRY_DATE"]);
                            m_balance.NextExpiryForfeit = System.Convert.ToDouble(row["UNUSED_HOURS"]);
                        }else if (m_balance.NextExpiryDate == System.Convert.ToDateTime(row["EXPIRY_DATE"]))
                        {
                            m_balance.NextExpiryForfeit += System.Convert.ToDouble(row["UNUSED_HOURS"]);
                        }
                    }
                }
                if (m_balance.NextExpiryForfeit <= 0)
                {
                    m_balance.NextExpiryDate = new DateTime(); 
                }
            }

            // find the reserved
            m_balance.Reserved = GetReservedBalance(takenList, asOfDate);

            balanceItemList.Add(m_balance);
        }

        protected double GetReservedBalance(ArrayList takenList, DateTime asOfDate)
        {
            double m_balance = 0;
            foreach (ELeaveApplication o in takenList)
            {
                if (o.LeaveAppDateFrom > asOfDate)
                {
                    m_balance += o.LeaveAppHours;
                }
            }
            return m_balance;
        }

        protected double GetUnusedExpiredBalance(DateTime periodEndDate)
        {
            double m_expired = 0;

            // ******** remove expired entitlement
            foreach (DataRow row in m_earned.Rows)
            {
                if (row["EXPIRY_DATE"] != null && System.Convert.ToDateTime(row["EXPIRY_DATE"]) < periodEndDate)
                {
                    m_expired += System.Convert.ToDouble(row["UNUSED_HOURS"]);
                    row["UNUSED_HOURS"] = 0;
                    //row.Delete();
                }
            }
            return m_expired;
        }

        protected ELeaveBalance GetAvailableBalance(ELeaveApplication leaveApplicatoin, DateTime periodEndDate)
        {
            ELeaveBalance m_balance = new ELeaveBalance();

            // ******** remove expired entitlement
            foreach (DataRow row in m_earned.Rows)
            {
                if (row["EXPIRY_DATE"] != null && System.Convert.ToDateTime(row["EXPIRY_DATE"]) < leaveApplicatoin.LeaveAppDateFrom)
                {
                    //m_balance.LeaveBalanceEntitled += System.Convert.ToDouble(row["UNUSED_HOURS"]);
                    m_balance.ExpiryForfeit += System.Convert.ToDouble(row["UNUSED_HOURS"]);
                    row["UNUSED_HOURS"] = 0;
                    //row.Delete();
                }
            }


            // ******* calculate entitled from entitledList and processedList
            foreach (DataRow row in m_earned.Rows)
            {
                if (leaveApplicatoin != null && leaveApplicatoin.LeaveAppHours > 0)
                {
                    if (leaveApplicatoin.LeaveAppHours >= System.Convert.ToDouble(row["UNUSED_HOURS"]))
                    {
                        leaveApplicatoin.LeaveAppHours -= System.Convert.ToDouble(row["UNUSED_HOURS"]);
                        row["UNUSED_HOURS"] = 0;
                        //row.Delete();
                    }
                    else
                    {
                        row["UNUSED_HOURS"] = System.Convert.ToDouble(row["UNUSED_HOURS"]) - leaveApplicatoin.LeaveAppHours;
                        leaveApplicatoin.LeaveAppHours = 0;
                    }
                }
                else
                    break;
            }

            m_earned.DefaultView.RowFilter = "UNUSED_HOURS > 0";
            m_earned = m_earned.DefaultView.ToTable();
            
            return m_balance;
        }
    }
}
