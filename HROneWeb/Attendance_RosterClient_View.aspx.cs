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

public partial class Attendance_RosterClient_View : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT012";
    
    public Binding binding;
    public DBManager db = ERosterClient.db;
    public ERosterClient obj;

    // add
    protected DBManager RosterClientSitedb = ERosterClientSite.db;
    protected ListInfo info;
    protected DataView view;
    protected SearchBinding sbinding;
    public Binding RosterClientSitebinding;
    public Binding RosterClientSiteebinding;
    public ERosterClientSite RosterClientSiteobj;

    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        siteToolBar.FunctionCode = FUNCTION_CODE;
        

        binding = new Binding(dbConn, db);
        binding.add(RosterClientID);
        binding.add(RosterClientCode);
        binding.add(RosterClientName);
        binding.add(new BlankZeroLabelVLBinder(db, RosterClientMappingSiteCodeToHLevelID, EHierarchyLevel.VLHierarchy));
        binding.add(new BlankZeroLabelVLBinder(db, CostCenterID, ECostCenter.VLCostCenter).setTextDisplayForZero(HROne.Common.WebUtility.GetLocalizedString(ECostCenter.DEFAULT_COST_CENTER_TEXT)));

        sbinding = new SearchBinding(dbConn, RosterClientSitedb);
        sbinding.add(new HiddenMatchBinder(RosterClientID));
        sbinding.init(DecryptedRequest, null);


        // by Ben

        binding.init(Request, Session);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["RosterClientID"], out CurID))
            CurID = -1;

        CostCenterRow.Visible = WebUtils.productLicense(Session).IsCostCenter;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            view = loadData(info, RosterClientSitedb, Repeater); //add by Ben

            if (CurID > 0)
            {
                loadObject();
                siteToolBar.DeleteButton_Visible = toolBar.DeleteButton_Visible;
                siteToolBar.NewButton_Visible = toolBar.DeleteButton_Visible;
            }
            else
            {
                toolBar.DeleteButton_Visible = false;
                siteToolBar.Visible = false;
            }
        }
    }

    protected bool loadObject()
    {
        obj = new ERosterClient();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;



        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager RosterClientSitedb, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
           filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + RosterClientSitedb.dbclass.tableName + " c ";

           DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    //protected void Search_Click(object sender, EventArgs e)
    //{
    //    view = loadData(info, RosterClientSitedb, Repeater);

    //}
    //protected void Reset_Click(object sender, EventArgs e)
    //{
    //    sbinding.clear();
    //    view = loadData(info, RosterClientSitedb, Repeater);

    //}
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

        //Repeater.EditItemIndex = -1;
        view = loadData(info, RosterClientSitedb, Repeater);

        //Response.Redirect(Request.Url.LocalPath + "?RosterClientID=" + CurID);

    }
    //protected void Add_Click(object sender, EventArgs e)
    //{
    //}

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {


        {
            e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("RosterClientSiteID");
            h.Value = ((DataRowView)e.Item.DataItem)["RosterClientSiteID"].ToString();

        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //Button b = (Button)e.CommandSource;




        //if (b.ID.Equals("Edit"))
        //{
        //    Repeater.EditItemIndex = e.Item.ItemIndex;
        //    view = loadData(info, RosterClientSitedb, Repeater);
        //}
        //else if (b.ID.Equals("Cancel"))
        //{
        //    Repeater.EditItemIndex = -1;
        //    view = loadData(info, RosterClientSitedb, Repeater);
        //}
        //else if (b.ID.Equals("Save"))
        //{
        //    RosterClientSiteebinding = new Binding(RosterClientSitedb);
        //    RosterClientSiteebinding.add((HtmlInputHidden)e.Item.FindControl("RosterClientSiteID"));
        //   // RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientID"));              // not defined by ben
        //    RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientSiteYearOfService"));
        //    RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientSiteERBelowRI"));
        //    RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientSiteERAboveRI"));
        //    RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientSiteERFix"));
        //    RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientSiteEEBelowRI"));
        //    RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientSiteEEAboveRI"));
        //    RosterClientSiteebinding.add((TextBox)e.Item.FindControl("RosterClientSiteEEFix"));
        //    RosterClientSiteebinding.init(Request, Session);


        //    ERosterClientSite obj = new ERosterClientSite();
        //    Hashtable values = new Hashtable();

        //    PageErrors errors = PageErrors.getErrors(RosterClientSitedb, Page);
        //    errors.clear();


        //    RosterClientSiteebinding.toValues(values);
        //    RosterClientSitedb.validate(errors, values);

        //    if (!errors.isEmpty())
        //        return;

        //    RosterClientSitedb.parse(values, obj);
        //    //if (!AppUtils.checkDuplicate(dbConn, RosterClientSitedb, obj, errors, "RosterClientID"))
        //        //return;

        //    WebUtils.StartFunction(Session, FUNCTION_CODE);
        //    RosterClientSitedb.update(dbConn, obj);
        //    WebUtils.EndFunction(dbConn);

        //    Repeater.EditItemIndex = -1;
        //    Response.Redirect(Request.Url.LocalPath + "?RosterClientID=" + CurID);
        //}


    }



//    protected void Save_Click(object sender, EventArgs e)
//    {
//        ERosterClient c = new ERosterClient();

//        Hashtable values = new Hashtable();
//        binding.toValues(values);

//        PageErrors errors = PageErrors.getErrors(db, Page.Master);
//        errors.clear();


//        db.validate(errors, values);

//        if (!errors.isEmpty())
//            return;


//        db.parse(values, c);
//        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "RosterClientCode"))
//            return;

//        WebUtils.StartFunction(Session, FUNCTION_CODE);
//        if (CurID < 0)
//        {
////            Utils.MarkCreate(Session, c);

//            db.insert(dbConn, c);
//            CurID = c.RosterClientID;
////            url = Utils.BuildURL(-1, CurID);
//        }
//        else
//        {
////            Utils.Mark(Session, c);
//            db.update(dbConn, c);
//        }
//        WebUtils.EndFunction(dbConn);


//        Response.Redirect(Request.Url.LocalPath+"?RosterClientID="+CurID);


//    }

    protected void Delete_ClickTop(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ERosterClient o = new ERosterClient();
        o.RosterClientID = CurID;
        db.select(dbConn, o);
        DBFilter rosterCodeFilter = new DBFilter();
        rosterCodeFilter.add(new Match("RosterClientID", o.RosterClientID));
        ArrayList rosterClientSiteList = ERosterCode.db.select(dbConn, rosterCodeFilter);
        if (rosterClientSiteList.Count > 0)
        {
            errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_IS_IN_USE, new string[] { HROne.Common.WebUtility.GetLocalizedString("Roster Client Code"), o.RosterClientCode }));
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
            return;
        }
        else
        {
            DBFilter obj = new DBFilter();
            obj.add(new Match("RosterClientID", o.RosterClientID));
            ArrayList objList = ERosterClientSite.db.select(dbConn, obj);
            foreach (ERosterClientSite match in objList)
                ERosterClientSite.db.delete(dbConn, match);

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, o);
            WebUtils.EndFunction(dbConn);
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_List.aspx");

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        //ERosterClient c = new ERosterClient();
        //c.RosterClientID = CurID;
        //db.delete(dbConn, c);
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "RosterClient_List.aspx");
        ArrayList list = new ArrayList();
        foreach (RepeaterItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("ItemSelect");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("RosterClientSiteID");
            if (c.Checked)
            {
                ERosterClientSite obj = new ERosterClientSite();
                obj.RosterClientSiteID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        foreach (ERosterClientSite obj in list)
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            RosterClientSitedb.delete(dbConn, obj);
            WebUtils.EndFunction(dbConn);
        }
        view = loadData(info, RosterClientSitedb, Repeater);

    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClientSite_Edit.aspx?RosterClientID=" + CurID);
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_Edit.aspx?RosterClientID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_RosterClient_List.aspx");
    }
}
