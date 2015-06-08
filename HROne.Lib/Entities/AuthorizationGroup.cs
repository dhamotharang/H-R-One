using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("AuthorizationGroup")]
    public class EAuthorizationGroup : BaseObjectWithRecordInfo
    {

        public static DBManager db = new DBManagerWithRecordInfo(typeof(EAuthorizationGroup));
        public static WFValueList VLAuthorizationGroupID = new WFDBCodeList(EAuthorizationGroup.db, "AuthorizationGroupID", "AuthorizationCode", "AuthorizationDesc", "AuthorizationCode");

        protected int m_AuthorizationGroupID;
        [DBField("AuthorizationGroupID", true, true), TextSearch, Export(false)]
        public int AuthorizationGroupID
        {
            get { return m_AuthorizationGroupID; }
            set { m_AuthorizationGroupID = value; modify("AuthorizationGroupID"); }
        }

        protected string m_AuthorizationCode;
        [DBField("AuthorizationCode"), TextSearch,MaxLength(20,10), Export(false),Required]
        public string AuthorizationCode
        {
            get { return m_AuthorizationCode; }
            set { m_AuthorizationCode = value; modify("AuthorizationCode"); }
        }

        protected string m_AuthorizationDesc;
        [DBField("AuthorizationDesc"), TextSearch, MaxLength(100, 50), Export(false), Required]
        public string AuthorizationDesc
        {
            get { return m_AuthorizationDesc; }
            set { m_AuthorizationDesc = value; modify("AuthorizationDesc"); }
        }

        //protected bool m_AuthorizationGroupIsApproveEEInfo;
        //[DBField("AuthorizationGroupIsApproveEEInfo"), TextSearch, Export(false)]
        //public bool AuthorizationGroupIsApproveEEInfo
        //{
        //    get { return m_AuthorizationGroupIsApproveEEInfo; }
        //    set { m_AuthorizationGroupIsApproveEEInfo = value; modify("AuthorizationGroupIsApproveEEInfo"); }
        //}

        //protected bool m_AuthorizationGroupIsApproveLeave;
        //[DBField("AuthorizationGroupIsApproveLeave"), TextSearch, Export(false)]
        //public bool AuthorizationGroupIsApproveLeave
        //{
        //    get { return m_AuthorizationGroupIsApproveLeave; }
        //    set { m_AuthorizationGroupIsApproveLeave = value; modify("AuthorizationGroupIsApproveLeave"); }
        //}

        protected bool m_AuthorizationGroupIsReceiveOtherGrpAlert;
        [DBField("AuthorizationGroupIsReceiveOtherGrpAlert"), TextSearch, Export(false)]
        public bool AuthorizationGroupIsReceiveOtherGrpAlert
        {
            get { return m_AuthorizationGroupIsReceiveOtherGrpAlert; }
            set { m_AuthorizationGroupIsReceiveOtherGrpAlert = value; modify("AuthorizationGroupIsReceiveOtherGrpAlert"); }
        }

        protected string m_AuthorizationGroupEmailAddress;
        [DBField("AuthorizationGroupEmailAddress"), TextSearch, Export(false)]
        public string AuthorizationGroupEmailAddress
        {
            get { return m_AuthorizationGroupEmailAddress; }
            set { m_AuthorizationGroupEmailAddress = value; modify("AuthorizationGroupEmailAddress"); }
        }


    }
}
