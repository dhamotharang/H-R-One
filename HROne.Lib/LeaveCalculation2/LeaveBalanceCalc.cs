//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Text;
//using HROne.Lib.Entities;
//using HROne.DataAccess;
//using HROne.CommonLib;
//namespace HROne.LeaveCalc
//{
//    public class LeaveBalanceCalc
//    {
//        int EmpID;

//        ArrayList leaveBalanceProcessList = new ArrayList();

//        public LeaveBalanceCalc(int EmpID)
//        {
//            this.EmpID = EmpID;
//            //position = AppUtils.GetLastPositionInfo(dbConn, asOfDate, EmpID);
//            //if (position == null)
//            //    throw new Exception("No position defined");
//            //leavePlan = new ELeavePlan(position.LeavePlanID);
//            leaveBalanceProcessList.Add(new AnnualLeaveBalanceProcess(EmpID));
//            leaveBalanceProcessList.Add(new SickLeaveBalanceProcess(EmpID));
//            leaveBalanceProcessList.Add(new CompensationLeaveBalanceProcess(EmpID));
//            ArrayList leaveTypeArray = ELeaveType.db.select(dbConn, new DBFilter());
//            foreach (ELeaveType leaveType in leaveTypeArray)
//                if (leaveType.LeaveType != ELeaveType.LEAVETYPECODE_ANNUAL && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_SLCAT1 && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_SLCAT2 && leaveType.LeaveType != ELeaveType.LEAVETYPECODE_COMPENSATION)
//                    leaveBalanceProcessList.Add(new LeaveBalanceProcess(EmpID, leaveType.LeaveTypeID));
//        }

//        public LeaveBalanceCalc(int EmpID, DateTime AsOfDate)
//            : this(EmpID)
//        {
//            loadData(AsOfDate);
//        }


//        public ArrayList getCurrentBalanceList()
//        {
//            ArrayList balanceList = new ArrayList();


//            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList)
//            {
//                ELeaveBalance leaveBalance = balanceProcess.getLatestLeaveBalance();
//                if (leaveBalance != null)
//                {
//                    LoadLeaveTypeAndRounding(leaveBalance);
//                    if (leaveBalance.LeaveBalanceEntitled != 0 || leaveBalance.Balance != 0 || leaveBalance.Taken != 0 || leaveBalance.Reserved != 0)
//                        balanceList.Add(leaveBalance);
//                }

//                if (balanceProcess is SickLeaveBalanceProcess)
//                {
//                    leaveBalance = ((SickLeaveBalanceProcess)balanceProcess).getLatestCAT2LeaveBalance();
//                    if (leaveBalance != null)
//                    {
//                        LoadLeaveTypeAndRounding(leaveBalance);
//                        if (leaveBalance.LeaveBalanceEntitled != 0 || leaveBalance.Balance != 0 || leaveBalance.Taken != 0 || leaveBalance.Reserved != 0)
//                            balanceList.Add(leaveBalance);
//                    }
//                }
//            }

//            return balanceList;
//        }

//        public void loadData(DateTime asOfDate)
//        {
//            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList)
//            {
//                balanceProcess.LoadData(asOfDate);
//            }

//        }

//        public void Recalculate()
//        {
//            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList)
//            {
//                balanceProcess.Recalculate();
//            }
//        }

//        public void RecalculateAfter(DateTime DateAfter)
//        {
//            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList)
//            {
//                balanceProcess.RecalculateAfter(DateAfter);
//            }
//        }
//        public void RecalculateAfter(DateTime DateAfter, int LeaveTypeID)
//        {
//            foreach (LeaveBalanceProcess balanceProcess in leaveBalanceProcessList)
//            {
//                if (balanceProcess.LeaveTypeID.Equals(LeaveTypeID))
//                    balanceProcess.RecalculateAfter(DateAfter);
//            }
//        }

//        private void LoadLeaveTypeAndRounding(ELeaveBalance b)
//        {
//            if (b != null)
//            {
//                ELeaveType type = new ELeaveType();
//                type.LeaveTypeID = b.LeaveTypeID;
//                if (ELeaveType.db.select(dbConn, type))
//                {
//                    b.Name = type.LeaveType;
//                    b.Description = type.LeaveTypeDesc;
//                    b.StringFormat = "0." + string.Empty.PadRight(type.LeaveDecimalPlace, '0');
//                }
//            }
//        }
//        //public TimeLineItem AdjustBalance(ELeaveBalance b, ArrayList list, LeaveBalanceAdjuster adjuster, int LeaveTypeID)
//        //{
//        //}

//    }




//}
