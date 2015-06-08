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
using HROne.Translation;

public partial class Emp_Beneficiaries_List : HROneWebControl
{
    private const string FUNCTION_CODE = "PER021";
    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = EEmpBeneficiaries.db;
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
                
        PreRender += new EventHandler(Emp_Beneficiaries_List_PreRender);
        
        sbinding = new SearchBinding(dbConn, sdb);
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

    void Emp_Beneficiaries_List_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            view = loadData(info, sdb, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("EmpID", this.CurID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
            filter.add(info.orderby, info.order);
        }
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
                EEmpBeneficiaries o = new EEmpBeneficiaries();
                WebFormUtils.GetKeys(sdb, o, cb);
                list.Add(o);
            }

        }
        if (list.Count > 0)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, CurID);
            foreach (BaseObject o in list)
            {
                if (sdb.select(dbConn, o))
                    sdb.delete(dbConn, o);
            }
            WebUtils.EndFunction(dbConn);
        }
        loadData(info, sdb, Repeater);
    }

    protected void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Beneficiaries_Edit.aspx?EmpID=" + EmpID.Value);
    }
}
