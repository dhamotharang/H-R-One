using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Globalization;

namespace HROne.DataAccess
{
    public class HiddenMatchBinder : SearchBinder
    {
        string name;
        HtmlInputHidden c;
        string op = "=";
        string field;
        bool includeEmpty;
        public HiddenMatchBinder(HtmlInputHidden c, string name, string op)
        {
            this.op = op;
            this.name = name;
            this.field = c.ID;
            this.c = c;
        }
        public HiddenMatchBinder(HtmlInputHidden c, string name)
            : this(c, name, "=")
        {
        }
        public HiddenMatchBinder(HtmlInputHidden c)
            : this(c, c.ID, "=")
        {
        }
        public HiddenMatchBinder setName(string field)
        {
            this.field = field;
            return this;
        }
        public HiddenMatchBinder setIncludeEmpty(bool includeEmpty)
        {
            this.includeEmpty = includeEmpty;
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
            string v = c.Value;
            v = v.Trim();
            if (v.Equals(""))
            {
                OR or = new OR();
                or.add(new Match(name, op, ""));
                or.add(new Match(name, op, null));
                filter.add(or);
            }
            else
            {
                filter.add(new Match(name, op, v));
            }
            return true;

        }
    }

}
