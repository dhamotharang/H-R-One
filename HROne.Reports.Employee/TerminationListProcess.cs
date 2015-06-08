using System;
using System.Collections;
using System.Text;
using HROne.Common;
using HROne.Lib.Entities;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Employee 
{
    public class TerminationListProcess:GenericReportProcess 
    {
        private ArrayList EmpTermList;

        public TerminationListProcess(DatabaseConnection dbConn, ArrayList EmpTermList)
            : base(dbConn)
        {
            this.EmpTermList = EmpTermList;
        }

        public override ReportDocument GenerateReport()
        {
            DataSet.EmpTerminationListSet dataSet = new DataSet.EmpTerminationListSet();

            string strPrintPeriod = string.Empty;

            DataSet.EmpTerminationListSet.TerminationListDataTable dataTable = dataSet.TerminationList;

            DBFilter hierarchyLevelFilter = new DBFilter();
            hierarchyLevelFilter.add("HLevelSeqNo", true);
            ArrayList hierarchyLevelList = EHierarchyLevel.db.select(dbConn, hierarchyLevelFilter);

            foreach (EEmpTermination empTermination in EmpTermList)
            {

                EEmpTermination.db.select(dbConn, empTermination);

                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                empInfo.EmpID = empTermination.EmpID;
                EEmpPersonalInfo.db.select(dbConn, empInfo);




                DBFilter positionFilter = new DBFilter();
                positionFilter.add(new Match("EmpID", empInfo.EmpID));
                //if (lngPayPeriodFr != 0 && lngPayPeriodTo != 0)
                //{
                positionFilter.add(new Match("EmpPosEffFr", "<=", empTermination.EmpTermLastDate));
                OR orPosEffToTerms = new OR();
                orPosEffToTerms.add(new Match("EmpPosEffTo", ">=", empTermination.EmpTermLastDate));
                orPosEffToTerms.add(new NullTerm("EmpPosEffTo"));
                positionFilter.add(orPosEffToTerms);
                //}
                positionFilter.add("EmpPosEffFr", false);

                ArrayList empPositionInfos = EEmpPositionInfo.db.select(dbConn, positionFilter);
                EEmpPositionInfo empPositionInfo;
                if (empPositionInfos.Count > 0)
                    empPositionInfo = (EEmpPositionInfo)empPositionInfos[0];
                else
                    empPositionInfo = new EEmpPositionInfo();

                ECompany company = new ECompany();
                company.CompanyID = empPositionInfo.CompanyID;
                ECompany.db.select(dbConn, company);

                EPosition position = new EPosition();
                position.PositionID = empPositionInfo.PositionID;
                EPosition.db.select(dbConn, position);

                ECessationReason cessationReason = new ECessationReason();
                cessationReason.CessationReasonID = empTermination.CessationReasonID;
                ECessationReason.db.select(dbConn, cessationReason);

                string businessHierarchy = string.Empty;
                foreach (EHierarchyLevel hLevel in hierarchyLevelList)
                {
                    DBFilter empHierarchyFilter = new DBFilter();
                    empHierarchyFilter.add(new Match("EmpPosID", empPositionInfo.EmpPosID));
                    empHierarchyFilter.add(new Match("HLevelID", hLevel.HLevelID));
                    ArrayList empHierarchyList = EEmpHierarchy.db.select(dbConn, empHierarchyFilter);
                    foreach (EEmpHierarchy empHierarchy in empHierarchyList)
                    {
                        EHierarchyElement obj = new EHierarchyElement();
                        obj.HElementID = empHierarchy.HElementID;
                        if (EHierarchyElement.db.select(dbConn, obj))
                            if (string.IsNullOrEmpty(businessHierarchy))
                                businessHierarchy = obj.HElementDesc;
                            else
                                businessHierarchy += "\t" + obj.HElementDesc;
                    }
                }
                //DBFilter HierarchyElementFilter = new DBFilter();
                //System.Data.DataTable hierarchyElementTable = HierarchyElementFilter.loadData("Select HE.*, HL.HLevelSeqNo from HierarchyElement he left join HierarchyLevel hl on he.HLevelID=hl.HLevelID where HElementID in (Select HElementID from EmpHierarchy where EmpPosID=" + empPositionInfo.EmpPosID + ") order by HLevelSeqNo");

                //foreach (System.Data.DataRow hierarchyElementRow in hierarchyElementTable.Rows)
                //{
                //    EHierarchyElement obj = new EHierarchyElement();
                //    EHierarchyElement.db.toObject(hierarchyElementRow, obj);
                //    if (string.IsNullOrEmpty(businessHierarchy))
                //        businessHierarchy = obj.HElementDesc;
                //    else
                //        businessHierarchy += "\t" + obj.HElementDesc;

                //}

                DataSet.EmpTerminationListSet.TerminationListRow row = dataTable.NewTerminationListRow();
                row.EmpID = empInfo.EmpID;
                row.EmpNo = empInfo.EmpNo;
                row.CompanyID = company.CompanyID;
                row.CompanyName = company.CompanyName;
                row.EmpName = empInfo.EmpEngFullName;
                row.BusinessHierarchy = businessHierarchy;
                row.Position = position.PositionDesc;
                row.TerminationCode = cessationReason.CessationReasonCode;
                row.TerminationDesc = cessationReason.CessationReasonDesc;
                row.LastEmploymentDate = empTermination.EmpTermLastDate;

                dataTable.Rows.Add(row);


            }

            if (reportDocument == null)
            {
                reportDocument = new ReportTemplate.Report_Employee_TerminationList();
                //reportDocument.Load(@"ReportTemplate\Report_Payroll_DiscrepancyList.rpt");
            }
            else
            {

            }


            reportDocument.SetDataSource(dataSet);

            return reportDocument;
        }
    }
}
