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

public partial class AVCPlan_View : HROneWebPage
{
    private const string FUNCTION_CODE = "MPF003";
    
    public Binding binding;
    public DBManager db = EAVCPlan.db;
    public EAVCPlan obj;

    // add
    protected DBManager AVCPlanDetaildb = EAVCPlanDetail.db;
    protected ListInfo info;
    protected DataView view;
    protected SearchBinding sbinding;
    public Binding AVCPlanDetailbinding;
    public Binding AVCPlanDetailebinding;
    public EAVCPlanDetail AVCPlanDetailobj;

    protected SearchBinding ceilingbinding;
    protected ListInfo avcCeilingInfo;
    protected DataView avcCeilingView;
    

    //by Ben


    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(AVCPlanID);
        binding.add(AVCPlanCode);
        binding.add(AVCPlanDesc);
        binding.add(new LabelVLBinder(db, AVCPlanEmployerResidual, Values.VLTrueFalseYesNo));
        binding.add(AVCPlanEmployerResidualCap);
        binding.add(new LabelVLBinder(db, AVCPlanEmployeeResidual, Values.VLTrueFalseYesNo));
        binding.add(AVCPlanEmployeeResidualCap);
        binding.add(new LabelVLBinder(db, AVCPlanUseMPFExemption, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AVCPlanJoinDateStart, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AVCPlanContributeMaxAge, Values.VLTrueFalseYesNo));
        binding.add(new LabelVLBinder(db, AVCPlanContributeMinRI, Values.VLTrueFalseYesNo));
        binding.add(AVCPlanMaxEmployerVC);
        binding.add(AVCPlanMaxEmployeeVC);
        binding.add(new LabelVLBinder(db, AVCPlanNotRemoveContributionFromTopUp, Values.VLTrueFalseYesNo));

        //Add
        AVCPlanDetailbinding = new Binding(dbConn, AVCPlanDetaildb);
        AVCPlanDetailbinding.add(AVCPlanID);
        AVCPlanDetailbinding.add(AVCPlanDetailYearOfService);
        AVCPlanDetailbinding.add(AVCPlanDetailERBelowRI);
        AVCPlanDetailbinding.add(AVCPlanDetailERAboveRI);
        AVCPlanDetailbinding.add(AVCPlanDetailERFix);
        AVCPlanDetailbinding.add(AVCPlanDetailEEBelowRI);
        AVCPlanDetailbinding.add(AVCPlanDetailEEAboveRI);
        AVCPlanDetailbinding.add(AVCPlanDetailEEFix);
        AVCPlanDetailbinding.init(Request, Session);
        binding.add(new LabelVLBinder(db, AVCPlanEmployerRoundingRule, Values.VLRoundingRule));
        binding.add(AVCPlanEmployerDecimalPlace);
        binding.add(new LabelVLBinder(db, AVCPlanEmployeeRoundingRule, Values.VLRoundingRule));
        binding.add(AVCPlanEmployeeDecimalPlace);

        sbinding = new SearchBinding(dbConn, AVCPlanDetaildb);
        sbinding.add(new HiddenMatchBinder(AVCPlanID));
        sbinding.init(DecryptedRequest, null);

        ceilingbinding = new SearchBinding(dbConn, EPaymentCode.db);
        ceilingbinding.add(new HiddenMatchBinder(AVCPlanID, "apc.AVCPlanID"));
        ceilingbinding.add(new HiddenMatchBinder(AVCPlanID, "apcc.AVCPlanID"));
        ceilingbinding.initValues("AVCPlanPaymentConsiderAfterMPF", null, Values.VLYesNo, null);
        ceilingbinding.init(DecryptedRequest, null);

        binding.init(Request, Session);

        avcCeilingInfo = avcCeilingListFooter.ListInfo;
        info = ListFooter.ListInfo;

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            NotRemoveContributionFromTopUpRow.Visible = false;
            PaymentCeilingSection.Visible = false;
        }
        else
        {
            NotRemoveContributionFromTopUpRow.Visible = true;
            PaymentCeilingSection.Visible = true;
        } 
        
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["AVCPlanID"], out CurID))
            CurID = -1;

        Delete.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(Delete);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
		if(!Page.IsPostBack) 
		{

            view = loadData(info, AVCPlanDetaildb, Repeater); //add by Ben
            avcCeilingView = loadAVCCeilingData(avcCeilingInfo, EPaymentCode.db, avcCeilingRepeater);
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
        obj = new EAVCPlan();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        if (string.IsNullOrEmpty(obj.AVCPlanEmployerRoundingRule) && string.IsNullOrEmpty(obj.AVCPlanEmployeeRoundingRule))
        {
            obj.AVCPlanEmployerRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.AVCPlanEmployerDecimalPlace = 2;
            obj.AVCPlanEmployeeRoundingRule = Values.ROUNDING_RULE_ROUND_TO;
            obj.AVCPlanEmployeeDecimalPlace = 2;
        }

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    public DataView loadAVCCeilingData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = new DBFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = " pc.*, apc.AVCPlanPaymentCeilingAmount, apcc.AVCPlanPaymentConsiderAfterMPF ";
        string from = " from " + EPaymentCode.db.dbclass.tableName + " pc " 
            + " left join " + EAVCPlanPaymentCeiling.db.dbclass.tableName + " apc on  pc.paymentcodeid=apc.paymentcodeid and apc.AVCPlanID=" + (string.IsNullOrEmpty(AVCPlanID.Value) ? "0" : AVCPlanID.Value) 
            + " left join " + EAVCPlanPaymentConsider.db.dbclass.tableName + " apcc on pc.paymentcodeid = apcc.paymentcodeid and apcc.AVCPlanID=" + (string.IsNullOrEmpty(AVCPlanID.Value) ? "0" : AVCPlanID.Value + " ");

        OR orTerms = new OR();
        orTerms.add(new NullTerm("Not AVCPlanPaymentCeilingAmount"));
        orTerms.add(new Match("AVCPlanPaymentConsiderAfterMPF", true));

        filter.add(orTerms);
        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        avcCeilingView = new DataView(table);

        avcCeilingListFooter.Refresh();

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
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
        ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        return view;
    }
    protected void AVCPlanCeiling_ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (avcCeilingInfo.orderby == null)
            avcCeilingInfo.order = true;
        else if (avcCeilingInfo.orderby.Equals(id))
            avcCeilingInfo.order = !avcCeilingInfo.order;
        else
            avcCeilingInfo.order = true;
        avcCeilingInfo.orderby = id;

        avcCeilingView = loadAVCCeilingData(avcCeilingInfo, EPaymentCode.db, avcCeilingRepeater);

        //Response.Redirect(Request.Url.LocalPath + "?AVCPlanID=" + CurID);

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        Repeater.EditItemIndex = -1;
        EAVCPlanDetail c = new EAVCPlanDetail();

        Hashtable values = new Hashtable();
        AVCPlanDetailbinding.toValues(values);

        PageErrors errors = PageErrors.getErrors(AVCPlanDetaildb, Page);
        errors.clear();


        AVCPlanDetaildb.validate(errors, values);

        if (!errors.isEmpty())
            return;


        AVCPlanDetaildb.parse(values, c);

        //if (!AppUtils.checkDuplicate(dbConn, AVCPlanDetaildb, c, errors, "AVCPlanDetailYearOfService"))
        //    return;
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        AVCPlanDetaildb.insert(dbConn, c);
        WebUtils.EndFunction(dbConn);

        AVCPlanDetailEEAboveRI.Text = string.Empty;
        AVCPlanDetailEEBelowRI.Text = string.Empty;
        AVCPlanDetailEEFix.Text = string.Empty;
        AVCPlanDetailERAboveRI.Text = string.Empty;
        AVCPlanDetailERBelowRI.Text = string.Empty;
        AVCPlanDetailERFix.Text = string.Empty;
        AVCPlanDetailYearOfService.Text = string.Empty;

        view = loadData(info, AVCPlanDetaildb, Repeater); //add by Ben
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {


        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            AVCPlanDetailebinding = new Binding(dbConn, AVCPlanDetaildb);
            AVCPlanDetailebinding.add((HtmlInputHidden)e.Item.FindControl("AVCPlanDetailID"));
           // AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanID"));              // not defined by ben
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailYearOfService"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailERBelowRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailERAboveRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailERFix"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailEEBelowRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailEEAboveRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailEEFix"));
            AVCPlanDetailebinding.init(Request, Session);


            EAVCPlanDetail obj = new EAVCPlanDetail();
            AVCPlanDetaildb.toObject(((DataRowView)e.Item.DataItem).Row, obj);
            Hashtable values = new Hashtable();
            AVCPlanDetaildb.populate(obj, values);
            AVCPlanDetailebinding.toControl(values);
        }
        else
        {
            e.Item.FindControl("Edit").Visible = toolBar.DeleteButton_Visible;
            e.Item.FindControl("DeleteItem").Visible = toolBar.DeleteButton_Visible;
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("AVCPlanDetailID");
            h.Value = ((DataRowView)e.Item.DataItem)["AVCPlanDetailID"].ToString();

        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }

    protected void Repeater_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Button b = (Button)e.CommandSource;




        if (b.ID.Equals("Edit"))
        {
            Repeater.EditItemIndex = e.Item.ItemIndex;
            view = loadData(info, AVCPlanDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel1, false);
            WebUtils.SetEnabledControlSection(AddPanel2, false);
        }
        else if (b.ID.Equals("Cancel"))
        {
            Repeater.EditItemIndex = -1;
            view = loadData(info, AVCPlanDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel1, true);
            WebUtils.SetEnabledControlSection(AddPanel2, true);
        }
        else if (b.ID.Equals("Save"))
        {
            AVCPlanDetailebinding = new Binding(dbConn, AVCPlanDetaildb);
            AVCPlanDetailebinding.add((HtmlInputHidden)e.Item.FindControl("AVCPlanDetailID"));
           // AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanID"));              // not defined by ben
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailYearOfService"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailERBelowRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailERAboveRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailERFix"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailEEBelowRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailEEAboveRI"));
            AVCPlanDetailebinding.add((TextBox)e.Item.FindControl("AVCPlanDetailEEFix"));
            AVCPlanDetailebinding.init(Request, Session);


            EAVCPlanDetail obj = new EAVCPlanDetail();
            Hashtable values = new Hashtable();

            PageErrors errors = PageErrors.getErrors(AVCPlanDetaildb, Page);
            errors.clear();


            AVCPlanDetailebinding.toValues(values);
            AVCPlanDetaildb.validate(errors, values);

            if (!errors.isEmpty())
                return;

            AVCPlanDetaildb.parse(values, obj);
            //if (!AppUtils.checkDuplicate(dbConn, AVCPlanDetaildb, obj, errors, "AVCPlanID"))
                //return;

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            AVCPlanDetaildb.update(dbConn, obj);
            WebUtils.EndFunction(dbConn);

            Repeater.EditItemIndex = -1;
            view = loadData(info, AVCPlanDetaildb, Repeater);
            WebUtils.SetEnabledControlSection(AddPanel1, true);
            WebUtils.SetEnabledControlSection(AddPanel2, true);
        }


    }



//    protected void Save_Click(object sender, EventArgs e)
//    {
//        EAVCPlan c = new EAVCPlan();

//        Hashtable values = new Hashtable();
//        binding.toValues(values);

//        PageErrors errors = PageErrors.getErrors(db, Page.Master);
//        errors.clear();


//        db.validate(errors, values);

//        if (!errors.isEmpty())
//            return;


//        db.parse(values, c);
//        if (!AppUtils.checkDuplicate(dbConn, db, c, errors, "AVCPlanCode"))
//            return;

//        WebUtils.StartFunction(Session, FUNCTION_CODE);
//        if (CurID < 0)
//        {
////            Utils.MarkCreate(Session, c);

//            db.insert(dbConn, c);
//            CurID = c.AVCPlanID;
////            url = Utils.BuildURL(-1, CurID);
//        }
//        else
//        {
////            Utils.Mark(Session, c);
//            db.update(dbConn, c);
//        }
//        WebUtils.EndFunction(dbConn);


//        Response.Redirect(Request.Url.LocalPath+"?AVCPlanID="+CurID);


//    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        //EAVCPlan c = new EAVCPlan();
        //c.AVCPlanID = CurID;
        //db.delete(dbConn, c);
        //HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_List.aspx");
        ArrayList list = new ArrayList();
        foreach (DataListItem item in Repeater.Items)
        {
            CheckBox c = (CheckBox)item.FindControl("DeleteItem");
            HtmlInputHidden h = (HtmlInputHidden)item.FindControl("AVCPlanDetailID");
            if (c.Checked)
            {
                EAVCPlanDetail obj = new EAVCPlanDetail();
                obj.AVCPlanDetailID = Int32.Parse(h.Value);
                list.Add(obj);
            }
        }
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        foreach (EAVCPlanDetail obj in list)
            if (AVCPlanDetaildb.select(dbConn, obj))
                AVCPlanDetaildb.delete(dbConn, obj);
        WebUtils.EndFunction(dbConn);
        view = loadData(info, AVCPlanDetaildb, Repeater); 

    }
    protected void Delete_ClickTop(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAVCPlan o = new EAVCPlan();
        o.AVCPlanID = CurID;
        if (db.select(dbConn, o))
        {
            DBFilter empAVCFilter = new DBFilter();
            empAVCFilter.add(new Match("AVCPlanID", o.AVCPlanID));
            empAVCFilter.add("empid", true);
            ArrayList empAVCList = EEmpAVCPlan.db.select(dbConn, empAVCFilter);
            if (empAVCList.Count > 0)
            {
                int curEmpID = 0;
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("AVC Plan Code"), o.AVCPlanCode }));
                foreach (EEmpAVCPlan empAVCPlan in empAVCList)
                {
                    EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                    empInfo.EmpID = empAVCPlan.EmpID;
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                        if (curEmpID != empAVCPlan.EmpID)
                        {
                            errors.addError("- " + empInfo.EmpNo + ", " + empInfo.EmpEngFullName);
                            curEmpID = empAVCPlan.EmpID;
                        }
                        else
                            EEmpAVCPlan.db.delete(dbConn, empAVCPlan);

                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;
            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, o);
                DBFilter obj = new DBFilter();
                obj.add(new Match("AVCPlanID", o.AVCPlanID));
                ArrayList objList = EAVCPlanDetail.db.select(dbConn, obj);
                foreach (EAVCPlanDetail match in objList)
                    EAVCPlanDetail.db.delete(dbConn, match);

                objList = EAVCPlanPaymentCeiling.db.select(dbConn, obj);
                foreach (EAVCPlanPaymentCeiling match in objList)
                    EAVCPlanPaymentCeiling.db.delete(dbConn, match);

                objList = EAVCPlanPaymentConsider.db.select(dbConn, obj);
                foreach (EAVCPlanPaymentConsider match in objList)
                    EAVCPlanPaymentConsider.db.delete(dbConn, match);
                WebUtils.EndFunction(dbConn);

            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_List.aspx");

    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_Edit.aspx?AVCPlanID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "AVCPlan_List.aspx");
    }
}
