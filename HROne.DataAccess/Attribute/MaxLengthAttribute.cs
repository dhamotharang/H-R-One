using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaxLengthAttribute : ValidationAttribute 
    {
        int maxLength;
        int textboxSize;
        /** 
         * maxLength - Maximum length of the field, 0 = unlimited
         **/
        public MaxLengthAttribute(int maxLength)
            : this(maxLength, maxLength)
        {
        }

        /** 
         * maxLength - Maximum length of the field, 0 = unlimited
         * textboxSize - Size of Textbox
         **/
        public MaxLengthAttribute(int maxLength, int textboxSize)
        {
            this.maxLength = maxLength;
            this.textboxSize = textboxSize;
        }

        public int getMaxLength()
        {
            return maxLength;
        }

        public int getMaxTextBoxLength()
        {
            return textboxSize;
        }

        public override void validate(PageErrors errors, DBField field, System.Collections.Hashtable valueList)
        {
            if (maxLength > 0)
            {
                object v = valueList[field.name];
                if (v != null)
                {
                    if (v is string)
                    {
                        string s = (string)v;
                        if (s.Length > maxLength)
                        {
                            errors.addError(field.name, "validate.maxlength", maxLength);
                        }
                    }
                }
            }
        }
    }
}
