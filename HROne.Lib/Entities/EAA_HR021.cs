using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EAA_HR021")]
    public class EEAA_HR021 : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManager(typeof(EEAA_HR021));

        public static EEAA_HR021 GetObject(DatabaseConnection dbConn, int EmpID)
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("EmpID", EmpID));
            ArrayList m_list = db.select(dbConn, m_filter);
            if (m_list.Count > 0)
                return (EEAA_HR021)m_list[0];
            else
                return null;
        }
        
        protected int m_rowid;
        [DBField("RowID", true, true), TextSearch, Export(false)]
        public int rowid
        {
            get { return m_rowid; }
            set { m_rowid = value; modify("RowID"); }
        }

        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }

        protected DateTime m_reviewPeriodFr;
        [DBField("reviewPeriodFr"), TextSearch, Export(false)]
        public DateTime reviewPeriodFr
        {
            get { return m_reviewPeriodFr; }
            set { m_reviewPeriodFr = value; modify("reviewPeriodFr"); }
        }
        
        protected DateTime m_reviewPeriodTo;
        [DBField("reviewPeriodTo"), TextSearch, Export(false)]
        public DateTime reviewPeriodTo
        {
            get { return m_reviewPeriodTo; }
            set { m_reviewPeriodTo = value; modify("reviewPeriodTo"); }
        }
        
        protected string m_b1_text;
        [DBField("b1_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string b1_text
        {
            get { return m_b1_text; }
            set { m_b1_text = value; modify("b1_text"); }
        }
        
        protected bool m_b2a_bool;
        [DBField("b2a_bool"), TextSearch, Export(false)]
        public bool b2a_bool
        {
            get { return m_b2a_bool; }
            set { m_b2a_bool = value; modify("b2a_bool"); }
        }
        
        protected bool m_b2b_bool;
        [DBField("b2b_bool"), TextSearch, Export(false)]
        public bool b2b_bool
        {
            get { return m_b2b_bool; }
            set { m_b2b_bool = value; modify("b2b_bool"); }
        }

        protected bool m_b2c_bool;
        [DBField("b2c_bool"), TextSearch, Export(false)]
        public bool b2c_bool
        {
            get { return m_b2c_bool; }
            set { m_b2c_bool = value; modify("b2c_bool"); }
        }

        protected bool m_b2d_bool;
        [DBField("b2d_bool"), TextSearch, Export(false)]
        public bool b2d_bool
        {
            get { return m_b2d_bool; }
            set { m_b2d_bool = value; modify("b2d_bool"); }
        }

        protected bool m_b2e_bool;
        [DBField("b2e_bool"), TextSearch, Export(false)]
        public bool b2e_bool
        {
            get { return m_b2e_bool; }
            set { m_b2e_bool = value; modify("b2e_bool"); }
        }

        protected string m_b2e_remark_text;
        [DBField("b2e_remark_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string b2e_remark_text
        {
            get { return m_b2e_remark_text; }
            set { m_b2e_remark_text = value; modify("b2e_remark_text"); }
        }

        protected string m_b3_text;
        [DBField("b3_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string b3_text
        {
            get { return m_b3_text; }
            set { m_b3_text = value; modify("b3_text"); }
        }

        protected bool m_di_1_bool;
        [DBField("di_1_bool"), TextSearch, Export(false)]
        public bool di_1_bool
        {
            get { return m_di_1_bool; }
            set { m_di_1_bool = value; modify("di_1_bool"); }
        }

        protected bool m_di_2_bool;
        [DBField("di_2_bool"), TextSearch, Export(false)]
        public bool di_2_bool
        {
            get { return m_di_2_bool; }
            set { m_di_2_bool = value; modify("di_2_bool"); }
        }

        protected bool m_di_3_bool;
        [DBField("di_3_bool"), TextSearch, Export(false)]
        public bool di_3_bool
        {
            get { return m_di_3_bool; }
            set { m_di_3_bool = value; modify("di_3_bool"); }
        }

        protected bool m_di_4_bool;
        [DBField("di_4_bool"), TextSearch, Export(false)]
        public bool di_4_bool
        {
            get { return m_di_4_bool; }
            set { m_di_4_bool = value; modify("di_4_bool"); }
        }

        protected bool m_di_5_bool;
        [DBField("di_5_bool"), TextSearch, Export(false)]
        public bool di_5_bool
        {
            get { return m_di_5_bool; }
            set { m_di_5_bool = value; modify("di_5_bool"); }
        }

        protected string m_dii_text;
        [DBField("dii_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string dii_text
        {
            get { return m_dii_text; }
            set { m_dii_text = value; modify("dii_text"); }
        }

        protected DateTime m_submission_date;
        [DBField("submission_date"), TextSearch, Export(false)]
        public DateTime submission_date
        {
            get { return m_submission_date; }
            set { m_submission_date = value; modify("submission_date"); }
        }

        protected DateTime m_JoinDate;
        [DBField("JoinDate"), TextSearch, Export(false)]
        public DateTime JoinDate
        {
            get { return m_JoinDate; }
            set { m_JoinDate = value; modify("JoinDate"); }
        }

        protected DateTime m_AppointmentDate;
        [DBField("AppointmentDate"), TextSearch, Export(false)]
        public DateTime AppointmentDate
        {
            get { return m_AppointmentDate; }
            set { m_AppointmentDate = value; modify("AppointmentDate"); }
        }

        protected string m_EnglishName;
        [DBField("EnglishName"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string EnglishName
        {
            get { return m_EnglishName; }
            set { m_EnglishName = value; modify("EnglishName"); }
        }

        protected string m_ChineseName;
        [DBField("ChineseName"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ChineseName
        {
            get { return m_ChineseName; }
            set { m_ChineseName = value; modify("ChineseName"); }
        }

        protected string m_Section;
        [DBField("Section"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string Section
        {
            get { return m_Section; }
            set { m_Section = value; modify("Section"); }
        }

        protected string m_Position;
        [DBField("Position"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string Position
        {
            get { return m_Position; }
            set { m_Position = value; modify("Position"); }
        }
    }
}
