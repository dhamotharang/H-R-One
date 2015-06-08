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

public partial class EmpTab_Position_View : HROneWebPage
{
    public int CurID = -1;
    private const string FUNCTION_CODE_1 = "PER007";
    private const string FUNCTION_CODE_2 = "PER007-1";
    static string prevPage = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
		//Start 0000173, Miranda, 2015-06-03
        this.Page.Form.DefaultButton = "mainContentPlaceHolder$Jump";
        //End 0000173, Miranda, 2015-06-03
        if (!Page.IsPostBack)
        {
            prevPage = Request.UrlReferrer.ToString();
        }

        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE_1, WebUtils.AccessLevel.Read) && !WebUtils.CheckPermission(Session, FUNCTION_CODE_2, WebUtils.AccessLevel.Read))
            Server.Transfer("AccessDeny.aspx");

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        Emp_RecurringPayment_List1.CurrentEmpID = CurID;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

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
		//Start 0000173, Miranda, 2015-05-30
        else {
            PageErrors errors = PageErrors.getErrors(EEmpPositionInfo.db, this.Master);
            errors.addError("Employee Not Found");
        }
        //End 0000173, Miranda, 2015-05-30
    }
    //End 0000173, Miranda, 2015-04-26
}
