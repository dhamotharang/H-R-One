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
using HROne.DataAccess;
//using perspectivemind.validation;
using HROne.Lib.Entities;

//public class EmpUtils
//{
//    public static void DeleteEmp(int EmpID)
//    {
//        DBFilter filter=new DBFilter();
//        filter.add(new Match("EmpID", EmpID));


//        EEmpAVCPlan.db.delete(dbConn, filter);
//        EEmpBankAccount.db.delete(dbConn, filter);
//        EEmpContractTerms.db.delete(dbConn, filter);
//        EEmpDependant.db.delete(dbConn, filter);
//        EEmpFinalPayment.db.delete(dbConn, filter);
//        EEmpHierarchy.db.delete(dbConn, filter);
//        EEmpMPFPlan.db.delete(dbConn, filter);
//        EEmpPersonalInfo.db.delete(dbConn, filter);
//        EEmpPlaceOfResidence.db.delete(dbConn, filter);
//        EEmpPositionInfo.db.delete(filter);
//        EEmpQualification.db.delete(filter);
//        EEmpRecurringPayment.db.delete(filter);
//        EEmpSkill.db.delete(filter);
//        EEmpSpouse.db.delete(filter);
//        EEmpTermination.db.delete(filter);
//        EEmpFinalPayment.db.delete(filter);

//        ELeaveApplication.db.delete(filter);
//        ELeaveBalance.db.delete(filter);
//        ELeaveBalanceAdjustment.db.delete(filter);

//        EClaimsAndDeductions.db.delete(filter);
//        ArrayList empPayrolllList = EEmpPayroll.db.select(filter);
//        foreach (EEmpPayroll empPayrll in empPayrolllList)
//        {
//            DBFilter empPayrollFilter = new DBFilter();
//            empPayrollFilter.add(new Match("EmpPayrollID", empPayrll.EmpPayrollID));
//            EPaymentRecord.db.delete(empPayrollFilter);
//        }
//        EEmpPayroll.db.delete(filter);
        

//    }
//}
public class HKIDLabel : LabelBinder
{
    public HKIDLabel(DBManager db, Label c, string name)
        : base(db, c, name)
    {
        
    }

    public override void toControl(Hashtable values)
    {
        base.toControl(values);
        if(c.Text.Equals("()"))
            c.Text="";
    }

}
public class HKIDBinder : TextBoxBinder
{
    TextBox Digit;
    public HKIDBinder(DBManager db, TextBox c, TextBox Digit)
        : base(db, c)
    {
        this.Digit = Digit;
    }
    public override void toControl(Hashtable values)
    {
        object o = values[name];
        string s;
        if (o != null)
            s = o.ToString();
        else
            s = "";

        int index = s.LastIndexOf('(');
        if (index < 0)
        {
            c.Text = s;
            Digit.Text = "";
        }
        else
        {
            c.Text = s.Substring(0, index);
            if (s.Length > index + 2)
                Digit.Text = s.Substring(index + 1, 1);
        }
    }
    public override void toValues(Hashtable values)
    {
        values.Add(name, c.Text + "(" + Digit.Text + ")");
    }

}
