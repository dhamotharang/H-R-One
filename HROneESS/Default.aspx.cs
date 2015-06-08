using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using HROne.Lib.Entities;
using HROne.DataAccess;

public partial class _Default : HROneWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string DatabaseUpgradePage = "DatabaseUpgrade.aspx";
        //if (Session["NeedDBUpgrade"] != null)
        //    if (Application["NeedDBUpgrade"].Equals(true))
        //        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/" + DatabaseUpgradePage);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            //else
            {
                //EUser dummyUser = null;
                //if (!WebUtils.ValidateUser(user.LoginID, user.UserPassword, false, false, out dummyUser))
                //{
                //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx");
                //}
                if (DecryptedRequest["LastURL"] != null)
                {
                    string lastURL = DecryptedRequest["LastURL"].ToString();
                    if (!string.IsNullOrEmpty(lastURL))
                        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(HttpUtility.UrlDecode(lastURL))));
                }
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_Home.aspx");
            }
        }
        else
        {

            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx");
        }
    }

}