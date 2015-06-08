using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.UI.WebControls;
using HROne.DataAccess;
//using perspectivemind.validation;


public class HKIDLabel : LabelBinder
{
    public HKIDLabel(DBManager db, Label c, string name)
        : base(db, c, name)
    {
        
    }

    public override void toControl(Hashtable values)
    {
        base.toControl(values);
        if(c.Text.Equals("()"))
            c.Text="";
    }

}
public class HKIDBinder : TextBoxBinder
{
    TextBox HKIDMainTextBox;
    TextBox Digit;


    public HKIDBinder(DBManager db, TextBox c, TextBox Digit)
        : this(db, c, Digit, false)
    {
    }

    public HKIDBinder(DBManager db, TextBox c, TextBox Digit, bool AutoFillCheckDigit)
        : base(db, c)
    {
        this.HKIDMainTextBox = c;
        this.Digit = Digit;
        if (AutoFillCheckDigit)
            Digit.Attributes.Add("onfocus", "if (" + Digit.ClientID + ".value=='') " + Digit.ClientID + ".value=getHKIDCheckDigit(" + c.ClientID + ".value);");
    }

    public override void init(System.Web.HttpRequest Request, System.Web.SessionState.HttpSessionState Session)
    {
        base.init(Request, Session);
        HKIDMainTextBox.Columns = 8;
        //  "Gap Sourcing" may use up to 12 digit for HKID input
        //HKIDMainTextBox.MaxLength = 8;
        Digit.Columns = 1;
        Digit.MaxLength = 1;
    }
    public override void toControl(Hashtable values)
    {
        object o = values[name];
        string s;
        if (o != null)
            s = o.ToString();
        else
            s = "";

        int index = s.LastIndexOf('(');
        if (index < 0)
        {
            c.Text = s;
            Digit.Text = "";
        }
        else
        {
            c.Text = s.Substring(0, index);
            if (s.Length > index + 2)
                Digit.Text = s.Substring(index + 1, 1);
        }
    }
    public override void toValues(Hashtable values)
    {
        if (!c.Text.Equals(string.Empty) || !Digit.Text.Equals(string.Empty))
            values.Add(name, c.Text + "(" + Digit.Text + ")");
        else
            values.Add(name, string.Empty);
    }

}

public class BlankZeroLabelVLBinder : LabelVLBinder
{
    Label c;
    string name;
    string textDisplayforZeroValue = "-";

    public BlankZeroLabelVLBinder(DBManager db, Label c, WFValueList vl)
        : base(db, c, vl)
    {
        this.c = c;
        this.name = c.ID;
    }

    public BlankZeroLabelVLBinder(DBManager db, Label c, string name, WFValueList vl)
        : base(db, c, name, vl)
    {
        this.c = c;
        this.name = name;
    }

    public BlankZeroLabelVLBinder(DBManager db, Label c, WFValueList vl, DBFilter filter)
        : base(db, c, vl, filter)
    {
        this.c = c;
        this.name = c.ID;
    }

    public BlankZeroLabelVLBinder setTextDisplayForZero(string value)
    {
        textDisplayforZeroValue = value;
        return this;
    }

    public override void toControl(Hashtable values)
    {
        base.toControl(values);
        object o = values[name];
        if (o != null)
        {
            if (o.ToString().Equals("0") && c.Text.Equals("0"))
                c.Text = textDisplayforZeroValue;
        }
    }
}

public class TextBoxXMLNodeBinder : TextBoxBinder
{
    TextBox textbox;
    string fieldName;
    string xmlNodeName;

    public TextBoxXMLNodeBinder(DBManager db, TextBox textbox, string fieldName, string xmlNodeName)
        : base(db, textbox, fieldName)
    {
        this.textbox = textbox;
        this.fieldName = fieldName;
        this.xmlNodeName = xmlNodeName;
    }

    public override void toControl(Hashtable values)
    {
        //base.toControl(values);
        object o = values[fieldName];
        if (o != null)
        {
            string xmlString = o.ToString();
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (!string.IsNullOrEmpty(xmlString))
            {
                xmlDoc.LoadXml(xmlString);
                System.Xml.XmlNodeList xmlNodeList = xmlDoc.DocumentElement.GetElementsByTagName(xmlNodeName);
                if (xmlNodeList.Count > 0)
                {
                    textbox.Text = xmlNodeList[0].InnerXml;
                    return;
                }
            }
        }
        textbox.Text = string.Empty;
    }
    
    public override void toValues(Hashtable values)
    {
        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        if (!values.ContainsKey(fieldName))
        {
            System.Xml.XmlElement rootNode = xmlDoc.CreateElement(fieldName);
            xmlDoc.AppendChild(rootNode);
            values.Add(fieldName, xmlDoc.InnerXml);

        }
        else
            xmlDoc.LoadXml(values[fieldName].ToString());

        if (!textbox.Text.Equals(string.Empty))
        {

            System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(xmlNodeName);
            xmlElement.InnerText = textbox.Text.Trim();
            xmlDoc.DocumentElement.AppendChild(xmlElement);
            values[fieldName] = xmlDoc.InnerXml;
        }

    }
}

public class CheckBoxXMLNodeBinder : CheckBoxBinder
{
    CheckBox checkbox;
    string fieldName;
    string xmlNodeName;

    public CheckBoxXMLNodeBinder(DBManager db, CheckBox checkbox, string fieldName, string xmlNodeName)
        : base(db, checkbox, fieldName)
    {
        this.checkbox = checkbox;
        this.fieldName = fieldName;
        this.xmlNodeName = xmlNodeName;
    }

    public override void toControl(Hashtable values)
    {
        //base.toControl(values);
        object o = values[fieldName];
        if (o != null)
        {
            string xmlString = o.ToString();
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (!string.IsNullOrEmpty(xmlString))
            {
                xmlDoc.LoadXml(xmlString);
                System.Xml.XmlNodeList xmlNodeList = xmlDoc.DocumentElement.GetElementsByTagName(xmlNodeName);
                if (xmlNodeList.Count > 0)
                {
                    if (xmlNodeList[0].InnerXml.Equals("YES", StringComparison.CurrentCultureIgnoreCase))
                        checkbox.Checked = true;
                    return;
                }
            }
        }
        checkbox.Checked = false;
    }

    public override void toValues(Hashtable values)
    {
        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        if (!values.ContainsKey(fieldName))
        {
            System.Xml.XmlElement rootNode = xmlDoc.CreateElement(fieldName);
            xmlDoc.AppendChild(rootNode);
            values.Add(fieldName, xmlDoc.InnerXml);

        }
        else
            xmlDoc.LoadXml(values[fieldName].ToString());

        if (checkbox.Checked)
        {

            System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(xmlNodeName);
            xmlElement.InnerText = "YES";
            xmlDoc.DocumentElement.AppendChild(xmlElement);
            values[fieldName] = xmlDoc.InnerXml;
        }

    }
}

public class LabelXMLNodeBinder : LabelBinder
{
    Label label;
    string fieldName;
    string xmlNodeName;

    public LabelXMLNodeBinder(DBManager db, Label label, string fieldName, string xmlNodeName)
        : base(db, label, fieldName)
    {
        this.label = label;
        this.fieldName = fieldName;
        this.xmlNodeName = xmlNodeName;
    }

    public override void toControl(Hashtable values)
    {
        //base.toControl(values);
        object o = values[fieldName];
        if (o != null)
        {
            string xmlString = o.ToString();
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (!string.IsNullOrEmpty(xmlString))
            {
                xmlDoc.LoadXml(xmlString);
                System.Xml.XmlNodeList xmlNodeList = xmlDoc.DocumentElement.GetElementsByTagName(xmlNodeName);
                if (xmlNodeList.Count > 0)
                {
                    label.Text = HTMLUtils.toHTMLText(xmlNodeList[0].InnerXml);
                }
            }
        }
    }
    public override void toValues(Hashtable values)
    {
        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        if (!values.ContainsKey(fieldName))
        {
            System.Xml.XmlElement rootNode = xmlDoc.CreateElement(fieldName);
            xmlDoc.AppendChild(rootNode);
            values.Add(fieldName, xmlDoc.InnerXml);

        }
        else
            xmlDoc.LoadXml(values[fieldName].ToString());

        if (!label.Text.Equals(string.Empty))
        {

            System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(xmlNodeName);
            xmlElement.InnerText = label.Text.Trim();
            xmlDoc.DocumentElement.AppendChild(xmlElement);
            values[fieldName] = xmlDoc.InnerXml;
        }
    }

}
public class DropDownListXMLNodeVLBinder : DropDownVLBinder
{
    DropDownList dropDownList;
    string fieldName;
    string xmlNodeName;

    public DropDownListXMLNodeVLBinder(DBManager db, DropDownList dropDownList, WFValueList vl, DBFilter filter, string fieldName, string xmlNodeName)
        : base(db, dropDownList, vl, filter, fieldName)
    {
        this.dropDownList = dropDownList;
        this.fieldName = fieldName;
        this.xmlNodeName = xmlNodeName;
    }
    public override void toControl(Hashtable values)
    {
        if (dropDownList.Items.Count > 0)
        {
            //base.toControl(values);
            object o = values[fieldName];
            if (o != null)
            {
                string xmlString = o.ToString();
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                if (!string.IsNullOrEmpty(xmlString))
                {
                    xmlDoc.LoadXml(xmlString);
                    System.Xml.XmlNodeList xmlNodeList = xmlDoc.DocumentElement.GetElementsByTagName(xmlNodeName);
                    if (xmlNodeList.Count > 0)
                    {
                        ListItem selectedItem = dropDownList.Items.FindByValue(xmlNodeList[0].InnerXml);
                        if (selectedItem != null)
                        {
                            selectedItem.Selected = true;
                            dropDownList.SelectedValue = selectedItem.Value;
                            return;
                        }
                    }
                }
            }
            dropDownList.SelectedValue = dropDownList.Items[0].Value;
        }
    }
    public override void toValues(Hashtable values)
    {
        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        if (!values.ContainsKey(fieldName))
        {
            System.Xml.XmlElement rootNode = xmlDoc.CreateElement(fieldName);
            xmlDoc.AppendChild(rootNode);
            values.Add(fieldName, xmlDoc.InnerXml);

        }
        else
            xmlDoc.LoadXml(values[fieldName].ToString());

        if (!dropDownList.SelectedValue.Equals(string.Empty))
        {

            System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(xmlNodeName);
            xmlElement.InnerText = dropDownList.SelectedValue.Trim();
            xmlDoc.DocumentElement.AppendChild(xmlElement);
            values[fieldName] = xmlDoc.InnerXml;
        }

    }
}

public class LabelXMLNodeVLBinder : LabelVLBinder
{
    Label label;
    WFValueList valueList;
    DBFilter filter;
    string fieldName;
    string xmlNodeName;
    bool showInvalid = true;

    public LabelXMLNodeVLBinder(DBManager db, Label label, WFValueList valueList, DBFilter filter, string fieldName, string xmlNodeName)
        : base(db, label, fieldName, valueList, filter)
    {
        this.label = label;
        this.valueList = valueList;
        this.filter = filter;
        this.fieldName = fieldName;
        this.xmlNodeName = xmlNodeName;
    }

    public override void toControl(Hashtable values)
    {
        object o = values[fieldName];
        if (o != null)
        {
            string xmlString = o.ToString();
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (!string.IsNullOrEmpty(xmlString))
            {
                xmlDoc.LoadXml(xmlString);
                System.Xml.XmlNodeList xmlNodeList = xmlDoc.DocumentElement.GetElementsByTagName(xmlNodeName);
                if (xmlNodeList.Count > 0)
                {
                    o = xmlNodeList[0].InnerXml;

                    bool found = false;
                    System.Collections.Generic.List<WFSelectValue> list = valueList.getValues(Binding.DBConn, filter, null);
                    foreach (WFSelectValue sv in list)
                    {
                        if (sv.key.Equals(o))
                        {
                            o = sv.name;
                            found = true;
                            break;
                        }
                    }
                    if (o.ToString().Equals("0"))
                        label.Text = string.Empty;
                    else if (found || showInvalid)
                        label.Text = HTMLUtils.toHTMLText(o.ToString());
                    else
                        label.Text = string.Empty;
                    return;
                }
            }
        }
        label.Text = string.Empty;
    }

    public override void toValues(Hashtable values)
    {
        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        if (!values.ContainsKey(fieldName))
        {
            System.Xml.XmlElement rootNode = xmlDoc.CreateElement(fieldName);
            xmlDoc.AppendChild(rootNode);
            values.Add(fieldName, xmlDoc.InnerXml);

        }
        else
            xmlDoc.LoadXml(values[fieldName].ToString());

        if (!label.Text.Equals(string.Empty))
        {

            System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(xmlNodeName);
            xmlElement.InnerText = label.Text.Trim();
            xmlDoc.DocumentElement.AppendChild(xmlElement);
            values[fieldName] = xmlDoc.InnerXml;
        }
    }
}
