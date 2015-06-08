using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class LikeSearchBinder : SearchBinder
    {
        string[] fieldList;
        TextBox c;
        public LikeSearchBinder(TextBox c, string fieldName)
            : this(c, new string[] { fieldName })
        {

        }
        //  currently not support search with multiple field if column is encrypted and should not support
        protected LikeSearchBinder(TextBox c, string[] fieldList)
        {
            this.fieldList = fieldList;
            this.c = c;

        }
        public override bool buildFilter(DBFilter filter)
        {
            if (c.Text.Length > 0)
            {
                OR or = new OR();
                foreach (string fieldName in fieldList)
                {
                    or.add(new Match(fieldName, "LIKE", "%" + c.Text + "%"));
                }
                if (or.terms().Count > 0)
                    if (or.terms().Count > 1)
                        filter.add(or);
                    else
                        filter.add(or.terms()[0]);
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void clear()
        {
            this.c.Text = "";
        }
    }
}
