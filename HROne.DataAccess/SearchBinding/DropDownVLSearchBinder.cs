using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Globalization;

namespace HROne.DataAccess
{
    public class DropDownVLSearchBinder : SearchBinder
    {

        string fieldName;
        DropDownList c;
        WFValueList vl;
        CultureInfo ci;
        DBFilter filter;
        string notSelected = "combobox.notselected";
        bool hasNotSelected = true;
        public DropDownVLSearchBinder(DropDownList c, string fieldName, WFValueList vl)
            : this(c, fieldName, vl, true)
        {
        }
        public DropDownVLSearchBinder(DropDownList c, string fieldName, WFValueList vl, bool hasNotSelected)
        {
            this.fieldName = fieldName;
            this.c = c;
            this.vl = vl;
            this.hasNotSelected = hasNotSelected;
        }
        public DropDownVLSearchBinder setLocale(CultureInfo ci)
        {
            this.ci = ci;
            return this;
        }
        public DropDownVLSearchBinder setFilter(DBFilter filter)
        {
            this.filter = filter;
            return this;
        }
        public override void init(System.Collections.Specialized.NameValueCollection Request, SearchContext context, SearchBinding binding, Hashtable values, DBManager db, System.EventHandler search)
        {
            c.SelectedIndexChanged += search;
            if (vl != null)
            {
                string selectedValue = c.SelectedValue;
                if (selectedValue != null)
                    selectedValue = selectedValue.Trim();
                //CultureInfo ci= System.Globalization.CultureInfo.CurrentUICulture;
                if (hasNotSelected)
                    WebFormUtils.loadValues(binding.DBConn, c, vl, this.filter, ci, selectedValue, notSelected);
                else
                    WebFormUtils.loadValues(binding.DBConn, c, vl, this.filter, ci, selectedValue, null);
            }
        }

        public override bool buildFilter(DBFilter filter)
        {
            if ((hasNotSelected && c.SelectedIndex > 0) || (!hasNotSelected && c.SelectedIndex >= 0))
            {
                string column;
                column = fieldName;

                string s = c.SelectedValue;
                if (s.Equals(Common.EMPTY_STRING))
                    return false;
                //				object v=f.parseValue(s);
                filter.add(new Match(column, s));
                return true;
            }
            else
            {
                return false;
            }


        }
        public override void buildFilterFilter(DBFilter filter)
        {
            if (c.SelectedIndex > 0)
            {
                string column;
                column = fieldName;

                string s = c.SelectedValue;
                //object v=f.parseValue(s);
                filter.add(new Match(column, s));
            }
        }
    }
}
