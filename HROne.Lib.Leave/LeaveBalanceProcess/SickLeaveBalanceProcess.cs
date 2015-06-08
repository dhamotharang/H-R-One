using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.LeaveCalc
{
    public class SickLeaveBalanceProcess : LeaveBalanceProcess
    {
        protected ArrayList balanceItemListSLCat2 = new ArrayList();
        protected int leaveTypeIDSLCat2 = -1;
        protected bool leaveTypeDisabledSLCat2 = true;
        protected ELeaveBalance currentSLCat2balanceItem = null;

        //private SickLeaveCat2BalanceProcess CAT2Process = null;
        public SickLeaveBalanceProcess(DatabaseConnection dbConn, int EmpID)
            : base(dbConn, EmpID, 0)
        {
            EntitlePeriodUnit = "M";
            allowEntitleProrata = false;
            m_LeaveTypeID = ELeaveType.SLCAT1_LEAVE_TYPE(dbConn).LeaveTypeID;

            ELeaveType Cat2LeaveType = ELeaveType.SLCAT2_LEAVE_TYPE(dbConn);
            if (Cat2LeaveType != null)
            {
                leaveTypeIDSLCat2 = Cat2LeaveType.LeaveTypeID;
                leaveTypeDisabledSLCat2 = Cat2LeaveType.LeaveTypeIsDisabled;


                // Start 2013-12-02, Ricky So, pretend CAT2 always enabled to retain Jimmy's calculation.
                leaveTypeDisabledSLCat2 = false;
                // End 2013-12-02, Ricky So, pretend CAT2 always enabled to retain Jimmy's calculation.
            }
        }

        public override void Recalculate()
        {
            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("LeaveTypeID", leaveTypeIDSLCat2));  // if Cat2 leave type is disabled, LeaveTypeIDSLCat2 is -1

            ELeaveBalance.db.delete(dbConn, filter);
            ELeaveBalanceEntitle.db.delete(dbConn, filter);
            balanceItemListSLCat2.Clear();
            base.Recalculate();
        }

        //public override void RecalculateAfter(DateTime DateAfter)
        //{
        //    ELeaveBalance.DeleteLeaveBalanceAfter(dbConn, EmpID, LeaveTypeIDSLCat2, DateAfter);
        //    balanceItemListSLCat2.Clear();
        //    base.RecalculateAfter(DateAfter);
        //}

        protected override void GetLeaveYearStartDate(DateTime AsOfDate, out int cutoffMonth, out int cutoffDay)
        {
            cutoffMonth = DefaultServiceStartDate.Month;
            cutoffDay = DefaultServiceStartDate.Day;
            return;
        }

        protected override void LoadServerData(DateTime AsOfDate)
        {
            base.LoadServerData(AsOfDate);

            DBFilter filter = new DBFilter();
            filter.add(new Match("EmpID", EmpID));
            filter.add(new Match("LeaveBalanceEffectiveDate", "<=", AsOfDate));
            filter.add(new Match("LeaveTypeID", leaveTypeIDSLCat2));
            filter.add("LeaveBalanceEffectiveDate", false);
            balanceItemListSLCat2 = ELeaveBalance.db.select(dbConn, filter);

            if (balanceItemListSLCat2.Count > 0)
            {
                currentSLCat2balanceItem = (ELeaveBalance)balanceItemListSLCat2[0];
            }
            //CAT2Process = new SickLeaveCat2BalanceProcess(EmpID, balanceItemList);
            //CAT2Process.LoadData(AsOfDate);
        }

        protected override double CalculateProrata(LeaveProrataEntitle prorataEntitle)
        {
            DateTime MonthlyServiceStartDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month);
            DateTime MonthlyServiceEndDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month + 1);
            if (MonthlyServiceStartDate > prorataEntitle.To)
            {
                MonthlyServiceStartDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month - 1);
                MonthlyServiceEndDate = prorataEntitle.ServerYearReferenceDate.AddMonths((prorataEntitle.From.Year - prorataEntitle.ServerYearReferenceDate.Year) * 12 + prorataEntitle.From.Month - prorataEntitle.ServerYearReferenceDate.Month);
            }
            return ((double)((TimeSpan)prorataEntitle.To.Subtract(prorataEntitle.From)).Days + 1) * prorataEntitle.LeavePlanEntitle.LeavePlanEntitleDays / (((TimeSpan)MonthlyServiceEndDate.Subtract(MonthlyServiceStartDate)).Days);
        }

        public ELeaveBalance getLatestCAT2LeaveBalance()
        {
            if (balanceItemListSLCat2.Count > 0)
                return (ELeaveBalance)balanceItemListSLCat2[0];
            else
                return null;
        }

        public ArrayList getCAT2BalanceHistory()
        {
            return balanceItemListSLCat2;
        }

        protected override void LoadBalance(ELeaveBalance balanceItem, DateTime AsOfDate)
        {

            base.LoadBalance(balanceItem, AsOfDate);

            // if CAT2 is enabled
            if (leaveTypeDisabledSLCat2 == false && currentSLCat2balanceItem != null)
            {
                currentSLCat2balanceItem.BalanceUnit = m_BalanceUnit;
                //currentSLCat2balanceItem.Balance = currentSLCat2balanceItem.getBalance();
            }
        }

        protected override void BroughtForwardCalculation(DateTime AsOfDate)
        {
            // if CAT2 is enabled
            if (leaveTypeDisabledSLCat2 == false)
            {
                ELeaveBalance lastSL2LeaveBalanceItem;
                if (balanceItemListSLCat2.Count > 0)
                {
                    lastSL2LeaveBalanceItem = (ELeaveBalance)balanceItemListSLCat2[0];
                }
                else
                {
                    lastSL2LeaveBalanceItem = CreateLeaveBalanceItemForCat2();//CreateLeaveBalanceItem();
                }
                lastSL2LeaveBalanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(lastSL2LeaveBalanceItem.LeaveBalanceEffectiveDate).AddDays(-1);
            }
            base.BroughtForwardCalculation(AsOfDate);
        }

        protected override double LoadTaken(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            // if CAT2 is enabled and balanceItem is not null
            if (leaveTypeDisabledSLCat2 == false && currentSLCat2balanceItem != null)
            {
                base.LoadTaken(currentSLCat2balanceItem, AsOfDate);
            }
            return base.LoadTaken(balanceItem, AsOfDate);
        }

        protected override double LoadAdjust(ELeaveBalance balanceItem, DateTime AsOfDate)
        {

            // if CAT2 is enabled and balanceItem is not null
            if (leaveTypeDisabledSLCat2 == false && currentSLCat2balanceItem != null)
            {
                base.LoadAdjust(currentSLCat2balanceItem, AsOfDate);
            }
            return base.LoadAdjust(balanceItem, AsOfDate);
        }

        protected override double LoadProrata(ELeaveBalance balanceItem, DateTime AsOfDate)
        {
            double prorata = base.LoadProrata(balanceItem, AsOfDate);

            // if CAT2 is enabled...
            if (leaveTypeDisabledSLCat2 == false && balanceItem.LeaveTypeID != leaveTypeIDSLCat2 && currentSLCat2balanceItem != null)
            {
                double maxBF = MaximumBroughtForward(balanceItem.LeaveBalanceEffectiveDate);
                if (balanceItem.getBalance() > maxBF && maxBF >= 0)
                {
                    double difference = balanceItem.getBalance() - maxBF;
                    balanceItem.LeaveBalanceEntitled -= difference;

                    currentSLCat2balanceItem.LeaveBalanceEntitled += difference;
                }
            }
            return prorata;
        }

        protected override ELeaveBalance CreateLeaveBalanceItem()
        {
            if (leaveTypeDisabledSLCat2 == false)
                currentSLCat2balanceItem = CreateLeaveBalanceItemForCat2();
            return base.CreateLeaveBalanceItem();
        }

        protected ELeaveBalance CreateLeaveBalanceItemForCat2()
        {
            if (leaveTypeDisabledSLCat2 == false)
            {
                currentSLCat2balanceItem = new ELeaveBalance();
                currentSLCat2balanceItem.LeaveBalanceEffectiveDate = NextStartDate();
                currentSLCat2balanceItem.LeaveBalanceEffectiveEndDate = NextStartDate(currentSLCat2balanceItem.LeaveBalanceEffectiveDate).AddDays(-1);
                currentSLCat2balanceItem.EmpID = EmpID;
                currentSLCat2balanceItem.LeaveTypeID = leaveTypeIDSLCat2;
                return currentSLCat2balanceItem;
            }
            else
                return null; 
        }

        protected override ELeaveBalance CreateNextLeaveBalanceItem(ELeaveBalance lastLeaveBalanceItem)
        {

            if (leaveTypeDisabledSLCat2 == false)
            {
                ELeaveBalance lastSLCat2balanceItem = currentSLCat2balanceItem;
                ELeaveBalance SLCat1balanceItem = base.CreateNextLeaveBalanceItem(lastLeaveBalanceItem);
                ELeaveBalanceEntitle leaveBalanceEntitleSL2 = null;

                if (lastSLCat2balanceItem != null && lastLeaveBalanceItem != null)
                {

                    currentSLCat2balanceItem.LeaveBalanceBF = lastSLCat2balanceItem.getBalance();
                    if (SLCat1balanceItem.LeaveBalanceForfeiture > 0)
                    {
                        SLCat1balanceItem.LeaveBalanceBF -= SLCat1balanceItem.LeaveBalanceForfeiture;
                        currentSLCat2balanceItem.LeaveBalanceBF += SLCat1balanceItem.LeaveBalanceForfeiture;
                        SLCat1balanceItem.LeaveBalanceForfeiture = 0;
                    }

                    if (lastSLCat2balanceItem.LeaveBalanceEntitled > 0)
                    {
                        leaveBalanceEntitleSL2 = new ELeaveBalanceEntitle();
                        leaveBalanceEntitleSL2.LeaveBalanceEntitleEffectiveDate = currentSLCat2balanceItem.LeaveBalanceEffectiveDate;
                        leaveBalanceEntitleSL2.LeaveBalanceEntitleGrantPeriodFrom = lastSLCat2balanceItem.LeaveBalanceEffectiveDate;
                        leaveBalanceEntitleSL2.LeaveBalanceEntitleGrantPeriodTo = lastSLCat2balanceItem.LeaveBalanceEffectiveEndDate;
                        leaveBalanceEntitleSL2.EmpID = currentSLCat2balanceItem.EmpID;
                        leaveBalanceEntitleSL2.LeaveTypeID = currentSLCat2balanceItem.LeaveTypeID;
                        leaveBalanceEntitleSL2.LeaveBalanceEntitleDays = lastSLCat2balanceItem.LeaveBalanceEntitled;
                    }

                    //  Initial Forfeiture
                    currentSLCat2balanceItem.LeaveBalanceForfeiture = 0;
                    int MaxBF = MaximumBroughtForwardForSLCat2(currentSLCat2balanceItem.LeaveBalanceEffectiveDate);

                    if (MaxBF >= 0)
                    {
                        currentSLCat2balanceItem.LeaveBalanceForfeiture = currentSLCat2balanceItem.LeaveBalanceBF - MaxBF;
                        if (currentSLCat2balanceItem.LeaveBalanceForfeiture < 0)
                            currentSLCat2balanceItem.LeaveBalanceForfeiture = 0;
                    }
                }

                balanceItemListSLCat2.Insert(0, currentSLCat2balanceItem);
                submit(currentSLCat2balanceItem, leaveBalanceEntitleSL2);
                return SLCat1balanceItem;
            }
            else
            {
                return base.CreateNextLeaveBalanceItem(lastLeaveBalanceItem);
            }
        }

        protected virtual int MaximumBroughtForwardForSLCat2(DateTime AsOfDate)
        {
            EEmpPositionInfo empPos = AppUtils.GetLastPositionInfo(dbConn, AsOfDate, EmpID);
            if (empPos != null)
            {
                if (leaveTypeDisabledSLCat2 == false)   // if CAT2 is enabled
                {
                    ELeavePlan leavePlan = new ELeavePlan();
                    leavePlan.LeavePlanID = empPos.LeavePlanID;
                    if (ELeavePlan.db.select(dbConn, leavePlan))
                    {
                        DBFilter dbFilter = new DBFilter();
                        dbFilter.add(new Match("LeavePlanID", leavePlan.LeavePlanID));
                        dbFilter.add(new Match("LeaveTypeID", leaveTypeIDSLCat2));
                        ArrayList list = ELeavePlanBroughtForward.db.select(dbConn, dbFilter);
                        if (list.Count > 0)
                        {
                            ELeavePlanBroughtForward leavePlanBroughtForward = (ELeavePlanBroughtForward)list[0];
                            return leavePlanBroughtForward.LeavePlanBroughtForwardMax;
                        }
                    }
                }
            }
            return 0;
            //return 9999;
        }
    }
}
