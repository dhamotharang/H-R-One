using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;

namespace HROne.Lib.Entities
{
    [DBClass("AuthorizationWorkFlow")]
    public class EAuthorizationWorkFlow : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAuthorizationWorkFlow));
        public static WFValueList VLAuthorizationWorkFlow = new WFDBCodeList(EAuthorizationWorkFlow.db, "AuthorizationWorkFlowID", "AuthorizationWorkFlowCode", "AuthorizationWorkFlowDescription", "AuthorizationWorkFlowCode");

        protected int m_AuthorizationWorkFlowID;
        [DBField("AuthorizationWorkFlowID", true, true), TextSearch, Export(false)]
        public int AuthorizationWorkFlowID
        {
            get { return m_AuthorizationWorkFlowID; }
            set { m_AuthorizationWorkFlowID = value; modify("AuthorizationWorkFlowID"); }
        }
        protected string m_AuthorizationWorkFlowCode;
        [DBField("AuthorizationWorkFlowCode"), TextSearch, MaxLength(20, 20), Export(false), Required]
        public string AuthorizationWorkFlowCode
        {
            get { return m_AuthorizationWorkFlowCode; }
            set { m_AuthorizationWorkFlowCode = value; modify("AuthorizationWorkFlowCode"); }
        }
        protected string m_AuthorizationWorkFlowDescription;
        [DBField("AuthorizationWorkFlowDescription"), TextSearch, MaxLength(100, 100), Export(false), Required]
        public string AuthorizationWorkFlowDescription
        {
            get { return m_AuthorizationWorkFlowDescription; }
            set { m_AuthorizationWorkFlowDescription = value; modify("AuthorizationWorkFlowDescription"); }
        }

    }

    [DBClass("AuthorizationWorkFlowDetail")]
    public class EAuthorizationWorkFlowDetail : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAuthorizationWorkFlowDetail));

        protected int m_AuthorizationWorkFlowDetailID;
        [DBField("AuthorizationWorkFlowDetailID", true, true), TextSearch, Export(false)]
        public int AuthorizationWorkFlowDetailID
        {
            get { return m_AuthorizationWorkFlowDetailID; }
            set { m_AuthorizationWorkFlowDetailID = value; modify("AuthorizationWorkFlowDetailID"); }
        }
        protected int m_AuthorizationWorkFlowID;
        [DBField("AuthorizationWorkFlowID"), TextSearch, Export(false), Required]
        public int AuthorizationWorkFlowID
        {
            get { return m_AuthorizationWorkFlowID; }
            set { m_AuthorizationWorkFlowID = value; modify("AuthorizationWorkFlowID"); }
        }
        protected int m_AuthorizationWorkFlowIndex;
        [DBField("AuthorizationWorkFlowIndex"), TextSearch, Export(false), Required]
        public int AuthorizationWorkFlowIndex
        {
            get { return m_AuthorizationWorkFlowIndex; }
            set { m_AuthorizationWorkFlowIndex = value; modify("AuthorizationWorkFlowIndex"); }
        }
        protected int m_AuthorizationGroupID;
        [DBField("AuthorizationGroupID"), TextSearch, Export(false), Required]
        public int AuthorizationGroupID
        {
            get { return m_AuthorizationGroupID; }
            set { m_AuthorizationGroupID = value; modify("AuthorizationGroupID"); }
        }

        public ArrayList GetActualAutorizerObjectList(DatabaseConnection dbConn, int AuthorizerEmpID)
        {

            DBFilter authorizerFilter = new DBFilter();
            OR orDelegateEmpIDTerms = new OR();
            orDelegateEmpIDTerms.add(new Match("EmpID", AuthorizerEmpID));
            DBFilter authorizerDelegateFilter = new DBFilter();
            authorizerDelegateFilter.add(new Match("ad.AuthorizerDelegateEmpID", AuthorizerEmpID));
            orDelegateEmpIDTerms.add(new IN("EmpID", "SELECT ad.EmpID FROM " + EAuthorizerDelegate.db.dbclass.tableName + " ad", authorizerDelegateFilter));
            authorizerFilter.add(orDelegateEmpIDTerms);
            authorizerFilter.add(new Match("AuthorizationGroupID", AuthorizationGroupID));
            return EAuthorizer.db.select(dbConn, authorizerFilter);
        }
    }
}
