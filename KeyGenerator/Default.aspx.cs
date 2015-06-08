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
using HROne.CommonLib;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);
        string trialKey = string.Empty;
        trialKey = crypto.Encrypting(System.DateTime.Now.ToString("yyyyMMdd"), "HROne");

        keyText.Text = HROne.License.Base32.ConvertBase64ToBase32(trialKey);
    }
}
