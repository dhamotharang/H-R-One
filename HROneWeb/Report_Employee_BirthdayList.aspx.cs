using System;
using System.Text;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;
//using perspectivemind.validation;

using HROne.Lib.Entities;

public partial class Report_Employee_BirthdayList : HROneWebPage
{
    private const string FUNCTION_CODE = "RPT110";
    protected DBManager db = EEmpPersonalInfo.db;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


    }





    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EEmpPersonalInfo.db, this.Master);
        errors.clear();

        DateTime dtBirthdayFrom = new DateTime();
        DateTime dtBirthdayTo = new DateTime();

        if (!DateTime.TryParse(BirthdayFrom.Value, out dtBirthdayFrom))
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT, new string[] { BirthdayFrom.Value }));
        if (!DateTime.TryParse(BirthdayTo.Value, out dtBirthdayTo))
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT, new string[] { BirthdayTo.Value }));
        if (dtBirthdayFrom > dtBirthdayTo)
        {
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_PERIOD);
        }

        DBFilter filter = new DBFilter();
        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField(EEmpPersonalInfo.db.dbclass.tableName + ".EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        ArrayList empList = EEmpPersonalInfo.db.select(dbConn, filter);

        if (errors.isEmpty())
        {
            HROne.Reports.Employee.EmployeeBirthdayProcess rpt = new HROne.Reports.Employee.EmployeeBirthdayProcess(dbConn, empList, dtBirthdayFrom, dtBirthdayTo, chkDisplayYear.Checked);
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Employee_BirthdayList.rpt"));
            WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "BirthdayList", true);

            //Server.Transfer("Report_Employee_TerminationList_View.aspx?"
            //+ "EmpTermID=" + strTermEmpList
            //);
        }
        //        emploadData(empInfo, EEmpPayroll.db, empRepeater);
    }    

}
