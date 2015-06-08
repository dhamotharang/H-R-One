using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using HROne.DataAccess;
//using HROne.Lib.Entities;

namespace HROne.Common
{
    /// <summary>
    /// Summary description for GenericUtility
    /// </summary>
    /// 
    public static class WebUtility
    {

        public static void WebControlsLocalization(Page page, ControlCollection ctrls)
        {
            WebControlsLocalization(page.Session, ctrls);
            //System.Resources.ResourceManager rm = AppUtils.getResourceManager();
            //System.Globalization.CultureInfo ci = HROne.Common.WebUtility.GetSessionCultureInfo(page.Session);
            //System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

            //foreach (Control ctrl in ctrls)
            //{
            //    if (!ctrl.GetType().Equals(typeof(perspectivemind.common.ui.ILabel)) && !ctrl.GetType().Equals(typeof(DropDownList)))
            //    {
            //        System.Reflection.PropertyInfo propInfo = ctrl.GetType().GetProperty("Text");
            //        if (propInfo != null)
            //        {
            //            string originalString = propInfo.GetValue(ctrl, null).ToString();
            //            string newString = rm.GetString(originalString, ci);
            //            if (!string.IsNullOrEmpty(newString))
            //                propInfo.SetValue(ctrl, newString, null);
            //            else
            //            {
            //                newString = rm.GetString(originalString.Trim(), ci);
            //                if (!string.IsNullOrEmpty(newString))
            //                    propInfo.SetValue(ctrl, newString, null);
            //            }
            //        }
            //        if (ctrl.Controls != null)
            //            WebControlsLocalization(page, ctrl.Controls);
            //    }
            //}

        }

        public static void initLanguage(HttpSessionState session)
        {
            string lang = (string)session["lang"];
            if (!string.IsNullOrEmpty(lang))
                if (lang.Equals("big5", StringComparison.CurrentCultureIgnoreCase))
                    lang = "zh-cht";
            try
            {
                System.Globalization.CultureInfo ci = new CultureInfo(lang);
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            }
            catch
            {
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");
        }
        public static CultureInfo GetSessionUICultureInfo(HttpSessionState session)
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture;
        }
        public static void WebControlsLocalization(System.Web.SessionState.HttpSessionState session, ControlCollection ctrls)
        {
            //System.Globalization.CultureInfo ci = GetSessionUICultureInfo(session); //System.Threading.Thread.CurrentThread.CurrentUICulture;//HROne.Common.WebUtility.GetSessionCultureInfo(session);
            //WebControlsLocalization(ctrls, ci);
            ////initLanguage(session);
            LocalizationProcess localizationProcess = new LocalizationProcess(GetDatabaseConnection(session),GetSessionUICultureInfo(session));
            localizationProcess.WebControlsLocalization(ctrls);
        }

        public class LocalizationProcess
        {
            DatabaseConnection dbConn;
            System.Globalization.CultureInfo ci;
            System.Resources.ResourceManager rm;
            System.Collections.ArrayList TextTransformationList;

            public LocalizationProcess(DatabaseConnection dbConn, System.Globalization.CultureInfo ci)
            {
                this.dbConn = dbConn;
                this.ci = ci;
                rm = AppUtils.getResourceManager();
                if (dbConn != null)
                {
                    DBFilter dbFilter = new DBFilter();
                    dbFilter.add("LEN(TextTransformationOriginalString)", false);
                    dbFilter.add("LEN(TextTransformationReplacedTo)", false);
                    TextTransformationList = HROne.Lib.Entities.ETextTransformation.db.select(dbConn, dbFilter);
                }
            }

            public void WebControlsLocalization(ControlCollection ctrls)
            {
                foreach (Control ctrl in ctrls)
                {
                    if (ctrl is WebControl)
                    {
                        WebControlLocalization((WebControl)ctrl);
                    }
                    if (ctrl.Controls != null)
                        if (ctrl.Controls.Count > 0)
                            WebControlsLocalization(ctrl.Controls);

                }
            }

            public void WebControlLocalization(WebControl ctrl)
            {
                if (ctrl is ListControl)
                {
                    foreach (ListItem item in ((ListControl)ctrl).Items)
                    {
                        string originalString = item.Attributes["OriginalText"];
                        if (string.IsNullOrEmpty(originalString))
                        {
                            originalString = item.Text;
                        }
                        //else
                        //{
                        //    //  Dummy, for adding breakpoint only
                        //    originalString = originalString;
                        //}
                        if (!string.IsNullOrEmpty(originalString.Trim()))
                        {
                            string newString = GetLocalizedString(originalString, string.Empty);
                            if (!string.IsNullOrEmpty(newString))
                                item.Text = newString;
                            else
                            {
                                // temporily unuse or will change debug mode by config file since multiple database issue
                                //if (HROne.Lib.Entities.ESystemParameter.getParameter("DebugMode").Equals("Y"))
                                //    CreateUnTranslateRecord(originalString, ci.Name);
                                //continue;
                            }

                            item.Attributes.Add("OriginalText", originalString);
                        }
                    }
                }
                //else if (ctrl is Repeater)
                //{
                //    Repeater repeaterControl = (Repeater)ctrl;
                //    foreach (RepeaterItem item in repeaterControl.Items)
                //        WebControlsLocalization(session, item.Controls);

                //}
                else if (!(ctrl is TextBox))
                {
                    System.Reflection.PropertyInfo propInfo = ctrl.GetType().GetProperty("Text");
                    if (propInfo == null)
                        propInfo = ctrl.GetType().GetProperty("ToolTip");

                    if (propInfo != null)
                    {
                        string originalString = ((WebControl)ctrl).Attributes["OriginalText"];
                        if (string.IsNullOrEmpty(originalString))
                        {
                            originalString = propInfo.GetValue(ctrl, null).ToString();
                        }
                        //else
                        //{
                        //    //  Dummy, for adding breakpoint only
                        //    originalString = originalString;
                        //}
                        if (!string.IsNullOrEmpty(originalString.Trim()))
                        {
                            string newString = GetLocalizedString(originalString, string.Empty);
                            if (!string.IsNullOrEmpty(newString))
                                propInfo.SetValue(ctrl, newString, null);
                            else
                            {
                                // temporily unuse or will change debug mode by config file since multiple database issue
                                //if (HROne.Lib.Entities.ESystemParameter.getParameter("DebugMode").Equals("Y"))
                                //    CreateUnTranslateRecord(originalString, ci.Name);
                                //return;
                            }

                            ((WebControl)ctrl).Attributes.Add("OriginalText", originalString);

                        }
                    }
                }

            }

            public void MenuLocalization(Menu menu)
            {
                foreach (MenuItem menuItem in menu.Items)
                    MenuItemLocalization(menuItem);
            }

            private void MenuItemLocalization(MenuItem menuItem)
            {
                foreach (MenuItem subMenuItem in menuItem.ChildItems)
                    MenuItemLocalization(subMenuItem);
                menuItem.Text = GetLocalizedString(menuItem.Text, menuItem.Text);
                menuItem.ToolTip = GetLocalizedString(menuItem.ToolTip, menuItem.ToolTip);
            }

            public void ReportSectionsLocalization(CrystalDecisions.CrystalReports.Engine.ReportDocument rptDocument, bool IsAllowGrow)
            {
                //System.Drawing.FontFamily chineseFontFamily = AppUtils.GetChineseFontFamily(dbConn);

                if (!rptDocument.IsSubreport)
                    foreach (CrystalDecisions.CrystalReports.Engine.ReportDocument subReport in rptDocument.Subreports)
                        try
                        {
                            ReportSectionsLocalization(subReport, IsAllowGrow);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }

                foreach (CrystalDecisions.CrystalReports.Engine.Section rptSection in rptDocument.ReportDefinition.Sections)
                {
                    foreach (CrystalDecisions.CrystalReports.Engine.ReportObject rptObj in rptSection.ReportObjects)
                    {
                        if (rptObj is CrystalDecisions.CrystalReports.Engine.SubreportObject)
                        {
                            //  do nothing on subreport object because there is a bug on visual studio 2008 such that the link parameter will be broken if ANY change of subreoprt object is made.

                        }
                        else
                        {
                            try
                            {
                                rptObj.ObjectFormat.EnableCanGrow = true;// IsAllowGrow;
                                //if (!IsAllowGrow)
                                //{
                                //    rptObj.Left *= 4;
                                //    rptObj.Width *= 4;
                                //}
                            }
                            catch
                            { }
                        }
                        System.Reflection.PropertyInfo propInfo = rptObj.GetType().GetProperty("Text");
                        if (rptObj is CrystalDecisions.CrystalReports.Engine.TextObject)
                        {
                            //                    System.Reflection.PropertyInfo propInfo = rptObj.GetType().GetProperty("Text");
                            if (propInfo != null)
                            {
                                string originalString = propInfo.GetValue(rptObj, null).ToString();
                                string newString = GetLocalizedString(originalString, string.Empty);
                                if (!string.IsNullOrEmpty(newString))
                                    propInfo.SetValue(rptObj, newString, null);
                                else
                                {
                                    if (originalString.EndsWith(":"))
                                    {
                                        originalString = originalString.Substring(0, originalString.Length - 1).Trim();
                                        newString = rm.GetString(originalString.Trim(), ci);
                                        if (!string.IsNullOrEmpty(newString))
                                            propInfo.SetValue(rptObj, newString + ":", null);
                                    }
                                }

                            }
                        }

                        //System.Reflection.PropertyInfo fontPropInfo = rptObj.GetType().GetProperty("Font");
                        //if (propInfo != null && fontPropInfo != null)
                        //{
                        //    string data = propInfo.GetValue(rptObj, null).ToString();
                        //    if (!IsEnglish(data) && chineseFontFamily != null)
                        //    {
                        //        System.Drawing.Font originalFont = (System.Drawing.Font)fontPropInfo.GetValue(rptObj, null);
                        //        System.Drawing.Font chineseFont = new System.Drawing.Font(chineseFontFamily, originalFont.Size, originalFont.Style);
                        //        rptObj.GetType().GetMethod("ApplyFont").Invoke(rptObj, new object[] { chineseFont });
                        //    }
                        //}
                    }
                }

            }

            public string GetLocalizedString(string originalString)
            {
                return GetLocalizedString(originalString, originalString);
            }

            public string GetLocalizedString(string originalString, string StringIfNotExists)
            {
                string newString = rm.GetString(originalString, ci);
                if (string.IsNullOrEmpty(newString))
                {
                    newString = rm.GetString(originalString.Trim(), ci);
                    if (string.IsNullOrEmpty(newString))
                        newString = StringIfNotExists;
                }
                if (TextTransformationList != null)
                    foreach (HROne.Lib.Entities.ETextTransformation textTransform in TextTransformationList)
                        newString = textTransform.Replace(newString);
                return newString;
            }

            public string GetLocalizedStringByCode(string code, string defaultString)
            {
                string result = GetLocalizedString(code, string.Empty);
                if (!string.IsNullOrEmpty(result))
                    return result;
                return GetLocalizedString(defaultString);

            }
        }

        //private static void WebControlsLocalization(ControlCollection ctrls, System.Globalization.CultureInfo ci)
        //{
        //    foreach (Control ctrl in ctrls)
        //    {
        //        if (ctrl is WebControl)
        //        {
        //            WebControlLocalization((WebControl)ctrl, ci);
        //        }
        //        if (ctrl.Controls != null)
        //            if (ctrl.Controls.Count > 0)
        //                WebControlsLocalization(ctrl.Controls, ci);

        //    }
        //}
        //private static void WebControlLocalization(WebControl ctrl, System.Globalization.CultureInfo ci)
        //{
        //    System.Resources.ResourceManager rm = AppUtils.getResourceManager();
        //    if (ctrl is ListControl)
        //    {
        //        foreach (ListItem item in ((ListControl)ctrl).Items)
        //        {
        //            string originalString = item.Attributes["OriginalText"];
        //            if (string.IsNullOrEmpty(originalString))
        //            {
        //                originalString = item.Text;
        //            }
        //            //else
        //            //{
        //            //    //  Dummy, for adding breakpoint only
        //            //    originalString = originalString;
        //            //}
        //            if (!string.IsNullOrEmpty(originalString.Trim()))
        //            {
        //                string newString = rm.GetString(originalString, ci);
        //                if (!string.IsNullOrEmpty(newString))
        //                    item.Text = newString;
        //                else
        //                {
        //                    newString = rm.GetString(originalString.Trim(), ci);
        //                    if (!string.IsNullOrEmpty(newString))
        //                        item.Text = newString;
        //                    else
        //                    {
        //                        // temporily unuse or will change debug mode by config file since multiple database issue
        //                        //if (HROne.Lib.Entities.ESystemParameter.getParameter("DebugMode").Equals("Y"))
        //                        //    CreateUnTranslateRecord(originalString, ci.Name);
        //                        //continue;
        //                    }
        //                }
        //                item.Attributes.Add("OriginalText", originalString);
        //            }
        //        }
        //    }
        //    //else if (ctrl is Repeater)
        //    //{
        //    //    Repeater repeaterControl = (Repeater)ctrl;
        //    //    foreach (RepeaterItem item in repeaterControl.Items)
        //    //        WebControlsLocalization(session, item.Controls);

        //    //}
        //    else if (!(ctrl is TextBox))
        //    {
        //        System.Reflection.PropertyInfo propInfo = ctrl.GetType().GetProperty("Text");
        //        if (propInfo == null)
        //            propInfo = ctrl.GetType().GetProperty("ToolTip");

        //        if (propInfo != null)
        //        {
        //            string originalString = ((WebControl)ctrl).Attributes["OriginalText"];
        //            if (string.IsNullOrEmpty(originalString))
        //            {
        //                originalString = propInfo.GetValue(ctrl, null).ToString();
        //            }
        //            //else
        //            //{
        //            //    //  Dummy, for adding breakpoint only
        //            //    originalString = originalString;
        //            //}
        //            if (!string.IsNullOrEmpty(originalString.Trim()))
        //            {
        //                string newString = rm.GetString(originalString, ci);
        //                if (!string.IsNullOrEmpty(newString))
        //                    propInfo.SetValue(ctrl, newString, null);
        //                else
        //                {
        //                    newString = rm.GetString(originalString.Trim(), ci);
        //                    if (!string.IsNullOrEmpty(newString))
        //                        propInfo.SetValue(ctrl, newString, null);
        //                    else
        //                    {
        //                        // temporily unuse or will change debug mode by config file since multiple database issue
        //                        //if (HROne.Lib.Entities.ESystemParameter.getParameter("DebugMode").Equals("Y"))
        //                        //    CreateUnTranslateRecord(originalString, ci.Name);
        //                        //return;
        //                    }
        //                }
        //                ((WebControl)ctrl).Attributes.Add("OriginalText", originalString);

        //            }
        //        }
        //    }

        //}
        public static void MenuLocalization(Menu menu)
        {
            //CultureInfo ci = GetSessionUICultureInfo(menu.Page.Session);
            //foreach (MenuItem menuItem in menu.Items)
            //    MenuItemLocalization(menuItem, ci);
            LocalizationProcess localizationProcess = new LocalizationProcess(GetDatabaseConnection(menu.Page.Session), GetSessionUICultureInfo(menu.Page.Session));
            localizationProcess.MenuLocalization(menu);
        }

        //private static void MenuItemLocalization(MenuItem menuItem, CultureInfo ci)
        //{
        //    foreach (MenuItem subMenuItem in menuItem.ChildItems)
        //        MenuItemLocalization(subMenuItem, ci);
        //    menuItem.Text = GetLocalizedStringByCode(menuItem.Text, menuItem.Text, ci);
        //    menuItem.ToolTip = GetLocalizedStringByCode(menuItem.ToolTip, menuItem.ToolTip, ci);
        //}

        public static string GetLocalizedStringByCode(string code, string defaultString)
        {
            return GetLocalizedStringByCode(code, defaultString, CultureInfo.CurrentUICulture);
        }
        public static string GetLocalizedStringByCode(string code, string defaultString, CultureInfo ci)
        {
            string result = HROne.Common.WebUtility.GetLocalizedString(code, ci);
            if (code != result)
                return result;
            return HROne.Common.WebUtility.GetLocalizedString(defaultString, ci);

        }
        public static string GetLocalizedString(string originalString)
        {
            return GetLocalizedString(originalString, CultureInfo.CurrentUICulture);
        }
        public static string GetLocalizedString(string originalString, CultureInfo ci)
        {
            System.Resources.ResourceManager rm = AppUtils.getResourceManager();

            string newString = rm.GetString(originalString, ci);
            if (string.IsNullOrEmpty(newString))
            {
                newString = rm.GetString(originalString.Trim(), ci);
                if (string.IsNullOrEmpty(newString))
                    return originalString;
            }
            return newString;

        }

        public static DatabaseConnection GetDatabaseConnection(HttpSessionState Session)
        {
            if (Session != null)
            {
                return (DatabaseConnection)Session["DatabaseConnection"];
            }
            return null;
        }
        private static void CreateUnTranslateRecord(string UntranlsateString, string lang)
        {
            System.Data.OleDb.OleDbConnection oleDBConn = new System.Data.OleDb.OleDbConnection();
            string DebugFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Debug.mdb");
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DebugFilePath + ";";
            oleDBConn.ConnectionString = strConn;
            try
            {
                oleDBConn.Open();
            }
            catch
            {
                oleDBConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DebugFilePath + ";";
                oleDBConn.Open();
            }

            System.Data.OleDb.OleDbDataAdapter oleDbAdapter = new System.Data.OleDb.OleDbDataAdapter("Select * from [NotTranslated]"
                + " WHERE " + "TranslationString='" + UntranlsateString.Replace("'", "''") + "' and LanguageString='" + lang + "'"
                , oleDBConn);
            System.Data.OleDb.OleDbCommandBuilder commandBuilder = new System.Data.OleDb.OleDbCommandBuilder(oleDbAdapter);
            System.Data.DataTable table = new DataTable();
            oleDbAdapter.Fill(table);
            DateTime currentDatetime = AppUtils.ServerDateTime();
            //DataRow[] rowList = table.Select();
            if (table.Rows.Count <= 0)
            {
                DataRow row = table.NewRow();
                row["TranslationString"] = UntranlsateString;
                row["LanguageString"] = lang;
                row["CreateDate"] = currentDatetime;
                row["LastUpdateDate"] = currentDatetime;
                table.Rows.Add(row);
            }
            else
            {
                table.Rows[0]["LastUpdateDate"] = currentDatetime;
            }
            oleDbAdapter.Update(table);
            oleDBConn.Close();
        }

        public static string URLwithEncryptQueryString(HttpSessionState session, string URL)
        {
            if (session["EncryptQueryString"] != null)
                if (session["EncryptQueryString"].Equals(true))
                {
                    int QueryStartQuestionMarkPos = URL.IndexOf('?');
                    if (QueryStartQuestionMarkPos >= 0)
                    {
                        string originalQueryString = URL.Substring(QueryStartQuestionMarkPos + 1);

                        if (!string.IsNullOrEmpty(originalQueryString))
                        {
                            HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);
                            string newQueryString = crypto.Encrypting(originalQueryString, "0123456789abcdef");
                            URL = URL.Replace(originalQueryString, System.Web.HttpUtility.UrlEncode(newQueryString));

                        }
                    }
                }
            return URL;
        }
        public static void RedirectURLwithEncryptedQueryString(HttpResponse Response,HttpSessionState Session, string URL)
        {
            Response.Redirect(URLwithEncryptQueryString(Session, URL));
        }
        public static string DecryptQueryString(HttpSessionState session, string QueryString)
        {
            if (QueryString.StartsWith("?"))
                QueryString = QueryString.Substring(1);
            if (session["EncryptQueryString"] != null)
                if (session["EncryptQueryString"].Equals(true))
                {

                    if (!string.IsNullOrEmpty(QueryString))
                    {
                        string tmpQueryString = HttpUtility.UrlDecode(QueryString);
                        if (tmpQueryString.Length % 4 != 0)
                            return QueryString;

                        System.Text.RegularExpressions.Regex regExBase64 = new System.Text.RegularExpressions.Regex(@"[0-9a-zA-Z\+/=]{20,}");
                        if (!regExBase64.IsMatch(tmpQueryString))
                            return QueryString;

                        //if (((string)value).Contains(" "))
                        //    return value;
                        //if (((string)value).Contains("(") || ((string)value).Contains(")"))
                        //    return value;
                        byte[] bytIn;
                        try
                        {
                            bytIn = Convert.FromBase64String(tmpQueryString);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                            return QueryString;
                        }
                        if (bytIn.Length % 16 != 0)
                        {
                            return QueryString;
                        }

                        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);
                        try
                        {
                            string newQueryString = crypto.Decrypting(tmpQueryString, "0123456789abcdef");
                            return newQueryString;
                        }
                        catch
                        {
                            return QueryString;
                        }

                    }
                }
            return QueryString;
        }
        public static System.Collections.Specialized.NameValueCollection getDecryptQueryStringCollection(HttpSessionState session, string QueryString)
        {
            
            string actualQueryString = DecryptQueryString(session, QueryString);
            return HttpUtility.ParseQueryString(actualQueryString);
            //string[] queryStringList = actualQueryString.Split(new char[] { '&' });
            //System.Collections.Specialized.NameValueCollection QueryValue = new System.Collections.Specialized.NameValueCollection();
            //foreach (string queryString in queryStringList)
            //{
            //    int equalsignPos = queryString.IndexOf('=');
            //    if (equalsignPos >= 0)
            //    {
            //        string name = queryString.Substring(0, equalsignPos);
            //        if (queryString.Length > (equalsignPos))
            //        {
            //            string value = queryString.Substring(equalsignPos + 1);
            //            QueryValue.Add(name, value);
            //        }
            //        else
            //            QueryValue.Add(name, string.Empty);
            //    }
            //}
            //return QueryValue;
        }

        public static string getDefaultSessionCookieName()
        {
            System.Web.Configuration.SessionStateSection sessionStateSection = (System.Web.Configuration.SessionStateSection)System.Configuration.ConfigurationManager.GetSection("system.web/sessionState");
            return sessionStateSection.CookieName;
        }

        public static void AddBrowserCompatibilityMeta(Page page)
        {
            HttpBrowserCapabilities browser = page.Request.Browser;
            if (browser.Browser.Equals("IE"))
            {
                //  Add meta for IE9 to run as IE8
                if (browser.Version.StartsWith("9."))
                {
                    HtmlMeta htmlMeta = new HtmlMeta();
                    htmlMeta.HttpEquiv = "X-UA-Compatible";
                    htmlMeta.Content = "IE=EmulateIE8";
                    page.Header.Controls.AddAt(0, htmlMeta);
                }
                //  Add meta for IE10 to run as IE9
                else if (browser.Version.StartsWith("10."))
                {
                    HtmlMeta htmlMeta = new HtmlMeta();
                    htmlMeta.HttpEquiv = "X-UA-Compatible";
                    htmlMeta.Content = "IE=EmulateIE9";
                    page.Header.Controls.AddAt(0, htmlMeta);
                }
            }
        }
        //public static EUser GetCurUser(HttpSessionState Session)
        //{
        //    return (EUser)Session["User"];
        //}
    }

}