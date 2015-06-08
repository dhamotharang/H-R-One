using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace HROne.DataAccess
{
    public class WFHourList : WFValueList
    {
        int from = 0;
        int to = 23;
        public WFHourList(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
        public WFHourList()
            : this(0, 23)
        {
        }
        public List<WFSelectValue> getValues(DatabaseConnection DBConn, DBFilter filter, CultureInfo ci)
        {
            List<WFSelectValue> list = new List<WFSelectValue>();

            for (int i = from; i != to; i++)
            {
                list.Add(new WFSelectValue(i.ToString(), i.ToString("00") + ":00"));
            }
            return list;
        }
    }


}