using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.Lib
{
    public abstract class Reflection
    {
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }



    }
}
