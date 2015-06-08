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

public partial class Training_Seminar_View : HROneWebPage
{
    private const string FUNCTION_CODE = "TRA002";
    private const string ENROLL_FUNCTION_CODE = "TRA003";
    public Binding binding;
    public DBManager db = ETrainingSeminar.db;
    public ETrainingSeminar obj;
    public int CurID = -1;
    protected SearchBinding trainingEnrollBinding;
    protected ListInfo info;
    protected DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(TrainingSeminarID);
        binding.add(new LabelVLBinder(db, TrainingCourseID, ETrainingCourse.VLTrainingCourse));
        binding.add(TrainingSeminarDesc);
        binding.add(TrainingSeminarDateFrom);
        binding.add(TrainingSeminarDateTo);
        binding.add(TrainingSeminarDuration);
        binding.add(new LabelVLBinder(db, TrainingSeminarDurationUnit, ETrainingSeminar.VLTrainingDurationUnit));
        binding.add(TrainingSeminarTrainer);
        binding.init(Request, Session);

        trainingEnrollBinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        trainingEnrollBinding.add(new HiddenMatchSearchBinder(TrainingSeminarID, "ete.TrainingSeminarID"));
        trainingEnrollBinding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, null);
        trainingEnrollBinding.init(DecryptedRequest, null);


        if (!int.TryParse(DecryptedRequest["TrainingSeminarID"], out CurID))
            CurID = -1;

        info = ListFooter.ListInfo;

        TrainingSeminarEnrollPanel.Visible = WebUtils.CheckPermission(Session, ENROLL_FUNCTION_CODE, WebUtils.AccessLevel.Read);
        btnEnroll.Visible = WebUtils.CheckPermission(Session, ENROLL_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
        btnRemove.Visible = WebUtils.CheckPermission(Session, ENROLL_FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite);
        SelectAllPanel.Visible = btnRemove.Visible;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
		if(!Page.IsPostBack) 
		{
            if (CurID > 0)
            {
                loadObject();
                view = loadData(info, db, Repeater);
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }
    protected bool loadObject() 
    {
	    obj=new ETrainingSeminar();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = trainingEnrollBinding.createFilter();

        //if (info != null && info.orderby != null && !info.orderby.Equals(""))
        //    filter.add(info.orderby, info.order);

        string select = "e.*, ete.EmpTrainingEnrollID  ";
        string from = "from [" + EEmpPersonalInfo.db.dbclass.tableName + "] e, ["  + EEmpTrainingEnroll.db.dbclass.tableName + "] ete ";//LEFT JOIN " + EEmpPositionInfo.db.dbclass.tableName + " p ON c.EmpID=p.EmpID AND p.EmpPosEffTo IS NULL";

        filter.add(new MatchField("e.EmpID", "ete.EmpID"));
        filter.add(WebUtils.AddRankFilter(Session, "ete.EmpID", true));


        //DBFilter empPosFilter = new DBFilter();

        //bool isHierarchyFilterExists = false;
        //foreach (RepeaterItem item in HierarchyLevel.Items)
        //{
        //    DBFilter sub = null;
        //    DropDownList c = (DropDownList)item.FindControl("HElementID");
        //    string v = c.SelectedValue;
        //    if (!v.Equals(""))
        //    {
        //        if (sub == null)
        //        {
        //            sub = new DBFilter();
        //            sub.add(new MatchField("p.EmpPosID", "EmpPosID"));
        //        }
        //        sub.add(new Match("HLevelID", c.Attributes["HLevelID"]));
        //        sub.add(new Match("HElementID", v));
        //    }
        //    if (sub != null)
        //    {
        //        isHierarchyFilterExists = true;
        //        empPosFilter.add(new Exists(EEmpHierarchy.db.dbclass.tableName, sub));
        //    }
        //}

        //if (isHierarchyFilterExists)
        //{
        //    empPosFilter.add(new MatchField("c.EmpID", "p.EmpID"));
        //    OR orEmpPosEffToTerms = new OR();
        //    orEmpPosEffToTerms.add(new Match("p.empPosEffTo", ">=", AppUtils.ServerDateTime()));
        //    orEmpPosEffToTerms.add(new NullTerm("p.empPosEffTo"));
        //    empPosFilter.add(orEmpPosEffToTerms);


        //    filter.add(new Exists(EEmpPositionInfo.db.dbclass.tableName + " p", empPosFilter));
        //}

        //DBFilter empInfoFilter = EmployeeSearchControl1.GetEmpInfoFilter(AppUtils.ServerDateTime(), AppUtils.ServerDateTime());
        //empInfoFilter.add(new MatchField("e.EmpID", "ee.EmpID"));
        //filter.add(new Exists(EEmpPersonalInfo.db.dbclass.tableName + " ee", empInfoFilter));

        //DataTable table = filter.loadData(dbConn, null, select, from);
        //table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
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
        WebFormUtils.LoadKeys(EEmpTrainingEnroll.db, row, cb);
        cb.Visible = btnRemove.Visible;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        ETrainingSeminar c = new ETrainingSeminar();
        c.TrainingSeminarID = CurID;
        if (db.select(dbConn, c))
        {
            DBFilter trainingSeminarFilter = new DBFilter();
            trainingSeminarFilter.add(new Match("TrainingSeminarID", CurID));
            EEmpTrainingEnroll.db.delete(dbConn, trainingSeminarFilter);

            WebUtils.StartFunction(Session, FUNCTION_CODE);
            db.delete(dbConn, c);
            WebUtils.EndFunction(dbConn);
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_List.aspx");
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_Edit.aspx?TrainingSeminarID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_List.aspx");
    }
    protected void btnEnroll_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_Enroll.aspx?TrainingSeminarID=" + CurID);
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(EEmpTrainingEnroll.db, Repeater, "ItemSelect");
        foreach (EEmpTrainingEnroll o in list)
        {
            if (EEmpTrainingEnroll.db.select(dbConn, o))
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE, o.EmpID);
                EEmpTrainingEnroll.db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_View.aspx?TrainingSeminarID=" + CurID);
    }
}
