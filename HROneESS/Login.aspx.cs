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
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, Request.Url.PathAndQuery);
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
            Page.Title = "iStaff";
        }

        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");

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

    protected void Login_Click(object sender, EventArgs e)
    {
        EESSUser user = null;

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
                        HROne.Lib.Entities.ESystemParameter.setParameter(mainDBConn, smptParameter, ESystemParameter.getParameter(masterDBConn, smptParameter));
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
        if (mainDBConn != null)
            WebUtils.SetSessionDatabaseConnection(Session, mainDBConn);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);
        if (productLicense != null)
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
            bool ChangePassword = false;
            try
            {
                WebUtils.ValidateUser(mainDBConn, Username.Text, HROne.CommonLib.Hash.PasswordHash(Password.Text), true, out ChangePassword, out user);
            }
            catch (Exception ex)
            {
                hasError = true;
                message = ex.Message;
            }
			// copy from GAP customization, checking Active Directory ID
            if (user != null)
            {
                DBFilter ADIDFilter = new DBFilter();
                ADIDFilter.add(new Match("EmpExtraFieldName", "AD ID"));
                DBFilter ADIDValueFilter = new DBFilter();
                ADIDValueFilter.add(new IN("EmpExtraFieldID", "Select EmpExtraFieldID from " + EEmpExtraField.db.dbclass.tableName, ADIDFilter));
                ADIDValueFilter.add(new Match("EmpID", user.EmpID));

                ArrayList ADIDList = EEmpExtraFieldValue.db.select(dbConn, ADIDValueFilter);
                if (ADIDList.Count > 0)
                {
                    string remoteUser = Request.ServerVariables["REMOTE_USER"].Trim();
                    string ADID = ((EEmpExtraFieldValue)ADIDList[0]).EmpExtraFieldValue.Trim();

                    if (!remoteUser.Equals(ADID, StringComparison.CurrentCultureIgnoreCase) && !remoteUser.EndsWith("\\" + ADID, StringComparison.CurrentCultureIgnoreCase) && !remoteUser.EndsWith("/" + ADID, StringComparison.CurrentCultureIgnoreCase))
                    {
                        hasError = true;
                        message = "This PC is not logged with your network ID!";
                    }
                }
            }
            if (companyDB != null && !hasError)
            {
                if (!companyDB.CompanyDBIsActive)
                {
                    hasError = true;
                    message = "Subscribed service has been expired; please contact customer service hotline for assistant.";
                }
                if (!productLicense.IsESS)
                {
                    hasError = true;
                    message = "You are NOT subscribed the service.";
                } 

            }

            if (!hasError)
            {

                HROne.Lib.Entities.EESSLoginAudit.CreateLoginAudit(mainDBConn, user.EmpID, Username.Text, Request, AppUtils.ServerDateTime(), false, string.Empty);

                // ADD the Session "User" Value is user
                Session["User"] = user;

                if (ChangePassword)
                    Session.Add("ForceChangePassword", true);
                WebUtils.SetSessionLanguage(Session, user);
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
            if (user != null)
                HROne.Lib.Entities.EESSLoginAudit.CreateLoginAudit(mainDBConn, user.EmpID, Username.Text, Request, AppUtils.ServerDateTime(), true, message);
            else
                HROne.Lib.Entities.EESSLoginAudit.CreateLoginAudit(mainDBConn, 0, Username.Text, Request, AppUtils.ServerDateTime(), true, message);

            //message = message.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            //message = message.Replace(HROne.Common.WebUtility.GetLocalizedString("validate.prompt"), "");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "errorMessage", "alert(\"" + message + "\");", true);
            Prompt.Text = message;
            System.Threading.Thread.Sleep(1000);
        }
    }
}
