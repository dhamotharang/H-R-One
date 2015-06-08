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

public partial class Payroll_GenerateBankFile_DBS : HROneWebControl, BankFileControlInterface
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public GenericBankFile CreateBankFileObject()
    {
        if (string.IsNullOrEmpty(BatchID.Text))
            throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblBatchID.Text));

        if (string.IsNullOrEmpty(BatchName.Text.Trim()))
            throw new Exception(string.Format(HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED, lblBatchName.Text));


        DBSBankFile DBSBankFileObject = new DBSBankFile(dbConn);
        DateTime currentDateTime = AppUtils.ServerDateTime();
        DBSBankFileObject.BatchID = BatchID.Text;//HROne.CommonLib.base32.encodeb32((long)currentDateTime.Month, 1) + HROne.CommonLib.base32.encodeb32((long)currentDateTime.Day, 1) + HROne.CommonLib.base32.encodeb32((long)currentDateTime.Hour, 1) + HROne.CommonLib.base32.encodeb32((long)currentDateTime.Minute, 2);
        DBSBankFileObject.BatchName = BatchName.Text;
        DBSBankFileObject.TransactionType = (DBSBankFile.TransactionTypeEnum)Enum.Parse(typeof(DBSBankFile.TransactionTypeEnum), TransactionType.SelectedValue);
        DBSBankFileObject.SecondPartyReference = SecondPartyReference.Text;
        DBSBankFileObject.IsSecondPartyReferenceIncludeEmpNo = IncludeEmpNo.Checked;
        return DBSBankFileObject;
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
