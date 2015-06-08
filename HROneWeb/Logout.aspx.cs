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
using HROne.Import;
public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["User"] = null;
        //Session["Permissions"] = null;
        //Session["TrialVersion"] = null;
        //try
        //{
        //    WebUtils.ClearTempTable(dbConn, Session.SessionID);
        //    //System.Diagnostics.Debug.Write("Clear Upload Claims and Deduction Table...");
        //    //ImportClaimsAndDeductionsProcess.ClearTempTable(dbConn, Session.SessionID);
        //    //System.Diagnostics.Debug.WriteLine("OK");
        //    //System.Diagnostics.Debug.Write("Clear Upload Employee Table...");
        //    //ImportEmpPersonalInfoProcess.ClearTempTable(Session.SessionID);
        //    //System.Diagnostics.Debug.WriteLine("OK");
        //}
        //catch
        //{
        //    System.Diagnostics.Debug.WriteLine("Fail");
        //}
        //Session.Abandon();
        WebUtils.Logout(Session);
        Request.Cookies.Remove(HROne.Common.WebUtility.getDefaultSessionCookieName());
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx");
        Response.End();

    }
}
