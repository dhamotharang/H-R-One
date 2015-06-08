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
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Emp_BankAccount_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER002";
    public int CurID = -1;
    public int CurEmpID = -1;
    public int enableSkip = 0;
    public Hashtable CurEmpBankAccountGroups = new Hashtable();
    public Hashtable CurRanks = new Hashtable();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;
        
        if (!int.TryParse(DecryptedRequest["EmpBankAccountID"], out CurID))
            CurID = -1;
        else
            Emp_BankAccount_Edit1.EmpBankAccountID = CurID;
        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;
        else
            Emp_BankAccount_Edit1.EmpID = CurEmpID;


        int.TryParse(DecryptedRequest["EnableSkip"], out enableSkip);

        if (enableSkip == -1)
            toolBar.CustomButton1_Visible = true;
        else
            toolBar.CustomButton1_Visible = false;

        

        if (CurID > 0)
            ActionHeader.Text = "Edit";
        else
            ActionHeader.Text = "Add";


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID <= 0)
                toolBar.DeleteButton_Visible = false;

            if ("true" == DecryptedRequest["Flow"])
            {
                Flow.Value = "true";
            }
            else
            {
                Flow.Value = "false";
            }


        }
    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        TitleCode.Text = ((TextBox)Emp_BankAccount_Edit1.FindControl("EmpBankAccountHolderName")).Text;
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        if (Emp_BankAccount_Edit1.Save())
        {
            if (Flow.Value.Equals("true"))
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_PositionInfo_Edit.aspx?Flow=true&EmpID=" + Emp_BankAccount_Edit1.EmpID);
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_BankAccount_View.aspx?EmpBankAccountID=" + Emp_BankAccount_Edit1.EmpBankAccountID + "&EmpID=" + Emp_BankAccount_Edit1.EmpID);
        }
    }

    protected void Delete_Click(object sender, EventArgs e)
    {

        if (Emp_BankAccount_Edit1.Delete())
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_BankAccount_View.aspx?EmpID=" + Emp_BankAccount_Edit1.EmpID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_BankAccount_View.aspx?EmpBankAccountID=" + Emp_BankAccount_Edit1.EmpBankAccountID + "&EmpID=" + Emp_BankAccount_Edit1.EmpID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_BankAccount_View.aspx?EmpID=" + Emp_BankAccount_Edit1.EmpID);

    }

    protected void Skip_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_PositionInfo_Edit.aspx?Flow=true&EmpID=" + Emp_BankAccount_Edit1.EmpID);
    }

}
