using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    public class WFYearList : WFValueList
    {
        int prev;
        int next;
        public WFYearList(int prev, int next)
        {
            this.prev = prev;
            this.next = next;
        }
        public WFYearList()
            : this(10, 10)
        {
        }
        public List<WFSelectValue> getValues(DatabaseConnection DBConn, DBFilter filter, CultureInfo ci)
        {
            List<WFSelectValue> list = new List<WFSelectValue>();

            DateTime now = DateTime.Now;
            int year = now.Year - prev;

            for (int i = 0; i != prev + next + 1; i++)
            {
                list.Add(new WFSelectValue(year.ToString(), year.ToString()));
                year++;
            }
            return list;
        }
    }
}
