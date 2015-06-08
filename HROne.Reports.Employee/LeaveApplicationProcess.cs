using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Employee
{
    public class LeaveApplicationProcess : GenericReportProcess
    {

        private ArrayList EmpList;
        private DateTime DateFrom;
        private DateTime DateTo;

        public LeaveApplicationProcess(DatabaseConnection dbConn, ArrayList EmpList, DateTime DateFrom, DateTime DateTo)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
        }

        public override ReportDocument GenerateReport()
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

            if (EmpList.Count > 0)
            {
                DataSet.EmployeeDetail ds = new DataSet.EmployeeDetail();


                foreach (int EmpID in EmpList)
                {

                    EmployeeDetailProcess.ImportEmployeeDetailRow(dbConn, ds.employeedetail, EmpID, AppUtils.ServerDateTime().Date);

                    DBFilter filter = new DBFilter();
                    //filter.add(new Match("LeaveAppDateFrom", ">=", startDate));
                    //filter.add(new Match("LeaveAppDateFrom", "<=", AsOfDate));
                    filter.add(new Match("EmpID", EmpID));
                    if (!DateFrom.Ticks.Equals(0))
                        filter.add(new Match("LeaveAppDateFrom", "<=", DateTo));
                    if (!DateTo.Ticks.Equals(0))
                        filter.add(new Match("LeaveAppDateTo", ">=", DateFrom));
                    ArrayList leaveApplicationList = ELeaveApplication.db.select(dbConn, filter);
                    DataSet.EmployeeDetail.LeaveApplicationDataTable leaveApplicationTable=ds.LeaveApplication;
                    foreach (ELeaveApplication leaveApplication in leaveApplicationList)
                    {
                        DataSet.EmployeeDetail.LeaveApplicationRow row = leaveApplicationTable.NewLeaveApplicationRow();
                        row.EmpID = leaveApplication.EmpID;
                        row.EmpPaymentID = leaveApplication.EmpPaymentID;
                        row.LeaveAppDateFrom = leaveApplication.LeaveAppDateFrom;
                        row.LeaveAppDateTo = leaveApplication.LeaveAppDateTo;
                        if (leaveApplication.LeaveAppUnit == "H")
                            row.LeaveAppDays = leaveApplication.LeaveAppHours;
                        else
                            row.LeaveAppDays = leaveApplication.LeaveAppDays;
                        row.LeaveAppID = leaveApplication.LeaveAppID;
                        row.LeaveAppRemark = leaveApplication.LeaveAppRemark;
                        if (!leaveApplication.LeaveAppTimeFrom.Ticks.Equals(0))
                            row.LeaveAppTimeFrom = leaveApplication.LeaveAppTimeFrom;
                        if (!leaveApplication.LeaveAppTimeTo.Ticks.Equals(0))
                            row.LeaveAppTimeTo = leaveApplication.LeaveAppTimeTo;
                        row.LeaveAppUnit = leaveApplication.LeaveAppUnit;
                        row.LeaveCodeID = leaveApplication.LeaveCodeID;
                        row.LeaveAppHasMedicalCertificate = leaveApplication.LeaveAppHasMedicalCertificate;
                        row.LeaveAppNoPayProcess = leaveApplication.LeaveAppNoPayProcess;
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
                }
                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Employee_LeaveApplicationList();
                }
                else
                {

                }

                reportDocument.SetDataSource(ds);
                for (int i = 0; i < reportDocument.ParameterFields.Count; i++)
                {
                    if (reportDocument.ParameterFields[i].Name == "HierarchyLevel1")
                        reportDocument.SetParameterValue("HierarchyLevel1", HierarchyLevel1);

                    if (reportDocument.ParameterFields[i].Name == "HierarchyLevel2")
                        reportDocument.SetParameterValue("HierarchyLevel2", HierarchyLevel2);
                }
                return reportDocument;
            }
            else
                return null;

        }
    }

}
