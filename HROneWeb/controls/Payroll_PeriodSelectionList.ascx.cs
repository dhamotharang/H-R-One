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
using HROne.Lib.Entities;

public partial class Payroll_PeriodSelectionList : HROneWebControl
{

    protected SearchBinding payBatchSBinding;

    protected ListInfo payBatchInfo;
    protected DataView payBatchView;

    public event EventHandler PayrollBatchChecked;
    

    public enum PayrollBatchStatusSelectionOptionEnum
    {
        All = 0,
        TrialRunOnly,
        ConfirmOnly
    }

    public enum PayrollBatchCheckBoxDefaultCheckedOptionEnum
    {
        All = 0,
        ExcludeBankFileGenerated,
        ReleasePaySlipToESS
    }

    public string SelectedPayrollStatus
    {
        get { return PayrollStatus.SelectedValue; }
    }

    public bool ShowPayrollGroupDropDownList
    {
        set { hiddenFieldShowPayrollGroupDropDownList.Value = (value ? "True" : "False"); }
        get { return (hiddenFieldShowPayrollGroupDropDownList.Value.Equals("True", StringComparison.CurrentCultureIgnoreCase)); }
    }

    public bool SkipProcessEndPeriod
    {
        set { hiddenFieldSkipProcessEndPeriod.Value = (value ? "True" : "False"); }
        get { return (hiddenFieldSkipProcessEndPeriod.Value.Equals("True", StringComparison.CurrentCultureIgnoreCase)); }
    }
    public PayrollBatchStatusSelectionOptionEnum PayrollBatchStatusSelectionOption
    {
        set { hiddenFieldPayrollBatchStatusSelectionOption.Value = value.ToString(); }
        get { return (PayrollBatchStatusSelectionOptionEnum)Enum.Parse(typeof(PayrollBatchStatusSelectionOptionEnum), hiddenFieldPayrollBatchStatusSelectionOption.Value, true); }
    }

    public PayrollBatchCheckBoxDefaultCheckedOptionEnum PayrollBatchCheckBoxDefaultCheckedOption
    {
        set { hiddenFieldPayrollBatchCheckBoxDefaultCheckedOption.Value = value.ToString(); }
        get { return (PayrollBatchCheckBoxDefaultCheckedOptionEnum)Enum.Parse(typeof(PayrollBatchCheckBoxDefaultCheckedOptionEnum), hiddenFieldPayrollBatchCheckBoxDefaultCheckedOption.Value, true); }
    }

    public int DefaultMonthPeriod
    {
        set { hiddenFieldDefaultMonthPeriod.Value = value.ToString(); }
        get { return int.Parse(hiddenFieldDefaultMonthPeriod.Value); }
    }

    public DBTerm GetEmpPayrollDBTerm()
    {
        return GetEmpPayrollDBTerm(string.Empty);
    }
    public DBTerm GetEmpPayrollDBTerm(string EmpPayrollTableAlias)
    {
        if (!string.IsNullOrEmpty(EmpPayrollTableAlias))
            if (!EmpPayrollTableAlias.EndsWith(".") && !EmpPayrollTableAlias.Trim().Equals(string.Empty))
                EmpPayrollTableAlias += ".";
        AND andTerms = new AND();
        andTerms.add(new Match(EmpPayrollTableAlias + "EmpPayStatus", PayrollStatus.SelectedValue));

        //OR orPayrollBatchTerm = new OR();
        ////  Add dummy PayBatch Selection for handling unselected issue
        //orPayrollBatchTerm.add(new Match(EmpPayrollTableAlias + "PayBatchID", -1));
        if (PayrollStatus.SelectedValue.Equals("C"))
        {
            ArrayList payBatchList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollBatch.db, payBatchRepeater, "ItemSelect");
            string payrollBatchIDList = "-1";
            foreach (EPayrollBatch payBatch in payBatchList)
            {
                //orPayrollBatchTerm.add(new Match(EmpPayrollTableAlias + "PayBatchID", payBatch.PayBatchID));
                payrollBatchIDList += "," + payBatch.PayBatchID;
            }
            andTerms.add(new IN(EmpPayrollTableAlias + "PayBatchID", payrollBatchIDList, null));
        }
        else if (PayrollStatus.SelectedValue.Equals("T"))
        {
            ArrayList payPeriodList = WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollPeriod.db, payBatchRepeater, "ItemSelect");
            string payrollBatchIDList = "-1";
            foreach (EPayrollPeriod payPeriod in payPeriodList)
            {
                //orPayrollBatchTerm.add(new Match(EmpPayrollTableAlias + "PayPeriodID", payPeriod.PayPeriodID));
                payrollBatchIDList += "," + payPeriod.PayPeriodID;
            }
            andTerms.add(new IN(EmpPayrollTableAlias + "PayPeriodID", payrollBatchIDList, null));
        }
        //andTerms.add(orPayrollBatchTerm);
        return andTerms;
    }

    public ArrayList GetPayBatchList()
    {
        if (PayrollStatus.SelectedValue.Equals("C"))
            return WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollBatch.db, payBatchRepeater, "ItemSelect");
        else
            return null;
    }

    public ArrayList GetTrialRunPayPeriodList()
    {
        if (PayrollStatus.SelectedValue.Equals("T"))
            return WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollPeriod.db, payBatchRepeater, "ItemSelect");
        else
            return null;
    }

    public DateTime GetPeriodDateFrom()
    {
        DateTime dtPeriodFrom = new DateTime();
        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFrom))
            return dtPeriodFrom;
        else
            return dtPeriodFrom;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        chkPayBatchCheckAll.Attributes.Add("onclick", "checkAll('" + payBatchRepeater.ClientID + "','ItemSelect',this.checked);");
        btnSelectAll.Attributes.Add("onclick", "checkAll('" + payBatchRepeater.ClientID + "','ItemSelect', true);");
        btnClearAll.Attributes.Add("onclick", "checkAll('" + payBatchRepeater.ClientID + "','ItemSelect', false);");

        payBatchSBinding = new SearchBinding(dbConn, EPayrollGroup.db);
        if (ShowPayrollGroupDropDownList)
        {
            if (string.IsNullOrEmpty(PayGroupID.SelectedValue))
            {
                initPayrollGroup();
            }
            // End 0000069, KuangWei, 2014-08-26
            PayGroupID.Visible = true;
        }
        else
        {
            PayGroupID.Visible = false;
        }

        if (PayrollBatchStatusSelectionOption==PayrollBatchStatusSelectionOptionEnum.All)
            PayrollStatusRow.Visible = true;
        else
        {
            PayrollStatusRow.Visible = false;
            if (PayrollBatchStatusSelectionOption == PayrollBatchStatusSelectionOptionEnum.ConfirmOnly)
                PayrollStatus.SelectedValue = "C";
            else if (PayrollBatchStatusSelectionOption == PayrollBatchStatusSelectionOptionEnum.TrialRunOnly)
                PayrollStatus.SelectedValue = "T";
        }

        payBatchSBinding.initValues("PayBatchFileGenBy", null, EUser.VLUserName, null);
        payBatchSBinding.init(DecryptedRequest, null);
        
        payBatchInfo = PayBatchListFooter.ListInfo;
    }

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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

        if (PayrollStatus.SelectedValue.Equals("C") && !SkipProcessEndPeriod)
            PayPeriodSelectionRow.Visible = true;
        else
            PayPeriodSelectionRow.Visible = false;

        if (!Page.IsPostBack)
        {
            //DateTime currentDateTime = AppUtils.ServerDateTime();
            //PayPeriodFr.Value = currentDateTime.AddDays(-15).ToString("yyyy-MM-01");
            payBatchView = payBatchloadData(payBatchInfo, null, payBatchRepeater);
            if (PayrollBatchChecked != null)
                PayrollBatchChecked(sender, e);
        }

    }
    public DataView payBatchloadData(ListInfo info, DBManager db, Repeater repeater)
    {

        DBFilter filter = payBatchSBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "pg.PayGroupCode, pg.PayGroupDesc, pp.PayPeriodFr, pp.PayPeriodTo";
        string from = "from [" + EPayrollGroup.db.dbclass.tableName + "] pg, [" + EPayrollPeriod.db.dbclass.tableName + "] pp";

        filter.add(new MatchField("pg.PayGroupID", "pp.PayGroupID"));
        // Start 0000069, KuangWei, 2014-08-28

        int m_payGroupID;

        if (int.TryParse(PayGroupID.SelectedValue, out m_payGroupID) && m_payGroupID > 0)
        {
            filter.add(new Match("pg.PayGroupID", m_payGroupID));
        }
        else
        {
            DBFilter m_payrollGroupUsersFilter = new DBFilter();
            m_payrollGroupUsersFilter.add(new Match("UserID", WebUtils.GetCurUser(Session).UserID));

            DBFilter m_payrollGroupFilter = new DBFilter();
            m_payrollGroupFilter.add(new Match("PayGroupIsPublic", true));

            OR m_OrPayGroupID = new OR();
            m_OrPayGroupID.add(new IN("pg.PayGroupID", "SELECT PayGroupID FROM PayrollGroupUsers", m_payrollGroupUsersFilter));
            m_OrPayGroupID.add(new IN("pg.PayGroupID", "SELECT PayGroupID FROM PayrollGroup", m_payrollGroupFilter));

            filter.add(m_OrPayGroupID);
        }


        // End 0000069, KuangWei, 2014-08-28

        if (PayrollStatus.SelectedValue.Equals("C"))
        {
            if (SkipProcessEndPeriod)
                filter.add(new Match("pp.PayPeriodStatus", "<>", "E"));
            else
            {
                DateTime dtPeriodFr = new DateTime(), dtPeriodTo = new DateTime();
                if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr))
                    filter.add(new Match("pp.PayPeriodTo", ">=", dtPeriodFr));

                if (DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
                    filter.add(new Match("pp.PayPeriodFr", "<=", dtPeriodTo));
                if (dtPeriodFr.Ticks.Equals(0) || dtPeriodTo.Ticks.Equals(0))
                    filter.add(new Match("pp.PayPeriodID", "=", 0));
            }
        }

        DBFilter empPayrollFilter = new DBFilter();
        //empPayrollFilter.add(new MatchField("pb.payBatchID", "ep.payBatchID"));
        empPayrollFilter.add(new Match("ep.EmpPayStatus", PayrollStatus.SelectedValue));


        empPayrollFilter.add(new MatchField("ep.PayPeriodID", "pp.PayPeriodID"));

        //DBFilter payRecordFilter = new DBFilter();
        ////payRecordFilter.add(new MatchField("ep.EmpPayrollID", "pr.EmpPayrollID"));

        //OR orPayMethodTerm = new OR();
        //orPayMethodTerm.add(new Match("PayRecMethod", "A"));

        //if (bankFileControl != null)
        //{
        //    if (bankFileControl.IsAllowChequePayment())
        //        orPayMethodTerm.add(new Match("PayRecMethod", "Q"));

        //}
        //payRecordFilter.add(orPayMethodTerm);
        ////Exists existsPayRec = new Exists(EPaymentRecord.db.dbclass.tableName + " pr", payRecordFilter);
        ////empPayrollFilter.add(existsPayRec);

        ////Exists exists = new Exists("EmpPayroll ep", empPayrollFilter);
        ////        IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + " group by EmpPayrollID, PayRecMethod) pr", payRecordFilter);
        //IN existsPayRec = new IN("ep.EmpPayrollID", "Select distinct pr.EmpPayrollID from (select distinct EmpPayrollID,PayRecMethod from " + EPaymentRecord.db.dbclass.tableName + ") pr", payRecordFilter);
        //empPayrollFilter.add(existsPayRec);

        if (PayrollStatus.SelectedValue.Equals("C"))
        {
            select += " , pb.PayBatchID, pb.PayBatchConfirmDate, pb.PayBatchFileGenDate, pb.PayBatchValueDate, pb.PayBatchFileGenBy, pb.PayBatchRemark ";
            from += ", [" + EPayrollBatch.db.dbclass.tableName + "] pb ";
            IN exists = new IN("pb.payBatchID", "Select distinct ep.payBatchID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);
            filter.add(exists);
        }
        else if (PayrollStatus.SelectedValue.Equals("T"))
        {
            select += " , pp.PayPeriodID, NULL AS PayBatchConfirmDate, NULL AS PayBatchFileGenDate, NULL AS PayBatchValueDate, NULL AS PayBatchFileGenBy, NULL AS PayBatchRemark ";
            IN exists = new IN("pp.PayPeriodID", "Select distinct ep.PayPeriodID from " + EEmpPayroll.db.dbclass.tableName + " ep", empPayrollFilter);
            filter.add(exists);
        }

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, payBatchInfo);

        //if (table.Rows.Count != 0)
        //{
        //    panelPayPeriod.Visible = true;
        //    btnGenerate.Visible = true & IsAllowEdit;
        //    btnAutoPayList.Visible = true & IsAllowEdit;
        //    btnAutoPayListExcel.Visible = true & IsAllowEdit;
        //}
        //else
        //{
        //    panelPayPeriod.Visible = false;
        //    btnGenerate.Visible = false;
        //    btnAutoPayList.Visible = false;
        //}
        payBatchView = new DataView(table);

        PayBatchListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = payBatchView;
            repeater.DataBind();
        }

        return payBatchView;
    }
    protected void payBatchChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (payBatchInfo.orderby == null)
            payBatchInfo.order = true;
        else if (payBatchInfo.orderby.Equals(id))
            payBatchInfo.order = !payBatchInfo.order;
        else
            payBatchInfo.order = true;
        payBatchInfo.orderby = id;

        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);
        if (PayrollBatchChecked != null)
            PayrollBatchChecked(sender, e);

    }
    protected void payBatchRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");

        if (PayrollBatchCheckBoxDefaultCheckedOption == PayrollBatchCheckBoxDefaultCheckedOptionEnum.All)
            cb.Checked = true;
        else
            cb.Checked = false;

        if (PayrollStatus.SelectedValue.Equals("C"))
        {
            EPayrollBatch obj = new EPayrollBatch();
            EPayrollBatch.db.toObject(row.Row, obj);
            if (EPayrollBatch.db.select(dbConn, obj))
            {
                Hashtable values = new Hashtable();
                EPayrollBatch.db.populate(obj, values);

                WebFormUtils.LoadKeys(EPayrollBatch.db, row, cb);
                Binding ebinding = new Binding(dbConn, EPayrollBatch.db);
                ebinding.add(new BlankZeroLabelVLBinder(EPayrollBatch.db, (Label)e.Item.FindControl("PayBatchFileGenBy"), EUser.VLUserName));
                ebinding.init(Request, Session);
                ebinding.toControl(values);
                if (PayrollBatchCheckBoxDefaultCheckedOption == PayrollBatchCheckBoxDefaultCheckedOptionEnum.ReleasePaySlipToESS)
                    cb.Checked = obj.PayBatchIsESSPaySlipRelease;
                else if (row.Row.IsNull("PayBatchFileGenDate") && PayrollBatchCheckBoxDefaultCheckedOption == PayrollBatchCheckBoxDefaultCheckedOptionEnum.ExcludeBankFileGenerated)
                    cb.Checked = true;
            }
        }
        else if (PayrollStatus.SelectedValue.Equals("T"))
            WebFormUtils.LoadKeys(EPayrollPeriod.db, row, cb);

        //if (row["PayBatchFileGenDate"] != DBNull.Value)
        //    cb.Checked = false;
    }

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
                PayPeriodTo.Focus();
            }
            else if (sender == PayPeriodTo.TextBox && !dtPeriodTo.Ticks.Equals(0) && dtPeriodFr.Ticks.Equals(0))
                PayPeriodFr.Value = dtPeriodTo.AddDays(1).AddMonths(-DefaultMonthPeriod).ToString("yyyy-MM-dd");
        }
        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);
        if (PayrollBatchChecked != null)
            PayrollBatchChecked(sender, e);

    }

    protected void PayBatchItem_OnCheckedChanged(object sender, EventArgs e)
    {
        if (PayrollBatchChecked != null)
            PayrollBatchChecked(sender, e);
    }
    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        payBatchView = payBatchloadData(payBatchInfo, EPayrollBatch.db, payBatchRepeater);
        if (PayrollBatchChecked != null)
            PayrollBatchChecked(sender, e);
    }

    public void SaveAsReleaseESSPaySlip()
    {

        foreach (RepeaterItem i in payBatchRepeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            EPayrollBatch o = new EPayrollBatch();
            WebFormUtils.GetKeys(EPayrollBatch.db, o, cb);
            if (EPayrollBatch.db.select(dbConn, o))
            {
                if (o.PayBatchIsESSPaySlipRelease != cb.Checked)
                {
                    o.PayBatchIsESSPaySlipRelease = cb.Checked;
                    EPayrollBatch.db.update(dbConn, o);
                }
            }
        }
    }
}
