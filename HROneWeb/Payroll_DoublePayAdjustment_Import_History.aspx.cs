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
using System.Data.OleDb;
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class Payroll_DoublePayAdjustment_Import_History : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY021";

    private DBManager db = EDoublePayAdjustmentImportBatch.db;
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        sbinding = new SearchBinding(dbConn, db);

        sbinding.init(DecryptedRequest, null);
        sbinding.initValues("DoublePayAdjustImportBatchUploadedBy", new DBFilter(), EUser.VLUserName, null);
        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            loadData(info, db, Repeater);
        }
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
        {
        }
        else
        {
            info.orderby = "DoublePayAdjustImportBatchDateTime";
            info.order = false;
        }

        string select = "c.* ";
        string from = "from " + db.dbclass.tableName + " c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);
        DataView view = new DataView(table);
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

        Repeater.EditItemIndex = -1;
        loadData(info, db, Repeater);

    }

    protected void ChangePage(object sender, EventArgs e)
    {
        loadData(ListFooter.ListInfo, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("DoublePayAdjustImportBatchID");
        h.Value = ((DataRowView)e.Item.DataItem)["DoublePayAdjustImportBatchID"].ToString();
    }

    protected void Import_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Payroll_DoublePayAdjustment_Import.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Payroll_DoublePayAdjustment_List.aspx");
    }

}
