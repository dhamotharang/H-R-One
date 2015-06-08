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
//using perspectivemind.validation;

public partial class Attendance_Record_Import : HROneWebPage
{
    private DBManager db = EUploadAttendanceRecord.db;
    private const string FUNCTION_CODE = "ATT010";
    protected SearchBinding sbinding;
    protected Binding ebinding;
    protected ListInfo info;
    private DataView view;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;

        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);


        sbinding.initValues("RosterCodeID", new DBFilter(), ERosterCode.VLRosterCode, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.initValues("AttendanceRecordIsAbsent", new DBFilter(), Values.VLYesNo, HROne.Common.WebUtility.GetSessionUICultureInfo(Session));
        sbinding.init(DecryptedRequest, null);

        info = ListFooter.ListInfo;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }

        //CNDImportFile.ControlStyle.CssClass = "button";
    }

    public DataView loadData(ListInfo info, DBManager db, DataList repeater)
    {
        //filter.add(new Match("EmpPayrollID",EmpPayrollID.Value));

        ImportAttendanceRecordProcess attendanceRecordImport = new ImportAttendanceRecordProcess(dbConn, Session.SessionID, false);
        DataTable table = attendanceRecordImport.GetImportDataFromTempDatabase(info);

        if (info != null)
            if (!string.IsNullOrEmpty(info.orderby))
                if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!table.Columns.Contains("EmpEngFullName"))
                    {
                        table.Columns.Add("EmpEngFullName", typeof(string));
                        foreach (System.Data.DataRow row in table.Rows)
                        {
                            EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                            empInfo.EmpID = (int)row["EmpID"];
                            if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                                row["EmpEngFullName"] = empInfo.EmpEngFullName;
                        }
                    }
                }

        table = WebUtils.DataTableSortingAndPaging(table, info); 
        
        view = new DataView(table);
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }
        if (table.Rows.Count > 0)
            ImportSection.Visible = true;
        else
            ImportSection.Visible = false;

        return view;
    }


    protected void Upload_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        if (CNDImportFile.HasFile)
        {
            //DataTable dataTable = HROne.CSVProcess.CSVReader.parse(CNDImportFile.PostedFile.InputStream);
            string strTmpFolder = HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).FullName;// System.IO.Path.GetTempPath(); //Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string strTmpFile = System.IO.Path.Combine(strTmpFolder, AppUtils.ServerDateTime().ToString("~yyyyMMddHHmmss_") + CNDImportFile.FileName);
            CNDImportFile.SaveAs(strTmpFile);

            ImportAttendanceRecordProcess attendanceRecordImport = new ImportAttendanceRecordProcess(dbConn, Session.SessionID, false);
            //DataTable dataTable = HROne.Import.ExcelImport.parse(strTmpFile);
            //using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\csv\;Extended Properties='Text;'"))
            try
            {
                DataTable table = attendanceRecordImport.UploadToTempDatabase(strTmpFile, WebUtils.GetCurUser(Session).UserID, string.Empty);
                //Repeater.DataSource = table;
                //Repeater.DataBind();
                ImportSection.Visible = true;
            }
            catch (HRImportException ex)
            {
                if (attendanceRecordImport.errors.List.Count > 0)
                    foreach (string errorString in attendanceRecordImport.errors.List)
                        errors.addError(errorString);
                else
                    errors.addError(ex.Message);
                attendanceRecordImport.ClearTempTable();
            }
            //System.IO.File.Delete(strTmpFile);
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
        //view = loadData(info, db, Repeater);

    }

    protected void Repeater_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        EUploadAttendanceRecord obj = new EUploadAttendanceRecord();
        db.toObject(((DataRowView)e.Item.DataItem).Row, obj);
        Hashtable values = new Hashtable();
        db.populate(obj, values);

        if (e.Item.ItemIndex == Repeater.EditItemIndex)
        {
            //Binding eBinding;
            //eBinding = new Binding(dbConn, db);
            //eBinding.add(EmpID);
            //eBinding.add((HtmlInputHidden)e.Item.FindControl("AttendanceRecordID"));
            //eBinding.add((Label)e.Item.FindControl("AttendanceRecordDate"));
            //eBinding.add(new DropDownVLBinder(db, (DropDownList)e.Item.FindControl("RosterCodeID"), ERosterCode.VLRosterCode));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeInTimeOverride"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchStartTimeOverride"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeLunchEndTimeOverride"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRosterCodeOutTimeOverride"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkStart"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchOut"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchIn"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkEnd"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkStartLocation"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchOutLocation"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordLunchInLocation"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordWorkEndLocation"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLateMins"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualEarlyLeaveMins"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchLateMins"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchEarlyLeaveMins"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualOvertimeMins"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualWorkingHour"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualWorkingDay"));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordActualLunchTimeMins"));
            //eBinding.add(new CheckBoxBinder(db, (CheckBox)e.Item.FindControl("AttendanceRecordIsAbsent")));
            //eBinding.add((TextBox)e.Item.FindControl("AttendanceRecordRemark"));


            //eBinding.init(Request, Session);

            //eBinding.toControl(values);

            //if (obj.AttendanceRecordOverrideBonusEntitled)
            //    if (obj.AttendanceRecordHasBonus)
            //        ((DropDownList)e.Item.FindControl("AttendanceRecordHasBonus")).SelectedValue = "Y";
            //    else
            //        ((DropDownList)e.Item.FindControl("AttendanceRecordHasBonus")).SelectedValue = "N";
            //else
            //    ((DropDownList)e.Item.FindControl("AttendanceRecordHasBonus")).SelectedValue = "";
        }
        else
        {
            Binding eBinding;
            eBinding = new Binding(dbConn, db);
            eBinding.add(new BlankZeroLabelVLBinder(db, (Label)e.Item.FindControl("RosterCodeID"), ERosterCode.VLRosterCode));
            eBinding.init(Request, Session);
            eBinding.toControl(values);

            //HtmlInputHidden h = (HtmlInputHidden)e.Item.FindControl("AttendanceRecordID");
            //h.Value = ((DataRowView)e.Item.DataItem)["AttendanceRecordID"].ToString();
            if (obj.AttendanceRecordOverrideBonusEntitled)
                if (obj.AttendanceRecordHasBonus)
                    ((Label)e.Item.FindControl("AttendanceRecordHasBonus")).Text = "Yes";
                else
                    ((Label)e.Item.FindControl("AttendanceRecordHasBonus")).Text = "No";
            else
                ((Label)e.Item.FindControl("AttendanceRecordHasBonus")).Text = "By Attendance Plan";
        }
        HROne.Common.WebUtility.WebControlsLocalization(Session, e.Item.Controls);
    }
    protected void Import_Click(object sender, EventArgs e)
    {
        ImportAttendanceRecordProcess attendanceRecordImport = new ImportAttendanceRecordProcess(dbConn, Session.SessionID, Recalculate.Checked);
        WebUtils.StartFunction(Session, FUNCTION_CODE);
        attendanceRecordImport.ImportToDatabase();
        WebUtils.EndFunction(dbConn);
        //loadData(info, db, Repeater);
        PageErrors.getErrors(db, Page).addError(HROne.Translation.PageMessage.IMPORT_SUCCESSFUL);

    }
    protected void ChangePage(object sender, EventArgs e)
    {
        //view = loadData(info, db, Repeater);
    }
}
