using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("ESSAnnouncement")]
    public class EESSAnnouncement : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EESSAnnouncement));

        protected int m_ESSAnnouncementID;
        [DBField("ESSAnnouncementID", true, true), TextSearch, Export(false)]
        public int ESSAnnouncementID
        {
            get { return m_ESSAnnouncementID; }
            set { m_ESSAnnouncementID = value; modify("ESSAnnouncementID"); }
        }
        protected string m_ESSAnnouncementCode;
        [DBField("ESSAnnouncementCode"), TextSearch, MaxLength(20), Export(false), Required]
        public string ESSAnnouncementCode
        {
            get { return m_ESSAnnouncementCode; }
            set { m_ESSAnnouncementCode = string.IsNullOrEmpty(value) ? value : value.Trim().ToUpper(); modify("ESSAnnouncementCode"); }
        }

        protected DateTime m_ESSAnnouncementEffectiveDate;
        [DBField("ESSAnnouncementEffectiveDate"), TextSearch, Export(false)]
        public DateTime ESSAnnouncementEffectiveDate
        {
            get { return m_ESSAnnouncementEffectiveDate; }
            set { m_ESSAnnouncementEffectiveDate = value; modify("ESSAnnouncementEffectiveDate"); }
        }
        protected DateTime m_ESSAnnouncementExpiryDate;
        [DBField("ESSAnnouncementExpiryDate"), TextSearch, Export(false)]
        public DateTime ESSAnnouncementExpiryDate
        {
            get { return m_ESSAnnouncementExpiryDate; }
            set { m_ESSAnnouncementExpiryDate = value; modify("ESSAnnouncementExpiryDate"); }
        }
        protected string m_ESSAnnouncementContent;
        [DBField("ESSAnnouncementContent"), TextSearch, Export(false)]
        public string ESSAnnouncementContent
        {
            get { return m_ESSAnnouncementContent; }
            set { m_ESSAnnouncementContent = value; modify("ESSAnnouncementContent"); }
        }
        protected string m_ESSAnnouncementTargetCompanies;
        [DBField("ESSAnnouncementTargetCompanies"), TextSearch, Export(false)]
        public string ESSAnnouncementTargetCompanies
        {
            get { return m_ESSAnnouncementTargetCompanies; }
            set { m_ESSAnnouncementTargetCompanies = value; modify("ESSAnnouncementTargetCompanies"); }
        }
        protected string m_ESSAnnouncementTargetRanks;
        [DBField("ESSAnnouncementTargetRanks"), TextSearch, Export(false)]
        public string ESSAnnouncementTargetRanks
        {
            get { return m_ESSAnnouncementTargetRanks; }
            set { m_ESSAnnouncementTargetRanks = value; modify("ESSAnnouncementTargetRanks"); }
        }

        //public static EESSAnnouncement GetObject(DatabaseConnection dbConn, string ESSAnnouncementCode)
        //{
        //    DBFilter filter = new DBFilter();
        //    filter.add(new Match("ESSAnnouncementCode", ESSAnnouncementCode));
        //    ArrayList essAnnouncementCodeList = EESSAnnouncement.db.select(dbConn, filter);
        //    if (essAnnouncementCodeList.Count > 0)
        //    {
        //        return (EESSAnnouncement)essAnnouncementCodeList[0];
        //    }
        //    else
        //        return new EESSAnnouncement();
        //}
    }
}
