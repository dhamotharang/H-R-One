using System;
using System.Collections;
using System.Text;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Employee
{
    public class EmpContactProcess :GenericReportProcess
    {
        private ArrayList EmpList;

        public EmpContactProcess(DatabaseConnection dbConn, ArrayList EmpList)
            : base(dbConn)
        {
            this.EmpList = EmpList;
        }
        public override ReportDocument GenerateReport()
        {
            if (EmpList.Count > 0)
            {
                DataSet.EmployeeDetail ds = new DataSet.EmployeeDetail();

                foreach (int EmpID in EmpList)
                {
                    EmployeeDetailProcess.ImportEmployeeDetailRow(dbConn, ds.employeedetail, EmpID, AppUtils.ServerDateTime().Date);
                }

                //System.Data.DataTable table = null;
                //foreach (int EmpID in EmpList)
                //{
                //    string select = "P.*,EmpPos.*,Pos.*,Comp.* ";
                //    string from = "from EmpPersonalInfo P LEFT JOIN EmpPositionInfo EmpPos ON P.EmpID=EmpPos.EmpID AND EmpPos.EmpPosEffTo IS NULL LEFT JOIN Position Pos ON EmpPos.PositionID=Pos.PositionID LEFT JOIN Company Comp ON EmpPos.CompanyID=Comp.CompanyID";
                //    DBFilter filter = new DBFilter();
                //    filter.add(new Match("P.EmpID", EmpID));
                //    System.Data.DataTable resulttable = filter.loadData(null, select, from);
                //    if (table == null)
                //        table = resulttable;
                //    else
                //        table.Merge(resulttable);
                //}

                //DBAESEncryptStringFieldAttribute.decode(table, "EmpResAddr", true);
                //DBAESEncryptStringFieldAttribute.decode(table, "EmpCorAddr", true);

                if (reportDocument == null)
                {
                    reportDocument = new ReportTemplate.Report_Employee_Contact();
                }
                else
                {

                }

                reportDocument.SetDataSource(ds);
                return reportDocument;

            }
            else
                return null;
        }
    }
}
