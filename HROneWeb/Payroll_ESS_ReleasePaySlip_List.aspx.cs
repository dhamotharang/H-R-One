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
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Payroll;

public partial class Payroll_ESS_ReleasePaySlip_List : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY015";

    protected SearchBinding sbinding;

    private bool IsAllowEdit = true;
    
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            IsAllowEdit = false;
            btnSubmit.Visible = false;
        }

        


        sbinding = new SearchBinding(dbConn, EEmpPersonalInfo.db);





        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(EEmpPayroll.db, Page.Master);
        errors.clear();

        Payroll_PeriodSelectionList1.SaveAsReleaseESSPaySlip();

        errors.addError("ESS Pay Slip is updated.");

    }

}
