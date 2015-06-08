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

public partial class Payroll_TrialRunAdjust_List : HROneWebPage
{
    protected DBManager db = EEmpPersonalInfo.db;
    protected SearchBinding sbinding;
    protected Binding binding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, "PAY005", WebUtils.AccessLevel.Read))
            return;


        binding = new Binding(dbConn, EPayrollGroup.db);
        // Start 0000069, KuangWei, 2014-08-26
        //binding.add(new DropDownVLBinder(EPayrollGroup.db, PayGroupID, EPayrollGroup.VLPayrollGroup));        
        initPayrollGroup();
        // End 0000069, KuangWei, 2014-08-26
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpName, "EmpEngSurname", "EmpEngOtherName", "EmpChiFullName"));
        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus).setLocale(ci));
        //sbinding.add(new DropDownVLSearchBinder(PayGroupID, "pp.PayGroupID", EPayrollGroup.VLPayrollGroup).setLocale(ci));

        sbinding.init(DecryptedRequest, null);
        // Start 0000069, KuangWei, 2014-08-27
        //if (string.IsNullOrEmpty(PayGroupID.SelectedValue))
        if (string.IsNullOrEmpty(PayGroupID.SelectedValue) || PayGroupID.SelectedValue.Equals("-1"))
        // End 0000069, KuangWei, 2014-08-27
            panelEmployeeList.Visible = false;
        else
            panelEmployeeList.Visible = true;

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(Page, Page.Controls);



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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, ep.EmpPayrollID";
        string from = "from [" + db.dbclass.tableName + "] e, EmpPayroll ep, PayrollPeriod  pp";
        filter.add(new Match("PayGroupID", PayGroupID.SelectedValue));
        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(new MatchField("ep.PayperiodID", "pp.PayperiodID"));
        filter.add(new Match("ep.EmpPayStatus", "T"));
        filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
        filter.add(new IN("e.EmpID", "Select EmpID From " + EEmpPersonalInfo.db.dbclass.tableName + " ee", EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime())));

        DataTable table = filter.loadData(dbConn, null, select, from);

        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);

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
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        EmployeeSearchControl1.Reset();
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


    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
}
