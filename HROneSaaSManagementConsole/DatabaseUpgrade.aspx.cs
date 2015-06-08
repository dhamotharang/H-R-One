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
using HROne.DataAccess;
public partial class DatabaseUpgrade : HROneWebPage
{
    HROne.ProductVersion.Database database = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        database = new HROne.ProductVersion.Database(dbConn);
        lblDBVersion.Text = database.RunningDatabaseVersion();
        lblRequiredDBVersion.Text = ProductVersion.CURRENT_DB_VERSION; 

    }
    protected void btnPatchWithoutBackup_Click(object sender, EventArgs e)
    {
        if (ProductVersion.UpdateDatabaseVersion(dbConn, false))
        {
            Session.Remove("NeedDBUpgrade");
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx");
        }
        else
            Message.Text = ProductVersion.LastErrorMessage;

    }
}
