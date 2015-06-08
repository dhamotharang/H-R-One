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
using HROne.Common;

public partial class eChannel_AuthorizedSignatureSetup_View : HROneWebPage
{
    private const string FUNCTION_CODE = "ECH003";

    protected SearchBinding binding;
    protected ListInfo info;
    private DataView view;
    protected ArrayList selectedItemList = new ArrayList();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new SearchBinding(dbConn, EUser.db);
        binding.initValues("UserAccountStatus", null, EUser.VLAccountStatus, null);
        binding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            loadObject();
            view = loadData(info, Repeater);

        }
    }

    protected bool loadObject()
    {
        chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE.Text= ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? "Yes" : "No";
        chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? "Yes" : "No";

        txtPARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED);

        return true;
    }

    public DataView loadData(ListInfo info, Repeater repeater)
    {
        if (IsPostBack)
            selectedItemList = WebUtils.SelectedRepeaterItemToBaseObjectList(EUser.db, Repeater, "ItemSelect");

        DBFilter filter = binding.createFilter();

        filter.add(new Match("UserAccountStatus", "<>", "D"));

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "usr.*";
        string from = "from " + EUser.db.dbclass.tableName + " usr, " + EeChannelAuthorizedSignature.db.dbclass.tableName + " eas ";

        filter.add(new MatchField("usr.UserID", "eas.UserID"));
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

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(EUser.db, row, cb);

        //EUser obj = new EUser();
        //EUser.db.toObject(row.Row, obj);

        //if (!Page.IsPostBack)
        //{
        //    DBFilter eChannelAuthorizedSignatureFilter = new DBFilter();
        //    eChannelAuthorizedSignatureFilter.add(new Match("UserID", row["UserID"]));
        //    if (EeChannelAuthorizedSignature.db.count(dbConn, eChannelAuthorizedSignatureFilter) > 0)
        //        cb.Checked = true;
        //}
        //else
        //{
        //    foreach (BaseObject selectedObj in selectedItemList)
        //    {
        //        bool isSelected = true;
        //        foreach (DBField keyObj in EUser.db.keys)
        //        {
        //            if (!keyObj.getValue(selectedObj).Equals(keyObj.getValue(obj)))
        //            {
        //                isSelected = false;
        //            }
        //        }
        //        if (isSelected)
        //        {
        //            cb.Checked = true;
        //            break;
        //        }
        //    }
        //}
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

        view = loadData(info, Repeater);

    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/eChannel_AuthorizedSignatureSetup_Edit.aspx");
    }

}
