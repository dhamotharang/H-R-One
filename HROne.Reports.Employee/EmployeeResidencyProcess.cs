using System;
using System.Collections;
using System.Text;
using HROne.Lib.Entities;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Employee
{
    public class EmployeeResidencyProcess : GenericReportProcess
    {
        private DataSet.HeadCountSummarySet dataSet;
        private DateTime CurrentDate = new DateTime();
        private DateTime ReferenceDate = new DateTime();
        private ReportGroupBy GroupBy = ReportGroupBy.Unknown;
        private int HierarchyLevelID = 0;
        private string HierarchyLevelName = string.Empty;
        // Start 0000185, KuangWei, 2015-05-05
        private ArrayList empList = new ArrayList();
        // End 0000185, KuangWei, 2015-05-05

        public enum ReportGroupBy
        {
            Unknown = 0,
            Company,
            Hierarchy,
            PayrollGroup
        }

        // Start 0000185, KuangWei, 2015-05-05
        public EmployeeResidencyProcess(DatabaseConnection dbConn, DateTime AsOfDate, ReportGroupBy GroupBy, int HierarchyLevelID, ArrayList empList)
            : base(dbConn)
        {
            this.CurrentDate = AsOfDate;
            this.ReferenceDate = AsOfDate;
            this.GroupBy = GroupBy;
            this.HierarchyLevelID = HierarchyLevelID;
            EHierarchyLevel HLevel = new EHierarchyLevel();
            HLevel.HLevelID = HierarchyLevelID;
            if (EHierarchyLevel.db.select(dbConn, HLevel))
                this.HierarchyLevelName = HLevel.HLevelDesc;
            this.empList = empList;
        }
        // End 0000185, KuangWei, 2015-05-05

        // Start 0000185, KuangWei, 2015-05-05
        public override ReportDocument GenerateReport()
        {
            if (CurrentDate.Ticks != 0 && ReferenceDate.Ticks != 0)
            {
                dataSet = new DataSet.HeadCountSummarySet();

                dataSet.Gender.Rows.Add(new string[] { "M", "Male" });
                dataSet.Gender.Rows.Add(new string[] { "F", "Female" });

                dataSet.AreaCode.Rows.Add(new string[] { "H", "Hong Kong" });
                dataSet.AreaCode.Rows.Add(new string[] { "K", "Kowloon" });
                dataSet.AreaCode.Rows.Add(new string[] { "N", "New Territories" });
                dataSet.AreaCode.Rows.Add(new string[] { "O", "Others" });
                // Get Total PreviousCount

                DBFilter EmpReferencePosFilter = new DBFilter();
                EmpReferencePosFilter.add(new Match("EmpPosEffFr", "<=", ReferenceDate));
                OR orReferencePosEffToDate = new OR();
                orReferencePosEffToDate.add(new Match("EmpPosEffTo", ">=", ReferenceDate));
                orReferencePosEffToDate.add(new NullTerm("EmpPosEffTo"));

                EmpReferencePosFilter.add(orReferencePosEffToDate);

                DBFilter empReferenceTerminationFilter = new DBFilter();
                empReferenceTerminationFilter.add(new Match("EmpTermLastDate", "<", ReferenceDate));
                EmpReferencePosFilter.add(new IN("not empid", "Select empid from EmpTermination", empReferenceTerminationFilter));

                ArrayList empReferenceDatePosList = EEmpPositionInfo.db.select(dbConn, EmpReferencePosFilter);

                foreach (EEmpPositionInfo empPos in empReferenceDatePosList)
                {
                    foreach (EEmpPersonalInfo obj in empList)
                    {
                        if (obj.EmpID == empPos.EmpID)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = empPos.EmpID;
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            {
                                DataSet.HeadCountSummarySet.HeadCountSummaryRow row = CreateHeadCountRow(empPos.EmpPosID);
                                row.PreviousCount = 1;
                                dataSet.HeadCountSummary.Rows.Add(row);
                            }
                        }
                    }
                }
                // Get Total CurrentCount

                DBFilter EmpCurrentPosFilter = new DBFilter();
                EmpCurrentPosFilter.add(new Match("EmpPosEffFr", "<=", CurrentDate));
                OR orCurrentPosEffToDate = new OR();
                orCurrentPosEffToDate.add(new Match("EmpPosEffTo", ">=", CurrentDate));
                orCurrentPosEffToDate.add(new NullTerm("EmpPosEffTo"));

                EmpCurrentPosFilter.add(orCurrentPosEffToDate);

                DBFilter empCurrentTerminationFilter = new DBFilter();
                empCurrentTerminationFilter.add(new Match("EmpTermLastDate", "<", CurrentDate));
                EmpCurrentPosFilter.add(new IN("not empid", "Select empid from EmpTermination", empCurrentTerminationFilter));

                ArrayList empCurrentDatePosList = EEmpPositionInfo.db.select(dbConn, EmpCurrentPosFilter);

                foreach (EEmpPositionInfo empPos in empCurrentDatePosList)
                {
                    foreach (EEmpPersonalInfo obj in empList)
                    {
                        if (obj.EmpID == empPos.EmpID)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = empPos.EmpID;
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            {
                                DataSet.HeadCountSummarySet.HeadCountSummaryRow row = CreateHeadCountRow(empPos.EmpPosID);
                                row.CurrentCount = 1;
                                dataSet.HeadCountSummary.Rows.Add(row);
                            }
                        }
                    }
                }

                // Get New Join Count and transfer count

                DBFilter EmpNewPosFilter = new DBFilter();
                EmpNewPosFilter.add(new Match("EmpPosEffFr", ">", ReferenceDate));
                EmpNewPosFilter.add(new Match("EmpPosEffFr", "<=", CurrentDate));


                ArrayList empNewPosList = EEmpPositionInfo.db.select(dbConn, EmpNewPosFilter);

                foreach (EEmpPositionInfo empPos in empNewPosList)
                {
                    foreach (EEmpPersonalInfo obj in empList)
                    {
                        if (obj.EmpID == empPos.EmpID)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = empPos.EmpID;
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            {
                                DataSet.HeadCountSummarySet.HeadCountSummaryRow row = CreateHeadCountRow(empPos.EmpPosID);

                                // check2 if have previous position change
                                {
                                    DBFilter previousEmpPosFilter = new DBFilter();
                                    previousEmpPosFilter.add(new Match("EmpID", empPos.EmpID));
                                    previousEmpPosFilter.add(new Match("EmpPosEffTo", "<", empPos.EmpPosEffFr));
                                    previousEmpPosFilter.add("EmpPosEffFr", false);

                                    ArrayList previousEmpPosList = EEmpPositionInfo.db.select(dbConn, previousEmpPosFilter);
                                    if (previousEmpPosList.Count > 0)
                                    {
                                        EEmpPositionInfo empPreviousPos = (EEmpPositionInfo)previousEmpPosList[0];
                                        EHierarchyElement hPreviousElement = getHierarchyElement(empPreviousPos.EmpPosID);
                                        if (empPos.CompanyID != empPreviousPos.CompanyID || row.HElementID != hPreviousElement.HElementID)
                                        {
                                            row.TransferInCount = 1;
                                            if (empPreviousPos.EmpPosEffFr <= (CurrentDate < ReferenceDate ? CurrentDate : ReferenceDate))
                                            {
                                                DataSet.HeadCountSummarySet.HeadCountSummaryRow previousrow = CreateHeadCountRow(empPreviousPos.EmpPosID);
                                                previousrow.TransferOutCount = 1;
                                                dataSet.HeadCountSummary.Rows.Add(previousrow);

                                            }
                                        }
                                    }
                                    else
                                        row.NewJoinCount = 1;
                                }

                                // check if have next position change
                                if (empPos.EmpPosEffFr < empPos.EmpPosEffTo)
                                {
                                    DBFilter nextEmpPosFilter = new DBFilter();
                                    nextEmpPosFilter.add(new Match("EmpID", empPos.EmpID));
                                    nextEmpPosFilter.add(new Match("EmpPosEffFr", ">", empPos.EmpPosEffTo));
                                    nextEmpPosFilter.add(new Match("EmpPosEffFr", "<=", CurrentDate));
                                    nextEmpPosFilter.add("EmpPosEffFr", true);

                                    ArrayList nextEmpPosList = EEmpPositionInfo.db.select(dbConn, nextEmpPosFilter);
                                    if (nextEmpPosList.Count > 0)
                                    {
                                        EEmpPositionInfo empNextPos = (EEmpPositionInfo)nextEmpPosList[0];
                                        EHierarchyElement hNextElement = getHierarchyElement(empNextPos.EmpPosID);
                                        if (empPos.CompanyID != empNextPos.CompanyID || row.HElementID != hNextElement.HElementID)
                                        {
                                            row.TransferOutCount = 1;
                                            if (empNextPos.EmpPosEffTo.Ticks.Equals(0) || empNextPos.EmpPosEffTo > (CurrentDate > ReferenceDate ? CurrentDate : ReferenceDate))
                                            {
                                                DataSet.HeadCountSummarySet.HeadCountSummaryRow nextRow = CreateHeadCountRow(empNextPos.EmpPosID);
                                                nextRow.TransferInCount = 1;
                                                dataSet.HeadCountSummary.Rows.Add(nextRow);

                                            }
                                        }
                                    }
                                }
                                dataSet.HeadCountSummary.Rows.Add(row);
                            }
                        }
                    }
                }

                // Get Terminated Count

                DBFilter empTerminationFilter = new DBFilter();
                empTerminationFilter.add(new Match("EmpTermLastDate", ">=", ReferenceDate));
                empTerminationFilter.add(new Match("EmpTermLastDate", "<", CurrentDate));
                ArrayList empTerminationList = EEmpTermination.db.select(dbConn, empTerminationFilter);
                if (empTerminationList.Count > 0)
                    foreach (EEmpTermination empTermination in empTerminationList)
                    {
                        foreach (EEmpPersonalInfo obj in empList)
                        {
                            if (obj.EmpID == empTermination.EmpID)
                            {
                                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                                empInfo.EmpID = empTermination.EmpID;
                                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                {

                                    DBFilter empTerminationPosFilter = new DBFilter();
                                    empTerminationPosFilter.add(new Match("EmpID", empTermination.EmpID));
                                    empTerminationPosFilter.add(new Match("EmpPosEffFr", "<=", empTermination.EmpTermLastDate));
                                    OR orEmpTerminationPosEffToDate = new OR();
                                    orEmpTerminationPosEffToDate.add(new Match("EmpPosEffTo", ">=", empTermination.EmpTermLastDate));
                                    orEmpTerminationPosEffToDate.add(new NullTerm("EmpPosEffTo"));
                                    empTerminationPosFilter.add(orEmpTerminationPosEffToDate);
                                    empTerminationPosFilter.add("EmpPosEffFr", false);
                                    ArrayList empTerminationPosList = EEmpPositionInfo.db.select(dbConn, empTerminationPosFilter);
                                    if (empTerminationPosList.Count > 0)
                                    {
                                        EEmpPositionInfo empPos = (EEmpPositionInfo)empTerminationPosList[0];
                                        DataSet.HeadCountSummarySet.HeadCountSummaryRow row = CreateHeadCountRow(empPos.EmpPosID);
                                        row.TerminateCount = 1;
                                        dataSet.HeadCountSummary.Rows.Add(row);

                                    }
                                }
                            }
                        }
                    }
                if (reportDocument == null)
                {
                    if (GroupBy.Equals(ReportGroupBy.Company))
                        reportDocument = new ReportTemplate.Report_Employee_Residency_Company();
                    else if (GroupBy.Equals(ReportGroupBy.Hierarchy))
                        reportDocument = new ReportTemplate.Report_Employee_Residency_Hierarchy();
                    else if (GroupBy.Equals(ReportGroupBy.PayrollGroup))
                        reportDocument = new ReportTemplate.Report_Employee_Residency_PayrollGroup();
                }
                else
                {

                }
                reportDocument.SetDataSource(dataSet);
                if (GroupBy.Equals(ReportGroupBy.Hierarchy))
                    reportDocument.SetParameterValue("HierarchyLevelName", HierarchyLevelName);
                reportDocument.SetParameterValue("AsOfDateString", CurrentDate.ToString("yyyy-MM-dd"));

                //reportDocument.SetParameterValue("CurrentDate", CurrentDate);
                //reportDocument.SetParameterValue("ReferenceDate", ReferenceDate);
                ////if (Gender.Equals("M"))
                ////    reportDocument.SetParameterValue("Gender", "Male");
                ////else if (Gender.Equals("F"))
                ////    reportDocument.SetParameterValue("Gender", "Female");
                ////else
                //    reportDocument.SetParameterValue("Gender", "All");

                //            rpt.SetDataSource((DataTable)mpfTable);
                return reportDocument;
            }
            else
                return null;




        }
        // End 0000185, KuangWei, 2015-05-05

        private DataSet.HeadCountSummarySet.HeadCountSummaryRow CreateHeadCountRow(int EmpPosID)
        {
            DataSet.HeadCountSummarySet.HeadCountSummaryRow row = dataSet.HeadCountSummary.NewHeadCountSummaryRow();
            EEmpPositionInfo empPos = new EEmpPositionInfo();
            empPos.EmpPosID = EmpPosID;
            if (EEmpPositionInfo.db.select(dbConn, empPos))
            {
                row.EmpID = empPos.EmpID;
                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empPos.EmpID;
                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                {
                    row.EmpGender = empInfo.EmpGender;
                    row.EmpAreaCode = empInfo.EmpResAddrAreaCode;

                    ECompany company = new ECompany();
                    company.CompanyID = empPos.CompanyID;
                    if (ECompany.db.select(dbConn, company))
                    {
                        row.CompanyID = company.CompanyID;
                        row.CompanyCode = company.CompanyCode;
                        row.CompanyName = company.CompanyName;
                    }

                    EPayrollGroup payGroup = new EPayrollGroup();
                    payGroup.PayGroupID = empPos.PayGroupID;
                    if (EPayrollGroup.db.select(dbConn, payGroup))
                    {
                        row.PayGroupID = payGroup.PayGroupID;
                        row.PayGroupCode = payGroup.PayGroupCode;
                        row.PayGroupDesc = payGroup.PayGroupDesc;
                    }

                    EHierarchyElement hElement = getHierarchyElement(EmpPosID);
                    if (hElement != null)
                    {
                        row.HElementID = hElement.HElementID;
                        row.HElementCode = hElement.HElementCode;
                        row.HElementDesc = hElement.HElementDesc;
                    }
                    else
                    {
                        row.HElementID = 0;
                        row.HElementCode = " ";
                        row.HElementDesc = " ";
                    }
                }
            }

            row.PreviousCount = 0;
            row.CurrentCount = 0;
            row.NewJoinCount = 0;
            row.TerminateCount = 0;
            row.TransferInCount = 0;
            row.TransferOutCount = 0;
            return row;
        }

        private EHierarchyElement getHierarchyElement(int empPosID)
        {
            DBFilter EmpHLevelFilter = new DBFilter();
            EmpHLevelFilter.add(new Match("EmpPosID", empPosID));

            DBFilter HierarchyFilter = new DBFilter();
            HierarchyFilter.add(new IN("HElementID", "Select HElementID from EmpHierarchy", EmpHLevelFilter));
            HierarchyFilter.add(new Match("HLevelID", HierarchyLevelID));

            ArrayList hierarchyList = EHierarchyElement.db.select(dbConn, HierarchyFilter);
            if (hierarchyList.Count > 0)
            {
                return (EHierarchyElement)hierarchyList[0];
            }
            else
                return new EHierarchyElement();
        }
    }
}
