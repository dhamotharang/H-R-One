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
using HROne.SaaS.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Common;
public partial class SystemParameter : HROneWebPage
{
    private const string FUNCTION_CODE = "ADM004";

    private DBManager db = ESystemParameter.db;
    private const string PASSWORD_NO_CHANGE_KEYWORD = "%PASSWORD_NO_CHANGE%";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        HROne.ProductVersion.Database database = new HROne.ProductVersion.Database(dbConn);

        SYSTEM_DB_VERSION.Text = ProductVersion.CURRENT_DB_VERSION;
        RUNNING_DB_VERSION.Text = database.RunningDatabaseVersion();
        if (!Page.IsPostBack)
        {

            cbxPARAM_CODE_DEFAULT_RECORDS_PER_PAGE.SelectedValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_RECORDS_PER_PAGE);

            txtPARAM_CODE_DEFAULT_DOCUMENT_FOLDER.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_DOCUMENT_FOLDER);
            txtPARAM_CODE_BANKFILE_UPLOAD_FOLDER.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);
            txtPARAM_CODE_HSBC_MRI_DIRECTORY.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY);
            txtPARAM_CODE_BANKFILE_CUTOFF_TIME.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_CUTOFF_TIME);
            txtPARAM_CODE_BANKFILE_LAST_CANCEL_TIME.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_LAST_CANCEL_TIME);


            txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_LOGIN_MAX_FAIL_COUNT);
            txtPARAM_CODE_SESSION_TIMEOUT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SESSION_TIMEOUT);

            txtPARAM_CODE_SMTP_SERVER_NAME.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SERVER_NAME);
            txtPARAM_CODE_SMTP_PORT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PORT);
            chkPARAM_CODE_SMTP_ENABLE_SSL.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_ENABLE_SSL).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            txtPARAM_CODE_SMTP_USERNAME.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_USERNAME);
            txtPARAM_CODE_SMTP_PASSWORD.Attributes.Add("value", PASSWORD_NO_CHANGE_KEYWORD);//ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PASSWORD);
            txtPARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS);

            txtPARAM_CODE_DEFAULT_MAX_INBOX_SIZE_MB.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_MAX_INBOX_SIZE_MB);
        }


    }
    protected void Save_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        int testInteger;
        double testDouble;
        if (!txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text.Equals(string.Empty) && !int.TryParse(txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text, out testInteger))
            errors.addError(WebUtility.GetLocalizedStringByCode("validate.int.prompt", "").Replace("{0}", HROne.Common.WebUtility.GetLocalizedStringByCode("PARAM_CODE_LOGIN_MAX_FAIL_COUNT", "PARAM_CODE_LOGIN_MAX_FAIL_COUNT")));
        if (!txtPARAM_CODE_SESSION_TIMEOUT.Text.Equals(string.Empty) && !int.TryParse(txtPARAM_CODE_SESSION_TIMEOUT.Text, out testInteger))
            errors.addError(WebUtility.GetLocalizedStringByCode("validate.int.prompt", "").Replace("{0}", HROne.Common.WebUtility.GetLocalizedStringByCode("PARAM_CODE_SESSION_TIMEOUT", "PARAM_CODE_SESSION_TIMEOUT")));

        {
            string strBankFilePath = txtPARAM_CODE_BANKFILE_UPLOAD_FOLDER.Text;
            //if (string.IsNullOrEmpty(strDocumentPath))
            //    strDocumentPath = HROne.CommonLib.FileIOProcess.DefaultUploadFolder();
            string strOldBankFilePath = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER);

            if (!strBankFilePath.Equals(strOldBankFilePath))
            {
                if (!string.IsNullOrEmpty(strBankFilePath))
                {
                    string errorMessage;
                    if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(strBankFilePath, out errorMessage))
                        errors.addError(errorMessage);
                    else
                        if (!string.IsNullOrEmpty(strOldBankFilePath))
                        {
                            if (HROne.CommonLib.FileIOProcess.IsFolderExists(strOldBankFilePath, out errorMessage))
                            {
                                if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(strOldBankFilePath, out errorMessage))
                                    errors.addError(errorMessage);
                            }
                            else
                                strOldBankFilePath = string.Empty;
                        }
                }
                if (!strOldBankFilePath.Equals(string.Empty))
                    HROne.CommonLib.FileIOProcess.MoveDocument(strOldBankFilePath, strBankFilePath);
            }
        }
        {
            string strDefaultDocumentPath = txtPARAM_CODE_DEFAULT_DOCUMENT_FOLDER.Text;
            //if (string.IsNullOrEmpty(strDocumentPath))
            //    strDocumentPath = HROne.CommonLib.FileIOProcess.DefaultUploadFolder();
            string strOlddDefaultDocumentPath = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_DOCUMENT_FOLDER);

            if (!strDefaultDocumentPath.Equals(strOlddDefaultDocumentPath))
            {
                if (!string.IsNullOrEmpty(strDefaultDocumentPath))
                {
                    string errorMessage;
                    if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(strDefaultDocumentPath, out errorMessage))
                        errors.addError(errorMessage);
                    else
                        if (!string.IsNullOrEmpty(strOlddDefaultDocumentPath))
                        {
                            if (HROne.CommonLib.FileIOProcess.IsFolderExists(strOlddDefaultDocumentPath, out errorMessage))
                            {
                                if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(strOlddDefaultDocumentPath, out errorMessage))
                                    errors.addError(errorMessage);
                            }
                            else
                                strOlddDefaultDocumentPath = string.Empty;
                        }
                }
                if (!strOlddDefaultDocumentPath.Equals(string.Empty))
                    HROne.CommonLib.FileIOProcess.MoveDocument(strOlddDefaultDocumentPath, strDefaultDocumentPath);
            }
        }
        if (!string.IsNullOrEmpty(txtPARAM_CODE_HSBC_MRI_DIRECTORY.Text))
        {
            string errorMessage;

            if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(txtPARAM_CODE_HSBC_MRI_DIRECTORY.Text, out errorMessage))
                errors.addError(errorMessage);
        }

        DateTime tmpTestTime = new DateTime();
        if (!DateTime.TryParseExact(txtPARAM_CODE_BANKFILE_CUTOFF_TIME.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out tmpTestTime))
            errors.addError("Invalid Time format");

        if (!DateTime.TryParseExact(txtPARAM_CODE_BANKFILE_LAST_CANCEL_TIME.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out tmpTestTime))
            errors.addError("Invalid Time format");

        if (errors.isEmpty())
        {
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_RECORDS_PER_PAGE, cbxPARAM_CODE_DEFAULT_RECORDS_PER_PAGE.SelectedValue);

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_UPLOAD_FOLDER, txtPARAM_CODE_BANKFILE_UPLOAD_FOLDER.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_DOCUMENT_FOLDER, txtPARAM_CODE_DEFAULT_DOCUMENT_FOLDER.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY, txtPARAM_CODE_HSBC_MRI_DIRECTORY.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_CUTOFF_TIME, txtPARAM_CODE_BANKFILE_CUTOFF_TIME.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_BANKFILE_LAST_CANCEL_TIME, txtPARAM_CODE_BANKFILE_LAST_CANCEL_TIME.Text);


            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_LOGIN_MAX_FAIL_COUNT, txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SESSION_TIMEOUT, txtPARAM_CODE_SESSION_TIMEOUT.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_MAX_INBOX_SIZE_MB, txtPARAM_CODE_DEFAULT_MAX_INBOX_SIZE_MB.Text);


            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SERVER_NAME, txtPARAM_CODE_SMTP_SERVER_NAME.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PORT, txtPARAM_CODE_SMTP_PORT.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_ENABLE_SSL, chkPARAM_CODE_SMTP_ENABLE_SSL.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_USERNAME, txtPARAM_CODE_SMTP_USERNAME.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS, txtPARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS.Text);
            if (!txtPARAM_CODE_SMTP_PASSWORD.Text.Equals(PASSWORD_NO_CHANGE_KEYWORD))
                ESystemParameter.setParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_SMTP_PASSWORD, txtPARAM_CODE_SMTP_PASSWORD.Text);

            errors.addError(WebUtility.GetLocalizedString("Updated Successful"));

        }

    }
    protected void btnTestEmail_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        int port;
        string password;

        if (!txtPARAM_CODE_SMTP_PASSWORD.Text.Equals(PASSWORD_NO_CHANGE_KEYWORD))
        {
            password = txtPARAM_CODE_SMTP_PASSWORD.Text;
            txtPARAM_CODE_SMTP_PASSWORD.Attributes.Add("value", password);//ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PASSWORD);
        }
        else
            password = ESystemParameter.getParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_SMTP_PASSWORD);


        if (!int.TryParse(txtPARAM_CODE_SMTP_PORT.Text, out port))
        {
            port = 0;
        }

        try
        {
            AppUtils.TestSendEmail(this.txtPARAM_CODE_SMTP_SERVER_NAME.Text, port, txtPARAM_CODE_SMTP_USERNAME.Text, password, txtPARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS.Text, txtTestEmailAddress.Text, chkPARAM_CODE_SMTP_ENABLE_SSL.Checked);
        }
        catch(Exception ex)
        {
            errors.addError(ex.Message.Replace("\r\n", " "));
            return;
        }
        errors.addError(HROne.Common.WebUtility.GetLocalizedString("Test Email has been sent. Please check email box."));
    }


    protected void ProductKey_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ProductKey.aspx");
    }
}
