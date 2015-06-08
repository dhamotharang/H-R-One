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
    public class LeaveSummaryProcess :GenericReportProcess
    {
        private ArrayList values;
        private DateTime AsOfDate;

        public LeaveSummaryProcess(DatabaseConnection dbConn, ArrayList EmpList, DateTime AsOfDate)
            : base(dbConn)
        {
            this.values = EmpList;
            this.AsOfDate = AsOfDate;
        }

        public override ReportDocument GenerateReport()
        {
            if (values.Count > 0)
            {
                DateTime startDate = new DateTime(AsOfDate.Year, 1, 1);

                //System.Data.DataSet ds = new System.Data.DataSet();

                //System.Data.DataTable sum = ds.Tables.Add("LeaveBalance");
                //sum.Columns.Add("LeaveBalanceID", typeof(int));
                //sum.Columns.Add("EmpID", typeof(int));
                //sum.Columns.Add("LeaveBalanceEffectiveDate", typeof(DateTime));
                //sum.Columns.Add("LeaveBalanceBF", typeof(double));
                //sum.Columns.Add("LeaveBalanceEntitled", typeof(double));
                //sum.Columns.Add("LeaveBalanceForfeiture", typeof(double));
                //sum.Columns.Add("Taken", typeof(double));
                //sum.Columns.Add("Balance", typeof(double));
                //sum.Columns.Add("Adjust", typeof(double));
                //sum.Columns.Add("Reserved", typeof(double));
                //sum.Columns.Add("Name", typeof(string));
                //sum.Columns.Add("Description", typeof(string));
                //sum.Columns.Add("StringFormat", typeof(string));

                DataSet.EmployeeDetail ds = new DataSet.EmployeeDetail();
                //string select;
                //string from;
                //DBFilter filter;

                foreach (int EmpID in values)
                {
                    //from = "from EmpPersonalInfo P LEFT JOIN EmpPositionInfo EmpPos ON P.EmpID=EmpPos.EmpID AND EmpPos.EmpPosEffTo IS NULL LEFT JOIN Position Pos ON EmpPos.PositionID=Pos.PositionID";
                    //select = "P.*,EmpPos.*,Pos.*";
                    //filter = new DBFilter();
                    //filter.add(new Match("P.EmpID", EmpID));

                    //filter.loadData(ds, "employeedetail", null, select, from, null);

                    EmployeeDetailProcess.ImportEmployeeDetailRow(dbConn, ds.employeedetail, EmpID, AsOfDate);


                    //from = "from LeaveApplication P LEFT JOIN LeaveCode C ON P.LeaveCodeID=C.LeaveCodeID LEFT JOIN LeaveType T ON T.LeaveTypeID=C.LeaveTypeID";
                    //select = "P.*, T.LeaveType, T.LeaveTypeDesc";
                    //filter = new DBFilter();
                    //filter.add(new Match("LeaveAppDateFrom", ">=", startDate));
                    //filter.add(new Match("LeaveAppDateFrom", "<=", AsOfDate));
                    //filter.add(new Match("P.EmpID", EmpID));
                    //filter.loadData(ds, "LeaveApplication", null, select, from, null);
                    //try
                    //{
                    //    LeaveBalanceCalc calc = new LeaveBalanceCalc(EmpID, AsOfDate);
                    //    ArrayList balanceItems = calc.getCurrentBalanceList();
                    //    foreach (ELeaveBalance b in balanceItems)
                    //    {
                    //        System.Data.DataRow rr = sum.NewRow();
                    //        rr["LeaveBalanceID"] = b.LeaveBalanceID;
                    //        rr["EmpID"] = EmpID;
                    //        rr["LeaveBalanceEffectiveDate"] = b.LeaveBalanceEffectiveDate;
                    //        rr["LeaveBalanceBF"] = b.LeaveBalanceBF;
                    //        rr["LeaveBalanceEntitled"] = b.LeaveBalanceEntitled;
                    //        rr["LeaveBalanceForfeiture"] = b.LeaveBalanceForfeiture;
                    //        rr["Taken"] = b.Taken;
                    //        rr["Balance"] = b.Balance;
                    //        rr["Adjust"] = b.Adjust;
                    //        rr["Reserved"] = b.Reserved;
                    //        rr["Name"] = b.Name;
                    //        rr["Description"] = b.Description;
                    //        rr["StringFormat"] = b.StringFormat;


                    //        sum.Rows.Add(rr);


                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //}

                    DBFilter filter = new DBFilter();
                    filter.add(new Match("LeaveAppDateFrom", ">=", startDate));
                    filter.add(new Match("LeaveAppDateFrom", "<=", AsOfDate));
                    filter.add(new Match("EmpID", EmpID));
                    ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, filter);
                    DataSet.EmployeeDetail.LeaveApplicationDataTable leaveApplicationTable=ds.LeaveApplication;
                    foreach (ELeaveApplication leaveApplication in leaveApplicationList)
                    {
                        DataSet.EmployeeDetail.LeaveApplicationRow row = leaveApplicationTable.NewLeaveApplicationRow();
                        row.EmpID = leaveApplication.EmpID;
                        row.EmpPaymentID = leaveApplication.EmpPaymentID;
                        row.LeaveAppDateFrom = leaveApplication.LeaveAppDateFrom;
                        row.LeaveAppDateTo = leaveApplication.LeaveAppDateTo;
                        row.LeaveAppDays = leaveApplication.LeaveAppDays;
                        row.LeaveAppID = leaveApplication.LeaveAppID;
                        row.LeaveAppRemark = leaveApplication.LeaveAppRemark;
                        if (!leaveApplication.LeaveAppTimeFrom.Ticks.Equals(0))
                            row.LeaveAppTimeFrom = leaveApplication.LeaveAppTimeFrom;
                        if (!leaveApplication.LeaveAppTimeTo.Ticks.Equals(0))
                            row.LeaveAppTimeTo = leaveApplication.LeaveAppTimeTo;
                        row.LeaveAppUnit = leaveApplication.LeaveAppUnit;
                        row.LeaveCodeID = leaveApplication.LeaveCodeID;

                        ELeaveCode leaveCode = new ELeaveCode();
                        leaveCode.LeaveCodeID = leaveApplication.LeaveCodeID;
                        if (ELeaveCode.db.select(dbConn, leaveCode))
                        {
                            ELeaveType leaveType = new ELeaveType();
                            leaveType.LeaveTypeID = leaveCode.LeaveTypeID;
                            if (ELeaveType.db.select(dbConn, leaveType))
                            {
                                row.LeaveType = leaveType.LeaveType;
                                row.LeaveTypeDesc = leaveType.LeaveTypeDesc;
                            }
                        }
                        leaveApplicationTable.AddLeaveApplicationRow(row);
                    }

                    //try
                    //{
                    LeaveBalanceCalc calc = new LeaveBalanceCalc(dbConn, EmpID, AsOfDate);
                        ArrayList balanceItems = calc.getCurrentBalanceList();
                        foreach (ELeaveBalance b in balanceItems)
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
                    //}
                    //catch (Exception)
                    //{
                    //    //  fail to create leave balance record by some reason
                    //    //  no handling so far
                    //}

                }


                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Employee_LeaveSummary();
                }
                else
                {

                }




                reportDocument.SetDataSource(ds);
                reportDocument.SetParameterValue("AsOfDate", AsOfDate);

                return reportDocument;
            }
            else
                return null;
        }
    }
}
