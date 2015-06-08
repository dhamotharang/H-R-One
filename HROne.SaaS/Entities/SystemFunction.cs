using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;

namespace HROne.SaaS.Entities
{
    [DBClass("SystemFunction")]
    public class ESystemFunction : DBObject
    {
        public static DBManager db = new DBManager(typeof(ESystemFunction));
        public static WFValueList VLSystemFunction = new WFDBCodeList(db, "FunctionID", "FunctionCode", "Description", "FunctionCode");
        protected int m_FunctionID;
        [DBField("FunctionID", true, true), TextSearch, Export(false)]
        public int FunctionID
        {
            get { return m_FunctionID; }
            set { m_FunctionID = value; modify("FunctionID"); }
        }
        protected string m_FunctionCode;
        [DBField("FunctionCode"), MaxLength(10, 10), TextSearch, Export(false)]
        public string FunctionCode
        {
            get { return m_FunctionCode; }
            set { m_FunctionCode = value; modify("FunctionCode"); }
        }
        protected string m_Description;
        [DBField("Description"), MaxLength(100, 50), TextSearch, Export(false)]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; modify("Description"); }
        }

        public static ESystemFunction GetObjectByCode(DatabaseConnection dbConn, string FunctionCode)
        {
            DBFilter functionFilter = new DBFilter();
            functionFilter.add(new Match("FunctionCode", FunctionCode));
            ArrayList functionList = ESystemFunction.db.select(dbConn, functionFilter);
            if (functionList.Count > 0)
                return (ESystemFunction)functionList[0];
            else
                return null;
        }

        public static void AddSystemFunction(DatabaseConnection dbConn, string FunctionCode, string FunctionDescription)
        {
            ESystemFunction function = GetObjectByCode(dbConn, FunctionCode);
            if (function != null)
            {
                if (!function.Description.Equals(FunctionDescription))
                    function.Description = FunctionDescription;
                ESystemFunction.db.update(dbConn, function);
            }
            else
            {
                function = new ESystemFunction();
                function.FunctionCode = FunctionCode;
                function.Description = FunctionDescription;
                ESystemFunction.db.insert(dbConn, function);
            }
        }
    }
}
