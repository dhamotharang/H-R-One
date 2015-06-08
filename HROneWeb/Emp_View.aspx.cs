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

public partial class Emp_View : HROneWebPage
{
    private const string FUNCTION_CODE = "PER001";
    public int CurID = -1;

    protected bool IsAllowAddRow = true;
    static string prevPage = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Start 0000173, Miranda, 2015-06-03
        this.Page.Form.DefaultButton = "mainContentPlaceHolder$Jump";
        //End 0000173, Miranda, 2015-06-03
        if (!Page.IsPostBack)
        {
            //prevPage = Request.UrlReferrer.ToString();
        }
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
        {
            Emp_PersonalInfo1.Visible = false;
        }
        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.iMGR)
            IsAllowAddRow = false;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = CurID;
        EEmpPersonalInfo.db.select(dbConn, empInfo);
        if (!IsPostBack)
        {
        }
        if (CurID > 0)
        {
        }
        else
        {
            toolBar.EditButton_Visible = false;
        }
        DBFilter empTerminationFilter = new DBFilter();
        empTerminationFilter.add(new Match("EmpID", CurID));
        if (EEmpTermination.db.count(dbConn, empTerminationFilter) <= 0)
            toolBar.CustomButton1_Visible = false;
        else
            toolBar.CustomButton1_Visible = toolBar.EditButton_Visible;

        if (WebUtils.productLicense(Session).IsESS)
            if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
                toolBar.CustomButton2_Visible = false;
            else
                toolBar.CustomButton2_Visible = toolBar.EditButton_Visible;
        else
            toolBar.CustomButton2_Visible = false;

        if (empInfo.MasterEmpID > 0)
            toolBar.CustomButton3_Visible = false;
        else
            toolBar.CustomButton3_Visible = toolBar.EditButton_Visible & IsAllowAddRow;
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        if (((Button)sender).ID.Equals("CustomButton1"))
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Edit.aspx?PrevEmpID=" + CurID);
        else if (((Button)sender).ID.Equals("CustomButton3"))
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Edit.aspx?PrevEmpID=" + CurID + "&NewRole=Y");
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_Edit.aspx?EmpID=" + CurID);
    }

    protected void Back_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prevPage))
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, prevPage);
        }
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_list.aspx");
    }

    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
        empInfo.EmpID = CurID;
        empInfo.EmpPW = string.Empty;

        WebUtils.StartFunction(Session, FUNCTION_CODE);
        EEmpPersonalInfo.db.update(dbConn, empInfo);
        WebUtils.EndFunction(dbConn);
        PageErrors errors = PageErrors.getErrors(EEmpPositionInfo.db, this.Master);
        errors.addError("Reset Password Successfully");
    }

    //Start 0000173, Miranda, 2015-04-26
    protected void Jump_Click(object sender, EventArgs e)
    {
        DBFilter dbFilter = new DBFilter();
        OR orEmpNo = new OR();
        orEmpNo.add(new Match("EmpNo", EmpNo.Text));
        DBFieldTranscoder transcoder = EEmpPersonalInfo.db.getField("EmpNo").transcoder;
        if (transcoder != null)
            orEmpNo.add(new Match("EmpNo", transcoder.toDB(EmpNo.Text)));
        dbFilter.add(orEmpNo);
        ArrayList list = EEmpPersonalInfo.db.select(dbConn, dbFilter);
        if (list.Count > 0)
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_View.aspx?EmpID=" + ((EEmpPersonalInfo)list[0]).EmpID);
        }
        //Start 0000173, Miranda, 2015-06-03
        else {
            PageErrors errors = PageErrors.getErrors(EEmpPositionInfo.db, this.Master);
            errors.addError("Employee Not Found");
        }
        //End 0000173, Miranda, 2015-06-03

    }
    //End 0000173, Miranda, 2015-04-26
}
