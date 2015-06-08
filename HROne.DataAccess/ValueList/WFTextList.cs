using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace HROne.DataAccess
{
    public class WFTextList : WFValueList
    {

        string[] values;
        string[] display;
        public WFTextList(string[] values, string[] display)
        {
            this.values = values;
            this.display = display;
        }
        public WFTextList(string[] values)
        {
            this.values = values;
            this.display = values;
        }
        public List<WFSelectValue> getValues(DatabaseConnection DBConn, DBFilter filter, CultureInfo ci)
        {
            List<WFSelectValue> list = new List<WFSelectValue>();
            ResourceManager rm = DBUtils.getResourceManager();
            for (int i = 0; i != values.Length; i++)
            {
                string s = rm.GetString(display[i], ci);
                if (s == null)
                    s = display[i];
                WFSelectValue sv = new WFSelectValue(values[i], s);
                list.Add(sv);
            }
            return list;
        }

    }
}
