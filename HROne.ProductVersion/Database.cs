using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using System.Data;
using System.Collections;
using HROne.Lib.Entities;

namespace HROne.ProductVersion
{
    public sealed class Database
    {
        public const string CURRENT_DB_VERSION = "1.0.31b";

        DatabaseConnection dbConn;
        string HROneWebPath = string.Empty;
        public Database(DatabaseConnection dbConn)
        {
            this.dbConn = dbConn;
            this.HROneWebPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        public Database(DatabaseConnection dbConn, string HROneWebPath)
        {
            this.dbConn = dbConn;
            this.HROneWebPath = HROneWebPath;
        }

        public string RunningDatabaseVersion()
        {
            return ESystemParameter.getParameter(dbConn, "DBVersion");
        }

        private string strLastErrorMessage = string.Empty;
        public string LastErrorMessage
        {
            get { return strLastErrorMessage; }
        }

        public bool IsUpdateRequired()
        {
            //string[] running_elements = RunningDatabaseVersion().Split('.');
            //string[] system_elements = CURRENT_DB_VERSION.Split('.');

            try
            {
                return (RunningDatabaseVersion().CompareTo(CURRENT_DB_VERSION) < 0);
                //return (int.Parse(running_elements[0]) * 10000 + int.Parse(running_elements[1]) * 100 + int.Parse(running_elements[2]) <
                //        int.Parse(system_elements[0]) * 10000 + int.Parse(system_elements[1]) * 100 + int.Parse(system_elements[2])
                //        );
            }
            catch 
            {
            }
            return false;
        }

        public bool UpdateDatabaseVersion(bool ForceBackupDatabase)
        {
            strLastErrorMessage = string.Empty;

            if (IsUpdateRequired())
            {
                string dbVersion = RunningDatabaseVersion();
                bool passBackupDatabase = true;
                if (ForceBackupDatabase)
                    passBackupDatabase = AutoBackUpDatabase(dbVersion);
                if (passBackupDatabase)
                {
                    string scriptFolder = System.IO.Path.Combine(HROneWebPath, @"DBpatch");
                    System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(scriptFolder);
                    System.IO.FileInfo[] fileInfoList = directoryInfo.GetFiles("*.sql", System.IO.SearchOption.AllDirectories);

                    foreach (System.IO.FileInfo m_file in fileInfoList)
                    {
                        string m_versionBeforePatch = RunningDatabaseVersion();

                        if (m_file.ToString().ToLower().CompareTo((m_versionBeforePatch + ".sql").ToLower()) > 0)
                        {
                            if (m_versionBeforePatch.CompareTo(CURRENT_DB_VERSION) < 0)
                            {
                                RunDataPatch(m_file.FullName);
                                string m_versionAfterPatch = RunningDatabaseVersion();
                                
                                if (m_versionBeforePatch != m_versionAfterPatch)
                                    DataPatchProgram(m_versionAfterPatch);
                            }
                            else
                                break;
                        }
                    }
                }

                if (RunningDatabaseVersion() != CURRENT_DB_VERSION)
                {
                    strLastErrorMessage = "Patch failed.  Current DB Version is " + RunningDatabaseVersion().ToString() + " and expected version is " + CURRENT_DB_VERSION;
                }

                if (!String.IsNullOrEmpty(LastErrorMessage))
                {
                    return false;
                }
            }
            return true;

        }

        public void DataPatchProgram(string versionNo)
        {
            if (versionNo == "1.0.20")  // patch Nationality, Country, Place Of Birth
            {
                // Patch Nationality
                DBFilter m_empPersonalInfoFilter = new DBFilter();
                m_empPersonalInfoFilter.add(new NullTerm("NOT EmpNationality"));
                DataTable m_table = m_empPersonalInfoFilter.loadData(dbConn, "SELECT DISTINCT EmpNationality FROM EmpPersonalInfo");
                string m_decodedString;

                foreach (DataRow m_row in m_table.Rows)
                {
                    m_decodedString = AppUtils.Decode(EEmpPersonalInfo.db.getField("EmpNationality"), m_row[0].ToString().Trim());

                    if (m_decodedString != "")
                    {
                        ENationality m_nationality = new ENationality();

                        if (m_decodedString.Length > 20)
                            m_nationality.NationalityCode = m_decodedString.Substring(0, 20);
                        else
                            m_nationality.NationalityCode = m_decodedString;

                        if (m_decodedString.Length > 100)
                            m_nationality.NationalityDesc = m_decodedString.Substring(0, 100);
                        else
                            m_nationality.NationalityDesc = m_decodedString;

                        ENationality.db.insert(dbConn, m_nationality);
                    }
                }

                // patch Country Code
                m_empPersonalInfoFilter.add(new NullTerm("NOT EmpPassportIssuedCountry"));
                m_table = m_empPersonalInfoFilter.loadData(dbConn, "SELECT DISTINCT EmpPassportIssuedCountry FROM EmpPersonalInfo");

                foreach (DataRow m_row in m_table.Rows)
                {
                    m_decodedString = AppUtils.Decode(EEmpPersonalInfo.db.getField("EmpPassportIssuedCountry"), m_row[0].ToString().Trim());

                    if (m_decodedString != "")
                    {
                        EIssueCountry m_country = new EIssueCountry();

                        if (m_decodedString.Length > 20)
                            m_country.CountryCode = m_decodedString.Substring(0, 20);
                        else
                            m_country.CountryCode = m_decodedString;

                        if (m_decodedString.Length > 100)
                            m_country.CountryDesc = m_decodedString.Substring(0, 100);
                        else
                            m_country.CountryDesc = m_decodedString;

                        EIssueCountry.db.insert(dbConn, m_country);
                    }
                }

                // patch Place of Birth
                m_empPersonalInfoFilter.add(new NullTerm("NOT EmpPlaceOfBirth"));
                m_table = m_empPersonalInfoFilter.loadData(dbConn, "SELECT DISTINCT EmpPlaceOfBirth FROM EmpPersonalInfo");
                
                foreach (DataRow m_row in m_table.Rows)
                {
                    m_decodedString = AppUtils.Decode(EEmpPersonalInfo.db.getField("EmpPlaceOfBirth"), m_row[0].ToString().Trim());

                    if (m_decodedString != "")
                    {
                        EPlaceOfBirth m_placeOfBirth = new EPlaceOfBirth();

                        if (m_decodedString.Length > 20)
                            m_placeOfBirth.PlaceOfBirthCode = m_decodedString.Substring(0, 20);
                        else
                            m_placeOfBirth.PlaceOfBirthCode = m_decodedString;

                        if (m_decodedString.Length > 100)
                            m_placeOfBirth.PlaceOfBirthDesc = m_decodedString.Substring(0, 100);
                        else
                            m_placeOfBirth.PlaceOfBirthDesc = m_decodedString;

                        EPlaceOfBirth.db.insert(dbConn, m_placeOfBirth);
                    }
                }

                // patch EmpPersonalInfo
                foreach (EEmpPersonalInfo m_info in EEmpPersonalInfo.db.select(dbConn, new DBFilter()))
                {
                    bool m_update = false; 
                    if (!string.IsNullOrEmpty(m_info.EmpPlaceOfBirth))
                    {
                        DBFilter m_filter = new DBFilter();
                        m_filter.add(new Match("PlaceOfBirthCode", m_info.EmpPlaceOfBirth));
                        ArrayList m_list = EPlaceOfBirth.db.select(dbConn, m_filter);
                        if (m_list.Count > 0)
                        {
                            m_info.EmpPlaceOfBirthID = ((EPlaceOfBirth)m_list[0]).PlaceOfBirthID;
                        }
                        m_update = true;
                    }

                    if (!string.IsNullOrEmpty(m_info.EmpPassportIssuedCountry))
                    {
                        DBFilter m_filter = new DBFilter();
                        m_filter.add(new Match("CountryCode", m_info.EmpPassportIssuedCountry));
                        ArrayList m_list = EIssueCountry.db.select(dbConn, m_filter);
                        if (m_list.Count > 0)
                        {
                            m_info.EmpPassportIssuedCountryID = ((EIssueCountry)m_list[0]).CountryID;
                        }
                    }

                    if (!string.IsNullOrEmpty(m_info.EmpNationality))
                    {
                        DBFilter m_filter = new DBFilter();
                        m_filter.add(new Match("NationalityCode", m_info.EmpNationality));
                        ArrayList m_list = ENationality.db.select(dbConn, m_filter);
                        if (m_list.Count > 0)
                        {
                            m_info.EmpNationalityID = ((ENationality)m_list[0]).NationalityID;
                        }
                    }

                    if (m_update == true)
                    {
                        EEmpPersonalInfo.db.update(dbConn, m_info);                        
                    }
                
                }
            }
        }

        public void RunDataPatch(string Filename)
        {
            strLastErrorMessage = string.Empty;
            try
            {
                string FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"DBPatch"), Filename);
                System.IO.FileInfo dbPatchFile = new System.IO.FileInfo(FullPath);
                System.IO.StreamReader reader = dbPatchFile.OpenText();
                string PatchString = reader.ReadToEnd();
                reader.Close();
                AppUtils.DBPatch(dbConn, PatchString, out strLastErrorMessage);
            }
            catch (Exception ex)
            {
                strLastErrorMessage = ex.Message;
            }
        }

        private bool AutoBackUpDatabase(string VersionNo)
        {
            string filePath = string.Empty;
            if (dbConn.dbType == DatabaseConnection.DatabaseType.MSSQL)
            {
                System.Data.SqlClient.SqlConnection conn = (System.Data.SqlClient.SqlConnection)dbConn.Connection;
                filePath = conn.Database + "_" + VersionNo + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".bak";
            }
            return AppUtils.BackUpDatabase(dbConn, filePath, out strLastErrorMessage);
            //if (perspectivemind.common.DBUtil.type is perspectivemind.common.SQLType)
            //{
            //    System.Data.SqlClient.SqlConnection conn = (System.Data.SqlClient.SqlConnection)perspectivemind.common.DBUtil.getConnection();
            //    string backupSQL = "BACKUP DATABASE " + conn.Database + "\r\n" +
            //        "TO DISK='" + conn.Database + "_" + VersionNo + "_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".bak'";
            //    if (AppUtils.DBPatch(backupSQL, out strLastErrorMessage))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        public static void CreateSchema(string ConnectionStringWithoutInitialCatalog, string DatabaseName, string GrantUserID)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionStringWithoutInitialCatalog);

            System.Data.SqlClient.SqlCommand command = conn.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "Create Database " + DatabaseName + "\r\n";

            command.Connection.Open();
            command.ExecuteNonQuery();

            //if (MSSQLUserID.Text.Trim() != saUser.Trim())
            {
                command.CommandText = "USE " + DatabaseName + "\r\n"
                + "CREATE USER " + GrantUserID + " FOR LOGIN " + GrantUserID + "\r\n"
                + "EXEC sp_addrolemember N'db_owner', N'" + GrantUserID + "'";

                command.ExecuteNonQuery();
            }
            command.Connection.Close();
        }

        public static void CreateTableAndData(string HROneWebPath, string ConnectionString)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);

            System.Data.SqlClient.SqlCommand command = conn.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;

            string FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"EmptyDatabase"), "HROneDBScheme.sql");
            System.IO.FileInfo dbPatchFile = new System.IO.FileInfo(FullPath);
            System.IO.StreamReader reader = dbPatchFile.OpenText();
            string PatchString = reader.ReadToEnd();
            reader.Close();
            command.CommandText = PatchString;

            command.Connection.Open();
            command.ExecuteNonQuery();

            //FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"EmptyDatabase"), "SystemData.sql");
            //dbPatchFile = new System.IO.FileInfo(FullPath);
            //reader = dbPatchFile.OpenText();
            //PatchString = reader.ReadToEnd();
            //reader.Close();
            //command.CommandText = PatchString;

            //command.ExecuteNonQuery();

            //FullPath = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"EmptyDatabase"), "SupplementaryData.sql");
            //dbPatchFile = new System.IO.FileInfo(FullPath);
            //reader = dbPatchFile.OpenText();
            //PatchString = reader.ReadToEnd();
            //reader.Close();
            //command.CommandText = PatchString;

            //command.ExecuteNonQuery();

            // apply pre-HROne scripts
            string scriptFolder = System.IO.Path.Combine(System.IO.Path.Combine(HROneWebPath, @"EmptyDatabase"), @"preload");
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(scriptFolder);
            System.IO.FileInfo[] fileInfoList = directoryInfo.GetFiles("*.sql", System.IO.SearchOption.AllDirectories);

            foreach (System.IO.FileInfo m_file in fileInfoList)
            {
                dbPatchFile = new System.IO.FileInfo(m_file.FullName);
                reader = dbPatchFile.OpenText();
                PatchString = reader.ReadToEnd();
                reader.Close();
                command.CommandText = PatchString;

                System.Threading.Thread.Sleep(1000);
                command.ExecuteNonQuery();
            }
            command.Connection.Close();
        }
    }
}
