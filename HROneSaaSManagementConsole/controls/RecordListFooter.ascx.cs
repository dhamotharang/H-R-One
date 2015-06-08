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

public partial class RecordListFooter : HROneWebControl
{

    private ListInfo info = new ListInfo();

    public event EventHandler FirstPageClick;
    public event EventHandler PrevPageClick;
    public event EventHandler NextPageClick;
    public event EventHandler LastPageClick;

    public bool ShowAllRecords
    {
        get { return info.recordPerPage <=0; }
        set
        {
            if (value)
            {
                info.recordPerPage = 0; 
                RecordsPerPage.Value = "0";
            }
            else
            {
            }
        }
    }

    public ListInfo ListInfo
    {
        get { return info; }
        set { info = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        loadState();
        lblRecordCount.Text = string.Empty;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        Refresh();
    }

    public string ListOrderBy
    {
        get
        {
            return OrderBy.Value;
        }
        set
        {
            OrderBy.Value = value;
            info.orderby = value;
        }
    }
    public bool ListOrder
    {
        get
        {
            return Order.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase); 
        }
        set
        {
            Order.Value = value.ToString();
            info.order = value;
        }
    }
    public void loadState()
    {
        int page = 0;
        if (!CurPage.Value.Equals(""))
            page = Int32.Parse(CurPage.Value);
        info.page = page;
        int numRecord = 0;
        if (!NumRecord.Value.Equals(""))
            numRecord = Int32.Parse(NumRecord.Value);
        info.numRecord = numRecord;
        //info.loadState(Request, page);
        info.order = Order.Value.Equals("True");
        info.orderby = OrderBy.Value;
        if (info.orderby == "")
            info.orderby = null;

        int recordsPerPage = info.recordPerPage; ;
        if (int.TryParse(RecordsPerPage.Value, out recordsPerPage))
        {
            info.recordPerPage = recordsPerPage;
            RecordsPerPage.Value = recordsPerPage.ToString();
        }
        else if (int.TryParse(HROne.Lib.Entities.ESystemParameter.getParameter(dbConn, HROne.Lib.Entities.ESystemParameter.PARAM_CODE_DEFAULT_RECORDS_PER_PAGE), out recordsPerPage))
        {
            info.recordPerPage = recordsPerPage;
            RecordsPerPage.Value = recordsPerPage.ToString();
        }
        if (info.recordPerPage > 0)
        {
            info.numPage = info.numRecord / info.recordPerPage;
            if (info.numRecord % info.recordPerPage > 0)
                info.numPage++;
            if (info.page == info.numPage && info.numPage > 0)
                info.page--;

        }
    }

    public void Refresh()
    {
        if (info.recordPerPage <= 0)
        {
            PrevPage.Visible = false;
            PrevPageImg.Visible = false;
            NextPage.Visible = false;
            NextPageImg.Visible = false;
            FirstPage.Visible = false;
            FirstPageImg.Visible = false;
            LastPage.Visible = false;
            LastPageImg.Visible = false;
        }
        else
        {
            PrevPage.Visible = true;
            PrevPageImg.Visible = true;
            NextPage.Visible = true;
            NextPageImg.Visible = true;
            FirstPage.Visible = true;
            FirstPageImg.Visible = true;
            LastPage.Visible = true;
            LastPageImg.Visible = true;

            info.loadPageList(null, PrevPage, NextPage, FirstPage, LastPage);
            loadPageList(PrevPage, PrevPageImg, NextPage, NextPageImg, FirstPage, FirstPageImg, LastPage, LastPageImg);
        }
        CurPage.Value = info.page.ToString();
        NumPage.Value = info.numPage.ToString();
        NumRecord.Value = info.numRecord.ToString();
        Order.Value = info.order.ToString();
        OrderBy.Value = info.orderby == null ? "" : info.orderby;

        if (info.numRecord <= info.recordPerPage || info.recordPerPage <= 0)

            lblRecordCount.Text = info.numRecord + " " + HROne.Common.WebUtility.GetLocalizedString("Records");
        else
            lblRecordCount.Text = "( " + HROne.Common.WebUtility.GetLocalizedString("Records") + ": " + info.startRecord + " - " + info.endRecord + " of " + info.numRecord + " )";

    }

    protected void FirstPage_Click(object sender, EventArgs e)
    {
        info.page = 0;
        //view = loadData(info, db, Repeater);
        FirstPageClick(sender, e);
    }
    protected void PrevPage_Click(object sender, EventArgs e)
    {
        info.page--;
        //view = loadData(info, db, Repeater);
        PrevPageClick(sender, e);

    }
    protected void NextPage_Click(object sender, EventArgs e)
    {
        info.page++;
        //view = loadData(info, db, Repeater);
        NextPageClick(sender, e);

    }
    protected void LastPage_Click(object sender, EventArgs e)
    {

        info.page = Int32.Parse(NumPage.Value);
        //view = loadData(info, db, Repeater);
        LastPageClick(sender, e);

    }

    public void loadPageList(LinkButton PrevPage, HtmlImage PrevPageImg,
    LinkButton NextPage, HtmlImage NextPageImg,
    LinkButton FirstPage, HtmlImage FirstPageImg,
    LinkButton LastPage, HtmlImage LastPageImg)
    {
        string s = "~";//PrevPage.Page.Request.ApplicationPath;
        if (PrevPage.Enabled)
            PrevPageImg.Src = s + "/images/previous.gif";
        else
            PrevPageImg.Src = s + "/images/previous_off.gif";
        if (NextPage.Enabled)
            NextPageImg.Src = s + "/images/next.gif";
        else
            NextPageImg.Src = s + "/images/next_off.gif";
        if (FirstPage.Enabled)
            FirstPageImg.Src = s + "/images/start.gif";
        else
            FirstPageImg.Src = s + "/images/start_off.gif";
        if (LastPage.Enabled)
            LastPageImg.Src = s + "/images/end.gif";
        else
            LastPageImg.Src = s + "/images/end_off.gif";
    }
}
