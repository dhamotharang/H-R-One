//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using HROne.DataAccess;
////using perspectivemind.validation;
//using HROne.Lib.Entities;

//public partial class Emp_Business_View : HROneWebPage
//{
//    public int CurID = -1;


//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!WebUtils.CheckAccess(Response, Session, "SYS001", WebUtils.AccessLevel.Read))
//            return;


//        try
//        {
//            CurID = Int32.Parse(DecryptedRequest["EmpID"]);
//            EmpID.Value = CurID.ToString();
//        }
//        catch (Exception ex)
//        {
//        }
//        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

//    }
//    protected void Back_Click(object sender, EventArgs e)
//    {
//        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_List.aspx");
//    }
//}
