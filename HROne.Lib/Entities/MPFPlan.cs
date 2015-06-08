using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("MPFPlan")]
    public class EMPFPlan : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EMPFPlan));
        public static WFValueList VLMPFPlan = new WFDBCodeList(EMPFPlan.db, "MPFPlanID", "MPFPlanCode", "MPFPlanDesc", "MPFPlanCode");

        public static EMPFPlan GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EMPFPlan obj = new EMPFPlan();
                obj.MPFPlanID = ID;
                if (EMPFPlan.db.select(dbConn, obj))
                {
                    return obj;
                }
            }
            return null;
        }

        protected int m_MPFPlanID;
        [DBField("MPFPlanID", true, true), TextSearch, Export(false)]
        public int MPFPlanID
        {
            get { return m_MPFPlanID; }
            set { m_MPFPlanID = value; modify("MPFPlanID"); }
        }
        protected string m_MPFPlanCode;
        [DBField("MPFPlanCode"), TextSearch,MaxLength(20), Export(false), Required]
        public string MPFPlanCode
        {
            get { return m_MPFPlanCode; }
            set { m_MPFPlanCode = value; modify("MPFPlanCode"); }
        }
        protected string m_MPFPlanDesc;
        [DBField("MPFPlanDesc"), TextSearch, MaxLength(100, 40), Export(false), Required]
        public string MPFPlanDesc
        {
            get { return m_MPFPlanDesc; }
            set { m_MPFPlanDesc = value; modify("MPFPlanDesc"); }
        }
        //protected string m_MPFPlanTrusteeName;
        //[DBField("MPFPlanTrusteeName"), TextSearch, MaxLength(100, 25), Export(false)]
        //public string MPFPlanTrusteeName
        //{
        //    get { return m_MPFPlanTrusteeName; }
        //    set { m_MPFPlanTrusteeName = value; modify("MPFPlanTrusteeName"); }
        //}
        //protected string m_MPFPlanSchemeNo;
        //[DBField("MPFPlanSchemeNo"), TextSearch, MaxLength(20, 25), Export(false)]
        //public string MPFPlanSchemeNo
        //{
        //    get { return m_MPFPlanSchemeNo; }
        //    set { m_MPFPlanSchemeNo = value; modify("MPFPlanSchemeNo"); }
        //}
        protected string m_MPFPlanCompanyName;
        [DBField("MPFPlanCompanyName"), TextSearch, MaxLength(100, 40), Export(false)]
        public string MPFPlanCompanyName
        {
            get { return m_MPFPlanCompanyName; }
            set { m_MPFPlanCompanyName = value; modify("MPFPlanCompanyName"); }
        }
        protected string m_MPFPlanCompanyAddress;
        [DBField("MPFPlanCompanyAddress"), TextSearch, MaxLength(200), Export(false)]
        public string MPFPlanCompanyAddress
        {
            get { return m_MPFPlanCompanyAddress; }
            set { m_MPFPlanCompanyAddress = value; modify("MPFPlanCompanyAddress"); }
        }
        protected string m_MPFPlanContactName;
        [DBField("MPFPlanContactName"), TextSearch, MaxLength(100, 40), Export(false)]
        public string MPFPlanContactName
        {
            get { return m_MPFPlanContactName; }
            set { m_MPFPlanContactName = value; modify("MPFPlanContactName"); }
        }
        protected string m_MPFPlanContactNo;
        [DBField("MPFPlanContactNo"), TextSearch, MaxLength(20), Export(false)]
        public string MPFPlanContactNo
        {
            get { return m_MPFPlanContactNo; }
            set { m_MPFPlanContactNo = value; modify("MPFPlanContactNo"); }
        }
        protected int m_MPFSchemeID;
        [DBField("MPFSchemeID"), TextSearch, Export(false), Required]
        public int MPFSchemeID
        {
            get { return m_MPFSchemeID; }
            set { m_MPFSchemeID = value; modify("MPFSchemeID"); }
        }
        protected string m_MPFPlanParticipationNo;
        [DBField("MPFPlanParticipationNo"), TextSearch, MaxLength(50, 40), Export(false)]
        public string MPFPlanParticipationNo
        {
            get { return m_MPFPlanParticipationNo; }
            set { m_MPFPlanParticipationNo = value; modify("MPFPlanParticipationNo"); }
        }

        protected int m_MPFPlanEmployerDecimalPlace;
        [DBField("MPFPlanEmployerDecimalPlace"), TextSearch, Export(false), Required]
        public int MPFPlanEmployerDecimalPlace
        {
            get { return m_MPFPlanEmployerDecimalPlace; }
            set { m_MPFPlanEmployerDecimalPlace = value; modify("MPFPlanEmployerDecimalPlace"); }
        }

        protected string m_MPFPlanEmployerRoundingRule;
        [DBField("MPFPlanEmployerRoundingRule"), TextSearch, Export(false), Required]
        public string MPFPlanEmployerRoundingRule
        {
            get { return m_MPFPlanEmployerRoundingRule; }
            set { m_MPFPlanEmployerRoundingRule = value; modify("MPFPlanEmployerRoundingRule"); }
        }

        protected int m_MPFPlanEmployeeDecimalPlace;
        [DBField("MPFPlanEmployeeDecimalPlace"), TextSearch, Export(false), Required]
        public int MPFPlanEmployeeDecimalPlace
        {
            get { return m_MPFPlanEmployeeDecimalPlace; }
            set { m_MPFPlanEmployeeDecimalPlace = value; modify("MPFPlanEmployeeDecimalPlace"); }
        }

        protected string m_MPFPlanEmployeeRoundingRule;
        [DBField("MPFPlanEmployeeRoundingRule"), TextSearch, Export(false), Required]
        public string MPFPlanEmployeeRoundingRule
        {
            get { return m_MPFPlanEmployeeRoundingRule; }
            set { m_MPFPlanEmployeeRoundingRule = value; modify("MPFPlanEmployeeRoundingRule"); }
        }
        protected string m_MPFPlanExtendData;
        [DBField("MPFPlanExtendData"), TextSearch, Export(false)]
        public string MPFPlanExtendData
        {
            get { return m_MPFPlanExtendData; }
            set { m_MPFPlanExtendData = value; modify("MPFPlanExtendData"); }
        }
    }
}
