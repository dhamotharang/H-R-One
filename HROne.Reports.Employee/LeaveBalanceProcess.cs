using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.Lib.Entities;
using HROne.LeaveCalc;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Employee
{
    public class LeaveBalanceProcess : GenericReportProcess
    {
        private ArrayList values;
        private DateTime AsOfDate;
        private ArrayList LeaveTypeList;

        public LeaveBalanceProcess(DatabaseConnection dbConn, ArrayList EmpList, DateTime AsOfDate, ArrayList LeaveTypeList)
            : base(dbConn)
        {
            this.values = EmpList;
            this.AsOfDate = AsOfDate;
            this.LeaveTypeList = LeaveTypeList;
        }

        public override ReportDocument GenerateReport()
        {
            if (values.Count > 0)
            {
                string HierarchyLevel1 = string.Empty;
                string HierarchyLevel2 = string.Empty;
                string HierarchyLevel3 = string.Empty;

                ArrayList HierarchyLevelList = EHierarchyLevel.db.select(dbConn, new DBFilter());
                foreach (EHierarchyLevel hLevel in HierarchyLevelList)
                {

                    if (hLevel.HLevelSeqNo.Equals(1))
                        HierarchyLevel1 = hLevel.HLevelDesc;
                    else if (hLevel.HLevelSeqNo.Equals(2))
                        HierarchyLevel2 = hLevel.HLevelDesc;
                    else if (hLevel.HLevelSeqNo.Equals(3))
                        HierarchyLevel3 = hLevel.HLevelDesc;
                }

                DataSet.EmployeeDetail ds = new DataSet.EmployeeDetail();

                foreach (int EmpID in values)
                {

                    EmployeeDetailProcess.ImportEmployeeDetailRow(dbConn, ds.employeedetail, EmpID, AsOfDate);


                    //DBFilter filter = new DBFilter();
                    //filter.add(new Match("LeaveAppDateFrom", ">=", startDate));
                    //filter.add(new Match("LeaveAppDateFrom", "<=", AsOfDate));
                    //filter.add(new Match("EmpID", EmpID));
                    //ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, filter);
                    //DataSet.EmployeeDetail.LeaveApplicationDataTable leaveApplicationTable=ds.LeaveApplication;
                    //foreach (ELeaveApplication leaveApplication in leaveApplicationList)
                    //{
                    //    DataSet.EmployeeDetail.LeaveApplicationRow row = leaveApplicationTable.NewLeaveApplicationRow();
                    //    row.EmpID = leaveApplication.EmpID;
                    //    row.EmpPaymentID = leaveApplication.EmpPaymentID;
                    //    row.LeaveAppDateFrom = leaveApplication.LeaveAppDateFrom;
                    //    row.LeaveAppDateTo = leaveApplication.LeaveAppDateTo;
                    //    row.LeaveAppDays = leaveApplication.LeaveAppDays;
                    //    row.LeaveAppID = leaveApplication.LeaveAppID;
                    //    row.LeaveAppRemark = leaveApplication.LeaveAppRemark;
                    //    if (!leaveApplication.LeaveAppTimeFrom.Ticks.Equals(0))
                    //        row.LeaveAppTimeFrom = leaveApplication.LeaveAppTimeFrom;
                    //    if (!leaveApplication.LeaveAppTimeTo.Ticks.Equals(0))
                    //        row.LeaveAppTimeTo = leaveApplication.LeaveAppTimeTo;
                    //    row.LeaveAppUnit = leaveApplication.LeaveAppUnit;
                    //    row.LeaveCodeID = leaveApplication.LeaveCodeID;

                    //    ELeaveCode leaveCode = new ELeaveCode();
                    //    leaveCode.LeaveCodeID = leaveApplication.LeaveCodeID;
                    //    if (ELeaveCode.db.select(dbConn, leaveCode))
                    //    {
                    //        ELeaveType leaveType = new ELeaveType();
                    //        leaveType.LeaveTypeID = leaveCode.LeaveTypeID;
                    //        if (ELeaveType.db.select(dbConn, leaveType))
                    //        {
                    //            row.LeaveType = leaveType.LeaveType;
                    //            row.LeaveTypeDesc = leaveType.LeaveTypeDesc;
                    //        }
                    //    }
                    //    leaveApplicationTable.AddLeaveApplicationRow(row);
                    //}

                    //try
                    //{
                    LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, EmpID, AsOfDate);
                        ArrayList balanceItems = calc.getCurrentBalanceList();
                        foreach (ELeaveBalance b in balanceItems)
                        {
                            foreach (ELeaveType leaveType in LeaveTypeList)
                                if (leaveType.LeaveTypeID.Equals(b.LeaveTypeID))
                                {
                                    DataSet.EmployeeDetail.LeaveBalanceRow rr = ds.LeaveBalance.NewLeaveBalanceRow();
                                    rr["LeaveBalanceID"] = b.LeaveBalanceID;
                                    rr["EmpID"] = EmpID;
                                    rr["LeaveBalanceEffectiveDate"] = b.LeaveBalanceEffectiveDate;
                                    rr["LeaveBalanceBF"] = b.LeaveBalanceBF;
                                    rr["LeaveBalanceEntitled"] = b.LeaveBalanceEntitled;
                                    rr["LeaveBalanceForfeiture"] = b.LeaveBalanceForfeiture + b.ExpiryForfeit;
                                    rr["Taken"] = b.Taken;
                                    rr["Balance"] = b.getBalance();
                                    rr["Adjust"] = b.Adjust;
                                    rr["Reserved"] = b.Reserved;
                                    rr["Name"] = b.Name;
                                    rr["Description"] = b.Description;
                                    rr["ExpiryForfeit"] = b.ExpiryForfeit;
                                    if (!b.NextExpiryDate.Ticks.Equals(0))
                                        rr["NextExpiryDate"] = b.NextExpiryDate;
                                    rr["NextExpiryForfeit"] = b.NextExpiryForfeit;
                                    rr["StringFormat"] = b.StringFormat;
                                    ds.LeaveBalance.Rows.Add(rr);
                                }



                        }
                    //}
                    //catch (Exception)
                    //{
                    //    //  fail to create leave balance record by some reason
                    //    //  no handling so far
                    //}

                }

                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Employee_LeaveBalance();
                }
                else
                {

                }

                reportDocument.SetDataSource(ds);
                reportDocument.SetParameterValue("AsOfDate", AsOfDate);
                reportDocument.SetParameterValue("HierarchyLevel1", HierarchyLevel1);
                reportDocument.SetParameterValue("HierarchyLevel2", HierarchyLevel2);
                return reportDocument;
            }
            else
                return null;
        }
    }
}
