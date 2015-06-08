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

public partial class AdvancedCheckBoxList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(Page, Page.GetType(), "popUpListHandler", ResolveClientUrl("~/javascript/popUpListHandler.js"));
        ScriptManager.RegisterClientScriptInclude(Page, Page.GetType(), "advancedCheckBoxListHandler", ResolveClientUrl("~/javascript/advancedCheckBoxListHandler.js"));
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.Visible)
        {

            btnMore.Attributes.Add("onclick", "popUpListHandler(document.getElementById('" + checkBoxListDiv.ClientID + "'),document.getElementById('" + btnClose.ClientID + "'),this,true); return false;");
            ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, "advancedCheckBoxListHandler('" + CheckBoxListObject.ClientID + "', '" + lblOptionList.ClientID + "', document.getElementById('" + btnSelectAll.ClientID + "'), document.getElementById('" + btnClearAll.ClientID + "'),'');\r\n", true);
        }
    }

    public void LoadListControl(DatabaseConnection dbConn, WFValueList ValueList, bool hasNotSelected)
    {
        WebFormUtils.loadValues(dbConn, CheckBoxListObject, ValueList, new DBFilter(), System.Threading.Thread.CurrentThread.CurrentUICulture, null, (string)"combobox.notselected");
        if (!hasNotSelected)
            if (CheckBoxListObject.Items[0].Value.Equals(string.Empty))
                CheckBoxListObject.Items.RemoveAt(0);

    }

    public void Reset()
    {
        foreach (ListItem listItem in CheckBoxListObject.Items)
            listItem.Selected = false;
    }

    public ListControl ListControl
    {
        get
        {
            return CheckBoxListObject;
        }

    }

}
