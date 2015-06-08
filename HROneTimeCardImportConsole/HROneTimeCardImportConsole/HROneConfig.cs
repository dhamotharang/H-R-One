using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using HROne.DataAccess;
using HROne.Common;

namespace HROneImportConsole
{
    public enum DBTypeEmun
    {
        None = 0,
        MSSQL = 1

    }

    public class DatabaseConfig
    {
        public string name = string.Empty;
        public DBTypeEmun DBType = DBTypeEmun.None;
        public string ConnectionString = string.Empty;

        public bool TestConnection()
        {
            if (DBType.Equals(DBTypeEmun.MSSQL))
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
            if (DBType.Equals(DBTypeEmun.MSSQL))
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
            if (DBType == DBTypeEmun.MSSQL)
                return new DatabaseConnectionWithAudit(ConnectionString, DatabaseConnection.DatabaseType.MSSQL);
            else
                return null;
        }
    }


    public class HROneConfig
    {
        private const string keyString = "HROne";

        //[ThreadStatic]
        //private static HROneConfig currentHROneConfig = null;

        private const string ConfigFilename = "HROne.config";
        //public DBTypeEmun DBType = DBTypeEmun.None;
        //public string ConnectionString = string.Empty;
        public bool ShutDownDomainAfterUsed = false;
        public bool AllowMultiDB = false;

        public List<DatabaseConfig> DatabaseConfigList = new List<DatabaseConfig>();

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


            System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
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
                config.DBType = DBTypeEmun.MSSQL;
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


            System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();
            XmlElement settings = config.CreateElement("Settings");
            XmlAttribute version = config.CreateAttribute("Version");
            version.Value = "2.0";
            settings.Attributes.Append(version);
            config.AppendChild(settings);
            SetDatabaseConfigList(settings);

            settings.AppendChild(config.CreateElement("ShutdownAfterUsed"));
            settings["ShutdownAfterUsed"].InnerText = ShutDownDomainAfterUsed ? "true" : "false";

            settings.AppendChild(config.CreateElement("AllowMultiDB"));
            settings["AllowMultiDB"].InnerText = AllowMultiDB ? "true" : "false";

            config.Save(filename);
        }
        private void SetDatabaseConfigList(XmlElement settingNode)
        {
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
            if (dbConfig.DBType.Equals(DBTypeEmun.MSSQL))
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
}
