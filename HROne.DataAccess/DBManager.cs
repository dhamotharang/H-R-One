using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Data;

namespace HROne.DataAccess
{
    public class DBManager
    {
        Type type;

        DBClassAttribute m_dbclass = null;
        Dictionary<string, DBField> fieldDictionary = null;
        ICollection<DBField> fieldList = null;
        List<DBField> keyFieldList = null;

        string m_selectSQLHeader = string.Empty;
        string m_countSQLHeader = string.Empty;
        string m_insertSQLHeader = string.Empty;
        string m_deleteSQLHeader = string.Empty;

        public List<DBField> keys
        {
            get
            {
                return keyFieldList;
            }
        }
        public ICollection<DBField> fields
        {
            get
            {
                return fieldList;
            }
        }

        public DBClassAttribute dbclass
        {
            get 
            { 
                return m_dbclass; 
            }
        }

        public Type DBObjectType
        {
            get
            {
                return type;
            }
        }

        public string selectSQLHeader
        {
            get { return m_selectSQLHeader; }
        }

        public string insertSQLHeader
        {
            get { return m_insertSQLHeader; }
        }

        public string deleteSQLHeader
        {
            get { return m_deleteSQLHeader; }
        }

        public string countSQLHeader
        {
            get { return m_countSQLHeader; }
        }

        public DBManager(Type type)
        {
            System.Diagnostics.Debug.Write("Initialize DBManager for " + type.Name + ".");
            this.type = type;
            m_dbclass = DBUtils.GetDBClassAttributes(type);
            if (m_dbclass == null)
                m_dbclass = new DBClassAttribute(type.Name);
            System.Diagnostics.Debug.Write(".");


            fieldDictionary = new Dictionary<string, DBField>();
            fieldList = fieldDictionary.Values;
            keyFieldList = new List<DBField>();

            PropertyInfo[] propInfoList = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo propInfo in propInfoList)
            {


                object[] attributeList = propInfo.GetCustomAttributes(typeof(DBFieldAttribute), false);

                foreach (Attribute attribute in attributeList)
                {

                    if (attribute is DBFieldAttribute)
                    {
                        DBFieldAttribute fieldAttribute = (DBFieldAttribute)attribute;
                        DBField field = new DBField(fieldAttribute, propInfo);

                        fieldDictionary.Add(fieldAttribute.columnName, field);

                        if (fieldAttribute.isKey)
                            keyFieldList.Add(field);

                        //if (!string.IsNullOrEmpty(fieldAttribute.columnName))
                        //{
                        //    object value = SourceRow[fieldAttribute.columnName];
                        //    if (field.transcoder != null)
                        //        value = field.transcoder.fromDB(value);
                        //    propInfo.SetValue(DestinationDBObject, value, null);
                        //}
                    }
                }

            }
            System.Diagnostics.Debug.Write(".");
            prepareSQLStatement();
            System.Diagnostics.Debug.WriteLine(".done");
        }

        private void prepareSQLStatement()
        {
            m_countSQLHeader = "SELECT COUNT(*) FROM " + dbclass.tableName;
            m_deleteSQLHeader = "DELETE FROM " + dbclass.tableName;

            foreach (DBField dbField in fields)
            {
                if (!dbField.isAuto)
                {
                    if (!string.IsNullOrEmpty(m_insertSQLHeader))
                        m_insertSQLHeader += ", ";
                    m_insertSQLHeader += dbField.columnName;
                }
                if (!string.IsNullOrEmpty(m_selectSQLHeader))
                    m_selectSQLHeader += ", ";
                m_selectSQLHeader += dbField.columnName;
            }
            m_selectSQLHeader = "SELECT " + m_selectSQLHeader + " FROM " + dbclass.tableName;
            m_insertSQLHeader = "INSERT INTO " + dbclass.tableName + "(" + m_insertSQLHeader + ") ";
        }

        public DBObject createObject()
        {
            ConstructorInfo i = type.GetConstructor(new Type[0]);
            return (DBObject)i.Invoke(new object[0]);
        }
        public void copyObject(DBObject src, DBObject dest)
        {
            copyObject(src, dest, this.type);
        }
        public void copyObject(DBObject src, DBObject dest, Type type)
        {
            if (!this.type.IsSubclassOf(type) && this.type != type)
                throw new Exception("error");
            foreach (DBField field in fields)
            {
                if (!field.property.DeclaringType.IsSubclassOf(type) || field.property.DeclaringType == type)
                {
                    object v = field.property.GetValue(src, null);
                    field.property.SetValue(dest, v, null);
                }
            }
        }

        public void validate(PageErrors errors, Hashtable values)
        {
            foreach (DBField f in fields)
            {
                foreach (ValidationAttribute v in f.validations)
                {
                    v.validate(errors, f, values);
                }
            }
        }
        public string getTableName()
        {
            return dbclass.tableName;
        }
        public string getLabel(string field, CultureInfo ci)
        {
            ResourceManager rm = DBUtils.getResourceManager();
            string s = rm.GetString(type.FullName + "." + field, ci);
            if (s == null)
                s = rm.GetString("field." + field, ci);
            if (s == null)
                s = field;
            return s;
        }
        public DBField getField(string fieldName)
        {
            if (fieldDictionary.ContainsKey(fieldName))
                return fieldDictionary[fieldName];
            else
                return null;
        }
        public void populate(DBObject o, Hashtable values)
        {
            foreach (DBField f in fields)
            {
                string v = f.populate(o);
                values.Add(f.name, v);
            }
        }
        public void parse(Hashtable values, DBObject o)
        {
            foreach (DBField f in fields)
            {
                object v = values[f.name];
                if (v != null)
                {
                    v = f.parseValue(v.ToString());
                    f.property.SetValue(o, v, null);
                }
            }
        }

        public void toObject(DataRow row, DBObject o)
        {
            DBUtils.ConvertDataRowToDBObject(row, o, fields);
        }

        public bool select(HROne.DataAccess.DatabaseConnection dbAccess, DBObject obj)
        {

            return dbAccess.Select(obj, this);
        }

        public ArrayList select(HROne.DataAccess.DatabaseConnection dbAccess, DBFilter filter)
        {
            return dbAccess.Select(filter, this);
        }

        public DataTable loadDataSet(DatabaseConnection DBConn, ListInfo info, DBFilter filter)
        {
            DataSet ds= new DataSet();
            string tableName=dbclass.tableName;
            filter.loadData(DBConn, ds, tableName, info, "*", "FROM " + dbclass.tableName, null);
            return ds.Tables[tableName];
        }

        public virtual bool update(HROne.DataAccess.DatabaseConnection dbAccess, DBObject obj)
        {
            obj.beforeUpdate(dbAccess, this);
            bool result = dbAccess.Update(obj, dbclass.tableName, fields, keys);
            obj.afterUpdate(dbAccess, this);
            return result;
        }

        public virtual bool updateByTemplate(HROne.DataAccess.DatabaseConnection dbAccess, DBObject obj, DBFilter filter)
        {
            bool result = dbAccess.UpdateByTemplate(obj, dbclass.tableName, fields, filter);
            return result;
        }

        public virtual bool insert(HROne.DataAccess.DatabaseConnection dbAccess, DBObject obj)
        {
            obj.beforeInsert(dbAccess, this);
            bool result = dbAccess.Insert(obj, this);
            obj.afterInsert(dbAccess, this);
            return result;
        }

        public bool delete(HROne.DataAccess.DatabaseConnection dbAccess, DBObject obj)
        {
            obj.beforeDelete(dbAccess, this);
            bool result = dbAccess.Delete(obj, this);
            obj.afterDelete(dbAccess, this);
            return result;
        }

        public bool delete(HROne.DataAccess.DatabaseConnection dbAccess, DBFilter filter)
        {
            return dbAccess.Delete(filter, this);
        }

        public int count(HROne.DataAccess.DatabaseConnection dbAcces, DBFilter filter)
        {
            return dbAcces.Count(filter, this);
        }

        public string GetTrailInsert(DBObject o)
        {
            StringBuilder b = new StringBuilder();
            GetTrailKey(b, o);
            foreach (DBField f in fields)
            {
                if (f.isKey)
                    continue;
                b.Append(f.columnName).Append("=").Append(f.populate(o)).Append("\r\n");
            }
            return b.ToString();
        }
        public string GetTrailUpdate(DBObject o)
        {
            StringBuilder b = new StringBuilder();
            GetTrailKey(b, o);
            foreach (DBField f in fields)
            {
                if (!o.isModified(f.name))
                    continue;
                b.Append(f.columnName).Append("=").Append(f.populate(o)).Append("\r\n");
            }
            return b.ToString();
        }
        public string GetTrailDelete(DBObject o)
        {
            StringBuilder b = new StringBuilder();
            GetTrailKey(b, o);
            return o.ToString();
        }
        public void GetTrailKey(StringBuilder b, DBObject o)
        {
            bool first;
            first = true;
            foreach (DBField f in fields)
            {
                if (!f.isKey)
                    continue;
                b.Append(f.columnName).Append("=").Append(f.populate(o));
                if (first)
                    first = false;
                else
                    b.Append(",");
            }
            b.Append("\r\n");
        }

    }
}
