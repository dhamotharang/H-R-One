using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Xml;
using HROne.DataAccess;

public class DatabaseConfig
{
    public string name = string.Empty;
    public WebUtils.DBTypeEmun DBType = WebUtils.DBTypeEmun.None;
    public string ConnectionString = string.Empty;

    public bool TestConnection()
    {
        if (DBType.Equals(WebUtils.DBTypeEmun.MSSQL))
        {
            System.Data.SqlClient.SqlConnection testConn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            try
            {
                testConn.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                testConn.Close();
            }
            return true;
        }
        else
            return false;
    }

    public bool TestServerConnectionWithoutDatabase()
    {
        if (DBType.Equals(WebUtils.DBTypeEmun.MSSQL))
        {
            System.Data.SqlClient.SqlConnection testConn = new System.Data.SqlClient.SqlConnection();
            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);
            connStringBuilder.InitialCatalog = "";
            testConn.ConnectionString = connStringBuilder.ConnectionString;
            try
            {
                testConn.Open();
                testConn.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
        else
            return false;
    }

    public DatabaseConnectionWithAudit CreateDatabaseConnectionObject()
    {
        if (DBType == WebUtils.DBTypeEmun.MSSQL)
            return new DatabaseConnectionWithAudit(ConnectionString, DatabaseConnection.DatabaseType.MSSQL);
        else
            return null;
    }
}


public class HROneConfig
{
    //[ThreadStatic]
    //private static HROneConfig currentHROneConfig = null;
    private const string keyString = "HROne";

    private const string ConfigFilename = "HROne.config";
    //public DBTypeEmun DBType = DBTypeEmun.None;
    //public string ConnectionString = string.Empty;
    public bool ShutDownDomainAfterUsed = false;
    public bool AllowMultiDB = false;
    public string DefaultDocumentPath = string.Empty;
    public bool GenerateReportAsInbox = false;
    public bool EncryptedURLQueryString = false;
    // Start 0000134, Ricky So, 2014-11-21
    public bool SkipEncryptionToDB = false;
    // End 0000134, Ricky So, 2014-11-21
    public bool UsePDFCreator = false;
    public string PDFCreatorPrinterName = string.Empty;
    public string databaseEncryptKey = string.Empty;

    protected System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
    public List<DatabaseConfig> DatabaseConfigList = new List<DatabaseConfig>();
    public DatabaseConfig MasterDatabaseConfig = null;

    protected HROneConfig()
    {
        load(getFilename());
    }

    public HROneConfig(string filename)
    {
        load(filename);
    }

    public static HROneConfig GetCurrentHROneConfig()
    {
        //if (currentHROneConfig == null)
        //    currentHROneConfig = new HROneConfig();
        //return currentHROneConfig;
        return new HROneConfig();
    }

    public void load(string filename)
    {
        MasterDatabaseConfig = null;

        try
        {
            config.Load(filename);
            if (config["Settings"] != null)
            {
                if (config["Settings"].Attributes["Version"] == null)
                {
                    DatabaseConfig dbconfig = LoadDatabaseConfig(config["Settings"]);
                    if (dbconfig != null)
                    {
                        DatabaseConfigList.Add(dbconfig);
                        //DBType = dbconfig.DBType;
                        //ConnectionString = dbconfig.ConnectionString;
                    }
                    if (config["Settings"]["ShutdownAfterUsed"] != null)
                    {
                        string strShutdownAfterUsed = config["Settings"]["ShutdownAfterUsed"].InnerText;
                        if (strShutdownAfterUsed.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                        || strShutdownAfterUsed.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ShutDownDomainAfterUsed = true;
                        }
                        else
                            ShutDownDomainAfterUsed = false;
                    }
                }
                else if (config["Settings"].Attributes["Version"].Value == "2.0")
                {
                    XmlNodeList masterDbConfigXmlList = config["Settings"].GetElementsByTagName("MasterDatabaseConfig");
                    foreach (XmlElement dbConfigXML in masterDbConfigXmlList)
                    {
                        MasterDatabaseConfig = LoadDatabaseConfig(dbConfigXML);
                    }

                    if (config["Settings"]["DatabaseEncryptKey"] != null)
                    {
                        databaseEncryptKey = config["Settings"]["DatabaseEncryptKey"].InnerText;
                    }
                    if (config["Settings"]["UsePDFCreator"] != null)
                    {
                        string tmpValue = config["Settings"]["UsePDFCreator"].InnerText;
                        if (tmpValue.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                        || tmpValue.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                        {
                            UsePDFCreator = true;
                        }
                        else
                            UsePDFCreator = false;
                    }
                    if (config["Settings"]["PDFCreatorPrinterName"] != null)
                    {
                        PDFCreatorPrinterName = config["Settings"]["PDFCreatorPrinterName"].InnerText;
                    }

                    if (MasterDatabaseConfig == null)
                    {
                        XmlNodeList dbConfigXmlList = config["Settings"].GetElementsByTagName("DatabaseConfig");
                        foreach (XmlElement dbConfigXML in dbConfigXmlList)
                        {
                            DatabaseConfig dbconfig = LoadDatabaseConfig(dbConfigXML);
                            if (dbconfig != null)
                            {
                                DatabaseConfigList.Add(dbconfig);
                                //DBType = dbconfig.DBType;
                                //ConnectionString = dbconfig.ConnectionString;
                            }
                        }
                        if (config["Settings"]["ShutdownAfterUsed"] != null)
                        {
                            string strShutdownAfterUsed = config["Settings"]["ShutdownAfterUsed"].InnerText;
                            if (strShutdownAfterUsed.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                            || strShutdownAfterUsed.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                            {
                                ShutDownDomainAfterUsed = true;
                            }
                            else
                                ShutDownDomainAfterUsed = false;
                        }
                        if (config["Settings"]["AllowMultiDB"] != null)
                        {
                            string strAllowMultiDB = config["Settings"]["AllowMultiDB"].InnerText;
                            if (strAllowMultiDB.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                            || strAllowMultiDB.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                            {
                                AllowMultiDB = true;
                            }
                            else
                                AllowMultiDB = false;
                        }
                        if (config["Settings"]["GenerateReportAsInbox"] != null)
                        {
                            string tmpValue = config["Settings"]["GenerateReportAsInbox"].InnerText;
                            if (tmpValue.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                            || tmpValue.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                            {
                                GenerateReportAsInbox = true;
                            }
                            else
                                GenerateReportAsInbox = false;
                        }
                        if (config["Settings"]["EncryptedURLQueryString"] != null)
                        {
                            string tmpValue = config["Settings"]["EncryptedURLQueryString"].InnerText;
                            if (tmpValue.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                            || tmpValue.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                            {
                                EncryptedURLQueryString = true;
                            }
                            else
                                EncryptedURLQueryString = false;
                        }
                        // Start 0000134, Ricky So, 2014-11-21
                        if (config["Settings"]["SkipEncryptionToDB"] != null)
                        {
                            string tmpValue = config["Settings"]["SkipEncryptionToDB"].InnerText;
                            if (tmpValue.Equals("Yes", StringComparison.CurrentCultureIgnoreCase)
                            || tmpValue.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                            {
                                SkipEncryptionToDB = true;
                            }
                            else
                                SkipEncryptionToDB = false;
                        }
                        // End 0000134, Ricky So, 2014-11-21                        
                    }
                    else
                    {
                        GenerateReportAsInbox = true;
                        EncryptedURLQueryString = true;
                    }
                }
            }



        }
        catch
        {
        }

    }

    private DatabaseConfig LoadDatabaseConfig(XmlElement node)
    {
        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

        string strDBType = string.Empty;
        string strConnectionString = string.Empty;
        string strName = string.Empty;
        if (node["dbtype"] != null)
        {
            strDBType = node["dbtype"].InnerText;

        }
        if (node["name"] != null)
        {
            strName = node["name"].InnerText;

        }
        if (node["ConnectionString"] != null)
            strConnectionString = node["ConnectionString"].InnerText;

        if (strDBType.Equals("MSSQL"))
        {
            DatabaseConfig config = new DatabaseConfig();
            config.DBType = WebUtils.DBTypeEmun.MSSQL;
            config.name = strName;
            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(strConnectionString);
            try
            {
                connStringBuilder.Password = crypto.Decrypting(connStringBuilder.Password, keyString);
                connStringBuilder.ApplicationName = "HROneWeb";
                config.ConnectionString = connStringBuilder.ConnectionString;
            }
            catch
            {

            }
            return config;
        }
        else
            return null;
    }

    public void Save()
    {
        string errorMessage = null;
        if (!HROne.CommonLib.FileIOProcess.IsFolderAllowWritePermission(AppDomain.CurrentDomain.BaseDirectory, out errorMessage))
            throw new Exception(errorMessage);
        string filename = getFilename();

        if (config == null)
            config = new System.Configuration.ConfigXmlDocument();
        else
            filename = config.Filename;

        XmlElement settings = GetOrCreateElement(config, "Settings");

        if (settings.Attributes["Version"] == null)
        {
            settings.RemoveAll();
        }
        settings.RemoveAttribute("Version");
        XmlAttribute version = config.CreateAttribute("Version");
        version.Value = "2.0";
        settings.Attributes.Append(version);
        config.AppendChild(settings);
        SetDatabaseConfigList(settings);

        XmlElement shutdownOption = GetOrCreateElement(settings, "ShutdownAfterUsed");
        shutdownOption.InnerText = ShutDownDomainAfterUsed ? "true" : "false";

        XmlElement multiDBOption = GetOrCreateElement(settings, "AllowMultiDB");
        multiDBOption.InnerText = AllowMultiDB ? "true" : "false";

        XmlElement EncryptedURLOption = GetOrCreateElement(settings, "EncryptedURLQueryString");
        EncryptedURLOption.InnerText = EncryptedURLQueryString ? "true" : "false";

        XmlElement UsePDFCreatorOption = GetOrCreateElement(settings, "UsePDFCreator");
        UsePDFCreatorOption.InnerText = UsePDFCreator ? "true" : "false";

        XmlElement PDFCreatorPrinterNameOption = GetOrCreateElement(settings, "PDFCreatorPrinterNameOption");
        PDFCreatorPrinterNameOption.InnerText = PDFCreatorPrinterName;

        config.Save(filename);
    }

    protected XmlElement GetOrCreateElement(XmlNode parentNode, string elementName)
    {
        XmlElement element = parentNode[elementName];
        if (element == null)
        {
            XmlDocument xmlDocument = null;
            if (parentNode is XmlDocument)
                xmlDocument = (XmlDocument)parentNode;
            else
                xmlDocument = parentNode.OwnerDocument;

            parentNode.AppendChild(xmlDocument.CreateElement(elementName));
            element = parentNode[elementName];
        }
        return element;
    }

    private void SetDatabaseConfigList(XmlElement settingNode)
    {
        //  clear the array
        XmlNodeList nodeList = settingNode.GetElementsByTagName("DatabaseConfig");

        List<XmlNode> removeNodeList = new List<XmlNode>();
        for (int idx = 0; idx < nodeList.Count; idx++)
            removeNodeList.Add(nodeList[idx]);

        foreach (XmlNode node in removeNodeList)
            settingNode.RemoveChild(node);

        for (int idx = 0; idx < DatabaseConfigList.Count; idx++)
        {
            if (!AllowMultiDB)
                idx = DatabaseConfigList.Count - 1;
            DatabaseConfig dbConfig = DatabaseConfigList[idx];
            XmlElement databaseConfigXML = settingNode.OwnerDocument.CreateElement("DatabaseConfig");
            databaseConfigXML.SetAttribute("index", idx.ToString());
            SetDatabaseConfig(databaseConfigXML, dbConfig);
            settingNode.AppendChild(databaseConfigXML);
        }
    }
    private void SetDatabaseConfig(XmlElement databaseConfigRootNode, DatabaseConfig dbConfig)
    {
        XmlDocument config = databaseConfigRootNode.OwnerDocument;
        databaseConfigRootNode.AppendChild(config.CreateElement("name"));
        databaseConfigRootNode["name"].InnerText = dbConfig.name;
        databaseConfigRootNode.AppendChild(config.CreateElement("dbtype"));
        if (dbConfig.DBType.Equals(WebUtils.DBTypeEmun.MSSQL))
        {
            HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

            databaseConfigRootNode["dbtype"].InnerText = "MSSQL";
            databaseConfigRootNode.AppendChild(config.CreateElement("ConnectionString"));

            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(dbConfig.ConnectionString);
            connStringBuilder.Password = crypto.Encrypting(connStringBuilder.Password, keyString);
            databaseConfigRootNode["ConnectionString"].InnerText = connStringBuilder.ConnectionString;

        }
    }
    private string getFilename()
    {
        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFilename);

    }



    //public perspectivemind.common.DatabaseType GetDatabaseType()
    //{
    //    if (TestConnection())
    //        if (DBType.Equals(DBTypeEmun.MSSQL))
    //        {
    //            SQLType sqlType = new SQLType();
    //            sqlType.url = ConnectionString;
    //            return sqlType;
    //        }
    //        else
    //            return null;
    //    else
    //        return null;
    //}
    //public DatabaseConnection GetDatabaseConnection(int index)
    //{
    //    DatabaseConfig dbConfig =DatabaseConfigList[index];
    //    if (dbConfig.TestConnection())
    //        if (dbConfig.DBType.Equals(DBTypeEmun.MSSQL))
    //        {
    //            return new DatabaseConnection(dbConfig.ConnectionString, DatabaseConnection.DatabaseType.MSSQL);
    //        }
    //    return null;
    //}
    public DatabaseConnection GetDatabaseConnection()
    {
        if (DatabaseConfigList.Count > 1)
            return null;
        foreach (DatabaseConfig dbConfig in DatabaseConfigList)
        {

            if (dbConfig.TestConnection())
                return dbConfig.CreateDatabaseConnectionObject();
        }
        return null;
    }
    public DatabaseConnection GetDatabaseConnection(string DatabaseName)
    {
        foreach (DatabaseConfig dbConfig in DatabaseConfigList)
        {
            if (dbConfig.name.Equals(DatabaseName, StringComparison.CurrentCultureIgnoreCase))
                if (dbConfig.TestConnection())
                    return dbConfig.CreateDatabaseConnectionObject();
                else
                    break;
        }
        return null;
    }
}
