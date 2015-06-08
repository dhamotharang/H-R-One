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
using HROne.Import;
using HROne.Lib;
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Payroll_SalaryIncrementBatch_Edit : HROneWebPage
{
    private const string FUNCTION_CODE = "PAY016";

    public Binding binding;
    public SearchBinding detailBinding;
    public DBManager db = ESalaryIncrementBatch.db;
    public ESalaryIncrementBatch obj;
    protected ListInfo info;
    protected DataView view;

    public int CurID = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        if (ESystemParameter.getParameter(dbConn, ESystemParameter.PARAM_CODE_PAYSCALE_POINT_SYSTEM) != "Y")
        {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "~/AccessDeny.aspx");
            return;
        }

        toolBar.FunctionCode = FUNCTION_CODE;

        #region "binding for Process information"
        binding = new Binding(dbConn, db);
        //binding.add(BackpayProcessID);
        binding.add(new TextBoxBinder(db, AsAtDate.TextBox, AsAtDate.ID));
        binding.add(new TextBoxBinder(db, PaymentDate.TextBox, PaymentDate.ID));
        binding.add(new CheckBoxBinder(db, DeferredBatch));
        binding.add(new LabelVLBinder(db, Status, ESalaryIncrementBatch.VLStatusDesc));
        binding.add(new DropDownVLBinder(db, PaymentCodeID, EPaymentCode.VLPaymentCode));

        binding.add(UploadDateTime);
        binding.add(ConfirmDateTime);
        // binding.add(UploadEmpID); // load employee name from LoadData
        // binding.add(ConfirmEmpID);   // load employee name from LoadData

        binding.init(Request, Session);
        #endregion

        #region"binding for Process Detail"
        detailBinding = new SearchBinding(dbConn, db);
        detailBinding.init(DecryptedRequest, null);
        #endregion

        if (!int.TryParse(DecryptedRequest["BatchID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
                if (Status.Text != ESalaryIncrementBatch.STATUS_OPEN_DESC)
                {
                    btnExport.Visible = false;
                    toolBar.DeleteButton_Visible = true;
                    toolBar.SaveButton_Visible = true;
                }
                else
                {   
                    // Open status
                    btnExport.Visible = true;
                    toolBar.DeleteButton_Visible = true;
                    toolBar.SaveButton_Visible = true;
                }
            }
            else
            {
                btnExport.Visible = false;
                toolBar.DeleteButton_Visible = false;
                toolBar.SaveButton_Visible = true;
            }

            //init_PaymentCodeDropdown();
            CNDRow.Visible = DeferredBatch.Checked;
        }
        info = ListFooter.ListInfo;
        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

    }

    //protected void init_PaymentCodeDropdown()
    //{
    //    if (PaymentCode.Items.Count <= 0)
    //    {
    //        PaymentCode.Items.Add("Not Selected");

    //        DBFilter m_filter = new DBFilter();
    //        m_filter.add("PaymentCode", true);

    //        DataTable m_table = AppUtils.runSelectSQL("PaymentCode, PaymentCodeDesc ", "From PaymentCode ", m_filter, dbConn);

    //        foreach (DataRow row in m_table.Rows)
    //        {
    //            PaymentCode.Items.Add(string.Format("{0} - {1}", row["PaymentCode"], row["PaymentCodeDesc"]));
    //        }
    //    }
    //}

    protected void DeferredBatch_OnClick(object sender, EventArgs e)
    {
        CNDRow.Visible = DeferredBatch.Checked;
    }
    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            view = loadData(info, db, Repeater);
        }
    }

    protected void ChangeOrder_Click(object sender, EventArgs e)
    {
        LinkButton l = (LinkButton)sender;
        String id = l.ID.Substring(1);
        if (info.orderby == null)
            info.order = true;
        else if (info.orderby.Equals(id))
            info.order = !info.order;
        else
            info.order = true;
        info.orderby = id;

        view = loadData(info, db, Repeater);
    }

    protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView row = (DataRowView)e.Item.DataItem;
        CheckBox cb = (CheckBox)e.Item.FindControl("ItemSelect");
        WebFormUtils.LoadKeys(db, row, cb);
        cb.Checked = true;
    }

    protected DataView loadData(ListInfo info, DBManager db, Repeater repeater)
    {
        DBFilter filter = detailBinding.createFilter();

        if (info != null && info.orderby != null && !info.orderby.Equals(""))
            filter.add(info.orderby, info.order);
        else
            filter.add("emp.EmpNo", true);

        filter.add(new Match("d.BatchID", CurID));

        //filter.add(WebUtils.AddRankFilter(Session, "e.EmpID", true));

        /****************************************************************************  
        * SELECT d.BatchID, d.DetailID, emp.EmpNo, emp.EmpEngSurname, emp.EmpEngOthername, emp.EmpAlias, d.SchemeCode, d.Capacity, d.CurrentPoint, d.NewPoint 
        * FROM [PS_SalaryIncrementBatchDetail] AS d
        * INNER JOIN [EmpPersonalInfo] AS emp ON d.EmpID = emp.EmpID
         ****************************************************************************/

        string select = "d.BatchID, d.DetailID, emp.EmpID, emp.EmpNo, emp.EmpEngSurname, emp.EmpEngOthername, emp.EmpAlias, d.SchemeCode, d.Capacity, d.CurrentPoint, d.NewPoint ";
        string from = "FROM [PS_SalaryIncrementBatchDetail] AS d " +
                      "INNER JOIN [EmpPersonalInfo] AS emp ON d.EmpID = emp.EmpID ";

        DataTable table = filter.loadData(dbConn, null, select, from);

        foreach (DataColumn col in table.Columns)
        {
            if (col.DataType.Equals(typeof(string)))
            {
                DBAESEncryptStringFieldAttribute.decode(table, col.ColumnName);
            }
        }

        //table = EmployeeSearchControl1.FilterEncryptedEmpInfoField(table, info);
        view = new DataView(table);

        ListFooter.Refresh();
        if (repeater != null)
        {
            repeater.DataSource = view;
            repeater.DataBind();
        }

        // get BackpayProcess
        ESalaryIncrementBatch m_process = ESalaryIncrementBatch.GetObject(dbConn, CurID);
        EUser m_user;
        int m_id;

        UploadBy.Text = "";
        ConfirmBy.Text = "";
        if (int.TryParse(UploadBy.Text, out m_id))
        {
            m_user = EUser.GetObject(dbConn, m_id);
            if (m_user != null)
            {
                UploadBy.Text = m_user.UserName;
            }
        }
        if (int.TryParse(ConfirmBy.Text, out m_id))
        {
            m_user = EUser.GetObject(dbConn, m_id);
            if (m_user != null)
            {
                ConfirmBy.Text = m_user.UserName;
            }
        }

        //if (m_process.PaymentCodeID > 0)
        //{
        //    EPaymentCode m_paymentcode = EPaymentCode.GetObject(dbConn, m_process.PaymentCodeID);
        //    if (m_paymentcode != null)
        //    {
        //        PaymentCode.Text = m_paymentcode.PaymentCode + " - " + m_paymentcode.PaymentCodeDesc;
        //    }

        //}

        return view;
    }

    protected bool loadObject()
    {
        obj = new ESalaryIncrementBatch();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        ESalaryIncrementBatch c = new ESalaryIncrementBatch();
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        DateTime m_asAtDate = new DateTime();
        DateTime m_paymentDate = new DateTime();
        int m_recordsGenerated = 0;
//        string m_paymentCode = "";

        errors.clear();

        if (!DateTime.TryParse(AsAtDate.Value, out m_asAtDate))
        {
            errors.addError("Invalid As At Date");
            return;
        }
        if (DeferredBatch.Checked)
        {
            if (!DateTime.TryParse(PaymentDate.Value, out m_paymentDate))
            {
                errors.addError("Invalid Payment Date");
                return;
            }
            if (PaymentCodeID.SelectedIndex <= 0)
            {
                errors.addError("Invalid Payment Code");
                return;
            }
        }

        WebUtils.StartFunction(Session, FUNCTION_CODE);

        if (CurID < 0)
        {
            c.AsAtDate = m_asAtDate;
            c.Status = ESalaryIncrementBatch.STATUS_OPEN;
            if (DeferredBatch.Checked)
            {
                c.DeferredBatch = DeferredBatch.Checked;
                c.PaymentDate = m_paymentDate;
                c.PaymentCodeID = System.Convert.ToInt32(PaymentCodeID.Text);
            }
            db.insert(dbConn, c);
            CurID = c.BatchID;

            m_recordsGenerated = GenerateInitialData(errors);

            if (errors.isEmpty())
            {
                if (m_recordsGenerated > 0)
                {
                    string m_message = "Batch initialization completed.  " + m_recordsGenerated.ToString("0") + " employee(s) added.";
                    string m_url = "Payroll_SalaryIncrementBatch_View.aspx?BatchID=" + CurID;

                    WebUtils.RegisterRedirectJavaScript(this, m_url, HROne.Common.WebUtility.GetLocalizedString(m_message));

                }
            }        
        }
        else
        {
            // retrieve current object....
            c.BatchID = CurID;
            if (ESalaryIncrementBatch.db.select(dbConn, c))
            {
                c.AsAtDate = m_asAtDate;
                if (DeferredBatch.Checked)
                {
                    c.DeferredBatch = DeferredBatch.Checked;
                    c.PaymentDate = m_paymentDate;
                    c.PaymentCodeID = System.Convert.ToInt32(PaymentCodeID.Text);
                }
                else
                {
                    c.DeferredBatch = false;
                    c.PaymentDate = new DateTime();
                    c.PaymentCodeID = 0;
                }
                db.update(dbConn, c);
            }
        }

        WebUtils.EndFunction(dbConn);
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        ESalaryIncrementBatch o = new ESalaryIncrementBatch();
        o.BatchID = CurID;
        if (ESalaryIncrementBatch.db.select(dbConn, o))
        {
            if (o.Status == ESalaryIncrementBatch.STATUS_OPEN)
            {
                dbConn.BeginTransaction();

                WebUtils.StartFunction(Session, FUNCTION_CODE);

                DBFilter detailFilter = new DBFilter();
                detailFilter.add(new Match("BatchID", o.BatchID));
                foreach (ESalaryIncrementBatchDetail d in ESalaryIncrementBatchDetail.db.select(dbConn, detailFilter))
                {
                    ESalaryIncrementBatchDetail.db.delete(dbConn, d);
                }

                db.delete(dbConn, o);
                WebUtils.EndFunction(dbConn);

                dbConn.CommitTransaction();
            }
            else if (o.Status == ESalaryIncrementBatch.STATUS_CONFIRMED || o.Status == ESalaryIncrementBatch.STATUS_APPLIED)
            {
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_REMOVE_BATCH);
            }
        }

        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_List.aspx");
    }
    protected void Back_Click(object sender, EventArgs e)
    {
        if (CurID < 0)
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_List.aspx");
        else
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Payroll_SalaryIncrementBatch_View.aspx?BatchID=" + CurID);
    }

    protected int GenerateInitialData(PageErrors errors)
    {
        DateTime m_AsAtDate = new DateTime();
        int m_count = 0;
        if (!(DateTime.TryParse(AsAtDate.Value, out m_AsAtDate)))
        {
            errors.addError("Invalid Date Format: As At Date");
            return 0;
        }

        if (CurID < 0)
        {
            errors.addError("Cannot get BatchID");
            return 0; 
        }

        if (errors.isEmpty())
        {
            // clear current batch detail
            DBFilter m_BatchDetailFilter = new DBFilter();
            m_BatchDetailFilter.add(new Match("BatchID", CurID));
            ESalaryIncrementBatchDetail.db.delete(dbConn, m_BatchDetailFilter);

            // select as-at-month and as-at-month-1 join-date employee, remove those join-day <=15 (of as-at-month-1) and join-day > 16(of as-at-month)
            DBFilter m_allFilter = new DBFilter();
            DBFilter m_recurringPaymentFilter = new DBFilter();

            m_recurringPaymentFilter.add(new Match("Point", ">=", 0));

            m_allFilter.add(new Match("EmpNextSalaryIncrementDate", m_AsAtDate));
            m_allFilter.add(new IN("EmpID", "SELECT EmpID FROM EmpRecurringPayment", m_recurringPaymentFilter));

            m_allFilter.add(WebUtils.AddRankFilter(Session, "EmpID", true));

            ArrayList m_list = EEmpPersonalInfo.db.select(dbConn, m_allFilter);

            if (m_list.Count > 0)
            {
                DBFilter m_paymentTypeFilter = new DBFilter();
                m_paymentTypeFilter.add(new Match("PaymentTypeCode", "BASICSAL"));

                DBFilter m_paymentCodeFilter = new DBFilter();
                m_paymentCodeFilter.add(new IN("PaymentTypeID", "SELECT PaymentTypeID FROM PaymentType", m_paymentTypeFilter));

                ArrayList m_paymentCodeList = EPaymentCode.db.select(dbConn, m_paymentCodeFilter);

                int[] m_paymentCodeIDList = AppUtils.ObjectList2IDList(m_paymentCodeList, "PaymentCodeID");

                foreach (EEmpPersonalInfo empInfo in m_list)
                {
                    if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                    {
                        DBFilter m_recurringFilter = new DBFilter();
                        m_recurringFilter.add(new NullTerm("EmpRPEffTo"));
                        m_recurringFilter.add(new Match("EmpID", empInfo.EmpID));
//                        m_recurringFilter.add(new IN("PayCodeID", m_paymentCodeIDList));

                        foreach (EEmpRecurringPayment rp in EEmpRecurringPayment.db.select(dbConn, m_recurringFilter))
                        {
                            m_count++;
                            ESalaryIncrementBatchDetail m_detail = new ESalaryIncrementBatchDetail();

                            m_detail.BatchID = CurID;
                            m_detail.EmpID = empInfo.EmpID;
                            m_detail.SchemeCode = rp.SchemeCode;
                            m_detail.Capacity = rp.Capacity;
                            m_detail.CurrentPoint = rp.Point;
                            m_detail.EmpRPID = rp.EmpRPID;

                            ESalaryIncrementBatchDetail.db.insert(dbConn, m_detail);
                        }
                    }
                }

                return m_count;
            }
            else
            {
                errors.addError("No employee in range");
            }
        }
        return m_count;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        foreach (RepeaterItem i in Repeater.Items)
        {
            CheckBox cb = (CheckBox)i.FindControl("ItemSelect");
            if (cb.Checked)
            {
                HtmlInputHidden m_hiddenID = (HtmlInputHidden)i.FindControl("DetailID");

                ESalaryIncrementBatchDetail o = new ESalaryIncrementBatchDetail();
                o.DetailID = int.Parse(m_hiddenID.Value);
                ESalaryIncrementBatchDetail.db.delete(dbConn, o);
            }
        }

        loadData(info, db, Repeater);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();
        
        int m_recordsGenerated = GenerateInitialData(errors);

        if (errors.isEmpty())
        {
            string m_message = "Batch initialization completed.  " + m_recordsGenerated.ToString("0") + " employee(s) added.";
            //string m_url = "Payroll_SalaryIncrementBatch_View.aspx?BatchID=" + CurID;

            errors.addError(m_message);
        }


    }
}
