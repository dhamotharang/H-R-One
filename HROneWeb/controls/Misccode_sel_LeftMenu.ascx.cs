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

public partial class Misccode_sel_LeftMenu : System.Web.UI.UserControl
{


    protected void Page_Load(object sender, EventArgs e)
    {
        HROne.ProductLicense productLicense=WebUtils.productLicense(Session);
        if (productLicense.IsCostCenter)
            this.CostCenterPanel.Visible = true;
        else
            this.CostCenterPanel.Visible = false;

        if (productLicense.IsTraining)
            this.TrainingPanel.Visible = true;
        else
            this.TrainingPanel.Visible = false;

        if (productLicense.ProductType == HROne.ProductLicense.ProductLicenseType.HROne)
        {
            DocumentTypePanel.Visible = true;
            PermitTypePanel.Visible = true;
        }
        else
        {
            DocumentTypePanel.Visible = false;
            PermitTypePanel.Visible = false;
        }

        BoldActiveLink(this);
    }

    protected void BoldActiveLink(Control ParentControl)
    {
        string currentURL = Request.Url.AbsolutePath;
        string currentPage = currentURL.Substring(currentURL.LastIndexOf("/") + 1);
        foreach (Control ctrl in ParentControl.Controls)
        {
            if (ctrl is HtmlAnchor)
            {
                HtmlAnchor a = (HtmlAnchor)ctrl;
                if (a.HRef.IndexOf(currentPage, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    a.Style.Add("font-weight", "bolder");
                    a.Style.Add("font-size", "13px");
                }
            }
            if (ctrl.HasControls())
                BoldActiveLink(ctrl);
        }
    }

}
