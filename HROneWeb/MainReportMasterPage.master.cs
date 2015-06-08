using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;

public partial class MainReportMasterPage : HROneWebMasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["LastURL"] = Request.Url.PathAndQuery;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string dbTitle = HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_DB_TITLE);
        if (!string.IsNullOrEmpty(dbTitle))
            Page.Title += " (" + dbTitle + ")";
        this.form1.Attributes.Add("AUTOCOMPLETE", "OFF");

        HROne.Common.WebUtility.AddBrowserCompatibilityMeta(Page);

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
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
            Page page = (Page)HttpContext.Current.Handler;
            if (page != null)
            {
                string message = pageError.getErrors().getPrompt();
                message = message.Replace("\r", "\\r").Replace("\n", "\\n");
                message = message.Replace(HROne.Common.WebUtility.GetLocalizedString("validate.prompt"), "");
                ScriptManager.RegisterStartupScript(page, page.GetType(), "errorMessage", "alert(\"" + message + "\");", true);
            }


        }
    }
}
