using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Web.SessionState;
using System.Web;
using System.Web.UI;
using System.IO;
using HROne.DataAccess;
using HROne.SaaS.Entities;
using System.Web.UI.WebControls;

public class HROneWebMasterPage : System.Web.UI.MasterPage
{
    protected DatabaseConnection m_dbConn = null;

    public DatabaseConnection dbConn
    {
        get
        {
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
}

public class HROneWebPage : System.Web.UI.Page
{
    protected DatabaseConnection m_dbConn = null;
    protected HROne.SaaS.Entities.EUser m_user = null;
    protected System.Collections.Specialized.NameValueCollection m_decryptedRequest = null;

    protected System.Collections.Specialized.NameValueCollection DecryptedRequest
    {
        get
        {
            if (m_decryptedRequest == null)
                m_decryptedRequest = HROne.Common.WebUtility.getDecryptQueryStringCollection(Session, Request.Url.Query);
            return m_decryptedRequest;
        }

    }
    public DatabaseConnection dbConn
    {
        get
        {
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
    public HROne.SaaS.Entities.EUser user
    {
        get
        {
            if (m_user == null)
                m_user = WebUtils.GetCurUser(Session);
            return m_user;
        }
    }
}

public class HROneWebControl : System.Web.UI.UserControl
{
    protected System.Globalization.CultureInfo m_ci = null;
    protected DatabaseConnection m_dbConn = null;

    protected System.Collections.Specialized.NameValueCollection m_decryptedRequest = null;

    public System.Collections.Specialized.NameValueCollection DecryptedRequest
    {
        get
        {
            if (m_decryptedRequest == null)
                m_decryptedRequest = HROne.Common.WebUtility.getDecryptQueryStringCollection(Session, Request.Url.Query);
            return m_decryptedRequest;
        }

    }

    public DatabaseConnection dbConn
    {
        get
        {
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
    public System.Globalization.CultureInfo ci
    {
        get
        {
            if (m_ci == null)
                m_ci = HROne.Common.WebUtility.GetSessionUICultureInfo(Session);
            return m_ci;
        }
    }
}

/// <summary>
/// Summary description for WebUtils
/// </summary>
public abstract class WebUtils
{
    public static string CreateConfirmDialogJavascript(string message)
    {
        message = message.Replace(@"\", @"\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
        return "if (!confirm('" + message + "')) return false;";
    }

    public static HROne.SaaS.Entities.EUser GetCurUser(HttpSessionState Session)
    {
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        HROne.SaaS.Entities.EUser CurrentUser = null;
        if (Session["User"] == null)
        {
            if (Session["LoginID"] != null)
            {
                HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);
                if (Session["LoginID"].ToString().Equals(crypto.Encrypting("EM", Session.SessionID)))
                {
                    CurrentUser = new HROne.SaaS.Entities.EUser();
                    CurrentUser.LoginID = "EM";
                    CurrentUser.UserID = -1;
                }
                if (Session["PasswordEncrypted"] != null)
                {
                    HROne.SaaS.Entities.EUser user = null;
                    if (ValidateUser(dbConn, Session["LoginID"].ToString(), Session["PasswordEncrypted"].ToString(), false, false, out user))
                        CurrentUser = user;
                    else
                    {
                        Session.Remove("LoginID");
                        Session.Remove("PasswordEncrypted");
                    }
                }
            }
            if (CurrentUser != null)
                Session["User"] = CurrentUser;
        }
        else
            CurrentUser = (HROne.SaaS.Entities.EUser)Session["User"];
        return CurrentUser;
    }

    public static void RegisterRedirectJavaScript(Control ctrl, string url, int delayMillSecond)
    {
        url = HROne.Common.WebUtility.URLwithEncryptQueryString(ctrl.Page.Session, url);
        string javascriptString = "window.open('" + url + "','_self');";
        if (delayMillSecond < 100)
            delayMillSecond = 100;
        if (delayMillSecond > 0)
        {
            javascriptString = "setTimeout(\"" + javascriptString + "\"," + delayMillSecond.ToString() + ");";
        }
        ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "redirect", javascriptString, true);
    }
    public static void RegisterRedirectJavaScript(Control ctrl, string url, string alertMessage)
    {
        url = HROne.Common.WebUtility.URLwithEncryptQueryString(ctrl.Page.Session, url);
        string javascriptString = "window.open('" + url + "','_self');";
        if (!string.IsNullOrEmpty(alertMessage))
        {
            alertMessage = alertMessage.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            //javascriptString = "alert(\"" + alertMessage + "\"); " + javascriptString + "";
            javascriptString = "messagePopupPostBackScript=\"" + javascriptString + "\"; messagePopupDetail=\"" + alertMessage + "\";";
        }
        ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "redirect", javascriptString, true);
    }

    public static bool ValidateUser(DatabaseConnection dbConn, string username, string encryptedPassword, bool throwException, bool CheckFailCount, out HROne.SaaS.Entities.EUser user)
    {
        user = null;
        string message = string.Empty;
        DBFilter filter = new DBFilter();
        filter.add(new Match("LoginID", username));
        filter.add(new Match("UserAccountStatus", "<>", "D"));
        ArrayList list = HROne.SaaS.Entities.EUser.db.select(dbConn, filter);
        if (list.Count == 0)
        {
            if (throwException)
            {
                message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name or Password");
                throw new Exception(message);
            }
            return false;
        }
        user = (HROne.SaaS.Entities.EUser)list[0];
        if (user.UserPassword == null)
            user.UserPassword = "";
        if (!(user.UserAccountStatus == "A"))
        {
            if (throwException)
            {
                message = HROne.Common.WebUtility.GetLocalizedString("Account is Inactive/Locked");
                throw new Exception(message);
            }
            return false;
        }

        if (!user.UserPassword.Equals(encryptedPassword))
        {
            message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name or Password");
            if (CheckFailCount)
            {
                user.FailCount++;
                //string maxFailCountParameterString = ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_LOGIN_MAX_FAIL_COUNT);
                //if (!maxFailCountParameterString.Equals(string.Empty))
                //{
                //    int MaxFailCount = 0;
                //    if (int.TryParse(maxFailCountParameterString, out MaxFailCount))
                //        if (MaxFailCount > 0)
                //            if (user.FailCount >= MaxFailCount)
                //            {
                //                user.UserAccountStatus = "I";
                //                user.FailCount = 0;
                //                message += "\r\n" + HROne.Common.WebUtility.GetLocalizedString("Account is Locked");
                //            }
                //            else if (MaxFailCount - user.FailCount == 1)
                //            {
                //                message += "\r\n" + HROne.Common.WebUtility.GetLocalizedString("The account will be locked if you fail to login 1 more time");
                //            }


                //}
                HROne.SaaS.Entities.EUser.db.update(dbConn, user);
            }
            if (throwException)
            {
                throw new Exception(message);
            }
            return false;
        }

        if (CheckFailCount)
        {
            user.FailCount = 0;
            HROne.SaaS.Entities.EUser.db.update(dbConn, user);
        }
        return true;

    }

    public static bool CheckAccess(HttpResponse Response, HttpSessionState Session)
    {
        return CheckAccess(Response, Session, string.Empty);
    }

    public static bool CheckAccess(HttpResponse Response, HttpSessionState Session, string function)
    {
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        if (GetCurUser(Session) == null)
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx");
            return false;
        }

        //if (IsSuperUserMissing(dbConn) && ((bool)Session["IgnoreEM"]) != true)
        //{
        //    if (function.Equals("SEC001") || function.Equals("SEC002"))
        //        return true;
        //    else
        //    {
        //        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EM_Warning.aspx");
        //        return false;
        //    }
        //}
        if (Session["ForceChangePassword"] != null)
            if (Session["ForceChangePassword"].Equals(true))
            {
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ChangePassword.aspx");
            }



        EUser user = GetCurUser(Session);
        //if (IsEMUser(user))
        //    HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Logout.aspx");
        if (!string.IsNullOrEmpty(function))
            if (!CheckPermission(Session, function))
            {
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
                Response.End();
                return false;
            }
        return true;
    }

    public static bool CheckPermission(HttpSessionState Session, string function)
    {
        EUser user = WebUtils.GetCurUser(Session);
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        if (user != null)
        {
            //if (WebUtils.IsEMUser(user) && (function.Equals("SEC001") || function.Equals("SEC002")))
            //    if (IsSuperUserMissing(dbConn))
            //        return true;
            //    else
            //        return false;

            DBFilter UserFilter = new DBFilter();
            UserFilter.add(new Match("UserID", user.UserID));


            DBFilter systemFunctionFilter = new DBFilter();
            systemFunctionFilter.add(new Match("FunctionCode", function));
            UserFilter.add(new IN("FunctionID", "Select FunctionID from " + ESystemFunction.db.dbclass.tableName, systemFunctionFilter));

            if (EUserFunction.db.count(dbConn, UserFilter)>0)
                return true;

        }
        return false;
    }

    public interface HROneDBConfigUIInterface
    {
        DatabaseConfig GenerateDBType();
    }
    public enum DBTypeEmun
    {
        None = 0,
        MSSQL = 1

    }

    public static DatabaseConnection GetDatabaseConnection()
    {
        HROneSaaSConfig config = HROneSaaSConfig.GetCurrentConfig();
        if (System.IO.File.Exists(config.HROneConfigFullPath))
        {
            HROneConfig HROneConfig = new HROneConfig(config.HROneConfigFullPath);
            return HROneConfig.GetDatabaseConnection();
        }
        return null;
    }
    public static void SetSessionDatabaseConnection(HttpSessionState Session, DatabaseConnection dbConn)
    {
        Session["DatabaseConnection"] = dbConn;
        //DatabaseConnection.SetDefaultDatabaseConnection(dbConn);

        if (!ProductVersion.UpdateDatabaseVersion(dbConn, true))
            Session["NeedDBUpgrade"] = true;

        string parameterValue = string.Empty;



    }

    public static void Logout(HttpSessionState Session)
    {
        if (Session != null)
        {

            Session.Clear();
            Session.Abandon();
            //DatabaseConnection dbConn = WebUtils.GetDatabaseConnection();
            //if (dbConn != null)
            //{
            //    WebUtils.SetSessionDatabaseConnection(Session, dbConn);
            //}

        }
    }

    public static DataTable GetDataTableFromSelectQueryWithFilter(DatabaseConnection dbConn, string select, string from, DBFilter filter, ListInfo info)
    {
        DBFilter queryFilter = new DBFilter();
        ArrayList afterFilterList = new ArrayList();
        foreach (DBTerm term in filter.terms())
        {
            if (term is Match)
            {
                Match match = (Match)term;
                if (match.value is string && match.op.Equals("Like", StringComparison.CurrentCultureIgnoreCase))
                {
                    afterFilterList.Add(term);
                }
                else
                    queryFilter.add(term);
            }
            else
                queryFilter.add(term);
        }

        DataTable table = null;
        if (dbConn != null)
            table = queryFilter.loadData(dbConn, null, select, from, null);
        //else
        //{
        //    dbConn = DatabaseConnection.GetDatabaseConnection();
        //    table = queryFilter.loadData(dbConn, null, select, from);
        //}
        foreach (Match match in afterFilterList)
        {
            DBAESEncryptStringFieldAttribute.decode(table, match.name);
            DataView view = new DataView(table);
            view.RowFilter = match.name + " " + (string.IsNullOrEmpty(match.op) ? "=" : match.op) + " '" + ((string)match.value).Replace("'", "''") + "' ";
            table = view.ToTable();
        }

        if (info != null)
        {
            //if (!string.IsNullOrEmpty(info.orderby))
            //    if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        if (!table.Columns.Contains("EmpEngFullName"))
            //        {
            //            table.Columns.Add("EmpEngFullName", typeof(string));
            //            foreach (System.Data.DataRow row in table.Rows)
            //            {
            //                EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
            //                empInfo.EmpID = (int)row["EmpID"];
            //                if (EEmpPersonalInfo.db.select(dbConn, empInfo))
            //                    row["EmpEngFullName"] = empInfo.EmpEngFullName;
            //            }
            //        }
            //    }
            return DataTableSortingAndPaging(table, info);
        }
        else
            return table;
    }

    public static DataTable DataTableSortingAndPaging(DataTable sourceTable, ListInfo info)
    {
        DataTable table = sourceTable.Copy();
        if (info != null)
        {
            if (!string.IsNullOrEmpty(info.orderby))
            {
                string[] orderbyList = info.orderby.Split(new char[] { ',' });
                string actualOrderString = string.Empty;
                foreach (string orderBy in orderbyList)
                {
                    DBAESEncryptStringFieldAttribute.decode(table, orderBy.Trim());
                    string orderString = orderBy + (info.order ? "" : " DESC");
                    if (string.IsNullOrEmpty(actualOrderString))
                        actualOrderString = orderString;
                    else
                        actualOrderString += "," + orderString;
                }
                DataView resultView = new DataView(table);
                resultView.Sort = actualOrderString;
                table = resultView.ToTable();
            }

            if (info.recordPerPage > 0)
            {
                info.numRecord = table.Rows.Count;
                info.numPage = info.numRecord / info.recordPerPage;
                if (info.numRecord % info.recordPerPage > 0)
                    info.numPage++;
                if (info.page == info.numPage && info.numPage > 0)
                    info.page--;
                int startIndex = info.recordPerPage * (info.page);
                int endIndex = info.recordPerPage * (info.page + 1) - 1;

                for (int i = table.Rows.Count - 1; i >= 0; i--)
                {
                    if (i < startIndex || i > endIndex)
                        table.Rows.Remove(table.Rows[i]);
                }
            }
            else
                info.numRecord = table.Rows.Count;

        }
        return table;
    }

    public static ArrayList SelectedRepeaterItemToBaseObjectList(DBManager db, Repeater RepeaterControl, string CheckBoxName)
    {
        ArrayList list = new ArrayList();
        foreach (RepeaterItem i in RepeaterControl.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl(CheckBoxName);
            if (cb.Checked)
            {
                DBObject o = (DBObject)db.createObject();
                WebFormUtils.GetKeys(db, o, cb);
                list.Add(o);
            }

        }
        return list;
    }

    public static void SetEnabledControlSection(Control ctrl, bool isEnabled)
    {
        foreach (Control obj in ctrl.Controls)
        {
            if (obj is WebControl)
                ((WebControl)obj).Enabled = isEnabled;
            if (obj.HasControls())
                SetEnabledControlSection(obj, isEnabled);
        }
    }

    public static void TransmitFile(HttpResponse Response, string FilenameWithFullPath, string clientSideFileName, bool DeleteAfterTransmit)
    {
        FileInfo transmiteFileInfo = new System.IO.FileInfo(FilenameWithFullPath);
        if (transmiteFileInfo.Exists)
        {
            if (Response.IsClientConnected)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + clientSideFileName);
                Response.ContentType = "application/download";
                Response.AppendHeader("Content-Length", transmiteFileInfo.Length.ToString());
                Response.Expires = -1;
                if (DeleteAfterTransmit)
                {
                    Response.WriteFile(FilenameWithFullPath, true);
                    Response.Flush();
                    System.IO.File.Delete(FilenameWithFullPath);
                }
                else
                {
                    Response.TransmitFile(FilenameWithFullPath);
                    Response.Flush();
                }
                Response.End();
            }
            else
                transmiteFileInfo.Delete();
        }
        else
            throw new System.IO.FileNotFoundException("Internal File Not Found: " + FilenameWithFullPath, FilenameWithFullPath);
    }
    public static DateTime toDateTimeObject(object dateTimeExpression)
    {
        if (dateTimeExpression is DateTime)
            return (DateTime)dateTimeExpression;
        else
        {
            string dateTimeString = dateTimeExpression.ToString();
            DateTime tryParseDateTime = new DateTime();

            if (DateTime.TryParse(dateTimeString, out tryParseDateTime))
                return tryParseDateTime;
            long tryEffDateString;
            if (dateTimeString.Trim().Length == 8 && long.TryParse(dateTimeString, out tryEffDateString))
            {
                DateTime tmpDateTime = new DateTime(int.Parse(dateTimeString.Substring(0, 4)), int.Parse(dateTimeString.Substring(4, 2)), int.Parse(dateTimeString.Substring(6, 2)));
                if (tmpDateTime.Year < 1800)
                    throw new Exception("Year too small");
                return tmpDateTime;
            }
            else
                return new DateTime();
        }
    }

}
