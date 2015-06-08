using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    /* will be changed to IDBFieldTranscoder */
    public interface DBFieldTranscoder
    {
        object fromDB(object value);
        object toDB(object value);
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class DBFieldTranscoderAttribute : Attribute, DBFieldTranscoder
    {
        public virtual object fromDB(object value)
        {
            return value;
        }
        public virtual object toDB(object value)
        {
            return value;
        }
    }

}
