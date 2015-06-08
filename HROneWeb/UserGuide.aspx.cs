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

public partial class UserGuide : HROneWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense != null )
            if (productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.iMGR)
        {
            if (ci.Name.Equals("zh-cht", StringComparison.CurrentCultureIgnoreCase))
                Response.Redirect("http://www.peopletech.hk/userguide/to_be_provided_Userguide(chi).pdf");
            else
                Response.Redirect("http://www.peopletech.hk/userguide/to_be_provided_Userguide.pdf");
        }
        else
        {
            Response.Redirect("http://www.peopletech.hk/userguide/HROne_Userguide_Standard.pdf");
        }
    }
}
