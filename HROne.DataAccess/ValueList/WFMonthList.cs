using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    public class WFMonthList : WFValueList
    {
        public WFMonthList()
        {
        }
        public List<WFSelectValue> getValues(DatabaseConnection DBConn, DBFilter filter, CultureInfo ci)
        {
            List<WFSelectValue> list = new List<WFSelectValue>();

            for (int i = 1; i <= 12; i++)
                list.Add(new WFSelectValue(i.ToString(), i.ToString()));

            return list;
        }
    }
}
