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

public partial class MiscCode : HROneWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtils.CheckAccess(Response, Session);
        if (WebUtils.CheckPermission(Session, "SYS010", WebUtils.AccessLevel.Read))
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/EmploymentType.aspx");

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }
}
