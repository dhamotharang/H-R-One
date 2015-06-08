using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("HierarchyElement")]
    public class EHierarchyElement : BaseObject 
    {

        public static DBManager db = new DBManager(typeof(EHierarchyElement));
        public static WFValueList VLHierarchyElement = new AppUtils.EncryptedDBCodeList(EHierarchyElement.db, "HElementID", new string[] { "HElementCode", "HElementDesc" }, " - ", "HElementCode");

        public static EHierarchyElement GetObject(DatabaseConnection dbConn, int objectID)
        {
            EHierarchyElement o = new EHierarchyElement();
            o.HElementID = objectID;
            if (EHierarchyElement.db.select(dbConn, o))
            {
                return o;
            }
            return null; 
        }

        protected int m_HElementID;
        [DBField("HElementID",true,true), TextSearch, Export(false)]
        public int HElementID
        {
            get { return m_HElementID; }
            set { m_HElementID = value; modify("HElementID"); }
        }
        protected int m_CompanyID;
        [DBField("CompanyID"), TextSearch, Export(false), Required]
        public int CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; modify("CompanyID"); }
        }
        protected string m_HElementCode;
        [DBField("HElementCode"), TextSearch, DBAESEncryptStringField, MaxLength(20), Export(false), Required]
        public string HElementCode
        {
            get { return m_HElementCode; }
            set { m_HElementCode = value; modify("HElementCode"); }
        }
        protected string m_HElementDesc;
        [DBField("HElementDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 40), Export(false), Required]
        public string HElementDesc
        {
            get { return m_HElementDesc; }
            set { m_HElementDesc = value; modify("HElementDesc"); }
        }

        protected int m_HLevelID;
        [DBField("HLevelID"), TextSearch, Export(false), Required]
        public int HLevelID
        {
            get { return m_HLevelID; }
            set { m_HLevelID = value; modify("HLevelID"); }
        }

    }
}
