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

public partial class EmpTab_LeaveBalanceEntitleHistory_View : HROneWebPage
{
    public int CurID = -1;
    public int CurLeaveTypeID = -1;
    private const string FUNCTION_CODE = "PER010";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        if (!int.TryParse(DecryptedRequest["LeaveTypeID"], out CurLeaveTypeID))
            CurLeaveTypeID = -1;
        EmpID.Value = CurID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        ELeaveType leaveType = new ELeaveType();
        leaveType.LeaveTypeID = CurLeaveTypeID;
        if (ELeaveType.db.select(dbConn, leaveType))
        {
            LeaveTypeID.Text = leaveType.LeaveTypeDesc;
        }
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "EmpTab_LeaveBalance_View.aspx?EmpID=" + CurID);
    }
}
