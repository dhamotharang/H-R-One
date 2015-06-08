using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Payroll;
using HROne.Translation;
using HROne.CommonLib;

public partial class Payroll_RollbackPayrollPeriod : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY900";
    public Binding binding;

    public DBManager db = EPayrollGroup.db;
    public EPayrollGroup obj;
    public int CurID = -1;
    public int CurPayPeriodID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.Read))
            return;
        if (!WebUtils.CheckPermission(Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
        {
            panelRollbackOption.Visible = false;
        }
        else
        {
            panelRollbackOption.Visible = true;
        }

        btnRollback.OnClientClick = HROne.Translation.PromptMessage.PAYROLL_ROLLBACK_PAYROLL_PERIOD_GENERIC_JAVASCRIPT;

        binding = new Binding(dbConn, db);
        // Start 0000069, KuangWei, 2014-08-28
        initPayrollGroup();
        // End 0000069, KuangWei, 2014-08-28
        binding.add(CurrentPayPeriodID);

        OR orPayPeriodStauts = new OR();
        orPayPeriodStauts.add(new Match("PayPeriodStatus", "<>", "E"));
        orPayPeriodStauts.add(new Match("PayPeriodIsAutoCreate", false));

        DBFilter payPeriodFilter = new DBFilter();
        payPeriodFilter.add(orPayPeriodStauts);
        if (!int.TryParse(PayGroupID.SelectedValue, out CurID))
            if (!int.TryParse(DecryptedRequest["PayGroupID"], out CurID))
                CurID = -1;
        payPeriodFilter.add(new Match("PayGroupID", CurID));
        payPeriodFilter.add("PayPeriodFr", false);

        binding.add(new DropDownVLBinder(EPayrollPeriod.db, PayPeriodID, EPayrollPeriod.VLPayrollPeriod, payPeriodFilter));

        binding.init(Request, Session);


        if (!int.TryParse(PayPeriodID.SelectedValue, out CurPayPeriodID))
            if (!int.TryParse(DecryptedRequest["PayPeriodID"], out CurPayPeriodID))
            {
                EPayrollGroup obj = new EPayrollGroup();
                obj.PayGroupID = CurID;
                if (EPayrollGroup.db.select(dbConn, obj))
                    CurPayPeriodID = obj.CurrentPayPeriodID;
                else
                    CurPayPeriodID = -1;
            }

        if (!Page.IsPostBack)
        {
            loadState();
            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                loadObject();
            }
            else
            {
                panelPayPeriod.Visible = false;
                panelRollbackOption.Visible = false;
            }

        }

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    // Start 0000069, KuangWei, 2014-08-28
    protected void initPayrollGroup()
    {
        DBFilter m_filter = new DBFilter();
        DBFilter m_userFilter = new DBFilter();

        PayGroupID.Items.Add(new ListItem("Not Selected", "-1"));

        m_userFilter.add(new Match("UserID", WebUtils.GetCurUser(Session).UserID));

        m_filter.add(new IN("PayGroupID", "SELECT PayGroupID FROM PayrollGroupUsers ", m_userFilter));
        m_filter.add("PayGroupCode", true);
        // since sorting is not feasible directly using DBFilter (because of the encrypted data), we use a local data table as a temp buffer
        DataTable m_localTable = new DataTable();
        m_localTable.Columns.Add("PayGroupCode", typeof(string));
        m_localTable.Columns.Add("PayGroupDesc", typeof(string));
        m_localTable.Columns.Add("PayGroupID", typeof(int));

        foreach (EPayrollGroup o in EPayrollGroup.db.select(dbConn, m_filter))
        {
            DataRow m_row = m_localTable.NewRow();
            m_row["PayGroupCode"] = o.PayGroupCode;
            m_row["PayGroupDesc"] = o.PayGroupDesc;
            m_row["PayGroupID"] = o.PayGroupID;
            m_localTable.Rows.Add(m_row);
        }

        foreach (DataRow m_o in m_localTable.Select("", "payGroupCode"))
        {
            PayGroupID.Items.Add(new ListItem(m_o["PayGroupCode"].ToString() + " - " + m_o["PayGroupDesc"].ToString(), m_o["PayGroupID"].ToString()));
        }
    }

    protected bool loadObject()
    {
        obj = new EPayrollGroup();
        obj.PayGroupID = CurID;
        db.select(dbConn, obj);
        //if (!db.select(dbConn, obj))
        //return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        try
        {
            PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
        }
        catch
        {
            CurPayPeriodID = 0;
        }
        if (CurPayPeriodID <= 0 && obj.CurrentPayPeriodID > 0)
        {
            CurPayPeriodID = obj.CurrentPayPeriodID;
            try
            {
                PayPeriodID.SelectedValue = CurPayPeriodID.ToString();
            }
            catch
            {
                CurPayPeriodID = 0;
            }
        }
        ucPayroll_PeriodInfo.CurrentPayPeriodID = CurPayPeriodID;

        if (CurPayPeriodID > 0)
        {
            panelPayPeriod.Visible = true;

        }
        else
        {
            panelPayPeriod.Visible = false;
            panelRollbackOption.Visible = false;
        }

        return true;
    }

    protected void PayGroupID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
    }
    protected void PayPeriodID_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadObject();
    }
    public void loadState()
    {

    }

    protected void btnRollback_Click(object sender, EventArgs e)
    {


        PageErrors pageErrors = PageErrors.getErrors(db, Page.Master);
        if (!IsValidRollbackKey(txtPassCode.Text))
        {
            pageErrors.addError("Incorrect pass code!");
            return;
        }
        else
        {
            PayrollProcess payrollProcess= new PayrollProcess(dbConn);
            WebUtils.StartFunction(Session, FUNCTION_CODE, true);
            payrollProcess.RollBackPayroll(CurPayPeriodID, WebUtils.GetCurUser(Session).UserID);
            WebUtils.EndFunction(dbConn);
            pageErrors.addError("Rollback Complete");
            binding.init(Request, Session);

            if (CurID > 0)
            {
                panelPayPeriod.Visible = true;
                loadObject();
            }
            else
            {
                panelPayPeriod.Visible = false;
                panelRollbackOption.Visible = false;
            }
        }
        loadState();

        //        Response.Redirect(Request.Url.LocalPath + "?" + Request.QueryString);
    }

    public bool IsValidRollbackKey(string trialKey)
    {
        DateTime permittedDate = getDateFromKey(trialKey);
        if (permittedDate.Equals(AppUtils.ServerDateTime().Date))
            return true;
        else
            return false;
    }

    private DateTime getDateFromKey(string trialKey)
    {
        HROne.CommonLib.Crypto crypto = new HROne.CommonLib.Crypto(HROne.CommonLib.Crypto.SymmProvEnum.DES);
        try
        {
            trialKey = base32.ConvertBase32ToBase64(trialKey);

            string realTrialKey = crypto.Decrypting(trialKey, "HROne");
            string strYear = realTrialKey.Substring(0, 4);
            string strMonth = realTrialKey.Substring(4, 2);
            string strDay = realTrialKey.Substring(6, 2);

            return new DateTime(int.Parse(strYear), int.Parse(strMonth), int.Parse(strDay));

        }
        catch
        {
            return new DateTime();
        }
    }

    //private class base32
    //{
    //    private static char[] b32cd;

    //    /// <summary>
    //    /// initilises the characters in base 32
    //    /// </summary>
    //    protected static void initbase32()
    //    {
    //        b32cd = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'T', 'U', 'V', 'W', 'X', 'Y', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    //    }

    //    public static string ConvertToBase32(byte[] byteArray)
    //    {
    //        initbase32();

    //        byte remainder = 0;
    //        ArrayList list = new System.Collections.ArrayList();
    //        string resultString = string.Empty;
    //        int shift = 0;
    //        foreach (byte byteData in byteArray)
    //        {
    //            shift += 8;
    //            int newValue = byteData;
    //            while (shift >= 5)
    //            {
    //                newValue = (int)(newValue + remainder * 256);
    //                shift -= 5;
    //                int divident = (int)(Math.Pow(2, shift));
    //                byte newRemainder = (byte)(newValue % divident);
    //                byte result = (byte)(newValue / divident);
    //                resultString += b32cd[result];
    //                newValue = newRemainder;
    //                remainder = 0;
    //            }
    //            remainder = (byte)newValue;
    //            //if (result >= b32cd.Length)
    //            //    resultString += b32cd[result % b32cd.Length] + b32cd[result / b32cd.Length];
    //            //else
    //        }

    //        if (shift != 0)
    //            resultString += b32cd[remainder * (int)Math.Pow(2, 5 - shift)];

    //        return resultString;

    //    }

    //    public static string ConvertBase64ToBase32(string source)
    //    {
    //        byte[] byteArray = Convert.FromBase64String(source);
    //        return ConvertToBase32(byteArray);
    //    }

    //    public static string ConvertBase32ToBase64(string source)
    //    {
    //        initbase32();

    //        char[] charArray = source.ToCharArray();
    //        ArrayList byteArray = new ArrayList();
    //        int shift = 0;
    //        int result = 0;
    //        for (int i = 0; i < charArray.Length; i++)
    //        {
    //            shift += 5;
    //            // find the index in the array that the char resides
    //            int valueindex = Array.IndexOf(b32cd, charArray[i]);
    //            result = result * 32 + valueindex;
    //            if (shift >= 8)
    //            {
    //                shift -= 8;
    //                int divident = (int)Math.Pow(2, shift);
    //                byte value = (byte)(result / divident);
    //                result = result % divident;
    //                byteArray.Add(value);
    //            }
    //        }
    //        if (shift != 0 && result != 0)
    //        {
    //            byteArray.Add((byte)(result * (int)Math.Pow(2, 5 - shift)));

    //        }
    //        return Convert.ToBase64String((byte[])byteArray.ToArray(typeof(byte)));
    //    }

    //    /// <summary>
    //    /// Encodes an int into a base 36 string
    //    /// if you were to alter the values in the b36cd then 
    //    /// this could be to any base value.
    //    /// </summary>
    //    /// <param name="value">(int) the input decimal value</param>
    //    /// <returns>(string) the output string base 36 converter</returns>
    //    public static string encodeb32(Int64 value, int numberOfDigit)
    //    {
    //        initbase32();   // set the char[] array
    //        string rv = ""; // starting value
    //        while (value != 0)
    //        {
    //            rv = b32cd[value % b32cd.Length] + rv;
    //            value /= b32cd.Length;
    //        }
    //        rv = rv.ToUpper();
    //        rv = rv.PadLeft(numberOfDigit, b32cd[0]);

    //        return rv;
    //    }


    //    /// <summary>
    //    /// This decodes the base 36 number into a decimal number
    //    /// though this could be used to decode any base number depending on the input
    //    /// on the char[] b36cd
    //    /// </summary>
    //    /// <param name="input"></param>
    //    /// <returns>(int) the decimal value of the base 36 number</returns>
    //    public static Int64 base32decode(string input)
    //    {
    //        initbase32();
    //        input = input.Trim();
    //        input = input.ToLower();
    //        Int64 rv = 0;
    //        // break string into characters
    //        char[] encchars = input.ToCharArray();
    //        // reverse the array
    //        Array.Reverse(encchars);
    //        // loop through the values
    //        for (int i = 0; i < encchars.Length; i++)
    //        {
    //            // find the index in the array that the char resides
    //            int valueindex = Array.IndexOf(b32cd, encchars[i]);
    //            // the actual value given by that is 
    //            // the index multiplied by the base number to the power of the index
    //            double temp = valueindex * Math.Pow(b32cd.Length, i);
    //            // add this value to the counter until there are no more values
    //            rv += Convert.ToInt64(temp);
    //        }
    //        // return the total result
    //        return rv;
    //    }

    //    public static string ReverseEncodeString(string base36string)
    //    {
    //        char[] encchars = base36string.ToLower().ToCharArray();
    //        for (int i = encchars.GetLowerBound(0); i <= encchars.GetUpperBound(0); i++)
    //            encchars[i] = b32cd[b32cd.GetUpperBound(0) - b32cd.GetLowerBound(0) - Array.IndexOf(b32cd, encchars[i]) - 1];
    //        return new string(encchars);
    //    }

    //    public static char CheckSum(string base36string)
    //    {
    //        char[] encchars = base36string.ToLower().ToCharArray();
    //        int checkSum = 0;
    //        for (int i = encchars.GetLowerBound(0); i <= encchars.GetUpperBound(0); i++)
    //        {
    //            checkSum += Array.IndexOf(b32cd, encchars[i]) - 1;
    //            checkSum %= encchars.Length;
    //        }
    //        return encchars[checkSum];
    //    }
    //}

}
