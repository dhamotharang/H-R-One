using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpQualification")]
    public class EEmpQualification : BaseObjectWithRecordInfo
    {
        public static DBManager db = new DBManagerWithRecordInfo(typeof(EEmpQualification));
        public static WFValueList VLLearningMethod = new AppUtils.NewWFTextList(new string[] { "OC", "LD" }, new string[] { "On-Campus", "Distance Learning" });

        public const string LEARNING_METHOD_CODE_ONCAMPUS = "OC";
        public const string LEARNING_METHOD_CODE_DISTANCE_LEARNING = "LD";
        public const string LEARNING_METHOD_DESC_ONCAMPUS = "On-Campus";
        public const string LEARNING_METHOD_DESC_DISTANCE_LEARNING = "Distance Learning";

        protected int m_EmpQualificationID;
        [DBField("EmpQualificationID", true, true), TextSearch, Export(false)]
        public int EmpQualificationID
        {
            get { return m_EmpQualificationID; }
            set { m_EmpQualificationID = value; modify("EmpQualificationID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_QualificationID;
        [DBField("QualificationID"), TextSearch, Export(false), Required]
        public int QualificationID
        {
            get { return m_QualificationID; }
            set { m_QualificationID = value; modify("QualificationID"); }
        }
        protected DateTime m_EmpQualificationFrom;
        [DBField("EmpQualificationFrom"), TextSearch, MaxLength(10), Export(false)]
        public DateTime EmpQualificationFrom
        {
            get { return m_EmpQualificationFrom; }
            set { m_EmpQualificationFrom = value; modify("EmpQualificationFrom"); }
        }
        protected DateTime m_EmpQualificationTo;
        [DBField("EmpQualificationTo"), TextSearch, MaxLength(10), Export(false)]
        public DateTime EmpQualificationTo
        {
            get { return m_EmpQualificationTo; }
            set { m_EmpQualificationTo = value; modify("EmpQualificationTo"); }
        }
        protected string m_EmpQualificationInstitution;
        [DBField("EmpQualificationInstitution"), TextSearch, MaxLength(100,50), Export(false)]
        public string EmpQualificationInstitution
        {
            get { return m_EmpQualificationInstitution; }
            set { m_EmpQualificationInstitution = value; modify("EmpQualificationInstitution"); }
        }
        protected string m_EmpQualificationLearningMethod;
        [DBField("EmpQualificationLearningMethod"), TextSearch, Export(false)]
        public string EmpQualificationLearningMethod
        {
            get { return m_EmpQualificationLearningMethod; }
            set { m_EmpQualificationLearningMethod = value; modify("EmpQualificationLearningMethod"); }
        }
        protected string m_EmpQualificationRemark;
        [DBField("EmpQualificationRemark"), TextSearch, Export(false)]
        public string EmpQualificationRemark
        {
            get { return m_EmpQualificationRemark; }
            set { m_EmpQualificationRemark = value; modify("EmpQualificationRemark"); }
        }

        //  For Synchronize Use only
        protected string m_SynID;
        [DBField("SynID"), TextSearch, Export(false)]
        public string SynID
        {
            get { return m_SynID; }
            set { m_SynID = value; modify("SynID"); }
        }


    }
}
