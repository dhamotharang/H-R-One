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

public partial class Reports_Payroll_MPF : HROneWebPage
{
    //private const string FUNCTION_CODE = "RPT002";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session))
            return;
        if (!ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_USE_ORSO).Equals("Y"))
            ReportPFundPanel.Visible = false;

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        HROne.ProductLicense productLicense = WebUtils.productLicense(Session);

        if (productLicense.ProductType != HROne.ProductLicense.ProductLicenseType.HROne)
        {
            //  Use FindControl to prevent compatibility with customized menu
            PlaceHolder section = (PlaceHolder)Master.FindControl("mainContentPlaceHolder").FindControl("HROneOnlyReportSection");
            if (section != null)
                section.Visible = false;
        }
    }

    protected void Page_Prerender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (Control c in Page.Controls)
            {
                HideCustomControl(c);
            }
        }
    }

    protected void HideCustomControl(Control c)
    {
        if (c != null)
        {
            if (c.Controls.Count > 0 )
            {
                foreach (Control childc in c.Controls)
                {
                    HideCustomControl(childc);
                }
            }
            
            if (c.ID != null)
            {
                if (c.ID.PadRight(6).Substring(0, 6).Equals("CUSTOM", StringComparison.CurrentCultureIgnoreCase))
                {
                    ESystemFunction m_function = ESystemFunction.GetObjectByCode(dbConn, c.ID.ToUpper());
                    if (m_function == null || m_function.FunctionIsHidden == true)
                    {
                        c.Visible = false;
                    }
                }
            }
        }
    }
}
