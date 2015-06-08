using System;
using System.Collections;
using System.Text;

namespace HROne.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IntAttribute : ValidationAttribute
    {
        public IntAttribute()
        {

        }
        public override void validate(PageErrors errors, DBField field, Hashtable values)
        {
            validate(errors, field.name, values[field.name]);

        }
        public static void validate(PageErrors errors, string field, object v)
        {
            if (v == null || v.Equals(""))
                return;
            if (v.GetType() != typeof(string))
                return;

            string s = (string)v;
            s = s.Trim();

            Int64 val = 0;
            if (Int64.TryParse(s, out val))
            {
                if (val > Int32.MaxValue)
                    errors.addError(field, "validate.toobig");
                if (val < Int32.MinValue)
                    errors.addError(field, "validate.toosmall");

            }
            else
            {
                errors.addError(field, "validate.int");
            }
        }
    }
}
