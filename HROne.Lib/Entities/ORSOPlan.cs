using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("ORSOPlan")]
    public class EORSOPlan : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EORSOPlan));
        public static WFValueList VLORSOPlan = new WFDBCodeList(EORSOPlan.db, "ORSOPlanID", "ORSOPlanCode", "ORSOPlanDesc", "ORSOPlanCode");

        public static EORSOPlan GetObject(DatabaseConnection dbConn, int ID)
        {
            if (ID > 0)
            {
                EORSOPlan obj = new EORSOPlan();
                obj.ORSOPlanID = ID;
                if (EORSOPlan.db.select(dbConn, obj))
                {
                    return obj;
                }
            }
            return null;
        }

        protected int m_ORSOPlanID;
        [DBField("ORSOPlanID", true, true), TextSearch, Export(false)]
        public int ORSOPlanID
        {
            get { return m_ORSOPlanID; }
            set { m_ORSOPlanID = value; modify("ORSOPlanID"); }
        }
        protected string m_ORSOPlanCode;
        [DBField("ORSOPlanCode"), TextSearch,MaxLength(20,20), Export(false),Required]
        public string ORSOPlanCode
        {
            get { return m_ORSOPlanCode; }
            set { m_ORSOPlanCode = value; modify("ORSOPlanCode"); }
        }
        protected string m_ORSOPlanDesc;
        [DBField("ORSOPlanDesc"), TextSearch, MaxLength(100, 50), Export(false), Required]
        public string ORSOPlanDesc
        {
            get { return m_ORSOPlanDesc; }
            set { m_ORSOPlanDesc = value; modify("ORSOPlanDesc"); }
        }

        protected string m_ORSOPlanSchemeNo;
        [DBField("ORSOPlanSchemeNo"), TextSearch, MaxLength(100, 50), Export(false), Required]
        public string ORSOPlanSchemeNo
        {
            get { return m_ORSOPlanSchemeNo; }
            set { m_ORSOPlanSchemeNo = value; modify("ORSOPlanSchemeNo"); }
        }

        protected string m_ORSOPlanCompanyName;
        [DBField("ORSOPlanCompanyName"), TextSearch, MaxLength(100, 50), Export(false), Required]
        public string ORSOPlanCompanyName
        {
            get { return m_ORSOPlanCompanyName; }
            set { m_ORSOPlanCompanyName = value; modify("ORSOPlanCompanyName"); }
        }

        protected string m_ORSOPlanPayCenter;
        [DBField("ORSOPlanPayCenter"), TextSearch, MaxLength(20, 20), Export(false)]
        public string ORSOPlanPayCenter
        {
            get { return m_ORSOPlanPayCenter; }
            set { m_ORSOPlanPayCenter = value; modify("ORSOPlanPayCenter"); }
        }

        protected double m_ORSOPlanMaxEmployerVC;
        [DBField("ORSOPlanMaxEmployerVC", "#,##0.00"), TextSearch, MaxLength(13), Export(false)]
        public double ORSOPlanMaxEmployerVC
        {
            get { return m_ORSOPlanMaxEmployerVC; }
            set { m_ORSOPlanMaxEmployerVC = value; modify("ORSOPlanMaxEmployerVC"); }
        }
        protected double m_ORSOPlanMaxEmployeeVC;
        [DBField("ORSOPlanMaxEmployeeVC", "#,##0.00"), TextSearch, MaxLength(13), Export(false)]
        public double ORSOPlanMaxEmployeeVC
        {
            get { return m_ORSOPlanMaxEmployeeVC; }
            set { m_ORSOPlanMaxEmployeeVC = value; modify("ORSOPlanMaxEmployeeVC"); }
        }

        protected int m_ORSOPlanEmployerDecimalPlace;
        [DBField("ORSOPlanEmployerDecimalPlace"), TextSearch, Export(false), Required]
        public int ORSOPlanEmployerDecimalPlace
        {
            get { return m_ORSOPlanEmployerDecimalPlace; }
            set { m_ORSOPlanEmployerDecimalPlace = value; modify("ORSOPlanEmployerDecimalPlace"); }
        }

        protected string m_ORSOPlanEmployerRoundingRule;
        [DBField("ORSOPlanEmployerRoundingRule"), TextSearch, MaxLength(50, 25), Export(false), Required]
        public string ORSOPlanEmployerRoundingRule
        {
            get { return m_ORSOPlanEmployerRoundingRule; }
            set { m_ORSOPlanEmployerRoundingRule = value; modify("ORSOPlanEmployerRoundingRule"); }
        }

        protected int m_ORSOPlanEmployeeDecimalPlace;
        [DBField("ORSOPlanEmployeeDecimalPlace"), TextSearch, Export(false), Required]
        public int ORSOPlanEmployeeDecimalPlace
        {
            get { return m_ORSOPlanEmployeeDecimalPlace; }
            set { m_ORSOPlanEmployeeDecimalPlace = value; modify("ORSOPlanEmployeeDecimalPlace"); }
        }

        protected string m_ORSOPlanEmployeeRoundingRule;
        [DBField("ORSOPlanEmployeeRoundingRule"), TextSearch, MaxLength(50, 25), Export(false), Required]
        public string ORSOPlanEmployeeRoundingRule
        {
            get { return m_ORSOPlanEmployeeRoundingRule; }
            set { m_ORSOPlanEmployeeRoundingRule = value; modify("ORSOPlanEmployeeRoundingRule"); }
        }

        protected bool m_ORSOPlanEmployerResidual;
        [DBField("ORSOPlanEmployerResidual"), TextSearch, Export(false)]
        public bool ORSOPlanEmployerResidual
        {
            get { return m_ORSOPlanEmployerResidual; }
            set { m_ORSOPlanEmployerResidual = value; modify("ORSOPlanEmployerResidual"); }
        }

        protected bool m_ORSOPlanEmployeeResidual;
        [DBField("ORSOPlanEmployeeResidual"), TextSearch, Export(false)]
        public bool ORSOPlanEmployeeResidual
        {
            get { return m_ORSOPlanEmployeeResidual; }
            set { m_ORSOPlanEmployeeResidual = value; modify("ORSOPlanEmployeeResidual"); }
        }

        protected double m_ORSOPlanEmployerResidualCap;
        [DBField("ORSOPlanEmployerResidualCap", "#,##0.00"), TextSearch, Export(false)]
        public double ORSOPlanEmployerResidualCap
        {
            get { return m_ORSOPlanEmployerResidualCap; }
            set { m_ORSOPlanEmployerResidualCap = value; modify("ORSOPlanEmployerResidualCap"); }
        }

        protected double m_ORSOPlanEmployeeResidualCap;
        [DBField("ORSOPlanEmployeeResidualCap", "#,##0.00"), TextSearch, Export(false)]
        public double ORSOPlanEmployeeResidualCap
        {
            get { return m_ORSOPlanEmployeeResidualCap; }
            set { m_ORSOPlanEmployeeResidualCap = value; modify("ORSOPlanEmployeeResidualCap"); }
        }

        public EORSOPlanDetail GetORSOPlanDetail(DatabaseConnection dbConn, double YearOfService)
        {
            DBFilter orsoPlanDetailFilter = new DBFilter();
            orsoPlanDetailFilter.add(new Match("ORSOPlanID", ORSOPlanID));
            orsoPlanDetailFilter.add(new Match("ORSOPlanDetailYearOfService", ">=", YearOfService));
            orsoPlanDetailFilter.add("ORSOPlanDetailYearOfService", true);
            System.Collections.ArrayList orsoPlanDetailList = EORSOPlanDetail.db.select(dbConn, orsoPlanDetailFilter);
            if (orsoPlanDetailList.Count > 0)
                return (EORSOPlanDetail)orsoPlanDetailList[0];
            else
                return null;

        }
    }


}
