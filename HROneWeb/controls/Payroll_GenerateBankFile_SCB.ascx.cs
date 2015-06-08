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

public partial class Payroll_GenerateBankFile_SCB : HROneWebControl, BankFileControlInterface
{

    protected const string PARAM_CODE_AUTOPAY_SCB_ER_CMG_REFERENCE = "AUTOPAY_SCB_ER_CMG_REFERENCE";
    protected const string PARAM_CODE_AUTOPAY_SCB_FILE_FORMAT = "AUTOPAY_SCB_FILE_FORMAT";

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ERCMGReference.Text = ESystemParameter.getParameter(dbConn, PARAM_CODE_AUTOPAY_SCB_ER_CMG_REFERENCE);
            ListItem fileTypeItem = FileType.Items.FindByValue(ESystemParameter.getParameter(dbConn, PARAM_CODE_AUTOPAY_SCB_FILE_FORMAT));
            if (fileTypeItem != null)
                FileType.SelectedValue = fileTypeItem.Value;
        }
        // Start 0000197, KuangWei, 2015-05-27
        if (FileType.SelectedValue.Equals("DGPOnly"))
        {
            DGPOnlyPanel.Visible = true;
            DGPwMPFPanel.Visible = false;
            PaymentCentrePanel.Visible = false;
        }
        else if (FileType.SelectedValue.Equals("DGPwMPF"))
        {
            DGPOnlyPanel.Visible = false;
            DGPwMPFPanel.Visible = true;
            PaymentCentrePanel.Visible = false;
        }
        else if (FileType.SelectedValue.Equals("iPayment"))
        {
            DGPOnlyPanel.Visible = false;
            DGPwMPFPanel.Visible = false;
            PaymentCentrePanel.Visible = true;
        }
        else
        {
            DGPOnlyPanel.Visible = false;
            DGPwMPFPanel.Visible = false;
            PaymentCentrePanel.Visible = false;
        }
        // End 0000197, KuangWei, 2015-05-27
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
    }
    public GenericBankFile CreateBankFileObject()
    {


        if (FileType.SelectedValue.Equals("DGPOnly"))
        {
            SCBBankFile SCBBankFileObject = new SCBBankFile(dbConn);
            SCBBankFileObject.CustomerReference = CustomerReference.Text;
            return SCBBankFileObject;
        }
        else if (FileType.SelectedValue.Equals("DGPwMPF"))
        {
            SCBBankMPFFile SCBBankFileObject = new SCBBankMPFFile(dbConn);
            SCBBankFileObject.ERCMGReference = ERCMGReference.Text;
            SCBBankFileObject.BatchDescription = BatchDescription.Text;
            SCBBankFileObject.IncludeMPFRecord = IncludeMPFRecord.Checked;

            ESystemParameter.setParameter(dbConn, PARAM_CODE_AUTOPAY_SCB_ER_CMG_REFERENCE, ERCMGReference.Text);
            ESystemParameter.setParameter(dbConn, PARAM_CODE_AUTOPAY_SCB_FILE_FORMAT, FileType.SelectedValue);

            return SCBBankFileObject;
        }
        // Start 0000197, KuangWei, 2015-05-27
        else if (FileType.SelectedValue.Equals("iPayment"))
        {
            SCBiPaymentBankFile SCBiPaymentBankFileObject = new SCBiPaymentBankFile(dbConn);
            SCBiPaymentBankFileObject.PaymentCustomerReference = PaymentCustomerReference.Text;
            SCBiPaymentBankFileObject.PaymentCustomerMemo = PaymentCustomerMemo.Text;
            return SCBiPaymentBankFileObject;

        }
        // End 0000197, KuangWei, 2015-05-27

        return null;
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
        return true;
    }
}
