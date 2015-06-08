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

//  Class name Login should NOT be used to prevent a bug caused by old version of Microsoft.Net framework
public partial class ASPLogin : HROneWebPage
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

        if (Application["MasterDBConfig"] == null)
        {
            SaaSCustomerIDRow.Visible = false;
            DatabaseConnection dbConn = WebUtils.GetDatabaseConnection();
            if (dbConn != null)
            {
                multiDBRow.Visible = false;
                string dbTitle = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_DB_TITLE);
                if (!string.IsNullOrEmpty(dbTitle))
                    Page.Title += " (" + dbTitle + ")";
            }
            else
            {
                multiDBRow.Visible = true;

                if (!IsPostBack)
                {
                    HROneConfig config = HROneConfig.GetCurrentHROneConfig();
                    for (int idx = 0; idx < config.DatabaseConfigList.Count; idx++)
                    {
                        DatabaseConfig dbConfig = config.DatabaseConfigList[idx];
                        cboDatabase.Items.Add(new ListItem(dbConfig.name, idx.ToString()));
                    }
                }
            }
        }
        else
        {
            SaaSCustomerIDRow.Visible = true;
            multiDBRow.Visible = false;
            Page.Title = "iMGR";
        }

        lblVersionNo.Text = ProductVersion.CURRENT_PROGRAM_VERSION;

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
    //        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Emp_List.aspx");

    //}

    protected void Login_Click(object sender, EventArgs e)
    {
        HROne.Lib.Entities.EUser user = null;

        Session.Clear();

        HROne.DataAccess.DatabaseConnection mainDBConn = null;
        HROne.SaaS.Entities.ECompanyDatabase companyDB = null;
        bool hasError = false;
        string message = string.Empty;

        // always check whether companyDB exists after click
        if (Application["MasterDBConfig"] != null)
        {
            DatabaseConfig masterDBConfig = (DatabaseConfig)Application["MasterDBConfig"];
            HROne.DataAccess.DatabaseConnection masterDBConn = masterDBConfig.CreateDatabaseConnectionObject();
            DBFilter companyDatabaseFilter = new DBFilter();
            companyDatabaseFilter.add(new Match("CompanyDBClientCode", txtCustomerID.Text));
            ArrayList companyDatabaseList = HROne.SaaS.Entities.ECompanyDatabase.db.select(masterDBConn, companyDatabaseFilter);
            if (companyDatabaseList.Count > 0)
            {
                companyDB = (HROne.SaaS.Entities.ECompanyDatabase)companyDatabaseList[0];

                DatabaseConfig tempDBConfig = new DatabaseConfig();
                tempDBConfig.ConnectionString = companyDB.getConnectionString(masterDBConn);
                tempDBConfig.DBType = WebUtils.DBTypeEmun.MSSQL;

                if (tempDBConfig.TestConnection())
                {
                    mainDBConn = tempDBConfig.CreateDatabaseConnectionObject();

                    HROne.Lib.Entities.ESystemParameter.setParameter(mainDBConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_PRODUCTKEY, companyDB.CompanyDBProductKey);
                    HROne.Lib.Entities.ESystemParameter.setParameter(mainDBConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_PRODUCTFEATURECODE, companyDB.getProductFeatureCode(masterDBConn));

                    string[] smtpParameterList = new string[]
                        {
                            HROne.Lib.Entities.ESystemParameter.PARAM_CODE_SMTP_SERVER_NAME,
                            HROne.Lib.Entities.ESystemParameter.PARAM_CODE_SMTP_PORT,
                            HROne.Lib.Entities.ESystemParameter.PARAM_CODE_SMTP_USERNAME,
                            HROne.Lib.Entities.ESystemParameter.PARAM_CODE_SMTP_ENABLE_SSL,
                            HROne.Lib.Entities.ESystemParameter.PARAM_CODE_SMTP_SMTP_OUTGOING_EMAIL_ADDRESS
                        };
                    foreach (string smptParameter in smtpParameterList)
                        HROne.Lib.Entities.ESystemParameter.setParameter(mainDBConn, smptParameter, HROne.SaaS.Entities.ESystemParameter.getParameter(masterDBConn, smptParameter));
                    HROne.Lib.Entities.ESystemParameter.setParameterWithEncryption(mainDBConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_SMTP_PASSWORD
                        , HROne.SaaS.Entities.ESystemParameter.getParameterWithEncryption(masterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_SMTP_PASSWORD));
                }
                else
                {
                    if (HROne.DataAccess.DBAESEncryptStringFieldAttribute.InvalidRSAKey)
                    {
                        HttpRuntime.UnloadAppDomain();
                    }
                }
            }
        }
        else
        {
            mainDBConn = WebUtils.GetDatabaseConnection();
            if (mainDBConn == null)
            {

                if (multiDBRow.Visible)
                {
                    int idx;
                    if (int.TryParse(cboDatabase.SelectedValue, out idx))
                    {
                        HROneConfig config = HROneConfig.GetCurrentHROneConfig();
                        DatabaseConfig dbconfig = config.DatabaseConfigList[idx];
                        if (dbconfig.TestConnection())
                        {
                            mainDBConn = dbconfig.CreateDatabaseConnectionObject();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "errorMessage", "alert(\"Fail to connect database\");", true);
                            return;
                        }
                    }
                }
            }
        }
        if (mainDBConn!=null)
            WebUtils.SetSessionDatabaseConnection(Session, mainDBConn);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense!=null)
            if (companyDB == null && productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.HROneSaaS)
            {
                productLicense.LoadProductKey(string.Empty);
            }

        if (mainDBConn == null && companyDB != null)
        {
            hasError = true;
            message = "Fail to connect to database. Please contact to customer service.";
        }
        if (!hasError)
        {
            try
            {
                WebUtils.ValidateUser(mainDBConn, Username.Text, HROne.CommonLib.Hash.PasswordHash(Password.Text), true, true, out user);
            }
            catch (Exception ex)
            {
                hasError = true;
                message = ex.Message;
            }
            // Check the database status after login is verified
            if (companyDB != null && !hasError)
            {
                if (!companyDB.CompanyDBIsActive)
                {
                    hasError = true;
                    message = "Subscribed service has been expired; please contact customer service hotline for assistant.";
                }
            }

            if (!hasError)
            {
                //  Create Login Audit for successfully login
                HROne.Lib.Entities.ELoginAudit.CreateLoginAudit(mainDBConn, user.UserID, Username.Text, Request, AppUtils.ServerDateTime(), false, string.Empty);
                Session["LoginID"] = user.LoginID;
                Session["PasswordEncrypted"] = user.UserPassword;
                if (companyDB != null)
                    Session["CompanyDBID"] = companyDB.CompanyDBID;

                if (mainDBConn is HROne.DataAccess.DatabaseConnectionWithAudit)
                {
                    ((HROne.DataAccess.DatabaseConnectionWithAudit)mainDBConn).UserID = user.UserID;
                }
                WebUtils.SetSessionLanguage(Session, user);

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

                WebUtils.SetSessionLanguage(Session, WebUtils.GetCurUser(Session));
                //RegenerateSessionId();
                HROne.Lib.Entities.EInbox.DeleteAllDeletedUserID(mainDBConn);
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx?LastURL=" + DecryptedRequest["LastURL"]);
            }
        }
        //  Remove all item for release session
        Session.Clear();
        Request.Cookies.Remove(HROne.Common.WebUtility.getDefaultSessionCookieName());
        //if (companyDB != null)
        //{
        //    Session.Remove("DatabaseConnection");
        //}
        {
            //  Create Login Audit for fail login
            if (user != null)
                HROne.Lib.Entities.ELoginAudit.CreateLoginAudit(mainDBConn, user.UserID, Username.Text, Request, AppUtils.ServerDateTime(), true, message);
            else
                HROne.Lib.Entities.ELoginAudit.CreateLoginAudit(mainDBConn, 0, Username.Text, Request, AppUtils.ServerDateTime(), true, message);

            message = message.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            message = message.Replace(HROne.Common.WebUtility.GetLocalizedString("validate.prompt"), "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "errorMessage", "alert(\"" + message + "\");", true);
            System.Threading.Thread.Sleep(1000);
        }
    }

    //string RegenerateSessionId()
    //{
    //    System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
    //    string oldId = manager.GetSessionID(Context);
    //    manager.RemoveSessionID(Context);
    //    string newId = manager.CreateSessionID(Context);
    //    bool isAdd = false, isRedir = false;
    //    manager.SaveSessionID(Context, newId, out isRedir, out isAdd);

    //    System.Web.SessionState.HttpSessionState state = Context.Session;
    //    System.Web.SessionState.SessionStateItemCollection items = new System.Web.SessionState.SessionStateItemCollection();
    //    HttpStaticObjectsCollection staticObjects = System.Web.SessionState.SessionStateUtility.GetSessionStaticObjects(Context);
    //    System.Web.SessionState.HttpSessionStateContainer replacement = new System.Web.SessionState.HttpSessionStateContainer(
    //             newId, items, staticObjects, state.Timeout, false, state.CookieMode, state.Mode, state.IsReadOnly);
    //    foreach (string key in Context.Session.Keys)
    //    {
    //        replacement.Add(key, Context.Session[key]);
    //    }
    //    System.Web.SessionState.SessionStateUtility.RemoveHttpSessionStateFromContext(Context);
    //    System.Web.SessionState.SessionStateUtility.AddHttpSessionStateToContext(Context, replacement);

    //    //HttpApplication ctx = (HttpApplication)HttpContext.Current.ApplicationInstance;
    //    //HttpModuleCollection mods = ctx.Modules;
    //    //System.Web.SessionState.SessionStateModule ssm = (System.Web.SessionState.SessionStateModule)mods.Get("Session");
    //    //System.Reflection.FieldInfo[] fields = ssm.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //    //System.Web.SessionState.SessionStateStoreProviderBase store = null;
    //    //System.Reflection.FieldInfo rqIdField = null, rqLockIdField = null, rqStateNotFoundField = null;
    //    //foreach (System.Reflection.FieldInfo field in fields)
    //    //{
    //    //    if (field.Name.Equals("_store")) store = (System.Web.SessionState.SessionStateStoreProviderBase)field.GetValue(ssm);
    //    //    if (field.Name.Equals("_rqId")) rqIdField = field;
    //    //    if (field.Name.Equals("_rqLockId")) rqLockIdField = field;
    //    //    if (field.Name.Equals("_rqSessionStateNotFound")) rqStateNotFoundField = field;
    //    //}
    //    //object lockId = rqLockIdField.GetValue(ssm);
    //    //if ((lockId != null) && (oldId != null)) store.ReleaseItemExclusive(Context, oldId, lockId);
    //    //rqStateNotFoundField.SetValue(ssm, true);
    //    //rqIdField.SetValue(ssm, newId);
    //    return newId;
    //}
}