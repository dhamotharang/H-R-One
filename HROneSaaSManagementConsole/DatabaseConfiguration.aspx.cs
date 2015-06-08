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

public partial class DatabaseConfiguration : System.Web.UI.Page
{
    HROneSaaSConfig config;
    protected void Page_Load(object sender, EventArgs e)
    {
        config = HROneSaaSConfig.GetCurrentConfig();
        if (!System.IO.File.Exists(config.HROneConfigFullPath))
            Response.Redirect("~/HROneConfiguration.aspx");

    }
    protected void OK_Click(object sender, EventArgs e)
    {
        WebUtils.HROneDBConfigUIInterface ui;
        if (true)   //MSSQL
        {
            ui = DBConfig_SQLServer1;
        }
        if (ui != null)
            try
            {
                HROneConfig HROneConfig = new HROneConfig(config.HROneConfigFullPath);

                DatabaseConfig dbConfig = ui.GenerateDBType();
                dbConfig.name = string.Empty;
                HROneConfig.MasterDatabaseConfig = dbConfig;
                HROneConfig.Save();
                //HROne.DataAccess.DatabaseConnection.SetDefaultDatabaseConnection(WebUtils.GetDatabaseConnection());
                //Session.Abandon();
                //HttpRuntime.UnloadAppDomain();
                WebUtils.Logout(Session);
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                Message.Text = ex.Message;
                Message.ForeColor = System.Drawing.Color.Red;
            }
        else
        {
        }
    }
}
