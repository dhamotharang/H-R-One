using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("MPFSchemeCessationReasonMapping")]
    public class EMPFSchemeCessationReasonMapping : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EMPFSchemeCessationReasonMapping));

        protected int m_MPFSchemeCessationReasonMappingID;
        [DBField("MPFSchemeCessationReasonMappingID", true, true), TextSearch, Export(false)]
        public int MPFSchemeCessationReasonMappingID
        {
            get { return m_MPFSchemeCessationReasonMappingID; }
            set { m_MPFSchemeCessationReasonMappingID = value; modify("MPFSchemeCessationReasonMappingID"); }
        }
        protected int m_MPFSchemeCessationReasonID;
        [DBField("MPFSchemeCessationReasonID"), TextSearch, Export(false)]
        public int MPFSchemeCessationReasonID
        {
            get { return m_MPFSchemeCessationReasonID; }
            set { m_MPFSchemeCessationReasonID = value; modify("MPFSchemeCessationReasonID"); }
        }

        protected int m_CessationReasonID;
        [DBField("CessationReasonID"), TextSearch, Export(false)]
        public int CessationReasonID
        {
            get { return m_CessationReasonID; }
            set { m_CessationReasonID = value; modify("CessationReasonID"); }
        }

        public static string GetMPFSchemeTermCode(DatabaseConnection dbConn, int MPFSchemeID, int CessationReasonID)
        {
            DBFilter cessationReasonFilter = new DBFilter();
            cessationReasonFilter.add(new Match("CessationReasonID", CessationReasonID));

            DBFilter MPFSchemeCessationReasonFilter = new DBFilter();
            MPFSchemeCessationReasonFilter.add(new Match("MPFSchemeID", MPFSchemeID));
            MPFSchemeCessationReasonFilter.add(new IN("MPFSchemeCessationReasonID", "Select MPFSchemeCessationReasonID from MPFSchemeCessationReasonMapping", cessationReasonFilter));

            ArrayList mpfSchemeCesationReasonList = EMPFSchemeCessationReason.db.select(dbConn, MPFSchemeCessationReasonFilter);
            if (mpfSchemeCesationReasonList.Count > 0)
                return ((EMPFSchemeCessationReason)mpfSchemeCesationReasonList[0]).MPFSchemeCessationReasonCode;
            else
            {
                ECessationReason cessationReason = new ECessationReason();
                cessationReason.CessationReasonID = CessationReasonID;
                ECessationReason.db.select(dbConn, cessationReason);
                return cessationReason.CessationReasonCode;
            }
        }
    }
}
