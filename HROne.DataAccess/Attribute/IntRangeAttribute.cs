using System;
using System.Collections;
using System.Text;

namespace HROne.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IntRangeAttribute : ValidationAttribute
    {
        int min;
        int max;
        public IntRangeAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;

        }
        public override void validate(PageErrors errors, DBField field, Hashtable valueList)
        {
            object v = valueList[field.name];
            if (v != null)
            {

                string s = v.ToString().Trim();

                int vv = 0;
                if (Int32.TryParse(s, out vv))
                    if (vv > max || vv < min)
                        errors.addError(field.name, "validate.intrange");
            }
        }
    }
}
