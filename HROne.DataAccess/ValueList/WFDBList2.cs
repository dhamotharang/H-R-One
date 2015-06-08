using System.Globalization;
using System.Collections.Generic;
using System.Data;

namespace HROne.DataAccess
{
    abstract public class WFDBList2 : WFValueList
    {
        string tableName;
        string sortField;
        List<DBTerm> terms = new List<DBTerm>();
        //DBFilter filter;
        string[] fields;
        public bool distinct = false;
        public WFDBList2(DBManager db, string[] fields)
        {
            this.fields = fields;
            this.tableName = db.dbclass.tableName;
        }
        public WFDBList2(string tableName, string[] fields)
        {
            this.fields = fields;
            this.tableName = tableName;
        }
        public WFDBList2(string tableName, string[] fields, string sortField)
        {
            this.fields = fields;
            this.tableName = tableName;
            this.sortField = sortField;
        }
        public WFDBList2(DBManager db, string[] fields, string sortField)
        {
            this.tableName = db.dbclass.tableName;
            this.fields = fields;
            this.sortField = sortField;
        }
        public WFDBList2(string tableName, string keyField, string nameField)
        {
            this.tableName = tableName;
            this.sortField = nameField;
        }
        public void add(DBTerm term)
        {
            terms.Add(term);
        }


        abstract public string GetText(IDataReader reader);
        abstract public string GetKey(IDataReader reader);

        public List<WFSelectValue> getValues(DatabaseConnection DBConn, DBFilter filter, CultureInfo ci)
        {
            string sql = "SELECT ";
            if (distinct)
                sql += "DISTINCT ";
            for (int i = 0; i != fields.Length; i++)
            {
                if (i > 0)
                    sql += ",";
                sql += fields[i];
            }
            sql += " FROM " + tableName;

            if (terms.Count > 0)
            {
                if (filter == null)
                    filter = new DBFilter();
                foreach (DBTerm t in terms)
                {
                    filter.add(t);
                }
            }
            if (!string.IsNullOrEmpty(sortField))
            {
                if (filter == null)
                    filter = new DBFilter();
                filter.add(sortField, true);
            }
            IDbCommand command = DBConn.CreateSelectCommand(sql, filter);


            DataTable table = DBConn.ExecuteToDataTable(command);

            List<WFSelectValue> list = new List<WFSelectValue>();
            IDataReader reader = DBConn.ExecuteReader(command);
            while (reader.Read())
            {
                string name = GetText(reader);
                string val = GetKey(reader);
                WFSelectValue sv = new WFSelectValue(val, name);
                list.Add(sv);
            }
            reader.Close();
            return list;

        }
    }
}
