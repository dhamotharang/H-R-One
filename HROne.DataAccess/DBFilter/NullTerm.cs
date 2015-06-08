using System;
using System.Text;
using System.Data;

namespace HROne.DataAccess
{
    public class NullTerm : DBTerm
    {
        string fieldName;
        public NullTerm(string fieldName)
        {
            this.fieldName = fieldName;
        }
        public int buildSQL(DatabaseConnection dbConn, int index, StringBuilder b)
        {
            b.Append(fieldName).Append(" IS NULL");
            return index;
        }
        public int setParam(int index, IDbCommand c)
        {
            return index;
        }

    }
}
