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
using HROne.Import;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Customize_AttendancePreparationProcess_View : HROneWebPage
{
    private const string FUNCTION_CODE = "CUSTOM003";
    public Binding binding;
    public DBManager db = EAttendancePreparationProcess.db;
    public EAttendancePreparationProcess obj;
    public int CurID = -1;

    protected SearchBinding sbinding;
    protected ListInfo info;
    private DataView view;

    protected void Page_Load(object sender, EventArgs e)
    {

        btnConfirmAndSeal.OnClientClick = HROne.Translation.PromptMessage.CreateConfirmDialogJavascript(HROne.Common.WebUtility.GetLocalizedString("ARE_YOU_SURE_TO_CONFIRM", ci), btnConfirmAndSeal);

        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        sbinding = new SearchBinding(dbConn, db);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

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
                btnConfirmAndSeal.Visible = false;
            }
        }
    }
    protected bool loadObject() 
    {
        obj = new EAttendancePreparationProcess();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        ProcessID.Value = obj.AttendancePreparationProcessID.ToString("0");
        AttendancePreparationProcessDesc.Text = obj.AttendancePreparationProcessDesc;
        AttendancePreparationProcessMonth.Text = obj.AttendancePreparationProcessMonth.ToString("yyyy-MM");
        AttendancePreparationProcessPayDate.Text = obj.AttendancePreparationProcessPayDate.ToString("yyyy-MM-dd");
        AttendancePreparationProcessStatus.Text = obj.AttendancePreparationProcessStatus;
        switch (obj.AttendancePreparationProcessStatus)
        {
            case EAttendancePreparationProcess.STATUS_NORMAL:
                AttendancePreparationProcessStatusDesc.Text = EAttendancePreparationProcess.STATUS_NORMAL_DESC;
                break;
            case EAttendancePreparationProcess.STATUS_CONFIRMED:
                AttendancePreparationProcessStatusDesc.Text = EAttendancePreparationProcess.STATUS_CONFIRMED_DESC;
                break;
            case EAttendancePreparationProcess.STATUS_CANCELLED:
                AttendancePreparationProcessStatusDesc.Text = EAttendancePreparationProcess.STATUS_CANCELLED_DESC;
                break;
        }

        if (obj.AttendancePreparationProcessPeriodFr.Ticks != 0)
            AttendancePreparationProcessPeriodFr.Text = obj.AttendancePreparationProcessPeriodFr.ToString("yyyy-MM-dd");

        if (obj.AttendancePreparationProcessPeriodTo.Ticks != 0)
            AttendancePreparationProcessPeriodTo.Text = obj.AttendancePreparationProcessPeriodTo.ToString("yyyy-MM-dd");

        // load upload count
        DBFilter m_countFilter = new DBFilter();
        m_countFilter.add(new Match("AttendancePreparationProcessID", CurID));
        UploadCount.Text = EEmpAttendancePreparationProcess.db.count(dbConn, m_countFilter).ToString("0");

        // control visibility of commands 
        if (obj.AttendancePreparationProcessStatus != EAttendancePreparationProcess.STATUS_NORMAL)
        {
            BasicInfoCommands.Visible = false;
            toolBar.EditButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
            btnConfirmAndSeal.Visible = false;
            ButtonCommands.Visible = false;
        }

        if (this.UploadCount.Text == "0")
        {
            btnGenerateCalculatedReport.Enabled = false;
            btnGenerateCND.Enabled = false;
        }

        return true;
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_Edit.aspx?AttendancePreparationProcessID=" + CurID);
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
                errors.addError("Status must be Normal");
            }
            else
            {
                o.AttendancePreparationProcessStatus = EAttendancePreparationProcess.STATUS_CANCELLED;
                if (EAttendancePreparationProcess.db.update(dbConn, o))
                    errors.addError("Update Completed");
                else
                    errors.addError("Update failed");
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Customize_AttendancePreparationProcess_List.aspx");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/SelectEmployee_List.aspx?Process=AttendancePreparationProcess&&PID=" + CurID);
    }

    protected void Import_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Customize_AttendancePreparationProcess_Import.aspx?PID=" + CurID);
    }

    protected void btnConfirmProcess_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        HROne.Import.ImportAttendancePreparationProcess m_importProcess = new HROne.Import.ImportAttendancePreparationProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);
        if (!m_importProcess.ConfirmAttendanceProcess(errors))
        {
            errors.addError("Confirm failed, please note that Attendance Process must be " + EAttendancePreparationProcess.STATUS_NORMAL_DESC + " to be confirmed");
        }else
        {
            errors.addError("Attendance Process confirmed! It is locked and not allow for further operations");
        }
        loadObject();
    }

    protected void GenerateCalculatedReport_Click(object sender, EventArgs e)
    {
            //DBFilter m_rpFilter = new DBFilter();
            //OR m_or = new OR();
            //m_or.add(new NullTerm("EmpRPEffTo"));
            //m_or.add(new Match("EmpRPEffTo", ">=", HROne.CommonLib.Utility.LastDateOfMonth(AppUtils.ServerDateTime())));

            //m_rpFilter.add(m_or);
            //m_rpFilter.add(new Match("EmpRPEffFr", "<=", HROne.CommonLib.Utility.LastDateOfMonth(AppUtils.ServerDateTime())));

            //DBFilter m_isRPWinsonFilter = new DBFilter();
            //m_rpFilter.add(new IN("EmpRPID", "SELECT EmpRPID FROM EmpRPWinson", m_isRPWinsonFilter));

            //DBFilter m_EmpPersonalFilter = new DBFilter();
            //m_EmpPersonalFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpRecurringPayment", m_rpFilter));

            //ArrayList empList = EEmpPersonalInfo.db.select(dbConn, m_EmpPersonalFilter);

            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            //HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);

            ArrayList empList = EEmpPersonalInfo.db.select(dbConn, new DBFilter());

            ImportAttendancePreparationProcess.ExportCalculatedTemplate(dbConn, empList, exportFileName, CurID);

            WebUtils.TransmitFile(Response, exportFileName, "calculated_attendance_record_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);             
    }
    
    protected void btnGenerateCND_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAttendancePreparationProcess m_process = EAttendancePreparationProcess.GetObject(dbConn, CurID);
        if (m_process != null)
        {
            DBFilter m_rpFilter = new DBFilter();
            OR m_or = new OR();
            m_or.add(new NullTerm("EmpRPEffTo"));
            m_or.add(new Match("EmpRPEffTo", ">=", HROne.CommonLib.Utility.LastDateOfMonth(AppUtils.ServerDateTime())));

            m_rpFilter.add(m_or);
            m_rpFilter.add(new Match("EmpRPEffFr", "<=", HROne.CommonLib.Utility.LastDateOfMonth(AppUtils.ServerDateTime())));

            DBFilter m_isRPWinsonFilter = new DBFilter();
            m_rpFilter.add(new IN("EmpRPID", "SELECT EmpRPID FROM EmpRPWinson", m_isRPWinsonFilter));

            DBFilter m_EmpPersonalFilter = new DBFilter();
            m_EmpPersonalFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpRecurringPayment", m_rpFilter));

            ArrayList empList = EEmpPersonalInfo.db.select(dbConn, m_EmpPersonalFilter);

            HROne.Import.ImportAttendancePreparationProcess m_import = new HROne.Import.ImportAttendancePreparationProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);

            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            DataSet dataSet = m_import.GenerateCND(dbConn, empList, exportFileName, CurID);
            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "CND_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        }
        else
            errors.addError("Failed to open batch");
    }

    protected void btnClearData_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        HROne.Import.ImportAttendancePreparationProcess m_importProcess = new HROne.Import.ImportAttendancePreparationProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);
        if (m_importProcess.ClearUploadedData(errors, CurID))
        {
            errors.addError("Uploaded records cleared");
        }
        loadObject();
    }
}
