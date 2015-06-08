using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Resources;

namespace HROne.DataAccess
{
    public class DBUtils
    {
        private static ResourceManager rm;
        public static String rmpath;
        public static ResourceManager getResourceManager()
        {

            string resourceDirectory = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (string.IsNullOrEmpty(resourceDirectory))
            {
                resourceDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }

            rmpath = resourceDirectory;
            rm = ResourceManager.CreateFileBasedResourceManager("perspectivemind", resourceDirectory, null);
            return rm;
        }
        public static DBClassAttribute GetDBClassAttributes(Type type)
        {
            MemberInfo info = type;
            object[] attributeList = info.GetCustomAttributes(typeof(DBClassAttribute), false);
            if (attributeList.GetLength(0) == 1)
            {
                return ((DBClassAttribute)attributeList[0]);
            }
            return null;
        }
        public static string GetTableName(Type type)
        {
            DBClassAttribute attribute= GetDBClassAttributes(type);
            if (attribute==null)
                throw new Exception("Invalid DBClassAttribute Setting");
            return attribute.tableName;
        }
        //public static Dictionary<string, DBField> GetDBPrimaryKeyFieldList(Type type)
        //{
        //    Dictionary<string, DBField> dbFieldList = new Dictionary<string, DBField>();

        //    PropertyInfo[] propInfoList = type.GetProperties();

        //    foreach (PropertyInfo propInfo in propInfoList)
        //    {


        //        object[] attributeList = propInfo.GetCustomAttributes(typeof(DBFieldAttribute), false);

        //        foreach (Attribute attribute in attributeList)
        //        {

        //            if (attribute is DBFieldAttribute)
        //            {
        //                DBFieldAttribute fieldAttribute = (DBFieldAttribute)attribute;
        //                if (fieldAttribute.isKey)
        //                {
        //                    DBField field = new DBField(fieldAttribute, propInfo);

        //                    dbFieldList.Add(fieldAttribute.columnName, field);
        //                }
        //                //if (!string.IsNullOrEmpty(fieldAttribute.columnName))
        //                //{
        //                //    object value = SourceRow[fieldAttribute.columnName];
        //                //    if (field.transcoder != null)
        //                //        value = field.transcoder.fromDB(value);
        //                //    propInfo.SetValue(DestinationDBObject, value, null);
        //                //}
        //            }
        //        }

        //    }
        //    return dbFieldList;

        //}

        //public static DBFilter GetPrimaryKeyDBFilter(DBObject obj)
        //{
        //    ICollection<DBField> primaryKeyDBFieldList = GetDBPrimaryKeyFieldList(obj.GetType()).Values;

        //    return GetPrimaryKeyDBFilter(obj, primaryKeyDBFieldList);
        //}

        public static DBFilter GetPrimaryKeyDBFilter(DBObject obj, ICollection<DBField> primaryKeyDBFieldList)
        {


            DBFilter dbFilter = new DBFilter();


            foreach (DBField dbField in primaryKeyDBFieldList)
            {
                object value = dbField.property.GetValue(obj, null);
                dbFilter.add(new Match(dbField.columnName, value));
            }

            return dbFilter;
        }

        public static void ConvertDataRowToDBObject(System.Data.DataRow SourceRow, DBObject DestinationDBObject, ICollection<DBField> fieldList)
        {

            foreach (DBField dbField in fieldList)
            {
                object value = null;
                if (SourceRow.Table.Columns.Contains(dbField.columnName))
                {
                    //  run transcoder from source data
                    value = SourceRow[dbField.columnName];
                    value = dbField.convert(value);
                }
                else if (SourceRow.Table.Columns.Contains(dbField.name))
                {
                    value = SourceRow[dbField.name];
                }
                dbField.property.SetValue(DestinationDBObject, value, null);
            }

            //PropertyInfo[] propInfoList = DestinationDBObject.GetType().GetProperties();

            //foreach (PropertyInfo propInfo in propInfoList)
            //{


            //    object[] attributeList = propInfo.GetCustomAttributes(typeof(DBFieldAttribute), false);

            //    foreach (Attribute attribute in attributeList)
            //    {

            //        if (attribute is DBFieldAttribute)
            //        {
            //            DBFieldAttribute fieldAttribute = (DBFieldAttribute)attribute;
            //            DBField field = new DBField(fieldAttribute, propInfo);

            //            if (!string.IsNullOrEmpty(fieldAttribute.columnName))
            //            {
            //                object value = SourceRow[fieldAttribute.columnName];
            //                if (field.transcoder != null)
            //                    value = field.transcoder.fromDB(value);
            //                propInfo.SetValue(DestinationDBObject, value, null);
            //            }
            //        }
            //    }

            //}

        }

        //public static Hashtable CreateUpdateHashTable(DBObject obj)
        //{
        //}


    }
}
