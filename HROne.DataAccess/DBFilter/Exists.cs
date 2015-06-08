using System;
using System.Text;
using System.Data;

namespace HROne.DataAccess
{
    public class Exists : DBTerm
    {
        DBFilter filter;
        string tableName;
        bool notExist = false;
        string selectFieldList = "*";
        public Exists(string tableName, DBFilter filter)
            :this("*",tableName,filter,false)
        {
        }
        public Exists(string selectFieldList, string tableName, DBFilter filter)
            : this(selectFieldList, tableName, filter, false)
        {
        }
        public Exists(string tableName, DBFilter filter, bool notExist)
            : this("*", tableName, filter, notExist)
        {
        }
        public Exists(string selectFieldList, string tableName, DBFilter filter, bool notExist)
        {
            this.filter = filter;
            this.selectFieldList = selectFieldList;
            this.tableName = tableName;
            this.notExist = notExist;
        }

        public int buildSQL(DatabaseConnection dbConn, int index, StringBuilder b)
        {
            if (notExist)
                b.Append(" NOT");
            b.Append(" EXISTS (SELECT ").Append(selectFieldList).Append(" FROM ").Append(tableName);
            int out_index = 0;
            if (filter != null)
            {
                b.Append(filter.getWhereClause(dbConn, true, index, out out_index));
                b.Append(filter.getGroupCluase());
            }
            b.Append(")");

            return out_index;
        }
        public int setParam(int index, IDbCommand c)
        {
            if (filter != null)
                return filter.setParams(index, c);
            else
                return index;
        }

    }
}
