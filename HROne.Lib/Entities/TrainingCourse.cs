using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("TrainingCourse")]
    public class ETrainingCourse : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ETrainingCourse));
        public static WFValueList VLTrainingCourse = new AppUtils.EncryptedDBCodeList(ETrainingCourse.db, "TrainingCourseID", new string[] { "TrainingCourseCode", "TrainingCourseName" }, " - ", "TrainingCourseCode");

        protected int m_TrainingCourseID;
        [DBField("TrainingCourseID", true, true), TextSearch, Export(false)]
        public int TrainingCourseID
        {
            get { return m_TrainingCourseID; }
            set { m_TrainingCourseID = value; modify("TrainingCourseID"); }
        }
        protected string m_TrainingCourseCode;
        [DBField("TrainingCourseCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string TrainingCourseCode
        {
            get { return m_TrainingCourseCode; }
            set { m_TrainingCourseCode = value; modify("TrainingCourseCode"); }
        }
        protected string m_TrainingCourseName;
        [DBField("TrainingCourseName"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string TrainingCourseName
        {
            get { return m_TrainingCourseName; }
            set { m_TrainingCourseName = value; modify("TrainingCourseName"); }
        }
    }
}
