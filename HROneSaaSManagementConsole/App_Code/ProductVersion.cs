using System;
using System.Collections;
using System.IO;
using System.Configuration;
using HROne.Lib.Entities;
using HROne.DataAccess;
/// <summary>
/// Summary description for ProductVersion
/// </summary>
public static class ProductVersion
{

    public const string CURRENT_DB_VERSION = "2.0";

    public static string RunningDatabaseVersion(DatabaseConnection dbConn)
    {
        return ESystemParameter.getParameter(dbConn, "DBVersion");
    }

    private static string strLastErrorMessage = string.Empty;
    public static string LastErrorMessage
    {
        get { return strLastErrorMessage; }
    }
    public static bool UpdateDatabaseVersion(DatabaseConnection dbConn, bool ForceBackupDatabase)
    {
        strLastErrorMessage = string.Empty;

        string dbVersion = RunningDatabaseVersion(dbConn);
        if (string.Compare(dbVersion, CURRENT_DB_VERSION) < 0)
        {
            bool passBackupDatabase = true;
            if (ForceBackupDatabase)
                passBackupDatabase = AutoBackUpDatabase(dbConn, dbVersion);
            if (passBackupDatabase)
            {
                if (dbVersion.StartsWith("0."))
                {
                    if (dbVersion.Equals("0.1"))
                    {
                        RunDataPatch(dbConn, "20120829_0_2.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.2"))
                    {
                        RunDataPatch(dbConn, "20120905_0_3.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.3"))
                    {
                        RunDataPatch(dbConn, "20120907_0_4.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.4"))
                    {
                        RunDataPatch(dbConn, "20120911_0_5.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.5"))
                    {
                        RunDataPatch(dbConn, "20120914_0_6.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.6"))
                    {
                        RunDataPatch(dbConn, "20120917_0_7.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.7"))
                    {
                        RunDataPatch(dbConn, "20120919_0_8.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.8"))
                    {
                        RunDataPatch(dbConn, "20120922_0_9.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("0.9"))
                    {
                        RunDataPatch(dbConn, "20120926_1_0.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                }
                if (dbVersion.StartsWith("1."))
                {
                    if (dbVersion.Equals("1.0"))
                    {
                        RunDataPatch(dbConn, "20121003_1_1.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("1.1"))
                    {
                        RunDataPatch(dbConn, "20121004_1_2.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("1.2"))
                    {
                        RunDataPatch(dbConn, "20121024_1_3.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);                        
                    }
                    if (dbVersion.Equals("1.3"))
                    {
                        RunDataPatch(dbConn, "20121026_1_4.sql");
                        RunDataPatch(dbConn, "20121026_1_5.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("1.5"))
                    {
                        RunDataPatch(dbConn, "20121213_1_6.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("1.6"))
                    {
                        RunDataPatch(dbConn, "20121214_1_7.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("1.7"))
                    {
                        RunDataPatch(dbConn, "20121219_1_8.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("1.8"))
                    {
                        RunDataPatch(dbConn, "20130214_1_9.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                    if (dbVersion.Equals("1.9"))
                    {
                        RunDataPatch(dbConn, "20130812_2_0.sql");
                        dbVersion = RunningDatabaseVersion(dbConn);
                    }
                }
            }
            if (!String.IsNullOrEmpty(LastErrorMessage))
            {
                return false;
            }
        }
        return true;

    }

    public static void RunDataPatch(DatabaseConnection dbConn, string Filename)
    {
        strLastErrorMessage = string.Empty;
        try
        {
            string FullPath = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"DBPatch"), Filename);
            FileInfo dbPatchFile = new FileInfo(FullPath);
            StreamReader reader = dbPatchFile.OpenText();
            string PatchString = reader.ReadToEnd();
            reader.Close();
            AppUtils.DBPatch(dbConn, PatchString, out strLastErrorMessage);
        }
        catch (Exception ex)
        {
            strLastErrorMessage = ex.Message;
        }
    }

    private static bool AutoBackUpDatabase(DatabaseConnection dbConn, string VersionNo)
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

}
