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

public partial class Payroll_GenerateBankFile_BOCNY : HROneWebControl, BankFileControlInterface
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public virtual GenericBankFile CreateBankFileObject()
    {

        BOCNYBankFile BOCNYBankFileObject = new BOCNYBankFile(dbConn);
        BOCNYBankFileObject.FileType = (BOCNYBankFile.FileTypeEnum)Enum.Parse(typeof(BOCNYBankFile.FileTypeEnum), cboFileFormat.SelectedValue, true);
        //  Pay In/Out not implemented yet
        return BOCNYBankFileObject;
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
