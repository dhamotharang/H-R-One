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

public partial class Payroll_BonusProcess_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY020";
    public Binding binding;
    public DBManager db = EBonusProcess.db;
    public EBonusProcess obj;
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

        //binding = new Binding(dbConn, db);

        //binding.init(Request, Session);

        sbinding = new SearchBinding(dbConn, EBonusProcess.db);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        //info = ListFooter.ListInfo;

        if (!int.TryParse(DecryptedRequest["BonusProcessID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
            {
                BonusProcessStatus.Text = EBonusProcess.STATUS_NORMAL;
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_NORMAL_DESC;

                toolBar.DeleteButton_Visible = false;
                btnConfirmAndSeal.Visible = false;
            }
        }
    }
    protected bool loadObject() 
    {
	    obj=new EBonusProcess();
	    bool isNew=WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
	    if(!db.select(dbConn, obj))
		    return false;

        BonusProcessID.Value = obj.BonusProcessID.ToString("0");
        BonusProcessDesc.Text = obj.BonusProcessDesc;
        BonusProcessMonth.Text = obj.BonusProcessMonth.ToString("yyyy-MM");
        BonusProcessPayDate.Text = obj.BonusProcessPayDate.ToString("yyyy-MM-dd");
        if (obj.BonusProcessPayCodeID > 0)
        {
            EPaymentCode m_code = EPaymentCode.GetObject(dbConn, obj.BonusProcessPayCodeID);
            BonusProcessPayCode.Text = m_code.PaymentCodeDesc;
        }
        BonusProcessStatus.Text = obj.BonusProcessStatus;
        switch (obj.BonusProcessStatus)
        {
            case EBonusProcess.STATUS_NORMAL:
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_NORMAL_DESC;
                break;
            case EBonusProcess.STATUS_CONFIRMED:
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_CONFIRMED_DESC;
                break;
            case EBonusProcess.STATUS_CANCELLED:
                BonusProcessStatusDesc.Text = EBonusProcess.STATUS_CANCELLED_DESC;
                break;
        }

        if (obj.BonusProcessPeriodFr.Ticks != 0)
            BonusProcessPeriodFr.Text = obj.BonusProcessPeriodFr.ToString("yyyy-MM-dd");

        if (obj.BonusProcessPeriodTo.Ticks != 0)
            BonusProcessPeriodTo.Text = obj.BonusProcessPeriodTo.ToString("yyyy-MM-dd");

        // Part 1   
        if (obj.BonusProcessSalaryMonth.Ticks != 0)
            BonusProcessSalaryMonth.Text = obj.BonusProcessSalaryMonth.ToString("yyyy-MM-dd");

        BonusProcessStdRate.Text = obj.BonusProcessStdRate.ToString("0.0000");

        // Part2
        BonusProcessRank1.Text = obj.BonusProcessRank1.ToString("0.00");
        BonusProcessRank2.Text = obj.BonusProcessRank2.ToString("0.00");
        BonusProcessRank3.Text = obj.BonusProcessRank3.ToString("0.00");
        BonusProcessRank4.Text = obj.BonusProcessRank4.ToString("0.00");
        BonusProcessRank5.Text = obj.BonusProcessRank5.ToString("0.00");

        // load Part1 count
        DBFilter m_countFilter = new DBFilter();
        m_countFilter.add(new Match("BonusProcessID", CurID));
        m_countFilter.add(new Match("EmpBonusProcessType", "S"));
        Part1DataCount.Text = EEmpBonusProcess.db.count(dbConn, m_countFilter).ToString("0");

        // load Part2 count
        m_countFilter = new DBFilter();
        m_countFilter.add(new Match("BonusProcessID", CurID));
        m_countFilter.add(new Match("EmpBonusProcessType", "D"));
        Part2DataCount.Text = EEmpBonusProcess.db.count(dbConn, m_countFilter).ToString("0");

        // control visibility of commands 
        if (obj.BonusProcessStatus != EBonusProcess.STATUS_NORMAL)
        {
            BasicInfoCommands.Visible = false;
            Part1Commands.Visible = false;
            Part2Commands.Visible = false;

            toolBar.EditButton_Visible = false;
            toolBar.DeleteButton_Visible = false;
            btnConfirmAndSeal.Visible = false;
        }

        return true;
    }


    protected void btnGenerateCND_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EBonusProcess m_process = EBonusProcess.GetObject(dbConn, CurID);
        if (m_process != null)
        {
            HROne.Import.ImportBonusProcess m_import = new HROne.Import.ImportBonusProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);

            DataSet dataSet = m_import.GenerateCND();

            string exportFileName = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(exportFileName);
            exportFileName += ".xls";
            HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, "CND_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        }
        else
            errors.addError("Failed to open batch");
    }

    protected void btnImportPart1Template_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/SelectFile_For_Import.aspx?Process=BonusProcess&PID=" + CurID.ToString("0") + "&p1=S");
    }

    protected void btnExportPart1Template_Click(object sender, EventArgs e)
    {       
        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
        DataSet dataSet = new DataSet();// export.GetDataSet();

        HROne.Import.ImportBonusProcess m_importProcess = new HROne.Import.ImportBonusProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);
        DataTable m_table = m_importProcess.ExportStandardBonusTemplate(true);

        if (m_table != null)
        {
            dataSet.Tables.Add(m_table);
            export.Update(dataSet);
            WebUtils.TransmitFile(Response, exportFileName, m_table.TableName + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
        }
    }

    protected void btnGeneratePart1Data_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/SelectEmployee_List.aspx?Process=BonusProcess&PID=" + CurID.ToString("0") + "&p1=S");
    }

    protected void btnImportPart2Template_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/SelectFile_For_Import.aspx?Process=BonusProcess&PID=" + CurID.ToString("0") + "&p1=D");
    }

    protected void btnExportPart2Template_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/SelectEmployee_List.aspx?Process=BonusProcess&PID=" + CurID.ToString("0") + "&p1=D");
    }    

    protected void Edit_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_Edit.aspx?BonusProcessID=" + CurID);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        
        EBonusProcess o = new EBonusProcess();
        o.BonusProcessID = CurID;

        if (db.select(dbConn, o))
        {
            if (o.BonusProcessStatus != EBonusProcess.STATUS_NORMAL)
            {
                errors.addError("Status must be Normal");
            }
            else
            {
                o.BonusProcessStatus = EBonusProcess.STATUS_CANCELLED;
                if (EBonusProcess.db.update(dbConn, o))
                    errors.addError("Update Completed");
                else
                    errors.addError("Update failed");
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_List.aspx");
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_BonusProcess_List.aspx");
    }

    protected void btnClearPart1Data_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        HROne.Import.ImportBonusProcess m_importProcess = new HROne.Import.ImportBonusProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);
        if (m_importProcess.ClearUploadedStandardData(errors))
        {
            errors.addError("Uploaded records cleared");
        }
        loadObject();
    }

    protected void btnClearPart2Data_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        HROne.Import.ImportBonusProcess m_importProcess = new HROne.Import.ImportBonusProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);
        if (m_importProcess.ClearUploadedDiscretionaryData(errors))
        {
            errors.addError("Uploaded records cleared");
        }
        loadObject();
    }

    protected void btnConfirmProcess_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        HROne.Import.ImportBonusProcess m_importProcess = new HROne.Import.ImportBonusProcess(dbConn, Session.SessionID, WebUtils.GetCurUser(Session).UserID, CurID);
        if (!m_importProcess.ConfirmBonusProcess(errors))
        {
            errors.addError("Confirm failed, please note that Bonus Process must be " + EBonusProcess.STATUS_NORMAL_DESC + " to be confirmed");
        }else
        {
            errors.addError("Bonus Process confirmed! It is locked and not allow for further operations");
        }


        loadObject();
    }

}
