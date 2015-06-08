using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("RosterClient")]
    public class ERosterClient : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERosterClient));
        public static WFValueList VLRosterClient = new WFDBCodeList(ERosterClient.db, "RosterClientID", "RosterClientCode", "RosterClientName", "RosterClientCode");

        protected int m_RosterClientID;
        [DBField("RosterClientID", true, true), TextSearch, Export(false)]
        public int RosterClientID
        {
            get { return m_RosterClientID; }
            set { m_RosterClientID = value; modify("RosterClientID"); }
        }
        protected string m_RosterClientCode;
        [DBField("RosterClientCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string RosterClientCode
        {
            get { return m_RosterClientCode; }
            set { m_RosterClientCode = value; modify("RosterClientCode"); }
        }
        protected string m_RosterClientName;
        [DBField("RosterClientName"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string RosterClientName
        {
            get { return m_RosterClientName; }
            set { m_RosterClientName = value; modify("RosterClientName"); }
        }

        protected int m_RosterClientMappingSiteCodeToHLevelID;
        [DBField("RosterClientMappingSiteCodeToHLevelID"), TextSearch, Export(false), ]
        public int RosterClientMappingSiteCodeToHLevelID
        {
            get { return m_RosterClientMappingSiteCodeToHLevelID; }
            set { m_RosterClientMappingSiteCodeToHLevelID = value; modify("RosterClientMappingSiteCodeToHLevelID"); }
        }
        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

    }

    [DBClass("RosterClientSite")]
    public class ERosterClientSite : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ERosterClientSite));
        public static WFValueList VLRosterClientSite = new WFDBList(ERosterClientSite.db, "RosterClientSiteID", "RosterClientSiteCode", "RosterClientSitePropertyName");

        protected int m_RosterClientSiteID;
        [DBField("RosterClientSiteID", true, true), TextSearch, Export(false)]
        public int RosterClientSiteID
        {
            get { return m_RosterClientSiteID; }
            set { m_RosterClientSiteID = value; modify("RosterClientSiteID"); }
        }
        protected int m_RosterClientID;
        [DBField("RosterClientID"), TextSearch, Export(false)]
        public int RosterClientID
        {
            get { return m_RosterClientID; }
            set { m_RosterClientID = value; modify("RosterClientID"); }
        }
        protected string m_RosterClientSiteCode;
        [DBField("RosterClientSiteCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string RosterClientSiteCode
        {
            get { return m_RosterClientSiteCode; }
            set { m_RosterClientSiteCode = value; modify("RosterClientSiteCode"); }
        }
        protected string m_RosterClientSitePropertyName;
        [DBField("RosterClientSitePropertyName"), TextSearch,MaxLength(100, 25), Export(false)]
        public string RosterClientSitePropertyName
        {
            get { return m_RosterClientSitePropertyName; }
            set { m_RosterClientSitePropertyName = value; modify("RosterClientSitePropertyName"); }
        }
        protected string m_RosterClientSiteLocation;
        [DBField("RosterClientSiteLocation"), TextSearch, MaxLength(200, 75), Export(false)]
        public string RosterClientSiteLocation
        {
            get { return m_RosterClientSiteLocation; }
            set { m_RosterClientSiteLocation = value; modify("RosterClientSiteLocation"); }
        }

        protected string m_RosterClientSitePremisesNature;
        [DBField("RosterClientSitePremisesNature"), TextSearch, MaxLength(50, 25), Export(false)]
        public string RosterClientSitePremisesNature
        {
            get { return m_RosterClientSitePremisesNature; }
            set { m_RosterClientSitePremisesNature = value; modify("RosterClientSitePremisesNature"); }
        }
        protected string m_RosterClientSiteInCharge;
        [DBField("RosterClientSiteInCharge"), TextSearch, Export(false)]
        public string RosterClientSiteInCharge
        {
            get { return m_RosterClientSiteInCharge; }
            set { m_RosterClientSiteInCharge = value; modify("RosterClientSiteInCharge"); }
        }
        protected string m_RosterClientSiteInChargeContactNo;
        [DBField("RosterClientSiteInChargeContactNo"), TextSearch, Export(false)]
        public string RosterClientSiteInChargeContactNo
        {
            get { return m_RosterClientSiteInChargeContactNo; }
            set { m_RosterClientSiteInChargeContactNo = value; modify("RosterClientSiteInChargeContactNo"); }
        }


        protected string m_RosterClientSiteServiceHours;
        [DBField("RosterClientSiteServiceHours"), TextSearch, MaxLength(100, 25), Export(false)]
        public string RosterClientSiteServiceHours
        {
            get { return m_RosterClientSiteServiceHours; }
            set { m_RosterClientSiteServiceHours = value; modify("RosterClientSiteServiceHours"); }
        }
        protected string m_RosterClientSiteShift;
        [DBField("RosterClientSiteShift"), TextSearch, MaxLength(100, 25), Export(false)]
        public string RosterClientSiteShift
        {
            get { return m_RosterClientSiteShift; }
            set { m_RosterClientSiteShift = value; modify("RosterClientSiteShift"); }
        }
        protected int m_CostCenterID;
        [DBField("CostCenterID"), TextSearch, Export(false)]
        public int CostCenterID
        {
            get { return m_CostCenterID; }
            set { m_CostCenterID = value; modify("CostCenterID"); }
        }

        public int GetDefaultCostCenterID(DatabaseConnection dbConn)
        {
            if (m_CostCenterID != 0)
            {
                ECostCenter costCenter = new ECostCenter();
                costCenter.CostCenterID = m_CostCenterID;
                if (ECostCenter.db.select(dbConn, costCenter))
                    return costCenter.CostCenterID;
            }
            ERosterClient rosterClient = new ERosterClient();
            rosterClient.RosterClientID = m_RosterClientID;
            if (ERosterClient.db.select(dbConn, rosterClient))
            {
                ECostCenter costCenter = new ECostCenter();
                costCenter.CostCenterID = rosterClient.CostCenterID;
                if (ECostCenter.db.select(dbConn, costCenter))
                    return costCenter.CostCenterID;
            }
            return 0;
        }
    }

}
