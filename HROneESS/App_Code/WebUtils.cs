using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Xml;
using HROne.DataAccess;
using HROne.Lib.Entities;
using HROne.CommonLib;
using HROne;

public class HROneWebMasterPage : System.Web.UI.MasterPage
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

public class HROneWebPage : System.Web.UI.Page
{
    protected System.Globalization.CultureInfo m_ci = null;
    protected DatabaseConnection m_dbConn = null;

    protected System.Collections.Specialized.NameValueCollection m_decryptedRequest = null;

    public System.Collections.Specialized.NameValueCollection DecryptedRequest
    {
        get
        {
            if (this.Master is HROneWebMasterPage)
                return ((HROneWebMasterPage)Page.Master).DecryptedRequest;
            if (m_decryptedRequest == null)
                m_decryptedRequest = HROne.Common.WebUtility.getDecryptQueryStringCollection(Session, Request.Url.Query);
            return m_decryptedRequest;
        }

    }
    
    public DatabaseConnection dbConn
    {
        get
        {
            if (this.Master is HROneWebMasterPage)
                return ((HROneWebMasterPage)Page.Master).dbConn;
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
    public System.Globalization.CultureInfo ci
    {
        get
        {
            if (this.Master is HROneWebMasterPage)
                return ((HROneWebMasterPage)Page.Master).ci;
            if (m_ci == null)
                m_ci = HROne.Common.WebUtility.GetSessionUICultureInfo(Session);
            return m_ci;
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
            if (this.Page is HROneWebPage)
                return ((HROneWebMasterPage)Page.Master).DecryptedRequest;
            if (m_decryptedRequest == null)
                m_decryptedRequest = HROne.Common.WebUtility.getDecryptQueryStringCollection(Session, Request.Url.Query);
            return m_decryptedRequest;
        }

    }

    public DatabaseConnection dbConn
    {
        get
        {
            if (this.Page is HROneWebPage)
                return ((HROneWebMasterPage)Page.Master).dbConn;
            if (m_dbConn == null)
                m_dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            return m_dbConn;
        }
    }
    public System.Globalization.CultureInfo ci
    {
        get
        {
            if (this.Page is HROneWebPage)
                return ((HROneWebMasterPage)Page.Master).ci;
            if (m_ci == null)
                m_ci = HROne.Common.WebUtility.GetSessionUICultureInfo(Session);
            return m_ci;
        }
    }
}

public class WebUtils
{
    private const string keyString = "HROne";
    private const string SESSION_PRODUCTLICENSE = "ProductLicense";

    public static ProductLicense productLicense(HttpSessionState Session)
    {
        return (ProductLicense)Session[SESSION_PRODUCTLICENSE];
    }
    //public static DatabaseType LoadDBType()
    //{

    //    HROneConfig configure = new HROneConfig();
    //    DatabaseType DBType = configure.GetDatabaseType();
    //    if (DBType == null)
    //    {

    //        DatabaseType oldDBType = perspectivemind.common.DBUtil.type;

    //        if (oldDBType != null)
    //        {
    //            if (oldDBType is SQLType)
    //            {
    //                configure.DBType = HROneConfig.DBTypeEmun.MSSQL;
    //                configure.ConnectionString = ((SQLType)oldDBType).url;


    //            }
    //        }
    //        configure.Save();
    //        configure.load();
    //        DBType = configure.GetDatabaseType();
    //    }


    //    return DBType;
    //}

    public static DatabaseConnection GetDatabaseConnection()
    {
        HROneConfig configure = HROneConfig.GetCurrentHROneConfig();
        DatabaseConnection DBType = configure.GetDatabaseConnection();

        return DBType;
    }
    public static DatabaseConnection GetDatabaseConnection(string DatabaseName)
    {
        HROneConfig configure = HROneConfig.GetCurrentHROneConfig();
        DatabaseConnection DBType = configure.GetDatabaseConnection(DatabaseName);

        return DBType;
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
    //public class DatabaseConfig
    //{
    //    public string name = string.Empty;
    //    public DBTypeEmun DBType = DBTypeEmun.None;
    //    public string ConnectionString = string.Empty;

    //    public bool TestConnection()
    //    {
    //        if (DBType.Equals(DBTypeEmun.MSSQL))
    //        {
    //            System.Data.SqlClient.SqlConnection testConn = new System.Data.SqlClient.SqlConnection(ConnectionString);
    //            try
    //            {
    //                testConn.Open();
    //            }
    //            catch (Exception ex)
    //            {
    //                System.Diagnostics.Debug.WriteLine(ex.Message);
    //                return false;
    //            }
    //            finally
    //            {
    //                testConn.Close();
    //            }
    //            return true;
    //        }
    //        else
    //            return false;
    //    }

    //    public bool TestServerConnectionWithoutDatabase()
    //    {
    //        if (DBType.Equals(DBTypeEmun.MSSQL))
    //        {
    //            System.Data.SqlClient.SqlConnection testConn = new System.Data.SqlClient.SqlConnection();
    //            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);
    //            connStringBuilder.InitialCatalog = "";
    //            testConn.ConnectionString = connStringBuilder.ConnectionString;
    //            try
    //            {
    //                testConn.Open();
    //                testConn.Close();
    //            }
    //            catch
    //            {
    //                return false;
    //            }
    //            return true;
    //        }
    //        else
    //            return false;
    //    }

    //    public DatabaseConnection CreateDatabaseConnectionObject()
    //    {
    //        if (DBType == DBTypeEmun.MSSQL)
    //            return new DatabaseConnection(ConnectionString, DatabaseConnection.DatabaseType.MSSQL);
    //        else
    //            return null;
    //    }
    //}


    //public class HROneConfig
    //{
    //    //[ThreadStatic]
    //    //private static HROneConfig currentHROneConfig = null;

    //    private const string ConfigFilename = "HROne.config";
    //    //public DBTypeEmun DBType = DBTypeEmun.None;
    //    //public string ConnectionString = string.Empty;
    //    public bool ShutDownDomainAfterUsed = false;
    //    public bool AllowMultiDB = false;

    //    public List<DatabaseConfig> DatabaseConfigList = new List<DatabaseConfig>();

    //    protected HROneConfig()
    //    {
    //        load();
    //    }

    //    public static HROneConfig GetCurrentHROneConfig()
    //    {
    //        //if (currentHROneConfig == null)
    //        //    currentHROneConfig = new HROneConfig();
    //        //return currentHROneConfig;
    //        return new HROneConfig();
    //    }

    //    public void load()
    //    {
    //        string filename = getFilename();


    //        System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
    //        try
    //        {
    //            config.Load(filename);
    //            if (config["Settings"] != null)
    //            {
    //                if (config["Settings"].Attributes["Version"] == null)
    //                {
    //                    DatabaseConfig dbconfig = LoadDatabaseConfig(config["Settings"]);
    //                    if (dbconfig != null)
    //                    {
    //                        DatabaseConfigList.Add(dbconfig);
    //                        //DBType = dbconfig.DBType;
    //                        //ConnectionString = dbconfig.ConnectionString;
    //                    }
    //                    if (config["Settings"]["ShutdownAfterUsed"] != null)
    //                    {
    //                        string strShutdownAfterUsed = config["Settings"]["ShutdownAfterUsed"].InnerText;
    //                        if (strShutdownAfterUsed.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
    //                        || strShutdownAfterUsed.Equals("True", StringComparison.CurrentCultureIgnoreCase))
    //                        {
    //                            ShutDownDomainAfterUsed = true;
    //                        }
    //                        else
    //                            ShutDownDomainAfterUsed = false;
    //                    }
    //                }
    //                else if (config["Settings"].Attributes["Version"].Value == "2.0")
    //                {
    //                    XmlNodeList dbConfigXmlList = config["Settings"].GetElementsByTagName("DatabaseConfig");
    //                    foreach (XmlElement dbConfigXML in dbConfigXmlList)
    //                    {
    //                        DatabaseConfig dbconfig = LoadDatabaseConfig(dbConfigXML);
    //                        if (dbconfig != null)
    //                        {
    //                            DatabaseConfigList.Add(dbconfig);
    //                            //DBType = dbconfig.DBType;
    //                            //ConnectionString = dbconfig.ConnectionString;
    //                        }
    //                    }
    //                    if (config["Settings"]["ShutdownAfterUsed"] != null)
    //                    {
    //                        string strShutdownAfterUsed = config["Settings"]["ShutdownAfterUsed"].InnerText;
    //                        if (strShutdownAfterUsed.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
    //                        || strShutdownAfterUsed.Equals("True", StringComparison.CurrentCultureIgnoreCase))
    //                        {
    //                            ShutDownDomainAfterUsed = true;
    //                        }
    //                        else
    //                            ShutDownDomainAfterUsed = false;
    //                    }
    //                    // ESS do not support multiple database
    //                    //if (config["Settings"]["AllowMultiDB"] != null)
    //                    //{
    //                    //    string strAllowMultiDB = config["Settings"]["AllowMultiDB"].InnerText;
    //                    //    if (strAllowMultiDB.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
    //                    //    || strAllowMultiDB.Equals("True", StringComparison.CurrentCultureIgnoreCase))
    //                    //    {
    //                    //        AllowMultiDB = true;
    //                    //    }
    //                    //    else
    //                    //        AllowMultiDB = false;
    //                    //}

    //                }
    //            }



    //        }
    //        catch
    //        {
    //        }

    //    }

    //    private DatabaseConfig LoadDatabaseConfig(XmlElement node)
    //    {
    //        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

    //        string strDBType = string.Empty;
    //        string strConnectionString = string.Empty;
    //        string strName = string.Empty;
    //        if (node["dbtype"] != null)
    //        {
    //            strDBType = node["dbtype"].InnerText;

    //        }
    //        if (node["name"] != null)
    //        {
    //            strName = node["name"].InnerText;

    //        }
    //        if (node["ConnectionString"] != null)
    //            strConnectionString = node["ConnectionString"].InnerText;

    //        if (strDBType.Equals("MSSQL"))
    //        {
    //            DatabaseConfig config = new DatabaseConfig();
    //            config.DBType = DBTypeEmun.MSSQL;
    //            config.name = strName;
    //            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(strConnectionString);
    //            try
    //            {
    //                connStringBuilder.Password = crypto.Decrypting(connStringBuilder.Password, keyString);
    //                connStringBuilder.ApplicationName = "HROneWeb";
    //                config.ConnectionString = connStringBuilder.ConnectionString;
    //            }
    //            catch
    //            {

    //            }
    //            return config;
    //        }
    //        else
    //            return null;
    //    }

    //    public void Save()
    //    {
    //        string errorMessage = null;
    //        if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(AppDomain.CurrentDomain.BaseDirectory, out errorMessage))
    //            throw new Exception(errorMessage);
    //        string filename = getFilename();


    //        System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
    //        XmlElement settings = config.CreateElement("Settings");
    //        XmlAttribute version = config.CreateAttribute("Version");
    //        version.Value = "2.0";
    //        settings.Attributes.Append(version);
    //        config.AppendChild(settings);
    //        SetDatabaseConfigList(settings);

    //        settings.AppendChild(config.CreateElement("ShutdownAfterUsed"));
    //        settings["ShutdownAfterUsed"].InnerText = ShutDownDomainAfterUsed ? "true" : "false";

    //        // ESS do not support multiple database
    //        //settings.AppendChild(config.CreateElement("AllowMultiDB"));
    //        //settings["AllowMultiDB"].InnerText = AllowMultiDB ? "true" : "false";

    //        config.Save(filename);
    //    }
    //    private void SetDatabaseConfigList(XmlElement settingNode)
    //    {
    //        for (int idx = 0; idx < DatabaseConfigList.Count; idx++)
    //        {
    //            if (!AllowMultiDB)
    //                idx = DatabaseConfigList.Count - 1;
    //            DatabaseConfig dbConfig = DatabaseConfigList[idx];
    //            XmlElement databaseConfigXML = settingNode.OwnerDocument.CreateElement("DatabaseConfig");
    //            databaseConfigXML.SetAttribute("index", idx.ToString());
    //            SetDatabaseConfig(databaseConfigXML, dbConfig);
    //            settingNode.AppendChild(databaseConfigXML);
    //        }
    //    }
    //    private void SetDatabaseConfig(XmlElement databaseConfigRootNode, DatabaseConfig dbConfig)
    //    {
    //        XmlDocument config = databaseConfigRootNode.OwnerDocument;
    //        databaseConfigRootNode.AppendChild(config.CreateElement("name"));
    //        databaseConfigRootNode["name"].InnerText = dbConfig.name;
    //        databaseConfigRootNode.AppendChild(config.CreateElement("dbtype"));
    //        if (dbConfig.DBType.Equals(DBTypeEmun.MSSQL))
    //        {
    //            HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

    //            databaseConfigRootNode["dbtype"].InnerText = "MSSQL";
    //            databaseConfigRootNode.AppendChild(config.CreateElement("ConnectionString"));

    //            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(dbConfig.ConnectionString);
    //            connStringBuilder.Password = crypto.Encrypting(connStringBuilder.Password, keyString);
    //            databaseConfigRootNode["ConnectionString"].InnerText = connStringBuilder.ConnectionString;

    //        }
    //    }
    //    private string getFilename()
    //    {
    //        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFilename);

    //    }



    //    //public perspectivemind.common.DatabaseType GetDatabaseType()
    //    //{
    //    //    if (TestConnection())
    //    //        if (DBType.Equals(DBTypeEmun.MSSQL))
    //    //        {
    //    //            SQLType sqlType = new SQLType();
    //    //            sqlType.url = ConnectionString;
    //    //            return sqlType;
    //    //        }
    //    //        else
    //    //            return null;
    //    //    else
    //    //        return null;
    //    //}
    //    //public DatabaseConnection GetDatabaseConnection(int index)
    //    //{
    //    //    DatabaseConfig dbConfig =DatabaseConfigList[index];
    //    //    if (dbConfig.TestConnection())
    //    //        if (dbConfig.DBType.Equals(DBTypeEmun.MSSQL))
    //    //        {
    //    //            return new DatabaseConnection(dbConfig.ConnectionString, DatabaseConnection.DatabaseType.MSSQL);
    //    //        }
    //    //    return null;
    //    //}
    //    public DatabaseConnection GetDatabaseConnection()
    //    {
    //        if (DatabaseConfigList.Count > 1)
    //            return null;
    //        foreach (DatabaseConfig dbConfig in DatabaseConfigList)
    //        {

    //            if (dbConfig.TestConnection())
    //                return dbConfig.CreateDatabaseConnectionObject();
    //        }
    //        return null;
    //    }
    //}



    public static void AddLanguageOptionstoDropDownList(DropDownList dropDownList)
    {
        dropDownList.Items.Add(new ListItem("English", "en"));
        // Start 000170, Ricky So, 2015-02-05
        //dropDownList.Items.Add(new ListItem("いゅ(羉砰)", "big5"));
        dropDownList.Items.Add(new ListItem("中文(繁體)", "big5"));
        dropDownList.Items.Add(new ListItem("中文(簡體)", "gb"));
        // End 000170, Ricky So, 2015-02-05
    }

    public static void SetSessionLanguage(HttpSessionState Session, EESSUser user)
    {
        string defaultLang = string.Empty;
        if (user != null)
        {
            // Start 000170, Ricky So, 2015-02-05
            //if (EUser.db.select(dbConn, user))
                defaultLang = user.EmpESSLanguage;
            // End 000170, Ricky So, 2015-02-05
        }
        if (string.IsNullOrEmpty(defaultLang))
        {
            try
            {
                defaultLang = ESystemParameter.getParameter(HROne.Common.WebUtility.GetDatabaseConnection(Session), ESystemParameter.PARAM_CODE_ESS_DEFAULT_LANGUAGE);
            }
            catch
            {
                //Response.Redirect("~/DatabaseConfiguration.aspx");
            }
        }

        if (string.IsNullOrEmpty(defaultLang))
        {
            defaultLang = "en";
        }
        if (defaultLang.Equals("big5", StringComparison.CurrentCultureIgnoreCase))
            defaultLang = "zh-cht";
        else if (defaultLang.Equals("gb", StringComparison.CurrentCultureIgnoreCase))
            defaultLang = "zh-chs";
        
        Session.Add("lang", defaultLang);
    }

    public static void SetSessionDatabaseConnection(HttpSessionState Session, DatabaseConnection dbConn)
    {
        Session["DatabaseConnection"] = dbConn;
        //DatabaseConnection.SetDefaultDatabaseConnection(dbConn);
        WebUtils.LoadProductKey(Session);
        WebUtils.SetSessionLanguage(Session, null);

        HROneConfig config = HROneConfig.GetCurrentHROneConfig();
        Session["EncryptQueryString"] = config.EncryptedURLQueryString;

        //WebUtils.SetSessionLanguage(Session, null);
    }

    public enum AccessLevel
    {
        None,
        Read,
        ReadWrite
    };

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

    public static void loadPageList(LinkButton PrevPage, HtmlImage PrevPageImg,
        LinkButton NextPage, HtmlImage NextPageImg,
        LinkButton FirstPage, HtmlImage FirstPageImg,
        LinkButton LastPage, HtmlImage LastPageImg)
    {
        string s = PrevPage.Page.Request.ApplicationPath;
        if (PrevPage.Enabled)
            PrevPageImg.Src = s + "/images/previous.gif";
        else
            PrevPageImg.Src = s + "/images/previous_off.gif";
        if (NextPage.Enabled)
            NextPageImg.Src = s + "/images/next.gif";
        else
            NextPageImg.Src = s + "/images/next_off.gif";
        if (FirstPage.Enabled)
            FirstPageImg.Src = s + "/images/start.gif";
        else
            FirstPageImg.Src = s + "/images/start_off.gif";
        if (LastPage.Enabled)
            LastPageImg.Src = s + "/images/end.gif";
        else
            LastPageImg.Src = s + "/images/end_off.gif";
    }

    protected static bool LoadProductKey(HttpSessionState Session)
    {
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
        HROne.ProductLicense tmpProductKey = new HROne.ProductLicense();
        //  will create dummy product license if not exists
        tmpProductKey.LoadProductLicense(dbConn);
        //tmpProductKey.SetFeatureByCode(ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PRODUCTFEATURECODE));
        Session[SESSION_PRODUCTLICENSE] = tmpProductKey;
        if (tmpProductKey.IsValidProductKey)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public static bool CheckAccess(HttpResponse Response, HttpSessionState Session)
    {
        //Response.Cache.SetExpires(new DateTime(0));
        //return true;
        DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);

        if (GetCurUser(Session) == null)
        {
            string LastURL = Session["LastURL"].ToString();
            Session.Abandon();
            if (!string.IsNullOrEmpty(LastURL))
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx?LastURL=" + Convert.ToBase64String(Encoding.ASCII.GetBytes(LastURL)));
            else
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Login.aspx");
            return false;
        }
        if (Session["ForceChangePassword"] != null)
            if (Session["ForceChangePassword"].Equals(true))
            {
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/ESS_EmpChangePassword.aspx");
            }

        return true;
    }
    public static string PasswordHash(string text)
    {
        SHA1Managed sha1 = new SHA1Managed();
        sha1.ComputeHash(Encoding.Unicode.GetBytes(text));
        byte[] bytes = sha1.Hash;
        return Convert.ToBase64String(bytes);
    }

    public static EESSUser GetCurUser(HttpSessionState Session)
    {

        return (EESSUser)Session["User"];

    }

    public static void TransmitFile(HttpResponse Response, string FilenameWithFullPath, string clientSideFileName, bool DeleteAfterTransmit)
    {
        FileInfo transmiteFileInfo = new System.IO.FileInfo(FilenameWithFullPath);
        if (transmiteFileInfo.Exists)
        {
            if (Response.IsClientConnected)
            {
                Response.ClearContent();
                Response.ClearHeaders();
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

    public static string GetLocalizedReportFile(string Reportfilename)
    {
        System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
        string extension = Reportfilename.Substring(Reportfilename.LastIndexOf('.'));
        string checkFileName = Reportfilename.Replace(extension, string.Empty) + "_" + ci.Name + extension;
        if (System.IO.File.Exists(checkFileName))
            return checkFileName;
        else if (System.IO.File.Exists(Reportfilename))
            return Reportfilename;
        else
            return string.Empty;
    }

    public static void ReportExport(HttpResponse Response, HROne.Common.GenericReportProcess reportProcess, string reportFileName, string ExportFormat, string OutputFilenamePrefix)
    {
        ReportExport(Response, reportProcess, reportFileName, ExportFormat, OutputFilenamePrefix, true);
    }
    public static void ReportExport(HttpResponse Response, HROne.Common.GenericReportProcess reportProcess, string reportTemplateFileName, string ExportFormat, string OutputFilenamePrefix, bool IsLocalize)
    {
        if (Response.IsClientConnected)
        {
            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            HROne.Common.GenericReportProcess.UsePDFCreator = config.UsePDFCreator;
            if (config.UsePDFCreator)
                HROne.Common.PDFCreaterPrinter.PDFCreaterPrinterName = config.PDFCreatorPrinterName;

            string exportFileName = reportProcess.ReportExportToFile(reportTemplateFileName, ExportFormat, IsLocalize);
            string exportFileNameExtension = exportFileName.Substring(exportFileName.LastIndexOf("."));
            WebUtils.TransmitFile(Response, exportFileName, OutputFilenamePrefix + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + exportFileNameExtension, true);
        }
    }
    public static void RegisterRedirectJavaScript(Control ctrl, string url, int delayMillSecond)
    {
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
        string javascriptString = "window.open('" + url + "','_self');";
        if (!string.IsNullOrEmpty(alertMessage))
        {
            alertMessage = alertMessage.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            javascriptString = "alert(\"" + alertMessage + "\"); " + javascriptString + "";
        }
        ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "redirect", javascriptString, true);
// ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "errorMessage", "popupDialog(\"testing\");", true);

    }

    public static bool ValidateUser(DatabaseConnection dbConn, string username, string encryptedPassword, bool throwException, out bool ChangePasswordRequired, out EESSUser user)
    {
        string message = string.Empty;
        ChangePasswordRequired = false;
        user = null;

        if (dbConn == null)
        {
            if (throwException)
            {
                message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name or Password");
                throw new Exception(message);
            }
            return false;
        }

        DBFilter filter = new DBFilter();
        OR orEmpNoTerms = new OR();
        orEmpNoTerms.add(new Match("EmpNo", username));
        DBFieldTranscoder empNoTranscoder = EESSUser.db.getField("EmpNo").transcoder;
        if (empNoTranscoder != null)
            orEmpNoTerms.add(new Match("EmpNo", empNoTranscoder.toDB(username.ToUpper())));
        filter.add(orEmpNoTerms);
        ArrayList list = EESSUser.db.select(dbConn, filter);
        if (list.Count == 0)
        {
            if (throwException)
            {
                message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name / Password");
                throw new Exception(message);
            }
            return false;
        }
        {
            user = (EESSUser)list[0];

            ChangePasswordRequired = false;
            if (string.IsNullOrEmpty(user.EmpPW))
            {
                ChangePasswordRequired = true;
                if (!(user.EmpHKID == "()") && !string.IsNullOrEmpty(user.EmpHKID))
                    user.EmpPW = WebUtils.PasswordHash(user.EmpHKID.Substring(0, (user.EmpHKID.Length - 3)));
                else
                    user.EmpPW = WebUtils.PasswordHash(user.EmpPassportNo);
            }
            //if (user.EmpPW == WebUtils.PasswordHash(user.EmpPassportNo))
            //{
            //    if (!user.EmpPW.Equals(Password.Text))
            //    {
            //        Prompt.Text = "Invalid User Name / Password";
            //        return;
            //    }
            //    else
            //    {
            //        user.EmpPW = WebUtils.PasswordHash(Password.Text);
            //        EESSUser.db.update(user);
            //    }
            //}
            //else
            if (!user.EmpPW.Equals(encryptedPassword))
            {
                if (throwException)
                {
                    message = HROne.Common.WebUtility.GetLocalizedString("Invalid User Name / Password");
                    throw new Exception(message);
                }
                return false;
            }

            EEmpTermination empTerm = EEmpTermination.GetObjectByEmpID(dbConn, user.EmpID);
            if (empTerm != null)
            {
                if (throwException)
                {
                    if (empTerm.EmpTermLastDate < AppUtils.ServerDateTime().Date)
                    {
                        message = HROne.Common.WebUtility.GetLocalizedString("User is terminated");
                        throw new Exception(message);
                    }
                    return false;
                }
            }
        }




        return true;

    }

    public static void Logout(HttpSessionState Session)
    {
        if (Session != null)
        {
            DatabaseConnection dbConn = HROne.Common.WebUtility.GetDatabaseConnection(Session);
            Session.Clear();
            Session.Abandon();
            //dbConn = WebUtils.GetDatabaseConnection();
            //if (dbConn != null)
            //{
            //    WebUtils.SetSessionDatabaseConnection(Session, dbConn);
            //}

        }
    }
}
