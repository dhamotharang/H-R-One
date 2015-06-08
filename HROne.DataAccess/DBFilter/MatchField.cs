using System;
using System.Text;
using System.Data;

namespace HROne.DataAccess
{
    public class MatchField : DBTerm
    {
        protected string fieldName1;
        protected string fieldName2;
        protected string op = "=";
        public MatchField(String fieldName1, string fieldName2)
            : this(fieldName1, "=", fieldName2)
        {
        }
        public MatchField(String fieldName1, String op, string fieldName2)
        {
            this.fieldName1 = fieldName1;
            this.op = op;
            this.fieldName2 = fieldName2;
        }

        #region DBTerm Members

        public int buildSQL(DatabaseConnection dbConn, int index, StringBuilder b)
        {

            b.Append(fieldName1).Append(op).Append(fieldName2);
            return index;
        }

        public int setParam(int index, IDbCommand c)
        {
            return index;
        }

        #endregion
    }
}
