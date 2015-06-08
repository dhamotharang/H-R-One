using System.Collections.Generic;
using System.Globalization;
using System.Data;

namespace HROne.DataAccess
{

    public class WFDBList : WFValueList
    {
        string tableName;
        string keyField;
        string nameField;
        string sortField;
        List<DBTerm> terms = new List<DBTerm>();
        //DBFilter filter;
        public WFDBList(DBManager db, string keyField, string nameField)
        {
            this.tableName = db.getTableName();
            this.keyField = keyField;
            this.nameField = nameField;
            this.sortField = nameField;
        }
        public WFDBList(DBManager db, string keyField, string nameField, string sortField)
        {
            this.tableName = db.getTableName();
            this.keyField = keyField;
            this.nameField = nameField;
            this.sortField = sortField;
        }
        public WFDBList(string tableName, string keyField, string nameField)
        {
            this.tableName = tableName;
            this.keyField = keyField;
            this.nameField = nameField;
            this.sortField = nameField;
        }
        public WFDBList(string tableName, string keyField, string nameField, string sortField)
        {
            this.tableName = tableName;
            this.keyField = keyField;
            this.nameField = nameField;
            this.sortField = sortField;
        }
        public void add(DBTerm term)
        {
            terms.Add(term);
        }

       public List<WFSelectValue> getValues(DatabaseConnection DBConn, DBFilter filter, CultureInfo ci)
        {
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

            IDbCommand command = DBConn.CreateSelectCommand("SELECT DISTINCT " + keyField + " K, " + nameField + " V," + sortField + " FROM " + tableName, filter);

            DataTable table = DBConn.ExecuteToDataTable(command);

            List<WFSelectValue> list = new List<WFSelectValue>();
            foreach (DataRow row in table.Rows)
            {
                string name = row["V"].ToString().Trim();
                string val = row["K"].ToString().Trim();
                WFSelectValue sv = new WFSelectValue(val, name);
                list.Add(sv);
            }
            return list;

        }
    }

}
