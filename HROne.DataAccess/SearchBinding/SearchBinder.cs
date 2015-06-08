using System;
using System.Collections;
using System.Text;
using System.Web;

namespace HROne.DataAccess
{
    public abstract class SearchBinder
    {
        public virtual void init(System.Collections.Specialized.NameValueCollection Request, SearchContext context, SearchBinding binding, Hashtable values, DBManager db, System.EventHandler search)
        {
        }
        public virtual void buildFilterFilter(DBFilter filter)
        {
            buildFilter(filter);
        }
        public abstract bool buildFilter(DBFilter filter);
        public virtual void clear()
        {
        }
    }
}
