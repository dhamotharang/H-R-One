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
using HROne.SaaS.Entities;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Common;
public partial class GenerateKeyPair : HROneWebPage
{
    private const string FUNCTION_CODE = "ADM006";

    private DBManager db = ESystemParameter.db;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE))
            return;

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);


        if (!Page.IsPostBack)
        {
            HROne.CommonLib.RSACrypto rsa = new HROne.CommonLib.RSACrypto("HROneSaaS");
            //txtPublicKeyXMLString.Text = rsa.ToXmlString();
        }


    }
    protected void Save_Click(object sender, EventArgs e)
    {

        HROne.CommonLib.RSACrypto rsa = null;
        if (cbxEncryptedDataBy.SelectedValue.Equals("buildin", StringComparison.CurrentCultureIgnoreCase))
        {
            rsa = new HROne.CommonLib.RSACrypto("HROneSaaS");
        }
        //else
        //{
        //    rsa = new HROne.CommonLib.RSACrypto();
        //    rsa.FromXMLString(txtPublicKeyXMLString.Text);
        //}
        string encryptedString = rsa.Encrypting(txtPasspharse.Text);
        HROneSaaSConfig SaaSconfig = HROneSaaSConfig.GetCurrentConfig();
        HROneConfig config = new HROneConfig(SaaSconfig.HROneConfigFullPath);
        config.databaseEncryptKey = encryptedString;
        config.Save();

    }
}
