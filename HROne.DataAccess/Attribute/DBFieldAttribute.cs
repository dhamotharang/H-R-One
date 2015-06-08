using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DBFieldAttribute : Attribute
    {
        public string columnName = string.Empty;
        public string excelFormat = string.Empty;
        public string format = string.Empty;
        public bool isAuto = false;
        public bool isKey = false;
        public bool textsearch = false;

        public DBFieldAttribute()
        {
        }
        public DBFieldAttribute(string columnName)
        {
            this.columnName = columnName;
        }
        public DBFieldAttribute(string columnName, string format)
        {
            this.columnName = columnName;
            this.format = format;
        }
        public DBFieldAttribute(string columnName, bool isKey, bool isAuto)
        {
            this.columnName = columnName;
            this.isKey = isKey;
            this.isAuto = isAuto;
        }
        public DBFieldAttribute(string columnName, string format, string excelFormat)
        {
            this.columnName = columnName;
            this.format = format;
            this.excelFormat = excelFormat;
        }
    }
}
