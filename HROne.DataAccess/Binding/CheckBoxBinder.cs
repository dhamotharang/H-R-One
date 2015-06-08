using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class CheckBoxBinder : Binder
    {
        CheckBox c;
        String name;
        DBField field;

        public CheckBoxBinder(DBManager db, CheckBox c, string name)
        {
            this.c = c;
            this.name = name;
            this.field = db.getField(name);
        }
        public CheckBoxBinder(DBManager db, CheckBox c)
        {
            this.c = c;
            this.name = c.ID;
            this.field = db.getField(name);
        }
        public override void init(HttpRequest Request, HttpSessionState Session)
        {
        }
        public override void toControl(Hashtable values)
        {
            object o = values[name];
            if (true.Equals(o) || "true".Equals(o) || "True".Equals(o) || "1".Equals(o) || "-1".Equals(o))
                c.Checked = true;
            else
                c.Checked = false;
        }
        public override void toValues(Hashtable values)
        {
            values.Add(name, c.Checked ? -1 : 0);
        }

    }
}
