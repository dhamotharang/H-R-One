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

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense != null && productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.iMGR)
            if (!productLicense.IsLeaveManagement)
            {
                HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/eChannel_CompanyInbox_List.aspx");
            }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/Emp_List.aspx");
    }
}
