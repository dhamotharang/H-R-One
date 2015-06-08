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

public partial class EmpTab_EmergencyContact_View : HROneWebPage
{
    public int CurID = -1;
    private const string FUNCTION_CODE = "PER015";

    protected void Page_Load(object sender, EventArgs e)
    {
		//Start 0000173, Miranda, 2015-06-03
        this.Page.Form.DefaultButton = "mainContentPlaceHolder$Jump";
        //End 0000173, Miranda, 2015-06-03
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;
        EmpID.Value = CurID.ToString();

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    protected void Back_Click(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_List.aspx");
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
