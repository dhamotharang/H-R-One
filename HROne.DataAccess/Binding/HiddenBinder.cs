using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class HiddenBinder : Binder
    {
        HtmlInputHidden c;
        String name;
        DBField field;
        public HiddenBinder(DBManager db, HtmlInputHidden c)
        {
            this.c = c;
            this.name = c.ID;
            this.field = db.getField(name);

        }
        public HiddenBinder(DBManager db, HtmlInputHidden c, string name)
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
                //				if(field.vl!=null)
                //					o=field.vl.load(o.ToString());
                c.Value = HTMLUtils.toHTMLText(o.ToString());
            }
            else
                c.Value = "";
        }
        public override void toValues(Hashtable values)
        {
            values.Add(name, c.Value);
        }

    }
}
