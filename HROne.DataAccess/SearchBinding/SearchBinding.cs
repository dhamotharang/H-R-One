using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public interface SearchContext
    {
        void buildFilterFilter(DBFilter filter);
        void buildApplicationFilterFilter(DBFilter filter);
        //ArrayList getDistintValues(DBField f, DBFilter filter);
    }

    public class SearchBinding : SearchContext
    {
        public ArrayList binders = new ArrayList();
        public Hashtable values = new Hashtable();
        public ArrayList terms = new ArrayList();
        public ArrayList filterTerms = new ArrayList();

        public DBManager db;
        public DatabaseConnection DBConn = null;

        public SearchBinding(DatabaseConnection DBConn, DBManager db)
        {
            this.DBConn = DBConn;
            this.db = db;
        }
        public void initValues(string name, DBFilter filter, WFValueList vl, CultureInfo ci)
        {
            Hashtable map = new Hashtable();
            List<WFSelectValue> list = vl.getValues(DBConn, filter, ci);
            foreach (WFSelectValue v in list)
            {
                map[v.key] = v.name;
            }
            values[name] = map;
        }
        public void initValues()
        {
            //foreach (DBField f in db.fields)
            //{
            //    if (f.vl != null)
            //    {
            //        Hashtable map = new Hashtable();
            //        f.vl.loadValues(map);
            //        values[f.name] = map;
            //    }
            //}
        }
        public void init(System.EventHandler search)
        {
            init(null, search);
        }
        public void init(System.Collections.Specialized.NameValueCollection Request, System.EventHandler search)
        {
            initValues();
            foreach (SearchBinder b in binders)
            {
                b.init(Request, this, this, values, db, search);
            }
        }
        public string getJSValue(object v, string name)
        {
            string s = getValue(v, name);

            s = s.Replace("'", "\'");
            s = s.Replace("\"", "\\\"");
            return s;
        }
        public string getValue(object c, object v, string name)
        {
            return getValue(v, name);
        }
        public string getValue(object v, string name)
        {
            object s = null;
            if (v is DataRowView)
            {
                DataRowView r = (DataRowView)v;
                s = r[name];
            }
            else if (v is DataRow) 
            {
                DataRow r = (DataRow)v;
                s = r[name];
            }
            else if (v is DBObject)
            {
                System.Reflection.PropertyInfo propInfo = v.GetType().GetProperty(name);
                if (propInfo != null)
                    s = propInfo.GetValue(v, null);
            }

            if (s == null || Convert.IsDBNull(s))
                return "";

            if (db != null)
            {
                DBField f = db.getField(name);
                if (f != null && f.transcoder != null)
                    s = f.transcoder.fromDB(s);
            }

            Hashtable map = (Hashtable)values[name];
            if (map != null && s != null)
            {
                object s1 = map[s.ToString()];
                if (s1 == null)
                {
                    try
                    {
                        s1 = map[Convert.ToInt32(s)];
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
                if (s1 != null)
                    s = s1;
            }


            s = s.ToString().Trim();
            return HTMLUtils.toHTMLText(s.ToString());
        }
        public string getFValue(object v, string name, string format)
        {

            object s = null;
            if (v.GetType() == typeof(DataRowView))
            {
                DataRowView r = (DataRowView)v;
                s = r[name];
            }
            else if (v is DataRow)
            {
                DataRow r = (DataRow)v;
                s = r[name];
            }
            else if (v is DBObject)
            {
                System.Reflection.PropertyInfo propInfo = v.GetType().GetProperty(name);
                if (propInfo != null)
                    s = propInfo.GetValue(v, null);
            }

            if (s == null || Convert.IsDBNull(s))
                return "";

            if (db != null)
            {
                DBField f = db.getField(name);
                if (f != null && f.transcoder != null)
                    s = f.transcoder.fromDB(s);
            }

            Hashtable map = (Hashtable)values[name];
            if (map != null && s != null)
            {
                object s1 = map[s.ToString()];
                if (s1 == null)
                {
                    try
                    {
                        s1 = map[Convert.ToInt32(s)];
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
                if (s1 != null)
                    s = s1;
            }

            if (string.IsNullOrEmpty(format) && db != null)
            {
                DBField field = db.getField(name);
                if (field != null)
                {
                    s = field.populateValue(s);
                }
                else
                {
                    s = s.ToString().Trim();
                }
            }
            else if (s is DateTime)
            {
                DateTime dt = (DateTime)s;
                if (dt.Ticks == 0)
                    return "";
                if (format != null)
                    return ((DateTime)s).ToString(format);

            }
            else if (s is decimal)
            {
                if (format != null)
                    s = ((decimal)s).ToString(format);
            }
            else if (s is double)
            {
                if (Double.IsNaN((double)s))
                    s = "";
                else if (format != null)
                    s = ((double)s).ToString(format);
            }
            else if (s is float)
            {
                if (float.IsNaN((float)s))
                    s = "";
                else if (format != null)
                    s = ((float)s).ToString(format);
            }
            else if (s is int)
            {
                if (format != null)
                    s = ((int)s).ToString(format);
            }
            else if (s is long)
            {
                if (format != null)
                    s = ((long)s).ToString(format);

            }
            return HTMLUtils.toHTMLText(s.ToString().Trim());
        }

        public string getFValue(object v, string name)
        {
            return getFValue(v, name, string.Empty);
            //object s = null;
            //if (v.GetType() == typeof(DataRowView))
            //{
            //    DataRowView r = (DataRowView)v;
            //    s = r[name];
            //}
            //else
            //{
            //    DataRow r = (DataRow)v;
            //    s = r[name];
            //}

            //if (s == null || Convert.IsDBNull(s))
            //    return "";


            //Hashtable map = (Hashtable)values[name];
            //if (map != null)
            //{
            //    object s1 = map[s];
            //    if (s1 == null)
            //    {
            //        try
            //        {
            //            s1 = map[Convert.ToInt32(s)];
            //        }
            //        catch (Exception ex)
            //        {
            //            System.Diagnostics.Debug.WriteLine(ex.Message);
            //        }
            //    }
            //    if (s1 != null)
            //        s = s1;
            //}
            //DBField field = db.getField(name);
            //if (field != null)
            //{
            //    s = field.populateValue(s);
            //}
            //else
            //{
            //    s = s.ToString().Trim();
            //}
            //return HTMLUtils.toHTMLText(s.ToString());
        }

        //public string getTValue(object v, string name)
        //{
        //    DataRowView r = (DataRowView)v;
        //    object oo = r[name];
        //    if (oo == null || Convert.IsDBNull(oo))
        //        return "";
        //    string s = oo.ToString();

        //    Hashtable map = (Hashtable)values[name];
        //    if (map != null)
        //    {
        //        try
        //        {
        //            int index = Convert.ToInt32(s);
        //            string s1 = (string)map[index];
        //            if (s1 != null)
        //                s = s1;

        //        }
        //        catch (Exception e)
        //        {
        //        }
        //    }

        //    s = s.Trim();

        //    if (s.Length <= 10)
        //    {
        //        return HTMLUtils.toHTMLText(s);
        //    }

        //    string t = s.Substring(0, 10) + "...";
        //    string c = "<span title=\"" + HTMLUtils.toHTML(s, 24) + "\">" + HTMLUtils.toHTMLText(t) + "</span>";
        //    return c;
        //}
        public void add(DBTerm term)
        {
            terms.Add(term);
        }
        public void addFilter(DBTerm term)
        {
            filterTerms.Add(term);
        }
        public void add(SearchBinder binder)
        {
            binders.Add(binder);
        }
        //public void add(HtmlInputText startDate, HtmlInputText endDate, DropDownList fieldSelect)
        //{
        //    DateRangeSearchBinder b = new DateRangeSearchBinder(db, startDate, endDate, fieldSelect);
        //    binders.Add(b);
        //}
        //public void add(HtmlInputText startDate, HtmlInputText endDate, DropDownList fieldSelect, string prefix)
        //{
        //    DateRangeSearchBinder b = new DateRangeSearchBinder(db, startDate, endDate, fieldSelect, prefix);
        //    binders.Add(b);
        //}
        //public FieldDateRangeSearchBinder add(HtmlInputText startDate, HtmlInputText endDate, string column)
        //{
        //    FieldDateRangeSearchBinder b = new FieldDateRangeSearchBinder(startDate, endDate, column);
        //    binders.Add(b);
        //    return b;
        //}

        //public void addTextSearch(TextBox c, string text)
        //{
        //    TextSearchBinder b = new TextSearchBinder(db, c, text);
        //    binders.Add(b);
        //}
        //public void addFilter(DropDownList c, string name, string prefix)
        //{
        //    FilterSearchBinder b = new FilterSearchBinder(db, c, name, prefix);
        //    binders.Add(b);
        //}
        //public void addFilter(DropDownList c, string name)
        //{
        //    FilterSearchBinder b = new FilterSearchBinder(db, c, name);
        //    binders.Add(b);
        //}
        //public void addFilter2(DropDownList c, string name, string prefix, string title)
        //{
        //    FilterSearchBinder b = new FilterSearchBinder(db, c, name, prefix).setTitle(title);
        //    binders.Add(b);
        //}
        //public void addFilter2(DropDownList c, string name, string title)
        //{
        //    FilterSearchBinder b = new FilterSearchBinder(db, c, name).setTitle(title);
        //    binders.Add(b);
        //}
        //public void addFilter(DropDownList c)
        //{
        //    if (c == null)
        //        throw new Exception("Unexpected Exception");

        //    FilterSearchBinder b = new FilterSearchBinder(db, c, c.ID.Substring(1));
        //    binders.Add(b);
        //}
        //public void addFilter2(DropDownList c, string title)
        //{
        //    if (c == null)
        //        throw new Exception("Unexpected Exception");

        //    FilterSearchBinder b = new FilterSearchBinder(db, c, c.ID.Substring(1)).setTitle(title);
        //    binders.Add(b);
        //}
        public DBFilter createFilter()
        {
            DBFilter f = new DBFilter();
            createFilter(f);
            return f;
        }
        public bool createFilter(DBFilter f)
        {
            bool result = false;
            foreach (DBTerm t in terms)
            {
                f.add(t);
            }

            foreach (SearchBinder b in binders)
            {
                if (b.buildFilter(f))
                    result = true;
            }
            return result;

        }
        public void clear()
        {
            foreach (SearchBinder b in binders)
            {
                b.clear();

            }
        }



        //public ArrayList getDistintValues(DBField f, DBFilter filter)
        //{
        //    return db.getDistintValues(f, filter);

        //}
        public void buildApplicationFilterFilter(DBFilter filter)
        {
            foreach (DBTerm t in terms)
            {
                filter.add(t);
            }
            foreach (DBTerm t in filterTerms)
            {
                filter.add(t);
            }
        }
        public void buildFilterFilter(DBFilter filter)
        {
            foreach (DBTerm t in terms)
            {
                filter.add(t);
            }
            foreach (DBTerm t in filterTerms)
            {
                filter.add(t);
            }
            foreach (SearchBinder binder in binders)
            {
                binder.buildFilterFilter(filter);
            }
        }

    }



}
