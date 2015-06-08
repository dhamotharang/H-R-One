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

public partial class Leave_ALProrataRoundingRule_View : HROneWebPage
{
    private const string FUNCTION_CODE = "LEV004";
    
    public Binding binding;
    public DBManager db = EALProrataRoundingRule.db;
    public EALProrataRoundingRule obj;

    // add
    protected DBManager ALProrataRoundingRuleDetaildb = EALProrataRoundingRuleDetail.db;
    protected ListInfo info;
    protected DataView view;
    protected SearchBinding sbinding;
    public Binding ALProrataRoundingRuleDetailbinding;
    public Binding ALProrataRoundingRuleDetailebinding;
    public EALProrataRoundingRuleDetail ALProrataRoundingRuleDetailobj;

    

    //by Ben


    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(ALProrataRoundingRuleID);
        binding.add(ALProrataRoundingRuleCode);
        binding.add(ALProrataRoundingRuleDesc);

        //Add
        ALProrataRoundingRuleDetailbinding = new Binding(dbConn, ALProrataRoundingRuleDetaildb);
        ALProrataRoundingRuleDetailbinding.add(ALProrataRoundingRuleID);
        ALProrataRoundingRuleDetailbinding.add(ALProrataRoundingRuleDetailRangeTo);
        ALProrataRoundingRuleDetailbinding.add(ALProrataRoundingRuleDetailRoundTo);
        ALProrataRoundingRuleDetailbinding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, ALProrataRoundingRuleDetaildb);
        sbinding.add(new HiddenMatchBinder(ALProrataRoundingRuleID));
        sbinding.init(DecryptedRequest, null);

        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        info = ListFooter.ListInfo;

        if (!int.TryParse(DecryptedRequest["ALProrataRoundingRuleID"], out CurID))
            CurID = -1;

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) 
		{

            view = loadData(info, ALProrataRoundingRuleDetaildb, Repeater); //add by Ben

            if (CurID > 0)
            {
                loadObject();
                AddPanel.Visible = toolBar.DeleteButton_Visible;
                Delete.Visible = toolBar.DeleteButton_Visible;
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected bool loadObject()
    {
        obj = new EALProrataRoundingRule();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;


        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager AVCPlanDetaildb, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
           filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + AVCPlanDetaildb.dbclass.tableName + " c ";

           DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    //protected void ChangeOrder_Click(object sender, EventArgs e)
    //{
    //    LinkButton l = (LinkButton)sender;
    //    String id = l.ID.Substring(1);
    //    if (info.orderby == null)
    //        info.order = true;
    //    else if (info.orderby.Equals(id))
    //        info.order = !info.order;
    //    else
    //        info.order = true;
    //    info.orderby = id;

    //    Repeater.EditItemIndex = -1;
    //    view = loadData(info, AVCPlanDetaildb, Repeater);

    //    Response.Redirect(Request.Url.LocalPath + "?AVCPlanID=" + CurID);

    //}
    protected void Add_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(ALProrataRoundingRuleDetaildb, Page);
        errors.clear();

        Repeater.EditItemIndex = -1;
        EALProrataRoundingRuleDetail c = new EALProrataRoundingRuleDetail();

        Hashtable values = new Hashtable();
        ALProrataRoundingRuleDetailbinding.toValues(values);
        ALProrataRoundingRuleDetaildb.validate(errors, values);

        if (!errors.isEmpty())
            return;


        ALProrataRoundingRuleDetaildb.parse(values, c);

        //if (!AppUtils.checkDuplicate(dbConn, AVCPlanDetaildb, c, errors, "AVCPlanDetailYearOfService"))
        //    return;

        if (c.ALProrataRoundingRuleDetailRangeTo > 1 || c.ALProrataRoundingRuleDetailRangeTo < 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_NUMBR_RANGE, new string[] { "0", "1" }));
            ALProrataRoundingRuleDetailRangeTo.Focus();
            return;
        }

        if (c.ALProrataRoundingRuleDetailRoundTo > 1 || c.ALProrataRoundingRuleDetailRoundTo < 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_NUMBR_RANGE, new string[] { "0", "1" }));
            ALProrataRoundingRuleDetailRoundTo.Focus();
            return;
        }

        AND andALProrataRoundingRuleDetailTerm = new AND();
        andALProrataRoundingRuleDetailTerm.add(new Match("ALProrataRoundingRuleID", c.ALProrataRoundingRuleID));
        AppUtils.checkDuplicate(dbConn, ALProrataRoundingRuleDetaildb, c, errors, "ALProrataRoundingRuleDetailRangeTo", andALProrataRoundingRuleDetailTerm);

        if (!errors.isEmpty())
            return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        ALProrataRoundingRuleDetaildb.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        ALProrataRoundingRuleDetailRangeTo.Text = string.Empty;
        ALProrataRoundingRuleDetailRoundTo.Text = string.Empty;

        view = loadData(info, ALProrataRoundingRuleDetaildb, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {


        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ALProrataRoundingRuleDetailebinding = new Binding(dbConn, ALProrataRoundingRuleDetaildb);
            ALProrataRoundingRuleDetailebinding.add((HtmlInputHidden)e.Item.FindControl("ALProrataRoundingRuleDetailID"));
            ALProrataRoundingRuleDetailebinding.add((TextBox)e.Item.FindControl("ALProrataRoundingRuleDetailRangeTo"));
            ALProrataRoundingRuleDetailebinding.add((TextBox)e.Item.FindControl("ALProrataRoundingRuleDetailRoundTo"));
            ALProrataRoundingRuleDetailebinding.init(Request, Session);


            EALProrataRoundingRuleDetail obj = new EALProrataRoundingRuleDetail();
            ALProrataRoundingRuleDetaildb.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            ALProrataRoundingRuleDetaildb.populate(obj, values);
            ALProrataRoundingRuleDetailebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = toolBar.DeleteButton_Visible;
            e.Item.FindControl("DeleteItem").Visible = toolBar.DeleteButton_Visible;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("ALProrataRoundingRuleDetailID");
            h.Value = ((DataRowView)e.Item.DataItem)["ALProrataRoundingRuleDetailID"].ToString();

        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, ALProrataRoundingRuleDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, ALProrataRoundingRuleDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }
        else if (b.ID.Equals("Save"))
        {
            ALProrataRoundingRuleDetailebinding = new Binding(dbConn, ALProrataRoundingRuleDetaildb);
            ALProrataRoundingRuleDetailebinding.add((HtmlInputHidden)e.Item.FindControl("ALProrataRoundingRuleDetailID"));
            ALProrataRoundingRuleDetailebinding.add(ALProrataRoundingRuleID);
            ALProrataRoundingRuleDetailebinding.add((TextBox)e.Item.FindControl("ALProrataRoundingRuleDetailRangeTo"));
            ALProrataRoundingRuleDetailebinding.add((TextBox)e.Item.FindControl("ALProrataRoundingRuleDetailRoundTo"));
            ALProrataRoundingRuleDetailebinding.init(Request, Session);


            EALProrataRoundingRuleDetail obj = new EALProrataRoundingRuleDetail();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(ALProrataRoundingRuleDetaildb, Page);
            errors.clear();


            ALProrataRoundingRuleDetailebinding.toValues(values);

            ALProrataRoundingRuleDetaildb.validate(errors, values);

            if (!errors.isEmpty())
                return;



            ALProrataRoundingRuleDetaildb.parse(values, obj);
            //if (!AppUtils.checkDuplicate(dbConn, AVCPlanDetaildb, obj, errors, "AVCPlanID"))
                //return;


            if (obj.ALProrataRoundingRuleDetailRangeTo > 1 || obj.ALProrataRoundingRuleDetailRangeTo < 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_NUMBR_RANGE, new string[] { "0", "1" }));
                e.Item.FindControl("ALProrataRoundingRuleDetailRangeTo").Focus();
                return;
            }

            if (obj.ALProrataRoundingRuleDetailRoundTo > 1 || obj.ALProrataRoundingRuleDetailRoundTo < 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_NUMBR_RANGE, new string[] { "0", "1" }));
                e.Item.FindControl("ALProrataRoundingRuleDetailRoundTo").Focus();
                return;
            }

            if (!errors.isEmpty())
                return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            ALProrataRoundingRuleDetaildb.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, ALProrataRoundingRuleDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel, true);
        }


    }



    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("ALProrataRoundingRuleDetailID");
            if (c.Checked)
            {
                EALProrataRoundingRuleDetail obj = new EALProrataRoundingRuleDetail();
                obj.ALProrataRoundingRuleDetailID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (EALProrataRoundingRuleDetail obj in list)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            if (ALProrataRoundingRuleDetaildb.select(dbConn, obj))
                ALProrataRoundingRuleDetaildb.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, ALProrataRoundingRuleDetaildb, Repeater);

    }
    protected void Delete_ClickTop(object sender, EventArgs e)
    {

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EALProrataRoundingRule o = new EALProrataRoundingRule();
        o.ALProrataRoundingRuleID = CurID;
        if (db.select(dbConn, o))
        {
            DBFilter leavePlanFilter = new DBFilter();
            leavePlanFilter.add(new Match("ALProrataRoundingRuleID", o.ALProrataRoundingRuleID));
            ArrayList leavePlanList = ELeavePlan.db.select(dbConn, leavePlanFilter);
            if (leavePlanList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { o.ALProrataRoundingRuleCode }));
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;
            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                DBFilter obj = new DBFilter();
                obj.add(new Match("ALProrataRoundingRuleID", o.ALProrataRoundingRuleID));
                ArrayList objList = EALProrataRoundingRuleDetail.db.select(dbConn, obj);
                foreach (EALProrataRoundingRuleDetail match in objList)
                    EALProrataRoundingRuleDetail.db.delete(dbConn, match);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Leave_ALProrataRoundingRule_List.aspx");
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Leave_ALProrataRoundingRule_Edit.aspx?ALProrataRoundingRuleID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Leave_ALProrataRoundingRule_List.aspx");
    }
}
