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

public partial class Emp_LeftMenu : HROneWebControl
{

    //public Binding binding;
    //public DBManager db = EEmpPersonalInfo.db;
    //public EEmpPersonalInfo obj;
    public int CurID = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (WebUtils.productLicense(Session).IsCostCenter)
            this.CostCenterPanel.Visible = true;
        else
            this.CostCenterPanel.Visible = false;

        if (WebUtils.productLicense(Session).IsTraining)
            this.TrainingPanel.Visible = true;
        else
            this.TrainingPanel.Visible = false;

        if (WebUtils.productLicense(Session).IsAttendance)
            this.RosterTableGroupPanel.Visible = true;
        else
            this.RosterTableGroupPanel.Visible = false;

        if (WebUtils.productLicense(Session).ProductType == HROne.ProductLicense.ProductLicenseType.HROne)
        {
            this.DocumentPanel.Visible = true;
            this.PermitPanel.Visible = true;
            this.ContractPanel.Visible = true;
            this.CompensationLeavePanel.Visible = true;
        }
        else
        {
            this.DocumentPanel.Visible = false;
            this.PermitPanel.Visible = false;
            this.ContractPanel.Visible = false;
            this.CompensationLeavePanel.Visible = false;
        }

        if (!int.TryParse(DecryptedRequest["EmpID"], out CurID))
            CurID = -1;

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
                a.HRef = HROne.Common.WebUtility.URLwithEncryptQueryString(Session, a.HRef.Replace("%EmpID%", CurID.ToString()));
            }
            if (ctrl.HasControls())
                BoldActiveLink(ctrl);
        }
    }
}
