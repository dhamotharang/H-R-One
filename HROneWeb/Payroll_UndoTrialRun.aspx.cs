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

public partial class Payroll_UndoTrialRun : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY006";
    public Binding binding;

    protected SearchBinding sbinding;
    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected ListInfo info;
    protected DataView view;
    private bool IsAllowEdit = true;

    
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            btnUndo.Visible = false;
            UndoPayrollPanel.Visible = false;
        }

        

        binding = new Binding(dbConn, db);
        // Start 0000069, KuangWei, 2014-08-28
        initPayrollGroup();
        // End 0000069, KuangWei, 2014-08-28
        binding.add(CurrentPayPeriodID);

        DBFilter payPeriodFilter = new DBFilter();
        payPeriodFilter.add(new Match("PayPeriodStatus", "<>", "E"));
        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;
        payPeriodFilter.add(new Match("PayGroupID", CurID));
        payPeriodFilter.add("PayPeriodFr", false);

        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));
        
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);

        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurPayPeriodID))
            {
                EPayrollGroup obj = new EPayrollGroup();
                obj.PayGroupID = CurID;
                if (EPayrollGroup.db.select(dbConn, obj))
                    CurPayPeriodID = obj.CurrentPayPeriodID;
                else
                    CurPayPeriodID = -1;
            }

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    // Start 0000069, KuangWei, 2014-08-28
    protected void initPayrollGroup()
    {
        DBFilter m_filter = new DBFilter();
        DBFilter m_userFilter = new DBFilter();

        PayGroupID.Items.Add(new ListItem("Not Selected", "-1"));

        m_userFilter.add(new Match("UserID", WebUtils.GetCurUser(Session).UserID));

        m_filter.add(new IN("PayGroupID", "SELECT PayGroupID FROM PayrollGroupUsers ", m_userFilter));
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
    // End 0000069, KuangWei, 2014-08-28

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            loadObject();
            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                view = loadData(info, EEmpPayroll.db, Repeater);
            }
            else
                panelPayPeriod.Visible = false;

        }

    }

    protected bool loadObject()
    {
        obj = new EPayrollGroup();
        obj.PayGroupID = CurID;
        db.select(dbConn, obj);
        //if (!db.select(dbConn, obj))
        //return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        try
        {
            PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        }
        catch
        {
            CurPayPeriodID = 0;
        }
        if (CurPayPeriodID <= 0 && obj.CurrentPayPeriodID > 0)
        {
            CurPayPeriodID = obj.CurrentPayPeriodID;
            PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        }
        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;

        if (obj.CurrentPayPeriodID > 0)
        {
            panelPayPeriod.Visible = true;
            panelUndoTrialRunDetail.Visible = true;
        }
        else
        {
            panelPayPeriod.Visible = false;
            panelUndoTrialRunDetail.Visible = false;
        }
        return true;
    }

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        view = loadData(info, EEmpPayroll.db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue);
    }

    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
        view = loadData(info, EEmpPayroll.db, Repeater);
        //Response.Redirect(Request.Url.LocalPath + "?PayGroupID=" + PayGroupID.SelectedValue + "&PayPeriodID=" + PayPeriodID.SelectedValue);
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, ep.*";
        string from = "from [" + db.dbclass.tableName + "] ep, EmpPersonalInfo e ";

        filter.add(new MatchField("e.EmpID", "ep.EmpID"));
        filter.add(new Match("ep.EmpPayStatus", "T"));
        filter.add(new Match("ep.PayPeriodID", CurPayPeriodID));
        
        // Start 0000185, KuangWei, 2015-04-21
        //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));
        DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        // End 0000185, KuangWei, 2015-04-21

        if (table.Rows.Count > 0)
            btnUndo.Visible = IsAllowEdit;
        else
            btnUndo.Visible = false;



        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
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

        view = loadData(info, EEmpPayroll.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EEmpPayroll.db, row, cb);
        cb.Visible = IsAllowEdit;

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Binding ebinding = new Binding(dbConn, EEmpPayroll.db);
            ebinding.add(new BlankZeroLabelVLBinder(EEmpPayroll.db, (Label)e.Item.FindControl("EmpPayTrialRunBy"), EUser.VLUserName));
            ebinding.init(Request, Session);


            EEmpPayroll obj = new EEmpPayroll();
            EEmpPayroll.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            EEmpPayroll.db.populate(obj, values);
            ebinding.toControl(values);


            string[] payrollProcessTypeDescriptionList = obj.PayrollProcessTypeDescription();
            for (int i = 0; i < payrollProcessTypeDescriptionList.Length; i++)
                payrollProcessTypeDescriptionList[i] = HROne.Common.WebUtility.GetLocalizedString(payrollProcessTypeDescriptionList[i], ci);

            ((Label)e.Item.FindControl("Type")).Text = string.Join(" + ", payrollProcessTypeDescriptionList);
        }
    }

    protected void btnUndo_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpPayroll o = (EEmpPayroll)EEmpPayroll.db.createObject();
                WebFormUtils.GetKeys(EEmpPayroll.db, o, cb);
                list.Add(o);
            }

        }
        if (list.Count > 0)
        {
            PayrollProcess payrollProcess = new PayrollProcess(dbConn); 

            foreach (EEmpPayroll o in list)
            {
                if (EEmpPayroll.db.select(dbConn, o))
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID, true);
                    payrollProcess.UndoPayroll(o);
                    WebUtils.EndFunction(dbConn);
                }
            }

            DBFilter trialrunStatusEmpPayrollFilter = new DBFilter();
            trialrunStatusEmpPayrollFilter.add(new Match("EmpPayStatus", "T"));
            trialrunStatusEmpPayrollFilter.add(new Match("PayPeriodID", CurPayPeriodID));

            DBFilter confirmStatusEmpPayrollFilter = new DBFilter();
            confirmStatusEmpPayrollFilter.add(new Match("EmpPayStatus", "C"));
            confirmStatusEmpPayrollFilter.add(new Match("PayPeriodID", CurPayPeriodID));

            EPayrollPeriod payPeriod = new EPayrollPeriod();
            payPeriod.PayPeriodID = CurPayPeriodID;
            ArrayList trialrunStatusEmpPayrollList = EEmpPayroll.db.select(dbConn, trialrunStatusEmpPayrollFilter);
            ArrayList confirmStatusEmpPayrollList = EEmpPayroll.db.select(dbConn, confirmStatusEmpPayrollFilter);
            if (trialrunStatusEmpPayrollList.Count == 0 && confirmStatusEmpPayrollList.Count == 0)
                payPeriod.PayPeriodStatus = "N";
            else if (trialrunStatusEmpPayrollList.Count != 0)
                payPeriod.PayPeriodStatus = "T";
            else if (confirmStatusEmpPayrollList.Count != 0)
                payPeriod.PayPeriodStatus = "C";

            payPeriod.PayPeriodRollbackDate = AppUtils.ServerDateTime();
            payPeriod.PayPeriodRollbackBy = WebUtils.GetCurUser(Session).UserID;
            EPayrollPeriod.db.update(dbConn, payPeriod);

            errors.addError("Undo Successful");
        }
        loadData(info, EEmpPayroll.db, Repeater);
    }

    // Start 0000185, KuangWei, 2015-04-21
    protected void Search_Click(object sender, EventArgs e)
    {
        view = loadData(info, EEmpPayroll.db, Repeater);
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        EmployeeSearchControl1.Reset();
        EmployeeSearchControl1.EmpStatusValue = "A";
        view = loadData(info, EEmpPayroll.db, Repeater);
    }
    // End 0000185, KuangWei, 2015-04-21        
}
