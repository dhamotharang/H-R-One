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

public partial class Emp_CostCenter_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER012";
    public Binding binding;
    public DBManager db = EEmpCostCenter.db;
    public EEmpCostCenter obj;
    public int CurID = -1;
    public int CurEmpID = -1;

    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(EmpCostCenterID);
        binding.add(EmpID);
        binding.add(EmpCostCenterEffFr);
        binding.add(EmpCostCenterEffTo);

        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, ECostCenter.db);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["EmpCostCenterID"], out CurID))
            CurID = -1;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;

        info = ListFooter.ListInfo;
        EmpID.Value = CurEmpID.ToString();
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                view = loadData(info, ECostCenter.db, Repeater);
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }


    protected bool loadObject() 
    {
	    obj=new EEmpCostCenter();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurEmpID));
        filter.add(WebUtils.AddRankFilter(Session, "EmpID", true));
        if (EEmpPersonalInfo.db.count(dbConn, filter) == 0)
            if (CurEmpID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (!db.select(dbConn, obj))
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

        if (obj.EmpID != CurEmpID)
            if (CurID <= 0)
                return false;
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);


        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " cc.* , eccd.*";
        string from = "from [" + db.dbclass.tableName + "] cc, " + EEmpCostCenterDetail.db.dbclass.tableName + " eccd ";

        filter.add(new MatchField("eccd.CostCenterID", "cc.CostCenterID"));
        filter.add(new Match("eccd.EmpCostCenterID", CurID));
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        ListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, ECostCenter.db, Repeater);
    }

    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, ECostCenter.db, Repeater);
    }

    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, ECostCenter.db, Repeater);
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

        view = loadData(info, ECostCenter.db, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(ECostCenter.db, row, cb);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        EEmpCostCenter c = new EEmpCostCenter();
        c.EmpCostCenterID = CurID;
        if (EEmpCostCenter.db.select(dbConn, c))
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE, c.EmpID);
            db.delete(dbConn, c);
            DBFilter costCenterDetailFilter = new DBFilter();
            costCenterDetailFilter.add(new Match("EmpCostCenterID", c.EmpCostCenterID));
            ArrayList empCostCenterDetailList = EEmpCostCenterDetail.db.select(dbConn, costCenterDetailFilter);
            foreach (EEmpCostCenterDetail empCostCenterDetail in empCostCenterDetailList)
                EEmpCostCenterDetail.db.delete(dbConn, empCostCenterDetail);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_CostCenter_View.aspx?EmpID=" + EmpID.Value);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_CostCenter_Edit.aspx?EmpCostCenterID=" + EmpCostCenterID.Value + "&EmpID=" + EmpID.Value);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_CostCenter_View.aspx?EmpID=" + EmpID.Value);
    }
}
