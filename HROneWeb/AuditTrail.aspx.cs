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

public partial class AuditTrail : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS999";
    private DBManager db = EAuditTrail.db;
    private SearchBinding binding;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        binding = new SearchBinding(dbConn, db);
        //binding.add(new FieldDateRangeSearchBinder(new HtmlInputText(CreateDateFrom.Value), new HtmlInputText(CreateDateTo.Value), "CreateDate").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(Position, "epi.PositionID", EPosition.VLPosition));
        binding.init(DecryptedRequest, null);


        if (!Page.IsPostBack)
        {
            DBFilter auditTrailFunctionIDFilter = new DBFilter();
            auditTrailFunctionIDFilter.add(new IN("FunctionID", "Select Distinct FunctionID from " + EAuditTrail.db.dbclass.tableName, new DBFilter()));
            WebFormUtils.loadValues(dbConn, FunctionID, ESystemFunction.VLSystemFunction, auditTrailFunctionIDFilter, ci, null, null);

            DBFilter auditTrailUserIDFilter = new DBFilter();
            auditTrailUserIDFilter.add(new IN("UserID", "Select Distinct UserID from " + EAuditTrail.db.dbclass.tableName, new DBFilter()));
            WebFormUtils.loadValues(dbConn, UserID, EUser.VLUserName, auditTrailUserIDFilter, ci, null, null);
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        chkShowHeaderOnly.Attributes.Add("onclick",
            chkShowKeyIDOnly.ClientID + ".disabled=" + chkShowHeaderOnly.ClientID + ".checked" + ";"
            + chkDoNotConvertID.ClientID + ".disabled=" + chkShowHeaderOnly.ClientID + ".checked"
             + " || " + chkShowKeyIDOnly.ClientID + ".checked" + ";"
            );

        chkShowKeyIDOnly.Attributes.Add("onclick",
            chkDoNotConvertID.ClientID + ".disabled=" + chkShowHeaderOnly.ClientID + ".checked"
             + " || " + chkShowKeyIDOnly.ClientID + ".checked" + ";"
            );
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);

        string functionList = string.Empty;

        foreach (ListItem item in FunctionID.Items)
            if (item.Selected)
                if (item.Value.Equals(string.Empty))
                {
                    functionList = string.Empty;
                    break;
                }
                else
                {
                    if (string.IsNullOrEmpty(functionList))
                        functionList = item.Value;
                    else
                        functionList += "," + item.Value;

                }
        string userList = string.Empty;
        foreach (ListItem item in UserID.Items)
            if (item.Selected)
                if (item.Value.Equals(string.Empty))
                {
                    userList = null;
                    break;
                }
                else
                {
                    if (string.IsNullOrEmpty(userList))
                        userList = item.Value;
                    else
                        userList += "," + item.Value;
                }
        DateTime dtPeriodFr, dtPeriodTo;
        if (!DateTime.TryParse(CreateDateFrom.Value, out dtPeriodFr))
            dtPeriodFr = new DateTime();
        if (!DateTime.TryParse(CreateDateTo.Value, out dtPeriodTo))
            dtPeriodTo = new DateTime();

        HROneConfig config = HROneConfig.GetCurrentHROneConfig();
        if (config.GenerateReportAsInbox)
        {
            if (EInboxAttachment.GetTotalSize(dbConn, 0) < WebUtils.productLicense(Session).MaxInboxSizeMB * 1000 * 1000)
            {
                HROne.Lib.AuditTrailReportProcess process = new HROne.Lib.AuditTrailReportProcess(dbConn, null, functionList, userList, dtPeriodFr, dtPeriodTo, EmpNo.Text.Trim(), chkShowHeaderOnly.Checked, chkShowKeyIDOnly.Checked, chkDoNotConvertID.Checked, chkShowWithoutDataUpdate.Checked);
                HROne.TaskService.AuditTrailTaskFactory task = new HROne.TaskService.AuditTrailTaskFactory(dbConn, user, lblReportHeader.Text, process);
                AppUtils.reportTaskQueueService.AddTask(task);
                errors.addError(HROne.Translation.PageMessage.REPORT_GENERATING_TO_INBOX);
            }
            else
                errors.addError(HROne.Translation.PageMessage.INBOX_SIZE_EXCEEDED);

        }
        else
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/download"; //Fixed download problem on https
            Response.AddHeader("Content-Disposition", "attachment;filename=AuditTrail_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".txt");
            Response.Expires = -1;

            HROne.Lib.AuditTrailReportProcess process = new HROne.Lib.AuditTrailReportProcess(dbConn, Response.Output, functionList, userList, dtPeriodFr, dtPeriodTo, EmpNo.Text.Trim(), chkShowHeaderOnly.Checked, chkShowKeyIDOnly.Checked, chkDoNotConvertID.Checked, chkShowWithoutDataUpdate.Checked);
            process.Updated += new EventHandler(OnProcessUpdated);
            process.GenerateToFile();

            Response.End();
        }
    }

    protected void OnProcessUpdated(object sender, EventArgs args)
    {
        Response.Flush();
        if (!Response.IsClientConnected)
            ((HROne.Lib.AuditTrailReportProcess)sender).Close();
    }
}
