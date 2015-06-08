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
using HROne.Translation;

public partial class Emp_BankAccount_List : HROneWebControl
{
    private const string FUNCTION_CODE = "PER002";
    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = EEmpBankAccount.db;
    protected ListInfo info;
    protected DataView view;
    

    private bool m_IsAllowEdit=true;

    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            AllowEditPanel.Visible = false;
            m_IsAllowEdit = false;
        }
        AllowEditPanel.Visible = m_IsAllowEdit;

        

        PreRender += new EventHandler(Emp_BankAccount_List_PreRender);


        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.initValues("EmpAccDefault", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.init(DecryptedRequest, null);


        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        info = ListFooter.ListInfo;
    }

    public bool IsAllowEdit
    {
        get { return m_IsAllowEdit; }
        set
        {
            m_IsAllowEdit = value;
            AllowEditPanel.Visible = m_IsAllowEdit;
        }
    }

    void Emp_BankAccount_List_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, sdb, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", this.CurID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + sdb.dbclass.tableName + "] c ";

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

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, sdb, Repeater);

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

        view = loadData(info, sdb, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(sdb, row, cb);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(sdb, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                EEmpBankAccount o = new EEmpBankAccount();
                WebFormUtils.GetKeys(sdb, o, cb);
                list.Add(o);
            }

        }
        foreach (EEmpBankAccount obj in list)
        {
            sdb.select(dbConn, obj);
            DBFilter paymentRecordFilter = new DBFilter();
            paymentRecordFilter.add(new Match("EmpAccID", obj.EmpBankAccountID));

            IN inTerms = new IN("EmpPayrollID", "Select EmpPayrollID From " + EPaymentRecord.db.dbclass.tableName, paymentRecordFilter);

            DBFilter empPayrollFilter = new DBFilter();
            empPayrollFilter.add(inTerms);
            empPayrollFilter.add("empid", true);
            ArrayList empPayrollList = EEmpPayroll.db.select(dbConn, empPayrollFilter);
            if (empPayrollList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Bank Account") }));
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, obj.EmpID);
                sdb.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        loadData(info, sdb, Repeater);
    }

    protected void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_BankAccount_Edit.aspx?EmpID=" + EmpID.Value);
    }
}
