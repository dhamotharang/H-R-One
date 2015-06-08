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

public partial class Payroll_GenerateBankFile_ANZ : HROneWebControl, BankFileControlInterface
{

    const string PARAM_CODE_ANZ_AUTOPAY_FILE_CLIENT_CODE = "ANZ_AUTOPAY_FILE_CLIENT_CODE";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            ClientCode.Text = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, PARAM_CODE_ANZ_AUTOPAY_FILE_CLIENT_CODE);

    }

    public virtual GenericBankFile CreateBankFileObject()
    {
        if (string.IsNullOrEmpty(ClientCode.Text.Trim()))
            throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblClientCodeHeader.Text));

        ANZBankFile ANZBankFileObject = new ANZBankFile(dbConn);
        ANZBankFileObject.MyProductCode = MyProductCode.SelectedValue;
        ANZBankFileObject.PaymentProduct = PaymentProduct.SelectedValue;
        ANZBankFileObject.ClientCode = ClientCode.Text;

        HROne.Lib.Entities.ESystemParameter.setParameter(dbConn, PARAM_CODE_ANZ_AUTOPAY_FILE_CLIENT_CODE, ClientCode.Text);
        
        return ANZBankFileObject;
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
