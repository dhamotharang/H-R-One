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
using System.Data.OleDb;
using HROne.SaaS.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;

public partial class eChannel_CompanyInbox_List : HROneWebPage
{
    private const string FUNCTION_CODE = "ECH000";
    protected DatabaseConnection masterDBConn;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;

        if (Application["MasterDBConfig"] != null)
            masterDBConn = ((DatabaseConfig)Application["MasterDBConfig"]).CreateDatabaseConnectionObject();
        else
            Response.Redirect("~/AccessDeny.aspx");


        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //view = loadData(info, db, Repeater);
        }

    }




}
