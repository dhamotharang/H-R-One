using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace HROne.DataAccess
{
    public class OR : DBTerm, DBTermCollection
    {
        List<DBTerm> dbTermsList;
        string op = " OR ";
        public OR()
        {
            dbTermsList = new List<DBTerm>();
        }
        #region DBTerm Members

        public int buildSQL(DatabaseConnection dbConn, int index, StringBuilder b)
        {
            if (dbTermsList.Count > 0)
            {
                b.Append("(");
                bool first = true;
                foreach (DBTerm t in dbTermsList)
                {
                    if (first)
                        first = false;
                    else
                        b.Append(op);
                    index = t.buildSQL(dbConn, index, b);
                }
                b.Append(")");
            }
            return index;
        }

        public int setParam(int index, IDbCommand c)
        {
            foreach (DBTerm t in dbTermsList)
            {
                index = t.setParam(index, c);
            }
            return index;
        }

        #endregion

        #region DBTermCollection Members

        public void add(DBTerm term)
        {
            dbTermsList.Add(term);
        }

        public IList<DBTerm> terms()
        {
            return dbTermsList;
        }

        #endregion

    }
}
