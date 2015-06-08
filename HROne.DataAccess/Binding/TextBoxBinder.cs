using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class TextBoxBinder : Binder
    {
        protected TextBox c;
        protected String name;
        protected DBField field;
        protected int maxlength = -1;
        protected int size = -1;
        public TextBoxBinder(DBManager db, TextBox c, string name)
        {
            this.c = c;
            this.name = name;
            this.field = db.getField(name);
            if (this.field != null)
            {
                MaxLengthAttribute m = (MaxLengthAttribute)this.field.findAttribute(typeof(MaxLengthAttribute));
                if (m != null)
                {
                    maxlength = m.getMaxLength();
                    size = m.getMaxTextBoxLength();
                }
            }

        }
        public TextBoxBinder(DBManager db, TextBox c)
            : this(db, c, c.ID)
        {
        }
        public override void init(HttpRequest Request, HttpSessionState Session)
        {
            if (maxlength > 0 && c.MaxLength == 0)
            {
                c.MaxLength = maxlength;
            }
            if (size > 0 && c.TextMode == TextBoxMode.SingleLine)
            {
                c.Columns = size;
            }
            if (field.required)
            {
                c.CssClass = "pm_required";
            }
        }

        public override void toControl(Hashtable values)
        {
            object o = values[name];
            if (o != null)
                c.Text = o.ToString();
            else
                c.Text = "";
        }
        public override void toValues(Hashtable values)
        {
            values.Add(name, c.Text);
        }

    }
}
