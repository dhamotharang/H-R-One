using System;
using System.Data;
using System.Configuration;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("MPFScheme")]
    public class EMPFScheme: BaseObject
    {
        public static DBManager db = new DBManager(typeof(EMPFScheme));
        public static WFValueList VLMPFScheme = new WFDBList(EMPFScheme.db, "MPFSchemeID", "MPFSchemeCode + ' - ' + MPFSchemeDesc");
        public static WFValueList VLMPFSchemeTrusteeCode = new WFDBList(EMPFScheme.db, "MPFSchemeTrusteeCode", "MPFSchemeTrusteeCode");

        public static EMPFScheme GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EMPFScheme m_object = new EMPFScheme();
                m_object.MPFSchemeID = ID;
                if (EMPFScheme.db.select(dbConn, m_object))
                    return m_object;
            }
            return null;
        }

        protected int m_MPFSchemeID;
        [DBField("MPFSchemeID",true,true), TextSearch, Export(false), Required]
        public int MPFSchemeID
        {
            get { return m_MPFSchemeID; }
            set { m_MPFSchemeID = value; modify("MPFSchemeID"); }
        }
        protected string m_MPFSchemeCode;
        [DBField("MPFSchemeCode"), MaxLength(20, 20), TextSearch, Export(false), Required]
        public string MPFSchemeCode
        {
            get { return m_MPFSchemeCode; }
            set { m_MPFSchemeCode = value; modify("MPFSchemeCode"); }
        }
        protected string m_MPFSchemeDesc;
        [DBField("MPFSchemeDesc"), MaxLength(100, 25), TextSearch, Export(false), Required]
        public string MPFSchemeDesc
        {
            get { return m_MPFSchemeDesc; }
            set { m_MPFSchemeDesc = value; modify("MPFSchemeDesc"); }
        }
        protected string m_MPFSchemeTrusteeCode;
        [DBField("MPFSchemeTrusteeCode"), MaxLength(20, 20), TextSearch, Export(false)]
        public string MPFSchemeTrusteeCode
        {
            get { return string.IsNullOrEmpty(m_MPFSchemeTrusteeCode) ? string.Empty : m_MPFSchemeTrusteeCode; }
            set { m_MPFSchemeTrusteeCode = value; modify("MPFSchemeTrusteeCode"); }
        }

    }
}
