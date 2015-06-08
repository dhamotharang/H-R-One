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
using HROne.SaaS.Entities;

public partial class CompanyAutopayFile_List : HROneWebControl
{
    private bool IsAllowEdit = true;

    public int CurID = -1;

    protected SearchBinding sbinding;
    public DBManager sdb = ECompanyAutopayFile.db;
    protected ListInfo info;
    protected DataView view;
    
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!int.TryParse(DecryptedRequest["CompanyDBID"], out CurID))
            CurID = -1;
        CompanyDBID.Value = CurID.ToString();


        sbinding = new SearchBinding(dbConn, sdb);
        sbinding.initValues("HSBCExchangeProfileID", null, EHSBCExchangeProfile.VLRemoteProfileList, ci);
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        if (view == null)
        {
            //loadState();
            view = loadData(info, sdb, Repeater);
        }
    }

    //public void loadState()
    //{
    //    info = new ListInfo();
    //    int page = 0;
    //    if (!CurPage.Value.Equals(""))
    //        page = Int32.Parse(CurPage.Value);
    //    info.loadState(Request, page);
    //    info.order = !Order.Value.Equals("false", StringComparison.CurrentCultureIgnoreCase);
    //    info.orderby = OrderBy.Value;
    //    if (string.IsNullOrEmpty(info.orderby))
    //        info.orderby = "LeaveAppDateFrom";
    //}
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();
        filter.add(new Match("CompanyDBID", this.CurID));

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "*";
        string from = "from " + sdb.dbclass.tableName + " ";

        DataTable table = filter.loadData(dbConn, info, select, from);

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
        //loadState();
        //info.page = 0;
        view = loadData(info, sdb, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page--;
        view = loadData(info, sdb, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        //loadState();
        //info.page++;
        view = loadData(info, sdb, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        //loadState();

        //info.page = Int32.Parse(NumPage.Value);
        view = loadData(info, sdb, Repeater);

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

        view = loadData(info, sdb, Repeater);

    }
    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(sdb, row, cb);

        ECompanyAutopayFile obj = new ECompanyAutopayFile();
        sdb.toObject(row.Row, obj);

            cb.Visible = false;
    }

}
