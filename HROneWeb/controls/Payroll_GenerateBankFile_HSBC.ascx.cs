using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.BankFile;
using HROne.DataAccess;

public partial class Payroll_GenerateBankFile_HSBC : HROneWebControl, BankFileControlInterface
{
    const string PARAM_CODE_HSBC_AUTOPAY_FILE_CONTACT_PERSON = "HSBC_AUTOPAY_FILE_CONTACT_PERSON";
    const string PARAM_CODE_HSBC_AUTOPAY_FILE_CONTACT_PERSON_PHONE = "HSBC_AUTOPAY_FILE_CONTACT_PERSON_PHONE";

    public string BankCode
    {
        get { return ViewState["BankCode"].ToString(); }
        set { ViewState["BankCode"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["CompanyDBID"] == null)
        {
            FileType.Items.FindByValue("D").Enabled = false;
            FileType.Items.FindByValue("E").Enabled = true;
            FileTypeRow.Visible = false;
        }
        else
        {
            if (Application["MasterDBConfig"] != null && Session["CompanyDBID"] != null)
            {
                int CurID = (int)Session["CompanyDBID"];

                HROne.DataAccess.DatabaseConnection masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
                string CommandLineFolder = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY);
                if (string.IsNullOrEmpty(CommandLineFolder))
                {
                    FileType.Items.FindByValue("D").Enabled = false;
                }
                HROne.HSBC.Utility.HSBCMRICommandLineDirectory = CommandLineFolder;
            }
        }
        //HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        //if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROneSaaS)
        //{
        //}

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtContactPerson.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_HSBC_AUTOPAY_FILE_CONTACT_PERSON);
            txtContactPersonPhoneNumber.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_HSBC_AUTOPAY_FILE_CONTACT_PERSON_PHONE);
            FileType_SelectedIndexChanged(FileType, null);
        }
        
    }
    public virtual GenericBankFile CreateBankFileObject()
    {
        if (BankPaymentCode.Text.Length != 3)
        {
            
            throw new InvalidFieldValueException(lblPaymentCodeHeader.Text + " should be 3 digits");
            //return null;
        }

        HSBCBankFile HSBCBankFileObject;
        if (FileType.SelectedValue.Equals("D"))
        {
            if (string.IsNullOrEmpty(txtContactPerson.Text.Trim()))
                throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblContactPerson.Text));
            if (string.IsNullOrEmpty(txtContactPersonPhoneNumber.Text.Trim()))
                throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblContactPersonPhoneNumber.Text));

            string ProductName = "HROne ";
            if (WebUtils.productLicense(Session).ProductType == HROne.ProductLicense.ProductLicenseType.iMGR)
                ProductName = "iMGR ";

            HSBCBankFileObject = new HSBCBankFileEncrypted(dbConn, txtRemoteProfileID.Text, txtContactPerson.Text, txtContactPersonPhoneNumber.Text, ProductName + ProductVersion.CURRENT_PROGRAM_VERSION);
            HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_HSBC_AUTOPAY_FILE_CONTACT_PERSON, txtContactPerson.Text);
            HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_HSBC_AUTOPAY_FILE_CONTACT_PERSON_PHONE, txtContactPersonPhoneNumber.Text);

        }
        else
        {
            HSBCBankFileObject = new HSBCBankFile(dbConn);

            HSBCBankFileObject.UseBIBFormat = UseBIBFormat.Checked;
            if (UseBIBFormat.Checked)
            {
                HSBCBankFileObject.MultipleBankPaymentCode = multipleBankCode.Text.Replace("\n", string.Empty).Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tempPaymentCode in HSBCBankFileObject.MultipleBankPaymentCode)
                {
                    if (tempPaymentCode.Length != 3)
                    {
                        throw new InvalidFieldValueException(lblMultiplePaymentCodeHeader.Text + " should be 3 digits");
                        //return null;
                    }
                }
            }
        }

        HSBCBankFileObject.PlanCode = PlanCode.SelectedValue;
        HSBCBankFileObject.BankPaymentCode = BankPaymentCode.Text;
        HSBCBankFileObject.FirstPartyReference = FirstPartyReference.Text;
        return HSBCBankFileObject;
    }

    public bool IsAllowChequePayment()
    {
        return false; 
    }

    protected void UseBIBFormat_CheckedChanged(object sender, EventArgs e)
    {
        MultiplePaymentCodeRow.Visible = UseBIBFormat.Checked;
    }
    protected void FileType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (FileType.SelectedValue.Equals("D"))
        {
            DiskettePanel.Visible = true;
            eChannelPanel.Visible = false;
            RemoteProfileID.Visible = true;
            if (Application["MasterDBConfig"] != null && Session["CompanyDBID"] != null)
            {
                int CurID = (int)Session["CompanyDBID"];

                HROne.DataAccess.DatabaseConnection masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
                DBFilter filter = new DBFilter();
                filter.add(new Match("CompanyDBID", (int)Session["CompanyDBID"]));
                filter.add(new Match("HSBCExchangeProfileIsLocked", false));
                ArrayList exchangeProfileList = HROne.SaaS.Entities.EHSBCExchangeProfile.db.select(masterDBConn, filter);
                foreach (HROne.SaaS.Entities.EHSBCExchangeProfile exchangeProfile in exchangeProfileList)
                {
                    if (exchangeProfile.HSBCExchangeProfileBankCode.Equals(BankCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtRemoteProfileID.Text = exchangeProfile.HSBCExchangeProfileRemoteProfileID;
                        RemoteProfileID.Visible = false;
                    }
                }
                //HROne.SaaS.Entities.ECompanyDatabase companyDB = new HROne.SaaS.Entities.ECompanyDatabase();
                //companyDB.CompanyDBID = CurID;
                //if (HROne.SaaS.Entities.ECompanyDatabase.db.select(masterDBConn, companyDB))
                //{
                //    txtRemoteProfileID.Text = companyDB.CompanyDBClientCode;
                //    HROne.HSBC.Utility.HSBCMRICommandLineDirectory = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY);
                //    RemoteProfileID.Visible = false;
                //}
            }
        }
        else
        {
            DiskettePanel.Visible = false;
            eChannelPanel.Visible = true;
        }
    }

    public bool HasValueDate()
    {
        return true;
    }
    public bool IsShowAllPaymentMethod()
    {
        return false;
    }
}
