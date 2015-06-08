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

public partial class Training_Seminar_List : HROneWebPage
{
    private const string FUNCTION_CODE = "TRA002";

    protected DBManager db = ETrainingSeminar.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;
    private bool IsAllowEdit = true;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new SearchBinding(dbConn, db);
        binding.add(new DropDownVLSearchBinder(TrainingCourseID, "TrainingCourseID", ETrainingCourse.VLTrainingCourse));
        binding.add(new LikeSearchBinder(TrainingSeminarTrainer, "TrainingSeminarTrainer"));

        binding.initValues("TrainingCourseID", null, ETrainingCourse.VLTrainingCourse, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));

        binding.init(DecryptedRequest, null);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        info = ListFooter.ListInfo;

        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }


    }
    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = binding.createFilter();
        DateTime dtFrom, dtTo;
        if (DateTime.TryParse(TrainingSeminarDateTo.Value, out dtTo))
            filter.add(new Match("TrainingSeminarDateFrom", "<=", dtTo));
        if (DateTime.TryParse(TrainingSeminarDateFrom.Value, out dtFrom))
            filter.add(new Match("TrainingSeminarDateTo", ">=", dtFrom));
        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);

        string select = "c.*";
        string from = "from [" + db.dbclass.tableName + "] c ";

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
    protected void Search_Click(object sender, EventArgs e)
    {
        info.page = 0;
        view = loadData(info, db, Repeater);

    }
    protected void Reset_Click(object sender, EventArgs e)
    {
        binding.clear();
        TrainingSeminarDateFrom.Value = string.Empty;
        TrainingSeminarDateTo.Value = string.Empty;
        TrainingCourseID.SelectedIndex = 0;
        info.page = 0;
        view = loadData(info, db, Repeater);

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

        view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Visible = IsAllowEdit;
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;

    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (ETrainingSeminar o in list)
        {
            if (ETrainingSeminar.db.select(dbConn, o))
            {

                WebUtils.StartFunction(Session, FUNCTION_CODE);
                ETrainingSeminar.db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);
            }
        }
        loadData(info, db, Repeater);
    }
    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Training_Seminar_Edit.aspx");
    }
}
