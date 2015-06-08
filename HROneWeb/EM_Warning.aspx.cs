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
using HROne.Lib.Entities;
using HROne.DataAccess;

public partial class EM_Warning : HROneWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.IsSuperUserMissing(dbConn))
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx");

        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);

        if (Application["MasterDBConfig"] == null)
            Page.Title = "iMGR";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        PageErrors pageErrors = PageErrors.getErrors(null, this.Page);
        if (!IsValidPassCode(txtPassCode.Text))
        {
            pageErrors.addError("Incorrect Pass Code!");
            return;
        }
        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);
        Session["LoginID"] = crypto.Encrypting("EM", Session.SessionID);
        //EUser user = new EUser();
        //user.UserID = -1;
        //user.LoginID = "EM";
        //Session["User"] = user;
        Session["IgnoreEM"] = false;
        Session.Remove("User");
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx");

    }
    protected void btnSkip_Click(object sender, EventArgs e)
    {
        Session["IgnoreEM"] = true;
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx");
    }
    public bool IsValidPassCode(string code)
    {
        DateTime permittedDate = AppUtils.getDateFromCode(code, "HROne");
        if (permittedDate.Equals(AppUtils.ServerDateTime().Date))
            return true;
        else
            return false;
    }
}
