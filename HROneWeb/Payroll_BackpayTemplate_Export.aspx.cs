using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using HROne.Import;
using HROne.Lib.Entities;
using HROne.DataAccess;
using System.Collections.Generic;
//using perspectivemind.validation;

public partial class Payroll_BackpayTemplate_Export : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY017";

    private DBManager db = EPaymentCode.db;
    protected ListInfo info;
    private DataView view;

    public string selectedSchemeCode = "";
    public string selectedPaymentCode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            return;
        }

        if (!Page.IsPostBack)
        {
            init_SchemeCodeDropdown();
            init_PaymentCodeDropdown();
        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);
    }

    protected void init_SchemeCodeDropdown()
    {
        if (SchemeCode.Items.Count <= 0)
        {
            SchemeCode.Items.Add("All");

            DBFilter m_filter = new DBFilter();
            m_filter.add("SchemeCode", true);

            DataTable m_table = AppUtils.runSelectSQL("DISTINCT c.SchemeCode as SchemeCode", "From PayScale c ", m_filter, dbConn);

            foreach (DataRow row in m_table.Rows)
            {
                SchemeCode.Items.Add(row["SchemeCode"].ToString());
            }
        }
    }

    protected void init_PaymentCodeDropdown()
    {
        if (PaymentCode.Items.Count <= 0)
        {
            PaymentCode.Items.Add("Not Selected");

            DBFilter m_filter = new DBFilter();
            m_filter.add("PaymentCode", true);

            DataTable m_table = AppUtils.runSelectSQL("PaymentCode, PaymentCodeDesc ", "From PaymentCode ", m_filter, dbConn);

            foreach (DataRow row in m_table.Rows)
            {
                PaymentCode.Items.Add(string.Format("{0} - {1}", row["PaymentCode"], row["PaymentCodeDesc"]));
            }
        }
    }

    private decimal GetPayScale(EEmpRecurringPayment EmpRP)
    {
        DBFilter m_filter = new DBFilter();
        if (EmpRP.SchemeCode == null || EmpRP.SchemeCode == "" || EmpRP.Capacity == null || EmpRP.Capacity == "" || EmpRP.Point == null || EmpRP.Point < 0)
            return Decimal.Zero;

        m_filter.add(new Match("SchemeCode", AppUtils.Encode(EPayScaleMap.db.getField("SchemeCode"), EmpRP.SchemeCode)));
        m_filter.add(new NullTerm("ExpiryDate"));
        m_filter.add(new Match("Point", EmpRP.Point));
        decimal m_salary = Decimal.Zero;

        ArrayList m_salaryList = EPayScaleMap.db.select(dbConn, m_filter);
        if (m_salaryList.Count > 0)
            return ((EPayScaleMap)m_salaryList[0]).Salary;

        return -1;
    }

    private DataSet GenerateBackpayTemplate(String schemeCode, int paymentCodeID, DateTime paymentDate)
    {
        //Dictionary<int, string> m_PaymentCodeList = new Dictionary<int, string>();
        //foreach (EPaymentCode o in EPaymentCode.db.select(dbConn, new DBFilter()))
        //{
        //    m_PaymentCodeList.Add(o.PaymentCodeID, o.PaymentCode);
        //}

        string m_paymentCode = (EPaymentCode.GetObject(dbConn, paymentCodeID)).PaymentCode;

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

        DBFilter m_payscaleFilter = new DBFilter();
        m_payscaleFilter.add(new NullTerm("ExpiryDate"));

        if (schemeCode != "") 
            m_payscaleFilter.add(new Match("SchemeCode", schemeCode));

        m_payscaleFilter.add("EffectiveDate", false);

        foreach (EPayScaleMap m_latestPayscale in EPayScaleMap.db.select(dbConn, m_payscaleFilter))
        {
            DBFilter m_payPeriodFilter = new DBFilter();
            m_payPeriodFilter.add(new Match("PayPeriodFr", ">=", m_latestPayscale.EffectiveDate));
            m_payPeriodFilter.add(new Match("PayPeriodStatus", "=", EPayrollPeriod.PAYPERIOD_STATUS_PROCESSEND_FLAG));
            m_payPeriodFilter.add(WebUtils.AddRankFilter(Session, "EmpID", true));

            foreach (EPayrollPeriod m_payrollPeriod in EPayrollPeriod.db.select(dbConn, m_payPeriodFilter))
            {
                DBFilter m_empPayrollFilter = new DBFilter();
                m_empPayrollFilter.add(new Match("PayPeriodID", m_payrollPeriod.PayPeriodID));

                DBFilter m_paymentRecordFilter = new DBFilter();
                DBFilter m_paymentTypeFilter = new DBFilter();
                DBFilter m_paymentCodeFilter = new DBFilter();

                m_paymentTypeFilter.add(new Match("PaymentTypeCode", "BASICSAL"));
                m_paymentCodeFilter.add(new IN("PaymentTypeID", "SELECT PaymentTypeID FROM PaymentType", m_paymentTypeFilter));

                m_paymentRecordFilter.add(new Match("EmpRPIDforBP", ">", 0));
                m_paymentRecordFilter.add(new IN("EmpPayrollID", "SELECT EmpPayrollID FROM EmpPayroll", m_empPayrollFilter));
                m_paymentRecordFilter.add(new IN("PaymentCodeID", "SELECT PaymentCodeID FROM PaymentCode", m_paymentCodeFilter));

                foreach (EPaymentRecord m_payRecord in EPaymentRecord.db.select(dbConn, m_paymentRecordFilter))
                {
                    EEmpRecurringPayment m_empRP = EEmpRecurringPayment.GetObject(dbConn, m_payRecord.EmpRPIDforBP);

                    if (schemeCode != "")
                    {
                        if (m_empRP.SchemeCode == null || m_empRP.SchemeCode != schemeCode)
                        {
                            break;
                        }
                    }
                    decimal m_newSalary = GetPayScale(m_empRP);
                    string m_remarks = "";
                    double m_amount = 0;
                    if (m_newSalary > 0)
                    {
//                        m_amount = Math.Round((m_payRecord.PayRecActAmount / m_empRP.EmpRPAmount) * (System.Convert.ToDouble(m_newSalary) - m_empRP.EmpRPAmount), 2);

                        int m_daysInMonth = DateTime.DaysInMonth(m_payrollPeriod.PayPeriodFr.Year, m_payrollPeriod.PayPeriodFr.Month);
                        if (m_payRecord.PayRecNumOfDayAdj != m_daysInMonth)
                        {
                            m_amount = Math.Round((System.Convert.ToDouble(m_newSalary) - m_empRP.EmpRPAmount) * m_payRecord.PayRecNumOfDayAdj / m_daysInMonth, 2);

                            m_remarks = String.Format("Backpay {0}: ({1}-{2})*{3}/{4}.", m_payrollPeriod.PayPeriodFr.ToString("yyyy-MM"),
                                                                                        m_newSalary.ToString("#,##0.00"),
                                                                                        m_empRP.EmpRPAmount.ToString("#,##0.00"),
                                                                                        m_payRecord.PayRecNumOfDayAdj,
                                                                                        m_daysInMonth);
                        }
                        else
                        {
                            m_amount = Math.Round((System.Convert.ToDouble(m_newSalary) - m_empRP.EmpRPAmount), 2);
                            m_remarks = String.Format("Backpay {0}: {1}-{2}.", m_payrollPeriod.PayPeriodFr.ToString("yyyy-MM"),
                                                                                m_newSalary.ToString("#,##0.00"),
                                                                                m_empRP.EmpRPAmount.ToString("#,##0.00"));
                        }
                        if (m_amount > 0)
                        {
                            EEmpPersonalInfo m_empInfo = new EEmpPersonalInfo();
                            m_empInfo.EmpID = m_empRP.EmpID;
                            EEmpPersonalInfo.db.select(dbConn, m_empInfo);

                            DataRow m_row = dataTable.NewRow();
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EMP_NO] = m_empInfo.EmpNo;
                            m_row["English Name"] = m_empInfo.EmpEngFullName;
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_EFFECTIVE_DATE] = paymentDate;
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_CODE] = m_paymentCode;
                            switch(m_empRP.EmpRPMethod)
                            {
                                case "A":
                                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Autopay";
                                    break;
                                case "Q":
                                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cheque";
                                    break;
                                case "C":
                                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Cash";
                                    break;
                                default:
                                    m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_PAYMENT_METHOD] = "Other";
                                    break;
                            }
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_AMOUNT] = m_amount;
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_NUM_OF_DAY_ADJUST] = 0;
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REST_PAYMENT] = "No";
                            m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_REMARK] = m_remarks;

                            EEmpBankAccount m_bank = new EEmpBankAccount();
                            m_bank.EmpBankAccountID = m_empRP.EmpAccID;
                            if (EEmpBankAccount.db.select(dbConn, m_bank))
                                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_BANK_ACCOUNT_NO] = m_bank.EmpAccountNo;

                            ECostCenter m_costCenter = new ECostCenter();
                            m_costCenter.CostCenterID = m_empRP.CostCenterID;
                            if (ECostCenter.db.select(dbConn, m_costCenter))
                                m_row[HROne.Import.ImportClaimsAndDeductionsProcess.FIELD_COST_CENTER] = m_costCenter.CostCenterCode;
                            dataTable.Rows.Add(m_row);
                        }
                    }
                }
            }
            break;  // handle only 1 effective date 
        }
        return dataSet;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        //Hashtable values = new Hashtable();
        //binding.toValues(values);

        string m_schemeCode = "";
        string m_paymentCode = "";
        int m_paymentCodeID = -1;
        DateTime m_paymentDate = new DateTime();
        EPaymentCode m_paymentCodeObj;

        if (SchemeCode.SelectedIndex > 0)
        {
            m_schemeCode = SchemeCode.SelectedValue;
        }

        if (BackpayDate.Value != "")
        {
            m_paymentDate = DateTime.Parse(BackpayDate.Value);
        }
        else
        {
            errors.addError("Please provide a Backpay Payment Date");
            return;
        }

        if (PaymentCode.SelectedIndex > 0)
        {
            m_paymentCode = PaymentCode.SelectedValue;
            m_paymentCodeObj = EPaymentCode.GetObject(dbConn, m_paymentCode.Substring(0, m_paymentCode.IndexOf("-")).Trim());

            if (m_paymentCodeObj == null)
            {
                errors.addError("Cannot resolve Backpay Payment Code");
                return;
            }
        }
        else
        {
            errors.addError("Pelase select a Backpay Payment Code");
            return;
        }

        DataSet dataSet = GenerateBackpayTemplate(m_schemeCode, m_paymentCodeObj.PaymentCodeID, m_paymentDate);

        string exportFileName = System.IO.Path.GetTempFileName();
        System.IO.File.Delete(exportFileName);
        exportFileName += ".xls";
        HROne.Export.ExcelExport export = new HROne.Export.ExcelExport(exportFileName);
        export.Update(dataSet);
        WebUtils.TransmitFile(Response, exportFileName, "BackpayCND_" + AppUtils.ServerDateTime().ToString("yyyyMMddHHmmss") + ".xls", true);
    }
}
