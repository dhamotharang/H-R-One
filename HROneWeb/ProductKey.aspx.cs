using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using HROne.Lib.Entities;
using HROne.DataAccess;
using HROne;

public partial class ProductKey : HROneWebPage 
{
    public bool login = false;
    public ProductLicense currentProductKey;

    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (WebUtils.productLicense(Session) == null)
        {
            currentProductKey = new ProductLicense();
            currentProductKey.LoadProductLicense(dbConn);
        }
        else
            currentProductKey = WebUtils.productLicense(Session);

        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                PreviousURL.Value = "Default.aspx";
//                PreviousURL.Value = Request.UrlReferrer.AbsoluteUri;
            }
            else
                PreviousURL.Value = "Default.aspx";
            txtProductKey.Text = currentProductKey.ProductKey;//ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_PRODUCTKEY);
            txtRequestCode.Text = currentProductKey.getRequestCode();
        }
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!currentProductKey.IsValidAuthorizationCode())
        {
            if (currentProductKey.LastTrialDate < AppUtils.ServerDateTime().Date)
            {
                Prompt.Text = "Trial Period Expiry.<br/>";
                Cancel.Visible = false;
            }
            else
            {
                Prompt.Text = string.Format(HROne.Translation.PageMessage.PRODUCT_TRIAL_WILL_EXPIRE, ((TimeSpan)currentProductKey.LastTrialDate.Subtract(AppUtils.ServerDateTime().Date)).Days) + "<br/>";
            }
        }

        string dbTitle = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_DB_TITLE);
        if (!string.IsNullOrEmpty(dbTitle))
            Page.Title += " (" + dbTitle + ")";

        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");

        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);

    }
    protected void OK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtProductKey.Text) && string.IsNullOrEmpty(txtTrialKey.Text))
        {
            return;
        }
        HROne.ProductLicense productKey = new HROne.ProductLicense();
        productKey.LoadProductLicense(dbConn);
        bool IsRedirect = true;
        if (!string.IsNullOrEmpty(txtProductKey.Text))
        {
            productKey.LoadProductKey(txtProductKey.Text);
            if (productKey.IsValidProductKey)
            {
                //if (!txtProductKey.Text.Equals(ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_PRODUCTKEY), StringComparison.CurrentCultureIgnoreCase))
                //{
                //}
                //else
                //{
                //    if (AppUtils.ServerDateTime().Date > WebUtils.getLastTrialDate())
                //    {
                //        Prompt.Text = "Trial Period Expiry";
                //        return;
                //    }

                //}
                Session["TrialVersion"] = false;
            }
            else
            {
                Prompt.Text += "Invalid Product Key.<br/>";
                IsRedirect = false;
            }
        }
        if (!string.IsNullOrEmpty(txtTrialKey.Text))
        {
            string trialKey = txtTrialKey.Text;
            productKey.LoadTrialKey(trialKey, dbConn);
            if (productKey.TrialKey==string.Empty )
            {
                Prompt.Text += "Invalid Trial Key.<br/>";
                IsRedirect = false;
            }
            else
            {
                if (Session["TrialVersion"] ==null)
                    Session["TrialVersion"] = true;

            }
        }
        if (!string.IsNullOrEmpty(txtAuthorizationCode.Text))
        {
            if (!productKey.IsValidAuthorizationCode(txtAuthorizationCode.Text))
            {
                Prompt.Text += "Invalid Authorization Code.<br/>";
                IsRedirect = false;
            }
        }
        if (IsRedirect)
        {
            productKey.SaveProductLicense(dbConn);

            //  reset the connection for new key
            WebUtils.SetSessionDatabaseConnection(Session, dbConn);
            Response.Redirect(PreviousURL.Value);
        }
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Session["TrialVersion"] = true;
        //WebUtils.SetNewTrialKey();

        Response.Redirect(PreviousURL.Value);

    }


}
