using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EAA_HR020")]
    public class EEAA_HR020 : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManager(typeof(EEAA_HR020));

        public static EEAA_HR020 GetObject(DatabaseConnection dbConn, int EmpID)
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("EmpID", EmpID));
            ArrayList m_list = db.select(dbConn, m_filter);
            if (m_list.Count > 0)
                return (EEAA_HR020)m_list[0];
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

        protected string m_ci_1_key_text;
        [DBField("ci_1_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_1_key_text
        {
            get { return m_ci_1_key_text; }
            set { m_ci_1_key_text = value; modify("ci_1_key_text"); }
        }

        protected string m_ci_1_assess_text;
        [DBField("ci_1_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_1_assess_text
        {
            get { return m_ci_1_assess_text; }
            set { m_ci_1_assess_text = value; modify("ci_1_assess_text"); }
        }

        protected string m_ci_1_text3;
        [DBField("ci_1_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_1_text3
        {
            get { return m_ci_1_text3; }
            set { m_ci_1_text3 = value; modify("ci_1_text3"); }
        }

        protected string m_ci_1_text4;
        [DBField("ci_1_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_1_text4
        {
            get { return m_ci_1_text4; }
            set { m_ci_1_text4 = value; modify("ci_1_text4"); }
        }

        protected string m_ci_2_key_text;
        [DBField("ci_2_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_2_key_text
        {
            get { return m_ci_2_key_text; }
            set { m_ci_2_key_text = value; modify("ci_2_key_text"); }
        }

        protected string m_ci_2_assess_text;
        [DBField("ci_2_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_2_assess_text
        {
            get { return m_ci_2_assess_text; }
            set { m_ci_2_assess_text = value; modify("ci_2_assess_text"); }
        }

        protected string m_ci_2_text3;
        [DBField("ci_2_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_2_text3
        {
            get { return m_ci_2_text3; }
            set { m_ci_2_text3 = value; modify("ci_2_text3"); }
        }

        protected string m_ci_2_text4;
        [DBField("ci_2_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_2_text4
        {
            get { return m_ci_2_text4; }
            set { m_ci_2_text4 = value; modify("ci_2_text4"); }
        }

        protected string m_ci_3_key_text;
        [DBField("ci_3_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_3_key_text
        {
            get { return m_ci_3_key_text; }
            set { m_ci_3_key_text = value; modify("ci_3_key_text"); }
        }

        protected string m_ci_3_assess_text;
        [DBField("ci_3_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_3_assess_text
        {
            get { return m_ci_3_assess_text; }
            set { m_ci_3_assess_text = value; modify("ci_3_assess_text"); }
        }

        protected string m_ci_3_text3;
        [DBField("ci_3_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_3_text3
        {
            get { return m_ci_3_text3; }
            set { m_ci_3_text3 = value; modify("ci_3_text3"); }
        }

        protected string m_ci_3_text4;
        [DBField("ci_3_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_3_text4
        {
            get { return m_ci_3_text4; }
            set { m_ci_3_text4 = value; modify("ci_3_text4"); }
        }

        protected string m_ci_4_key_text;
        [DBField("ci_4_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_4_key_text
        {
            get { return m_ci_4_key_text; }
            set { m_ci_4_key_text = value; modify("ci_4_key_text"); }
        }

        protected string m_ci_4_assess_text;
        [DBField("ci_4_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_4_assess_text
        {
            get { return m_ci_4_assess_text; }
            set { m_ci_4_assess_text = value; modify("ci_4_assess_text"); }
        }

        protected string m_ci_4_text3;
        [DBField("ci_4_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_4_text3
        {
            get { return m_ci_4_text3; }
            set { m_ci_4_text3 = value; modify("ci_4_text3"); }
        }

        protected string m_ci_4_text4;
        [DBField("ci_4_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_4_text4
        {
            get { return m_ci_4_text4; }
            set { m_ci_4_text4 = value; modify("ci_4_text4"); }
        }

        protected string m_ci_5_key_text;
        [DBField("ci_5_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_5_key_text
        {
            get { return m_ci_5_key_text; }
            set { m_ci_5_key_text = value; modify("ci_5_key_text"); }
        }

        protected string m_ci_5_assess_text;
        [DBField("ci_5_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_5_assess_text
        {
            get { return m_ci_5_assess_text; }
            set { m_ci_5_assess_text = value; modify("ci_5_assess_text"); }
        }
        protected string m_ci_5_text3;
        [DBField("ci_5_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_5_text3
        {
            get { return m_ci_5_text3; }
            set { m_ci_5_text3 = value; modify("ci_5_text3"); }
        }

        protected string m_ci_5_text4;
        [DBField("ci_5_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_5_text4
        {
            get { return m_ci_5_text4; }
            set { m_ci_5_text4 = value; modify("ci_5_text4"); }
        }

        protected string m_ci_6_key_text;
        [DBField("ci_6_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_6_key_text
        {
            get { return m_ci_6_key_text; }
            set { m_ci_6_key_text = value; modify("ci_6_key_text"); }
        }

        protected string m_ci_6_assess_text;
        [DBField("ci_6_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_6_assess_text
        {
            get { return m_ci_6_assess_text; }
            set { m_ci_6_assess_text = value; modify("ci_6_assess_text"); }
        }

        protected string m_ci_6_text3;
        [DBField("ci_6_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_6_text3
        {
            get { return m_ci_6_text3; }
            set { m_ci_6_text3 = value; modify("ci_6_text3"); }
        }

        protected string m_ci_6_text4;
        [DBField("ci_6_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_6_text4
        {
            get { return m_ci_6_text4; }
            set { m_ci_6_text4 = value; modify("ci_6_text4"); }
        }

        protected string m_ci_7_key_text;
        [DBField("ci_7_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_7_key_text
        {
            get { return m_ci_7_key_text; }
            set { m_ci_7_key_text = value; modify("ci_7_key_text"); }
        }

        protected string m_ci_7_assess_text;
        [DBField("ci_7_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_7_assess_text
        {
            get { return m_ci_7_assess_text; }
            set { m_ci_7_assess_text = value; modify("ci_7_assess_text"); }
        }

        protected string m_ci_7_text3;
        [DBField("ci_7_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_7_text3
        {
            get { return m_ci_7_text3; }
            set { m_ci_7_text3 = value; modify("ci_7_text3"); }
        }

        protected string m_ci_7_text4;
        [DBField("ci_7_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_7_text4
        {
            get { return m_ci_7_text4; }
            set { m_ci_7_text4 = value; modify("ci_7_text4"); }
        }

        protected string m_ci_8_key_text;
        [DBField("ci_8_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_8_key_text
        {
            get { return m_ci_8_key_text; }
            set { m_ci_8_key_text = value; modify("ci_8_key_text"); }
        }

        protected string m_ci_8_assess_text;
        [DBField("ci_8_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_8_assess_text
        {
            get { return m_ci_8_assess_text; }
            set { m_ci_8_assess_text = value; modify("ci_8_assess_text"); }
        }
        protected string m_ci_8_text3;
        [DBField("ci_8_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_8_text3
        {
            get { return m_ci_8_text3; }
            set { m_ci_8_text3 = value; modify("ci_8_text3"); }
        }

        protected string m_ci_8_text4;
        [DBField("ci_8_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_8_text4
        {
            get { return m_ci_8_text4; }
            set { m_ci_8_text4 = value; modify("ci_8_text4"); }
        }

        protected string m_ci_9_key_text;
        [DBField("ci_9_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_9_key_text
        {
            get { return m_ci_9_key_text; }
            set { m_ci_9_key_text = value; modify("ci_9_key_text"); }
        }

        protected string m_ci_9_assess_text;
        [DBField("ci_9_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_9_assess_text
        {
            get { return m_ci_9_assess_text; }
            set { m_ci_9_assess_text = value; modify("ci_9_assess_text"); }
        }

        protected string m_ci_9_text3;
        [DBField("ci_9_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_9_text3
        {
            get { return m_ci_9_text3; }
            set { m_ci_9_text3 = value; modify("ci_9_text3"); }
        }

        protected string m_ci_9_text4;
        [DBField("ci_9_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_9_text4
        {
            get { return m_ci_9_text4; }
            set { m_ci_9_text4 = value; modify("ci_9_text4"); }
        }
        
        protected string m_ci_10_key_text;
        [DBField("ci_10_key_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_10_key_text
        {
            get { return m_ci_10_key_text; }
            set { m_ci_10_key_text = value; modify("ci_10_key_text"); }
        }

        protected string m_ci_10_assess_text;
        [DBField("ci_10_assess_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_10_assess_text
        {
            get { return m_ci_10_assess_text; }
            set { m_ci_10_assess_text = value; modify("ci_10_assess_text"); }
        }

        protected string m_ci_10_text3;
        [DBField("ci_10_text3"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_10_text3
        {
            get { return m_ci_10_text3; }
            set { m_ci_10_text3 = value; modify("ci_10_text3"); }
        }

        protected string m_ci_10_text4;
        [DBField("ci_10_text4"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string ci_10_text4
        {
            get { return m_ci_10_text4; }
            set { m_ci_10_text4 = value; modify("ci_10_text4"); }
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

        protected string m_dii_text;
        [DBField("dii_text"), TextSearch, DBAESEncryptStringField, Export(false)]
        public string dii_text
        {
            get { return m_dii_text; }
            set { m_dii_text = value; modify("dii_text"); }
        }
    }
}
