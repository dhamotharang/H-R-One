using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Resources;
using System.Globalization;

namespace HROne.DataAccess
{
    /// <summary>
    /// Summary description for WebCustomControl1.
    /// </summary>
    [DefaultProperty("Text"),
        ToolboxData("<{0}:PageError runat=server></{0}:PageError>")]
    public class PageError : WebControl
    {
        private string text;
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private string name;
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string label;
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Label
        {
            get { return label == null ? name : label; }
            set { label = value; }
        }
        private bool showFieldErrors;
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public bool ShowFieldErrors
        {
            get { return showFieldErrors; }
            set { showFieldErrors = value; }
        }
        private bool showPrompt = true;
        [Bindable(true), Category("Appearance"), DefaultValue(true)]
        public bool ShowPrompt
        {
            get { return showPrompt; }
            set { showPrompt = value; }
        }
        private bool isPopup = true;
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }

        PageErrors errors;
        public PageErrors getErrors()
        {
            if (errors == null)
                errors = new PageErrors();
            return errors;
        }

        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
            PageErrors errors = PageErrors.getErrors(null, Page);
            if (errors != null)
            {
                if (name == null)
                {
                    if (!errors.isEmpty())
                    {

                        StringBuilder b = new StringBuilder();

                        ResourceManager rm = DBUtils.getResourceManager();

                        CultureInfo ci = CultureInfo.CurrentUICulture;
                        if (showPrompt)
                            b.Append(rm.GetString("validate.prompt", ci));

                        foreach (Error s in errors.globalErrors)
                            b.Append("\n" + s.getPrompt(errors.db, rm, ci));

                        if (showFieldErrors)
                        {
                            foreach (string key in errors.errors.Keys)
                            {
                                Error er = errors.getError(key);
                                b.Append("\n - " + er.getPrompt(errors.db, rm, ci));
                            }
                        }
                        writeError(output, b, Page, errors);

                        if (isPopup)
                        {
                            output.Write("<script defer>alert(\"");
                            output.Write(b.ToString().Replace("\n", "\\n"));
                            output.Write("\");</script>");
                        }
                        else
                        {
                            output.Write(HTMLUtils.toHTMLText(b.ToString()));
                        }
                    }
                }
                else
                {
                    if (errors.hasError(name))
                    {
                        ResourceManager rm = DBUtils.getResourceManager();
                        Error er = errors.getError(name);
                        output.Write("<span style=\"color:#ff0000\">");
                        output.Write(er.getFieldPrompt(errors.db, rm));
                        output.Write("</span>");
                    }
                }
            }
        }

        public void writeError(HtmlTextWriter output, StringBuilder b, Control ctrl, PageErrors errors)
        {
            foreach (Control c in ctrl.Controls)
            {

                if (c.GetType() == typeof(PageError))
                {
                    PageError pe = (PageError)c;
                    if (pe.Name != null)
                    {
                        if (errors.hasError(pe.Name))
                        {
                            ResourceManager rm = DBUtils.getResourceManager();
                            Error er = errors.getError(pe.Name);
                            b.Append("\n - " + er.getPrompt(errors.db, rm));
                        }
                    }
                }
                writeError(output, b, c, errors);
            }
        }


    }
}
