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

public partial class eChannel_AuthorizedSignatureSetup_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "ECH003";

    protected SearchBinding binding;
    protected ListInfo info;
    private DataView view;
    protected ArrayList selectedItemList = new ArrayList();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
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
        chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
        chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;

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

        string select = "c.*";
        string from = "from [" + EUser.db.dbclass.tableName + "] c ";

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

        EUser obj = new EUser();
        EUser.db.toObject(row.Row, obj);

        if (!Page.IsPostBack)
        {
            DBFilter eChannelAuthorizedSignatureFilter = new DBFilter();
            eChannelAuthorizedSignatureFilter.add(new Match("UserID", row["UserID"]));
            if (EeChannelAuthorizedSignature.db.count(dbConn, eChannelAuthorizedSignatureFilter) > 0)
                cb.Checked = true;
        }
        else
        {
            foreach (BaseObject selectedObj in selectedItemList)
            {
                bool isSelected = true;
                foreach (DBField keyObj in EUser.db.keys)
                {
                    if (!keyObj.getValue(selectedObj).Equals(keyObj.getValue(obj)))
                    {
                        isSelected = false;
                    }
                }
                if (isSelected)
                {
                    cb.Checked = true;
                    break;
                }
            }
        }
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

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/eChannel_AuthorizedSignatureSetup_View.aspx");
    }

    protected void Save_Click(object sender, EventArgs e)
    {


        PageErrors errors = PageErrors.getErrors(ESystemParameter.db, Page.Master);
        int testInteger;
        if (!txtPARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED.Text.Equals(string.Empty) && !int.TryParse(txtPARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED.Text, out testInteger))
            errors.addError(WebUtility.GetLocalizedStringByCode("validate.int.prompt", "").Replace("{0}", lblPARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED.Text));

        if (errors.isEmpty())
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE, chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE, chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED, txtPARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED.Text);

            ArrayList selectedList = new ArrayList();
            ArrayList unselectedList = new ArrayList();
            foreach (RepeaterItem i in Repeater.Items)
            {
                CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
                BaseObject o = new EUser();
                WebFormUtils.GetKeys(EUser.db, o, cb);
                if (cb.Checked)
                    selectedList.Add(o);
                else
                    unselectedList.Add(o);

            }
            foreach (EUser o in selectedList)
            {
                DBFilter authorizedSignatureFilter = new DBFilter();
                authorizedSignatureFilter.add(new Match("UserID", o.UserID));
                ArrayList authorizedSignatureList = EeChannelAuthorizedSignature.db.select(dbConn, authorizedSignatureFilter);
                if (authorizedSignatureList.Count == 0)
                {
                    EeChannelAuthorizedSignature authorizedSignature = new EeChannelAuthorizedSignature();
                    authorizedSignature.UserID = o.UserID;
                    EeChannelAuthorizedSignature.db.insert(dbConn, authorizedSignature);
                }
            }

            foreach (EUser o in unselectedList)
            {
                DBFilter authorizedSignatureFilter = new DBFilter();
                authorizedSignatureFilter.add(new Match("UserID", o.UserID));
                ArrayList authorizedSignatureList = EeChannelAuthorizedSignature.db.select(dbConn, authorizedSignatureFilter);
                if (authorizedSignatureList.Count != 0)
                {
                    foreach (EeChannelAuthorizedSignature authorizedSignature in authorizedSignatureList)
                        EeChannelAuthorizedSignature.db.delete(dbConn, authorizedSignature);
                }
            }

            WebUtils.EndFunction(dbConn);
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/eChannel_AuthorizedSignatureSetup_View.aspx");

        }

    }

}
