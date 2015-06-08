using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("TrainingSeminar")]
    public class ETrainingSeminar : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ETrainingSeminar));
        public static WFValueList VLTrainingDurationUnit = new WFTextList(new string[] { "H" }, new string[] { "Hour(s)" });

        protected int m_TrainingSeminarID;
        [DBField("TrainingSeminarID", true, true), TextSearch, Export(false)]
        public int TrainingSeminarID
        {
            get { return m_TrainingSeminarID; }
            set { m_TrainingSeminarID = value; modify("TrainingSeminarID"); }
        }
        protected int m_TrainingCourseID;
        [DBField("TrainingCourseID"), TextSearch, Export(false), Required]
        public int TrainingCourseID
        {
            get { return m_TrainingCourseID; }
            set { m_TrainingCourseID = value; modify("TrainingCourseID"); }
        }
        protected string m_TrainingSeminarDesc;
        [DBField("TrainingSeminarDesc"), MaxLength(200, 75), TextSearch, Export(false)]
        public string TrainingSeminarDesc
        {
            get { return m_TrainingSeminarDesc; }
            set { m_TrainingSeminarDesc = value; modify("TrainingSeminarDesc"); }
        }
        protected DateTime m_TrainingSeminarDateFrom;
        [DBField("TrainingSeminarDateFrom"), TextSearch, Export(false), Required]
        public DateTime TrainingSeminarDateFrom
        {
            get { return m_TrainingSeminarDateFrom; }
            set { m_TrainingSeminarDateFrom = value; modify("TrainingSeminarDateFrom"); }
        }
        protected DateTime m_TrainingSeminarDateTo;
        [DBField("TrainingSeminarDateTo"), TextSearch, Export(false), Required]
        public DateTime TrainingSeminarDateTo
        {
            get { return m_TrainingSeminarDateTo; }
            set { m_TrainingSeminarDateTo = value; modify("TrainingSeminarDateTo"); }
        }
        protected double m_TrainingSeminarDuration;
        [DBField("TrainingSeminarDuration","0.00"), MaxLength(5, 5), TextSearch, Export(false)]
        public double TrainingSeminarDuration
        {
            get { return m_TrainingSeminarDuration; }
            set { m_TrainingSeminarDuration = value; modify("TrainingSeminarDuration"); }
        }
        protected string m_TrainingSeminarDurationUnit;
        [DBField("TrainingSeminarDurationUnit"), TextSearch, Export(false)]
        public string TrainingSeminarDurationUnit
        {
            get { return m_TrainingSeminarDurationUnit; }
            set { m_TrainingSeminarDurationUnit = value; modify("TrainingSeminarDurationUnit"); }
        }
        protected string m_TrainingSeminarTrainer;
        [DBField("TrainingSeminarTrainer"), MaxLength(200,75), TextSearch, Export(false) ]
        public string TrainingSeminarTrainer
        {
            get { return m_TrainingSeminarTrainer; }
            set { m_TrainingSeminarTrainer = value; modify("TrainingSeminarTrainer"); }
        }
    }
}
