using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Payroll;
using HROne.CommonLib;

public partial class Payroll_GroupProcessContinuous : HROneWebPage
{
    private const string TRIALRUN_FUNCTION_CODE = "PAY004";
    private const string CONFIRM_FUNCTION_CODE = "PAY007";
    private const string PROCESSEND_FUNCTION_CODE = "PAY008";
    private const string ROLLBACK_FUNCTION_CODE = "PAY900";

    public DBManager db = EPayrollGroup.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, TRIALRUN_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (!WebUtils.CheckAccess(Response, Session, CONFIRM_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (!WebUtils.CheckAccess(Response, Session, PROCESSEND_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        btnProcessEnd.OnClientClick = HROne.Translation.PromptMessage.PAYROLL_PROCESS_END_GENERIC_JAVASCRIPT;

        binding = new SearchBinding(dbConn, db);
        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);

        }

    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + db.dbclass.tableName + "] c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);

    }

    protected void btnProcessEnd_Click(object sender, EventArgs e)
    {
        PageErrors pageErrors = PageErrors.getErrors(db, Page.Master);
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollGroup.db, Repeater, "ItemSelect");

        if (list.Count <= 0)
        {
            pageErrors.addError("no payroll group is selected");
        }

        string payGroupStringList = string.Empty;
        if (!string.IsNullOrEmpty(PayPeriodFr.Value) && !string.IsNullOrEmpty(PayPeriodTo.Value))
        {
            DateTime dtPayPeriodFr = DateTime.Parse(PayPeriodFr.Value);
            DateTime dtPayPeriodTo = DateTime.Parse(PayPeriodTo.Value);

            foreach (EPayrollGroup payGroup in list)
            {
                DBFilter payPeriodFilter = new DBFilter();
                payPeriodFilter.add(new Match("PayGroupID", payGroup.PayGroupID));
                payPeriodFilter.add(new Match("PayPeriodFr", "<=", dtPayPeriodFr));
                payPeriodFilter.add(new Match("PayPeriodTo", ">=", dtPayPeriodFr));
                ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
                if (payPeriodList.Count <= 0)
                {
                    pageErrors.addError("Start Period does NOT exists");
                    break;
                }

                if (string.IsNullOrEmpty(payGroupStringList))
                    payGroupStringList = payGroup.PayGroupID.ToString();
                else
                    payGroupStringList += "|" + payGroup.PayGroupID.ToString();
            }



            if (pageErrors.isEmpty())
            {
                if (RollbackPayrollProcess.Checked)
                {
                    if (!WebUtils.CheckPermission( Session, ROLLBACK_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
                    {
                        pageErrors.addError("privilege for Rollback Payroll Process is required!");
                        return;
                    }
                    if (!IsValidRollbackKey(txtPassCode.Text))
                    {
                        pageErrors.addError("Incorrect pass code!");
                        return;
                    }
                    else
                    {
                        // Start rollback process
                        WebUtils.StartFunction(Session, ROLLBACK_FUNCTION_CODE, true);
                        PayrollProcess payrollProcess = new PayrollProcess(dbConn);
                        foreach (EPayrollGroup payGroup in list)
                        {
                            DBFilter payPeriodFilter = new DBFilter();
                            payPeriodFilter.add(new Match("PayGroupID", payGroup.PayGroupID));
                            payPeriodFilter.add(new Match("PayPeriodTo", ">=", dtPayPeriodFr));
                            payPeriodFilter.add(new Match("PayPeriodFr", "<=", dtPayPeriodTo));
                            ArrayList payPeriodList = EPayrollPeriod.db.select(dbConn, payPeriodFilter);
                            foreach (EPayrollPeriod payPeriod in payPeriodList)
                            {
                                payrollProcess.RollBackPayroll(payPeriod.PayPeriodID, WebUtils.GetCurUser(Session).UserID);
                            }
                        }
                        WebUtils.EndFunction(dbConn);

                    }
                }

                Session.Remove("PayrollContinuousProcess_EmpList");
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Payroll_GroupProcessContinuous_Process.aspx?"
                          + "PayGroupIDList=" + payGroupStringList
                          + "&PayPeriodFr=" + dtPayPeriodFr.Ticks
                          + "&PayPeriodTo=" + dtPayPeriodTo.Ticks
                          + "&SkipRecurringPaymentProcess=" + (SkipRecurringPaymentProcess.Checked ? "Yes" : "No")
                          + "&SkipClaimsAndDeductionsProcess=" + (SkipClaimsAndDeductionsProcess.Checked ? "Yes" : "No")
                          + "&SkipYearEndBonusProcess=" + (SkipYearEndBonusProcess.Checked ? "Yes" : "No")
                          + "&SkipAdditionalRenumerationProcess=" + (SkipAdditionalRenumerationProcess.Checked ? "Yes" : "No")
                            );
            }
        }
        else
        {
            pageErrors.addError("Invalid number of cycle");
        }
    }

    public bool IsValidRollbackKey(string trialKey)
    {
        DateTime permittedDate = getDateFromKey(trialKey);
        if (permittedDate.Equals(AppUtils.ServerDateTime().Date))
            return true;
        else
            return false;
    }

    private DateTime getDateFromKey(string trialKey)
    {
        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.DES);
        try
        {
            trialKey = base32.ConvertBase32ToBase64(trialKey);

            string realTrialKey = crypto.Decrypting(trialKey, "HROne");
            string strYear = realTrialKey.Substring(0, 4);
            string strMonth = realTrialKey.Substring(4, 2);
            string strDay = realTrialKey.Substring(6, 2);

            return new DateTime(int.Parse(strYear), int.Parse(strMonth), int.Parse(strDay));

        }
        catch
        {
            return new DateTime();
        }
    }
}
