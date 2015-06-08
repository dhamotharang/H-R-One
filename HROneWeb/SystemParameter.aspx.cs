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
using HROne.Translation;
using HROne.Lib.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Common;
public partial class SystemParameter : HROneWebPage
{
    private const string FUNCTION_CODE = "SYS000";

    private DBManager db = ESystemParameter.db;
    private const string PASSWORD_NO_CHANGE_KEYWORD = "%PASSWORD_NO_CHANGE%";

    public const double DEFAULT_MAX_MONTHLY_LSPSP_AMOUNT = 22500;
    public const double DEFAULT_MAX_TOTAL_LSPSP_AMOUNT = 390000;
    protected HROne.ProductLicense productLicense;

    //string[] fontList = new string[]{
    //        "MingLiU_HKSCS", "²Ó©úÅé_HKSCS",
    //        "PMingLiU", "·s²Ó©úÅé",
    //        "MingLiU", "²Ó©úÅé",
    //        "MingLiU", "²Ó©úÅé",
    //        "DFKai-SB", "¼Ð·¢Åé",
    //        "Microsoft JhengHei", "·L³n¥¿¶ÂÅé",
    //        "Arial Unicode MS"
    //    };

    string[] fontList = new string[]{
            "MingLiU_HKSCS", "¼šÃ÷ów_HKSCS",
            "PMingLiU", "ÐÂ¼šÃ÷ów",
            "MingLiU", "¼šÃ÷ów",
            "MingLiU", "¼šÃ÷ów",
            "DFKai-SB", "˜Ë¿¬ów",
            "Microsoft JhengHei", "Î¢Ü›ÕýºÚów",
            "Arial Unicode MS"
        };

    protected void Page_Init(object sender, EventArgs e)
    {
        ddbPARAM_CODE_REPORT_CHINESE_FONT.Items.Add(new ListItem(string.Empty, string.Empty));
        foreach (string fontName in fontList)
        {
            System.Drawing.FontFamily chineseFontFamily = null;
            try
            {
                chineseFontFamily = new System.Drawing.FontFamily(fontName);
            }
            catch { }
            if (chineseFontFamily != null)
            {
                if (ddbPARAM_CODE_REPORT_CHINESE_FONT.Items.FindByValue(chineseFontFamily.Name) == null)
                    ddbPARAM_CODE_REPORT_CHINESE_FONT.Items.Add(new ListItem(chineseFontFamily.GetName(0), chineseFontFamily.Name));
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        productLicense = WebUtils.productLicense(Session);
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            Save.Visible = false;
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        lblDefault_MAX_MONTHLY_LSPSP_AMOUNT.Text = DEFAULT_MAX_MONTHLY_LSPSP_AMOUNT.ToString();
        lblDefault_MAX_TOTAL_LSPSP_AMOUNT.Text = DEFAULT_MAX_TOTAL_LSPSP_AMOUNT.ToString();

        HROne.ProductVersion.Database database = new HROne.ProductVersion.Database(dbConn);

        SYSTEM_APP_VERSION.Text = ProductVersion.CURRENT_PROGRAM_VERSION;
        SYSTEM_DB_VERSION.Text = HROne.ProductVersion.Database.CURRENT_DB_VERSION;
        RUNNING_DB_VERSION.Text = database.RunningDatabaseVersion();
        PRODUCT_TYPE.Text = productLicense.ProductType.ToString();
        MAX_COMPANY.Text = productLicense.NumOfCompanies.ToString();
        MAX_USERS.Text = productLicense.NumOfUsers.ToString();
        if (productLicense.MaxInboxSizeMB.Equals(uint.MaxValue))
            MAX_INBOX_SIZE_MB.Text = HROne.Common.WebUtility.GetLocalizedString("Unlimited");
        else
            MAX_INBOX_SIZE_MB.Text = productLicense.MaxInboxSizeMB.ToString();

        if (productLicense.NumOfEmployees.Equals(uint.MaxValue))
            MAX_EMPLOYEES.Text = HROne.Common.WebUtility.GetLocalizedString("Unlimited");
        else
            MAX_EMPLOYEES.Text = productLicense.NumOfEmployees.ToString();

        lblActiveUser.Text = WebUtils.TotalActiveUser(dbConn, 0).ToString();

        lblActiveCompany.Text = WebUtils.TotalActiveCompany(dbConn, 0).ToString();

        lblActiveEmployee.Text = WebUtils.TotalActiveEmployee(dbConn, 0).ToString();
        lblTotalInboxSize.Text = ((double)EInboxAttachment.GetTotalSize(dbConn, 0) / 1000.0 / 1000.0).ToString("0.000");

        if (!productLicense.IsValidAuthorizationCode())
        {
            TrialPeriodPanel.Visible = true;
            TRIAL_PERIOD_TO.Text = productLicense.LastTrialDate.ToString("yyyy-MM-dd");
        }
        else
            TrialPeriodPanel.Visible = false;
        if (!Page.IsPostBack)
        {
            WebUtils.AddLanguageOptionstoDropDownList(cbxPARAM_CODE_DEFAULT_LANGUAGE);
            WebUtils.AddLanguageOptionstoDropDownList(cbxPARAM_CODE_ESS_DEFAULT_LANGUAGE);

            cbxPARAM_CODE_DEFAULT_LANGUAGE.SelectedValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_LANGUAGE);
            cbxPARAM_CODE_ESS_DEFAULT_LANGUAGE.SelectedValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEFAULT_LANGUAGE);
            cbxPARAM_CODE_DEFAULT_RECORDS_PER_PAGE.SelectedValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_RECORDS_PER_PAGE);
            txtPARAM_CODE_DB_TITLE.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DB_TITLE);
            txtPARAM_CODE_DOCUMENT_UPLOAD_FOLDER.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DOCUMENT_UPLOAD_FOLDER);
            if (string.IsNullOrEmpty(txtPARAM_CODE_DOCUMENT_UPLOAD_FOLDER.Text))
            {
                txtPARAM_CODE_DOCUMENT_UPLOAD_FOLDER.Text = HROne.CommonLib.FileIOProcess.DefaultUploadFolder();
            }


            txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_LOGIN_MAX_FAIL_COUNT);
            txtPARAM_CODE_SESSION_TIMEOUT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SESSION_TIMEOUT);

            chkPARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
			// Start 0000044, Miranda, 2014-05-09
            chkPARAM_CODE_EMP_NO_AUTO_GENERATE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_NO_AUTO_GENERATE).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            txtPARAM_CODE_EMP_NO_FORMAT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_NO_FORMAT);
			// End 0000044, Miranda, 2014-05-09

            cbxPARAM_CODE_USE_ORSO.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_USE_ORSO).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            txtPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT);
            txtPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT);
            cbxPARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            cbxPARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            txtPARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH);

            ddbPARAM_CODE_REPORT_CHINESE_FONT.SelectedValue = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_REPORT_CHINESE_FONT);

            chkPARAM_CODE_TAXATION_USE_CHINESE_NAME.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_TAXATION_USE_CHINESE_NAME).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;

            txtPARAM_CODE_SMTP_SERVER_NAME.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SERVER_NAME);
            txtPARAM_CODE_SMTP_PORT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PORT);
            chkPARAM_CODE_SMTP_ENABLE_SSL.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_ENABLE_SSL).Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? true : false;
            txtPARAM_CODE_SMTP_USERNAME.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_USERNAME);
            txtPARAM_CODE_SMTP_PASSWORD.Attributes.Add("value", PASSWORD_NO_CHANGE_KEYWORD);//ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PASSWORD);
            txtPARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS);

            chkPARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_CANCEL_LEAVE_APPLICATION.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CANCEL_LEAVE_APPLICATION).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_LEAVE_HISTORY.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_HISTORY).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_REPORT.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_REPORT).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION_LIST.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION_LIST).Equals("Y", StringComparison.CurrentCultureIgnoreCase);            
            chkPARAM_CODE_ESS_FUNCTION_ROSTER_TABLE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ROSTER_TABLE).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_PRINT_PAYSLIP.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_PRINT_PAYSLIP).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_PRINT_TAXREPORT.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_PRINT_TAXREPORT).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            // Start 0000060, Miranda, 2014-07-22
            if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM))
            {
                chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            }
            // End 0000060, Miranda, 2014-07-22
            // Start 0000112, Miranda, 2014-12-10
            if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE))
            {
                chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LATE_WAIVE).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
                chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            }
                // End 0000112, Miranda, 2014-12-10
            // Start 0000057, KuangWei, 2014-07-08  
            chkPARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            // End 0000057, KuangWei, 2014-07-08
            // Start 0000076, Miranda, 2014-08-21
            chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            // End 0000076, Miranda, 2014-08-21
            txtPARAM_CODE_ESS_LEAVE_MEDICIAL_CERT_ALERT.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_LEAVE_MEDICIAL_CERT_ALERT);

            wdpPARAM_CODE_ESS_PAYSLIP_START_DATE.Value = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_START_DATE);
            chkPARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            wdpPARAM_CODE_ESS_LEAVE_HISTORY_START_DATE.Value = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_LEAVE_HISTORY_START_DATE);
            // Start 0000060, Miranda, 2014-07-22
            //wdpPARAM_CODE_ESS_DEF_EOT_EXPIRY_DATE.Value = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY_DATE);

            string essDefEotExpiry = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY);
            txtPARAM_CODE_ESS_DEF_EOT_EXPIRY.Text = string.IsNullOrEmpty(essDefEotExpiry) ? "1" : essDefEotExpiry;
            WebUtils.AddYearMonthDayOptionstoDropDownList(cbxPARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE);
            string essDefEotExpiryType = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE);
            cbxPARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE.SelectedValue = string.IsNullOrEmpty(essDefEotExpiryType) ? "M" : essDefEotExpiryType;// "M" means Month
            // End 0000060, Miranda, 2014-07-22

            chkPARAM_CODE_EMP_LIST_SHOW_COMPANY.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_COMPANY).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_EMP_LIST_SHOW_H1.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H1).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_EMP_LIST_SHOW_H2.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H2).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            chkPARAM_CODE_EMP_LIST_SHOW_H3.Checked = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H3).Equals("Y", StringComparison.CurrentCultureIgnoreCase);
        }

        if (productLicense.IsESS)
            PanelESSSection.Visible = true;
        else
            PanelESSSection.Visible = false;

        if (productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.HROne)
        {
            DBTitleRow.Visible = true;
            BOCIEncryptPathSettingRow.Visible = true;
            DocumentUploadPathSettingRow.Visible = true;
            //ORSOSettingRow.Visible = true;
            PanelSMTPSection.Visible = true;
        }
        else
        {
            DBTitleRow.Visible = false;
            BOCIEncryptPathSettingRow.Visible = false;
            DocumentUploadPathSettingRow.Visible = false;
            //ORSOSettingRow.Visible = false;
            PanelSMTPSection.Visible = false;
        }

        eot_row.Visible = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM);

        this.chkPARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD.Enabled = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_TIMECARD_RECORD);
        this.lblEnabledTimeCardRecord.Visible = !this.chkPARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD.Enabled;
        
        this.chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST.Enabled = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_ATTENDANCE_TIMEENTRY_LIST);
        this.lblEnabledAttendanceTimeEntryList.Visible = !this.chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST.Enabled;
        
        this.chkPARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT.Enabled = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_MONTHLY_ATTENDANCE_REPORT);
        this.lblEnabledMonthlyAttendanceReport.Visible = !this.chkPARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT.Enabled;

        this.chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS.Enabled = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM);
        this.chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY.Enabled = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM);
        this.lblEnabledOTClaims.Visible = !this.chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS.Enabled;
        this.lblEnabledOTClaimsHistory.Visible = !this.chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY.Enabled;
        // Start 000112, Ricky So, 2014/12/18
        this.chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE.Enabled = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE);
        this.chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY.Enabled = ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE);
        this.lblEnabledLateWaive.Visible = !this.chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE.Enabled;
        this.lblEnabledLateWaiveHistory.Visible = !this.chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY.Enabled;
        // End 000112, Ricky So, 2014/12/18
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        if (txtPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT.Text.Equals(string.Empty))
            txtPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT.Text = DEFAULT_MAX_MONTHLY_LSPSP_AMOUNT.ToString();

        if (txtPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT.Text.Equals(string.Empty))
            txtPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT.Text = DEFAULT_MAX_TOTAL_LSPSP_AMOUNT.ToString();


        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        int testInteger;
        double testDouble;
        if (!txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text.Equals(string.Empty) && !int.TryParse(txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text, out testInteger))
            errors.addError(WebUtility.GetLocalizedStringByCode("validate.int.prompt", "").Replace("{0}", lblPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text));
        if (!txtPARAM_CODE_SESSION_TIMEOUT.Text.Equals(string.Empty) && !int.TryParse(txtPARAM_CODE_SESSION_TIMEOUT.Text, out testInteger))
            errors.addError(WebUtility.GetLocalizedStringByCode("validate.int.prompt", "").Replace("{0}", lblPARAM_CODE_SESSION_TIMEOUT.Text));

        if (!double.TryParse(txtPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT.Text, out testDouble))
            errors.addError(WebUtility.GetLocalizedStringByCode("validate.int.prompt", "").Replace("{0}", lblPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT.Text));
        if (!double.TryParse(txtPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT.Text, out testDouble))
            errors.addError(WebUtility.GetLocalizedStringByCode("validate.int.prompt", "").Replace("{0}", lblPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT.Text));

        string strBOCIEncryptPath = txtPARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH.Text;
        if (!string.IsNullOrEmpty(strBOCIEncryptPath))
        {
            string errorMessage;
            if (!HROne.CommonLib.FileIOProcess.IsFileExists(strBOCIEncryptPath, true, out errorMessage))
                errors.addError(errorMessage);
        }

        string strDocumentPath = txtPARAM_CODE_DOCUMENT_UPLOAD_FOLDER.Text;
        if (string.IsNullOrEmpty(strDocumentPath))
            strDocumentPath = HROne.CommonLib.FileIOProcess.DefaultUploadFolder();
        string strOldDocumentPath = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_DOCUMENT_UPLOAD_FOLDER);
        if (string.IsNullOrEmpty(strOldDocumentPath))
            strOldDocumentPath = HROne.CommonLib.FileIOProcess.DefaultUploadFolder();

        if (!strDocumentPath.Equals(strOldDocumentPath))
        {
            if (!string.IsNullOrEmpty(strDocumentPath))
            {
                string errorMessage;
                if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(strDocumentPath, out errorMessage))
                    errors.addError(errorMessage);
                else
                    if (!string.IsNullOrEmpty(strOldDocumentPath))
                    {
                        if (HROne.CommonLib.FileIOProcess.IsFolderExists(strOldDocumentPath, out errorMessage))
                        {
                            if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(strOldDocumentPath, out errorMessage))
                                errors.addError(errorMessage);
                        }
                        else
                            strOldDocumentPath = string.Empty;
                    }
            }
            if (!strOldDocumentPath.Equals(string.Empty))
                HROne.CommonLib.FileIOProcess.MoveDocument(strOldDocumentPath, strDocumentPath);
        }

        if (!SystemParameterCustomizationControl.IsValidEntry())
        {
            errors.addError(SystemParameterCustomizationControl.ErrorMessage);
        }
        if (errors.isEmpty())
        {
            WebUtils.StartFunction(Session, FUNCTION_CODE);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_LANGUAGE, cbxPARAM_CODE_DEFAULT_LANGUAGE.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEFAULT_LANGUAGE, cbxPARAM_CODE_ESS_DEFAULT_LANGUAGE.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DEFAULT_RECORDS_PER_PAGE, cbxPARAM_CODE_DEFAULT_RECORDS_PER_PAGE.SelectedValue);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DB_TITLE, txtPARAM_CODE_DB_TITLE.Text );
            if (txtPARAM_CODE_DOCUMENT_UPLOAD_FOLDER.Text.Equals(AppUtils.GetDocumentUploadFolder(dbConn)))
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DOCUMENT_UPLOAD_FOLDER, string.Empty);
            else
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_DOCUMENT_UPLOAD_FOLDER, txtPARAM_CODE_DOCUMENT_UPLOAD_FOLDER.Text);


            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_LOGIN_MAX_FAIL_COUNT, txtPARAM_CODE_LOGIN_MAX_FAIL_COUNT.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SESSION_TIMEOUT, txtPARAM_CODE_SESSION_TIMEOUT.Text);

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE, chkPARAM_CODE_HKID_CHECKDIGIT_AUTO_GENERATE.Checked ? "Y" : "N");
			// Start 0000044, Miranda, 2014-05-09
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_NO_AUTO_GENERATE, chkPARAM_CODE_EMP_NO_AUTO_GENERATE.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_NO_FORMAT, txtPARAM_CODE_EMP_NO_FORMAT.Text);
			// End 0000044, Miranda, 2014-05-09

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_USE_ORSO, cbxPARAM_CODE_USE_ORSO.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT, txtPARAM_CODE_PAYROLL_MAX_MONTHLY_LSPSP_AMOUNT.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT, txtPARAM_CODE_PAYROLL_MAX_TOTAL_LSPSP_AMOUNT.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE, cbxPARAM_CODE_PAY_SLIP_HIDE_LEAVE_BALANCE.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO, cbxPARAM_CODE_PAY_SLIP_HIDE_MINIMUM_WAGE_INFO.Checked ? "Y" : "N");
            
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH, txtPARAM_CODE_MPF_FILE_BOCI_ENCRYPT_PATH.Text);

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_REPORT_CHINESE_FONT, ddbPARAM_CODE_REPORT_CHINESE_FONT.SelectedValue);


            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_TAXATION_USE_CHINESE_NAME, chkPARAM_CODE_TAXATION_USE_CHINESE_NAME.Checked ? "Y" : "N");

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SERVER_NAME, txtPARAM_CODE_SMTP_SERVER_NAME.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_PORT, txtPARAM_CODE_SMTP_PORT.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_ENABLE_SSL, chkPARAM_CODE_SMTP_ENABLE_SSL.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_USERNAME, txtPARAM_CODE_SMTP_USERNAME.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS, txtPARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS.Text);
            if (!txtPARAM_CODE_SMTP_PASSWORD.Text.Equals(PASSWORD_NO_CHANGE_KEYWORD))
                ESystemParameter.setParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_SMTP_PASSWORD, txtPARAM_CODE_SMTP_PASSWORD.Text);

            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_COMPANY, chkPARAM_CODE_EMP_LIST_SHOW_COMPANY.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H1, chkPARAM_CODE_EMP_LIST_SHOW_H1.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H2, chkPARAM_CODE_EMP_LIST_SHOW_H2.Checked ? "Y" : "N");
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_EMP_LIST_SHOW_H3, chkPARAM_CODE_EMP_LIST_SHOW_H3.Checked ? "Y" : "N");

            if (productLicense.IsESS)
            {
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO, chkPARAM_CODE_ESS_FUNCTION_CHANGE_EE_INFO.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION, chkPARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_CANCEL_LEAVE_APPLICATION, chkPARAM_CODE_ESS_FUNCTION_CANCEL_LEAVE_APPLICATION.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_HISTORY, chkPARAM_CODE_ESS_FUNCTION_LEAVE_HISTORY.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY, chkPARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_ENQUIRY.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_REPORT, chkPARAM_CODE_ESS_FUNCTION_LEAVE_BALANCE_REPORT.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION_LIST, chkPARAM_CODE_ESS_FUNCTION_LEAVE_APPLICATION_LIST.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ROSTER_TABLE, chkPARAM_CODE_ESS_FUNCTION_ROSTER_TABLE.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_PRINT_PAYSLIP, chkPARAM_CODE_ESS_FUNCTION_PRINT_PAYSLIP.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_PRINT_TAXREPORT, chkPARAM_CODE_ESS_FUNCTION_PRINT_TAXREPORT.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY, chkPARAM_CODE_ESS_FUNCTION_OVERALL_PAYMENT_SUMMARY.Checked ? "Y" : "N");
                // Start 0000060, Miranda, 2014-07-22
                if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_OTCLAIM))
                {
                    ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS, chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS.Checked ? "Y" : "N");
                    ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY, chkPARAM_CODE_ESS_FUNCTION_OT_CLAIMS_HISTORY.Checked ? "Y" : "N");
                }
                // End 0000060, Miranda, 2014-07-22
                // Start 0000112, Miranda, 2014-12-10
                if (ESystemParameter.IsEnabled(dbConn, ESystemParameter.PARAM_CODE_ENABLE_LATE_WAIVE))
                {                
                    ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LATE_WAIVE, chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE.Checked ? "Y" : "N");
                    ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY, chkPARAM_CODE_ESS_FUNCTION_LATE_WAIVE_HISTORY.Checked ? "Y" : "N");
                }
                // End 0000112, Miranda, 2014-12-10
                // Start 0000057, KuangWei, 2014-07-07     
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT, chkPARAM_CODE_ESS_FUNCTION_MONTHLY_ATTENDANCE_REPORT.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD, chkPARAM_CODE_ESS_FUNCTION_TIMECARD_RECORD.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST, chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_LIST.Checked ? "Y" : "N");
                // End 0000057, KuangWei, 2014-07-07
                // Start 0000076, Miranda, 2014-08-21
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT, chkPARAM_CODE_ESS_FUNCTION_ATTENDANCE_TIMEENTRY_REPORT.Checked ? "Y" : "N");
                // End 0000076, Miranda, 2014-08-21
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_LEAVE_MEDICIAL_CERT_ALERT, txtPARAM_CODE_ESS_LEAVE_MEDICIAL_CERT_ALERT.Text);
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_START_DATE, wdpPARAM_CODE_ESS_PAYSLIP_START_DATE.Value);
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE, chkPARAM_CODE_ESS_PAYSLIP_AUTO_RELEASE.Checked ? "Y" : "N");
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_LEAVE_HISTORY_START_DATE, wdpPARAM_CODE_ESS_LEAVE_HISTORY_START_DATE.Value);
                // Start 0000060, Miranda, 2014-07-22
                //ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY_DATE, wdpPARAM_CODE_ESS_DEF_EOT_EXPIRY_DATE.Value);
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY, txtPARAM_CODE_ESS_DEF_EOT_EXPIRY.Text);
                ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE, cbxPARAM_CODE_ESS_DEF_EOT_EXPIRY_TYPE.SelectedValue);
                //End 0000060, Miranda, 2014-07-22
            }
            SystemParameterCustomizationControl.Save();
            WebUtils.EndFunction(dbConn);
            WebUtils.SetSessionLanguage(Session, WebUtils.GetCurUser(Session));
            HROne.Common.WebUtility.initLanguage(Session);

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
