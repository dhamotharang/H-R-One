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

public partial class ReportExportControl : System.Web.UI.UserControl
{
    public event EventHandler Click;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnGenerate.Click += Click;
        this.btnGenerateExcel.Click += Click;
        this.btnGenerateWord.Click += Click;
        btnGenerate.CssClass = m_ButtonCssClass;
        btnGenerateExcel.CssClass = m_ButtonCssClass;
        btnGenerateWord.CssClass = m_ButtonCssClass;
    }

    protected string m_ButtonCssClass;
    public string ButtonCssClass
    {
        get { return m_ButtonCssClass; }
        set { m_ButtonCssClass = value; }
    }
    //protected virtual void OnClick(EventArgs e)
    //{
    //}
}
