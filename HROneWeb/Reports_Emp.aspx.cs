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
//using perspectivemind.validation;
using HROne.Lib.Entities;

public partial class Reports_Emp : HROneWebPage
{
    //private const string FUNCTION_CODE = "RPT001";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        if (!WebUtils.productLicense(Session).IsESS)
            ESSRequestPanel.Visible = false;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


    }
}
