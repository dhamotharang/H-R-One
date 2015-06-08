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
public partial class BankKeyManagement : HROneWebPage
{
    private const string FUNCTION_CODE = "ADM007";

    private DBManager db = ESystemParameter.db;
    private string HSBCMRICommandLineDirectory = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        HSBCMRICommandLineDirectory = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY);
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        if (string.IsNullOrEmpty(HSBCMRICommandLineDirectory))
        {
            errors.addError("MRI Command Line Folder has not been set");
            Renew.Visible = false;

        }
        else
        {
            string commandLinePath = System.IO.Path.Combine(HSBCMRICommandLineDirectory, "MRI2.exe");
            if (!System.IO.File.Exists(commandLinePath))
            {
                errors.addError("MRI2.exe does not exists on Command Line Folder");
                Renew.Visible = false;

            }
            else
            {
                HROne.ProductVersion.Database database = new HROne.ProductVersion.Database(dbConn);
                try
                {

                    HROne.HSBC.Utility.HSBCMRICommandLineDirectory = HSBCMRICommandLineDirectory;

                    string HSBCKeyPath = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PATH);
                    if (System.IO.File.Exists(HSBCKeyPath))
                    {
                        lblHSBCKeyID.Text = HROne.HSBC.Utility.getKeyId(HSBCKeyPath);
                        if (!HROne.HSBC.Utility.isKeyExpired(HSBCKeyPath))
                            lblHSBCKeyDaysExpired.Text = HROne.HSBC.Utility.keyExpiredLeftDays(HSBCKeyPath) + " Days";
                        else
                            lblHSBCKeyDaysExpired.Text = "Expired";
                    }

                    string HASEKeyPath = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HASE_PATH);
                    if (System.IO.File.Exists(HASEKeyPath))
                    {
                        lblHASEKeyID.Text = HROne.HSBC.Utility.getKeyId(HASEKeyPath);
                        if (!HROne.HSBC.Utility.isKeyExpired(HASEKeyPath))
                            lblHASEKeyDaysExpired.Text = HROne.HSBC.Utility.keyExpiredLeftDays(HASEKeyPath) + " Days";
                        else
                            lblHASEKeyDaysExpired.Text = "Expired";
                    }
                }
                catch (Exception ex)
                {
                    errors.addError(ex.Message);
                    Renew.Visible = false;
                }
            }
        }
    }
    protected void Renew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/SetupBankKey.aspx");
    }


}
