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
using HROne.Translation;
using HROne.Lib.Entities;

public partial class Customize_AttendancePreparationProcess_List : HROneWebPage
{
    private const string FUNCTION_CODE = "CUSTOM003";

    protected DBManager db = EAttendancePreparationProcess.db;
    protected SearchBinding binding;
    protected ListInfo info;
    protected DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        SelectAllPanel.Visible = toolBar.DeleteButton_Visible;

        binding = new SearchBinding(dbConn, db);
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

        string select = "c.*, CONVERT(nvarchar(7), c.AttendancePreparationProcessMonth, 21) AttendancePreparationProcessMonth2 ";
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
        e.Item.FindControl("ItemSelect").Visible = toolBar.DeleteButton_Visible;

        Binding ebinding;

        EAttendancePreparationProcess obj = new EAttendancePreparationProcess();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (obj.AttendancePreparationProcessID > 0)
        {
            ((Label)e.Item.FindControl("AttendancePreparationProcessPayDate")).Text = obj.AttendancePreparationProcessPayDate.ToString("yyyy-MM-dd");
            switch (obj.AttendancePreparationProcessStatus)
            {
                case EAttendancePreparationProcess.STATUS_NORMAL:
                    ((Label)e.Item.FindControl("AttendancePreparationProcessStatus")).Text = EAttendancePreparationProcess.STATUS_NORMAL_DESC;
                    break;
                case EAttendancePreparationProcess.STATUS_CONFIRMED:
                    ((Label)e.Item.FindControl("AttendancePreparationProcessStatus")).Text = EAttendancePreparationProcess.STATUS_CONFIRMED_DESC;
                    break;
                case EAttendancePreparationProcess.STATUS_CANCELLED:
                    ((Label)e.Item.FindControl("AttendancePreparationProcessStatus")).Text = EAttendancePreparationProcess.STATUS_CANCELLED_DESC;
                    break;
            }
        }
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ArrayList list = WebUtils.SelectedRepeaterItemToBaseObjectList(db, Repeater, "ItemSelect");

        foreach (EAttendancePreparationProcess o in list)
        {           
            if (db.select(dbConn, o))
            {
                if (o.AttendancePreparationProcessStatus != EAttendancePreparationProcess.STATUS_NORMAL)
                {
                    errors.addError("Attendance Preparation Process remove failed. (Status = '" + o.AttendancePreparationProcessStatus + "')");
                }
                else
                {
                    WebUtils.StartFunction(Session, FUNCTION_CODE);
                    DBFilter m_filter = new DBFilter();
                    m_filter.add(new Match("AttendancePreparationProcessID", o.AttendancePreparationProcessID));
                    EAttendancePreparationProcess.db.delete(dbConn, m_filter);
                    db.delete(dbConn, o);
                    WebUtils.EndFunction(dbConn);
                }
            }
        }
        loadData(info, db, Repeater);
    }

    public void New_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_Edit.aspx");
    }
}
