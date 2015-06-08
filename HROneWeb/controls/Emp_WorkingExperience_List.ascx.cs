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

public partial class Emp_WorkingExperience_List : HROneWebControl
{
    private const string FUNCTION_CODE = "PER016";
    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = EEmpWorkExp.db;
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

        

        PreRender += new EventHandler(Emp_WorkingExperience_List_PreRender);


        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.initValues("EmpWorkExpFromMonth", null, Values.VLMonth, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("EmpWorkExpToMonth", null, Values.VLMonth, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
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

    void Emp_WorkingExperience_List_PreRender(object sender, EventArgs e)
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
        {
            if (info.orderby.Equals("EmpWorkExpFrom"))
            {
                filter.add("EmpWorkExpFromYear", info.order);
                filter.add("EmpWorkExpFromMonth", info.order);
            }
            else if (info.orderby.Equals("EmpWorkExpTo"))
            {
                filter.add("EmpWorkExpToYear", info.order);
                filter.add("EmpWorkExpToMonth", info.order);
            }
            else
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

        if (id.Equals("EmpWorkExpFromYear"))
            id = "EmpWorkExpFromYear, EmpWorkExpFromMonth";
        if (id.Equals("EmpWorkExpToYear"))
            id = "EmpWorkExpToYear, EmpWorkExpToMonth";

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

        EEmpWorkExp obj = new EEmpWorkExp();
        EEmpWorkExp.db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        EEmpWorkExp.db.populate(obj, values);

        Binding eBinding;
        eBinding = new Binding(dbConn, EEmpWorkExp.db);
        eBinding.add(new BlankZeroLabelVLBinder(EEmpWorkExp.db, (Label)e.Item.FindControl("EmpWorkExpFromMonth"), Values.VLMonth));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpWorkExp.db, (Label)e.Item.FindControl("EmpWorkExpFromYear"), new WFTextList(new string[]{}) ));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpWorkExp.db, (Label)e.Item.FindControl("EmpWorkExpToMonth"), Values.VLMonth));
        eBinding.add(new BlankZeroLabelVLBinder(EEmpWorkExp.db, (Label)e.Item.FindControl("EmpWorkExpToYear"), new WFTextList(new string[] { })));



        eBinding.init(Request, Session);

        eBinding.toControl(values);
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
                EEmpWorkExp o = new EEmpWorkExp();
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
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_WorkingExperience_Edit.aspx?EmpID=" + EmpID.Value);
    }
}
