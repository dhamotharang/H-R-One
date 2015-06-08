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

public partial class ESS_ChangePassword : HROneWebPage
{
    public Binding binding;
    public DBManager db = EEmpPersonalInfo.db;
    public EEmpPersonalInfo obj;
    public int CurID = -1;

    protected void Page_Prerender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            cbxPARAM_CODE_DEFAULT_LANGUAGE.Items.Add(new ListItem("Default", ""));
            WebUtils.AddLanguageOptionstoDropDownList(cbxPARAM_CODE_DEFAULT_LANGUAGE);
            if (string.IsNullOrEmpty(obj.EmpESSLanguage))
            {
                cbxPARAM_CODE_DEFAULT_LANGUAGE.SelectedIndex = 0;
            }
            else
            {
                try
                {
                    cbxPARAM_CODE_DEFAULT_LANGUAGE.SelectedValue = obj.EmpESSLanguage;
                }
                catch
                {
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool skipAccessChecking = false;
        if (Session["ForceChangePassword"] != null)
            if (Session["ForceChangePassword"].Equals(true))
                skipAccessChecking = true;

        if (!skipAccessChecking)
            if (!WebUtils.CheckAccess(Response, Session))
                return;

        binding = new Binding(dbConn, db);
        binding.add(EmpID);
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        EESSUser user = WebUtils.GetCurUser(Session);
        if (user != null)
        {
            CurID = user.EmpID;
        }


        if (!Page.IsPostBack)
        {

            if (CurID > 0)
            {
                loadObject();
            }
            else
            {

            }
        }

    }


    protected bool loadObject()
    {
        obj = new EEmpPersonalInfo();
        obj.EmpID = CurID;

        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);
        return true;
    }

    protected void SaveLanguage_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurID));
        ArrayList list = EESSUser.db.select(dbConn, filter);

        EESSUser user = (EESSUser)list[0];

        user.EmpESSLanguage = this.cbxPARAM_CODE_DEFAULT_LANGUAGE.SelectedValue;

        EESSUser.db.update(dbConn, user);
        //string redirectURL = "ESS_Home.aspx";
        
        //WebUtils.SetSessionLanguage(Session, user);
        //HROne.Common.WebUtility.initLanguage(Session);

        WebUtils.RegisterRedirectJavaScript(this, null , HROne.Common.WebUtility.GetLocalizedStringByCode("Please re-login", "Please logout and login again to have the system language to take effect"));

        return;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();


        DBFilter filter = new DBFilter();
        filter.add(new Match("EmpID", CurID));
        ArrayList list = EESSUser.db.select(dbConn, filter);

        // Start 000172, Ricky So, 2015-02-05
        if (NewPassword.Text.Equals(""))
        {
            errors.addError("New Password", HROne.Translation.PageErrorMessage.ERROR_FIELD_REQUIRED);
            return;
        }
        // End 000172, Ricky So, 2015-02-05

        EESSUser user = (EESSUser)list[0];
        if (!(ReTypePassword.Text == NewPassword.Text))
        {
            errors.addError("Password2", HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_CONFIRM_PASSWORD", "Confirm Password does not match"));
            return;
        }
        if (string.IsNullOrEmpty(user.EmpPW ))
        {
            if (!(user.EmpHKID == "()") && !string.IsNullOrEmpty(user.EmpHKID))
                user.EmpPW = WebUtils.PasswordHash(user.EmpHKID.Substring(0, (user.EmpHKID.Length - 3)));
            else
                user.EmpPW = WebUtils.PasswordHash(user.EmpPassportNo);
        }

        //if (user.EmpPW == user.EmpPassportNo)
        //{
        //    if (!user.EmpPW.Equals(Password.Text))
        //    {
        //        Prompt.Text = HROne.Common.WebUtility.GetLocalizedStringByCode("ERROR_INCORRECT_PASSWORD", "Incorrect password"); ;
        //        return;
        //    }
        //}
        //else
        {
            if (!user.EmpPW.Equals(WebUtils.PasswordHash(Password.Text)))
            {
                errors.addError("OldPassword", "ERROR_INCORRECT_PASSWORD", "Incorrect password");
                return;
            }
            else
                user.EmpPW = WebUtils.PasswordHash(NewPassword.Text);
        }
        EESSUser.db.update(dbConn, user);
        string redirectURL = "ESS_Home.aspx";
        if (Session["ForceChangePassword"] != null)
        {
            Session.Remove("ForceChangePassword");
        //    if (Session["LastURL"] != null)
        //        redirectURL = ((Uri)Session["LastURL"]).AbsolutePath;
        }
        WebUtils.RegisterRedirectJavaScript(this, redirectURL, HROne.Common.WebUtility.GetLocalizedStringByCode("Password has been changed successfully", "Password has been changed successfully"));

        return;

    }
}
