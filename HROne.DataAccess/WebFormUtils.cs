using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.Data;
namespace HROne.DataAccess
{
    public class WebFormUtils
    {

        public static void loadValues(DatabaseConnection DBConn, ListControl c, WFValueList vl, DBFilter filter)
        {
            loadValues(DBConn, c, vl, filter, null, null, "--");
        }

        public static void loadValues(DatabaseConnection DBConn, ListControl c, WFValueList vl, DBFilter filter, CultureInfo ci, string selected, string notSelected)
        {
            //if (selected != null && selected.Equals(string.Empty))
            //    selected = Common.EMPTY_STRING;

            List<WFSelectValue> list = vl.getValues(DBConn, filter, ci);
            c.Items.Clear();

            if (notSelected != null)
            {
                ResourceManager rm = DBUtils.getResourceManager();
                string s = rm.GetString(notSelected, ci);
                if (s == null)
                    s = notSelected;
                c.Items.Add(new ListItem(s, string.Empty));
            }

            foreach (WFSelectValue sv in list)
            {

                string key = sv.key;
                //if (key.Equals(""))
                //    key = Common.EMPTY_STRING;

                ListItem i = new ListItem(sv.name, key);
                if (selected != null && sv.key != null && sv.key.Equals(selected))
                    i.Selected = true;
                c.Items.Add(i);
            }

            if (selected != null)
            {
                ListItem item = c.Items.FindByValue(selected);
                if (item != null)
                    c.SelectedValue = item.Value;
            }
        }

        public static void LoadKeys(DBManager db, DataRowView row, WebControl c)
        {

            foreach (DBField f in db.keys)
            {
                object value = row[f.columnName];
                string s = f.populateValue(value);
                c.Attributes[f.name] = s;
            }
        }
        public static void GetKeys(DBManager db, DBObject o, WebControl c)
        {
            foreach (DBField f in db.keys)
            {
                string s = (string)c.Attributes[f.name];
                object value = f.parseValue(s);
                f.setValue(o, value);
            }

        }
    }

    public class WebFormWorkers
    {
        [Obsolete()]
        public static bool loadKeys(DBManager db, DBObject o, System.Web.HttpRequest Request)
        {

            foreach (DBField f in db.keys)
            {
                string str = Request[f.name];
                object val = f.parseValue(str);
                f.setValue(o, val);
            }
            return Request["_new"] != null && !Request["_new"].Equals("False");
        }
        public static bool loadKeys(DBManager db, DBObject o, System.Collections.Specialized.NameValueCollection QueryString)
        {

            foreach (DBField f in db.keys)
            {
                string str = QueryString[f.name];
                object val = f.parseValue(str);
                f.setValue(o, val);
            }
            return QueryString["_new"] != null && !QueryString["_new"].Equals("False");
        }
    }
}
