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

public partial class Payroll_GenerateBankFile_BOAmerica : HROneWebControl, BankFileControlInterface
{

    const string PARAM_CODE_BOAMERICA_AUTOPAY_FILE_EFTKey = "BOAMERICA_AUTOPAY_FILE_EFTKey";
    const string PARAM_CODE_BOAMERICA_AUTOPAY_FILE_COMPANYID = "BOAMERICA_AUTOPAY_FILE_COMPANYID";
    const string PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_1 = "BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_1";
    const string PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_2 = "BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_2";
    const string PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_CITYNAME = "BOAMERICA_AUTOPAY_FILE_ORDERING_CITYNAME";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            EFTKey.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_EFTKey);
            CompanyID.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_COMPANYID);
            OrderingPartyAddress1.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_1);
            OrderingPartyAddress2.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_2);
            OrderingPartyCityName.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_CITYNAME);
            if (string.IsNullOrEmpty(OrderingPartyCityName.Text))
                OrderingPartyCityName.Text = "HONG KONG";
        }
    }

    public virtual GenericBankFile CreateBankFileObject()
    {
        if (string.IsNullOrEmpty(EFTKey.Text.Trim()))
            throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblEFTKey.Text));
        if (string.IsNullOrEmpty(CompanyID.Text.Trim()))
            throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblCompanyID.Text));
        if (string.IsNullOrEmpty(OrderingPartyAddress1.Text.Trim()))
            throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblOrderingPartyAddress.Text));

        BOAmericaBankFile BOABankFileObject = new BOAmericaBankFile(dbConn);
        BOABankFileObject.Indicator = Indicator.SelectedValue;
        BOABankFileObject.EFTKey = EFTKey.Text;
        BOABankFileObject.CompanyID = CompanyID.Text;
        BOABankFileObject.PostalAddress1 = OrderingPartyAddress1.Text;
        BOABankFileObject.PostalAddress2 = OrderingPartyAddress2.Text;
        BOABankFileObject.PostalCityName = OrderingPartyCityName.Text;

        HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_COMPANYID, CompanyID.Text);
        HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_EFTKey, EFTKey.Text);
        HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_1, OrderingPartyAddress1.Text);
        HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_ADDRESS_2, OrderingPartyAddress2.Text);
        HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_BOAMERICA_AUTOPAY_FILE_ORDERING_PARTY_CITYNAME, OrderingPartyCityName.Text);

        return BOABankFileObject;
    }

    public bool IsAllowChequePayment()
    {
        return false; 
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
