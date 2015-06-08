using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.DataAccess
{
    public abstract class DBObject
    {
        //public DBObject()
        //{
        //}

        protected internal List<string> modifiedList = new List<string>();

        protected internal virtual void afterDelete(DatabaseConnection dbConn, DBManager db)
        {
        }
        protected internal virtual void afterInsert(DatabaseConnection dbConn, DBManager db)
        {
        }
        protected internal virtual void afterUpdate(DatabaseConnection dbConn, DBManager db)
        {
        }
        protected internal virtual void beforeDelete(DatabaseConnection dbConn, DBManager db)
        {
        }
        protected internal virtual void beforeInsert(DatabaseConnection dbConn, DBManager db)
        {
        }
        protected internal virtual void beforeUpdate(DatabaseConnection dbConn, DBManager db)
        {
        }
        protected internal bool isModified(string name)
        {
            if (modifiedList.Contains(name))
                return true;
            else
                return false;
        }
        protected internal void makeClean()
        {
            modifiedList.Clear();
        }
        protected virtual void modify(string name)
        {
            if (!modifiedList.Contains(name))
                modifiedList.Add(name);
        }
    }
}
