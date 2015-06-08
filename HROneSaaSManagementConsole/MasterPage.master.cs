using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;

public partial class MasterPage : System.Web.UI.MasterPage
{
    HROne.SaaS.Entities.EUser user = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        user = WebUtils.GetCurUser(Session);

        if (user != null)
        {
            if (!string.IsNullOrEmpty(user.UserName))
                UserName.Text = "[" + user.UserName + "]";
        }

        if (user == null)
            Response.Redirect("~/Login.aspx");

        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");

        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        Page page = this.Page;
        if (page != null)
        {

            //// Remove all white spacing from the DOM layout of the dynamic table
            //ScriptManager.RegisterStartupScript(page, page.GetType(), "ie9TableFix",
            //    @"var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');" + "\r\n" + @"document.body.innerHTML = document.body.innerHTML.replace(expr, '><');"
            //, true);

            if (!this.pageError.getErrors().isEmpty())
            {
                Dictionary<string, Error> errorTable = this.pageError.getErrors().errors;

                foreach (string key in errorTable.Keys)
                {
                    object newValue = errorTable[key];
                    if (newValue is Error)
                    {
                        ((Error)newValue).name = HROne.Common.WebUtility.GetLocalizedString(((Error)newValue).name);
                        //object[] parameters = ((Error)newValue).parameters;
                        //for (int i = 0; i < parameters.GetLength(0); i++)
                        //{
                        //    if (parameters[i] is string)
                        //        parameters[i] = HROne.Common.WebUtility.GetLocalizedString(((string)parameters[i]));
                        //}
                    }

                }
                string errorMessage = pageError.getErrors().getPrompt();
                errorMessage = errorMessage.Replace(HROne.Common.WebUtility.GetLocalizedString("validate.prompt"), "");
                errorMessage = errorMessage.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
                //ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage", "alert(\"" + errorMessage + "\");", true);

                string[] errorMessageLineArray = errorMessage.Split(new string[] { "\\r\\n" }, StringSplitOptions.None);
                if (errorMessageLineArray.Length > 20)
                {
                    errorMessage = string.Empty;
                    for (int idx = errorMessageLineArray.GetLowerBound(0); idx < errorMessageLineArray.GetLowerBound(0) + 20; idx++)
                        if (string.IsNullOrEmpty(errorMessage))
                            errorMessage = errorMessageLineArray[idx];
                        else
                            errorMessage += "\\r\\n" + errorMessageLineArray[idx];
                    errorMessage += "\\r\\n" + "... and more";
                }

                String scriptString = "messagePopupDetail=\"" + errorMessage + "\";"; 
                ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage", scriptString, true);
            }
        }
    }
}
