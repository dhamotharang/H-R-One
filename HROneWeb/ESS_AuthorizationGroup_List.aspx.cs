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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class ESS_AuthorizationGroup_List : HROneWebPage
{
    private const string FUNCTION_CODE = "SEC003";

    protected DBManager db = EAuthorizationGroup.db;
    protected SearchBinding sbinding;
    protected ListInfo info;
    protected DataView view;
    //public Binding binding;
    public Binding ebinding;
    public EAuthorizationGroup obj;
    public int CurID = -1;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            //AddPanel.Visible = false;
            IsAllowEdit = false;
        }



        //binding = new Binding(dbConn, db);
        //binding.add(AuthorizationCode);
        //binding.add(AuthorizationDesc);
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsApproveEEInfo));
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsApproveLeave));
        //binding.add(new CheckBoxBinder(db, AuthorizationGroupIsReceiveOtherGrpAlert));
        //binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);
        sbinding.add(new LikeSearchBinder(AuthorizationGroupCode, "AuthorizationCode"));
        sbinding.add(new LikeSearchBinder(AuthorizationGroupDesc, "AuthorizationDesc"));
        sbinding.init(DecryptedRequest, null);
        sbinding.initValues("AuthorizationGroupIsApproveEEInfo", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("AuthorizationGroupIsApproveLeave", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("AuthorizationGroupIsReceiveOtherGrpAlert", null, Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + db.dbclass.tableName + " c ";

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);

    }
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        sbinding.clear();
        info.page = 0;
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

        Repeater.EditItemIndex = -1;
        view = loadData(info, db, Repeater);

    }
    protected void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ESS_AuthorizationGroup_Edit.aspx");
    }


    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        e.Item.FindControl("DeleteItem").Visible = IsAllowEdit;
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("AuthorizationGroupID"));
            ebinding.add((TextBox)e.Item.FindControl("AuthorizationCode"));
            ebinding.add((TextBox)e.Item.FindControl("AuthorizationDesc"));
            ebinding.add(new CheckBoxBinder(db, ((CheckBox)e.Item.FindControl("AuthorizationGroupIsApproveEEInfo"))));
            ebinding.add(new CheckBoxBinder(db, ((CheckBox)e.Item.FindControl("AuthorizationGroupIsApproveLeave"))));
            ebinding.add(new CheckBoxBinder(db, ((CheckBox)e.Item.FindControl("AuthorizationGroupIsReceiveOtherGrpAlert"))));
            ebinding.init(Request, Session);


            EAuthorizationGroup obj = new EAuthorizationGroup();
            db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            db.populate(obj, values);
            ebinding.toControl(values);
        }
        else
        {
            //e.Item.FindControl("Edit").Visible = IsAllowEdit;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("AuthorizationGroupID");
            h.Value=((DataRowView)e.Item.DataItem)["AuthorizationGroupID"].ToString();
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }


    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = new ArrayList();
        foreach(DataListItem item in Repeater.Items) 
        {
            CheckBox c=(CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("AuthorizationGroupID");
            if (c.Checked)
            {
                EAuthorizationGroup obj = new EAuthorizationGroup();
                obj.AuthorizationGroupID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (EAuthorizationGroup obj in list)
        {
            db.select(dbConn, obj); 
            DBFilter workflowDetailFilter = new DBFilter();
            workflowDetailFilter.add(new Match("AuthorizationGroupID", obj.AuthorizationGroupID));

            DBFilter workflowFilter = new DBFilter();
            workflowFilter.add(new IN("AuthorizationWorkFlowID", "SELECT AuthorizationWorkFlowID FROM " + EAuthorizationWorkFlowDetail.db.dbclass.tableName, workflowDetailFilter));
            ArrayList workflowList = EAuthorizationWorkFlow.db.select(dbConn, workflowFilter);
            if (workflowList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Authorization Group") + ":" + obj.AuthorizationCode }));
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                view = loadData(info, db, Repeater);
                return;

            }
            else
            {
                DBFilter authorizerFilter = new DBFilter();
                authorizerFilter.add(new Match("AuthorizationGroupID", obj.AuthorizationGroupID));
                EAuthorizer.db.delete(dbConn, authorizerFilter);

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, obj);
                WebUtils.EndFunction(dbConn);
            }
        }
        view = loadData(info, db, Repeater);
    }
}
