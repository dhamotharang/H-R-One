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
                throw new Exception("Database does not contain table");
            return config;
        }
        else
        {
            if (config.TestServerConnectionWithoutDatabase())
            {

                throw new Exception("Invalid Database name.");
            }
            else
            {
                throw new Exception("Fail to connect to server.");
            }
        }
        //return null;

    }

}
