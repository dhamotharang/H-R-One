using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("EmpTrainingEnroll")]
    public class EEmpTrainingEnroll : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpTrainingEnroll));

        protected int m_EmpTrainingEnrollID;
        [DBField("EmpTrainingEnrollID", true, true), TextSearch, Export(false)]
        public int EmpTrainingEnrollID
        {
            get { return m_EmpTrainingEnrollID; }
            set { m_EmpTrainingEnrollID = value; modify("EmpTrainingEnrollID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false), Required]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_TrainingSeminarID;
        [DBField("TrainingSeminarID"), TextSearch, Export(false), Required]
        public int TrainingSeminarID
        {
            get { return m_TrainingSeminarID; }
            set { m_TrainingSeminarID = value; modify("TrainingSeminarID"); }
        }
    }
}
