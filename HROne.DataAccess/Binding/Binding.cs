using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace HROne.DataAccess
{
    public class Binding
    {

        Dictionary<string, Binder> binderMap = new Dictionary<string, Binder>();
        List<Binder> binders = new List<Binder>();
        DBManager db;
        public DatabaseConnection DBConn;

        public Binding(DatabaseConnection DBConn, DBManager db)
        {
            this.DBConn = DBConn;
            this.db = db;
        }
        public void toControl(Hashtable values)
        {
            foreach (Binder b in binders)
            {
                b.toControl(values);
            }
        }
        public void toValues(Hashtable values)
        {
            foreach (Binder b in binders)
            {
                b.toValues(values);
            }
        }
        public void init(HttpRequest Request, HttpSessionState Session)
        {
            foreach (Binder b in binders)
            {
                b.init(Request, Session);
            }
        }
        public void add(Binder binder)
        {
            binder.Binding = this;
            binders.Add(binder);
        }
        //public void add(DropDownList c)
        //{
        //    Binder b = new DropDownBinder(db, c, c.ID);
        //    binders.Add(b);
        //    binderMap.Add(c.ID, b);
        //}
        ////		public void add(ComboBox c) 
        //		{
        //			Binder b=new ComboBoxBinder(db,c,c.ID);
        //			binders.Add(b);
        //			binderMap.Add(c.ID,b);
        //		}
        public void add(RadioButtonList c)
        {
            Binder b = new RadioButtonListBinder(db, c, c.ID);
            binders.Add(b);
            binderMap.Add(c.ID, b);
        }

        public void add(TextBox c)
        {
            Binder b = new TextBoxBinder(db, c, c.ID);
            binders.Add(b);
            binderMap.Add(c.ID, b);
        }
        public void add(Label c)
        {
            Binder b = new LabelBinder(db, c, c.ID);
            binders.Add(b);
            binderMap.Add(c.ID, b);
        }
        public void add(HtmlInputHidden c)
        {
            Binder b = new HiddenBinder(db, c, c.ID);
            binders.Add(b);
            binderMap.Add(c.ID, b);
        }


    }

}
