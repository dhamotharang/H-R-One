using System;
using System.Collections;
using System.Web;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public abstract class Binder
    {
        protected internal Binding Binding;
        public abstract void toControl(Hashtable values);
        public abstract void toValues(Hashtable values);
        public abstract void init(HttpRequest Request, HttpSessionState Session);
    }
}
