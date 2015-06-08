//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;
//using HROne.Lib.Entities;
//using HROne.DataAccess;
//using HROne.CommonLib;

//namespace HROne.LeaveCalc
//{
//    public class SickLeaveBalanceProcess : LeaveBalanceProcess
//    {
//        protected ArrayList balanceItemListSLCat2 = new ArrayList();
//        protected int LeaveTypeIDSLCat2;
//        protected ELeaveBalance currentSLCat2balanceItem = null;

//        //private SickLeaveCat2BalanceProcess CAT2Process = null;
//        public SickLeaveBalanceProcess(int EmpID)
//            : base(EmpID, 0)
//        {
//            EntitlePeriodUnit = "M";
//            allowEntitleProrata = false;
//            m_LeaveTypeID = ELeaveType.SLCAT1_LEAVE_TYPE(dbConn).LeaveTypeID;
//            LeaveTypeIDSLCat2 = ELeaveType.SLCAT2_LEAVE_TYPE(dbConn).LeaveTypeID;
//        }

//        public override void Recalculate()
//        {
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("EmpID", EmpID));
//            filter.add(new Match("LeaveTypeID", LeaveTypeIDSLCat2));

//            ELeaveBalance.db.delete(dbConn, filter);
//            balanceItemListSLCat2.Clear();
//            base.Recalculate();
//        }

//        public override void RecalculateAfter(DateTime DateAfter)
//        {
//            DBFilter filter = new DBFilter();
//            filter.add(new Match("EmpID", EmpID));
//            filter.add(new Match("LeaveTypeID", LeaveTypeIDSLCat2));
//            filter.add(new Match("LeaveBalanceEffectiveDate", ">", DateAfter));
//            ELeaveBalance.db.delete(dbConn, filter);
//            balanceItemListSLCat2.Clear();
//            base.RecalculateAfter(DateAfter);
//        }

//        protected override void GetCutOffDayTime(DateTime ServiceStartDate, out int cutoffMonth, out int cutoffDay)
//        {
//            cutoffMonth = ServiceStartDate.Month;
//            cutoffDay = ServiceStartDate.Day;
//            return;
//        }

//        protected override void LoadServerData(DateTime AsOfDate)
//        {
//            base.LoadServerData(AsOfDate);

//            DBFilter filter = new DBFilter();
//            filter.add(new Match("EmpID", EmpID));
//            filter.add(new Match("LeaveBalanceEffectiveDate", "<=", AsOfDate));
//            filter.add(new Match("LeaveTypeID", LeaveTypeIDSLCat2));
//            filter.add("LeaveBalanceEffectiveDate", false);
//            balanceItemListSLCat2 = ELeaveBalance.db.select(dbConn, filter);

//            if (balanceItemListSLCat2.Count > 0)
//            {
//                currentSLCat2balanceItem = (ELeaveBalance)balanceItemListSLCat2[0];
//            }
//            //CAT2Process = new SickLeaveCat2BalanceProcess(EmpID, balanceItemList);
//            //CAT2Process.LoadData(AsOfDate);
//        }

//        protected override double CalculateProrata(LeaveProrataEntitle prorataEntitle)
//        {
//            DateTime MonthlyServiceStartDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month);
//            DateTime MonthlyServiceEndDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month + 1);
//            if (MonthlyServiceStartDate > prorataEntitle.To)
//            {
//                MonthlyServiceStartDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month - 1);
//                MonthlyServiceEndDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month);
//            }
//            return ((double)((TimeSpan)prorataEntitle.To.Subtract(prorataEntitle.From)).Days + 1) * prorataEntitle.LeavePlanEntitle.LeavePlanEntitleDays / (((TimeSpan)MonthlyServiceEndDate.Subtract(MonthlyServiceStartDate)).Days);
//        }

//        public ELeaveBalance getLatestCAT2LeaveBalance()
//        {
//            if (balanceItemListSLCat2.Count > 0)
//                return (ELeaveBalance)balanceItemListSLCat2[0];
//            else
//                return null;
//        }

//        public ArrayList getCAT2BalanceHistory()
//        {
//            return balanceItemListSLCat2;
//        }

//        protected override void LoadBalance(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {

//            base.LoadBalance(balanceItem, AsOfDate);
//            if (currentSLCat2balanceItem != null)
//            {
//                currentSLCat2balanceItem.BalanceUnit = m_BalanceUnit;
//                currentSLCat2balanceItem.Balance = currentSLCat2balanceItem.getBalance();
//            }
//        }

//        protected override double LoadTaken(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {

//            if (currentSLCat2balanceItem != null)
//            {
//                DateTime DateFrom, DateTo;
//                DateFrom = balanceItem.LeaveBalanceEffectiveDate;
//                DateTo = balanceItem.LeaveBalanceEffectiveEndDate;
//                currentSLCat2balanceItem.Taken = 0;
//                DBFilter filter = new DBFilter();
//                filter.add(new Match("LeaveAppDateFrom", ">=", DateFrom));
//                filter.add(new Match("EmpID", currentSLCat2balanceItem.EmpID));
//                DBFilter leaveCodeFilter = new DBFilter();
//                leaveCodeFilter.add(new Match("LeaveTypeID", currentSLCat2balanceItem.LeaveTypeID));
//                filter.add(new IN("LeaveCodeID", "Select LeaveCodeID from " + ELeaveCode.db.dbclass.tableName, leaveCodeFilter));
//                ArrayList leaveAppList = ELeaveApplication.db.select(dbConn, filter);
//                foreach (ELeaveApplication la in leaveAppList)
//                {
//                    if (la.LeaveAppDateFrom <= AsOfDate && la.LeaveAppDateFrom <= DateTo)
//                        currentSLCat2balanceItem.Taken += la.LeaveAppDays;
//                    else
//                        currentSLCat2balanceItem.Reserved += la.LeaveAppDays;
//                }
//            }
//            return base.LoadTaken(balanceItem, AsOfDate);
//        }

//        protected override double LoadProrata(ELeaveBalance balanceItem, DateTime AsOfDate)
//        {
//            double prorata = base.LoadProrata(balanceItem, AsOfDate);
//            if (currentSLCat2balanceItem != null)
//            {
//                int maxBF = MaximumBroughtForward(EmpID, balanceItem.LeaveBalanceEffectiveDate, m_LeaveTypeID);
//                if (balanceItem.getBalance() > maxBF && maxBF >= 0)
//                {
//                    double difference = balanceItem.getBalance() - maxBF;
//                    balanceItem.LeaveBalanceEntitled -= difference;
//                    currentSLCat2balanceItem.LeaveBalanceEntitled += difference;
//                }
//            }
//            return prorata;
//        }

//        protected override ELeaveBalance CreateLeaveBalanceItem()
//        {
//            currentSLCat2balanceItem = new ELeaveBalance();
//            currentSLCat2balanceItem.LeaveBalanceEffectiveDate = NextStartDate();
//            currentSLCat2balanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(currentSLCat2balanceItem.LeaveBalanceEffectiveDate);
//            currentSLCat2balanceItem.EmpID = EmpID;
//            currentSLCat2balanceItem.LeaveTypeID = LeaveTypeIDSLCat2;
//            return base.CreateLeaveBalanceItem();
//        }


//        protected override ELeaveBalance CreateNextLeaveBalanceItem(ELeaveBalance lastLeaveBalanceItem)
//        {

//            ELeaveBalance lastSLCat2balanceItem = currentSLCat2balanceItem;
//            ELeaveBalance SLCat1balanceItem = base.CreateNextLeaveBalanceItem(lastLeaveBalanceItem);


//            if (lastSLCat2balanceItem == null && currentSLCat2balanceItem != null)
//            {
//                //  To Resolve the previous bug that does not save new SL2 record
//                //  will be no longer use in future
//                lastSLCat2balanceItem = CreateLeaveBalanceItem();
//            }
//            //            currentSLCat2balanceItem = CreateSLCat2LeaveBalanceItem();

//            //            currentSLCat2balanceItem.LeaveBalanceEffectiveEndDate=NextStartDate(currentSLCat2balanceItem.LeaveBalanceEffectiveDate) ;
//            if (lastSLCat2balanceItem != null && lastLeaveBalanceItem != null)
//            {
//                currentSLCat2balanceItem.LeaveBalanceBF = lastSLCat2balanceItem.Balance;
//                if (SLCat1balanceItem.LeaveBalanceForfeiture > 0)
//                {
//                    SLCat1balanceItem.LeaveBalanceBF -= SLCat1balanceItem.LeaveBalanceForfeiture;
//                    currentSLCat2balanceItem.LeaveBalanceBF += SLCat1balanceItem.LeaveBalanceForfeiture;
//                    SLCat1balanceItem.LeaveBalanceForfeiture = 0;
//                }

//                //  Initial Forfeiture
//                currentSLCat2balanceItem.LeaveBalanceForfeiture = 0;
//                int MaxBF = MaximumBroughtForward(EmpID, currentSLCat2balanceItem.LeaveBalanceEffectiveDate, LeaveTypeIDSLCat2);

//                if (MaxBF >= 0)
//                {
//                    currentSLCat2balanceItem.LeaveBalanceForfeiture = currentSLCat2balanceItem.LeaveBalanceBF - MaxBF;
//                    if (currentSLCat2balanceItem.LeaveBalanceForfeiture < 0)
//                        currentSLCat2balanceItem.LeaveBalanceForfeiture = 0;
//                }
//            }
//            //else
//            //{
//            //    if (lastSLCat2balanceItem != null || lastLeaveBalanceItem != null)
//            //        throw new Exception("SL Cat 1 and Cat 2 not synchronize");
//            //}
//            balanceItemListSLCat2.Insert(0, currentSLCat2balanceItem);
//            submit(currentSLCat2balanceItem);
//            return SLCat1balanceItem;
//        }

//        //private class SickLeaveCat2BalanceProcess : LeaveBalanceProcess
//        //{
//        //    protected ArrayList balanceItemListSLCat1 = null;

//        //    public SickLeaveCat2BalanceProcess(int EmpID, ArrayList Cat1BalanceList)
//        //        : base(EmpID, 0)
//        //    {
//        //        EntitlePeriodUnit = "M";
//        //        LeaveTypeID = ELeaveType.SLCAT2_LEAVE_TYPE(dbConn).LeaveTypeID;
//        //        balanceItemListSLCat1 = Cat1BalanceList;
//        //    }

//        //    protected override void LoadCutOffDate()
//        //    {
//        //        CutOffMonth = ServiceStartDate.Month;
//        //        CutOffDay = ServiceStartDate.Day;
//        //        return;
//        //    }

//        //    protected override double LoadProrata(ELeaveBalance balanceItem, DateTime AsOfDate)
//        //    {

//        //        foreach (ELeaveBalance CAT1BalanceItem in balanceItemListSLCat1)
//        //        {
//        //            if (CAT1BalanceItem.LeaveBalanceEffectiveDate.Equals(balanceItem.LeaveBalanceEffectiveDate))
//        //            {
//        //                double prorata = CAT1BalanceItem.LeaveBalanceForfeiture;
//        //                if (prorata > 0)
//        //                {
//        //                    CAT1BalanceItem.LeaveBalanceForfeiture = 0;
//        //                    CAT1BalanceItem.LeaveBalanceBF -= prorata;
//        //                    submit(CAT1BalanceItem);
//        //                }
//        //                balanceItem.LeaveBalanceEntitled = prorata;
//        //                return prorata;
//        //            }
//        //        }
//        //        return 0;
//        //    }
//        //}
//    }
//}
