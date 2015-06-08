using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace HROne.DataAccess
{
    public class ListInfo
    {
        public int page;
        public int recordPerPage = 20;
        public int numPage;
        public int numRecord;
        public bool order = true;
        public string orderby = "";
        public int startRecord
        {
            get { return numRecord == 0 ? 0 : recordPerPage * page + 1; }
        }
        public int endRecord
        {
            get { int rec = recordPerPage * (page + 1); if (numRecord < rec)return numRecord; else return rec; }
        }

        public void loadPageList(DropDownList PageList, WebControl PrevPage, WebControl NextPage, WebControl FirstPage, WebControl LastPage)
        {
            loadPageList(PageList, PrevPage, NextPage);
            if (FirstPage != null)
            {
                FirstPage.Enabled = page != 0;
            }
            if (LastPage != null)
            {
                LastPage.Enabled = page < numPage - 1;
            }
        }

        public void loadPageList(DropDownList PageList, WebControl PrevPage, WebControl NextPage)
        {
            if (recordPerPage < 0)
                return;
            if (PageList != null)
            {
                PageList.Items.Clear();
                for (int i = 0; i < numPage; i++)
                    PageList.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                PageList.SelectedIndex = page;
            }
            if (PrevPage != null)
            {
                PrevPage.Enabled = (page != 0);
                NextPage.Enabled = (page < numPage - 1);
            }

        }

    }
}
