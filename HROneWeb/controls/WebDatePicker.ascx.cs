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

public partial class WebDatePicker : System.Web.UI.UserControl
{
    public event EventHandler Changed;

    protected string m_DateFormatString = "yyyy-mm-dd";
    public string DateFormatString
    {
        get { return m_DateFormatString; }
        set { m_DateFormatString = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "calendar", ResolveClientUrl("~/calendar/popcalendar.js"));
        if (Changed != null)
            DateTextBox.TextChanged += Changed;

        //DateTextBox.Attributes.Add("onchange", "if (!IsValidDateFormat(" + DateTextBox.ClientID + ".value, 'yyyy-MM-dd')) { alert('" + HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT + "'); return false;}");
        //DateTextBox.Attributes.Add("onsubmit", "if (!IsValidDateFormat(" + DateTextBox.ClientID + ".value, 'yyyy-MM-dd')) { alert('" + HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT + "'); return false;}");
        //DateTextBox.Attributes.Add("onblur", "if (!IsValidDateFormat(" + DateTextBox.ClientID + ".value, 'yyyy-MM-dd')) { void(0);" + DateTextBox.ClientID + ".focus(); alert('" + HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT + "'); return false;}");
        DateTextBox.Attributes.Add("onchange", "if (!IsValidDateFormat(this, 'yyyy-MM-dd')) { alert('" + HROne.Translation.PageErrorMessage.ERROR_DATE_FORMAT + "'); this.value=''; this.focus(); return false;}");
        DateTextBox.Attributes.Add("onkeypress", "if (event.keyCode==13) if (!IsValidDateFormat(this, 'yyyy-MM-dd')) { event.keyCode=0; return false;}");
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        DateFormatLabel.Text = "(" + DateFormatString + ")";
        DateTextBox.MaxLength = DateFormatString.Length;
        DateTextBox.Columns = DateFormatString.Length;

        if (this.Enabled)
            PopUpSection.Visible = true;
        else
            PopUpSection.Visible = false;
    }

    public TextBox TextBox
    {
        get { return DateTextBox; }
    }

    public string Value
    {
        get { return DateTextBox.Text; }
        set { DateTextBox.Text = value; }
    }

    public string CssClass
    {
        get { return DateTextBox.CssClass; }
        set { DateTextBox.CssClass = value; }
    }

    public bool ShowDateFormatLabel
    {
        get { return DateFormatLabel.Visible; }
        set { DateFormatLabel.Visible = value; }
    }

    public bool Enabled
    {
        get { return DateTextBox.Enabled; }
        set { DateTextBox.Enabled = value; }
    }

    public bool AutoPostBack
    {
        get { return DateTextBox.AutoPostBack; }
        set { DateTextBox.AutoPostBack = value; }
    }
}
