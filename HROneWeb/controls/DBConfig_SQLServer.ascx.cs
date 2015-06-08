using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class DBConfig_SQLServer : System.Web.UI.UserControl, WebUtils.HROneDBConfigUIInterface 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (chkCreateDatabase.Checked)
            CreateDatabaseSection.Visible = true;
        else
            CreateDatabaseSection.Visible = false;
    }

    public DatabaseConfig GenerateDBType()
    {
        DatabaseConfig config = new DatabaseConfig();

        if (string.IsNullOrEmpty(MSSQLDatabase.Text))
        {
            throw new Exception("Database must be fill in.");
            //return null;
        }

        config.DBType = WebUtils.DBTypeEmun.MSSQL;

        System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
        connStringBuilder.DataSource = MSSQLServerLocation.Text;
        connStringBuilder.InitialCatalog = MSSQLDatabase.Text;
        connStringBuilder.UserID = MSSQLUserID.Text;
        connStringBuilder.Password = MSSQLPassword.Text;
        config.ConnectionString = connStringBuilder.ConnectionString;

        if (config.TestConnection())
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connStringBuilder.ConnectionString);
            conn.Open();
            DataTable table = conn.GetSchema("Tables");
            conn.Close();
            DataRow[] rows = table.Select("TABLE_NAME='SystemParameter'");
            if (rows.GetLength(0) == 0)
                if (chkCreateDatabase.Checked)
                    HROne.ProductVersion.Database.CreateTableAndData(AppDomain.CurrentDomain.BaseDirectory, config.ConnectionString);
                else
                    throw new Exception("Database does not contain table");
            return config;
        }
        else
        {
            if (config.TestServerConnectionWithoutDatabase())
            {
                if (chkCreateDatabase.Checked)
                {
                    try
                    {
                        string saUser = SAUserID.Text;
                        string saPassword = SAPassword.Text;

                        if (saUser.Trim() == string.Empty)
                        {
                            saUser = MSSQLUserID.Text;
                            saPassword = MSSQLPassword.Text;
                        }
                        connStringBuilder.InitialCatalog = string.Empty;
                        connStringBuilder.UserID = saUser;
                        connStringBuilder.Password = saPassword;

                        HROne.ProductVersion.Database.CreateSchema(connStringBuilder.ConnectionString, MSSQLDatabase.Text, MSSQLUserID.Text);
                        connStringBuilder.InitialCatalog = MSSQLDatabase.Text;
                        HROne.ProductVersion.Database.CreateTableAndData(AppDomain.CurrentDomain.BaseDirectory, connStringBuilder.ConnectionString);

                        return config;

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error on creating Database:\n" + ex.Message);
                    }
                }
                else
                    throw new Exception("Invalid Database name.");
            }
            else
            {
                throw new Exception("Fail to connect to server.");
            }
        }
        //return null;

    }

    //private void CreateSchema(string ConnectionString)
    //{
    //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);

    //    System.Data.SqlClient.SqlCommand command = conn.CreateCommand();
    //    command.CommandType = System.Data.CommandType.Text;

    //    string FullPath = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmptyDatabase"), "HROneDBScheme.sql");
    //    System.IO.FileInfo dbPatchFile = new System.IO.FileInfo(FullPath);
    //    System.IO.StreamReader reader = dbPatchFile.OpenText();
    //    string PatchString = reader.ReadToEnd();
    //    reader.Close();
    //    command.CommandText = PatchString;

    //    command.Connection.Open();
    //    command.ExecuteNonQuery();

    //    FullPath = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmptyDatabase"), "SystemData.sql");
    //    dbPatchFile = new System.IO.FileInfo(FullPath);
    //    reader = dbPatchFile.OpenText();
    //    PatchString = reader.ReadToEnd();
    //    reader.Close();
    //    command.CommandText = PatchString;

    //    command.ExecuteNonQuery();

    //    FullPath = System.IO.Path.Combine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmptyDatabase"), "SupplementaryData.sql");
    //    dbPatchFile = new System.IO.FileInfo(FullPath);
    //    reader = dbPatchFile.OpenText();
    //    PatchString = reader.ReadToEnd();
    //    reader.Close();
    //    command.CommandText = PatchString;

    //    command.ExecuteNonQuery();
    //    command.Connection.Close();
    //}
}
