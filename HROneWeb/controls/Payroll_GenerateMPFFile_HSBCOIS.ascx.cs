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
using HROne.MPFFile;
using HROne.DataAccess;

public partial class Payroll_GenerateMPFFile_HSBCOIS : HROneWebControl, MPFFileControlInterface
{

    //public string BankCode
    //{
    //    get { return hfBankCode.Value; }
    //    set { hfBankCode.Value = value; }
    //}
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["CompanyDBID"] == null)
        //{
        //    FileType.Items.FindByValue("D").Enabled = false;
        //    if (!IsPostBack)
        //        FileType.Items.FindByValue("AMCND").Selected = true;
        //}
        //if (!IsPostBack)
        //    FileType_SelectedIndexChanged(FileType, EventArgs.Empty);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
    }


    public GenericMPFFile CreateMPFFileObject()
    {
        HSBCOISMPFFile mpfFile = new HSBCOISMPFFile(dbConn);
        return mpfFile; 


        //if (FileType.SelectedValue.Equals("AMCND"))
        //{
        //    HSBCMPFFile mpfFile = new HSBCMPFFile(dbConn);
        //    if (this.PaymentMethod.SelectedValue.Equals("B"))
        //        mpfFile.PaymentMethod = HSBCMPFFile.PaymentMethodEnum.DIRECT_CREDIT_TO_BANK_ACC;
        //    else if (this.PaymentMethod.SelectedValue.Equals("D"))
        //        mpfFile.PaymentMethod = HSBCMPFFile.PaymentMethodEnum.DIRECT_DEBIT;
        //    else if (this.PaymentMethod.SelectedValue.Equals("C"))
        //        mpfFile.PaymentMethod = HSBCMPFFile.PaymentMethodEnum.CHEQUE;
        //    return mpfFile;
        //}
        //else if (FileType.SelectedValue.Equals("D") || FileType.SelectedValue.Equals("AMPFF"))
        //{
        //    string RemoteProfileID = txtRemoteProfileID.Text;
        //    if (FileType.SelectedValue.Equals("D"))
        //    {
        //        HSBCMPFGatewayFileEncrypted mpfFile = new HSBCMPFGatewayFileEncrypted(dbConn, RemoteProfileID, "HROne " + ProductVersion.CURRENT_PROGRAM_VERSION);
        //        return mpfFile;
        //    }
        //    else if (FileType.SelectedValue.Equals("AMPFF"))    //check again for safety
        //    {
        //        HSBCMPFGatewayFile mpfFile = new HSBCMPFGatewayFile(dbConn, RemoteProfileID);
        //        return mpfFile;
        //    }
        //}

        //return new GenericMPFFile(dbConn);
    }
    //protected void FileType_SelectedIndexChanged(object sender, EventArgs e)
    //{


    //    if (FileType.SelectedValue.Equals("D"))
    //    {
    //        RemoteProfileID.Visible = true;

    //        PaymentMethodRow.Visible = false;
    //        if (Application["MasterDBConfig"] != null && Session["CompanyDBID"] != null)
    //        {
    //            int CurID = (int)Session["CompanyDBID"];

    //            HROne.DataAccess.DatabaseConnection masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
    //            DBFilter filter = new DBFilter();
    //            filter.add(new Match("CompanyDBID", (int)Session["CompanyDBID"]));
    //            filter.add(new Match("HSBCExchangeProfileIsLocked", false));
    //            ArrayList exchangeProfileList = HROne.SaaS.Entities.EHSBCExchangeProfile.db.select(masterDBConn, filter);
    //            if (exchangeProfileList.Count > 0)
    //            {
    //                foreach (HROne.SaaS.Entities.EHSBCExchangeProfile exchangeProfile in exchangeProfileList)
    //                {
    //                    if (exchangeProfile.HSBCExchangeProfileBankCode.Equals(BankCode, StringComparison.CurrentCultureIgnoreCase))
    //                    {
    //                        txtRemoteProfileID.Text = exchangeProfile.HSBCExchangeProfileRemoteProfileID;
    //                        RemoteProfileID.Visible = false;
    //                    }
    //                }
    //                if (RemoteProfileID.Visible)
    //                {
    //                    txtRemoteProfileID.Text = ((HROne.SaaS.Entities.EHSBCExchangeProfile)exchangeProfileList[0]).HSBCExchangeProfileRemoteProfileID;
    //                    RemoteProfileID.Visible = false;
    //                }
    //            }
    //            HROne.HSBC.Utility.HSBCMRICommandLineDirectory = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY);
    //            //HROne.SaaS.Entities.ECompanyDatabase companyDB = new HROne.SaaS.Entities.ECompanyDatabase();
    //            //companyDB.CompanyDBID = CurID;
    //            //if (HROne.SaaS.Entities.ECompanyDatabase.db.select(masterDBConn, companyDB))
    //            //{
    //            //    txtRemoteProfileID.Text = companyDB.CompanyDBClientCode;
    //            //    HROne.HSBC.Utility.HSBCMRICommandLineDirectory = HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_HSBC_MRI_DIRECTORY);
    //            //    RemoteProfileID.Visible = false;
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        RemoteProfileID.Visible = false;
    //        if (FileType.SelectedValue.Equals("AMCND"))
    //            PaymentMethodRow.Visible = true;
    //        else
    //            PaymentMethodRow.Visible = false;
    //    }
    //}
}
