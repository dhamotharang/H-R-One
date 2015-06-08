//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;
//using HROne.Lib.Entities;
//using HROne.DataAccess;
//using HROne.CommonLib;

//namespace HROne.LeaveCalc
//{
//    public class LeaveBalanceProcess
//    {
//        protected ArrayList balanceItemList = new ArrayList();
//        protected int EmpID;
//        //protected DateTime AsOfDate;
//        protected string EntitlePeriodUnit = "Y";
//        protected int m_LeaveTypeID;
//        //protected int CutOffDay = 1;
//        //protected int CutOffMonth = 1;
//        protected DateTime DefaultServiceStartDate;
//        protected bool allowEntitleProrata = true;

//        protected ELeaveBalance.LeaveBalanceUnit m_BalanceUnit = ELeaveBalance.LeaveBalanceUnit.Day;

//        public int LeaveTypeID
//        {
//            get
//            {
//                return m_LeaveTypeID;
//            }
//        }

//        public ELeaveBalance.LeaveBalanceUnit BalanceUnit
//        {
//            get
//            {
//                return m_BalanceUnit;
//            }
//        }

//        public LeaveBalanceProcess(int EmpID, int LeaveTypeID)
//        {
//            this.EmpID = EmpID;
//            this.m_LeaveTypeID = LeaveTypeID;

//            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
//            empInfo.EmpID = EmpID;
//            EEmpPersonalInfo.db.select(dbConn, empInfo);
//            DefaultServiceStartDate = empInfo.EmpServiceDate;


//        }

//        public ELeaveBalance getLatestLeaveBalance()
//        {
//            if (balanceItemList.Count > 0)
//                return (ELeaveBalance)balanceItemList[0];
//            else
//                return null;
//        }

//        public ArrayList getBalanceHistory()
//        {
//            return balanceItemList;
//        }

//        protected virtual ELeaveBalance CreateLeaveBalanceItem()
//        {
//            ELeaveBalance balanceItem = new ELeaveBalance();
//            balanceItem.LeaveBalanceEffectiveDate = NextStartDate();
//            balanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(balanceItem.LeaveBalanceEffectiveDate);
//            balanceItem.EmpID = EmpID;
//            balanceItem.LeaveTypeID = m_LeaveTypeID;
//            return balanceItem;
//        }

//        protected virtual ELeaveBalance CreateNextLeaveBalanceItem(ELeaveBalance lastLeaveBalanceItem)
//        {
//            ELeaveBalance balanceItem = CreateLeaveBalanceItem();

//            if (lastLeaveBalanceItem != null)
//            {
//                balanceItem.LeaveBalanceEffectiveDate = lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate.AddDays(1);
//                balanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(balanceItem.LeaveBalanceEffectiveDate).AddDays(-1);
//                balanceItem.LeaveBalanceBF = lastLeaveBalanceItem.Balance;
//                //  Initial Forfeiture
//                balanceItem.LeaveBalanceForfeiture = 0;
//                int MaxBF = MaximumBroughtForward(EmpID, balanceItem.LeaveBalanceEffectiveDate, m_LeaveTypeID);
//                //EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, balanceItem.LeaveBalanceEffectiveDate, EmpID);
//                //if (empPos != null)
//                //{
//                //    ELeavePlan leavePlan = new ELeavePlan();
//                //    leavePlan.LeavePlanID = empPos.LeavePlanID;
//                //    if (ELeavePlan.db.select(dbConn, leavePlan))
//                //    {
//                //        if (LeaveTypeID.Equals(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID))
//                //            MaxBF = leavePlan.LeavePlanALMaxBF;
//                //        else if (LeaveTypeID.Equals(ELeaveType.SLCAT1_LEAVE_TYPE(dbConn).LeaveTypeID))
//                //            MaxBF = leavePlan.LeavePlanSL1MaxBF;
//                //        else if (LeaveTypeID.Equals(ELeaveType.SLCAT2_LEAVE_TYPE(dbConn).LeaveTypeID))
//                //            MaxBF = leavePlan.LeavePlanSL2MaxBF;
//                //    }
//                //}
//                if (MaxBF >= 0)
//                {
//                    balanceItem.LeaveBalanceForfeiture = balanceItem.LeaveBalanceBF - MaxBF;
//                    if (balanceItem.LeaveBalanceForfeiture < 0)
//                        balanceItem.LeaveBalanceForfeiture = 0;
//                }
//            }
//            balanceItemList.Insert(0, balanceItem);
//            submit(balanceItem);
//            return balanceItem;
//        }

//        public virtual void Recalculate()
//        {
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("EmpID", EmpID));
//            filter.add(new Match("LeaveTypeID", m_LeaveTypeID));

//            ELeaveBalance.db.delete(dbConn, filter);
//            LoadData(AppUtils.ServerDateTime().Date);
//        }

//        public virtual void RecalculateAfter(DateTime DateAfter)
//        {
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("EmpID", EmpID));
//            filter.add(new Match("LeaveTypeID", m_LeaveTypeID));
//            filter.add(new Match("LeaveBalanceEffectiveDate", ">", DateAfter));
//            ELeaveBalance.db.delete(dbConn, filter);
//            LoadData(AppUtils.ServerDateTime().Date);
//        }
//        public void LoadData(DateTime AsOfDate)
//        {
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("EmpID", EmpID));
//            filter.add("EmpTermLastDate", false);

//            ArrayList list = EEmpTermination.db.select(dbConn, filter);
//            if (list.Count > 0)
//            {
//                EEmpTermination obj = (EEmpTermination)list[0];
//                if (AsOfDate > obj.EmpTermLastDate)
//                    AsOfDate = obj.EmpTermLastDate;
//            }


//            LoadServerData(AsOfDate);
//            BroughtForwardCalculation(AsOfDate);
//        }

//        //protected virtual void LoadCutOffDate(DateTime AsOfDate)
//        //{
//        //    GetCutOffDayTime(getLastServiceStartDate(AsOfDate), out CutOffMonth, out CutOffDay);
//        //}

//        protected virtual void LoadServerData(DateTime AsOfDate)
//        {
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("EmpID", EmpID));
//            filter.add(new Match("LeaveBalanceEffectiveDate", "<=", AsOfDate));
//            filter.add(new Match("LeaveTypeID", m_LeaveTypeID));
//            filter.add("LeaveBalanceEffectiveDate", false);
//            balanceItemList = ELeaveBalance.db.select(dbConn, filter);
//        }

//        protected virtual void LoadBalance(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {
//            LoadTaken(balanceItem, AsOfDate);
//            LoadProrata(balanceItem, AsOfDate);
//            LoadAdjust(balanceItem, AsOfDate);
//            balanceItem.BalanceUnit = m_BalanceUnit;
//            balanceItem.Balance = balanceItem.getBalance();
//        }

//        protected virtual void BroughtForwardCalculation(DateTime AsOfDate)
//        {
//            ELeaveBalance lastLeaveBalanceItem;
//            if (balanceItemList.Count > 0)
//            {
//                lastLeaveBalanceItem = (ELeaveBalance)balanceItemList[0];
//            }
//            else
//            {
//                lastLeaveBalanceItem = CreateNextLeaveBalanceItem(null);//CreateLeaveBalanceItem();
//                //LoadBalance(lastLeaveBalanceItem, AsOfDate);
//                //submit(lastLeaveBalanceItem);
//                //balanceItemList.Insert(0, lastLeaveBalanceItem);
//            }
//            lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(lastLeaveBalanceItem.LeaveBalanceEffectiveDate).AddDays(-1);
//            while (lastLeaveBalanceItem.LeaveBalanceEffectiveEndDate < AsOfDate)
//            {
//                LoadBalance(lastLeaveBalanceItem, AsOfDate);
//                ELeaveBalance balanceItem = CreateNextLeaveBalanceItem(lastLeaveBalanceItem);

//                //submit(balanceItem);
//                //balanceItemList.Insert(0, balanceItem);
//                lastLeaveBalanceItem = balanceItem;
//            }
//            LoadBalance(lastLeaveBalanceItem, AsOfDate);
//        }

//        protected DateTime NextStartDate()
//        {
//            if (balanceItemList.Count > 0)
//            {
//                return NextStartDate(((ELeaveBalance)balanceItemList[0]).LeaveBalanceEffectiveDate);
//            }
//            else
//            {
//                return DefaultServiceStartDate;
//            }
//        }

//        protected virtual DateTime NextStartDate(DateTime lastEffDate)
//        {
//            DateTime nextEffDate = new DateTime(9999, 9, 9);

//            //  Get next Start Date by default
//            int cutOffMonth, cutOffDay;
//            //            GetCutOffDayTime(getLastServiceDateChange(lastEffDate), out cutOffMonth, out cutOffDay);
//            GetCutOffDayTime(DefaultServiceStartDate, out cutOffMonth, out cutOffDay);

//            if (EntitlePeriodUnit.Equals("Y"))
//            {
//                nextEffDate = new DateTime(lastEffDate.Year, cutOffMonth, 1);
//                if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
//                    nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
//                else
//                    nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
//                while (nextEffDate <= lastEffDate)
//                {
//                    nextEffDate = nextEffDate.AddDays(-nextEffDate.Day + 1);
//                    nextEffDate = nextEffDate.AddYears(1);
//                    if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
//                        nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
//                    else
//                        nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
//                }
//            }
//            else if (EntitlePeriodUnit.Equals("M"))
//            {
//                nextEffDate = new DateTime(lastEffDate.Year, lastEffDate.Month, 1);
//                if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
//                    nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
//                else
//                    nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
//                while (nextEffDate <= lastEffDate)
//                {
//                    nextEffDate = nextEffDate.AddDays(-nextEffDate.Day + 1);
//                    nextEffDate = nextEffDate.AddMonths(1);
//                    if (DateTime.DaysInMonth(nextEffDate.Year, nextEffDate.Month) >= cutOffDay)
//                        nextEffDate = nextEffDate.AddDays(cutOffDay - 1);
//                    else
//                        nextEffDate = nextEffDate.AddMonths(1).AddDays(-1);
//                }
//            }

//            if (ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_LEAVE_ENTITLE_CUT_OFF_BY_SERVICE_DATE).Equals("Y"))
//            {
//                //  Check if there is another service date within "lastEffDate" and "nextEffDate"
//                DateTime lastServiceDate = DefaultServiceStartDate;// getLastServiceDateChange(nextEffDate);
//                if (lastEffDate < lastServiceDate)
//                {
//                    //  use service date between "lastEffDate" and "nextEffDate" if there is another service date within "lastEffDate" and "nextEffDate"
//                    nextEffDate = lastServiceDate;
//                }
//            }
//            return nextEffDate;
//        }

//        //protected DateTime getLastServiceDateChange(DateTime AsOfDate)
//        //{
//        //    DateTime lastServiceDateChange = DefaultServiceStartDate;

//        //    if (ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_LEAVE_ENTITLE_AUTO_RESET_SERVICE_DATE).Equals("Y"))
//        //    {
//        //        int previousLeavePlanID = 0;
//        //        DateTime previousLeavePlanLastEffDate = DefaultServiceStartDate;


//        //        DBFilter empPreviousPosFilter = new DBFilter();
//        //        empPreviousPosFilter.add(new Match("EmpID", EmpID));
//        //        empPreviousPosFilter.add(new Match("EmpPosEffFr", "<=", AsOfDate));
//        //        empPreviousPosFilter.add("EmpPosEffFr", true);
//        //        ArrayList empPreviousPosList = EEmpPositionInfo.db.select(dbConn, empPreviousPosFilter);

//        //        foreach (EEmpPositionInfo empPos in empPreviousPosList)
//        //        {
//        //            if (empPos.LeavePlanID != 0)
//        //            {
//        //                if (previousLeavePlanID != empPos.LeavePlanID)
//        //                {
//        //                    if (previousLeavePlanID != 0)
//        //                    {
//        //                        ELeavePlanEntitle leavePlanEntitlePrevious = GetLeavePlanEntitle(previousLeavePlanID, HROne.CommonLib.Utility.YearDifference(lastServiceDateChange, previousLeavePlanLastEffDate));
//        //                        ELeavePlanEntitle leavePlanEntitleNext = GetLeavePlanEntitle(empPos.LeavePlanID, 0);
//        //                        if (leavePlanEntitlePrevious != null && leavePlanEntitleNext != null)
//        //                        {
//        //                            if (leavePlanEntitleNext.LeavePlanEntitleDays > leavePlanEntitlePrevious.LeavePlanEntitleDays)
//        //                            {
//        //                                lastServiceDateChange = empPos.EmpPosEffFr;
//        //                            }
//        //                        }
//        //                    }

//        //                    previousLeavePlanID = empPos.LeavePlanID;
//        //                }
//        //                previousLeavePlanLastEffDate = empPos.EmpPosEffTo;
//        //            }
//        //        }
//        //    }
//        //    return lastServiceDateChange;
//        //}

//        protected virtual double LoadTaken(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {

//            DateTime DateFrom, DateTo;
//            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
//            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
//            balanceItem.Taken = 0;
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("LeaveAppDateFrom", ">=", DateFrom));
//            filter.add(new Match("EmpID", balanceItem.EmpID));
//            DBFilter leaveCodeFilter = new DBFilter();
//            leaveCodeFilter.add(new Match("LeaveTypeID", balanceItem.LeaveTypeID));
//            filter.add(new IN("LeaveCodeID", "Select LeaveCodeID from " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));
//            ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, filter);
//            balanceItem.Taken = 0;
//            balanceItem.Reserved = 0;
//            foreach (ELeaveApplication la in leaveAppList)
//            {
//                if (la.LeaveAppDateFrom <= AsOfDate && la.LeaveAppDateFrom <= DateTo)
//                    if (m_BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
//                        balanceItem.Taken += la.LeaveAppHours;
//                    else
//                        balanceItem.Taken += la.LeaveAppDays;

//                else
//                    if (m_BalanceUnit == ELeaveBalance.LeaveBalanceUnit.Hour)
//                        balanceItem.Reserved += la.LeaveAppHours;
//                    else
//                        balanceItem.Reserved += la.LeaveAppDays;
//            }
//            return balanceItem.Taken;
//        }

//        protected virtual double LoadAdjust(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {

//            DateTime DateFrom, DateTo;
//            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
//            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
//            if (DateTo >= AsOfDate)
//                DateTo = AsOfDate;

//            DBFilter filter = new DBFilter();
//            filter.add(new Match("LeaveBalAdjDate", ">=", DateFrom));
//            filter.add(new Match("LeaveBalAdjDate", "<=", DateTo));
//            filter.add(new Match("EmpID", balanceItem.EmpID));
//            filter.add(new Match("LeaveTypeID", balanceItem.LeaveTypeID));
//            filter.add("LeaveBalAdjDate", true);
//            ArrayList leaveBalAdjList = ELeaveBalanceAdjustment.db.select(dbConn, filter);
//            balanceItem.Adjust = 0;
//            foreach (ELeaveBalanceAdjustment la in leaveBalAdjList)
//            {
//                balanceItem.Adjust += la.LeaveBalAdjValue;
//                if (la.LeaveBalAdjType.Equals(ELeaveBalanceAdjustment.RESET_BALANCE_VALUE))
//                {
//                    ELeaveBalance tmpBalanceItem = new ELeaveBalance();
//                    ELeaveBalance.db.copyObject(balanceItem, tmpBalanceItem);
//                    tmpBalanceItem.LeaveBalanceEffectiveEndDate = balanceItem.LeaveBalanceEffectiveEndDate;
//                    LoadTaken(tmpBalanceItem, la.LeaveBalAdjDate);
//                    LoadProrata(tmpBalanceItem, la.LeaveBalAdjDate);
//                    balanceItem.Adjust -= tmpBalanceItem.getBalance();
//                }
//            }
//            return balanceItem.Taken;
//        }

//        protected virtual double LoadProrata(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {
//            double prorata = 0;
//            DateTime DateFrom, DateTo;
//            DateFrom = balanceItem.LeaveBalanceEffectiveDate;
//            DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
//            if (DateTo > AsOfDate)
//                if (allowEntitleProrata)
//                    DateTo = AsOfDate;
//                else
//                {
//                    balanceItem.LeaveBalanceEntitled = 0;
//                    return 0;
//                }
//            ArrayList timeFlameList = LoadProrataEntitlementList(DateFrom, DateTo);
//            foreach (LeaveProrataEntitle prorataEntitle in timeFlameList)
//            {
//                prorata += CalculateProrata(prorataEntitle);
//            }
//            balanceItem.LeaveBalanceEntitled = prorata;
//            return prorata;
//        }

//        protected virtual double CalculateProrata(LeaveProrataEntitle prorataEntitle)
//        {
//            ELeavePlan leavePlan = new ELeavePlan();
//            leavePlan.LeavePlanID = prorataEntitle.LeavePlanEntitle.LeavePlanID;
//            if (ELeavePlan.db.select(dbConn, leavePlan))
//            {
//                DateTime dateFeb29 = new DateTime();
//                int numberOfDayWithinPeriod = ((TimeSpan)prorataEntitle.ServerYearReferenceDate.AddYears(1).Subtract(prorataEntitle.ServerYearReferenceDate)).Days;
//                int numberOfDayProrata = ((TimeSpan)prorataEntitle.To.Subtract(prorataEntitle.From)).Days + 1;
//                if (leavePlan.LeavePlanProrataSkipFeb29)
//                {
//                    if (numberOfDayWithinPeriod.Equals(366))
//                        numberOfDayWithinPeriod = 365;
//                    if (DateTime.IsLeapYear(prorataEntitle.To.Year))
//                        dateFeb29 = new DateTime(prorataEntitle.To.Year, 2, 29);
//                    if (DateTime.IsLeapYear(prorataEntitle.From.Year))
//                        dateFeb29 = new DateTime(prorataEntitle.From.Year, 2, 29);
//                    if (!dateFeb29.Ticks.Equals(0))
//                        if (prorataEntitle.From <= dateFeb29 && prorataEntitle.To >= dateFeb29)
//                            numberOfDayProrata--;
//                }
//                return ((double)numberOfDayProrata) * prorataEntitle.LeavePlanEntitle.LeavePlanEntitleDays / ((double)numberOfDayWithinPeriod);
//            }
//            else
//                return 0;
//        }

//        protected ArrayList LoadProrataEntitlementList(DateTime FromDate, DateTime ToDate)
//        {

//            ArrayList prorataEntitlementList = new ArrayList();

//            DBFilter empPosFilter = new DBFilter();
//            empPosFilter.add(new Match("EmpID", EmpID));
//            empPosFilter.add(new Match("EmpPosEffFr", "<=", ToDate));
//            //OR orPosEffTerms = new OR();
//            //orPosEffTerms.add(new Match("EmpPosEffTo", ">=", FromDate));
//            //orPosEffTerms.add(new NullTerm("EmpPosEffTo"));
//            //empPosFilter.add(orPosEffTerms);
//            empPosFilter.add("EmpPosEffFr", true);
//            ArrayList empPosList = EEmpPositionInfo.db.select(dbConn, empPosFilter);

//            ArrayList mergePositionList = new ArrayList();

//            foreach (EEmpPositionInfo empPos in empPosList)
//            {
//                if (mergePositionList.Count <= 0)
//                    mergePositionList.Add(empPos);
//                else
//                {
//                    EEmpPositionInfo lastPosition = (EEmpPositionInfo)mergePositionList[mergePositionList.Count - 1];
//                    if (lastPosition.EmpPosEffTo.AddDays(1).Equals(empPos.EmpPosEffFr) && lastPosition.LeavePlanID.Equals(empPos.LeavePlanID))
//                        lastPosition.EmpPosEffTo = empPos.EmpPosEffTo;
//                    else
//                        mergePositionList.Add(empPos);
//                }
//            }

//            foreach (EEmpPositionInfo empPos in mergePositionList)
//            {
//                if (empPos.EmpPosEffTo.Ticks.Equals(0) || empPos.EmpPosEffTo >= FromDate)
//                {
//                    ELeavePlan leavePlan = new ELeavePlan();
//                    leavePlan.LeavePlanID = empPos.LeavePlanID;
//                    if (ELeavePlan.db.select(dbConn, leavePlan))
//                    {

//                        LeaveProrataEntitle prorataEntitlement = new LeaveProrataEntitle();

//                        prorataEntitlement.From = empPos.EmpPosEffFr < FromDate ? FromDate : empPos.EmpPosEffFr;

//                        if (empPos.EmpPosEffTo > empPos.EmpPosEffFr)
//                            prorataEntitlement.To = empPos.EmpPosEffTo > ToDate ? ToDate : empPos.EmpPosEffTo;
//                        else
//                            prorataEntitlement.To = ToDate;


//                        double yearOfServiceFrom;
//                        double yearOfServiceTo;

//                        if (leavePlan.LeavePlanResetYearOfService)
//                        {
//                            yearOfServiceFrom = Utility.YearDifference(empPos.EmpPosEffFr, prorataEntitlement.From);
//                            yearOfServiceTo = Utility.YearDifference(empPos.EmpPosEffFr, prorataEntitlement.To);
//                            prorataEntitlement.ServerYearReferenceDate = empPos.EmpPosEffFr.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceFrom)));
//                        }
//                        else
//                        {
//                            yearOfServiceFrom = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.From);
//                            yearOfServiceTo = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.To);
//                            prorataEntitlement.ServerYearReferenceDate = DefaultServiceStartDate.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceFrom)));
//                        }

//                        if (leavePlan.LeavePlanNoCountFirstIncompleteYearOfService)
//                        {
//                            prorataEntitlement.ServerYearReferenceDate = FromDate;
//                            yearOfServiceTo = yearOfServiceFrom;
//                        }
//                        ELeavePlanEntitle leavePlanEntitleFrom = GetLeavePlanEntitle(empPos.LeavePlanID, yearOfServiceFrom);
//                        ELeavePlanEntitle leavePlanEntitleTo = GetLeavePlanEntitle(empPos.LeavePlanID, yearOfServiceTo);

//                        if (leavePlan.LeavePlanComparePreviousLeavePlan)
//                        {
//                            foreach (EEmpPositionInfo empPreviousPos in mergePositionList)
//                            {
//                                if (empPreviousPos.EmpPosEffFr < empPos.EmpPosEffFr)
//                                {
//                                    ELeavePlan leavePreviousPlan = new ELeavePlan();
//                                    leavePreviousPlan.LeavePlanID = empPreviousPos.LeavePlanID;
//                                    if (ELeavePlan.db.select(dbConn, leavePreviousPlan))
//                                        if (leavePlan.LeavePlanLeavePlanCompareRank >= leavePreviousPlan.LeavePlanLeavePlanCompareRank)
//                                        {
//                                            double previousLeavePlanYearOfServiceFrom;
//                                            double previousLeavePlanYearOfServiceTo;
//                                            DateTime tmpServiceReferenceDate;
//                                            if (leavePreviousPlan.LeavePlanResetYearOfService)
//                                            {
//                                                previousLeavePlanYearOfServiceFrom = Utility.YearDifference(empPreviousPos.EmpPosEffFr, prorataEntitlement.From);
//                                                previousLeavePlanYearOfServiceTo = Utility.YearDifference(empPreviousPos.EmpPosEffFr, prorataEntitlement.To);
//                                                tmpServiceReferenceDate = empPreviousPos.EmpPosEffFr.AddYears(Convert.ToInt32(Math.Truncate(previousLeavePlanYearOfServiceFrom)));
//                                            }
//                                            else
//                                            {
//                                                previousLeavePlanYearOfServiceFrom = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.From);
//                                                previousLeavePlanYearOfServiceTo = Utility.YearDifference(DefaultServiceStartDate, prorataEntitlement.To);
//                                                tmpServiceReferenceDate = DefaultServiceStartDate.AddYears(Convert.ToInt32(Math.Truncate(previousLeavePlanYearOfServiceFrom)));
//                                            }

//                                            if (leavePreviousPlan.LeavePlanNoCountFirstIncompleteYearOfService)
//                                            {
//                                                previousLeavePlanYearOfServiceTo = previousLeavePlanYearOfServiceFrom;
//                                                tmpServiceReferenceDate = FromDate;
//                                            }
//                                            ELeavePlanEntitle previousLeavePlanLeavePlanEntitleFrom = GetLeavePlanEntitle(leavePreviousPlan.LeavePlanID, previousLeavePlanYearOfServiceFrom);
//                                            ELeavePlanEntitle previousLeavePlanLeavePlanEntitleTo = GetLeavePlanEntitle(leavePreviousPlan.LeavePlanID, previousLeavePlanYearOfServiceTo);

//                                            if (previousLeavePlanLeavePlanEntitleTo.LeavePlanEntitleDays > leavePlanEntitleTo.LeavePlanEntitleDays)
//                                            {
//                                                yearOfServiceFrom = previousLeavePlanYearOfServiceFrom;
//                                                yearOfServiceTo = previousLeavePlanYearOfServiceTo;
//                                                leavePlanEntitleFrom = previousLeavePlanLeavePlanEntitleFrom;
//                                                leavePlanEntitleTo = previousLeavePlanLeavePlanEntitleTo;
//                                                prorataEntitlement.ServerYearReferenceDate = tmpServiceReferenceDate;
//                                            }
//                                        }


//                                }
//                            }
//                        }
//                        //prorataEntitlement.ServerYearReferenceDate = getLastServiceDateChange(prorataEntitlement.From).AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceFrom)));




//                        if (leavePlanEntitleFrom != null || leavePlanEntitleTo != null)
//                        {
//                            if (!Convert.ToInt32(Math.Truncate(yearOfServiceFrom)).Equals(Convert.ToInt32(Math.Truncate(yearOfServiceTo))))
//                            {
//                                //DateTime cutOffDate = getLastServiceDateChange(prorataEntitlement.To).AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo)));
//                                DateTime cutOffDate = prorataEntitlement.ServerYearReferenceDate.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo) - Convert.ToInt32(Math.Truncate(yearOfServiceFrom))));
//                                //  Swapping Date for split the entitlement into 2
//                                DateTime nextToDate = prorataEntitlement.To;
//                                prorataEntitlement.To = cutOffDate.AddDays(-1);
//                                prorataEntitlement.LeavePlanEntitle = leavePlanEntitleFrom;
//                                prorataEntitlementList.Add(prorataEntitlement);

//                                prorataEntitlement = new LeaveProrataEntitle();
//                                prorataEntitlement.From = cutOffDate;
//                                prorataEntitlement.To = nextToDate;
//                                //prorataEntitlement.ServerYearReferenceDate = getLastServiceDateChange(prorataEntitlement.To).AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo)));
//                                prorataEntitlement.ServerYearReferenceDate = cutOffDate;// DefaultServiceStartDate.AddYears(Convert.ToInt32(Math.Truncate(yearOfServiceTo)));

//                            }

//                            prorataEntitlement.LeavePlanEntitle = leavePlanEntitleTo;
//                            prorataEntitlementList.Add(prorataEntitlement);
//                        }


//                    }
//                }
//            }

//            //DBFilter empPosFilter = new DBFilter();
//            //empPosFilter.add(new Match("EmpPosEffFr", "<=", ToDate));

//            return prorataEntitlementList;
//        }

//        protected ELeavePlanEntitle GetLeavePlanEntitle(int LeavePlanID, double YearOfService)
//        {
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("LeavePlanID", LeavePlanID));
//            filter.add(new Match("LeaveTypeID", m_LeaveTypeID));
//            filter.add(new Match("LeavePlanEntitleYearOfService", ">", YearOfService));
//            filter.add("LeavePlanEntitleYearOfService", true);
//            ArrayList list = ELeavePlanEntitle.db.select(dbConn, filter);
//            if (list.Count > 0)
//                return (ELeavePlanEntitle)list[0];
//            else
//            {
//                filter = new DBFilter();
//                filter.add(new Match("LeavePlanID", LeavePlanID));
//                filter.add(new Match("LeaveTypeID", m_LeaveTypeID));
//                filter.add("LeavePlanEntitleYearOfService", false);
//                list = ELeavePlanEntitle.db.select(dbConn, filter);
//                if (list.Count > 0)
//                    return (ELeavePlanEntitle)list[0];
//                else
//                    //  Use empty entitle with 0 day entitle
//                    return new ELeavePlanEntitle();
//            }
//        }

//        protected virtual void GetCutOffDayTime(DateTime ServiceStartDate, out int cutoffMonth, out int cutoffDay)
//        {

//            ESystemParameter param = new ESystemParameter();
//            param.ParameterCode = ESystemParameter.PARAM_CODE_LEAVE_ENTITLE_CUT_OFF_BY_SERVICE_DATE;
//            if (ESystemParameter.db.select(dbConn, param))
//            {
//                if (param.ParameterValue.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
//                {
//                    cutoffMonth = ServiceStartDate.Month;
//                    cutoffDay = ServiceStartDate.Day;
//                    return;
//                }

//            }
//            param.ParameterCode = ESystemParameter.PARAM_CODE_LEAVE_ENTITLE_CUT_OFF_MONTH;
//            if (ESystemParameter.db.select(dbConn, param))
//            {
//                if (!int.TryParse(param.ParameterValue, out cutoffMonth))
//                    cutoffMonth = 1;

//            }
//            else
//                cutoffMonth = 1;

//            param.ParameterCode = ESystemParameter.PARAM_CODE_LEAVE_ENTITLE_CUT_OFF_DAY;
//            if (ESystemParameter.db.select(dbConn, param))
//            {
//                if (!int.TryParse(param.ParameterValue, out cutoffDay))
//                    cutoffDay = 1;

//            }
//            else
//                cutoffDay = 1;
//        }

//        protected static int MaximumBroughtForward(int EmpID, DateTime AsOfDate, int LeaveTypeID)
//        {
//            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, EmpID);
//            if (empPos != null)
//            {
//                ELeavePlan leavePlan = new ELeavePlan();
//                leavePlan.LeavePlanID = empPos.LeavePlanID;
//                if (ELeavePlan.db.select(dbConn, leavePlan))
//                {
//                    DBFilter dbFilter = new DBFilter();
//                    dbFilter.add(new Match("LeavePlanID", leavePlan.LeavePlanID));
//                    dbFilter.add(new Match("LeaveTypeID", LeaveTypeID));
//                    ArrayList list = ELeavePlanBroughtForward.db.select(dbConn, dbFilter);
//                    if (list.Count > 0)
//                    {
//                        ELeavePlanBroughtForward leavePlanBroughtForward = (ELeavePlanBroughtForward)list[0];
//                        return leavePlanBroughtForward.LeavePlanBroughtForwardMax;
//                    }

//                    //if (LeaveTypeID.Equals(ELeaveType.ANNUAL_LEAVE_TYPE(dbConn).LeaveTypeID))
//                    //    return leavePlan.LeavePlanALMaxBF;
//                    //else if (LeaveTypeID.Equals(ELeaveType.SLCAT1_LEAVE_TYPE(dbConn).LeaveTypeID))
//                    //    return leavePlan.LeavePlanSL1MaxBF;
//                    //else if (LeaveTypeID.Equals(ELeaveType.SLCAT2_LEAVE_TYPE(dbConn).LeaveTypeID))
//                    //    return leavePlan.LeavePlanSL2MaxBF;
//                    //else return -1;

//                }
//            }
//            return 9999;
//        }
//        protected static void submit(ELeaveBalance balanceItem)
//        {
//            if (balanceItem.LeaveBalanceEffectiveDate <= AppUtils.ServerDateTime())
//            {
//                ELeaveBalance.db.insert(dbConn, balanceItem);
//            }
//        }
//    }
//    public class LeaveProrataEntitle
//    {
//        public DateTime From;
//        public DateTime To;
//        public DateTime ServerYearReferenceDate;
//        public ELeavePlanEntitle LeavePlanEntitle;
//        //public int YearOfService;
//    }

//}
