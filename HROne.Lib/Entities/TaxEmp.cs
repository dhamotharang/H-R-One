using System;
using System.Collections;
using HROne.DataAccess;
////using perspectivemind.validation;
using HROne.Common;

namespace HROne.Lib.Entities
{

    public class TaxEmpCompareByName : IComparer
    {
        bool m_ascending = true;
        public TaxEmpCompareByName(bool ascending)
        {
            m_ascending = ascending;
        }

        int IComparer.Compare(object x, object y)
        {
            ETaxEmp taxEmpX = (ETaxEmp)x;
            ETaxEmp taxEmpY = (ETaxEmp)y;
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return (m_ascending) ? -1 : 1;
            }
            else if (x != null && y == null)
            {
                return (m_ascending) ? 1 : -1;
            }
            else
            {
                if (m_ascending)
                {
                    int result = string.Compare(taxEmpX.TaxEmpSurname, taxEmpY.TaxEmpSurname, StringComparison.CurrentCultureIgnoreCase);
                    if (result.Equals(0))
                    {
                        result = string.Compare(taxEmpX.TaxEmpOtherName, taxEmpY.TaxEmpOtherName, StringComparison.CurrentCultureIgnoreCase);
                    }
                    return result;
                }
                else
                {
                    int result = string.Compare(taxEmpY.TaxEmpSurname, taxEmpX.TaxEmpSurname, StringComparison.CurrentCultureIgnoreCase);
                    if (result.Equals(0))
                    {
                        result = string.Compare(taxEmpY.TaxEmpOtherName, taxEmpX.TaxEmpOtherName, StringComparison.CurrentCultureIgnoreCase);
                    }
                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Summary description for TaxEmp
    /// </summary>
    [DBClass("TaxEmp")]
    public class ETaxEmp : BaseObject
    {
        public static DBManager db = new DBManager(typeof(ETaxEmp));

        public static WFValueList VLTaxMaritalStatus = new AppUtils.NewWFTextList(new string[] { "1", "2" }, new string[] { "Single/Widowed/Divorced/Living Apart", "Married" });
        public static WFValueList VLTaxYesNo = new AppUtils.NewWFTextList(new string[] { "N", "Y" }, new string[] { "No", "Yes" });
        public static WFValueList VLTaxDepartureReason = new AppUtils.NewWFTextList(new string[] { "Expatriate", "Secondment", "Emigration", "Other" }, new string[] { "Expatriate staff return to home country", "Secondment", "Emigration", "Other" });

        protected int m_TaxEmpID;
        [DBField("TaxEmpID", true, true), TextSearch, Export(false)]
        public int TaxEmpID
        {
            get { return m_TaxEmpID; }
            set { m_TaxEmpID = value; modify("TaxEmpID"); }
        }
        protected int m_TaxFormID;
        [DBField("TaxFormID"), TextSearch, Export(false), Required]
        public int TaxFormID
        {
            get { return m_TaxFormID; }
            set { m_TaxFormID = value; modify("TaxFormID"); }
        }
        protected int m_EmpID;
        [DBField("EmpID"), TextSearch, Export(false)]
        public int EmpID
        {
            get { return m_EmpID; }
            set { m_EmpID = value; modify("EmpID"); }
        }
        protected int m_TaxEmpSheetNo;
        [DBField("TaxEmpSheetNo"), TextSearch, Export(false)]
        public int TaxEmpSheetNo
        {
            get { return m_TaxEmpSheetNo; }
            set { m_TaxEmpSheetNo = value; modify("TaxEmpSheetNo"); }
        }
        protected string m_TaxEmpHKID;
        [DBField("TaxEmpHKID"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(12)]
        public string TaxEmpHKID
        {
            get { return m_TaxEmpHKID; }
            set { m_TaxEmpHKID = value; modify("TaxEmpHKID"); }
        }
        protected string m_TaxEmpStatus;
        [DBField("TaxEmpStatus"), TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpStatus
        {
            get { return m_TaxEmpStatus; }
            set { m_TaxEmpStatus = value; modify("TaxEmpStatus"); }
        }
        protected string m_TaxEmpSurname;
        [DBField("TaxEmpSurname"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(20)]
        public string TaxEmpSurname
        {
            get { return m_TaxEmpSurname; }
            set { m_TaxEmpSurname = value; modify("TaxEmpSurname"); }
        }
        protected string m_TaxEmpOtherName;
        [DBField("TaxEmpOtherName"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(55)]
        public string TaxEmpOtherName
        {
            get { return m_TaxEmpOtherName; }
            set { m_TaxEmpOtherName = value; modify("TaxEmpOtherName"); }
        }
        protected string m_TaxEmpChineseName;
        [DBField("TaxEmpChineseName"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(50)]
        public string TaxEmpChineseName
        {
            get { return m_TaxEmpChineseName; }
            set { m_TaxEmpChineseName = value; modify("TaxEmpChineseName"); }
        }
        protected string m_TaxEmpSex;
        [DBField("TaxEmpSex"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpSex
        {
            get { return m_TaxEmpSex; }
            set { m_TaxEmpSex = value; modify("TaxEmpSex"); }
        }

        protected string m_TaxEmpMartialStatus;
        [DBField("TaxEmpMartialStatus"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpMartialStatus
        {
            get { return m_TaxEmpMartialStatus; }
            set { m_TaxEmpMartialStatus = value; modify("TaxEmpMartialStatus"); }
        }
        protected string m_TaxEmpPassportNo;
        [DBField("TaxEmpPassportNo"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(40)]
        public string TaxEmpPassportNo
        {
            get { return m_TaxEmpPassportNo; }
            set { m_TaxEmpPassportNo = value; modify("TaxEmpPassportNo"); }
        }
        protected string m_TaxEmpIssuedCountry;
        [DBField("TaxEmpIssuedCountry"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(40)]
        public string TaxEmpIssuedCountry
        {
            get { return m_TaxEmpIssuedCountry; }
            set { m_TaxEmpIssuedCountry = value; modify("TaxEmpIssuedCountry"); }
        }
        protected string m_TaxEmpSpouseName;
        [DBField("TaxEmpSpouseName"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(50)]
        public string TaxEmpSpouseName
        {
            get { return m_TaxEmpSpouseName; }
            set { m_TaxEmpSpouseName = value; modify("TaxEmpSpouseName"); }
        }
        protected string m_TaxEmpSpouseHKID;
        [DBField("TaxEmpSpouseHKID"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(12)]
        public string TaxEmpSpouseHKID
        {
            get { return m_TaxEmpSpouseHKID; }
            set { m_TaxEmpSpouseHKID = value; modify("TaxEmpSpouseHKID"); }
        }
        protected string m_TaxEmpSpousePassportNo;
        [DBField("TaxEmpSpousePassportNo"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(40)]
        public string TaxEmpSpousePassportNo
        {
            get { return m_TaxEmpSpousePassportNo; }
            set { m_TaxEmpSpousePassportNo = value; modify("TaxEmpSpousePassportNo"); }
        }
        protected string m_TaxEmpSpouseIssuedCountry;
        [DBField("TaxEmpSpouseIssuedCountry"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(40)]
        public string TaxEmpSpouseIssuedCountry
        {
            get { return m_TaxEmpSpouseIssuedCountry; }
            set { m_TaxEmpSpouseIssuedCountry = value; modify("TaxEmpSpouseIssuedCountry"); }
        }
        protected string m_TaxEmpResAddr;
        [DBField("TaxEmpResAddr"), TextSearch, DBAESEncryptStringField, Export(false), MaxLengthAttribute(90)]
        public string TaxEmpResAddr
        {
            get { return m_TaxEmpResAddr; }
            set { m_TaxEmpResAddr = value; modify("TaxEmpResAddr"); }
        }
        protected string m_TaxEmpResAddrAreaCode;
        [DBField("TaxEmpResAddrAreaCode"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpResAddrAreaCode
        {
            get { return m_TaxEmpResAddrAreaCode; }
            set { m_TaxEmpResAddrAreaCode = value; modify("TaxEmpResAddrAreaCode"); }
        }
        protected string m_TaxEmpCorAddr;
        [DBField("TaxEmpCorAddr"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(60)]
        public string TaxEmpCorAddr
        {
            get { return m_TaxEmpCorAddr; }
            set { m_TaxEmpCorAddr = value; modify("TaxEmpCorAddr"); }
        }
        protected string m_TaxEmpCapacity;
        [DBField("TaxEmpCapacity"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(40)]
        public string TaxEmpCapacity
        {
            get { return m_TaxEmpCapacity; }
            set { m_TaxEmpCapacity = value; modify("TaxEmpCapacity"); }
        }
        protected string m_TaxEmpPartTimeEmployer;
        [DBField("TaxEmpPartTimeEmployer"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(30)]
        public string TaxEmpPartTimeEmployer
        {
            get { return m_TaxEmpPartTimeEmployer; }
            set { m_TaxEmpPartTimeEmployer = value; modify("TaxEmpPartTimeEmployer"); }
        }
        protected DateTime m_TaxEmpStartDate;
        [DBField("TaxEmpStartDate"), TextSearch, Export(false), Required]
        public DateTime TaxEmpStartDate
        {
            get { return m_TaxEmpStartDate; }
            set { m_TaxEmpStartDate = value; modify("TaxEmpStartDate"); }
        }
        protected DateTime m_TaxEmpEndDate;
        [DBField("TaxEmpEndDate"), TextSearch, Export(false), Required]
        public DateTime TaxEmpEndDate
        {
            get { return m_TaxEmpEndDate; }
            set { m_TaxEmpEndDate = value; modify("TaxEmpEndDate"); }
        }
        protected string m_TaxEmpCessationReason;
        [DBField("TaxEmpCessationReason"), DBAESEncryptStringField, TextSearch, Export(false), MaxLengthAttribute(100)]
        public string TaxEmpCessationReason
        {
            get { return m_TaxEmpCessationReason; }
            set { m_TaxEmpCessationReason = value; modify("TaxEmpCessationReason"); }
        }
        //  Obsolate, count the PlaceOfResidence to determine
        //protected int m_TaxEmpPlaceOfResidenceIndicator;
        //[DBField("TaxEmpPlaceOfResidenceIndicator"), TextSearch, Export(false)]
        //public int TaxEmpPlaceOfResidenceIndicator
        //{
        //    get { return m_TaxEmpPlaceOfResidenceIndicator; }
        //    set { m_TaxEmpPlaceOfResidenceIndicator = value; modify("TaxEmpPlaceOfResidenceIndicator"); }
        //}
        protected int m_TaxEmpOvearseasIncomeIndicator;
        [DBField("TaxEmpOvearseasIncomeIndicator"), TextSearch, Export(false)]
        public int TaxEmpOvearseasIncomeIndicator
        {
            get { return m_TaxEmpOvearseasIncomeIndicator; }
            set { m_TaxEmpOvearseasIncomeIndicator = value; modify("TaxEmpOvearseasIncomeIndicator"); }
        }

        // Start 0000020, KuangWei, 2014-07-22
        protected int m_TaxEmpSumWithheldIndicator;
        [DBField("TaxEmpSumWithheldIndicator"), TextSearch, Export(false)]
        public int TaxEmpSumWithheldIndicator
        {
            get { return m_TaxEmpSumWithheldIndicator; }
            set { m_TaxEmpSumWithheldIndicator = value; modify("TaxEmpSumWithheldIndicator"); }
        }

        protected string m_TaxEmpSumWithheldAmount;
        [DBField("TaxEmpSumWithheldAmount"), TextSearch, Export(false), MaxLengthAttribute(20)]
        public string TaxEmpSumWithheldAmount
        {
            get { return m_TaxEmpSumWithheldAmount; }
            set { m_TaxEmpSumWithheldAmount = value; modify("TaxEmpSumWithheldAmount"); }
        }
        // End 0000020, KuangWei, 2014-07-22

        protected string m_TaxEmpOverseasCompanyAmount;
        [DBField("TaxEmpOverseasCompanyAmount"), TextSearch, Export(false), MaxLengthAttribute(20)]
        public string TaxEmpOverseasCompanyAmount
        {
            get { return m_TaxEmpOverseasCompanyAmount; }
            set { m_TaxEmpOverseasCompanyAmount = value; modify("TaxEmpOverseasCompanyAmount"); }
        }
        protected string m_TaxEmpOverseasCompanyName;
        [DBField("TaxEmpOverseasCompanyName"), TextSearch, Export(false), MaxLengthAttribute(60)]
        public string TaxEmpOverseasCompanyName
        {
            get { return m_TaxEmpOverseasCompanyName; }
            set { m_TaxEmpOverseasCompanyName = value; modify("TaxEmpOverseasCompanyName"); }
        }
        protected string m_TaxEmpOverseasCompanyAddress;
        [DBField("TaxEmpOverseasCompanyAddress"), TextSearch, Export(false), MaxLengthAttribute(60)]
        public string TaxEmpOverseasCompanyAddress
        {
            get { return m_TaxEmpOverseasCompanyAddress; }
            set { m_TaxEmpOverseasCompanyAddress = value; modify("TaxEmpOverseasCompanyAddress"); }
        }
        protected string m_TaxEmpTaxFileNo;
        [DBField("TaxEmpTaxFileNo"), TextSearch, Export(false), MaxLengthAttribute(13)]
        public string TaxEmpTaxFileNo
        {
            get { return m_TaxEmpTaxFileNo; }
            set { m_TaxEmpTaxFileNo = value; modify("TaxEmpTaxFileNo"); }
        }
        protected string m_TaxEmpRemark;
        [DBField("TaxEmpRemark"), TextSearch, Export(false), MaxLengthAttribute(60)]
        public string TaxEmpRemark
        {
            get { return m_TaxEmpRemark; }
            set { m_TaxEmpRemark = value; modify("TaxEmpRemark"); }
        }
        protected string m_TaxEmpNewEmployerNameddress;
        [DBField("TaxEmpNewEmployerNameddress"), TextSearch, Export(false), MaxLengthAttribute(200)]
        public string TaxEmpNewEmployerNameddress
        {
            get { return m_TaxEmpNewEmployerNameddress; }
            set { m_TaxEmpNewEmployerNameddress = value; modify("TaxEmpNewEmployerNameddress"); }
        }
        protected string m_TaxEmpFutureCorAddr;
        [DBField("TaxEmpFutureCorAddr"), TextSearch, Export(false), MaxLengthAttribute(200)]
        public string TaxEmpFutureCorAddr
        {
            get { return m_TaxEmpFutureCorAddr; }
            set { m_TaxEmpFutureCorAddr = value; modify("TaxEmpFutureCorAddr"); }
        }

        protected DateTime m_TaxEmpLeaveHKDate;
        [DBField("TaxEmpLeaveHKDate"), TextSearch, Export(false)]
        public DateTime TaxEmpLeaveHKDate
        {
            get { return m_TaxEmpLeaveHKDate; }
            set { m_TaxEmpLeaveHKDate = value; modify("TaxEmpLeaveHKDate"); }
        }

        protected string m_TaxEmpIsERBearTax;
        [DBField("TaxEmpIsERBearTax"), TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpIsERBearTax
        {
            get { return m_TaxEmpIsERBearTax; }
            set { m_TaxEmpIsERBearTax = value; modify("TaxEmpIsERBearTax"); }
        }

        protected string m_TaxEmpIsMoneyHoldByOrdinance;
        [DBField("TaxEmpIsMoneyHoldByOrdinance"), TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpIsMoneyHoldByOrdinance
        {
            get { return m_TaxEmpIsMoneyHoldByOrdinance; }
            set { m_TaxEmpIsMoneyHoldByOrdinance = value; modify("TaxEmpIsMoneyHoldByOrdinance"); }
        }

        protected double m_TaxEmpHoldAmount;
        [DBField("TaxEmpHoldAmount"), TextSearch, Export(false)]
        public double TaxEmpHoldAmount
        {
            get { return m_TaxEmpHoldAmount; }
            set { m_TaxEmpHoldAmount = value; modify("TaxEmpHoldAmount"); }
        }

        protected string m_TaxEmpReasonForNotHold;
        [DBField("TaxEmpReasonForNotHold"), TextSearch, Export(false), MaxLengthAttribute(200)]
        public string TaxEmpReasonForNotHold
        {
            get { return m_TaxEmpReasonForNotHold; }
            set { m_TaxEmpReasonForNotHold = value; modify("TaxEmpReasonForNotHold"); }
        }

        protected string m_TaxEmpReasonForDepartureReason;
        [DBField("TaxEmpReasonForDepartureReason"), TextSearch, Export(false), MaxLengthAttribute(200)]
        public string TaxEmpReasonForDepartureReason
        {
            get { return m_TaxEmpReasonForDepartureReason; }
            set { m_TaxEmpReasonForDepartureReason = value; modify("TaxEmpReasonForDepartureReason"); }
        }

        protected string m_TaxEmpReasonForDepartureOtherReason;
        [DBField("TaxEmpReasonForDepartureOtherReason"), TextSearch, Export(false), MaxLengthAttribute(200)]
        public string TaxEmpReasonForDepartureOtherReason
        {
            get { return m_TaxEmpReasonForDepartureOtherReason; }
            set { m_TaxEmpReasonForDepartureOtherReason = value; modify("TaxEmpReasonForDepartureOtherReason"); }
        }

        protected string m_TaxEmpIsEEReturnHK;
        [DBField("TaxEmpIsEEReturnHK"), TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpIsEEReturnHK
        {
            get { return m_TaxEmpIsEEReturnHK; }
            set { m_TaxEmpIsEEReturnHK = value; modify("TaxEmpIsEEReturnHK"); }
        }
        protected DateTime m_TaxEmpEEReturnHKDate;
        [DBField("TaxEmpEEReturnHKDate"), TextSearch, Export(false)]
        public DateTime TaxEmpEEReturnHKDate
        {
            get { return m_TaxEmpEEReturnHKDate; }
            set { m_TaxEmpEEReturnHKDate = value; modify("TaxEmpEEReturnHKDate"); }
        }
        protected string m_TaxEmpIsShareOptionsGrant;
        [DBField("TaxEmpIsShareOptionsGrant"), TextSearch, Export(false), MaxLengthAttribute(1)]
        public string TaxEmpIsShareOptionsGrant
        {
            get { return m_TaxEmpIsShareOptionsGrant; }
            set { m_TaxEmpIsShareOptionsGrant = value; modify("TaxEmpIsShareOptionsGrant"); }
        }
        protected string m_TaxEmpShareOptionsGrantCount;
        [DBField("TaxEmpShareOptionsGrantCount"), TextSearch, Export(false), MaxLengthAttribute(200)]
        public string TaxEmpShareOptionsGrantCount
        {
            get { return m_TaxEmpShareOptionsGrantCount; }
            set { m_TaxEmpShareOptionsGrantCount = value; modify("TaxEmpShareOptionsGrantCount"); }
        }
        protected DateTime m_TaxEmpShareOptionsGrantDate;
        [DBField("TaxEmpShareOptionsGrantDate"), TextSearch, Export(false)]
        public DateTime TaxEmpShareOptionsGrantDate
        {
            get { return m_TaxEmpShareOptionsGrantDate; }
            set { m_TaxEmpShareOptionsGrantDate = value; modify("TaxEmpShareOptionsGrantDate"); }
        }

        protected string m_TaxEmpPreviousEmployerNameddress;
        [DBField("TaxEmpPreviousEmployerNameddress"), TextSearch, Export(false), MaxLengthAttribute(200)]
        public string TaxEmpPreviousEmployerNameddress
        {
            get { return m_TaxEmpPreviousEmployerNameddress; }
            set { m_TaxEmpPreviousEmployerNameddress = value; modify("TaxEmpPreviousEmployerNameddress"); }
        }

        protected DateTime m_TaxEmpGeneratedDate;
        [DBField("TaxEmpGeneratedDate"), TextSearch, Export(false)]
        public DateTime TaxEmpGeneratedDate
        {
            get { return m_TaxEmpGeneratedDate; }
            set { m_TaxEmpGeneratedDate = value; modify("TaxEmpGeneratedDate"); }
        }

        protected int m_TaxEmpGeneratedByUserID;
        [DBField("TaxEmpGeneratedByUserID"), TextSearch, Export(false)]
        public int TaxEmpGeneratedByUserID
        {
            get { return m_TaxEmpGeneratedByUserID; }
            set { m_TaxEmpGeneratedByUserID = value; modify("TaxEmpGeneratedByUserID"); }
        }

        protected bool m_TaxEmpIsReleasePrintoutToESS = false;
        [DBField("TaxEmpIsReleasePrintoutToESS"), TextSearch, Export(false)]
        public bool TaxEmpIsReleasePrintoutToESS
        {
            get { return m_TaxEmpIsReleasePrintoutToESS; }
            set { m_TaxEmpIsReleasePrintoutToESS = value; modify("TaxEmpIsReleasePrintoutToESS"); }
        }
        
    }
}
