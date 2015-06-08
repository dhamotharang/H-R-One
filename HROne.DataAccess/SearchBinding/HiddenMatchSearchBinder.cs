using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Globalization;

namespace HROne.DataAccess
{
    public class HiddenMatchSearchBinder : SearchBinder
    {
        string name;
        HtmlInputHidden c;
        string op = "=";
        string field;
        public HiddenMatchSearchBinder(HtmlInputHidden c, string name, string op)
        {
            this.op = op;
            this.name = name;
            this.field = c.ID;
            this.c = c;
        }
        public HiddenMatchSearchBinder(HtmlInputHidden c, string name)
            : this(c, name, "=")
        {
        }
        public HiddenMatchSearchBinder(HtmlInputHidden c)
            : this(c, c.ID, "=")
        {
        }
        public HiddenMatchSearchBinder setName(string field)
        {
            this.field = field;
            return this;
        }
        public override void init(System.Collections.Specialized.NameValueCollection Request, SearchContext context, SearchBinding binding, Hashtable values, DBManager db, System.EventHandler search)
        {
            string v = Request[field];
            if (v != null)
                c.Value = v;
        }

        public override bool buildFilter(DBFilter filter)
        {
            if (c.Value.Length > 0)
            {
                string v = c.Value;
                if (v.Equals(Common.EMPTY_STRING))
                    v = "";
                filter.add(new Match(name, op, v.Trim()));
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
