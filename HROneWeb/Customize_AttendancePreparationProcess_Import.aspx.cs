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
using System.Data.OleDb;
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;

public partial class Customize_AttendancePreparationProcess_Import : HROneWebPage
{
    private const string FUNCTION_CODE = "CUSTOM003";

    private DBManager db = EUploadAttendancePreparationProcess.db;
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;
    public int gPID;

    static string prevPage = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        if (!int.TryParse(DecryptedRequest["PID"], out gPID))
            gPID = -1;

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        view = loadData(info, db, Repeater);
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        HROne.Import.ImportAttendancePreparationProcess m_import = new ImportAttendancePreparationProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
        DataTable m_table = m_import.GetImportDataFromTempDatabase(info);

        if (info != null)
        {
            if (!string.IsNullOrEmpty(info.orderby))
            {
                if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!m_table.Columns.Contains("EmpEngFullName"))
                    {
                        m_table.Columns.Add("EmpEngFullName", typeof(string));
                        foreach (System.Data.DataRow row in m_table.Rows)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = (int)row["EmpID"];
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                row["EmpEngFullName"] = empInfo.EmpEngFullName;
                        }
                    }
                }
            }
        }
        m_table = WebUtils.DataTableSortingAndPaging(m_table, info);

        view = new DataView(m_table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        if (m_table.Rows.Count > 0)
            ImportSection.Visible = true;
        else
            ImportSection.Visible = false;

        return view;
    }

    protected void Upload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (ImportFile.HasFile)
        {
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName; ; //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + ImportFile.FileName);
            ImportFile.SaveAs(strTmpFile);

            ImportAttendancePreparationProcess m_import = new ImportAttendancePreparationProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, this.gPID);

            try
            {
                DataTable m_table = m_import.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty,gPID);
                ImportSection.Visible = true;
            }
            catch (HRImportException ex)
            {
                if (m_import.errors.List.Count > 0)
                    foreach (string errorString in m_import.errors.List)
                        errors.addError(errorString);
                else
                    errors.addError(ex.Message);
                m_import.ClearTempTable();
            }
            System.IO.File.Delete(strTmpFile);
        }
        else
            errors.addError(HROne.Translation.PageErrorMessage.ERROR_INVALID_FILE);
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
    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EUploadAttendancePreparationProcess obj = new EUploadAttendancePreparationProcess();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            ebinding = new Binding(dbConn, db);
            ebinding.add((HtmlInputHidden)e.Item.FindControl("AttendancePreparationProcessID"));
            ebinding.add((TextBox)e.Item.FindControl("TotalHours"));

            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);
        }
        else
        {
            HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("UploadAttendancePreparationProcessID");
            h.Value = ((DataRowView)e.Item.DataItem)["UploadAttendancePreparationProcessID"].ToString();

            ebinding = new Binding(dbConn, db);

            ebinding.init(Request, Session);

            Hashtable values = new Hashtable();
            db.populate(obj, values);

            ebinding.toControl(values);
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, e.Item.Controls);
    }

    protected void Import_Click(object sender, EventArgs e)
    {
        ImportAttendancePreparationProcess m_import = new ImportAttendancePreparationProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID);
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        m_import.ImportToDatabase();

        // Set employee count
        DBFilter m_countFilter = new DBFilter();
        m_countFilter.add(new Match("AttendancePreparationProcessID", gPID));
        int empCount = EEmpAttendancePreparationProcess.db.count(dbConn, m_countFilter);

        EAttendancePreparationProcess attendancePreparationProcess = EAttendancePreparationProcess.GetObject(dbConn, gPID);
        attendancePreparationProcess.AttendancePreparationProcessEmpCount = empCount;
        EAttendancePreparationProcess.db.update(dbConn, attendancePreparationProcess);

        DBFilter m_clearTempFilter = new DBFilter();
        m_clearTempFilter.add(new Match("AttendancePreparationProcessID", gPID));
        EUploadAttendancePreparationProcess.db.delete(dbConn, m_clearTempFilter);
        
        WebUtils.EndFunction(dbConn);

        PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);
    }

    protected void ChangePage(object sender, EventArgs e)
    {

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prevPage))
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, prevPage);
        }
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_View.aspx?AttendancePreparationProcessID=" + gPID);
    }
}
