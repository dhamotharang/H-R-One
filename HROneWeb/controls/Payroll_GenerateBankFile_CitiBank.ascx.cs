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

public partial class Payroll_GenerateBankFile_CitiBank : HROneWebControl, BankFileControlInterface
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public GenericBankFile CreateBankFileObject()
    {
        CitiBankFile citiBankFileObject = new CitiBankFile(dbConn, PaylinkCheque.Checked);
        citiBankFileObject.ProductCode = ProductCode.SelectedValue;
        if (FileFormat.SelectedValue.Equals("Excel", StringComparison.CurrentCultureIgnoreCase))
            citiBankFileObject.BankFileFormat = CitiBankFile.BankFileFormatEnum.EXCEL;
        else
            citiBankFileObject.BankFileFormat = CitiBankFile.BankFileFormatEnum.TEXT;
        citiBankFileObject.PaymentDetails = txtPaymentDetails.Text;

        return citiBankFileObject;
    }

    public bool IsAllowChequePayment()
    {
        return true; 
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
