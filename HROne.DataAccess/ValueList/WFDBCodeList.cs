using System;
using System.Collections.Generic;
using System.Data;


namespace HROne.DataAccess
{
    public class WFDBCodeList : WFDBList2
    {
        public WFDBCodeList(DBManager db, string id, string code, string desc, string sort)
            : base(db, new string[] { id, code, desc }, sort)
        {

        }
        public override string GetText(IDataReader reader)
        {
            String s = "";
            object o;
            o = reader[1];
            if (o != DBNull.Value)
                s += o.ToString();
            s += " - ";
            o = reader[2];
            if (o != DBNull.Value)
                s += o.ToString();
            return s;
        }
        public override string GetKey(IDataReader reader)
        {
            return reader[0].ToString();
        }

    }
}
