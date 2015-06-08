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
using HROne.Lib.Entities;

public partial class Payroll_GenerateBankFile_UOB : HROneWebControl, BankFileControlInterface
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
    }

    public GenericBankFile CreateBankFileObject()
    {
        //if (string.IsNullOrEmpty(BatchID.Text))
        //    throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblBatchID.Text));

        if (string.IsNullOrEmpty(Particulars.Text.Trim()))
            throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblParticulars.Text));


        UOBBankFile UOBBankFileObject = new UOBBankFile(dbConn);
        DateTime currentDateTime = AppUtils.ServerDateTime();
        //UOBBankFileObject.BatchID = BatchID.Text;//HROne.CommonLib.base32.encodeb32((long)currentDateTime.Month, 1) + HROne.CommonLib.base32.encodeb32((long)currentDateTime.Day, 1) + HROne.CommonLib.base32.encodeb32((long)currentDateTime.Hour, 1) + HROne.CommonLib.base32.encodeb32((long)currentDateTime.Minute, 2);
        UOBBankFileObject.Particulars = Particulars.Text;
        UOBBankFileObject.TransactionType = (UOBBankFile.TransactionTypeEnum)Enum.Parse(typeof(UOBBankFile.TransactionTypeEnum), TransactionType.SelectedValue);

       
        //UOBBankFileObject.SecondPartyReference = SecondPartyReference.Text;
        //UOBBankFileObject.IsSecondPartyReferenceIncludeEmpNo = IncludeEmpNo.Checked;
        return UOBBankFileObject;
    }

    public bool IsAllowChequePayment()
    {
        return false;
    }

    public bool HasValueDate()
    {
        return false;
    }
    public bool IsShowAllPaymentMethod()
    {
        return false;
    }
}
