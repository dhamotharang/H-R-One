using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
namespace HROne.Lib.Entities
{
    [DBClass("EmpRPWinson")]
    public class EEmpRPWinson : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EEmpRPWinson));

        public static EEmpRPWinson GetObjectByRPID(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                DBFilter m_filter = new DBFilter();
                m_filter.add(new Match("EmpRPID", ID));
                ArrayList m_list = EEmpRPWinson.db.select(dbConn, m_filter);
                if (m_list.Count > 0)
                {
                    return (EEmpRPWinson) m_list[0];
                }
            }
            return null;
        }       
        


        protected int m_EmpRPWinsonID;
        [DBField("EmpRPWinsonID", true, true), TextSearch, Export(false)]
        public int EmpRPWinsonID
        {
            get { return m_EmpRPWinsonID; }
            set { m_EmpRPWinsonID = value; modify("EmpRPWinsonID"); }
        }
        protected int m_EmpRPID;
        [DBField("EmpRPID"), TextSearch, Export(false), Required]
        public int EmpRPID
        {
            get { return m_EmpRPID; }
            set { m_EmpRPID = value; modify("EmpRPID"); }
        }
        protected int m_EmpRPShiftDutyID;
        [DBField("EmpRPShiftDutyID"), TextSearch, Export(false), Required]
        public int EmpRPShiftDutyID
        {
            get { return m_EmpRPShiftDutyID; }
            set { m_EmpRPShiftDutyID = value; modify("EmpRPShiftDutyID"); }
        }
        protected int m_EmpRPPayCalFormulaID;
        [DBField("EmpRPPayCalFormulaID"), TextSearch, Export(false), Required]
        public int EmpRPPayCalFormulaID
        {
            get { return m_EmpRPPayCalFormulaID; }
            set { m_EmpRPPayCalFormulaID = value; modify("EmpRPPayCalFormulaID"); }
        }
    }
}
