using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DBClassAttribute : Attribute
    {
        public string tableName;

        public DBClassAttribute(string tableName)
        {
            this.tableName = tableName;
        }
    }
}
