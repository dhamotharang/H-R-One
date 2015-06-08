using System;
using System.Text;
using System.Data;

namespace HROne.DataAccess
{
    public class IN : DBTerm
    {
		DBFilter filter;
        string selectQueryOrItemList;
        string searchFieldName;

		public IN(string searchFieldName, string selectQueryOrItemList,DBFilter filter) 
		{
			this.filter=filter;
            this.selectQueryOrItemList = selectQueryOrItemList;
            this.searchFieldName = searchFieldName;
		}

        public IN(string searchFieldName, string[] ItemArray)
        {
            this.filter = null;
            this.selectQueryOrItemList = "'" + string.Join("','", ItemArray) + "'";
            this.searchFieldName = searchFieldName;
        }

        public IN(string searchFieldName, int[] ItemArray)
        {
            this.filter = null;
            this.selectQueryOrItemList= string.Empty;
            foreach (int i in ItemArray)
                if (string.IsNullOrEmpty(selectQueryOrItemList))
                    selectQueryOrItemList = i.ToString();
                else
                    // Start 0000111, Miranda, 2015-01-03
                    selectQueryOrItemList += ("," + i.ToString());
                    // End 0000111, Miranda, 2015-01-03
            this.searchFieldName = searchFieldName;
        }
        public int buildSQL(DatabaseConnection dbConn, int index, StringBuilder b)
        {
            if (!string.IsNullOrEmpty(searchFieldName) && !string.IsNullOrEmpty(selectQueryOrItemList))
            {
                b.Append(searchFieldName).Append(" IN (").Append(selectQueryOrItemList);
                if (filter != null)
                    b.Append(" ").Append(filter.getWhereClause(dbConn, true, index, out index));
                b.Append(")");
            }
            return index;
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
