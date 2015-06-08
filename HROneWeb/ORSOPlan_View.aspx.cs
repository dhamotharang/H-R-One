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

public partial class ORSOPlan_View : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF004";
    
    public Binding binding;
    public DBManager db = EORSOPlan.db;
    public EORSOPlan obj;

    // add
    protected DBManager ORSOPlanDetaildb = EORSOPlanDetail.db;
    protected ListInfo info;
    protected DataView view;
    protected SearchBinding sbinding;
    public Binding ORSOPlanDetailbinding;
    public Binding ORSOPlanDetailebinding;
    public EORSOPlanDetail ORSOPlanDetailobj;

    

    //by Ben


    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(ORSOPlanID);
        binding.add(ORSOPlanCode);
        binding.add(ORSOPlanDesc);
        binding.add(ORSOPlanSchemeNo);
        binding.add(ORSOPlanCompanyName);
        binding.add(ORSOPlanPayCenter);

        binding.add(ORSOPlanMaxEmployerVC);
        binding.add(ORSOPlanMaxEmployeeVC);

        //Add
        ORSOPlanDetailbinding = new Binding(dbConn, ORSOPlanDetaildb);
        ORSOPlanDetailbinding.add(ORSOPlanID);
        ORSOPlanDetailbinding.add(ORSOPlanDetailYearOfService);
        ORSOPlanDetailbinding.add(ORSOPlanDetailER);
        ORSOPlanDetailbinding.add(ORSOPlanDetailERFix);
        ORSOPlanDetailbinding.add(ORSOPlanDetailEE);
        ORSOPlanDetailbinding.add(ORSOPlanDetailEEFix);
        ORSOPlanDetailbinding.init(Request, Session);

        binding.add(new LabelVLBinder(db, ORSOPlanEmployerResidual, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, ORSOPlanEmployeeResidual, Values.VLTrueFalseYesNo));
        binding.add(ORSOPlanEmployerResidualCap);
        binding.add(ORSOPlanEmployeeResidualCap);

        binding.add(new LabelVLBinder(db, ORSOPlanEmployerRoundingRule, Values.VLRoundingRule));
        binding.add(ORSOPlanEmployerDecimalPlace);
        binding.add(new LabelVLBinder(db, ORSOPlanEmployeeRoundingRule, Values.VLRoundingRule));
        binding.add(ORSOPlanEmployeeDecimalPlace);

        sbinding = new SearchBinding(dbConn, ORSOPlanDetaildb);
        sbinding.add(new HiddenMatchBinder(ORSOPlanID));
        sbinding.init(DecryptedRequest, null);

        binding.init(Request, Session);

        if (!int.TryParse(DecryptedRequest["ORSOPlanID"], out CurID))
            CurID = -1;

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

		if(!Page.IsPostBack) 
		{

            view = loadData(info, ORSOPlanDetaildb, Repeater); //add by Ben

            if (CurID > 0)
            {
                loadObject();
                AddPanel1.Visible = toolBar.DeleteButton_Visible;
                AddPanel2.Visible = toolBar.DeleteButton_Visible;
                Delete.Visible = toolBar.DeleteButton_Visible;
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }

    protected bool loadObject()
    {
        obj = new EORSOPlan();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        if (string.IsNullOrEmpty(obj.ORSOPlanEmployerRoundingRule) && string.IsNullOrEmpty(obj.ORSOPlanEmployeeRoundingRule))
        {
            obj.ORSOPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.ORSOPlanEmployerDecimalPlace = 2;
            obj.ORSOPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.ORSOPlanEmployeeDecimalPlace = 2;
        }

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadData(ListInfo info, DBManager ORSOPlanDetaildb, DataList repeater)
    {
        DBFilter filter = sbinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
           filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from " + ORSOPlanDetaildb.dbclass.tableName + " c ";

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
    //    view = loadData(info, ORSOPlanDetaildb, Repeater);

    //    Response.Redirect(Request.Url.LocalPath + "?ORSOPlanID=" + CurID);

    //}
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        EORSOPlanDetail c = new EORSOPlanDetail();

        Hashtable values = new Hashtable();
        ORSOPlanDetailbinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(ORSOPlanDetaildb, Page);
        errors.clear();


        ORSOPlanDetaildb.validate(errors, values);

        if (!errors.isEmpty())
            return;


        ORSOPlanDetaildb.parse(values, c);

        //if (!AppUtils.checkDuplicate(dbConn, ORSOPlanDetaildb, c, errors, "ORSOPlanDetailYearOfService"))
        //    return;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        ORSOPlanDetaildb.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        ORSOPlanDetailEE.Text = string.Empty;
        ORSOPlanDetailEEFix.Text = string.Empty;
        ORSOPlanDetailER.Text = string.Empty;
        ORSOPlanDetailERFix.Text = string.Empty;
        ORSOPlanDetailYearOfService.Text = string.Empty;

        view = loadData(info, ORSOPlanDetaildb, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ORSOPlanDetailebinding = new Binding(dbConn, ORSOPlanDetaildb);
            ORSOPlanDetailebinding.add((HtmlInputHidden)e.Item.FindControl("ORSOPlanDetailID"));
           // ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanID"));              // not defined by ben
            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailYearOfService"));
            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailER"));

            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailERFix"));
            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailEE"));

            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailEEFix"));
            ORSOPlanDetailebinding.init(Request, Session);


            EORSOPlanDetail obj = new EORSOPlanDetail();
            ORSOPlanDetaildb.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            ORSOPlanDetaildb.populate(obj, values);
            ORSOPlanDetailebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = toolBar.DeleteButton_Visible;
            e.Item.FindControl("DeleteItem").Visible = toolBar.DeleteButton_Visible;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("ORSOPlanDetailID");
            h.Value = ((DataRowView)e.Item.DataItem)["ORSOPlanDetailID"].ToString();

        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, ORSOPlanDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel1, false);
            WebUtils.SetEnabledControlSection(AddPanel2, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, ORSOPlanDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel1, true);
            WebUtils.SetEnabledControlSection(AddPanel2, true);
        }
        else if (b.ID.Equals("Save"))
        {
            ORSOPlanDetailebinding = new Binding(dbConn, ORSOPlanDetaildb);
            ORSOPlanDetailebinding.add((HtmlInputHidden)e.Item.FindControl("ORSOPlanDetailID"));
           // ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanID"));              // not defined by ben
            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailYearOfService"));
            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailER"));

            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailERFix"));
            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailEE"));

            ORSOPlanDetailebinding.add((TextBox)e.Item.FindControl("ORSOPlanDetailEEFix"));
            ORSOPlanDetailebinding.init(Request, Session);


            EORSOPlanDetail obj = new EORSOPlanDetail();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(ORSOPlanDetaildb, Page);
            errors.clear();


            ORSOPlanDetailebinding.toValues(values);
            ORSOPlanDetaildb.validate(errors, values);

            if (!errors.isEmpty())
                return;

            ORSOPlanDetaildb.parse(values, obj);
            //if (!AppUtils.checkDuplicate(dbConn, ORSOPlanDetaildb, obj, errors, "ORSOPlanID"))
                //return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            ORSOPlanDetaildb.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, ORSOPlanDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel1, true);
            WebUtils.SetEnabledControlSection(AddPanel2, true);
        }


    }



//    protected void Save_Click(object sender, EventArgs e)
//    {
//        EORSOPlan c = new EORSOPlan();

//        Hashtable values = new Hashtable();
//        binding.toValues(values);

//        PageErrors errors = PageErrors.getErrors(db, Page.Master);
//        errors.clear();


//        db.validate(errors, values);

//        if (!errors.isEmpty())
//            return;


//        db.parse(values, c);
//        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "ORSOPlanCode"))
//            return;

//        WebUtils.StartFunction(Session, FUNCTION_CODE);
//        if (CurID < 0)
//        {
////            Utils.MarkCreate(Session, c);

//            db.insert(dbConn, c);
//            CurID = c.ORSOPlanID;
////            url = Utils.BuildURL(-1, CurID);
//        }
//        else
//        {
////            Utils.Mark(Session, c);
//            db.update(dbConn, c);
//        }
//        WebUtils.EndFunction(dbConn);


//        Response.Redirect(Request.Url.LocalPath+"?ORSOPlanID="+CurID);


//    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("ORSOPlanDetailID");
            if (c.Checked)
            {
                EORSOPlanDetail obj = new EORSOPlanDetail();
                obj.ORSOPlanDetailID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        foreach (EORSOPlanDetail obj in list)
            if (ORSOPlanDetaildb.select(dbConn, obj))
                ORSOPlanDetaildb.delete(dbConn, obj);
        WebUtils.EndFunction(dbConn);
        view = loadData(info, ORSOPlanDetaildb, Repeater);
    }
    protected void Delete_ClickTop(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EORSOPlan o = new EORSOPlan();
        o.ORSOPlanID = CurID;
        if (db.select(dbConn, o))
        {
            DBFilter empORSOFilter = new DBFilter();
            empORSOFilter.add(new Match("ORSOPlanID", o.ORSOPlanID));
            empORSOFilter.add("empid", true);
            ArrayList empORSOList = EEmpORSOPlan.db.select(dbConn, empORSOFilter);
            if (empORSOList.Count > 0)
            {
                int curEmpID = 0;
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("P-Fund Plan Code"), o.ORSOPlanCode }));
                foreach (EEmpORSOPlan empORSOPlan in empORSOList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empORSOPlan.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        if (curEmpID != empORSOPlan.EmpID)
                        {
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                            curEmpID = empORSOPlan.EmpID;
                        }
                        else
                            EEmpORSOPlan.db.delete(dbConn, empORSOPlan);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;
            }
            else
            {

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                DBFilter obj = new DBFilter();
                obj.add(new Match("ORSOPlanID", o.ORSOPlanID));
                ArrayList objList = EORSOPlanDetail.db.select(dbConn, obj);
                foreach (EORSOPlanDetail match in objList)
                    EORSOPlanDetail.db.delete(dbConn, match);
                WebUtils.EndFunction(dbConn);

            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_List.aspx");
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_Edit.aspx?ORSOPlanID=" + CurID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "ORSOPlan_List.aspx");
    }
}
