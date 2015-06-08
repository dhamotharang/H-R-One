using System;
using HROne.Lib.Entities;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using HROne.Import;
using HROne.DataAccess;

namespace HROne
{
    /// <summary>
    /// Summary description for Global.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
            
        }

        protected void Application_Start(Object sender, EventArgs e)
        {
            Application["ActiveUsers"] = 0;

            //HROne.DataAccess.DatabaseConnection dbConn = WebUtils.GetDatabaseConnection();
            //perspectivemind.common.Debug.debug = true;

            HROne.Common.Folder.ClearApplicationTempFolder();

            HROneConfig configure = HROneConfig.GetCurrentHROneConfig();
            // Start 0000134, Ricky So, 2014-11-21
            HROne.DataAccess.DBAESEncryptStringFieldAttribute.skipEncryptionToDB = configure.SkipEncryptionToDB;
            // End 0000134, Ricky So, 2014-11-21
            HROne.DataAccess.DBAESEncryptStringFieldAttribute.RSAEncryptedKeyString = configure.databaseEncryptKey;
            if (configure.MasterDatabaseConfig != null)
            {
                Application["MasterDBConfig"] = configure.MasterDatabaseConfig;
                DatabaseConnection MasterDBConn = configure.MasterDatabaseConfig.CreateDatabaseConnectionObject();
                AppUtils.DefaultDocumentUploadFolder = HROne.SaaS.Entities.ESystemParameter.getParameter(MasterDBConn, HROne.SaaS.Entities.ESystemParameter.PARAM_CODE_DEFAULT_DOCUMENT_FOLDER);
                //  force RSA mode on SaaS
                if (string.IsNullOrEmpty(configure.databaseEncryptKey))
                    HROne.DataAccess.DBAESEncryptStringFieldAttribute.RSAEncryptedKeyString = "0123456789abcdef";
            }
            // run Import_Employee.sql ONLY when there is a DB patch
            //foreach (DatabaseConfig dbConfig in configure.DatabaseConfigList)
            //{
            //    if (dbConfig.TestConnection())
            //    {
            //        HROne.ProductVersion.Database database = new HROne.ProductVersion.Database(dbConfig.CreateDatabaseConnectionObject());
            //        try
            //        {
            //            database.RunDataPatch("Import_Employee.sql");
            //            //WebUtils.ClearTempTable(dbConfig.CreateDatabaseConnectionObject(), string.Empty);
            //        }
            //        catch (Exception ex)
            //        {
            //            System.Diagnostics.Debug.WriteLine(ex.Message);
            //        }
            //    }
            //}
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Request.Url.LocalPath.EndsWith(".aspx"))
            {
                DatabaseConnection dbConn;
                if (Request["Database"] != null)
                {
                    dbConn = WebUtils.GetDatabaseConnection(Request["Database"]);
                    WebUtils.Logout(Session);
                    if (dbConn != null)
                        WebUtils.SetSessionDatabaseConnection(Session, dbConn);
                }
                else
                    dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
                if (dbConn != null)
                {
                    //DatabaseConnection.SetDefaultDatabaseConnection(dbConn);
                    if (!Request.Url.LocalPath.EndsWith("/DatabaseUpgrade.aspx", StringComparison.CurrentCultureIgnoreCase))
                    {
                        HROne.ProductVersion.Database database = new HROne.ProductVersion.Database(dbConn);
                        if (database.IsUpdateRequired() && Session["NeedDBUpgrade"] != null)
                            if (Session["NeedDBUpgrade"].Equals(true))
                            {
                                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/DatabaseUpgrade.aspx");
                            }
                    }
                }
                if (!Request.Url.LocalPath.EndsWith("/DatabaseConfiguration.aspx", StringComparison.CurrentCultureIgnoreCase) && !Request.Url.LocalPath.EndsWith("/SelectDatabase.aspx", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Application["MasterDBConfig"] == null)
                    {
                        HROneConfig configure = HROneConfig.GetCurrentHROneConfig();
                        if (configure.DatabaseConfigList.Count < 1)
                            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/DatabaseConfiguration.aspx");
                        //else
                        //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/SelectDatabase.aspx");
                    }
                    //if (Request.Url.LocalPath.IndexOf("/Login.aspx") < 0)
                    //{
                    //    string LastPath = Request.Url.PathAndQuery;
                    //    if (Request.Url.LocalPath.IndexOf("/Default.aspx") > 0 || Request.Url.LocalPath.EndsWith("/"))
                    //        Response.Redirect("~/Login.aspx");
                    //    else
                    //        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx?LastURL=" + LastPath);
                    //}
                }
                HROne.Common.WebUtility.initLanguage(Session);
            }
        }

        //protected void Application_AcquireRequestState(object source, EventArgs args)
        //{
        //    HttpApplication app = (HttpApplication)source;
        //    HttpContext context = app.Context;
        //    bool isNew = false;
        //    string sessionID;
        //    HttpSessionState sessionData = null;
        //    bool supportSessionIDReissue = true;

        //    pSessionIDManager.InitializeRequest(context, false, out supportSessionIDReissue);
        //    sessionID = pSessionIDManager.GetSessionID(context);


        //    if (sessionID != null)
        //    {
        //        try
        //        {
        //            pHashtableLock.AcquireReaderLock(Int32.MaxValue);
        //            sessionData = (SessionItem)pSessionItems[sessionID];

        //            if (sessionData != null)
        //                sessionData.Expires = DateTime.Now.AddMinutes(pTimeout);
        //        }
        //        finally
        //        {
        //            pHashtableLock.ReleaseReaderLock();
        //        }
        //    }
        //    else
        //    {
        //        bool redirected, cookieAdded;

        //        sessionID = pSessionIDManager.CreateSessionID(context);
        //        pSessionIDManager.SaveSessionID(context, sessionID, out redirected, out cookieAdded);

        //        if (redirected)
        //            return;
        //    }

        //    if (sessionData == null)
        //    {
        //        // Identify the session as a new session state instance. Create a new SessionItem
        //        // and add it to the local Hashtable.

        //        isNew = true;

        //        sessionData = new SessionItem();

        //        sessionData.Items = new SessionStateItemCollection();
        //        sessionData.StaticObjects = SessionStateUtility.GetSessionStaticObjects(context);
        //        sessionData.Expires = DateTime.Now.AddMinutes(pTimeout);

        //        try
        //        {
        //            pHashtableLock.AcquireWriterLock(Int32.MaxValue);
        //            pSessionItems[sessionID] = sessionData;
        //        }
        //        finally
        //        {
        //            pHashtableLock.ReleaseWriterLock();
        //        }
        //    }

        //    // Add the session data to the current HttpContext.
        //    SessionStateUtility.AddHttpSessionStateToContext(context,
        //                     new HttpSessionStateContainer(sessionID,
        //                                                  sessionData.Items,
        //                                                  sessionData.StaticObjects,
        //                                                  pTimeout,
        //                                                  isNew,
        //                                                  pCookieMode,
        //                                                  SessionStateMode.Custom,
        //                                                  false));

        //    // Execute the Session_OnStart event for a new session.
        //    if (isNew && Start != null)
        //    {
        //        Start(this, EventArgs.Empty);
        //    }
        //}

        protected void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
        }


        void Global_PostAcquireRequestState(object sender, EventArgs e)
        {
            //if (Request.Url.LocalPath.EndsWith(".aspx"))
            //{
            //    if (Request.Url.LocalPath.EndsWith("Default.aspx"))
            //        return;

            //    try
            //    {
            //        EUser user = (EUser)Session["User"];
            //        if (user == null)
            //        {
            //            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Default.aspx");
            //            Response.End();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Default.aspx");
            //        Response.End();

            //    }

            //}
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] + 1;
            Application.UnLock();

            //Session["IgnoreEM"] = false;
            DatabaseConnection dbConn = null;
            if (Application["MasterDBConfig"] == null)
            {

                dbConn = WebUtils.GetDatabaseConnection();
                if (dbConn != null)
                {
                    WebUtils.SetSessionDatabaseConnection(Session, dbConn);
                }
            }
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string appPath = Request.ApplicationPath;
            if (Request.Url.PathAndQuery.StartsWith(appPath, StringComparison.CurrentCultureIgnoreCase)
                && !Request.Url.PathAndQuery.StartsWith(appPath, StringComparison.CurrentCulture))
                Response.Redirect("~" + Request.Url.PathAndQuery.Substring(appPath.Length));

            string SessionCookieName = HROne.Common.WebUtility.getDefaultSessionCookieName();
            if (Request.Cookies[SessionCookieName] != null && Request.Cookies[SessionCookieName].Value != null)
            {
                string newSessionID = Request.Cookies[SessionCookieName].Value;
                if (newSessionID.Length <= 24)
                {
                    //Log the attack details here
                    //throw new HttpException("Invalid Request");
                    Request.Cookies.Remove(SessionCookieName);
                }
                else if (GenerateHashKey() != newSessionID.Substring(24))
                {
                    //Log the attack details here
                    //throw new HttpException("Invalid Request");
                    Request.Cookies.Remove(SessionCookieName);
                }

                //Use the default one so application will work as usual//ASP.NET_SessionId
                if (Request.Cookies[SessionCookieName] != null)
                    Request.Cookies[SessionCookieName].Value = Request.Cookies[SessionCookieName].Value.Substring(0, 24);
            }

            if (Request.Url.LocalPath.EndsWith(".aspx") 
                || Request.Url.LocalPath.EndsWith(".htm")
                || Request.Url.LocalPath.EndsWith(".html")
                || Request.Url.LocalPath.EndsWith(".css")
                || Request.Url.LocalPath.EndsWith(".axd"))
            {
                //if (Request.UserAgent.IndexOf("AppleWebKit") > 0)
                //    Request.Browser.Adapters.Clear();

                //  Do NOT set SetCacheability to HttpCacheability.NoCache 
                //  to prevent download problem on IE8 + SSL 
                //  Unless it is make sure that "Response.ClearHeaders()" is include before download procedure
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetSlidingExpiration(true);
                Response.Cache.SetExpires(DateTime.Now);
                Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0));
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                Response.Cache.SetValidUntilExpires(false);
                Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            }
            else
            {
                Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));
            }
            //Response.Cache.SetExpires(new DateTime(0));
            //Response.Expires = -1;
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            string SessionCookieName = HROne.Common.WebUtility.getDefaultSessionCookieName();
            if (Response.Cookies[SessionCookieName] != null)
                if (!string.IsNullOrEmpty(Request.Cookies[SessionCookieName].Value))
                {
                    if (!string.IsNullOrEmpty(Response.Cookies[SessionCookieName].Value))
                        Response.Cookies[SessionCookieName].Value = Response.Cookies[SessionCookieName].Value + GenerateHashKey();
                    else
                    {
                        if (Request.Cookies[SessionCookieName].Value.Length <= 24)
                            Response.Cookies[SessionCookieName].Value = Request.Cookies[SessionCookieName].Value + GenerateHashKey();
                        else
                            return;
                    }
                    if (Response.Cookies[SessionCookieName].Value.Length > 52)
                        throw new Exception();
                    if (Request.IsSecureConnection)
                        Response.Cookies[SessionCookieName].Secure = true;
                    Response.Cookies[SessionCookieName].Path = Request.ApplicationPath;
                }
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            

        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            HROne.Common.Folder.GetOrCreateSessionTempFolder(Session.SessionID).Delete(true);
            DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            if (dbConn != null)
            {
                WebUtils.ClearTempTable(dbConn, Session.SessionID);
            }

            Application.Lock();
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] - 1;
            if ((int)Application["ActiveUsers"] == 0)
            {
                HROneConfig config = HROneConfig.GetCurrentHROneConfig();
                if (config.ShutDownDomainAfterUsed)
                    HttpRuntime.UnloadAppDomain();
            }
            Application.UnLock();

        }

        protected void Application_End(Object sender, EventArgs e)
        {
            HROne.Common.Folder.ClearApplicationTempFolder();
            //if (WebUtils.GetDatabaseConnection() != null)
            //{
            //    WebUtils.ClearTempTable(string.Empty);
            //}
        }

        string GenerateHashKey()
        {
            //  Reduce browser checking constraint for improving stability when switching compatibility of IE or non-desktop browser 
            System.Text.StringBuilder myStr = new System.Text.StringBuilder();
            myStr.Append(Request.Browser.Browser);
            myStr.Append(Request.Browser.Platform);
            //myStr.Append(Request.Browser.MajorVersion);
            //myStr.Append(Request.Browser.MinorVersion);
            //myStr.Append(Request.LogonUserIdentity.User.Value);
            myStr.Append(Request.UserHostAddress);
            myStr.Append(Request.UserHostName);

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(myStr.ToString()));
            return Convert.ToBase64String(hashdata);
        }
    }
}

