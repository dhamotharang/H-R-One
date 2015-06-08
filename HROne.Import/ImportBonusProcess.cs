using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using HROne.Lib.Entities;
using HROne.DataAccess;

//using perspectivemind.validation;

namespace HROne.Import
{
    //[DBClass("UploadCommissionAchievement")]
    //public class EUploadCommissionAchievement : ImportDBObject
    //{
    //    public static DBManager db = new DBManager(typeof(EUploadCommissionAchievement));

    //    protected int m_UploadCAID;
    //    [DBField("UploadCAID", true, true), TextSearch, Export(false)]
    //    public int UploadCAID
    //    {
    //        get { return m_UploadCAID; }
    //        set { m_UploadCAID = value; modify("UploadCAID"); }
    //    }

    //    protected int m_EmpID;
    //    [DBField("EmpID"), TextSearch, Export(false), Required]
    //    public int EmpID
    //    {
    //        get { return m_EmpID; }
    //        set { m_EmpID = value; modify("EmpID"); }
    //    }

    //    protected double m_CAPercent;
    //    [DBField("CAPercent", "0.00"), TextSearch, Export(false), Required]
    //    public double CAPercent
    //    {
    //        get { return m_CAPercent; }
    //        set { m_CAPercent = value; modify("CAPercent"); }
    //    }

    //    protected DateTime m_CAEffDate;
    //    [DBField("CAEffDate"), TextSearch, Export(false)]
    //    public DateTime CAEffDate
    //    {
    //        get { return m_CAEffDate; }
    //        set { m_CAEffDate = value; modify("CAEffDate"); }
    //    }
    //}


    /// <summary>
    /// Summary description for ImportCND
    /// </summary>
    public class ImportBonusProcess : ImportProcessInteface
    {
        public const string TABLE_NAME_S = "BonusProcess_StandardBonus";
        public const string TABLE_NAME_D = "BonusProcess_DiscretionaryBonus";
        public const string FIELD_EMP_NO = "Emp No";
        public const string FIELD_RANK = "Rank";
        public const string FIELD_STD_RATE = "Rate";
        public const string FIELD_YEAR_OF_SERVICE = "Proportion";
        public const string FIELD_TARGET_SALARY = "Target Salary";
        public const string FIELD_BONUS_AMOUNT = "Bonus Amount";


        protected int m_UserID;
        protected int m_BonusProcessID;

        protected DatabaseConnection m_dbConn;

        protected DateTime UploadDateTime = AppUtils.ServerDateTime();
        protected DBManager db = EUploadCommissionAchievement.db;

        public ImportErrorList errors = new ImportErrorList();

        public ImportBonusProcess(DatabaseConnection dbConn, string SessionID, int UserID, int pBonusProcessID)
            : base(dbConn, SessionID)
        {
            m_dbConn = dbConn;
            m_UserID = UserID;
            m_BonusProcessID = pBonusProcessID;
        }

        public override DataTable UploadToTempDatabase(string Filename, int UserID, string ZipPassword)
        {
            return null;
        }

        public override DataTable GetImportDataFromTempDatabase(ListInfo info)
        {
            return null;
        }

        public static void ClearTempTable(DatabaseConnection dbConn, string sessionID)
        {
        }

        public override void ClearTempTable()
        {
        }

        public bool ConfirmBonusProcess(PageErrors pErrors)
        {
            EBonusProcess m_process = EBonusProcess.GetObject(dbConn, m_BonusProcessID);

            if (m_process.BonusProcessStatus == EBonusProcess.STATUS_NORMAL)
            {
                m_process.BonusProcessStatus = EBonusProcess.STATUS_CONFIRMED;
                return (EBonusProcess.db.update(dbConn, m_process));
            }
            return false;
        }

        public bool ClearUploadedDiscretionaryData(PageErrors pErrors)
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("BonusProcessID", m_BonusProcessID));
            m_filter.add(new Match("EmpBonusProcessType", "D"));

            return (EEmpBonusProcess.db.delete(dbConn, m_filter));
       }

        public bool ClearUploadedStandardData(PageErrors pErrors)
        {
            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("BonusProcessID", m_BonusProcessID));
            m_filter.add(new Match("EmpBonusProcessType", "S"));

            return EEmpBonusProcess.db.delete(dbConn, m_filter);        
        }

        public void ImportDiscretionaryEmpBonusProcess(int pBonussProcessID, DataTable pRawDataTable, PageErrors pErrors)
        {
            if (!pRawDataTable.Columns.Contains(FIELD_RANK) ||
                !pRawDataTable.Columns.Contains(FIELD_EMP_NO) ||
                !pRawDataTable.Columns.Contains(FIELD_TARGET_SALARY))
            {
                pErrors.addError("Invalid file selected");
                return;
            }
            int m_uploadedCount = 0;
            int m_skippedCount = 0;
            int m_empID = 0;
            double m_targetSalary = 0;
            string m_empNo = "";
            string m_rank = "";
            bool m_dbUpdated = false;

            foreach (DataRow m_row in pRawDataTable.Rows)
            {
                m_empNo = m_row[FIELD_EMP_NO].ToString();
                m_rank = m_row[FIELD_RANK].ToString();

                if (!string.IsNullOrEmpty(m_empNo))
                {
                    if (!(m_rank == "1" || m_rank == "2" || m_rank == "3" || m_rank == "4" || m_rank == "5"))
                    {
                        pErrors.addError("Invalid RANK value (Employee No." + m_empNo + ")");
                        m_skippedCount++;
                        continue;
                    }

                    if (double.TryParse(m_row[FIELD_TARGET_SALARY].ToString(), out m_targetSalary))
                    {
                        m_empID = Parse.GetEmpID(dbConn, m_empNo, m_UserID);
                        if (m_empID > 0)
                        {
                            // check if the empID exists in the current EmpBonusProcess
                            DBFilter m_filter = new DBFilter();
                            m_filter.add(new Match("BonusProcessID", m_BonusProcessID));
                            m_filter.add(new Match("EmpID", m_empID));
                            m_filter.add(new Match("EmpBonusProcessType", "D"));
                            ArrayList m_list = EEmpBonusProcess.db.select(dbConn, m_filter);
                            if (m_list.Count > 0)
                            {
                                EEmpBonusProcess m_empBonusProcess = (EEmpBonusProcess)m_list[0];

                                if (string.IsNullOrEmpty(m_rank))
                                {
                                    m_dbUpdated = EEmpBonusProcess.db.delete(dbConn, m_empBonusProcess);
                                }
                                else
                                {
                                    m_empBonusProcess.EmpBonusProcessTargetSalary = m_targetSalary;
                                    m_empBonusProcess.EmpBonusProcessRank = m_rank;
                                    m_dbUpdated = EEmpBonusProcess.db.update(dbConn, m_empBonusProcess);
                                }
                            }
                            else
                            {
                                // add new
                                if (!string.IsNullOrEmpty(m_rank))
                                {
                                    EEmpBonusProcess m_newEmpBonusProcess = new EEmpBonusProcess();
                                    m_newEmpBonusProcess.EmpID = m_empID;
                                    m_newEmpBonusProcess.BonusProcessID = m_BonusProcessID;
                                    m_newEmpBonusProcess.EmpBonusProcessType = "D";
                                    m_newEmpBonusProcess.EmpBonusProcessRank = m_rank;
                                    m_newEmpBonusProcess.EmpBonusProcessTargetSalary = m_targetSalary;

                                    m_dbUpdated = EEmpBonusProcess.db.insert(dbConn, m_newEmpBonusProcess);
                                }
                                else
                                {
                                    m_dbUpdated = false;
                                }
                            }

                            if (m_dbUpdated)
                            {
                                m_uploadedCount++;
                            }
                            else
                            {
                                pErrors.addError("DB update failed. (Employee No.=" + m_empNo + ")");
                                m_skippedCount++;
                            }
                        }
                        else
                        {
                            pErrors.addError("You are not authorized to handle specified employee (Employee No=" + m_empNo + ")");
                        }
                    }
                    else
                    {
                        m_skippedCount++;
                        pErrors.addError("Employee Target Salary is not available (Employee No=" + m_empNo + ")");

                    }
                }
            }
        }

        public void ImportStandardEmpBonusProcess(int pBonussProcessID, DataTable pRawDataTable, PageErrors pErrors)
        {
            if (!pRawDataTable.Columns.Contains(FIELD_EMP_NO) ||
                !pRawDataTable.Columns.Contains(FIELD_BONUS_AMOUNT) ||
                !pRawDataTable.Columns.Contains(FIELD_TARGET_SALARY) ||
                !pRawDataTable.Columns.Contains(FIELD_STD_RATE))
            {
                pErrors.addError("Invalid file selected");
                return;
            }


            int m_uploadedCount = 0;
            int m_skippedCount = 0;
            int m_empID = 0;
            double m_bonusAmount = 0;
            double m_targetSalary = 0;
            string m_empNo = "";
            bool m_dbUpdated = false;
            
            foreach (DataRow m_row in pRawDataTable.Rows)
            {
                m_empNo = m_row[FIELD_EMP_NO].ToString();

                if (!string.IsNullOrEmpty(m_empNo))
                {
                    if (double.TryParse(m_row[FIELD_BONUS_AMOUNT].ToString(), out m_bonusAmount) && 
                        double.TryParse(m_row[FIELD_TARGET_SALARY].ToString(), out m_targetSalary))
                    {
                        m_empID = Parse.GetEmpID(dbConn, m_empNo, m_UserID);
                        if (m_empID > 0)
                        {
                            // check if the empID exists in the current EmpBonusProcess
                            DBFilter m_filter = new DBFilter();
                            m_filter.add(new Match("BonusProcessID", m_BonusProcessID));
                            m_filter.add(new Match("EmpID", m_empID));
                            m_filter.add(new Match("EmpBonusProcessType", "S"));
                            ArrayList m_list = EEmpBonusProcess.db.select(dbConn, m_filter);
                            if (m_list.Count > 0)
                            {
                                EEmpBonusProcess m_empBonusProcess = (EEmpBonusProcess)m_list[0];

                                if (m_bonusAmount <= 0)
                                {
                                    m_dbUpdated = EEmpBonusProcess.db.delete(dbConn, m_empBonusProcess);
                                }
                                else
                                {
                                    m_empBonusProcess.EmpBonusProcessTargetSalary = m_targetSalary;
                                    m_empBonusProcess.EmpBonusProcessBonusAmount = m_bonusAmount;
                                    m_dbUpdated = EEmpBonusProcess.db.update(dbConn, m_empBonusProcess);
                                }
                            }
                            else
                            {
                                // add new
                                if (m_bonusAmount > 0)
                                {
                                    EEmpBonusProcess m_newEmpBonusProcess = new EEmpBonusProcess();
                                    m_newEmpBonusProcess.EmpID = m_empID;
                                    m_newEmpBonusProcess.BonusProcessID = m_BonusProcessID;
                                    m_newEmpBonusProcess.EmpBonusProcessType = "S";
                                    m_newEmpBonusProcess.EmpBonusProcessTargetSalary = m_targetSalary;
                                    m_newEmpBonusProcess.EmpBonusProcessBonusAmount = m_bonusAmount;

                                    m_dbUpdated = EEmpBonusProcess.db.insert(dbConn, m_newEmpBonusProcess);
                                }
                                else
                                {
                                    m_dbUpdated = false;
                                }
                            }

                            if (m_dbUpdated)
                            {
                                m_uploadedCount++;
                            }
                            else
                            {
                                pErrors.addError("DB update failed. (Employee No.=" + m_empNo + ")");
                                m_skippedCount++;
                            }
                        }
                        else
                        {
                            pErrors.addError("You are not authorized to handle specified employee (Employee No=" + m_empNo + ")");
                        }
                    }
                    else
                    {
                        m_skippedCount++;
                        pErrors.addError("Employee Target Salary/Bonus Amount is not available (Employee No=" + m_empNo + ")");

                    }

                }
            }
        }

        public void ImportEmpBonusProcess(int pBonusProcessID, DataTable pRawDataTable, string pBonusProcessType, PageErrors pErrors)
        {
            if (pBonusProcessType == "S")
            {
                ClearUploadedStandardData(pErrors);
                ImportStandardEmpBonusProcess(pBonusProcessID, pRawDataTable, pErrors);
            }
            else
            {
                ClearUploadedDiscretionaryData(pErrors);
                ImportDiscretionaryEmpBonusProcess(pBonusProcessID, pRawDataTable, pErrors);
            }
        }

        public override void ImportToDatabase()
        {
            //DataTable dataTable = GetImportDataFromTempDatabase(null);
            //if (dataTable.Rows.Count > 0)
            //{
            //    ECommissionAchievementImportBatch batchDetail = new ECommissionAchievementImportBatch();
            //    batchDetail.CAImportBatchDateTime = AppUtils.ServerDateTime();
            //    //batchDetail.CAImportBatchOriginalFilename = OriginalBatchFilename;
            //    //batchDetail.CAImportBatchRemark = Remark;
            //    batchDetail.CAImportBatchUploadedBy = m_UserID;
            //    ECommissionAchievementImportBatch.db.insert(dbConn, batchDetail);

            //    foreach (DataRow row in dataTable.Rows)
            //    {
            //        EUploadCommissionAchievement obj = new EUploadCommissionAchievement();
            //        EUploadCommissionAchievement.db.toObject(row, obj);

            //        ECommissionAchievement CA = new ECommissionAchievement();
            //        CA.CAPercent = obj.CAPercent;
            //        CA.CAEffDate = obj.CAEffDate;
            //        CA.EmpID = obj.EmpID;
            //        CA.CAImportBatchID = batchDetail.CAImportBatchID;

            //        ECommissionAchievement.db.insert(dbConn, CA);
            //        EUploadCommissionAchievement.db.delete(dbConn, obj);
            //    }
            //}
        }

        public DataTable ExportDiscretionaryBonusTemplate(ArrayList pEmpList, bool IsIncludeCurrentPositionInfo)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME_D);
            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_EMP_NO, typeof(string));

            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }

            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_TARGET_SALARY, typeof(double));
            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_RANK, typeof(string));

            EBonusProcess m_bonusProcess = EBonusProcess.GetObject(dbConn, m_BonusProcessID);
            if (m_bonusProcess != null)
            {
                foreach (EEmpPersonalInfo empInfo in pEmpList)
                {
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        DataRow row = tmpDataTable.NewRow();
                        row[FIELD_EMP_NO] = empInfo.EmpNo;

                        if (IsIncludeCurrentPositionInfo)
                        {
                            ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                            ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                        }

                        // check if record already exists in EmpBonusProcess table
                        //DBFilter m_empBonusProcessFilter = new DBFilter();
                        //m_empBonusProcessFilter.add(new Match("EmpID", empInfo.EmpID));
                        //m_empBonusProcessFilter.add(new Match("BonusProecssID", m_bonusProcess.BonusProcessID ));
                        //m_empBonusProcessFilter.add(new Match("EmpID", empInfo.EmpID));
                        //m_empBonusProcessFilter.add(new Match("EmpBonusProcessType", "D"));

                        //ArrayList m_empBonusProcessList = EEmpBonusProcess.db.select(dbConn, m_empBonusProcessFilter);

                        //if (m_empBonusProcessList.Count > 0)
                        //{
                        //    EEmpBonusProcess m_empBonusProcess = (EEmpBonusProcess) m_empBonusProcessList[0];
                        //    row[FIELD_STD_RATE] = m_bonusProcess.BonusProcessStdRate;
                        //    row[FIELD_TARGET_SALARY] = m_empBonusProcess.EmpBonusProcessTargetSalary;
                        //    row[RANK] = m_empBonusProcess.EmpBonusProcessRank;
                        //}
                        //else
                        //{
                        EEmpRecurringPayment m_recurringPayment = GetSalaryMonthRecurringPayment(empInfo.EmpID, m_bonusProcess.BonusProcessSalaryMonth);
                        if (m_recurringPayment != null)
                        {
                            row[FIELD_TARGET_SALARY] = m_recurringPayment.EmpRPBasicSalary;
                        }
                        else
                            row[FIELD_TARGET_SALARY] = 0;
                        row[FIELD_RANK] = "";
                        //}
                        tmpDataTable.Rows.Add(row);
                    }
                }
            }

            if (IsIncludeCurrentPositionInfo)
                ImportEmpPositionInfoProcess.RetriveHierarchyLevelHeader(dbConn, tmpDataTable);

            return tmpDataTable;
        }

        public void GenerateStandardBonusData(ArrayList pEmpList)
        {
            //double m_targetSalary = 0;
            EBonusProcess m_bonusProcess = EBonusProcess.GetObject(dbConn, m_BonusProcessID);
            if (m_bonusProcess != null)
            {
                foreach (EEmpPersonalInfo empInfo in pEmpList)
                {
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo) && empInfo.EmpProbaLastDate <= m_bonusProcess.BonusProcessPeriodTo)
                    {
                        EEmpRecurringPayment m_recurringPayment = GetSalaryMonthRecurringPayment(empInfo.EmpID, m_bonusProcess.BonusProcessSalaryMonth);
                        if (m_recurringPayment != null)
                        {
                            EEmpBonusProcess m_empBonusProcess = new EEmpBonusProcess();
                            m_empBonusProcess.BonusProcessID = m_BonusProcessID;
                            m_empBonusProcess.EmpID = empInfo.EmpID;
                            m_empBonusProcess.EmpBonusProcessTargetSalary = m_recurringPayment.EmpRPBasicSalary;
                            
                            System.TimeSpan m_totalDaysInPeriod = m_bonusProcess.BonusProcessPeriodTo.Subtract(m_bonusProcess.BonusProcessPeriodFr);
                            System.TimeSpan m_totalDaysJoint = m_bonusProcess.BonusProcessPeriodTo.Subtract((empInfo.EmpDateOfJoin < m_bonusProcess.BonusProcessPeriodFr) ? m_bonusProcess.BonusProcessPeriodFr : empInfo.EmpDateOfJoin);

                            m_empBonusProcess.EmpBonusProcessBonusProportion = Math.Round(Convert.ToDouble(m_totalDaysJoint.Days + 1) / Convert.ToDouble(m_totalDaysInPeriod.Days + 1), 4);
                            m_empBonusProcess.EmpBonusProcessType = "S";
                            m_empBonusProcess.EmpBonusProcessBonusAmount = Math.Round(m_recurringPayment.EmpRPBasicSalary *
                                                                                      m_empBonusProcess.EmpBonusProcessBonusProportion * 
                                                                                      m_bonusProcess.BonusProcessStdRate, 2);

                            EEmpBonusProcess.db.insert(dbConn, m_empBonusProcess);
                        }
                    }
                }
            }
        }

        protected bool GetEmpRecurringPaymentInfo(int pEmpID, DateTime pSalaryMonth, out double pTargetSalary, out string pPayMethod, out string pBankAccountCode, out string pCostCenterCode)
        {
            DBFilter m_rpFilter = new DBFilter();
            OR m_orDate = new OR();

            m_orDate.add(new NullTerm("EmpRPEffTo"));
            m_orDate.add(new Match("EmpRPEffTo", ">=", pSalaryMonth));

            m_rpFilter.add(m_orDate);
            m_rpFilter.add(new Match("EmpRPEffFr", "<=", pSalaryMonth));
            m_rpFilter.add(new Match("EmpID", pEmpID));
            m_rpFilter.add(new Match("EmpRPBasicSalary", ">", 0));

            DBFilter m_payCodeFilter = new DBFilter();
            DBFilter m_payTypeFilter = new DBFilter();
            m_payTypeFilter.add(new Match("PaymentTypeCode", "BASICSAL"));
            m_payCodeFilter.add(new IN("PaymentTypeID", "SELECT PaymentTypeID FROM PaymentType", m_payTypeFilter));
            m_rpFilter.add(new IN("PayCodeID", "SELECT PaymentCodeID FROM PaymentCode", m_payCodeFilter));
            m_rpFilter.add("EmpRPID", false);

            ArrayList m_rpList = EEmpRecurringPayment.db.select(dbConn, m_rpFilter);

            pPayMethod = "";
            pBankAccountCode = "";
            pCostCenterCode = "";
            pTargetSalary = 0;

            if (m_rpList.Count > 0)
            {
                EEmpRecurringPayment m_empRP = (EEmpRecurringPayment)m_rpList[0];

                pTargetSalary = m_empRP.EmpRPBasicSalary;

                switch (m_empRP.EmpRPMethod)
                {
                    case "A":
                        pPayMethod = "Autopay";
                        break;
                    case "Q":
                        pPayMethod = "Cheque";
                        break;
                    case "C":
                        pPayMethod = "Cash";
                        break;
                    default:
                        pPayMethod = "Other";
                        break;
                }

                EEmpBankAccount m_bank = new EEmpBankAccount();
                m_bank.EmpBankAccountID = m_empRP.EmpAccID;
                if (EEmpBankAccount.db.select(dbConn, m_bank))
                    pBankAccountCode = m_bank.EmpAccountNo;

                ECostCenter m_costCenter = new ECostCenter();
                m_costCenter.CostCenterID = m_empRP.CostCenterID;
                if (ECostCenter.db.select(dbConn, m_costCenter))
                    pCostCenterCode = m_costCenter.CostCenterCode;
            }
            return true;
        }

        public DataSet GenerateCND()
        {
            EBonusProcess m_process = EBonusProcess.GetObject(dbConn, m_BonusProcessID);
            //        EPaymentCode m_paymentCode = EPaymentCode.GetObject(dbConn, m_process.BonusProcessPayCodeID);

            DataSet dataSet = new DataSet();//export.GetDataSet();
            DataTable dataTable = new DataTable("ClaimsAndDeduction$");
            dataSet.Tables.Add(dataTable);

            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO, typeof(string));
            dataTable.Columns.Add("English Name", typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE, typeof(DateTime));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT, typeof(double));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST, typeof(double));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK, typeof(string));
            dataTable.Columns.Add(HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER, typeof(string));

            DBFilter m_detailFilter = new DBFilter();

            m_detailFilter.add(new Match("BonusProcessID", m_BonusProcessID));
            m_detailFilter.add("EmpID", true);
            m_detailFilter.add("EmpBonusProcessType", false);  // Standard > Discretionary

            double m_bonusAmount = 0;
            double m_targetSalary = 0;
            int m_currentEmpID = -1;
            string m_payMethod = "";
            string m_remarks = "";
            string m_bankAccountCode = "";
            string m_costCenterCode = "";

            DataRow m_row = null;
            EEmpPersonalInfo m_empInfo = null;
            double m_proportion = 0; // the proportion of bonus recieved, depending on the join date

            EBonusProcess m_bonusProcess = EBonusProcess.GetObject(dbConn, m_BonusProcessID);

            foreach (EEmpBonusProcess m_detail in EEmpBonusProcess.db.select(dbConn, m_detailFilter))
            {
                if (m_detail.EmpID != m_currentEmpID)
                {
                    if (m_currentEmpID > -1)
                    {
                        dataTable.Rows.Add(m_row);
                    }
                    m_currentEmpID = m_detail.EmpID;
                    m_row = dataTable.NewRow();
                    GetEmpRecurringPaymentInfo(m_detail.EmpID, m_process.BonusProcessSalaryMonth, out m_targetSalary, out m_payMethod, out m_bankAccountCode, out m_costCenterCode);
                    m_empInfo = EEmpPersonalInfo.GetObject(dbConn, m_detail.EmpID);

                    System.TimeSpan m_totalDaysInPeriod = m_bonusProcess.BonusProcessPeriodTo.Subtract(m_bonusProcess.BonusProcessPeriodFr);
                    System.TimeSpan m_totalDaysJoint = m_bonusProcess.BonusProcessPeriodTo.Subtract((m_empInfo.EmpDateOfJoin < m_bonusProcess.BonusProcessPeriodFr) ? m_bonusProcess.BonusProcessPeriodFr : m_empInfo.EmpDateOfJoin);

                    m_proportion = Math.Round(Convert.ToDouble(m_totalDaysJoint.Days + 1) / Convert.ToDouble(m_totalDaysInPeriod.Days + 1), 4);
                    m_remarks = "";
                    m_bonusAmount = 0;

                    //re-take targetSalary from RecurringPayment
                    //m_targetSalary = m_detail.EmpBonusProcessTargetSalary;
                }
                else // same EmpID
                {
                    m_remarks += " + ";
                }

                if (m_detail.EmpBonusProcessType == "S")
                {
                    m_bonusAmount = m_targetSalary * m_proportion * m_bonusProcess.BonusProcessStdRate; 
                    m_remarks = m_remarks + "(" + m_targetSalary.ToString("#,##0.00") + " * " + m_proportion.ToString("0.0000") + " * " + m_process.BonusProcessStdRate.ToString("0.0000") + ")";

                }
                else if (m_detail.EmpBonusProcessType == "D")
                {
                    if (m_detail.EmpBonusProcessRank == "1")
                    {
                        m_bonusAmount = m_bonusAmount + m_targetSalary * m_proportion * m_process.BonusProcessRank1 / 100;
                        m_remarks = m_remarks + "(" + m_targetSalary.ToString("#,##0.00") + " * " + m_proportion.ToString("0.0000") + " * " + m_process.BonusProcessRank1.ToString("0.00") + "%)";
                    }
                    else if (m_detail.EmpBonusProcessRank == "2")
                    {
                        m_bonusAmount = m_bonusAmount + m_targetSalary * m_proportion * m_process.BonusProcessRank2 / 100;
                        m_remarks = m_remarks + "(" + m_targetSalary.ToString("#,##0.00") + " * " + m_proportion.ToString("0.0000") + " * " + m_process.BonusProcessRank2.ToString("0.00") + "%)";
                    }
                    else if (m_detail.EmpBonusProcessRank == "3")
                    {
                        m_bonusAmount = m_bonusAmount + m_targetSalary * m_proportion * m_process.BonusProcessRank3 / 100;
                        m_remarks = m_remarks + "(" + m_targetSalary.ToString("#,##0.00") + " * " + m_proportion.ToString("0.0000") + " * " + m_process.BonusProcessRank3.ToString("0.00") + "%)";
                    }
                    else if (m_detail.EmpBonusProcessRank == "4")
                    {
                        m_bonusAmount = m_bonusAmount + m_targetSalary * m_proportion * m_process.BonusProcessRank4 / 100;
                        m_remarks = m_remarks + "(" + m_targetSalary.ToString("#,##0.00") + " * " + m_proportion.ToString("0.0000") + " * " + m_process.BonusProcessRank4.ToString("0.00") + "%)";
                    }
                    else if (m_detail.EmpBonusProcessRank == "5")
                    {
                        m_bonusAmount = m_bonusAmount + m_targetSalary * m_proportion * m_process.BonusProcessRank5 / 100;
                        m_remarks = m_remarks + "(" + m_targetSalary.ToString("#,##0.00") + " * " + m_proportion.ToString("0.0000") + " * " + m_process.BonusProcessRank5.ToString("0.00") + "%)";
                    }
                }

                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO] = m_empInfo.EmpNo;
                m_row["English Name"] = m_empInfo.EmpEngFullName;
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE] = m_process.BonusProcessPayDate;

                EPaymentCode m_payCodeObj = EPaymentCode.GetObject(dbConn, m_process.BonusProcessPayCodeID);

                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE] = m_payCodeObj.PaymentCode;
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = m_payMethod;
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST] = 0; //DateTime.DaysInMonth(m_process.AsAtDate);
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT] = "No";
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT] = Math.Round(m_bonusAmount, 2);
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK] = m_remarks;
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO] = m_bankAccountCode;
                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER] = m_costCenterCode;
            }

            if (m_currentEmpID > -1)    // indicate there are CND records written into the table.
            {
                dataTable.Rows.Add(m_row);
            }
            return dataSet;
        }

        public DataTable ExportStandardBonusTemplate(bool IsIncludeCurrentPositionInfo)
        {
            DataTable tmpDataTable = new DataTable(TABLE_NAME_S);

            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_EMP_NO, typeof(string));
            if (IsIncludeCurrentPositionInfo)
            {
                ImportEmpPersonalInfoProcess.AddEmployeeInfoHeader(tmpDataTable);
                ImportEmpPositionInfoProcess.AddEmployeePositionInfoHeader(dbConn, tmpDataTable);
            }
            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_TARGET_SALARY, typeof(double));
            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_STD_RATE, typeof(double));
            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_YEAR_OF_SERVICE, typeof(double));
            tmpDataTable.Columns.Add(HROne.Import.ImportBonusProcess.FIELD_BONUS_AMOUNT, typeof(double));

            DBFilter m_filter = new DBFilter();
            m_filter.add(new Match("BonusProcessID", m_BonusProcessID));
            m_filter.add(new Match("EmpBonusProcessType", "S"));
            m_filter.add("EmpID", true);

            EBonusProcess m_bonusProcess = EBonusProcess.GetObject(dbConn, m_BonusProcessID);

            foreach (EEmpBonusProcess m_empBonusProcess in EEmpBonusProcess.db.select(dbConn, m_filter))
            {
                EEmpPersonalInfo empInfo = EEmpPersonalInfo.GetObject(dbConn, m_empBonusProcess.EmpID);

                DataRow row = tmpDataTable.NewRow();
                
                row[FIELD_EMP_NO] = empInfo.EmpNo;

                if (IsIncludeCurrentPositionInfo)
                {
                    ImportEmpPersonalInfoProcess.AddEmployeeInfo(dbConn, row, empInfo.EmpID);
                    ImportEmpPositionInfoProcess.AddEmployeePositionInfo(dbConn, row, empInfo.EmpID);
                }
                row[FIELD_TARGET_SALARY] = m_empBonusProcess.EmpBonusProcessTargetSalary;
                row[FIELD_STD_RATE] = m_bonusProcess.BonusProcessStdRate;
                row[FIELD_YEAR_OF_SERVICE] = Math.Round(m_empBonusProcess.EmpBonusProcessBonusProportion, 4);
                row[FIELD_BONUS_AMOUNT] = Math.Round(m_empBonusProcess.EmpBonusProcessBonusAmount, 2);

                tmpDataTable.Rows.Add(row);                
            }

            if (IsIncludeCurrentPositionInfo)
                ImportEmpPositionInfoProcess.RetriveHierarchyLevelHeader(dbConn, tmpDataTable);
            
            return tmpDataTable;
        }

        protected EEmpRecurringPayment GetSalaryMonthRecurringPayment(int pEmpID, DateTime pSalaryMonth)
        {
            DBFilter m_recurringPaymentFilter = new DBFilter();
            m_recurringPaymentFilter.add(new Match("EmpID", pEmpID));
            m_recurringPaymentFilter.add(new Match("EmpRPEffFr", "<=", pSalaryMonth));

            OR m_OREndDate = new OR();
            m_OREndDate.add(new Match("EmpRPEffTo", ">=", pSalaryMonth));
            m_OREndDate.add(new NullTerm("EmpRPEffTo"));
        
            m_recurringPaymentFilter.add(AppUtils.GetPayemntCodeDBTermByPaymentType(dbConn, "PayCodeID", "BASICSAL"));
            m_recurringPaymentFilter.add(m_OREndDate);
            m_recurringPaymentFilter.add("EmpRPID", false);

            ArrayList m_rpList = EEmpRecurringPayment.db.select(dbConn, m_recurringPaymentFilter);

            if (m_rpList.Count > 0)
                return (EEmpRecurringPayment)m_rpList[0];
            else
                return null;
        }
    }
}