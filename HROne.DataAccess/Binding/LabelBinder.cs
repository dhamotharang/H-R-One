using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class LabelBinder : Binder
    {
        public Label c;
        public String name;
        public DBField field;
        public string format;
        public LabelBinder(DBManager db, Label c, string name)
        {
            this.c = c;
            this.name = name;
            this.field = db.getField(name);
        }
        public LabelBinder setFormat(string format)
        {
            this.format = format;
            return this;
        }
        public override void init(HttpRequest Request, HttpSessionState Session)
        {
        }
        public override void toControl(Hashtable values)
        {
            object o = values[name];
            if (o != null)
            {
                string v;
                if (field.property.PropertyType == typeof(bool))
                {
                    bool b = o.Equals("True");
                    v = DBUtils.getResourceManager().GetString(b ? "value.true" : "value.false");
                    if (v == null)
                        v = b.ToString();
                }
                else
                {
                    if (format != null)
                    {
                        if (field.property.PropertyType == typeof(Int32))
                        {
                            if (o.Equals(""))
                                v = "";
                            else
                            {

                                int t = Convert.ToInt32(o);
                                v = t.ToString(format);
                            }
                        }
                        else if (field.property.PropertyType == typeof(double))
                        {
                            if (o.Equals(""))
                                v = "";
                            else
                            {
                                double t = (double)field.convert(o);
                                v = t.ToString(format);
                            }
                        }
                        else
                        {
                            v = o.ToString();
                        }

                    }
                    else
                    {
                        v = o.ToString();
                    }
                }
                c.Text = HTMLUtils.toHTMLText(v);
            }
            else
                c.Text = "";
        }
        public override void toValues(Hashtable values)
        {
            if (!values.ContainsKey(name))
                values.Add(name, c.Text);
        }

    }
}
