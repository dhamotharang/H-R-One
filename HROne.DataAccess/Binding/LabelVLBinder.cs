using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class LabelVLBinder : Binder
    {
        Label c;
        String name;
        DBField field;
        WFValueList vl;
        DBFilter filter = null;
        bool showInvalid = true;

        public LabelVLBinder(DBManager db, Label c, WFValueList vl)
            : this(db, c, c.ID, vl, null)
        {

        }
        public LabelVLBinder(DBManager db, Label c, string name, WFValueList vl)
            : this(db, c, name, vl, null)
        {
        }
        public LabelVLBinder(DBManager db, Label c, WFValueList vl, DBFilter filter)
            : this(db, c, c.ID, vl, filter)
        {
        }
        public LabelVLBinder(DBManager db, Label c, string name, WFValueList vl, DBFilter filter)
        {
            this.c = c;
            this.name = name;
            this.field = db.getField(name);
            this.vl = vl;

        }
        public LabelVLBinder setShowInvalid(bool showInvalid)
        {
            this.showInvalid = showInvalid;
            return this;
        }
        public override void toControl(Hashtable values)
        {
            object o = values[name];
            if (o != null)
            {
                bool found = false;
                List<WFSelectValue> list = vl.getValues(Binding.DBConn, filter, null);
                foreach (WFSelectValue sv in list)
                {
                    if (sv.key.Equals(o))
                    {
                        o = sv.name;
                        found = true;
                        break;
                    }
                }
                if (found || showInvalid)
                    c.Text = HTMLUtils.toHTMLText(o.ToString());
                else
                    c.Text = "";
            }
            else
                c.Text = "";
        }
        public override void toValues(Hashtable values)
        {

        }


        public override void init(HttpRequest Request, HttpSessionState Session)
        {

        }
    }
}
