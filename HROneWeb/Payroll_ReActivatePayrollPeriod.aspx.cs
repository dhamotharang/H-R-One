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
using HROne.Translation;
using HROne.CommonLib;

public partial class Payroll_ReActivatePayrollPeriod : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY900";
    public SearchBinding payPeriodSBinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    protected ListInfo payPeriodInfo;
    protected DataView payperiodView;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            panelRollbackOption.Visible = false;
        }
        else
        {
            panelRollbackOption.Visible = true;
        }

        btnRollback.OnClientClick = HROne.Translation.PromptMessage.PAYROLL_ROLLBACK_PAYROLL_PERIOD_GENERIC_JAVASCRIPT;

        payPeriodSBinding = new SearchBinding(dbConn, db);
        // Start 0000069, KuangWei, 2014-08-26
        //payPeriodSBinding.add(new DropDownVLSearchBinder(PayGroupID, "PayGroupID", EPayrollGroup.VLPayrollGroup, false));
        initPayrollGroup();
        // End 0000069, KuangWei, 2014-08-26
        payPeriodSBinding.initValues("PayPeriodConfirmBy", null, EUser.VLUserName, null);
        payPeriodSBinding.init(DecryptedRequest, null);

        payPeriodInfo = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!Page.IsPostBack)
        {
            payperiodView = payPeriodLoadData(payPeriodInfo, EPayrollPeriod.db, payPeriodRepeater);
        }

    }

    // Start 0000069, KuangWei, 2014-08-26
    protected void initPayrollGroup()
    {
        DBFilter m_filter = new DBFilter();
        DBFilter m_userFilter = new DBFilter();

        PayGroupID.Items.Add(new ListItem("Not Selected", "-1"));

        m_userFilter.add(new Match("UserID", WebUtils.GetCurUser(Session).UserID));

        OR m_or = new OR();

        m_or.add(new IN("PayGroupID", "SELECT PayGroupID FROM PayrollGroupUsers ", m_userFilter));
        m_or.add(new Match("PayGroupIsPublic", true));

        m_filter.add(m_or);
        m_filter.add("PayGroupCode", true);

        // since sorting is not feasible directly using DBFilter (because of the encrypted data), we use a local data table as a temp buffer
        DataTable m_localTable = new DataTable();
        m_localTable.Columns.Add("PayGroupCode", typeof(string));
        m_localTable.Columns.Add("PayGroupDesc", typeof(string));
        m_localTable.Columns.Add("PayGroupID", typeof(int));

        foreach (EPayrollGroup o in EPayrollGroup.db.select(dbConn, m_filter))
        {
            DataRow m_row = m_localTable.NewRow();
            m_row["PayGroupCode"] = o.PayGroupCode;
            m_row["PayGroupDesc"] = o.PayGroupDesc;
            m_row["PayGroupID"] = o.PayGroupID;
            m_localTable.Rows.Add(m_row);
        }

        foreach (DataRow m_o in m_localTable.Select("", "payGroupCode"))
        {
            PayGroupID.Items.Add(new ListItem(m_o["PayGroupCode"].ToString() + " - " + m_o["PayGroupDesc"].ToString(), m_o["PayGroupID"].ToString()));
        }
    }
    // End 0000069, KuangWei, 2014-08-26

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        payperiodView = payPeriodLoadData(payPeriodInfo, EPayrollPeriod.db, payPeriodRepeater);
    }

    public DataView payPeriodLoadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = payPeriodSBinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        //else
        //    filter.add("PayPeriodFr", true);


        filter.add(new Match("pp.PayPeriodStatus", "=", "E"));

        //if (CurPayGroupID > 0)
        //    filter.add(new Match("PayGroupID", CurID));
        //else
        //    filter.add(new Match("PayGroupID", 0));


        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr))
            filter.add(new Match("PayPeriodTo", ">=", dtPeriodFr));
        if (DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
            filter.add(new Match("PayPeriodFr", "<=", dtPeriodTo));

        // Start 0000069, KuangWei, 2014-08-27
        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;
        filter.add(new Match("PayGroupID", CurID));
        // End 0000069, KuangWei, 2014-08-27

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "pp.* ";
        string from = "from [" + db.dbclass.tableName + "] pp ";

        //filter.add(new Match("pp.PayPeriodStatus", "<>", "T"));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        payperiodView = new DataView(table);

        if (table.Rows.Count > 0)
        {
            panelPayPeriod.Visible = true;

        }
        else
        {
            panelPayPeriod.Visible = false;
        }


        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = payperiodView;
            repeater.DataBind();
        }

        return payperiodView;
    }

    protected void payPeriodRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EPayrollPeriod.db, row, cb);
    }
    protected void Search_Click(object sender, EventArgs e)
    {

        payperiodView = payPeriodLoadData(payPeriodInfo, EPayrollPeriod.db, payPeriodRepeater);
    }

    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (payPeriodInfo.orderby == null)
            payPeriodInfo.order = true;
        else if (payPeriodInfo.orderby.Equals(id))
            payPeriodInfo.order = !payPeriodInfo.order;
        else
            payPeriodInfo.order = true;
        payPeriodInfo.orderby = id;

        payperiodView = payPeriodLoadData(payPeriodInfo, EPayrollPeriod.db, payPeriodRepeater);

    }

    //public DBFilter getPayrollPeriodFilter()
    //{
    //    DBFilter filter = new DBFilter();
    //    filter.add(new Match("PayGroupID", CurPayGroupID));
    //    DateTime dtPayPeriodFr;
    //    DateTime dtPayPeriodTo;
    //    if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
    //    {
    //        filter.add(new Match("pp.PayPeriodFr", "<=", dtPayPeriodTo));
    //        filter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
    //    }
    //    return filter;
    //}
    protected void btnRollback_Click(object sender, EventArgs e)
    {


        PageErrors pageErrors = PageErrors.getErrors(db, Page.Master);
        if (!IsValidRollbackKey(txtPassCode.Text))
        {
            pageErrors.addError("Incorrect pass code!");
            return;
        }
        else
        {
            ArrayList payPeriodList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollPeriod.db, payPeriodRepeater, "ItemSelect");

            if (payPeriodList.Count > 0)
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, true);
                foreach (EPayrollPeriod payPeriod in payPeriodList)
                {
                    if (EPayrollPeriod.db.select(dbConn, payPeriod))
                    {
                        payPeriod.PayPeriodStatus = "C";
                        EPayrollPeriod.db.update(dbConn, payPeriod);
                    }
                }
                WebUtils.EndFunction(dbConn);
                pageErrors.addError("Payroll cycle is re-activated successfully.");
                payperiodView = payPeriodLoadData(payPeriodInfo, EPayrollPeriod.db, payPeriodRepeater);

                //if (CurID > 0)
                //{
                //    panelPayPeriod.Visible = true;
                //    loadObject();
                //}
                //else
                //{
                //    panelPayPeriod.Visible = false;
                //    panelRollbackOption.Visible = false;
                //}
            }
        }

        //        Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
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

    // Start 0000004, Miranda, 2014-06-19
    protected void PayPeriod_Changed(object sender, EventArgs e)
    {
        DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
        DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr);
        DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo);
        if (!dtPeriodFr.Ticks.Equals(0) && !dtPeriodTo.Ticks.Equals(0))
        {
            if (dtPeriodFr >= dtPeriodTo)
            {
                if (sender == PayPeriodFr.TextBox)
                    PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");
                else if (sender == PayPeriodTo.TextBox)
                    PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
            }
        }
        else
        {
            if (sender == PayPeriodFr.TextBox && !dtPeriodFr.Ticks.Equals(0) && dtPeriodTo.Ticks.Equals(0))
            {
                PayPeriodTo.Value = dtPeriodFr.AddMonths(DefaultMonthPeriod).AddDays(-1).ToString("yyyy-MM-dd");

            }
            else if (sender == PayPeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        }
    }

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }
    // End 0000004, Miranda, 2014-06-19

}
