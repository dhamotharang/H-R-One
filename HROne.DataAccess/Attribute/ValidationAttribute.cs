using System;
using System.Collections;
using System.Text;

namespace HROne.DataAccess
{
    public abstract class ValidationAttribute : Attribute
    {
        public abstract void validate(PageErrors errors, DBField field, Hashtable value);
    }
}
