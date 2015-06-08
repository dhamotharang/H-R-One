using System;
using System.Security.Cryptography;
using System.Text;
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
using HROne.SaaS.Entities;

public partial class UserChangePassword : HROneWebPage
{
    public Binding binding;
    public DBManager db = EUser.db;
    public EUser obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        bool skipAccessChecking = false;
        if (Session["ForceChangePassword"] != null)
            if (Session["ForceChangePassword"].Equals(true))
                skipAccessChecking = true;

        if (!skipAccessChecking)
            if (!WebUtils.CheckAccess(Response, Session))
                return;


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
        

    }

    protected void Save_Click(object sender, EventArgs e)
    {



        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        EUser user = WebUtils.GetCurUser(Session);
        EUser.db.select(dbConn, user);
        if (user.UserPassword == null)
            user.UserPassword = "";
        string h = HROne.CommonLib.Hash.PasswordHash(OldPassword.Text);
        if(!user.UserPassword.Equals(h))
            errors.addError("OldPassword", HROne.Translation.PageErrorMessage.ERROR_INCORRECT_PASSWORD);

        if(!Password.Text.Equals(Password2.Text))
            errors.addError("Password2", HROne.Translation.PageErrorMessage.ERROR_CONFIRM_PASSWORD);

        if (Password.Text.Equals(OldPassword.Text))
            errors.addError("Password", HROne.Translation.PageErrorMessage.ERROR_PASSWORD_SAME_AS_OLD_PASSWORD);

        if (!errors.isEmpty())
            return;

        user.UserPassword = HROne.CommonLib.Hash.PasswordHash(Password.Text);
        user.UserChangePasswordDate = AppUtils.ServerDateTime();
        user.UserChangePassword = false;

        EUser.db.update(dbConn, user);

        Session.Remove("ForceChangePassword");
        WebUtils.RegisterRedirectJavaScript(this, "Home.aspx", HROne.Common.WebUtility.GetLocalizedString("Password changed"));

    }
}
