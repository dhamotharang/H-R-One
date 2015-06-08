using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;

public partial class _Default : HROneWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (user == null)
        {
            Response.Redirect("~/login.aspx");
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Home.aspx");
    }
}
