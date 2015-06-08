//using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using HROne.DataAccess;
////using perspectivemind.validation;
//using HROne.Lib.Entities;
//using HROne.Payroll;

//public partial class Payroll_ProcessContinuous : HROneWebPage
//{
//    private const string TRIALRUN_FUNCTION_CODE = "PAY004";
//    private const string CONFIRM_FUNCTION_CODE = "PAY007";
//    private const string PROCESSEND_FUNCTION_CODE = "PAY008";
//    public Binding binding;

//    public DBManager db = EPayrollGroup.db;
//    public int CurID = -1;
//    public int CurPayPeriodID = -1;


//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!WebUtils.CheckAccess(Response, Session, TRIALRUN_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//            return;
//        if (!WebUtils.CheckAccess(Response, Session, CONFIRM_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//            return;
//        if (!WebUtils.CheckAccess(Response, Session, PROCESSEND_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//            return;
//        //if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
//        //{
//        //    panelProcessEndOption.Visible = false;
//        //}

//        btnProcessEnd.OnClientClick = HROne.Translation.PromptMessage.PAYROLL_PROCESS_END_GENERIC_JAVASCRIPT;

//        binding = new Binding(dbConn, db);
//        binding.add(new DropDownVLBinder(db, PayGroupID, EPayrollGroup.VLPayrollGroup));
//        binding.add(CurrentPayPeriodID);

//        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
//            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
//                CurID = -1;

//        DBFilter payPeriodFilter = new DBFilter();
//        payPeriodFilter.add(new Match("PayPeriodStatus", "<>", "E"));
//        payPeriodFilter.add(new Match("PayGroupID", CurID));

//        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

//        binding.init(Request, Session);



//        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
//            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurPayPeriodID))
//            {
//                EPayrollGroup obj = new EPayrollGroup();
//                obj.PayGroupID = CurID;
//                if (EPayrollGroup.db.select(dbConn, obj))
//                    CurPayPeriodID = obj.CurrentPayPeriodID;
//                else
//                    CurPayPeriodID = -1;
//            }


//        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

//        if (!Page.IsPostBack)
//        {
//            loadObject();
//            if (CurID > 0)
//            {
//                panelPayPeriod.Visible = true;
//            }
//            else
//            {
//                panelPayPeriod.Visible = false;
//                panelProcessEndOption.Visible = false;
//            }

//        }

//    }

//    protected bool loadObject()
//    {
//        EPayrollGroup obj = new EPayrollGroup();
//        obj.PayGroupID = CurID;
//        EPayrollGroup.db.select(dbConn, obj);

//        Hashtable values = new Hashtable();
//        db.populate(obj, values);
//        binding.toControl(values);

//        if (CurPayPeriodID <= 0)
//        {
//            CurPayPeriodID = obj.CurrentPayPeriodID;
//        }
//        try
//        {
//            PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
//        }
//        catch
//        {
//            if (PayPeriodID.Items.Count > 1)
//            {
//                CurPayPeriodID = int.Parse(PayPeriodID.Items[1].Value);
//                PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
//            }
//        }
//        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;

//        if (CurPayPeriodID > 0)
//        {
//            panelPayPeriod.Visible = true;
//            panelProcessEndOption.Visible = true;
//        }
//        else
//        {
//            panelPayPeriod.Visible = false;
//            panelProcessEndOption.Visible = false;
//        }

//        return true;
//    }

//    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        loadObject();
//        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue);
//    }
//    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        loadObject();
//        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&PayPeriodID=" + PayPeriodID.SelectedValue);
//    }

//    protected void btnProcessEnd_Click(object sender, EventArgs e)
//    {
//        int noOfCycle = 0;
//        if (int.TryParse(txtNoOfCycle.Text, out noOfCycle))
//        {
//            Session.Remove("PayrollContinuousProcess_EmpList");

//            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Payroll_ProcessContinuous_Process.aspx?"
//                  + "PayPeriodID=" + CurPayPeriodID
//                  + "&PayBatchID=" + 0
//                  + "&NoOfCycleLeft=" + noOfCycle
//                  + "&Total=" + 0);
//        }
//        else
//        {
//            PageErrors error = PageErrors.getErrors(db, Page.Master);
//            error.addError("Invalid number of cycle");
//        }
//    }
//}
