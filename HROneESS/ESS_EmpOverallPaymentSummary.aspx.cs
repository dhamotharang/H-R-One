using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class ESS_EmpOverallPaymentSummary : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public int CurID = -1;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            CurID = user.EmpID;


        if (!IsPostBack)
        {

        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }
    }

    protected bool loadObject()
    {
        EEmpPersonalInfo obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;
        // bool isNew = WebFormWorkers.loadKeys(db, obj, Request);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        DateTime dtPayPeriodFr = new DateTime();
        if (!DateTime.TryParse(PayrollPeriodFrom.Value, out dtPayPeriodFr))
        {
            errors.addError("Invalid Date Format");
        }
        else
        {
            if (dtPayPeriodFr.Day != 1)     // 2013-05-23 => 2013-05-01
            {
                dtPayPeriodFr = new DateTime(dtPayPeriodFr.Year, dtPayPeriodFr.Month, 1);
            }

            ArrayList empList = new ArrayList();

            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            empInfo.EmpID = CurID;

            if (EEmpPersonalInfo.db.select(dbConn, empInfo) && errors.isEmpty())    // employee found !
            {
                empList.Add(empInfo);

                HROne.Reports.Payroll.EEOverallPaymentSummaryProcess rpt = new HROne.Reports.Payroll.EEOverallPaymentSummaryProcess(dbConn, empList, dtPayPeriodFr);
                string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_EEOverallPaymentSummary.rpt"));
                WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "OverallPaymentSummary");
            }
        }


/////////////////////////////////////////////////////

        //PageErrors errors = PageErrors.getErrors(db, Page.Master);
        //errors.clear();

        //ArrayList empList = new ArrayList();
        //ArrayList payBatchList = new ArrayList();
        //DateTime dtPayPeriodFr = new DateTime();

        //foreach (RepeaterItem i in Repeater.Items)
        //{
        //    CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
        //    if (cb.Checked)
        //    {
        //        EEmpPersonalInfo o = (EEmpPersonalInfo)EEmpPersonalInfo.db.createObject();
        //        WebFormUtils.GetKeys(EEmpPersonalInfo.db, o, cb);
        //        empList.Add(o);
        //    }

        //}

        //if (empList.Count > 0)
        //{

        //    if (DateTime.TryParse(((WebDatePicker)EmployeeSearchControl1.AdditionElementControl.FindControl("PayPeriodFr")).Value, out dtPayPeriodFr))
        //    {
        //    }
        //    else
        //        errors.addError("Invalid Date Format");
        //}
        //else
        //    errors.addError("Employee not selected");

        //if (errors.isEmpty())
        //{
        //    HROne.Reports.Payroll.EEOverallPaymentSummaryProcess rpt = new HROne.Reports.Payroll.EEOverallPaymentSummaryProcess(dbConn, empList, dtPayPeriodFr);

        //    string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Payroll_EEOverallPaymentSummary.rpt"));

        //    WebUtils.ReportExport(dbConn, user, errors, lblReportHeader.Text, Response, rpt, reportFileName, ((Button)sender).CommandArgument, "EEOverallPaymentSummary", true);
        //}

    }

}

