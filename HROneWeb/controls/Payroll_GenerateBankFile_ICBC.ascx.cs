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

public partial class Payroll_GenerateBankFile_ICBC : HROneWebControl, BankFileControlInterface
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public GenericBankFile CreateBankFileObject()
    {
        ICBCBankFile ICBCBankFileObject = new ICBCBankFile(dbConn);
        return ICBCBankFileObject;
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
