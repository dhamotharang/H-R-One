using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.LeaveCalc
{
    public class LeaveBalanceCalc
    {
        int EmpID;

        LeaveBalanceProcessCollection leaveBalanceProcessList = new LeaveBalanceProcessCollection();
        protected DatabaseConnection dbConn;

        public LeaveBalanceCalc(DatabaseConnection dbConn, int EmpID)
        {
            this.dbConn = dbConn;
            this.EmpID = EmpID;
            //position = AppUtils.GetLastPositionInfo(dbConn, asOfDate, EmpID);
            //if (position == null)
            //    throw new Exception("No position defined");
            //leavePlan = new ELeavePlan(position.LeavePlanID);
            leaveBalanceProcessList.Add(new AnnualLeaveBalanceProcess(dbConn, EmpID));
            leaveBalanceProcessList.Add(new SickLeaveBalanceProcess(dbConn, EmpID));
            leaveBalanceProcessList.Add(new CompensationLeaveBalanceProcess(dbConn, EmpID));
            leaveBalanceProcessList.Add(new StatutoryHolidayBalanceProcess(dbConn, EmpID));
            leaveBalanceProcessList.Add(new PublicHolidayBalanceProcess(dbConn, EmpID));
            leaveBalanceProcessList.Add(new RestDayBalanceProcess(dbConn, EmpID));
            leaveBalanceProcessList.Add(new BirthdayLeaveBalanceProcess(dbConn, EmpID));

            DBFilter dbfilter = new DBFilter();
            dbfilter.add("LeaveType", true);

            ArrayList leaveTypeArray = ELeaveType.db.select(dbConn, dbfilter);
            foreach (ELeaveType leaveType in leaveTypeArray)
            {

                if (leaveType.LeaveType != ELeaveType.LEAVETYPECODE_ANNUAL
                    && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_SLCAT1
                    && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_SLCAT2
                    && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_COMPENSATION
                    && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_STATUTORYHOLIDAY
                    && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_PUBLICHOLIDAY
                    && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_RESTDAY
                    && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_BIRTHDAY
                    )
                {
                    if (!leaveType.LeaveTypeIsDisabled)
                        leaveBalanceProcessList.Add(leaveType.LeaveTypeID, new LeaveBalanceProcess(dbConn, EmpID, leaveType.LeaveTypeID));
                }
                else if (leaveType.LeaveTypeIsDisabled && leaveBalanceProcessList.ContainsKey(leaveType.LeaveTypeID))
                    leaveBalanceProcessList.Remove(leaveType.LeaveTypeID);
            }

//leaveBalanceProcessList.Clear();
//leaveBalanceProcessList.Add(new CompensationLeaveBalanceProcess(dbConn, EmpID));


        }

        public LeaveBalanceCalc(DatabaseConnection dbConn, int EmpID, DateTime AsOfDate)
            : this(dbConn, EmpID)
        {
            loadData(AsOfDate);
        }

        public LeaveBalanceProcess getLeaveBalanceProcess(int LeaveTypeID)
        {
            if (leaveBalanceProcessList.ContainsKey(LeaveTypeID))
                return leaveBalanceProcessList[LeaveTypeID];
            else
                return null;
        }

        public ArrayList getCurrentBalanceList()
        {
            return getCurrentBalanceList(false);
        }
        public ArrayList getCurrentBalanceList(bool forESSOnly)
        {
            ArrayList balanceList = new ArrayList();


            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList.Values)
            {
                ELeaveBalance leaveBalance = balanceProcess.getLatestLeaveBalance();
                if (leaveBalance != null)
                {
                    //bool addToList = true;
                    if (forESSOnly)
                    {
                        ELeaveType leaveType = new ELeaveType();
                        leaveType.LeaveTypeID = leaveBalance.LeaveTypeID;
                        if (!ELeaveType.db.select(dbConn, leaveType))
                            continue;
                        if (leaveType.LeaveTypeIsESSHideLeaveBalance)
                            continue;
                        DBFilter countESSLeaveCodeFilter = new DBFilter();
                        countESSLeaveCodeFilter.add(new Match("LeaveTypeID", leaveBalance.LeaveTypeID));
                        countESSLeaveCodeFilter.add(new Match("LeaveCodeHideInESS", false));
                        if (ELeaveCode.db.count(dbConn, countESSLeaveCodeFilter) <= 0)
                            continue;
                            //addToList = false;
                    }

                    //if (addToList)
                    {
                        LoadLeaveTypeAndRounding(leaveBalance);
                        if (leaveBalance.LeaveBalanceEntitled != 0 || leaveBalance.getBalance() != 0 || leaveBalance.Taken != 0 || leaveBalance.Reserved != 0)
                            balanceList.Add(leaveBalance);
                    }
                }

                // Start 2013-12-02, Ricky So, Avoid showing CAT2 Sick Leave entry
                if ((ELeaveType.SLCAT2_LEAVE_TYPE(dbConn)).LeaveTypeIsDisabled == false)
                {
                    if (balanceProcess is SickLeaveBalanceProcess)
                    {
                        leaveBalance = ((SickLeaveBalanceProcess)balanceProcess).getLatestCAT2LeaveBalance();
                        if (leaveBalance != null)
                        {
                            bool addToList = true;
                            if (forESSOnly)
                            {
                                DBFilter countESSLeaveCodeFilter = new DBFilter();
                                countESSLeaveCodeFilter.add(new Match("LeaveTypeID", leaveBalance.LeaveTypeID));
                                countESSLeaveCodeFilter.add(new Match("LeaveCodeHideInESS", false));
                                if (ELeaveCode.db.count(dbConn, countESSLeaveCodeFilter) <= 0)
                                    addToList = false;
                            }

                            if (addToList)
                            {
                                LoadLeaveTypeAndRounding(leaveBalance);
                                if (leaveBalance.LeaveBalanceEntitled != 0 || leaveBalance.getBalance() != 0 || leaveBalance.Taken != 0 || leaveBalance.Reserved != 0)
                                    balanceList.Add(leaveBalance);
                            }
                        }
                    }
                }
                //if (balanceProcess is SickLeaveBalanceProcess)
                //{
                //    leaveBalance = ((SickLeaveBalanceProcess)balanceProcess).getLatestCAT2LeaveBalance();
                //    if (leaveBalance != null)
                //    {
                //        bool addToList = true;
                //        if (forESSOnly)
                //        {
                //            DBFilter countESSLeaveCodeFilter = new DBFilter();
                //            countESSLeaveCodeFilter.add(new Match("LeaveTypeID", leaveBalance.LeaveTypeID));
                //            countESSLeaveCodeFilter.add(new Match("LeaveCodeHideInESS", false));
                //            if (ELeaveCode.db.count(dbConn, countESSLeaveCodeFilter) <= 0)
                //                addToList = false;
                //        }

                //        if (addToList)
                //        {
                //            LoadLeaveTypeAndRounding(leaveBalance);
                //            if (leaveBalance.LeaveBalanceEntitled != 0 || leaveBalance.getBalance() != 0 || leaveBalance.Taken != 0 || leaveBalance.Reserved != 0)
                //                balanceList.Add(leaveBalance);
                //        }
                //    }
                //}

                
                // End 2013-12-02, Ricky So, Avoid showing CAT2 Sick Leave entry
            }

            return balanceList;
        }

        public ELeaveBalance getCurrentBalanceObject(int LeaveTypeID)
        {
            ArrayList leaveBalanceList = getCurrentBalanceList(false);
            foreach (ELeaveBalance bal in leaveBalanceList)
            {
                if (bal.LeaveTypeID == LeaveTypeID)
                    return bal;
            }
            return null;
        }

        public void loadData(DateTime asOfDate)
        {
            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList.Values)
            {
                balanceProcess.LoadData(asOfDate);
            }

        }

        public void Recalculate()
        {
            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList.Values)
            {
                balanceProcess.Recalculate();
            }
        }

        //public void RecalculateAfter(DateTime DateAfter)
        //{
        //    foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList)
        //    {
        //        balanceProcess.RecalculateAfter(DateAfter);
        //    }
        //}
        //public void RecalculateAfter(DateTime DateAfter, int LeaveTypeID)
        //{
        //    foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList)
        //    {
        //        if (balanceProcess.LeaveTypeID.Equals(LeaveTypeID))
        //            balanceProcess.RecalculateAfter(DateAfter);
        //    }
        //}

        private void LoadLeaveTypeAndRounding(ELeaveBalance b)
        {
            if (b != null)
            {
                ELeaveType type = new ELeaveType();
                type.LeaveTypeID = b.LeaveTypeID;
                if (ELeaveType.db.select(dbConn, type))
                {
                    b.Name = type.LeaveType;
                    b.Description = type.LeaveTypeDesc;
                    b.StringFormat = "0." + string.Empty.PadRight(type.LeaveDecimalPlace, '0');
                }
            }
        }

        //public TimeLineItem AdjustBalance(ELeaveBalance b, ArrayList list, LeaveBalanceAdjuster adjuster, int LeaveTypeID)
        //{
        //}

    }
    public class LeaveBalanceProcessCollection : Dictionary<int, LeaveBalanceProcess>
    {
        public void Add(LeaveBalanceProcess process)
        {
            if (process != null)
                if (process.LeaveTypeID > 0)
                    this.Add(process.LeaveTypeID, process);
        }
    }




}
