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

public partial class HROneConfiguration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void OK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPhysiclPath.Text))
        {
            Message.Text = "Physcal Path cannot be empty";
            Message.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (System.IO.File.Exists(txtPhysiclPath.Text))
        {
            HROneSaaSConfig config = HROneSaaSConfig.GetCurrentConfig();
            config.HROneConfigFullPath = txtPhysiclPath.Text;
            config.Save();

            Response.Redirect("~/Default.aspx");
        }
        else
        {
            Message.Text = "File not found: " + txtPhysiclPath.Text;
            Message.ForeColor = System.Drawing.Color.Red;
            return;
        }
    }
}
