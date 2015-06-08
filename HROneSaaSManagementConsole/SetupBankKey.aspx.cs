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
public partial class SetupBankKey : HROneWebPage
{
    private const string FUNCTION_CODE = "ADM007";

    private DBManager db = ESystemParameter.db;
    private string HSBCMRICommandLineDirectory = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        HSBCMRICommandLineDirectory = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY);
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;

        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        if (string.IsNullOrEmpty(HSBCMRICommandLineDirectory))
        {
            errors.addError("MRI Command Line Folder has not been set");
            Save.Visible = false;

        }
        else
        {
            string commandLinePath = System.IO.Path.Combine(HSBCMRICommandLineDirectory, "MRI2.exe");
            if (!System.IO.File.Exists(commandLinePath))
            {
                errors.addError("MRI2.exe does not exists on Command Line Folder");
                Save.Visible = false;

            }
            else
            {
                HROne.HSBC.Utility.HSBCMRICommandLineDirectory = HSBCMRICommandLineDirectory;
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        //if (!Page.IsPostBack)
        //{
        //    txtPARAM_CODE_BANKKEY_HSBC_PATH.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PATH);
        //    txtPARAM_CODE_BANKKEY_HSBC_PASSWORD.Text = ESystemParameter.getParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PASSWORD);

        //    txtPARAM_CODE_BANKKEY_HASE_PATH.Text = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HASE_PATH);
        //    txtPARAM_CODE_BANKKEY_HASE_PASSWORD.Text = ESystemParameter.getParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HASE_PASSWORD);

        //}


    }
    protected void Save_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);

        if (!System.IO.File.Exists(txtPARAM_CODE_BANKKEY_HSBC_PATH.Text))
            errors.addError("File not found: " + txtPARAM_CODE_BANKKEY_HSBC_PATH.Text);
        else if (HROne.HSBC.Utility.isKeyExpired(txtPARAM_CODE_BANKKEY_HSBC_PATH.Text))
            errors.addError("HSBC Bank Key has expired");
        else if (!HROne.HSBC.Utility.verifyCustPwd(txtPARAM_CODE_BANKKEY_HSBC_PATH.Text, txtPARAM_CODE_BANKKEY_HSBC_PASSWORD.Text))
            errors.addError("Invalid password for HSBC Bank Key");

        if (!System.IO.File.Exists(txtPARAM_CODE_BANKKEY_HASE_PATH.Text))
            errors.addError("File not found: " + txtPARAM_CODE_BANKKEY_HASE_PATH.Text);
        else if (HROne.HSBC.Utility.isKeyExpired(txtPARAM_CODE_BANKKEY_HASE_PATH.Text))
            errors.addError("Hang Seng Bank Key has expired");
        else if (!HROne.HSBC.Utility.verifyCustPwd(txtPARAM_CODE_BANKKEY_HASE_PATH.Text, txtPARAM_CODE_BANKKEY_HASE_PASSWORD.Text))
            errors.addError("Invalid password for Hang Seng Bank Key");
        


        if (errors.isEmpty())
        {
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PATH, txtPARAM_CODE_BANKKEY_HSBC_PATH.Text);
            ESystemParameter.setParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HSBC_PASSWORD, txtPARAM_CODE_BANKKEY_HSBC_PASSWORD.Text);
            ESystemParameter.setParameter(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HASE_PATH, txtPARAM_CODE_BANKKEY_HASE_PATH.Text);
            ESystemParameter.setParameterWithEncryption(dbConn, ESystemParameter.PARAM_CODE_BANKKEY_HASE_PASSWORD, txtPARAM_CODE_BANKKEY_HASE_PASSWORD.Text);

            Response.Redirect("~/BankKeyManagement.aspx");

        }

    }


}
