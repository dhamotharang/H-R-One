using System;
using System.Text;
using System.Data;

namespace HROne.DataAccess
{
    public class Match : DBTerm
    {
        string m_name;
        string m_op;
        object m_value;

        public string name
        {
            get { return m_name; }
        }

        public string op
        {
            get { return m_op; }
        }

        public object value
        {
            get { return m_value; }
        }
        
        public Match(string name, object value)
        {
            this.m_name = name;
            this.m_op = "=";
            this.m_value = value;
        }
        public Match(string name, string op, object value)
        {
            this.m_name = name;
            this.m_op = op;
            this.m_value = value;
        }
        public int buildSQL(DatabaseConnection dbConn, int index, StringBuilder b)
        {
            //  add space before and after operator
            b.Append(m_name).Append(" ").Append(m_op).Append(" ").Append(dbConn.getQueryValueParameterName(index.ToString()));
            return ++index;
        }
        public int setParam(int index, IDbCommand c)
        {
            IDbDataParameter param = c.CreateParameter();
            param.ParameterName = "@" + index;
            if (m_value == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                if (m_value is int)
                    param.DbType = DbType.Int32;
                else if (m_value is string)
                    param.DbType = DbType.String;
                else if (m_value is DateTime)
                    param.DbType = DbType.DateTime;
                else if (m_value is double)
                    param.DbType = DbType.Double;
                else if (m_value is bool)
                    param.DbType = DbType.Boolean;
                param.Value = m_value;
            }
            param.Direction = ParameterDirection.Input;

            c.Parameters.Add(param);
            return ++index;
        }

    }
}
