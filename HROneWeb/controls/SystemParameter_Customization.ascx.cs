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

public partial class SystemParameter_Customization : HROneWebControl
{
    protected string m_ErrorMessage = string.Empty;

    public string ErrorMessage
    {
        get { return m_ErrorMessage; }
    }

    public virtual bool IsValidEntry()
    {
        return true;
    }

    public virtual bool Save()
    {
        return true;
    }
}
