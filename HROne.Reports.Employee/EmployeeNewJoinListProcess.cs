using System;
using System.Collections;
using System.Text;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;
using HROne.Lib.Entities;

namespace HROne.Reports.Employee
{
    public class EmployeeNewJoinListProcess:GenericReportProcess 
    {
        private ArrayList EmpList;
        private DateTime PeriodFrom;
        private DateTime PeriodTo;
        public EmployeeNewJoinListProcess(DatabaseConnection dbConn, ArrayList EmpList, DateTime PeriodFrom, DateTime PeriodTo)
            : base(dbConn)
        {
            this.EmpList = EmpList;
            this.PeriodFrom = PeriodFrom;
            this.PeriodTo = PeriodTo;
        }

        public override ReportDocument GenerateReport()
        {
            if (EmpList.Count > 0)
            {
                DataSet.EmployeeDetail ds = new DataSet.EmployeeDetail();

                foreach (EEmpPersonalInfo empInfo in EmpList)
                {
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        if (empInfo.EmpDateOfJoin >= PeriodFrom && empInfo.EmpDateOfJoin <= PeriodTo)
                            EmployeeDetailProcess.ImportEmployeeDetailRow(dbConn, ds.employeedetail, empInfo.EmpID, PeriodTo);
                    }
                }

                //System.Data.DataTable table = null;
                //foreach (int EmpID in EmpList)
                //{
                //    string select = "P.*,EmpPos.*,Pos.*,Comp.* ";
                //    string from = "from EmpPersonalInfo P LEFT JOIN EmpPositionInfo EmpPos ON P.EmpID=EmpPos.EmpID AND EmpPos.EmpPosEffTo IS NULL LEFT JOIN Position Pos ON EmpPos.PositionID=Pos.PositionID LEFT JOIN Company Comp ON EmpPos.CompanyID=Comp.CompanyID";
                //    DBFilter filter = new DBFilter();
                //    OR or = new OR();
                //    filter.add(new Match("P.EmpID", EmpID));
                //    System.Data.DataTable resulttable = filter.loadData(null, select, from);
                //    if (table == null)
                //        table = resulttable;
                //    else
                //        table.Merge(resulttable);
                //}
                //DBAESEncryptStringFieldAttribute.decode(table, "EmpHKID", true);
                //DBAESEncryptStringFieldAttribute.decode(table, "EmpPassportNo", false);


                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Employee_NewJoinList();
                }
                else
                {

                }
                reportDocument.SetDataSource(ds);
                if (reportDocument.ParameterFields["PeriodFrom"] != null)
                    reportDocument.SetParameterValue("PeriodFrom", PeriodFrom);
                if (reportDocument.ParameterFields["PeriodTo"] != null)
                    reportDocument.SetParameterValue("PeriodTo", PeriodTo);

                return reportDocument;
            }
            else
                return null;
        }
    }
}
