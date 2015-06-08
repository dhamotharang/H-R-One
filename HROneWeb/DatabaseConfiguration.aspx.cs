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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
            if (config.AllowMultiDB)
                DBNameRow.Visible = true;
            else
                DBNameRow.Visible = false;
        }
        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);

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
                HROneConfig config = HROneConfig.GetCurrentHROneConfig();
                DatabaseConfig dbConfig = ui.GenerateDBType();
                dbConfig.name = txtName.Text.Trim();
                config.DatabaseConfigList.Add(dbConfig);
                config.Save();
                //HROne.DataAccess.DatabaseConnection.SetDefaultDatabaseConnection(WebUtils.GetDatabaseConnection());
                //Session.Abandon();
                //HttpRuntime.UnloadAppDomain();
                WebUtils.Logout(Session);
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx");
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
