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

public partial class Emp_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER001";

    protected int CurID = -1;
    protected int PrevCurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        HROne.Common.WebUtility.WebControlsLocalization(Session, this.Controls);

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        else
            Emp_PersonalInfo_Edit1.EmpID = CurID;
        if (!int.TryParse(DecryptedRequest["PrevEmpID"], out PrevCurID))
            PrevCurID = -1;
        else
            Emp_PersonalInfo_Edit1.PrevEmpID = PrevCurID;

        if (DecryptedRequest["NewRole"] == "Y")
            Emp_PersonalInfo_Edit1.IsCreateNewRoleEmployee = true;

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {

            }
            else
            {

                if (PrevCurID > 0)
                {

                }
                //obj = new EEmpPersonalInfo();
                toolBar.DeleteButton_Visible = false;
            }

        }

    }
    protected void Save_Click(object sender, EventArgs e)
    {
        if (Emp_PersonalInfo_Edit1.Save())
        {
            if (CurID>0)
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_View.aspx?EmpID=" + CurID);
            else
                if (PrevCurID > 0)
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_PositionInfo_Edit.aspx?Flow=true&EmpID=" + Emp_PersonalInfo_Edit1.EmpID + "&PrevEmpID=" + PrevCurID);
                else
                    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_BankAccount_Edit.aspx?Flow=true&EmpID=" + Emp_PersonalInfo_Edit1.EmpID + "&EnableSkip=-1");
        }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        if (Emp_PersonalInfo_Edit1.Delete())
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_List.aspx");

    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_View.aspx?EmpID=" + CurID);
        else if (PrevCurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_View.aspx?EmpID=" + PrevCurID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_List.aspx");

    }
}
