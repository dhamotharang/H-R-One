using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Web.UI;

namespace HROne.DataAccess
{
    public class Error
    {
        public string name;
        public string error;
        public string fielderror;
        public object[] parameters;
        public Error(string name, string error)
        {
            this.name = name;
            this.error = error;
            this.parameters = new object[] { name };
        }
        public Error(string name, string error, object p)
        {
            this.name = name;
            this.error = error;
            this.parameters = new object[] { name, p };
        }
        public Error(string name, string error, object p, object p2)
        {
            this.name = name;
            this.error = error;
            this.parameters = new object[] { name, p, p2 };
        }
        public Error setFieldError(string fielderror)
        {
            this.fielderror = fielderror;
            return this;
        }
        public string getPrompt(DBManager db, ResourceManager rm)
        {
            return getPrompt(db, rm, null);
        }
        public string getPrompt(DBManager db, ResourceManager rm, CultureInfo ci)
        {
            return getPrompt(db, rm, error + ".prompt", error, ci);
        }
        public string getFieldPrompt(DBManager db, ResourceManager rm)
        {
            return getFieldPrompt(db, rm, null);
        }
        public string getFieldPrompt(DBManager db, ResourceManager rm, CultureInfo ci)
        {
            string key = fielderror;
            if (key == null)
                key = error;
            return getPrompt(db, rm, key, key);
        }
        public string getPrompt(DBManager db, ResourceManager rm, string key, string name)
        {
            return getPrompt(db, rm, key, name, null);
        }

        public string getPrompt(DBManager db, ResourceManager rm, string key, string name, CultureInfo ci)
        {
            string msg = rm.GetString(key, ci);
            if (msg == null)
            {
                msg = rm.GetString(name, ci);
                if (msg == null)
                    msg = name;
            }

            string label = "";
            if (db != null && name != null)
                label = db.getLabel(this.name, ci);
            object[] p = new object[parameters.Length];
            p[0] = label;
            if (p.Length > 1)
                Array.Copy(parameters, 1, p, 1, p.Length - 1);
            return String.Format(msg, p);
        }


    }

    public class PageErrors
    {
        public DBManager db;
        public Dictionary<string, Error> errors = new Dictionary<string, Error>();
        //		public Hashtable fieldErrors=new Hashtable();
        public List<Error> globalErrors = new List<Error>();
        public PageErrors()
        {
        }
        public PageErrors(DBManager db)
        {
            this.db = db;
        }
        public void addError(string name, String error)
        {
            if (!errors.ContainsKey(name))
            {
                errors.Add(name, new Error(name, error));
            }
        }
        public void addError(string name, String error, object param)
        {
            if (!errors.ContainsKey(name))
            {
                errors.Add(name, new Error(name, error, param));
            }
        }
        public void addError(string name, String error, object param, object param2)
        {
            if (!errors.ContainsKey(name))
            {
                errors.Add(name, new Error(name, error, param, param2));
            }
        }
        public void addFieldError(string name, String error, String fieldError)
        {
            if (!errors.ContainsKey(name))
                errors.Add(name, new Error(name, error).setFieldError(fieldError));
            //			if(!fieldErrors.ContainsKey(name))
            //				fieldErrors.Add(name,fieldError);
        }
        public void addError(string error)
        {
            globalErrors.Add(new Error(null, error));
        }
        public void addGlobalError(string error)
        {
            globalErrors.Add(new Error(null, error));
        }
        public void addGlobalError(string error, object param)
        {
            globalErrors.Add(new Error(null, error, param));
        }
        public bool hasError(string name)
        {
            return errors.ContainsKey(name);
        }
        public Error getError(string name)
        {
            return (Error)errors[name];
        }
        //		public string getFieldError(string name) 
        //		{
        //			string err=(string)fieldErrors[name];
        //			if(err!=null)
        //				return err;
        //			return (string)errors[name];
        //		}
        public bool isEmpty()
        {
            return errors.Count == 0 && globalErrors.Count == 0;
        }
        public void clear()
        {
            errors.Clear();
            globalErrors.Clear();
        }

        public string getPrompt()
        {
            return getPrompt(null);
        }
        public string getPrompt(CultureInfo ci)
        {
            ResourceManager rm = DBUtils.getResourceManager();


            StringBuilder b = new StringBuilder();
            b.Append(rm.GetString("validate.prompt", ci));

            foreach (Error s in globalErrors)
            {

                string err = s.getPrompt(db, rm, ci);
                b.Append("\r\n" + err);
            }
            foreach (string key in errors.Keys)
            {
                Error er = (Error)errors[key];
                b.Append("\r\n - " + er.getPrompt(db, rm, ci));
            }
            return b.ToString();
        }

        public static PageErrors getErrors(DBManager db, System.Web.UI.Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                PageErrors errors;
                if (c.GetType() == typeof(PageError))
                {
                    PageError pe = (PageError)c;
                    if (pe.Name == null)
                    {
                        errors = pe.getErrors();
                        if (db != null)
                            errors.db = db;
                        return errors;
                    }
                }

                errors = getErrors(db, c);
                if (errors != null)
                    return errors;
            }
            return null;
        }
    }
}
