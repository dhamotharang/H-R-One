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
public partial class controls_Payroll_ConfirmedPeriod_List : HROneWebControl
{
    protected ListInfo payPeriodInfo;
    protected DataView payperiodView;
    protected int CurPayGroupID;
    protected SearchBinding payPeriodSBinding;
    protected string m_PayrollStatus = string.Empty;
    public event EventHandler AfterSearch;
    
    protected void Page_Load(object sender, EventArgs e)
    {
       

        payPeriodSBinding = new SearchBinding(dbConn, EPayrollPeriod.db);
        payPeriodSBinding.initValues("PayPeriodConfirmBy", null, EUser.VLUserName, null);

        payPeriodInfo = ListFooter.ListInfo;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurPayGroupID > 0)
            {
                Refresh();
            }
        }

    }

    public int PayGroupID
    {
        set { CurPayGroupID = value; }
        get { return CurPayGroupID; }
    }

    public void Refresh()
    {
        payperiodView = payPeriodLoadData(payPeriodInfo, EPayrollPeriod.db, payPeriodRepeater);
    }

    public string PayrollStatus
    {
        set { m_PayrollStatus = value; }
        get { return m_PayrollStatus; }
    }

    public int RecordCount
    {
        get
        {

            return ListFooter.ListInfo.numRecord;
            //int recordCount;

            //if (int.TryParse(lblPayPeriodRecordCount.Text, out recordCount))
            //    return recordCount;
            //else
            //    return 0;
        }
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

    public ArrayList GetSelectedBaseObjectList()
    {
        return WebUtils.SelectedRepeaterItemToBaseObjectList(EPayrollPeriod.db, payPeriodRepeater, "ItemSelect");
    }

    public DataView payPeriodLoadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = payPeriodSBinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        //else
        //    filter.add("PayPeriodFr", true);

        DBFilter empPayrollFilter = new DBFilter();
        if (m_PayrollStatus.Equals("T"))
            empPayrollFilter.add(new Match("ep.EmpPayStatus", "=", "T"));
        else
            empPayrollFilter.add(new Match("ep.EmpPayStatus", "<>", "T"));

        filter.add(new IN("PayPeriodID", "Select ep.PayPeriodID from EmpPayroll ep", empPayrollFilter));

        if (CurPayGroupID > 0)
            filter.add(new Match("PayGroupID", CurPayGroupID));
        else
            filter.add(new Match("PayGroupID", 0));


        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(PayPeriodFr.Value, out dtPeriodFr))
            filter.add(new Match("PayPeriodTo", ">=", dtPeriodFr));
        if (DateTime.TryParse(PayPeriodTo.Value, out dtPeriodTo))
            filter.add(new Match("PayPeriodFr", "<=", dtPeriodTo));

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "pp.* ";
        string from = "from [" + db.dbclass.tableName + "] pp ";

        //filter.add(new Match("pp.PayPeriodStatus", "<>", "T"));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);


        payperiodView = new DataView(table);

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
        AfterSearch(sender, e);
    }

    public DBFilter getPayrollPeriodFilter()
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("PayGroupID", CurPayGroupID));
        DateTime dtPayPeriodFr;
        DateTime dtPayPeriodTo;
        if (DateTime.TryParse(PayPeriodFr.Value, out dtPayPeriodFr) && DateTime.TryParse(PayPeriodTo.Value, out dtPayPeriodTo))
        {
            filter.add(new Match("pp.PayPeriodFr", "<=", dtPayPeriodTo));
            filter.add(new Match("pp.PayPeriodTo", ">=", dtPayPeriodFr));
        }
        return filter;
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
