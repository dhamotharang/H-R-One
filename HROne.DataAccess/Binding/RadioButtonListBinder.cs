using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class RadioButtonListBinder : Binder
    {
        RadioButtonList c;
        String name;
        DBField field;

        public RadioButtonListBinder(DBManager db, RadioButtonList c, string name)
        {
            this.c = c;
            this.name = name;
            this.field = db.getField(name);

        }
        public override void init(HttpRequest Request, HttpSessionState Session)
        {
        }
        public override void toControl(Hashtable values)
        {
            object o = values[name];
            if (o != null)
            {
                string selected = o.ToString();
                selected = selected.Trim();
                foreach (ListItem item in c.Items)
                {
                    if (selected != null && selected.Equals(item.Value))
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }

        }
        public override void toValues(Hashtable values)
        {
            string s = c.SelectedValue;
            values.Add(name, s);
        }
    }
}
