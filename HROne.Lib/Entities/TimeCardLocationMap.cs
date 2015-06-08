using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("TimeCardLocationMap")]
    public class ETimeCardLocationMap : BaseObject 
    {
        public static DBManager db = new DBManager(typeof(ETimeCardLocationMap));

        protected int m_TimeCardLocationMapID;
        [DBField("TimeCardLocationMapID", true, true), TextSearch, Export(false)]
        public int TimeCardLocationMapID
        {
            get { return m_TimeCardLocationMapID; }
            set { m_TimeCardLocationMapID = value; modify("TimeCardLocationMapID"); }
        }
        protected string m_TimeCardLocationMapOriginalCode;
        [DBField("TimeCardLocationMapOriginalCode"), TextSearch, MaxLength(50), Export(false)]
        public string TimeCardLocationMapOriginalCode
        {
            get { return m_TimeCardLocationMapOriginalCode; }
            set { m_TimeCardLocationMapOriginalCode = value; modify("TimeCardLocationMapOriginalCode"); }
        }

        protected string m_TimeCardLocationMapNewCode;
        [DBField("TimeCardLocationMapNewCode"), TextSearch, MaxLength(50), Export(false)]
        public string TimeCardLocationMapNewCode
        {
            get { return m_TimeCardLocationMapNewCode; }
            set { m_TimeCardLocationMapNewCode = value; modify("TimeCardLocationMapNewCode"); }
        }

        public static string ConvertToNewLocationCode(DatabaseConnection dbConn, string originalCode)
        {
            if (!string.IsNullOrEmpty(originalCode))
            {
                originalCode = originalCode.Trim();
                ArrayList timeCardLocationMapList = ETimeCardLocationMap.db.select(dbConn, new DBFilter());
                foreach (ETimeCardLocationMap map in timeCardLocationMapList)
                    if (map.TimeCardLocationMapOriginalCode.Trim().Equals(originalCode, StringComparison.CurrentCultureIgnoreCase))
                        return map.TimeCardLocationMapNewCode;
            }
            return originalCode;
        }
    }

    ////  Testing Object
    //public class BaseObjectCollection<T> //: System.Collections.Generic.List<T>
    //{
    //    public T CreateObject() 
    //    {
    //        Type classType = typeof(T);
    //        System.Reflection.ConstructorInfo classConstructor = classType.GetConstructor(System.Type.EmptyTypes);
    //        T classInstance = (T)classConstructor.Invoke(null);
    //        return classInstance;
    //    }
    //}
}
