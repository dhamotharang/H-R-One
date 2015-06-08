using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    public interface DBTerm
    {
        int buildSQL(DatabaseConnection dbConn, int index, StringBuilder b);
        int setParam(int index, System.Data.IDbCommand c);
    }

    public interface DBTermCollection
    {
        void add(DBTerm term);
        IList<DBTerm> terms();

    }
}
