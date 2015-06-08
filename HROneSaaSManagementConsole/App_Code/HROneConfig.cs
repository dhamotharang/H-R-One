using System;
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

    public DatabaseConnection CreateDatabaseConnectionObject()
    {
        if (DBType == WebUtils.DBTypeEmun.MSSQL)
            return new DatabaseConnection(ConnectionString, DatabaseConnection.DatabaseType.MSSQL);
        else
            return null;
    }
}


public class HROneConfig
{
    private const string keyString = "HROne";

    private const string ConfigFilename = "HROne.config";
    //public DBTypeEmun DBType = DBTypeEmun.None;
    //public string ConnectionString = string.Empty;
    public string MasterDatabaseConnectionString = string.Empty;
    public string databaseEncryptKey = string.Empty;

    protected System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
    public DatabaseConfig  MasterDatabaseConfig = null;

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
                        MasterDatabaseConfig = dbconfig;
                        //DBType = dbconfig.DBType;
                        //ConnectionString = dbconfig.ConnectionString;
                    }

                }
                else if (config["Settings"].Attributes["Version"].Value == "2.0")
                {
                    XmlNodeList dbConfigXmlList = config["Settings"].GetElementsByTagName("MasterDatabaseConfig");
                    foreach (XmlElement dbConfigXML in dbConfigXmlList)
                    {
                        MasterDatabaseConfig = LoadDatabaseConfig(dbConfigXML);
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
                connStringBuilder.ApplicationName = "HROneWebSaaS";
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
        //{
        //    XmlAttribute version = config.CreateAttribute("Version");
        //    version.Value = "2.0";
        //    settings.Attributes.Append(version);
        //    config.AppendChild(settings);
        //}
        //SetDatabaseConfigList(settings);

        XmlElement masterDatabaseNode = GetOrCreateElement(settings, "MasterDatabaseConfig");
        SetDatabaseConfig(masterDatabaseNode, MasterDatabaseConfig);

        XmlElement databaseEncryptKeyNode = GetOrCreateElement(settings, "DatabaseEncryptKey");
        databaseEncryptKeyNode.InnerText = databaseEncryptKey;
        config.Save(filename);
    }

    protected XmlElement GetOrCreateElement(XmlNode parentNode, string elementName)
    {
        XmlElement element = parentNode[elementName];
        if (element == null)
        {
            parentNode.AppendChild(parentNode.OwnerDocument.CreateElement(elementName));
            element = parentNode[elementName];
        }
        return element;
    }

    private void SetDatabaseConfig(XmlElement databaseConfigRootNode, DatabaseConfig dbConfig)
    {
        GetOrCreateElement(databaseConfigRootNode, "name").InnerText = dbConfig.name;
        if (dbConfig.DBType.Equals(WebUtils.DBTypeEmun.MSSQL))
        {
            HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

            GetOrCreateElement(databaseConfigRootNode, "dbtype").InnerText = "MSSQL";

            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(dbConfig.ConnectionString);
            connStringBuilder.Password = crypto.Encrypting(connStringBuilder.Password, keyString);
            GetOrCreateElement(databaseConfigRootNode, "ConnectionString").InnerText = connStringBuilder.ConnectionString;

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
        if (MasterDatabaseConfig != null)
            if (MasterDatabaseConfig.TestConnection())
                return MasterDatabaseConfig.CreateDatabaseConnectionObject();
        return null;
    }
}
