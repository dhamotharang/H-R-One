using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("CostCenter")]
    public class ECostCenter : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ECostCenter));
        public static WFValueList VLCostCenter = new AppUtils.EncryptedDBCodeList(db, "CostCenterID", new string[] { "CostCenterCode", "CostCenterDesc" }," - ", "CostCenterCode");
        public const string DEFAULT_COST_CENTER_TEXT = "Default Cost Center";

        public static ECostCenter GetObject(DatabaseConnection dbConn, int ID)
        {
            ECostCenter m_object = new ECostCenter();
            m_object.CostCenterID = ID;
            if (ECostCenter.db.select(dbConn, m_object))
                return m_object;
            return null;
        }

        protected int m_CostCenterID;
        [DBField("CostCenterID", true, true), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }
        protected string m_CostCenterCode;
        [DBField("CostCenterCode"), TextSearch, DBAESEncryptStringField, MaxLength(20, 10), Export(false), Required]
        public string CostCenterCode
        {
            get { return m_CostCenterCode; }
            set { m_CostCenterCode = value; modify("CostCenterCode"); }
        }

        protected string m_CostCenterDesc;
        [DBField("CostCenterDesc"), TextSearch, DBAESEncryptStringField, MaxLength(100, 25), Export(false), Required]
        public string CostCenterDesc
        {
            get { return m_CostCenterDesc; }
            set { m_CostCenterDesc = value; modify("CostCenterDesc"); }
        }


    }
}
