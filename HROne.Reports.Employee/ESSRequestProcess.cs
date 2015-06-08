using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Employee
{
    public class ESSRequestProcess : GenericReportProcess
    {

        private ArrayList EmpList;

        public ESSRequestProcess(DatabaseConnection dbConn, ArrayList EmpList)
            : base(dbConn)
        {
            this.EmpList = EmpList;
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

                    DBFilter empRequestFilter = new DBFilter();
                    empRequestFilter.add(new Match("EmpID", EmpID));
                    //empRequestFilter.add(new Match("EmpRequestStatusCode", "<>", EEmpRequest.STATUS_APPROVED));
                    //empRequestFilter.add(new Match("EmpRequestStatusCode", "<>", EEmpRequest.STATUS_USRCANCEL));
                    //empRequestFilter.add(new Match("EmpRequestStatusCode", "<>", EEmpRequest.STATUS_FSTREJ));
                    //empRequestFilter.add(new Match("EmpRequestStatusCode", "<>", EEmpRequest.STATUS_SNDREJ));
                    empRequestFilter.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_CANCELLED));
                    empRequestFilter.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_REJECTED));
                    empRequestFilter.add(new Match("EmpRequestStatus", "<>", EEmpRequest.STATUS_APPROVED));

                    ArrayList empRequestList = EEmpRequest.db.select(dbConn, empRequestFilter);
                    DataSet.EmployeeDetail.EmpRequestDataTable empRequestTable = ds.EmpRequest;
                    foreach (EEmpRequest empRequest in empRequestList)
                    {
                        DataSet.EmployeeDetail.EmpRequestRow row = empRequestTable.NewEmpRequestRow();

                        row.EmpID = empRequest.EmpID;
                        row.EmpRequestID = empRequest.EmpRequestID;
                        row.EmpRequestCreateDate = empRequest.EmpRequestCreateDate;
                        row.EmpRequestModifyDate = empRequest.EmpRequestModifyDate;
                        row.EmpRequestRecordID = empRequest.EmpRequestRecordID;
                        //row.EmpRequestStatusCode = HROne.Common.WebUtility.GetLocalizedString(empRequest.EmpRequestStatusCode);
                        row.EmpRequestStatus = HROne.Common.WebUtility.GetLocalizedString(empRequest.EmpRequestStatus);
                        row.EmpRequestLastAuthorizationWorkFlowIndex = empRequest.EmpRequestLastAuthorizationWorkFlowIndex;
                        row.EmpRequestLastActionBy = empRequest.EmpRequestLastActionBy;
                        row.EmpRequestType = HROne.Common.WebUtility.GetLocalizedString(empRequest.EmpRequestType);

                        if (empRequest.EmpRequestType.Equals(EEmpRequest.TYPE_EELEAVEAPP))
                        {
                            ERequestLeaveApplication requestLeaveApplication = new ERequestLeaveApplication();
                            requestLeaveApplication.RequestLeaveAppID = empRequest.EmpRequestRecordID;
                            if (ERequestLeaveApplication.db.select(dbConn, requestLeaveApplication))
                            {
                                ELeaveCode leaveCode = new ELeaveCode();
                                leaveCode.LeaveCodeID = requestLeaveApplication.RequestLeaveCodeID;
                                if (ELeaveCode.db.select(dbConn, leaveCode))
                                    row.EmpRequestType = leaveCode.LeaveCodeDesc;

                                row.DateFrom = requestLeaveApplication.RequestLeaveAppDateFrom;
                                row.DateFromAM = requestLeaveApplication.RequestLeaveAppDateFromAM;
                                row.DateTo = requestLeaveApplication.RequestLeaveAppDateTo;
                                row.DateToAM = requestLeaveApplication.RequestLeaveAppDateToAM;
                                row.Unit = requestLeaveApplication.RequestLeaveDays;
                                row.Remark = requestLeaveApplication.RequestLeaveAppRemark;
                            }
                        }
                        empRequestTable.AddEmpRequestRow(row);
                    }
                }
                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Employee_ESSRequest();
                }
                else
                {

                }

                reportDocument.SetDataSource(ds);
                reportDocument.SetParameterValue("HierarchyLevel1", HierarchyLevel1);
                return reportDocument;
            }
            else
                return null;

        }
    }

}
