using System;
using System.Collections;
using System.Text;
using HROne.Common;
using CrystalDecisions.CrystalReports.Engine;
using HROne.DataAccess;

namespace HROne.Reports.Employee
{
    public class EmployeeListProcess:GenericReportProcess 
    {
        private ArrayList EmpList;

        public EmployeeListProcess(DatabaseConnection dbConn, ArrayList EmpList)
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
                    reportDocument = new ReportTemplate.Report_Employee_List();
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
