using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.CommonLib;
using System.Diagnostics;

namespace HROne.LeaveCalc
{
    public class LeaveBalanceProcess
    {

        protected int m_tab = 0;

        protected ArrayList balanceItemList = new ArrayList();
        protected ArrayList entitledHistoryItemList = new ArrayList();
 
        protected int EmpID;
        //protected DateTime AsOfDate;
        protected string EntitlePeriodUnit = "Y";
        protected int m_LeaveTypeID;
        //protected int CutOffDay = 1;
        //protected int CutOffMonth = 1;
        private DateTime m_DefaultServiceStartDate;
        private DateTime m_DefaultDateOfJoin;
        protected bool allowEntitleProrata = true;
        public bool skipLastEmploymentDateChecking = false;
        protected bool skipNextExpiryCalculation = false;
        protected DatabaseConnection dbConn;

        protected DateTime DefaultServiceStartDate
        {
            get
            {
                return m_DefaultServiceStartDate;
            }
        }
        protected DateTime DefaultDateOfJoin
        {
            get
            {
                return m_DefaultDateOfJoin;
            }
        }

        protected ELeaveBalance.LeaveBalanceUnit m_BalanceUnit = ELeaveBalance.LeaveBalanceUnit.Day;

        public int LeaveTypeID
        {
            get
            {
                return m_LeaveTypeID;
            }
        }

        public ELeaveBalance.LeaveBalanceUnit BalanceUnit
        {
            get
            {
                return m_BalanceUnit;
            }
        }

        //  To prevent incorrect prorata calculation on specific  leave type, the object can only be created internally
        protected internal LeaveBalanceProcess(DatabaseConnection dbConn, int EmpID, int LeaveTypeID)
        {
            this.dbConn = dbConn;
            this.EmpID = EmpID;
            this.m_LeaveTypeID = LeaveTypeID;

            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = EmpID;
            EEmpPersonalInfo.db.select(dbConn, empInfo);
            m_DefaultServiceStartDate = empInfo.EmpServiceDate;
            m_DefaultDateOfJoin = empInfo.EmpDateOfJoin;

        }

        protected virtual LeaveBalanceProcess CreateLeaveBalanceProcess()
        {
            LeaveBalanceProcess tmpLeaveBalanceProcess = new LeaveBalanceProcess(dbConn, EmpID, LeaveTypeID);

            tmpLeaveBalanceProcess.skipNextExpiryCalculation = true;

            return tmpLeaveBalanceProcess;
        }

        public ELeaveBalance getLatestLeaveBalance()
        {
            if (balanceItemList.Count > 0)
            {
                return (ELeaveBalance)balanceItemList[0];
            }
            else
                return null;
        }

        public ArrayList getBalanceHistory()
        {
            return balanceItemList;
        }

        protected String writeTab()
        {
            if (m_tab < 0) m_tab = 0;

            int i;
            String s="";

            for (i = 0; i < m_tab; i++)
            {
                s = s + "\t";
            }
            return s;
        }


        protected virtual ELeaveBalance CreateLeaveBalanceItem()
        {
            ELeaveBalance balanceItem = new ELeaveBalance();
            balanceItem.LeaveBalanceEffectiveDate = NextStartDate();
            balanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(balanceItem.LeaveBalanceEffectiveDate).AddDays(-1);
            balanceItem.EmpID = EmpID;
            balanceItem.LeaveTypeID = m_LeaveTypeID;
            return balanceItem;
        }

        protected virtual ELeaveBalance CreateNextLeaveBalanceItem(ELeaveBalance lastLeaveBalanceItem)
        {
            ELeaveBalance balanceItem = CreateLeaveBalanceItem();
            ELeaveBalanceEntitle currentEntitle = null;

            if (lastLeaveBalanceItem != null)
            {
                balanceItem.LeaveBalanceEffectiveDate = lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate.AddDays(1);
                balanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(balanceItem.LeaveBalanceEffectiveDate).AddDays(-1);
                balanceItem.LeaveBalanceBF = lastLeaveBalanceItem.getBalance();

                DBFilter leaveBalanceEntitleFilter = new DBFilter();
                leaveBalanceEntitleFilter.add(new Match("EmpID", balanceItem.EmpID));
                leaveBalanceEntitleFilter.add(new Match("LeaveTypeID", balanceItem.LeaveTypeID));
                leaveBalanceEntitleFilter.add(new Match("LeaveBalanceEntitleEffectiveDate", balanceItem.LeaveBalanceEffectiveDate));
                ArrayList leaveBalanceEntitleList = ELeaveBalanceEntitle.db.select(dbConn, leaveBalanceEntitleFilter);
                if (leaveBalanceEntitleList.Count > 0)
                    currentEntitle = (ELeaveBalanceEntitle)leaveBalanceEntitleList[0];

                
                if (lastLeaveBalanceItem.LeaveBalanceEntitled > 0)
                {
                    // update/add current balance entitled
                    if (currentEntitle == null)
                        currentEntitle = new ELeaveBalanceEntitle();
                    currentEntitle.LeaveBalanceEntitleEffectiveDate = balanceItem.LeaveBalanceEffectiveDate;
                    currentEntitle.LeaveBalanceEntitleGrantPeriodFrom = lastLeaveBalanceItem.LeaveBalanceEffectiveDate;
                    currentEntitle.LeaveBalanceEntitleGrantPeriodTo = lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate;
                    currentEntitle.EmpID = balanceItem.EmpID;
                    currentEntitle.LeaveTypeID = balanceItem.LeaveTypeID;
                    currentEntitle.LeaveBalanceEntitleDays = lastLeaveBalanceItem.LeaveBalanceEntitled;
                }
                else
                {
                    if (currentEntitle != null)
                    {
                        currentEntitle.LeaveBalanceEntitleGrantPeriodFrom = lastLeaveBalanceItem.LeaveBalanceEffectiveDate;
                        currentEntitle.LeaveBalanceEntitleGrantPeriodTo = lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate;
                        currentEntitle.LeaveBalanceEntitleDays = lastLeaveBalanceItem.LeaveBalanceEntitled;
                    }
                }
                //  Initial Forfeiture
                balanceItem.LeaveBalanceForfeiture = 0;
                double MaxBF = MaximumBroughtForward(balanceItem.LeaveBalanceEffectiveDate);
                if (MaxBF >= 0)
                {
                    // Start 2013-11-28, Ricky So, revised the formula to handle cases where last month having to-be-expired leave that should take into account of current month forfeit leave balance
                    //balanceItem.LeaveBalanceForfeiture = balanceItem.LeaveBalanceBF - MaxBF;
                     balanceItem.LeaveBalanceForfeiture = balanceItem.LeaveBalanceBF - MaxBF - lastLeaveBalanceItem.NextExpiryForfeit;
                    // End 2013-11-28, Ricky So, revised the formula to handle cases where last month having to-be-expired leave that should take into account of current month forfeit leave balance

                    ELeavePlanBroughtForward leavePlanBroughtForward = GetLeavePlanBroughtForward(balanceItem.LeaveBalanceEffectiveDate, EmpID, m_LeaveTypeID);

                    if (leavePlanBroughtForward != null)
                    {
                        if (currentEntitle != null)
                            if (currentEntitle.LeaveBalanceEntitleDateExpiry.Ticks.Equals(0) && leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired < 9999 && (leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired > 0 || leavePlanBroughtForward.LeavePlanBroughtForwardMax == 0))
                                currentEntitle.LeaveBalanceEntitleDateExpiry = currentEntitle.LeaveBalanceEntitleEffectiveDate.AddMonths(leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired).AddDays(-1);
                    }

                    if (balanceItem.LeaveBalanceForfeiture < 0)
                        balanceItem.LeaveBalanceForfeiture = 0;
                }
            }

            balanceItemList.Insert(0, balanceItem);
            if (currentEntitle != null)
                entitledHistoryItemList.Insert(0, currentEntitle);
            submit(balanceItem, currentEntitle);

            return balanceItem;
        }

        public virtual void Recalculate()
        {
            // clear existing LeaveBalance and LeaveBalanceEntitle data for the employee
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("LeaveTypeID", m_LeaveTypeID));

            ELeaveBalance.db.delete(dbConn, filter);
            ELeaveBalanceEntitle.db.delete(dbConn, filter);

            // Display employee data
           LoadData(AppUtils.ServerDateTime().Date);
        }

        public void LoadData(DateTime AsOfDate)
        {
            if (!skipLastEmploymentDateChecking)
            {
                // determine Employee's termination status so that confirm the As-Of-date for retrieving leave balance information
                DBFilter filter = new DBFilter();
                filter.add(new Match("EmpID", EmpID));
                filter.add("EmpTermLastDate", false);

                ArrayList list = EEmpTermination.db.select(dbConn, filter);
                if (list.Count > 0)
                {
                    EEmpTermination obj = (EEmpTermination)list[0];
                    if (AsOfDate > obj.EmpTermLastDate)
                        AsOfDate = obj.EmpTermLastDate;
                }
            }

            LoadServerData(AsOfDate);

            BroughtForwardCalculation(AsOfDate);
        }

        protected virtual void LoadServerData(DateTime AsOfDate) // build balanceItemList (for LeaveBalance) and entitledHistoryItemList (for LeaveBalanceEntitle)
        {
            // retrieve LeaveBalance information of the employee.
            // Nothing should be retrieved for Re-calculating
            DBFilter leaveBalanceFilter = new DBFilter();
            leaveBalanceFilter.add(new Match("EmpID", EmpID));
            leaveBalanceFilter.add(new Match("LeaveBalanceEffectiveDate", "<=", AsOfDate));
            leaveBalanceFilter.add(new Match("LeaveTypeID", m_LeaveTypeID));
            leaveBalanceFilter.add("LeaveBalanceEffectiveDate", false);
            balanceItemList = ELeaveBalance.db.select(dbConn, leaveBalanceFilter);


            // Retrieve leaveBalanceEntitle information of the employee.  Again, no records should be selected in Re-calculating
            //  Jimmy: Do NOT filter the history by expiry date because the leave balance record may not be up-to-date
            //         so that the brought forward calculation may require to use those informtion as expired leave record 
            DBFilter leaveBalanceEntitleHistoryFilter = new DBFilter();
            leaveBalanceEntitleHistoryFilter.add(new Match("EmpID", EmpID));
            leaveBalanceEntitleHistoryFilter.add(new Match("LeaveBalanceEntitleEffectiveDate", "<=", AsOfDate));
            if (balanceItemList.Count > 0)
            {
                //  try to speed up the search result by loading less leave balance entitle History
                DateTime newAsOfDate = ((ELeaveBalance)balanceItemList[0]).LeaveBalanceEffectiveDate;
                leaveBalanceEntitleHistoryFilter.add(new Match("LeaveBalanceEntitleEffectiveDate", "<=", newAsOfDate));
                OR orLeaveBalanceEntitleExpiryDateTerm = new OR();
                orLeaveBalanceEntitleExpiryDateTerm.add(new NullTerm("LeaveBalanceEntitleDateExpiry"));
                orLeaveBalanceEntitleExpiryDateTerm.add(new Match("LeaveBalanceEntitleDateExpiry", ">=", newAsOfDate.AddDays(-1)));
                leaveBalanceEntitleHistoryFilter.add(orLeaveBalanceEntitleExpiryDateTerm);
            }
            leaveBalanceEntitleHistoryFilter.add(new Match("LeaveTypeID", m_LeaveTypeID));
            leaveBalanceEntitleHistoryFilter.add("LeaveBalanceEntitleEffectiveDate", false);
            entitledHistoryItemList = ELeaveBalanceEntitle.db.select(dbConn, leaveBalanceEntitleHistoryFilter);
        }

        protected virtual void LoadBalance(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            LoadTaken(balanceItem, AsOfDate);
            LoadProrata(balanceItem, AsOfDate);
            LoadAdjust(balanceItem, AsOfDate);
            LoadExpiryForfeit(balanceItem, AsOfDate);
            balanceItem.BalanceUnit = m_BalanceUnit;
            //balanceItem.Balance = balanceItem.getBalance();

        }

        protected virtual void BroughtForwardCalculation(DateTime AsOfDate)
        {
            ELeaveBalance lastLeaveBalanceItem;
            if (balanceItemList.Count > 0)
            {
                lastLeaveBalanceItem = (ELeaveBalance)balanceItemList[0];
            }
            else
            {
                lastLeaveBalanceItem = CreateNextLeaveBalanceItem(null);
            }

            lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(lastLeaveBalanceItem.LeaveBalanceEffectiveDate).AddDays(-1);
            LoadBalance(lastLeaveBalanceItem, AsOfDate);
            // Start 2013-11-28, Ricky So, force calculation of every LeaveBalance item.
            LoadNextExpiryForfeit(lastLeaveBalanceItem);
            // End 2013-11-28, Ricky So, force calculation of every LeaveBalance item.


int i = 0;

Debug.WriteLine(writeTab() + "BroughtForwardCalculation >>> *** lastLeaveBalanceItem(" + i.ToString("0") + ") ***" + Environment.NewLine + lastLeaveBalanceItem.PrintMe());

            while (lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate < AsOfDate)
            {
Debug.WriteLine(writeTab() + "BroughtForwardCalculation >>> Inside the loop"); m_tab++;

                ELeaveBalance balanceItem = CreateNextLeaveBalanceItem(lastLeaveBalanceItem);

                lastLeaveBalanceItem = balanceItem;
                LoadBalance(lastLeaveBalanceItem, AsOfDate);
                // Start 2013-11-28, Ricky So, force calculation of every LeaveBalance item.
                LoadNextExpiryForfeit(lastLeaveBalanceItem);
                // End 2013-11-28, Ricky So, force calculation of every LeaveBalance item.
                i++;

Debug.WriteLine(writeTab() + "BroughtForwardCalculation >>> *** lastLeaveBalanceItem(" + i.ToString("0") + ") ***" + Environment.NewLine + lastLeaveBalanceItem.PrintMe());

            }
            
m_tab--;
Debug.WriteLine(writeTab() + "BroughtForwardCalculation >>> (Quit the loop) ");

        }

        protected DateTime NextStartDate()
        {
            if (balanceItemList.Count > 0)
            {
                DateTime o = NextStartDate(((ELeaveBalance)balanceItemList[0]).LeaveBalanceEffectiveDate);
                return o;
            }
            else
            {
                return DefaultServiceStartDate;
            }
        }

        protected virtual DateTime NextStartDate(DateTime lastEffDate)
        {
            DateTime nextEffDate = new DateTime(9999, 9, 9);

            int cutOffMonth, cutOffDay;

            GetLeaveYearStartDate(lastEffDate, out cutOffMonth, out cutOffDay);

            if (EntitlePeriodUnit.Equals("Y"))
            {
                nextEffDate = new DateTime(lastEffDate.Year, cutOffMonth, 1);
                if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
                    nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
                else
                    nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
                while (nextEffDate <= lastEffDate)
                {
                    nextEffDate = nextEffDate.AddDays(-nextEffDate.Day + 1);
                    nextEffDate = nextEffDate.AddYears(1);
                    if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
                        nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
                    else
                        nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
                }
            }
            else if (EntitlePeriodUnit.Equals("M"))
            {
                nextEffDate = new DateTime(lastEffDate.Year, lastEffDate.Month, 1);
                if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
                    nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
                else
                    nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
                while (nextEffDate <= lastEffDate)
                {
                    nextEffDate = nextEffDate.AddDays(-nextEffDate.Day + 1);
                    nextEffDate = nextEffDate.AddMonths(1);
                    if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
                        nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
                    else
                        nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
                }
            }

            //  Check if there is another service date within "lastEffDate" and "nextEffDate"
            DateTime lastServiceDate = DefaultServiceStartDate;// getLastServiceDateChange(nextEffDate);
            if (lastEffDate < DefaultServiceStartDate)
            {
                //  use service date between "lastEffDate" and "nextEffDate" if there is another service date within "lastEffDate" and "nextEffDate"
                nextEffDate = DefaultServiceStartDate;
            }


            return nextEffDate; 
        }


        protected virtual double LoadTaken(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            DateTime DateFrom, DateTo;
            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
            balanceItem.Taken = 0;
            DBFilter filter = new DBFilter();
            filter.add(new Match("LeaveAppDateFrom", ">=", DateFrom));
            filter.add(new Match("EmpID", balanceItem.EmpID));
            DBFilter leaveCodeFilter = new DBFilter();
            leaveCodeFilter.add(new Match("LeaveTypeID", balanceItem.LeaveTypeID));
            filter.add(new IN("LeaveCodeID", "Select LeaveCodeID from " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));

            OR leaveCancelIDOrTerm = new OR();
            leaveCancelIDOrTerm.add(new NullTerm("LeaveAppCancelID"));
            leaveCancelIDOrTerm.add(new Match("LeaveAppCancelID", "<=", 0));
            filter.add(leaveCancelIDOrTerm);

            ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, filter);
            balanceItem.Taken = 0;
            balanceItem.Reserved = 0;
            foreach (ELeaveApplication la in leaveAppList)
            {
                if (la.LeaveAppDateFrom <= AsOfDate && la.LeaveAppDateFrom <= DateTo)
                    if (m_BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        balanceItem.Taken += la.LeaveAppHours;
                    else
                        balanceItem.Taken += la.LeaveAppDays;

                else
                {
                    if (la.LeaveAppDateFrom > AsOfDate || la.LeaveAppDateFrom > DateTo)
                        if (m_BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                            balanceItem.TakenAfterAsOfDate += la.LeaveAppHours;
                        else
                            balanceItem.TakenAfterAsOfDate += la.LeaveAppDays;

                    if (m_BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
                        balanceItem.Reserved += la.LeaveAppHours;
                    else
                        balanceItem.Reserved += la.LeaveAppDays;
                }
            }
            return balanceItem.Taken;
        }

        protected virtual double LoadAdjust(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            DateTime DateFrom, DateTo;
            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
            if (DateTo >= AsOfDate)
                DateTo = AsOfDate;

            DBFilter filter = new DBFilter();
            filter.add(new Match("LeaveBalAdjDate", ">=", DateFrom));
            filter.add(new Match("LeaveBalAdjDate", "<=", DateTo));
            filter.add(new Match("EmpID", balanceItem.EmpID));
            filter.add(new Match("LeaveTypeID", balanceItem.LeaveTypeID));
            filter.add("LeaveBalAdjDate", true);
            ArrayList leaveBalAdjList = ELeaveBalanceAdjustment.db.select(dbConn, filter);
            balanceItem.Adjust = 0;
            foreach (ELeaveBalanceAdjustment la in leaveBalAdjList)
            {
                if (la.LeaveBalAdjType.Equals(ELeaveBalanceAdjustment.ADJUST_TYPE_RESET_BALANCE))
                {
                    ELeaveBalance tmpBalanceItem = new ELeaveBalance();
                    ELeaveBalance.db.copyObject(balanceItem, tmpBalanceItem);
                    tmpBalanceItem.LeaveBalanceEffectiveEndDate = balanceItem.LeaveBalanceEffectiveEndDate;
                    tmpBalanceItem.Adjust = balanceItem.Adjust;
                    LoadTaken(tmpBalanceItem, la.LeaveBalAdjDate);
                    LoadProrata(tmpBalanceItem, la.LeaveBalAdjDate);
                    LoadExpiryForfeit(tmpBalanceItem, la.LeaveBalAdjDate);
                    //  Do NOT use LoadAdjust() function. This may cause recurrsive loading problem
                    //  and reset balance will override all the previous adjusted value 
                    //  so that previous adjust value is not consider
                    balanceItem.Adjust -= tmpBalanceItem.getBalance();
                }
                balanceItem.Adjust += la.LeaveBalAdjValue;
            }
            return balanceItem.Adjust;
        }

        protected virtual double LoadProrata(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            double prorata = 0;
            DateTime DateFrom, DateTo;
            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
            if (DateTo > AsOfDate)
                if (allowEntitleProrata)
                    DateTo = AsOfDate;
                else
                {
                    balanceItem.LeaveBalanceEntitled = 0;
                    return 0;
                }
            ArrayList timeFlameList = LoadProrataEntitlementList(DateFrom, DateTo);
            while (timeFlameList.Count > 0)
            {
                LeaveProrataEntitle prorataEntitle = (LeaveProrataEntitle)timeFlameList[0];
                if (prorataEntitle.LeavePlanEntitle != null)
                    if (allowEntitleProrata)
                        prorata += CalculateProrata(prorataEntitle);
                    else if (prorata < prorataEntitle.LeavePlanEntitle.LeavePlanEntitleDays)
                        prorata = prorataEntitle.LeavePlanEntitle.LeavePlanEntitleDays;
                timeFlameList.Remove(prorataEntitle); 
                if (prorataEntitle.To < DateTo && timeFlameList.Count > 0)
                {
                    DateTime newStartDate = prorataEntitle.To.AddDays(1);
                    DateTime newEndDate = NextStartDate(newStartDate).AddDays(-1);
                    // Check the month/day only because system may get Next effective end date. 
                    // e.g. start new leave entitle on Jan 1 using common leave year
                    if (newEndDate.Month != balanceItem.LeaveBalanceEffectiveEndDate.Month || newEndDate.Day != balanceItem.LeaveBalanceEffectiveEndDate.Day)
                    {
                        balanceItem.LeaveBalanceEffectiveEndDate = newEndDate;
                        if (newEndDate > AsOfDate)
                            newEndDate = AsOfDate;
                        timeFlameList = LoadProrataEntitlementList(newStartDate, newEndDate);
                    }
                }
            }
            balanceItem.LeaveBalanceEntitled = prorata;
            return prorata;
        }

        protected virtual double LoadExpiryForfeit(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            DateTime DateFrom, DateTo;
            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
            if (DateTo > AsOfDate)
                DateTo = AsOfDate;
            balanceItem.ExpiryForfeit = 0;
            balanceItem.NextExpiryDate = new DateTime();

            DateTime latestExpiryDate = new DateTime();
            double entitle = 0; double expiry = 0;
            List<ELeaveBalanceEntitle> obsoleteEntitleHistoryList = new List<ELeaveBalanceEntitle>();

            List<ELeaveBalanceEntitle> EntitleHistoryNotExpiryList = new List<ELeaveBalanceEntitle>();

            //  Latest Expiry Date must be determine first so that the function should calculate ExpiryForfeit first before new entitle on same day
            foreach (ELeaveBalanceEntitle balanceEntitleObject in entitledHistoryItemList)
            {
                if (!balanceEntitleObject.LeaveBalanceEntitleDateExpiry.Ticks.Equals(0)
                && balanceEntitleObject.LeaveBalanceEntitleDateExpiry < DateTo)
                {
                    if (balanceEntitleObject.LeaveBalanceEntitleDateExpiry >= DateFrom.AddDays(-1))
                    {
                        expiry += balanceEntitleObject.LeaveBalanceEntitleDays;
                        if (latestExpiryDate < balanceEntitleObject.LeaveBalanceEntitleDateExpiry)
                            latestExpiryDate = balanceEntitleObject.LeaveBalanceEntitleDateExpiry;
                    }
                    else
                        obsoleteEntitleHistoryList.Add(balanceEntitleObject);
                }
                else
                {
                    EntitleHistoryNotExpiryList.Add(balanceEntitleObject);
                    if (!balanceEntitleObject.LeaveBalanceEntitleDateExpiry.Ticks.Equals(0))
                        if (balanceItem.NextExpiryDate.Ticks.Equals(0)
                        || balanceItem.NextExpiryDate > balanceEntitleObject.LeaveBalanceEntitleDateExpiry)
                            balanceItem.NextExpiryDate = balanceEntitleObject.LeaveBalanceEntitleDateExpiry;
                }
            }
            
            // if last expiry date found 
            if (!latestExpiryDate.Ticks.Equals(0))
            {
                foreach (ELeaveBalanceEntitle balanceEntitleObject in EntitleHistoryNotExpiryList)
                {
                    // Start 2013-12-13, Ricky So, fixing slow performance
                    //if (latestExpiryDate.Ticks.Equals(0) || balanceEntitleObject.LeaveBalanceEntitleEffectiveDate < latestExpiryDate)
                    if (balanceEntitleObject.LeaveBalanceEntitleEffectiveDate < latestExpiryDate)
                        entitle += balanceEntitleObject.LeaveBalanceEntitleDays;
                }

                //  Remove the obsolete Leave Entitle for faster calculation result
                foreach (ELeaveBalanceEntitle obsolateEntitleHistory in obsoleteEntitleHistoryList)
                {
                    entitledHistoryItemList.Remove(obsolateEntitleHistory);
                }
                if (expiry > 0 && latestExpiryDate >= DateFrom.AddDays(-1))
                {
                    LeaveBalanceProcess processAsAtLastExpiryDate = this.CreateLeaveBalanceProcess();

                    processAsAtLastExpiryDate.LoadData(latestExpiryDate);
                    ELeaveBalance balanceAsAtLatestExpiryDate = processAsAtLastExpiryDate.getLatestLeaveBalance();
                    double forfeit = balanceAsAtLatestExpiryDate.getBalanceWithoutProrata() - entitle;
                    if (forfeit > 0)
                        if (balanceAsAtLatestExpiryDate.LeaveBalanceEffectiveDate != balanceItem.LeaveBalanceEffectiveDate)
                        {
                            balanceItem.ExpiryForfeit = forfeit;
                        }
                        else
                        {
                            balanceItem.ExpiryForfeit = forfeit + balanceAsAtLatestExpiryDate.ExpiryForfeit;
                        }
                }
            }
            
            return balanceItem.ExpiryForfeit;
        }

        protected ELeavePlanBroughtForward GetLeavePlanBroughtForward(DateTime AsOfDate, int employeeID, int leaveTypeID)
        {
            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, employeeID);
            if (empPos != null)
            {
                ELeavePlan leavePlan = new ELeavePlan();
                leavePlan.LeavePlanID = empPos.LeavePlanID;
                if (ELeavePlan.db.select(dbConn, leavePlan))
                {
                    DBFilter dbFilter = new DBFilter();
                    dbFilter.add(new Match("LeavePlanID", leavePlan.LeavePlanID));
                    dbFilter.add(new Match("LeaveTypeID", leaveTypeID));
                    ArrayList list = ELeavePlanBroughtForward.db.select(dbConn, dbFilter);
                    if (list.Count > 0)
                    {
                        ELeavePlanBroughtForward leavePlanBroughtForward = (ELeavePlanBroughtForward)list[0];
                        return leavePlanBroughtForward;
                    }
                }
            }
            return null;
        }

        protected virtual double LoadNextExpiryForfeit(ELeaveBalance balanceItem)
        {
            // Start 2013-11-28, Ricky So, revised To-Be-Expired calculation
            //LeaveBalanceProcess processAsAtNextExpiryDate = this.CreateLeaveBalanceProcess();
            //processAsAtNextExpiryDate.LoadData(balanceItem.NextExpiryDate.AddDays(1));
            //ELeaveBalance balanceAsAtNextExpiryDate = processAsAtNextExpiryDate.getLatestLeaveBalance();
            //double forfeit = balanceAsAtNextExpiryDate.ExpiryForfeit;
            //if (forfeit > 0)
            //    if (balanceAsAtNextExpiryDate.LeaveBalanceEffectiveDate != balanceItem.LeaveBalanceEffectiveDate)
            //        balanceItem.NextExpiryForfeit = forfeit;
            //    else
            //        balanceItem.NextExpiryForfeit = forfeit - balanceItem.ExpiryForfeit;


            // get the max carry forward months of the Leave Plan
            // get last period (i.e. effective date - 1) LeavePlanBroughtForward info
            ELeavePlanBroughtForward leavePlanBroughtForwardInfo = GetLeavePlanBroughtForward(balanceItem.LeaveBalanceEffectiveDate.AddDays(-1), balanceItem.EmpID, balanceItem.LeaveTypeID);

            if (leavePlanBroughtForwardInfo != null && leavePlanBroughtForwardInfo.LeavePlanBroughtForwardNumOfMonthExpired < 9999)
            {
                // effective date + no.of months for balance carry forward (set in LeavePlanSetup) - 1 days (to make a complete month)
                balanceItem.NextExpiryDate = balanceItem.LeaveBalanceEffectiveDate.AddMonths(leavePlanBroughtForwardInfo.LeavePlanBroughtForwardNumOfMonthExpired).AddDays(-1);

                balanceItem.NextExpiryForfeit = balanceItem.LeaveBalanceBF - balanceItem.LeaveBalanceForfeiture - balanceItem.ExpiryForfeit - balanceItem.Taken + balanceItem.Adjust;
                if (balanceItem.NextExpiryForfeit < 0)
                {
                    balanceItem.NextExpiryForfeit = 0;
                }
                if (balanceItem.NextExpiryForfeit == 0)
                {
                    balanceItem.NextExpiryDate = new DateTime();
                }
            }
            // End 2013-11-28, Ricky So, revised To-Be-Expired calculation

            return balanceItem.NextExpiryForfeit;
        }
        protected virtual double CalculateProrata(LeaveProrataEntitle prorataEntitle)
        {
            ELeavePlan leavePlan = new ELeavePlan();
            leavePlan.LeavePlanID = prorataEntitle.LeavePlanEntitle.LeavePlanID;
            if (ELeavePlan.db.select(dbConn, leavePlan))
            {
                DateTime dateFeb29 = new DateTime();
                int numberOfDayWithinPeriod = ((TimeSpan)prorataEntitle.ServerYearReferenceDate.AddYears(1).Subtract(prorataEntitle.ServerYearReferenceDate)).Days;
                int numberOfDayProrata = ((TimeSpan)prorataEntitle.To.Subtract(prorataEntitle.From)).Days + 1;
                if (leavePlan.LeavePlanProrataSkipFeb29)
                {
                    if (numberOfDayWithinPeriod.Equals(366))
                        numberOfDayWithinPeriod = 365;
                    if (DateTime.IsLeapYear(prorataEntitle.To.Year))
                        dateFeb29 = new DateTime(prorataEntitle.To.Year, 2, 29);
                    if (DateTime.IsLeapYear(prorataEntitle.From.Year))
                        dateFeb29 = new DateTime(prorataEntitle.From.Year, 2, 29);
                    if (!dateFeb29.Ticks.Equals(0))
                        if (prorataEntitle.From <= dateFeb29 && prorataEntitle.To >= dateFeb29)
                            numberOfDayProrata--;
                }
                return ((double)numberOfDayProrata) * prorataEntitle.LeavePlanEntitle.LeavePlanEntitleDays / ((double)numberOfDayWithinPeriod);
            }
            else
            {
                return 0;
            }
        }

        protected ArrayList LoadProrataEntitlementList(DateTime FromDate, DateTime ToDate)
        {

            ArrayList prorataEntitlementList = new ArrayList();

            DBFilter empPosFilter = new DBFilter();
            empPosFilter.add(new Match("EmpID", EmpID));
            empPosFilter.add(new Match("EmpPosEffFr", "<=", ToDate));
            //OR orPosEffTerms = new OR();
            //orPosEffTerms.add(new Match("EmpPosEffTo", ">=", FromDate));
            //orPosEffTerms.add(new NullTerm("EmpPosEffTo"));
            //empPosFilter.add(orPosEffTerms);
            empPosFilter.add("EmpPosEffFr", true);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);

            ArrayList mergePositionList = new ArrayList();

            foreach (EEmpPositionInfo empPos in empPosList)
            {
                if (mergePositionList.Count <= 0)
                    mergePositionList.Add(empPos);
                else
                {
                    EEmpPositionInfo lastPosition = (EEmpPositionInfo)mergePositionList[mergePositionList.Count - 1];
                    if (lastPosition.EmpPosEffTo.AddDays(1).Equals(empPos.EmpPosEffFr) && lastPosition.LeavePlanID.Equals(empPos.LeavePlanID))
                        lastPosition.EmpPosEffTo = empPos.EmpPosEffTo;
                    else
                        mergePositionList.Add(empPos);
                }
            }

            foreach (EEmpPositionInfo empPos in mergePositionList)
            {
                if (empPos.EmpPosEffTo.Ticks.Equals(0) || empPos.EmpPosEffTo >= FromDate)
                {

                    LeaveProrataEntitle prorataEntitlement = new LeaveProrataEntitle();

                    prorataEntitlement.From = empPos.EmpPosEffFr < FromDate ? FromDate : empPos.EmpPosEffFr;

                    if (empPos.EmpPosEffTo > empPos.EmpPosEffFr)
                        prorataEntitlement.To = empPos.EmpPosEffTo > ToDate ? ToDate : empPos.EmpPosEffTo;
                    else
                        prorataEntitlement.To = ToDate;

                    ELeavePlan leavePlan = new ELeavePlan();
                    leavePlan.LeavePlanID = empPos.LeavePlanID;
                    if (!ELeavePlan.db.select(dbConn, leavePlan))
                    {
                        double yearOfServiceFrom = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.From);
                        double yearOfServiceTo = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.To);
                        prorataEntitlement.ServerYearReferenceDate = DefaultServiceStartDate.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceFrom)));

                        prorataEntitlement.LeavePlanEntitle = null;
                        prorataEntitlementList.Add(prorataEntitlement);
                    }
                    else
                    {

                        double yearOfServiceFrom;
                        double yearOfServiceTo;

                        if (leavePlan.LeavePlanResetYearOfService)
                        {
                            yearOfServiceFrom = Utility.YearDifference(empPos.EmpPosEffFr, prorataEntitlement.From);
                            yearOfServiceTo = Utility.YearDifference(empPos.EmpPosEffFr, prorataEntitlement.To);
                            prorataEntitlement.ServerYearReferenceDate = empPos.EmpPosEffFr.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceFrom)));
                        }
                        else
                        {
                            yearOfServiceFrom = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.From);
                            yearOfServiceTo = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.To);
                            prorataEntitlement.ServerYearReferenceDate = DefaultServiceStartDate.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceFrom)));
                        }
                        //  skip calculate prorata if either "Do not count first Incomplete year" or use service start date as entitle date
                        if (leavePlan.LeavePlanNoCountFirstIncompleteYearOfService || !leavePlan.LeavePlanUseCommonLeaveYear)
                        {
                            prorataEntitlement.ServerYearReferenceDate = FromDate;
                            yearOfServiceTo = yearOfServiceFrom;
                        }
                        // **** Start Ricky So, 0000013, 2014-04-15
                        else if (EntitlePeriodUnit == "M")
                        {
                            prorataEntitlement.ServerYearReferenceDate = FromDate;
                            yearOfServiceTo = yearOfServiceFrom;                            
                        }
                        // **** End Ricky So, 0000013, 2014-04-15

                        ELeavePlanEntitle leavePlanEntitleFrom = GetLeavePlanEntitle(empPos.LeavePlanID, yearOfServiceFrom);
                        ELeavePlanEntitle leavePlanEntitleTo = GetLeavePlanEntitle(empPos.LeavePlanID, yearOfServiceTo);

                        if (leavePlan.LeavePlanComparePreviousLeavePlan)
                        {
                            foreach (EEmpPositionInfo empPreviousPos in mergePositionList)
                            {
                                if (empPreviousPos.EmpPosEffFr < empPos.EmpPosEffFr)
                                {
                                    ELeavePlan leavePreviousPlan = new ELeavePlan();
                                    leavePreviousPlan.LeavePlanID = empPreviousPos.LeavePlanID;
                                    if (ELeavePlan.db.select(dbConn, leavePreviousPlan))
                                        if (leavePlan.LeavePlanLeavePlanCompareRank >= leavePreviousPlan.LeavePlanLeavePlanCompareRank)
                                        {
                                            double previousLeavePlanYearOfServiceFrom;
                                            double previousLeavePlanYearOfServiceTo;
                                            DateTime tmpServiceReferenceDate;
                                            if (leavePreviousPlan.LeavePlanResetYearOfService)
                                            {
                                                previousLeavePlanYearOfServiceFrom = Utility.YearDifference(empPreviousPos.EmpPosEffFr, prorataEntitlement.From);
                                                previousLeavePlanYearOfServiceTo = Utility.YearDifference(empPreviousPos.EmpPosEffFr, prorataEntitlement.To);
                                                tmpServiceReferenceDate = empPreviousPos.EmpPosEffFr.AddYears(Convert.ToInt32(Math.Truncate(previousLeavePlanYearOfServiceFrom)));
                                            }
                                            else
                                            {
                                                previousLeavePlanYearOfServiceFrom = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.From);
                                                previousLeavePlanYearOfServiceTo = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.To);
                                                tmpServiceReferenceDate = DefaultServiceStartDate.AddYears(Convert.ToInt32(Math.Truncate(previousLeavePlanYearOfServiceFrom)));
                                            }

                                            if (leavePreviousPlan.LeavePlanNoCountFirstIncompleteYearOfService)
                                            {
                                                previousLeavePlanYearOfServiceTo = previousLeavePlanYearOfServiceFrom;
                                                tmpServiceReferenceDate = FromDate;
                                            }
                                            ELeavePlanEntitle previousLeavePlanLeavePlanEntitleFrom = GetLeavePlanEntitle(leavePreviousPlan.LeavePlanID, previousLeavePlanYearOfServiceFrom);
                                            ELeavePlanEntitle previousLeavePlanLeavePlanEntitleTo = GetLeavePlanEntitle(leavePreviousPlan.LeavePlanID, previousLeavePlanYearOfServiceTo);

                                            if (previousLeavePlanLeavePlanEntitleTo.LeavePlanEntitleDays > leavePlanEntitleTo.LeavePlanEntitleDays)
                                            {
                                                yearOfServiceFrom = previousLeavePlanYearOfServiceFrom;
                                                yearOfServiceTo = previousLeavePlanYearOfServiceTo;
                                                leavePlanEntitleFrom = previousLeavePlanLeavePlanEntitleFrom;
                                                leavePlanEntitleTo = previousLeavePlanLeavePlanEntitleTo;
                                                prorataEntitlement.ServerYearReferenceDate = tmpServiceReferenceDate;
                                            }
                                        }


                                }
                            }
                        }
                        //prorataEntitlement.ServerYearReferenceDate = getLastServiceDateChange(prorataEntitlement.From).AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceFrom)));




                        if (leavePlanEntitleFrom != null || leavePlanEntitleTo != null)
                        {
                            if (!Convert.ToInt32(Math.Truncate(yearOfServiceFrom)).Equals(Convert.ToInt32(Math.Truncate(yearOfServiceTo))))
                            {
                                //DateTime cutOffDate = getLastServiceDateChange(prorataEntitlement.To).AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo)));
                                DateTime cutOffDate = prorataEntitlement.ServerYearReferenceDate.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo) - Convert.ToInt32(Math.Truncate(yearOfServiceFrom))));
                                //  Swapping Date for split the entitlement into 2
                                DateTime nextToDate = prorataEntitlement.To;
                                prorataEntitlement.To = cutOffDate.AddDays(-1);
                                prorataEntitlement.LeavePlanEntitle = leavePlanEntitleFrom;
                                prorataEntitlementList.Add(prorataEntitlement);

                                prorataEntitlement = new LeaveProrataEntitle();
                                prorataEntitlement.From = cutOffDate;
                                prorataEntitlement.To = nextToDate;
                                //prorataEntitlement.ServerYearReferenceDate = getLastServiceDateChange(prorataEntitlement.To).AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo)));
                                prorataEntitlement.ServerYearReferenceDate = cutOffDate;// DefaultServiceStartDate.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo)));

                            }

                            prorataEntitlement.LeavePlanEntitle = leavePlanEntitleTo;
                            prorataEntitlementList.Add(prorataEntitlement);
                        }


                    }
                }
            }

            return prorataEntitlementList;
        }

        protected ELeavePlanEntitle GetLeavePlanEntitle(int LeavePlanID, double YearOfService)
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("LeavePlanID", LeavePlanID));
            filter.add(new Match("LeaveTypeID", m_LeaveTypeID));
            filter.add(new Match("LeavePlanEntitleYearOfService", ">", YearOfService));
            filter.add("LeavePlanEntitleYearOfService", true);
            ArrayList list = ELeavePlanEntitle.db.select(dbConn, filter);
            if (list.Count > 0)
            {
                return (ELeavePlanEntitle)list[0];
            }
            else
            {
                filter = new DBFilter();
                filter.add(new Match("LeavePlanID", LeavePlanID));
                filter.add(new Match("LeaveTypeID", m_LeaveTypeID));
                filter.add("LeavePlanEntitleYearOfService", false);
                list = ELeavePlanEntitle.db.select(dbConn, filter);
                if (list.Count > 0)
                {
                    return (ELeavePlanEntitle)list[0];
                }
                else
                {
                    //  Use empty entitle with 0 day entitle
                    ELeavePlanEntitle emptyLeavePlanEntitle = new ELeavePlanEntitle();
                    emptyLeavePlanEntitle.LeavePlanID = LeavePlanID;
                    emptyLeavePlanEntitle.LeavePlanEntitleID = m_LeaveTypeID;
                    return emptyLeavePlanEntitle;
                }
            }
        }

        protected virtual void GetLeaveYearStartDate(DateTime AsOfDate, out int cutoffMonth, out int cutoffDay)
        {
            cutoffMonth = DefaultServiceStartDate.Month;
            cutoffDay = DefaultServiceStartDate.Day;

            DateTime NextAsOfDate = AsOfDate;
            if (EntitlePeriodUnit.Equals("Y"))
                NextAsOfDate = NextAsOfDate.AddYears(1);
            else if (EntitlePeriodUnit.Equals("M"))
                NextAsOfDate = NextAsOfDate.AddMonths(1);

            DBFilter empPosWithResetEffectiveDateFilter = new DBFilter();
            empPosWithResetEffectiveDateFilter.add(new Match("EmpID", EmpID));
            empPosWithResetEffectiveDateFilter.add(new Match("EmpPosEffFr", "<", NextAsOfDate));
            empPosWithResetEffectiveDateFilter.add(new Match("EmpPosIsLeavePlanResetEffectiveDate", true));
            empPosWithResetEffectiveDateFilter.add("EmpPosEffFr", false);
            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosWithResetEffectiveDateFilter);
            if (empPosList.Count > 0)
            {
                EEmpPositionInfo lastResetEmpPos = (EEmpPositionInfo)empPosList[0];
                cutoffMonth = lastResetEmpPos.EmpPosEffFr.Month;
                cutoffDay = lastResetEmpPos.EmpPosEffFr.Day;
            }

            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, EmpID);
            if (empPos != null)
            {
                ELeavePlan leavePlan = new ELeavePlan();
                leavePlan.LeavePlanID = empPos.LeavePlanID;
                if (ELeavePlan.db.select(dbConn, leavePlan))
                    if (leavePlan.LeavePlanUseCommonLeaveYear)
                    {
                        cutoffMonth = leavePlan.LeavePlanCommonLeaveYearStartMonth;
                        cutoffDay = leavePlan.LeavePlanCommonLeaveYearStartDay;
                    }
            }

            if (cutoffMonth < 1 || cutoffMonth > 12)
                cutoffMonth = 1;
            if (cutoffDay < 1 || cutoffDay > 31)
                cutoffDay = 1;
        }

        protected virtual double MaximumBroughtForward(DateTime AsOfDate)
        {
            ELeavePlanBroughtForward leavePlanBroughtForward = GetLeavePlanBroughtForward(AsOfDate, EmpID, m_LeaveTypeID);

            if (leavePlanBroughtForward != null)
            {
                return leavePlanBroughtForward.LeavePlanBroughtForwardMax;
            }
            else
                return 9999;

            //EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, EmpID);
            //if (empPos != null)
            //{
            //    ELeavePlan leavePlan = new ELeavePlan();
            //    leavePlan.LeavePlanID = empPos.LeavePlanID;
            //    if (ELeavePlan.db.select(dbConn, leavePlan))
            //    {
            //        DBFilter dbFilter = new DBFilter();
            //        dbFilter.add(new Match("LeavePlanID", leavePlan.LeavePlanID));
            //        dbFilter.add(new Match("LeaveTypeID", m_LeaveTypeID));
            //        ArrayList list = ELeavePlanBroughtForward.db.select(dbConn, dbFilter);
            //        if (list.Count > 0)
            //        {
            //            ELeavePlanBroughtForward leavePlanBroughtForward = (ELeavePlanBroughtForward)list[0];
            //            if (leavePlanBroughtForward.LeavePlanBroughtForwardMax > 0 || leavePlanBroughtForward.LeavePlanBroughtForwardNumOfMonthExpired == 0)
            //            {
            //                //*********** debug*****
            //                m_tab--;
            //                Debug.WriteLine(writeTab() + "MaximumBroughtForward >>> out");

                            
                            
            //                return leavePlanBroughtForward.LeavePlanBroughtForwardMax;
            //            }
            //            else
            //            {
            //            }
            //        }
            //    }
            //}

            return 9999;
        }

        protected virtual void submit(ELeaveBalance balanceItem, ELeaveBalanceEntitle balanceEntitle)
        {

            if (!skipLastEmploymentDateChecking)
            {
                if (balanceItem.LeaveBalanceEffectiveDate <= AppUtils.ServerDateTime() && balanceItem.LeaveTypeID > 0)
                {
                    ELeaveBalance.db.insert(dbConn, balanceItem);
                }
                if (balanceEntitle != null)
                {
                    if (balanceEntitle.LeaveBalanceEntitleEffectiveDate <= AppUtils.ServerDateTime() && balanceEntitle.LeaveTypeID > 0)
                    {

                        if (balanceEntitle.LeaveBalanceEntitleID > 0)
                        {
                            ELeaveBalanceEntitle.db.update(dbConn, balanceEntitle);
                        }
                        else
                        {
                            ELeaveBalanceEntitle.db.insert(dbConn, balanceEntitle);
                        }
                    }
                }
m_tab--;
Debug.WriteLine(writeTab() + "submit >>> out");
            }
        }
    }
    public class LeaveProrataEntitle
    {
        public DateTime From;
        public DateTime To;
        public DateTime ServerYearReferenceDate;
        public ELeavePlanEntitle LeavePlanEntitle;
        //public int YearOfService;
    }

}
