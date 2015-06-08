using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace HROne.DataAccess
{
    public class DBFilter : DBTermCollection
    {
        public bool aborted;
        protected List<string> groupByList;
        protected Dictionary<string, bool> orderByList;
        protected List<DBTerm> dbTermsList;
        //private IDbCommand cmd;

        public DBFilter()
        {
            dbTermsList = new List<DBTerm>();
            groupByList = new List<string>();
            orderByList = new Dictionary<string, bool>();
        }

        public DBFilter(DBFilter f)
        {
            dbTermsList = new List<DBTerm>(f.dbTermsList);
            groupByList = new List<string>(f.groupByList);
            orderByList = new Dictionary<string, bool>(f.orderByList);
        }

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

        public void add(string orderby, bool ascending)
        {
            if (!string.IsNullOrEmpty(orderby))
            {
                orderby = orderby.Trim();
                if (orderby.EndsWith(" DESC", StringComparison.CurrentCultureIgnoreCase))
                    add(orderby.Substring(0, orderby.Length - 5), false);
                else if (orderby.EndsWith(" ASC", StringComparison.CurrentCultureIgnoreCase))
                    add(orderby.Substring(0, orderby.Length - 4), true);
                else if (!orderByList.ContainsKey(orderby))
                    orderByList.Add(orderby, ascending);
            }
        }
        public void addGroupBy(string groupby)
        {
            groupByList.Add(groupby);
        }
        public string getGroupCluase()
        {
            if (groupByList.Count <= 0)
                return string.Empty;
            else
            {
                StringBuilder b = new StringBuilder();
                foreach (string groupBy in groupByList)
                {
                    if (b.Length == 0)
                        b.Append(" GROUP BY ");
                    else
                        b.Append(", ");
                    b.Append(groupBy);
                }
                return b.ToString();
            }
        }
        public string getOrderClause()
        {
            return getOrderClause(null);
        }
        public String getOrderClause(ListInfo info)
        {
            if (orderByList.Count <= 0 && info == null)
                return string.Empty;
            else
            {

                StringBuilder b = new StringBuilder();
                if (info != null && info.orderby != null && info.orderby.Length > 0)
                {
                    if (b.Length == 0)
                        b.Append(" ORDER BY ");
                    else
                        b.Append(",");
                    b.Append(info.orderby);
                    if (info.order)
                        b.Append(" ASC");
                    else
                        b.Append(" DESC");
                }

                foreach (string orderby in orderByList.Keys)
                {
                    if (b.Length == 0)
                        b.Append(" ORDER BY ");
                    else
                        b.Append(", ");
                    b.Append(orderby);
                    if (!orderByList[orderby])
                        b.Append(" DESC");
                }
                return b.ToString();
            }
        }
        public string getWhereClause(DatabaseConnection dbConn)
        {
            return getWhereClause(dbConn, false);
        }
        public string getWhereClause(DatabaseConnection dbConn, bool first)
        {
            int out_index;
            return getWhereClause(dbConn, first, 0, out out_index);
        }
        public string getWhereClause(DatabaseConnection dbConn, bool first, int index, out int out_index)
        {
            out_index = index;

            if (dbTermsList.Count <= 0)
                return string.Empty;
            else
            {
                StringBuilder b = new StringBuilder();
                foreach (DBTerm dbTerm in dbTermsList)
                {
                    if (dbTerm is DBTermCollection)
                        if (((DBTermCollection)dbTerm).terms().Count <= 0)
                            continue;

                    if (b.Length.Equals(0))
                        b.Append(" WHERE ");
                    else
                        b.Append(" AND ");

                    out_index = dbTerm.buildSQL(dbConn, out_index, b);
                }
                return b.ToString();
            }
        }
        //public bool isEmpty();
        //public DataTable loadAggregateData(ListInfo info, string select, string from, string distinct);
        //public DataTable loadData(string sql);
        //public DataTable loadData(ListInfo info, string select, string from);
        //public DataTable loadData(ListInfo info, string select, string from, string distinct);
        //public void loadData(DataSet ds, string tableName, ListInfo info, string select, string from, string distinct);
        //public DataTable loadDataWithCount(ListInfo info, string select, string from, string distinct, string countSelect, string countFrom, DBFilter countFilter);
        public void setParams(IDbCommand c)
        {
            setParams(0, c);
        }
        public int setParams(int index, IDbCommand c)
        {
            int nextIndex = index;
            foreach (DBTerm dbTerm in dbTermsList)
            {
                nextIndex = dbTerm.setParam(nextIndex, c);
            }
            return nextIndex;
        }

        public DataTable loadData(DatabaseConnection DBConn, ListInfo info, string select, string from)
        {
            return loadData(DBConn, info, select, from, null);
        }

        public DataTable loadData(DatabaseConnection DBConn, ListInfo info, string select, string from, string distinct)
        {
            DataSet ds = new DataSet();
            loadData(DBConn, ds, "table", info, select, from, distinct);
            return ds.Tables["table"];
        }

        public DataTable loadData(DatabaseConnection DBConn, string SelectQuery)
        {
            DataSet ds = new DataSet();
            return DBConn.ExecuteToDataTable(DBConn.CreateSelectCommand(SelectQuery, this));
        }

        public void loadData(DatabaseConnection DBConn, DataSet ds, string tableName, ListInfo info, string select, string from, string distinct)
        {

            StringBuilder s = new StringBuilder();
            s.Append("SELECT ");
            if (!string.IsNullOrEmpty(distinct))
                s.Append("DISTINCT ");
            s.Append(select).Append(" ");
            s.Append(from).Append(" ");
            //s.Append(getWhereClause());
            //s.Append(getGroupCluase());
            //s.Append(getOrderClause(info));




            DbCommand command = DBConn.CreateSelectCommand(s.ToString(), this, info);
            DbDataAdapter adapter = DBConn.CreateAdapter();
            adapter.SelectCommand = command;
            try
            {
                //conn.Open();


                if (info != null)
                {

                    StringBuilder cs = new StringBuilder();
                    cs.Append("SELECT ");
                    if (!string.IsNullOrEmpty(distinct))
                        cs.Append("COUNT(DISTINCT ").Append(distinct).Append(") ");
                    else
                        cs.Append("COUNT(*) ");
                    cs.Append(from);
                    cs.Append(getWhereClause(DBConn));
                    //cs.Append(getGroupCluase());
                    DbCommand ccommand = DBConn.CreateCommand();
                    ccommand.CommandText = cs.ToString();
                    setParams(ccommand);
                    info.numRecord = Convert.ToInt32(DBConn.ExecuteScalar(ccommand));

                    if (info.recordPerPage > 0)
                    {
                        info.numPage = (int)Math.Ceiling((double)info.numRecord / info.recordPerPage);
                        if (info.page >= info.numPage)
                            info.page = info.numPage - 1;
                        if (info.page < 0)
                            info.page = 0;
                        int rowIndex = info.page * info.recordPerPage;
                        adapter.Fill(ds, rowIndex, info.recordPerPage, "table");


                    }
                    else
                    {
                        info.numPage = 1;
                        info.page = 0;
                        adapter.Fill(ds, tableName);
                    }

                }
                else
                {
                    adapter.Fill(ds, tableName);
                }
                //DataTable table=ds.Tables["table"];
                //ds.Tables.Clear();
                //return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\r\n" + select + from);
            }
            finally
            {
                adapter.Dispose();
                //conn.Close();
                //conn.Dispose();
            }
        }
    }
}
