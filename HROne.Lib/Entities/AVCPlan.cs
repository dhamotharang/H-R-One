using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("AVCPlan")]
    public class EAVCPlan : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAVCPlan));
        public static WFValueList VLAVCPlan = new WFDBCodeList(EAVCPlan.db, "AVCPlanID", "AVCPlanCode", "AVCPlanDesc", "AVCPlanCode");

        protected int m_AVCPlanID;
        [DBField("AVCPlanID", true, true), TextSearch, Export(false)]
        public int AVCPlanID
        {
            get { return m_AVCPlanID; }
            set { m_AVCPlanID = value; modify("AVCPlanID"); }
        }
        protected string m_AVCPlanCode;
        [DBField("AVCPlanCode"), TextSearch,MaxLength(20,20), Export(false),Required]
        public string AVCPlanCode
        {
            get { return m_AVCPlanCode; }
            set { m_AVCPlanCode = value; modify("AVCPlanCode"); }
        }
        protected string m_AVCPlanDesc;
        [DBField("AVCPlanDesc"), TextSearch, MaxLength(100, 50), Export(false), Required]
        public string AVCPlanDesc
        {
            get { return m_AVCPlanDesc; }
            set { m_AVCPlanDesc = value; modify("AVCPlanDesc"); }
        }
        protected bool m_AVCPlanEmployerResidual;
        [DBField("AVCPlanEmployerResidual"), TextSearch, Export(false)]
        public bool AVCPlanEmployerResidual
        {
            get { return m_AVCPlanEmployerResidual; }
            set { m_AVCPlanEmployerResidual = value; modify("AVCPlanEmployerResidual"); }
        }
        protected double m_AVCPlanEmployerResidualCap;
        [DBField("AVCPlanEmployerResidualCap", format="#,##0.00"), TextSearch, Export(false)]
        public double AVCPlanEmployerResidualCap
        {
            get { return m_AVCPlanEmployerResidualCap; }
            set { m_AVCPlanEmployerResidualCap = value; modify("AVCPlanEmployerResidualCap"); }
        }
        protected bool m_AVCPlanEmployeeResidual;
        [DBField("AVCPlanEmployeeResidual"), TextSearch, Export(false)]
        public bool AVCPlanEmployeeResidual
        {
            get { return m_AVCPlanEmployeeResidual; }
            set { m_AVCPlanEmployeeResidual = value; modify("AVCPlanEmployeeResidual"); }
        }
        protected double m_AVCPlanEmployeeResidualCap;
        [DBField("AVCPlanEmployeeResidualCap", format = "#,##0.00"), TextSearch, Export(false)]
        public double AVCPlanEmployeeResidualCap
        {
            get { return m_AVCPlanEmployeeResidualCap; }
            set { m_AVCPlanEmployeeResidualCap = value; modify("AVCPlanEmployeeResidualCap"); }
        }     
        
        protected bool m_AVCPlanUseMPFExemption;
        [DBField("AVCPlanUseMPFExemption"), TextSearch, Export(false)]
        public bool AVCPlanUseMPFExemption
        {
            get { return m_AVCPlanUseMPFExemption; }
            set { m_AVCPlanUseMPFExemption = value; modify("AVCPlanUseMPFExemption"); }
        }
        protected bool m_AVCPlanJoinDateStart;
        [DBField("AVCPlanJoinDateStart"), TextSearch, Export(false)]
        public bool AVCPlanJoinDateStart
        {
            get { return m_AVCPlanJoinDateStart; }
            set { m_AVCPlanJoinDateStart = value; modify("AVCPlanJoinDateStart"); }
        }
        protected bool m_AVCPlanContributeMaxAge;
        [DBField("AVCPlanContributeMaxAge"), TextSearch, Export(false)]
        public bool AVCPlanContributeMaxAge
        {
            get { return m_AVCPlanContributeMaxAge; }
            set { m_AVCPlanContributeMaxAge = value; modify("AVCPlanContributeMaxAge"); }
        }
        protected bool m_AVCPlanContributeMinRI;
        [DBField("AVCPlanContributeMinRI"), TextSearch, Export(false)]
        public bool AVCPlanContributeMinRI
        {
            get { return m_AVCPlanContributeMinRI; }
            set { m_AVCPlanContributeMinRI = value; modify("AVCPlanContributeMinRI"); }
        }
        protected double m_AVCPlanMaxEmployerVC;
        [DBField("AVCPlanMaxEmployerVC","#,##0.00"), TextSearch, MaxLength(13), Export(false)]
        public double AVCPlanMaxEmployerVC
        {
            get { return m_AVCPlanMaxEmployerVC; }
            set { m_AVCPlanMaxEmployerVC = value; modify("AVCPlanMaxEmployerVC"); }
        }
        protected double m_AVCPlanMaxEmployeeVC;
        [DBField("AVCPlanMaxEmployeeVC", "#,##0.00"), TextSearch, MaxLength(13), Export(false)]
        public double AVCPlanMaxEmployeeVC
        {
            get { return m_AVCPlanMaxEmployeeVC; }
            set { m_AVCPlanMaxEmployeeVC = value; modify("AVCPlanMaxEmployeeVC"); }
        }
        protected int m_AVCPlanEmployerDecimalPlace;
        [DBField("AVCPlanEmployerDecimalPlace"), TextSearch, Export(false), Required]
        public int AVCPlanEmployerDecimalPlace
        {
            get { return m_AVCPlanEmployerDecimalPlace; }
            set { m_AVCPlanEmployerDecimalPlace = value; modify("AVCPlanEmployerDecimalPlace"); }
        }

        protected string m_AVCPlanEmployerRoundingRule;
        [DBField("AVCPlanEmployerRoundingRule"), TextSearch, MaxLength(50, 25), Export(false), Required]
        public string AVCPlanEmployerRoundingRule
        {
            get { return m_AVCPlanEmployerRoundingRule; }
            set { m_AVCPlanEmployerRoundingRule = value; modify("AVCPlanEmployerRoundingRule"); }
        }

        protected int m_AVCPlanEmployeeDecimalPlace;
        [DBField("AVCPlanEmployeeDecimalPlace"), TextSearch, Export(false), Required]
        public int AVCPlanEmployeeDecimalPlace
        {
            get { return m_AVCPlanEmployeeDecimalPlace; }
            set { m_AVCPlanEmployeeDecimalPlace = value; modify("AVCPlanEmployeeDecimalPlace"); }
        }

        protected string m_AVCPlanEmployeeRoundingRule;
        [DBField("AVCPlanEmployeeRoundingRule"), TextSearch, MaxLength(50, 25), Export(false), Required]
        public string AVCPlanEmployeeRoundingRule
        {
            get { return m_AVCPlanEmployeeRoundingRule; }
            set { m_AVCPlanEmployeeRoundingRule = value; modify("AVCPlanEmployeeRoundingRule"); }
        }

        protected bool m_AVCPlanNotRemoveContributionFromTopUp;
        [DBField("AVCPlanNotRemoveContributionFromTopUp"), TextSearch, Export(false), Required]
        public bool AVCPlanNotRemoveContributionFromTopUp
        {
            get { return m_AVCPlanNotRemoveContributionFromTopUp; }
            set { m_AVCPlanNotRemoveContributionFromTopUp = value; modify("AVCPlanNotRemoveContributionFromTopUp"); }
        }

        public EAVCPlanDetail GetAVCPlanDetail(DatabaseConnection dbConn, double YearOfService)
        {
            DBFilter avcPlanDetailFilter = new DBFilter();
            avcPlanDetailFilter.add(new Match("AVCPlanID", AVCPlanID));
            avcPlanDetailFilter.add(new Match("AVCPlanDetailYearOfService", ">=", YearOfService));
            avcPlanDetailFilter.add("AVCPlanDetailYearOfService", true);
            System.Collections.ArrayList avcPlanDetailList = EAVCPlanDetail.db.select(dbConn, avcPlanDetailFilter);
            if (avcPlanDetailList.Count > 0)
                return (EAVCPlanDetail)avcPlanDetailList[0];
            else
                return null;

        }
    }

    [DBClass("AVCPlanPaymentCeiling")]
    public class EAVCPlanPaymentCeiling : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAVCPlanPaymentCeiling));

        protected int m_AVCPlanPaymentCeilingID;
        [DBField("AVCPlanPaymentCeilingID", true, true), TextSearch, Export(false)]
        public int AVCPlanPaymentCeilingID
        {
            get { return m_AVCPlanPaymentCeilingID; }
            set { m_AVCPlanPaymentCeilingID = value; modify("AVCPlanPaymentCeilingID"); }
        }

        protected int m_AVCPlanID;
        [DBField("AVCPlanID"), TextSearch, Export(false), Required]
        public int AVCPlanID
        {
            get { return m_AVCPlanID; }
            set { m_AVCPlanID = value; modify("AVCPlanID"); }
        }

        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID"), TextSearch, Export(false), Required]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }

        protected double m_AVCPlanPaymentCeilingAmount;
        [DBField("AVCPlanPaymentCeilingAmount", "#,##0.00"), TextSearch, Export(false), Required]
        public double AVCPlanPaymentCeilingAmount
        {
            get { return m_AVCPlanPaymentCeilingAmount; }
            set { m_AVCPlanPaymentCeilingAmount = value; modify("AVCPlanPaymentCeilingAmount"); }
        }        
    }

    [DBClass("AVCPlanPaymentConsider")]
    public class EAVCPlanPaymentConsider : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EAVCPlanPaymentConsider));

        protected int m_AVCPlanPaymentConsiderID;
        [DBField("AVCPlanPaymentConsiderID", true, true), TextSearch, Export(false)]
        public int AVCPlanPaymentConsiderID
        {
            get { return m_AVCPlanPaymentConsiderID; }
            set { m_AVCPlanPaymentConsiderID = value; modify("m_AVCPlanPaymentConsiderID"); }
        }

        protected int m_AVCPlanID;
        [DBField("AVCPlanID"), TextSearch, Export(false), Required]
        public int AVCPlanID
        {
            get { return m_AVCPlanID; }
            set { m_AVCPlanID = value; modify("AVCPlanID"); }
        }

        protected int m_PaymentCodeID;
        [DBField("PaymentCodeID"), TextSearch, Export(false), Required]
        public int PaymentCodeID
        {
            get { return m_PaymentCodeID; }
            set { m_PaymentCodeID = value; modify("PaymentCodeID"); }
        }

        protected bool m_AVCPlanPaymentConsiderAfterMPF;
        [DBField("AVCPlanPaymentConsiderAfterMPF"), TextSearch, Export(false), Required]
        public bool AVCPlanPaymentConsiderAfterMPF
        {
            get { return m_AVCPlanPaymentConsiderAfterMPF; }
            set { m_AVCPlanPaymentConsiderAfterMPF = value; modify("AVCPlanPaymentConsiderAfterMPF;"); }
        }


    }
}
