using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Reflection;
using HROne.CommonLib;

namespace HROne.DataAccess
{
    public class DatabaseConnection :IDisposable
    {
        protected DbConnection dbConnection;
        protected DbTransaction dbTransaction;
        protected DbProviderFactory dbFactory;
        protected DatabaseType databaseType;
        protected string connectionString;
        protected bool allowTransaction = true;
        public enum DatabaseType
        {
            MSSQL = 1,
            OLEDB = 99
        }

        public DatabaseType dbType
        {
            get { return databaseType; }
        }
        public DbConnection Connection
        {
            get { return dbConnection; }
        }

        public DatabaseConnection(string connectionString, DatabaseType databaseType)
        {
            this.databaseType = databaseType;
            this.connectionString = connectionString;
            this.dbTransaction = null;
            if (databaseType.Equals(DatabaseType.MSSQL))
            {
                dbFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.SqlClient");

                dbConnection = dbFactory.CreateConnection();
                dbConnection.ConnectionString = connectionString;


                // test the connection
                dbConnection.Open();
                dbConnection.Close();
            }
        }
        public void BeginTransaction()
        {
            if (dbTransaction != null)
                throw new Exception("Only 1 transaction is supported");
            dbConnection.Open();
            dbTransaction = dbConnection.BeginTransaction();
        }
        public void CommitTransaction()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Commit();
                dbTransaction.Dispose();
                dbTransaction = null;
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }
        public virtual void RollbackTransaction()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbTransaction = null;
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }
        public virtual DatabaseConnection createClone()
        {

            return (DatabaseConnection)Activator.CreateInstance(this.GetType(), new object[] { connectionString, dbType });
        }
        public string getQueryValueParameterName(string Name)
        {
            if (databaseType.Equals(DatabaseType.MSSQL))
            {
                return "@" + Name;
            }
            return "?";
        }

        public bool Select(DBObject obj, DBManager db)
        {
            DateTime startTime = DateTime.Now;

            IDbCommand command = CreateSelectCommand(db.selectSQLHeader, DBUtils.GetPrimaryKeyDBFilter(obj, db.keys));

            bool result = ExecuteToDBObject(command, obj, db.fields);
            // remove the makeClean() so that the update function will update all the field in db every time
            // if select() and update() is called continuously
            // e.g use force decrypt function
            //obj.makeClean();

            
            return result;
        }

        public ArrayList Select(DBFilter filter, DBManager db)
        {
            DateTime startTime = DateTime.Now;

            IDbCommand command = CreateSelectCommand(db.selectSQLHeader, filter);
            
            ArrayList result = ExecuteToArrayList(command, db.fields, db.DBObjectType);
            // remove the makeClean() so that the update function will update all the field in db every time
            // if select() and update() is called continuously
            // e.g use force decrypt function
            //foreach (DBObject obj in result)
            //    obj.makeClean();

            return result;
        }

        public bool Update(DBObject obj, string table, ICollection<DBField> dbFieldList, ICollection<DBField> keys)
        {
            DateTime startTime = DateTime.Now;

            IDbCommand command = CreateUpdateCommand(table, obj, dbFieldList, DBUtils.GetPrimaryKeyDBFilter(obj, keys));

            bool result = ExecuteNonQuery(command) > 0 ? true : false;

            return result;
        }

        public bool UpdateByTemplate(DBObject templateObj, string table, ICollection<DBField> dbFieldList, DBFilter criteriaFilter)
        {
            DateTime startTime = DateTime.Now;

            IDbCommand command = CreateUpdateCommand(table, templateObj, dbFieldList, criteriaFilter);

            bool result = ExecuteNonQuery(command) > 0 ? true : false;

            return result;
        }

        public bool Insert(DBObject obj, DBManager db)
        {
            DateTime startTime = DateTime.Now;

            IDbCommand command = CreateInsertCommand(db.insertSQLHeader, obj, db.fields);

            object primaryKey = ExecuteScalar(command);
            UpdatePrimaryKey(obj, primaryKey, db.keys);
            
            return true;
            
        }
        public bool Delete(DBObject obj, DBManager db)
        {
            DateTime startTime = DateTime.Now;

            DBFilter primaryKeyFilter = DBUtils.GetPrimaryKeyDBFilter(obj, db.keys);

            bool result = Delete(primaryKeyFilter, db);

            return result;
        }
        public bool Delete(DBFilter filter, DBManager db)
        {
            DateTime startTime = DateTime.Now;

            IDbCommand command = CreateDeleteCommand(db.deleteSQLHeader, filter);
            bool result = ExecuteNonQuery(command) > 0 ? true : false;
            return result;
        }

        public int Count(DBFilter filter, DBManager db)
        {
            DateTime startTime = DateTime.Now;

            IDbCommand command = CreateSelectCommand(db.countSQLHeader, filter);
            object result = ExecuteScalar(command);

            return Convert.ToInt32(result);
        }

        public DataTable ExecuteToDataTable(IDbCommand command)
        {
            DateTime startTime = DateTime.Now;

            if (command == null)
                return null;
            DataTable table = new DataTable();
            try
            {
                if (dbTransaction == null)
                    command.Connection.Open();
                table.Load(command.ExecuteReader());
            }
            catch (Exception)
            {
                RollbackTransaction();
            }
            finally
            {
                if (dbTransaction == null)
                    if (command.Connection.State != ConnectionState.Closed)
                        command.Connection.Close();
            }
            return table;
        }

        private bool ExecuteToDBObject(IDbCommand command, DBObject obj, ICollection<DBField> dbFieldList)
        {
            if (command == null)
                return false;

            IDataReader reader = ExecuteReader(command);
            if (reader.Read())
            {
                RetriveDBObjectFromDBDataReader(obj, reader, dbFieldList);
                reader.Close();
                return true;
            }
            reader.Close();
            return false;

            //DataTable table = ExecuteToDataTable(command);
            //ArrayList valueList = new ArrayList();

            //if (table.Rows.Count == 1)
            //{
            //    DBUtils.ConvertDataRowToDBObject(table.Rows[0], obj, dbFieldList);
            //    return true;
            //}
            //else
            //    return false;
        }
        private ArrayList ExecuteToArrayList(IDbCommand command, ICollection<DBField> dbFieldList, Type type)
        {
            if (command == null)
                return null;

            ArrayList valueList = new ArrayList();
            IDataReader reader = ExecuteReader(command);
            while (reader.Read())
            {
                DBObject obj = (DBObject)Activator.CreateInstance(type);
                RetriveDBObjectFromDBDataReader(obj, reader, dbFieldList);
                valueList.Add(obj);
            }
            reader.Close();
            //DataTable table = ExecuteToDataTable(command);

            //foreach (DataRow row in table.Rows)
            //{
            //    DBObject obj = (DBObject)Activator.CreateInstance(type);
            //    DBUtils.ConvertDataRowToDBObject(row, obj, dbFieldList);
            //    valueList.Add(obj);
            //}
            return valueList;
        }
        protected void RetriveDBObjectFromDBDataReader(DBObject obj, IDataReader reader, ICollection<DBField> dbFieldList)
        {
            int idx = 0;
            foreach (DBField dbField in dbFieldList)
            {
                object value = dbField.convert(reader.GetValue(idx));
                dbField.property.SetValue(obj, value, null);
                idx++;
            }
        }
        public IDataReader ExecuteReader(IDbCommand command)
        {
            try
            {
                if (dbTransaction == null)
                {
                    command.Connection.Open();
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                    return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                if (dbTransaction == null)
                    if (command.Connection.State != ConnectionState.Closed)
                        command.Connection.Close();

                throw ex;
            }
            finally
            {
                // will be closed automatically
                //if (command.Connection.State != ConnectionState.Closed)
                //    command.Connection.Close();
            }

        }
        public object ExecuteScalar(IDbCommand command)
        {
            if (command == null)
                return null;
            try
            {
                if (dbTransaction == null)
                    command.Connection.Open();
                object result = command.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw ex;
            }
            finally
            {
                if (dbTransaction == null)
                    if (command.Connection.State != ConnectionState.Closed)
                    command.Connection.Close();
            }

        }
        public int ExecuteNonQuery(IDbCommand command)
        {
            if (command == null)
                return 0;
            try
            {
                if (dbTransaction == null)
                    command.Connection.Open();
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw ex;
            }
            finally
            {
                if (dbTransaction == null)
                    if (command.Connection.State != ConnectionState.Closed)
                    command.Connection.Close();
            }

        }
        public DbDataAdapter CreateAdapter()
        {
            DbDataAdapter dataAdapter = dbFactory.CreateDataAdapter();
            return dataAdapter;
        }

        /** Create Command object with default parameter
         * 
         **/
        public DbCommand CreateCommand()
        {
            DbCommand command = dbConnection.CreateCommand();
            //command.Connection = dbConn;
            command.CommandTimeout = 240;
            command.Transaction = dbTransaction;
            return command;
        }
        public DbCommand CreateSelectCommand(string SelectQueryWithoutConstraint, DBFilter dbFilter)
        {
            return CreateSelectCommand(SelectQueryWithoutConstraint, dbFilter, null);
        }
        public DbCommand CreateSelectCommand(string SelectQueryWithoutConstraint, DBFilter dbFilter, ListInfo info)
        {
            int commandIndex = 1;
            int buildSQLIndex = 1;

            string query = SelectQueryWithoutConstraint;

            DbCommand command = CreateCommand();
            command.CommandType = CommandType.Text;

            if (dbFilter != null)
            {
                query += dbFilter.getWhereClause(this, false, buildSQLIndex, out buildSQLIndex);
                query += dbFilter.getGroupCluase();
                query += dbFilter.getOrderClause(info);
                dbFilter.setParams(commandIndex, command);
            }
            command.CommandText = query;


            return command;

        }
        //public DbCommand CreateSelectCommand(string FieldList, string TableList, DBFilter dbFilter)
        //{

        //    string query = "SELECT " + FieldList + " FROM " + TableList;
        //    return CreateSelectCommand(query, dbFilter, null);
        //}

        private DbCommand CreateUpdateCommand(string tablename, DBObject obj, ICollection<DBField> dbFieldList, DBFilter dbFilter)
        {
            int commandIndex = 1;
            int buildSQLIndex = 1;

            DbCommand command = CreateCommand();
            command.CommandType = CommandType.Text;

            string updateField = string.Empty;

            foreach (DBField dbField in dbFieldList)
            {
                if (!dbField.isAuto)
                    if (obj.isModified(dbField.name))
                    {

                        object value = dbField.property.GetValue(obj, null);
                        if (value is string)
                            value = ((string)value).Trim();
                        if (dbField.transcoder != null)
                            value = dbField.transcoder.toDB(value);


                        string fieldParameterName = getQueryValueParameterName(buildSQLIndex.ToString());
                        buildSQLIndex++;
                        string valueParameterName = "@" + commandIndex++;
                        string assignValue = dbField.columnName + " = " + fieldParameterName;

                        if (updateField.Equals(string.Empty))
                            updateField = " SET " + assignValue;
                        else
                            updateField += ", " + assignValue;

                        SetUpdateCommandDBParameter(command, valueParameterName, value);
                    }
            }
            if (!string.IsNullOrEmpty(updateField))
            {

                string query = "UPDATE " + tablename + updateField;

                if (dbFilter != null)
                {
                    query += dbFilter.getWhereClause(this, false, buildSQLIndex, out buildSQLIndex);
                    dbFilter.setParams(commandIndex, command);
                }
                command.CommandText = query;

                return command;
            }
            else
                return null;
        }

        private DbCommand CreateInsertCommand(string InsertQueryHeader, DBObject obj, ICollection<DBField> dbFieldList)
        {
            int commandIndex = 1;
            int buildSQLIndex = 1;

            DbCommand command = CreateCommand();
            command.CommandType = CommandType.Text;


            //string fieldList = string.Empty;
            string valueList = string.Empty;

            foreach (DBField dbField in dbFieldList)
            {
                if (!dbField.isAuto)
                {
                    //  create ALL data for insert quert once to prevent default value not set to record
                    object value = dbField.property.GetValue(obj, null);
                    if (value is string)
                        value = ((string)value).Trim();
                    if (dbField.transcoder != null)
                        value = dbField.transcoder.toDB(value);

                    string fieldParameterName = getQueryValueParameterName(buildSQLIndex.ToString());
                    buildSQLIndex++;
                    string valueParameterName = "@" + commandIndex++;

                    //if (fieldList.Equals(string.Empty))
                    //    fieldList = dbField.columnName;
                    //else
                    //    fieldList += ", " + dbField.columnName;

                    if (valueList.Equals(string.Empty))
                        valueList = fieldParameterName;
                    else
                        valueList += ", " + fieldParameterName;


                    SetUpdateCommandDBParameter(command, valueParameterName, value);
                }
            }
            string query = InsertQueryHeader + " VALUES (" + valueList + ");";

            if (databaseType.Equals(DatabaseType.MSSQL))
                query += "SELECT SCOPE_IDENTITY()";
            
            command.CommandText = query;

            return command;
        }
        protected void SetUpdateCommandDBParameter(DbCommand command, string parameterName, object parameterValue)
        {
            DbParameter param = command.CreateParameter();

            object value = parameterValue;

            if (value == null)
                value = DBNull.Value;
            if (value is int)
                param.DbType = DbType.Int32;
            else if (value is long)
                param.DbType = DbType.Int64;
            else if (value is string)
                param.DbType = DbType.String;
            else if (value is DateTime)
            {
                param.DbType = DbType.DateTime;
                if (((DateTime)value).Ticks.Equals(0))
                    value = DBNull.Value;
            }
            else if (value is double)
            {
                param.DbType = DbType.Double;
                if (double.IsNaN((double)value))
                    value = DBNull.Value;
            }
            else if (value is float)
            {
                param.DbType = DbType.Double;
                if (double.IsNaN((double)value))
                    value = DBNull.Value;
            }
            else if (value is bool)
            {
                param.DbType = DbType.Boolean;
            }
            param.ParameterName = parameterName;// "@" + commandIndex++;
            param.Value = value;
            param.Direction = ParameterDirection.Input;

            command.Parameters.Add(param);

        }
        public DbCommand CreateDeleteCommand(string DeleteQueryHeader, DBFilter dbFilter)
        {
            int commandIndex = 1;
            int buildSQLIndex = 1;

            string query = DeleteQueryHeader;
            DbCommand command = CreateCommand();
            command.CommandType = CommandType.Text;

            if (dbFilter != null)
            {
                query += dbFilter.getWhereClause(this, false, buildSQLIndex, out buildSQLIndex);
                dbFilter.setParams(commandIndex, command);
            }
            command.CommandText = query;

            return command;
        }

        private void UpdatePrimaryKey(DBObject obj, object primaryKey, ICollection<DBField> keyDBFieldList)
        {
            foreach (DBField field in keyDBFieldList)
            {
                if (field.isAuto)
                {
                    field.property.SetValue(obj, Convert.ChangeType(primaryKey, field.property.PropertyType), null);
                }
            }
        }

        public DataTable GetDataTable(string SelectQuery)
        {
            IDbCommand command = CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = SelectQuery;
            return ExecuteToDataTable(command);
        }

        //public static DatabaseConnection GetDatabaseConnection(string ConfigFilename)
        //{
        //    HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.Rijndael);

        //    System.Configuration.ConfigXmlDocument config = new System.Configuration.ConfigXmlDocument();

        //    config.Load(ConfigFilename);
        //    string ConnectionString = string.Empty;
        //    if (config["Settings"] != null)
        //    {
        //        string strDBType = string.Empty;
        //        if (config["Settings"]["dbtype"] != null)
        //        {
        //            strDBType = config["Settings"]["dbtype"].InnerText;

        //        }
        //        if (config["Settings"]["ConnectionString"] != null)
        //            ConnectionString = config["Settings"]["ConnectionString"].InnerText;

        //        if (strDBType.Equals("MSSQL"))
        //        {
        //            System.Data.SqlClient.SqlConnectionStringBuilder connStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);
        //            try
        //            {
        //                connStringBuilder.Password = crypto.Decrypting(connStringBuilder.Password, "HROne");
        //                connStringBuilder.ApplicationName = AppDomain.CurrentDomain.FriendlyName;
        //                return new DatabaseConnection(connStringBuilder.ConnectionString, DatabaseType.MSSQL);
        //            }
        //            catch
        //            {

        //            }
        //        }
        //    }


        //    return null;

        //}

        //[ThreadStatic]
        //private static DatabaseConnection defaultDatabaseConnection;
        //public static void SetDefaultDatabaseConnection(DatabaseConnection dbconn)
        //{
        //    defaultDatabaseConnection = dbconn;
        //}
        //public static DatabaseConnection GetDatabaseConnection()
        //{
        //    //if (defaultDatabaseConnection == null)
        //    //{
        //    //    string ConfigFilename = "HROne.config";
        //    //    ConfigFilename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFilename);
        //    //    defaultDatabaseConnection = GetDatabaseConnection(ConfigFilename);
        //    //}

        //    if (defaultDatabaseConnection.Connection.State == ConnectionState.Closed)
        //        return defaultDatabaseConnection;
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine("Use Clone connection");
        //        return defaultDatabaseConnection.createClone();
        //    }
        //}

        #region IDisposable Members

        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine("Disposing DatabaseConnecton");
            if (dbTransaction!=null)
                dbTransaction.Dispose();
            if (dbConnection != null)
                dbConnection.Dispose();
        }

        #endregion
    }
}
