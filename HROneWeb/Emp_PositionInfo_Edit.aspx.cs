using System;
using System.Data;
using System.Globalization;
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

public partial class Emp_Position_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PER007";
    public Binding binding;
    public DBManager db = EEmpPositionInfo.db;
    public EEmpPositionInfo obj;
    public int CurID = -1;
    public int CurEmpID = -1;
    public Hashtable CurElements = new Hashtable();

    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!int.TryParse(DecryptedRequest["EmpPosID"], out CurID))
            CurID = -1;
        else
            Emp_PositionInfo_Edit1.EmpPosID = CurID;
        if (!int.TryParse(DecryptedRequest["EmpID"], out CurEmpID))
            CurEmpID = -1;
        else
            Emp_PositionInfo_Edit1.EmpID = CurEmpID;

        if (CurID > 0)
        {
            btnLastPositionTerms.Visible = false;
            ActionHeader.Text = "Edit";
        }
        else
        {
            if (AppUtils.GetLastPositionInfo(dbConn, CurEmpID) != null)
                btnLastPositionTerms.Visible = true;
            else
                btnLastPositionTerms.Visible = false;
            ActionHeader.Text = "Add";
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


        
    }
    protected void Page_PreRender()
    {
        if (!IsPostBack)
        {
            if (CurID <0)
                toolBar.DeleteButton_Visible = false;

            if ("true" == DecryptedRequest["Flow"])
            {
                Flow.Value = "true";
                int PrevEmpID = -1;
                if (!int.TryParse(DecryptedRequest["PrevEmpID"], out PrevEmpID))
                    PrevEmpID = -1;
                if (PrevEmpID > 0)
                {
                    Emp_PositionInfo_Edit1.LoadLastEmpPositionInfo(PrevEmpID);
                    Emp_PositionInfo_Edit1.EmpID = CurEmpID;
                }
            }
            else
            {
                Flow.Value = "false";
            }
        }
    }


    protected void Save_Click(object sender, EventArgs e)
    {
        if (Emp_PositionInfo_Edit1.Save())
        {

            if (Flow.Value.Equals("true"))
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_Pension_View.aspx?EmpID=" + CurEmpID);
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_PositionInfo_View.aspx?EmpPosID=" + Emp_PositionInfo_Edit1.EmpPosID + "&EmpID=" + CurEmpID );

        }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        if (Emp_PositionInfo_Edit1.Delete())
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_Position_View.aspx?EmpID=" + CurEmpID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID > 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_PositionInfo_View.aspx?EmpPosID=" + CurID + "&EmpID=" + CurEmpID);
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmpTab_Position_View.aspx?EmpID=" + CurEmpID);

    }
    protected void btnLastPositionTerms_Click(object sender, EventArgs e)
    {
        if (CurID <= 0 && CurEmpID > 0)
        {
            Emp_PositionInfo_Edit1.LoadLastEmpPositionInfo(CurEmpID);
        }
    }
}
