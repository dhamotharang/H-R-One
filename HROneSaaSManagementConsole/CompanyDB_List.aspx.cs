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
using System.Globalization;
using HROne.SaaS.Entities;

public partial class CompanyDB_List : HROneWebPage
{
    const string FUNCTION_CODE = "ADM002";

    protected DBManager db = ECompanyDatabase.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;

        info = ListFooter.ListInfo;



        binding = new SearchBinding(dbConn, db);
        binding.add(new LikeSearchBinder(CompanyDBClientCode, "CompanyDBClientCode"));
        binding.initValues("DBServerID", null, EDatabaseServer.VLDBServerList, null);
        binding.initValues("CompanyDBIsActive", null, HROne.Lib.Entities.Values.VLYesNo, null);
        binding.init(Request.QueryString, null);



    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();

        if (!string.IsNullOrEmpty(txtRemoteProfileID.Text))
        {
            DBFilter exchangeProfileFilter = new DBFilter();
            exchangeProfileFilter.add(new Match("HSBCExchangeProfileRemoteProfileID", " LIKE ", "%" + txtRemoteProfileID.Text + "%"));
            filter.add(new IN("CompanyDBID", "SELECT CompanyDBID FROM " + EHSBCExchangeProfile.db.dbclass.tableName, exchangeProfileFilter));
        }

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "*";
        string from = "from " + db.dbclass.tableName + "  ";//LEFT JOIN " + EEmpPositionInfo.db.dbclass.tableName + " p ON c.EmpID=p.EmpID AND p.EmpPosEffTo IS NULL";
        //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        DataTable table = filter.loadData(dbConn, null, select, from);
        table = WebUtils.DataTableSortingAndPaging(table, info);

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
        info.page = 0;
        view = loadData(info, db, Repeater);
    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        info.page = 0;

        view = loadData(info, db, Repeater);

    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page--;
        view = loadData(info, db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page++;
        view = loadData(info, db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //info.page = Int32.Parse(NumPage.Value);
        view = loadData(info, db, Repeater);

    }
    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        //loadState();
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
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");
        foreach (ECompanyDatabase o in list)
        {
            if (ECompanyDatabase.db.select(dbConn, o))
            {


                //DBFilter companyDBFilter = new DBFilter();
                //companyDBFilter.add(new Match("DBServerID", o.DBServerID));

                //if (ECompanyDatabase.db.count(dbConn, companyDBFilter) <= 0)
                //{
                    db.delete(dbConn, o);
                //}
                //else
                //{
                //    errors.addError(o.DBServerCode +" is in use.");
                //}
            }
        }
        //loadState();
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompanyDB_Edit.aspx");
    }


}
