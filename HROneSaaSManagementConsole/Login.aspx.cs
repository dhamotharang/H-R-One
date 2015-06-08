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

//  Class name Login should NOT be used to prevent a bug caused by old version of Microsoft.Net framework
public partial class ASPLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Logout previous session
        if (!IsPostBack)
            if (Session["User"] != null)
            {
                WebUtils.Logout(Session);
                Request.Cookies.Remove(HROne.Common.WebUtility.getDefaultSessionCookieName());
                Response.Redirect(Request.Url.PathAndQuery);
            }

        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");
        Password.Attributes.Add("onfocus", "this.select();");

        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        }
        if (Session.Count == 0)
            //  Release the session for renew Session ID on SaaS mode;
            Session.Abandon();
    }

    //public bool ValidateUser(string username, string encryptedPassword)
    //{
    //    string message = string.Empty;
    //    DBFilter filter = new DBFilter();
    //    filter.add(new Match("LoginID", username));
    //    filter.add(new Match("UserAccountStatus", "<>", "D"));
    //    ArrayList list = EUser.db.select(dbConn, filter);
    //    if (list.Count == 0)
    //    {
    //        message = "Invalid User Name or Password";
    //        throw new Exception(message);
    //        return;
    //    }
    //    EUser user = (EUser)list[0];
    //    if (user.UserPassword == null)
    //        user.UserPassword = "";
    //    if (!(user.UserAccountStatus == "A"))
    //    {
    //        message = "Account is Inactive/Locked";
    //        throw new Exception(message);
    //        return;
    //    }

    //    if (!user.UserPassword.Equals(encryptedPassword))
    //    {
    //        message = "Invalid User Name or Password";
    //        user.FailCount++;
    //        string maxFailCountParameterString = ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_LOGIN_MAX_FAIL_COUNT);
    //        if (!maxFailCountParameterString.Equals(string.Empty))
    //        {
    //            int MaxFailCount = 0;
    //            if (int.TryParse(maxFailCountParameterString, out MaxFailCount))
    //                if (MaxFailCount > 0)
    //                    if (user.FailCount >= MaxFailCount)
    //                    {
    //                        user.UserAccountStatus = "I";
    //                        user.FailCount = 0;
    //                    }
    //                    else if (user.FailCount - MaxFailCount == 1)
    //                    {
    //                        message += "\r\n" + "The account will be locked if you fail to login 1 more time";
    //                    }

    //        }
    //        EUser.db.update(dbConn, user);
    //        throw new Exception(message);
    //        return;
    //    }

    //    Session["User"] = user;

    //    WebUtils.SetSessionLanguage(Session, user);

    //    user.FailCount = 0;
    //    EUser.db.update(dbConn, user);
    //    //WebUtils.RefreshPermission(Session);
    //    bool isForceChangePassword = false;

    //    if (user.UserChangePasswordUnit == "D")
    //    {
    //        if (AppUtils.ServerDateTime() < user.UserChangePasswordDate.AddDays(user.UserChangePasswordPeriod))
    //            isForceChangePassword = false;
    //        else
    //            isForceChangePassword = true;
    //    }
    //    else if (user.UserChangePasswordUnit == "M")
    //    {
    //        if (AppUtils.ServerDateTime() < user.UserChangePasswordDate.AddMonths(user.UserChangePasswordPeriod))
    //            isForceChangePassword = false;
    //        else
    //            isForceChangePassword = true;
    //    }
    //    else if (user.UserChangePasswordUnit == "Y")
    //    {
    //        if (AppUtils.ServerDateTime() < user.UserChangePasswordDate.AddYears(user.UserChangePasswordPeriod))
    //            isForceChangePassword = false;
    //        else
    //            isForceChangePassword = true;
    //    }
    //    else
    //        isForceChangePassword = false;
    //    if (user.UserChangePassword)
    //        isForceChangePassword = true;

    //    //EInbox.GenerateInboxMessage(user.UserID);
    //    if (isForceChangePassword)
    //        Session["ForceChangePassword"] = true;
    //    if (Session["LastURL"] != null)
    //        Response.Redirect(Session["LastURL"].ToString());
    //    else
    //        Response.Redirect("Emp_List.aspx");

    //}

    protected void Login_Click(object sender, EventArgs e)
    {
        Session.Clear();

        //  re-assign the connection 
        HROne.DataAccess.DatabaseConnection mainDBConn = WebUtils.GetDatabaseConnection();
        if (mainDBConn!=null)
            WebUtils.SetSessionDatabaseConnection(Session, mainDBConn);

        HROne.SaaS.Entities.EUser user = null;
        string message = string.Empty;
        bool hasError = false;
        try
        {
            WebUtils.ValidateUser(mainDBConn, Username.Text, HROne.CommonLib.Hash.PasswordHash(Password.Text), true, true, out user);
        }
        catch (Exception ex)
        {
            hasError = true;
            message = ex.Message;

        }
        if (!hasError)
        {
            Session["LoginID"] = user.LoginID;
            Session["PasswordEncrypted"] = user.UserPassword;
            if (mainDBConn is HROne.DataAccess.DatabaseConnectionWithAudit)
            {
                ((HROne.DataAccess.DatabaseConnectionWithAudit)mainDBConn).UserID = user.UserID;
            }

            //WebUtils.RefreshPermission(Session);
            bool isForceChangePassword = false;

            if (user.UserChangePasswordUnit == "D")
            {
                if (AppUtils.ServerDateTime() < user.UserChangePasswordDate.AddDays(user.UserChangePasswordPeriod))
                    isForceChangePassword = false;
                else
                    isForceChangePassword = true;
            }
            else if (user.UserChangePasswordUnit == "M")
            {
                if (AppUtils.ServerDateTime() < user.UserChangePasswordDate.AddMonths(user.UserChangePasswordPeriod))
                    isForceChangePassword = false;
                else
                    isForceChangePassword = true;
            }
            else if (user.UserChangePasswordUnit == "Y")
            {
                if (AppUtils.ServerDateTime() < user.UserChangePasswordDate.AddYears(user.UserChangePasswordPeriod))
                    isForceChangePassword = false;
                else
                    isForceChangePassword = true;
            }
            else
                isForceChangePassword = false;
            if (user.UserChangePassword)
                isForceChangePassword = true;

            //EInbox.GenerateInboxMessage(user.UserID);
            if (isForceChangePassword)
                Session["ForceChangePassword"] = true;

            Response.Redirect("~/Default.aspx");
        }
        //  Remove all item for release session
        Session.Clear();
        Request.Cookies.Remove(HROne.Common.WebUtility.getDefaultSessionCookieName());

        {
            message = message.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            message = message.Replace(HROne.Common.WebUtility.GetLocalizedString("validate.prompt"), "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "errorMessage", "alert(\"" + message + "\");", true);
            System.Threading.Thread.Sleep(1000);
        }
    }
}