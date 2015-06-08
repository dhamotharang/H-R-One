using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Data;

namespace HROne.DataAccess
{

    public class WFSelectValue
    {
        public string key;
        public string name;
        public WFSelectValue(string key, string name)
        {
            this.key = key;
            this.name = name;
        }
        public override string ToString()
        {
            return name;
        }
    }

    public interface WFValueList
    {
        //List<WFSelectValue> getValues(DBFilter filter, CultureInfo ci);
        List<WFSelectValue> getValues(DatabaseConnection DBConn, DBFilter filter, CultureInfo ci);
    }


}
