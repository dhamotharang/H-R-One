using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class DropDownVLBinder : Binder
    {
        DropDownList c;
        String name;
        DBField field;
        DBFilter filter;

        WFValueList vl;
        string notSelected = "combobox.notselected";
        string notSelectedValue = "";
        public DropDownVLBinder(DBManager db, DropDownList c, WFValueList vl)
            : this(db,c,vl,null,c.ID)
        {
        }
        public DropDownVLBinder(DBManager db, DropDownList c, WFValueList vl, DBFilter filter, string name)
        {
            this.filter = filter;
            this.c = c;
            this.name = name;
            this.field = db.getField(name);
            this.vl = vl;
        }

        public DropDownVLBinder(DBManager db, DropDownList c, WFValueList vl, DBFilter filter)
            : this(db, c, vl, filter, c.ID)
        {
        }
        public DropDownVLBinder setNotSelected(string notSelected)
        {
            this.notSelected = notSelected;
            return this;
        }
        public override void init(HttpRequest Request, HttpSessionState Session)
        {
            if (vl != null)
            {
                string selectedValue = c.SelectedValue;
                if (selectedValue != null)
                    selectedValue = selectedValue.Trim();
                System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
                WebFormUtils.loadValues(Binding.DBConn, c, vl, filter, ci, selectedValue, notSelected);
            }
            if (field.required)
                c.CssClass = "pm_required";
        }
        public override void toControl(Hashtable values)
        {
            if (c.Items.Count > 0)
            {

                object o = values[name];
                if (o != null)
                {
                    string selected = o.ToString();
                    selected = selected.Trim();
                    if (selected.Equals(""))
                        selected = Common.EMPTY_STRING;
                    ListItem selectedItem = c.Items.FindByValue(selected);
                    if (selectedItem != null)
                    {
                        selectedItem.Selected = true;
                        c.SelectedValue = selectedItem.Value;
                        return;
                    }
                    //foreach (ListItem item in c.Items)
                    //{
                    //    if (selected != null && selected.Equals(item.Value))
                    //    {
                    //        item.Selected = true;
                    //        c.SelectedValue = item.Value;
                    //        break;
                    //    }
                    //}
                }
                c.SelectedValue = c.Items[0].Value;
            }
        }
        public override void toValues(Hashtable values)
        {
            string s = c.SelectedValue;
            if (s.Equals(Common.EMPTY_STRING))
                s = "";
            else if (s.Equals(""))
                //s=Common.NULL_STRING;
                s = notSelectedValue;
            else
                s = s.Trim();
            values.Add(name, s);
        }
    }
}
