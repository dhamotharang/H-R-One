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
    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = EEmpBeneficiaries.db;
    protected ListInfo info;
    protected DataView view;
    
    protected void Page_Load(object sender, EventArgs e)
    {
                
        PreRender += new EventHandler(Emp_Beneficiaries_List_PreRender);
        
        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.init(DecryptedRequest, null);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
        }

        EmpID.Value = CurID.ToString();

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

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
            filter.add(info.orderby, info.order);
        }

        string select = "c.*";
        string from = "from [" + sdb.dbclass.tableName + "] c ";

        DataTable table = filter.loadData(dbConn, info, select, from);

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

        view = loadData(info, sdb, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;

        EEmpBeneficiaries obj = new EEmpBeneficiaries();
        EEmpBeneficiaries.db.toObject(row.Row, obj);
    }

}
