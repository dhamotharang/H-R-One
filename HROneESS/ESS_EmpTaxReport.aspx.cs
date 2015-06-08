using System;
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

public partial class ESS_EmpTaxReport : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        binding = new Binding(dbConn, db);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
            CurID = user.EmpID;

        DBFilter empTaxFilter = new DBFilter();
        empTaxFilter.add(new Match("te.EmpID", CurID));
        empTaxFilter.add(new Match("te.TaxEmpIsReleasePrintoutToESS", true));
        empTaxFilter.add(new MatchField("TaxForm.TaxFormID", "te.TaxFormID"));

        //if (!IsPostBack)
        {

            DBFilter taxFormFilter = new DBFilter();
            taxFormFilter.add(new Match("TaxFormType", "B"));
            taxFormFilter.add("TaxFormYear", false);
            taxFormFilter.add(new Exists("TaxEmp te", empTaxFilter));
            binding.add(new DropDownVLBinder(ETaxForm.db, TaxFormID, ETaxForm.VLTaxFormYear, taxFormFilter));

        }


        binding.init(Request, Session);


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
        obj = new EEmpPersonalInfo();
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


        int intTaxFormID = 0;

        if (int.TryParse(TaxFormID.SelectedValue, out intTaxFormID))
        {
            DBFilter TaxEmpFilter = new DBFilter();
            TaxEmpFilter.add(new Match("EmpID", CurID));
            TaxEmpFilter.add(new Match("TaxFormID", intTaxFormID));
            ArrayList TaxEmpList = ETaxEmp.db.select(dbConn, TaxEmpFilter);

            //string hLevelIDListString = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIERARCHY_DISPLAY_SEQUENCE);
            //string[] hlevelIDList = hLevelIDListString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            //DBFilter payBatchFilter = new DBFilter();
            //DBFilter empPayrollFilter = new DBFilter();
            //empPayrollFilter.add(new Match("ep.PayPeriodID", PayPeriodID.SelectedValue));
            //payBatchFilter.add(new IN("PayBatchID", "SELECT DISTINCT ep.PayBatchID FROM " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter));
            //if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE).Equals("N", StringComparison.CurrentCultureIgnoreCase))
            //    payBatchFilter.add(new Match("PayBatchIsESSPaySlipRelease", true));


            HROne.Reports.Taxation.TaxationFormProcess rpt = new HROne.Reports.Taxation.TaxationFormProcess(dbConn, TaxEmpList, intTaxFormID, "B", string.Empty, HROne.Reports.Taxation.TaxationFormProcess.ReportLanguage.English);
            string reportFileName = WebUtils.GetLocalizedReportFile(Server.MapPath("~/Report_Taxation_IR56B.rpt"));
            WebUtils.ReportExport(Response, rpt, reportFileName, "PDF", "IR56B", false);
        }
    }
}
