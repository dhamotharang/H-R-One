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
using HROne.Translation;

public partial class Customize_AttendancePreparationProcess_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "CUSTOM003";
    public Binding binding;
    protected SearchBinding sbinding;
    public DBManager db = EAttendancePreparationProcess.db;
    public EAttendancePreparationProcess obj;
    public int CurID = -1;

    protected ListInfo info;
    private DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        binding = new Binding(dbConn, db);
        binding.add(AttendancePreparationProcessID);
        binding.add(AttendancePreparationProcessDesc);
        binding.add(AttendancePreparationProcessStatus);
        binding.add(new TextBoxBinder(db, AttendancePreparationProcessMonth.TextBox, AttendancePreparationProcessMonth.ID));
        binding.add(new TextBoxBinder(db, AttendancePreparationProcessPeriodFr.TextBox, AttendancePreparationProcessPeriodFr.ID));
        binding.add(new TextBoxBinder(db, AttendancePreparationProcessPeriodTo.TextBox, AttendancePreparationProcessPeriodTo.ID));
        binding.add(new TextBoxBinder(db, AttendancePreparationProcessPayDate.TextBox, AttendancePreparationProcessPayDate.ID));
        binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, db);

        if (!int.TryParse(DecryptedRequest["AttendancePreparationProcessID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                AttendancePreparationProcessStatus.Text = EAttendancePreparationProcess.STATUS_NORMAL;
                AttendancePreparationProcessStatusDesc.Text = EAttendancePreparationProcess.STATUS_NORMAL_DESC;
                toolBar.DeleteButton_Visible = false;
            }
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected bool loadObject() 
    {
        obj = new EAttendancePreparationProcess();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

	    Hashtable values=new Hashtable();
	    db.populate(obj,values);
	    binding.toControl(values);

        switch (obj.AttendancePreparationProcessStatus)
        {
            case EAttendancePreparationProcess.STATUS_NORMAL:
                AttendancePreparationProcessStatusDesc.Text = EAttendancePreparationProcess.STATUS_NORMAL_DESC;
                break;
            case EAttendancePreparationProcess.STATUS_CONFIRMED:
                AttendancePreparationProcessStatusDesc.Text = EAttendancePreparationProcess.STATUS_CONFIRMED_DESC;
                break;
        }

        return true;
    }

    public DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = sbinding.createFilter();

        string select = " c.* ";
        string from = "from  " + db.dbclass.tableName + " c ";

        filter.add(new Match("AttendancePreparationProcessStatus", EAttendancePreparationProcess.STATUS_NORMAL));

        DataTable table = WebUtils.GetDataTableFromSelectQueryWithFilter(dbConn, select, from, filter, info);

        view = new DataView(table);

        if (repeater != null)
        {
            repeater.DataSource = table;
            repeater.DataBind();
        }

        return view;
    }

    protected void AttendancePreparationProcessMonth_Changed(object sender, EventArgs e)
    {
        DateTime m_value;
        DateTime dt1;
        DateTime dt2;
        if (DateTime.TryParse(AttendancePreparationProcessMonth.Value, out m_value))
        {
            if (string.IsNullOrEmpty(AttendancePreparationProcessPeriodFr.Value) && string.IsNullOrEmpty(AttendancePreparationProcessPeriodTo.Value))
            {
                dt1 = new DateTime(m_value.Year, m_value.Month, 1);
                dt2 = dt1.AddMonths(1).AddDays(-1);
                AttendancePreparationProcessPeriodFr.Value = (dt1).ToString("yyyy-MM-dd");
                AttendancePreparationProcessPeriodTo.Value = (dt2).ToString("yyyy-MM-dd");
            }
        }
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        EAttendancePreparationProcess c = new EAttendancePreparationProcess();
        
        Hashtable values = new Hashtable();
        binding.toValues(values);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        db.validate(errors, values);

        if (!errors.isEmpty())
            return;

        db.parse(values, c);

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        if (CurID < 0)
        {
            db.insert(dbConn, c);
            CurID = c.AttendancePreparationProcessID;
        }
        else
        {
            db.update(dbConn, c);
        }

        WebUtils.EndFunction(dbConn);

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_View.aspx?AttendancePreparationProcessID=" + CurID);

    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAttendancePreparationProcess o = new EAttendancePreparationProcess();
        o.AttendancePreparationProcessID = CurID;

        if (db.select(dbConn, o))
        {
            if (o.AttendancePreparationProcessStatus != EAttendancePreparationProcess.STATUS_NORMAL)
            {
                errors.addError("Attendance Preparation Process remove failed.  Status is not " + EAttendancePreparationProcess.STATUS_NORMAL_DESC);

            }
            else
            {
                DBFilter m_filter = new DBFilter();
                m_filter.add(new Match("AttendancePreparationProcessID", CurID));

                if (EAttendancePreparationProcess.db.delete(dbConn, m_filter))
                {
                    errors.addError("Attendance Preparation Process removed");
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_List.aspx");
                }
                else
                    errors.addError("Attendance Preparation Process remove failed.");
            }
        }
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_View.aspx?AttendancePreparationProcessID=" + CurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_List.aspx");

    }
}
