using System;
using System.Collections;
using System.Text;

namespace HROne.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : ValidationAttribute 
    {
        public override void validate(PageErrors errors, DBField field, Hashtable valueList)
        {
            string fieldName = field.name;
            bool isValidFail = false;
            if (valueList.ContainsKey(fieldName))
            {
                object value = valueList[fieldName];
                if (value == null)
                    isValidFail = true;
                else
                {
                    string stringValue = value.ToString().Trim();
                    if (stringValue.Length == 0 || stringValue.Equals(Common.NULL_STRING))
                        isValidFail = true;
                }
            }
            else
                isValidFail = false;

            if (isValidFail)
                errors.addError(fieldName, "validate.required");

        }
    }
}
