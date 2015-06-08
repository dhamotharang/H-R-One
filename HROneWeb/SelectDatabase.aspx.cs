//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;

////  Class name Login should NOT be used to prevent a bug caused by old version of Microsoft.Net framework
//public partial class SelectDatabase : System.Web.UI.Page
//{
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        //if (!IsPostBack)
//        //    HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


//        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");

//        //  Add meta for IE9 to run as IE8
//        if (Request.Browser.Browser.Equals("IE") && Request.Browser.Version.StartsWith("9."))
//        {
//            HtmlMeta htmlMeta = new HtmlMeta();
//            htmlMeta.HttpEquiv = "X-UA-Compatible";
//            htmlMeta.Content = "IE=EmulateIE8";
//            Page.Header.Controls.AddAt(0, htmlMeta);
//        }

//        if (!IsPostBack)
//        {
//            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
//            for (int idx = 0;idx<config.DatabaseConfigList .Count ;idx++)
//            {
//                DatabaseConfig dbConfig = config.DatabaseConfigList[idx];
//                cboDatabase.Items.Add(new ListItem(dbConfig.name, idx.ToString()));
//            }
//        }
//    }


//    protected void btnGo_Click(object sender, EventArgs e)
//    {
//        int idx = 0;
//        if (int.TryParse(cboDatabase.SelectedValue, out idx))
//        {
//            HROneConfig config = HROneConfig.GetCurrentHROneConfig();
//            DatabaseConfig dbconfig = config.DatabaseConfigList[idx];
//            if (dbconfig.TestConnection())
//            {
//                //WebUtils.Logout(Session);
//                Session.Clear();
//                WebUtils.SetSessionDatabaseConnection(Session, dbconfig.CreateDatabaseConnectionObject());
//                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Default.aspx");
//            }
//            else
//                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorMessage", "alert(\"Fail to connect database\");", true);

//        }
//    }
//}