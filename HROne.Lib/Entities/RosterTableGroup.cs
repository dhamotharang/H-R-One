using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
using HROne.CommonLib;

namespace HROne.Lib.Entities
{
    [DBClass("RosterTableGroup")]
    public class ERosterTableGroup : BaseObject
    {

        public static DBManager db = new DBManager(typeof(ERosterTableGroup));
        public static WFValueList VLRosterTableGroup = new WFDBCodeList(ERosterTableGroup.db, "RosterTableGroupID", "RosterTableGroupCode", "RosterTableGroupDesc", "RosterTableGroupCode");

        protected int m_RosterTableGroupID;
        [DBField("RosterTableGroupID", true, true), TextSearch, Export(false)]
        public int RosterTableGroupID
        {
            get { return m_RosterTableGroupID; }
            set { m_RosterTableGroupID = value; modify("RosterTableGroupID"); }
        }
        protected string m_RosterTableGroupCode;
        [DBField("RosterTableGroupCode"), TextSearch, MaxLength(20, 10), Export(false), Required]
        public string RosterTableGroupCode
        {
            get { return m_RosterTableGroupCode; }
            set { m_RosterTableGroupCode = value; modify("RosterTableGroupCode"); }
        }
        protected string m_RosterTableGroupDesc;
        [DBField("RosterTableGroupDesc"), TextSearch, MaxLength(100, 25), Export(false), Required]
        public string RosterTableGroupDesc
        {
            get { return m_RosterTableGroupDesc; }
            set { m_RosterTableGroupDesc = value; modify("RosterTableGroupDesc"); }
        }
        protected int m_RosterClientID;
        [DBField("RosterClientID"), TextSearch, Export(false)]
        public int RosterClientID
        {
            get { return m_RosterClientID; }
            set { m_RosterClientID = value; modify("RosterClientID"); }
        }
        protected int m_RosterClientSiteID;
        [DBField("RosterClientSiteID"), TextSearch, Export(false)]
        public int RosterClientSiteID
        {
            get { return m_RosterClientSiteID; }
            set { m_RosterClientSiteID = value; modify("RosterClientSiteID"); }
        }

        protected string m_RosterTableGroupExtendData;
        [DBField("RosterTableGroupExtendData"), TextSearch, Export(false)]
        public string RosterTableGroupExtendData
        {
            get { return m_RosterTableGroupExtendData; }
            set { m_RosterTableGroupExtendData = value; modify("RosterTableGroupExtendData"); }
        }

        public string GetRosterTableGroupExtendData(string FieldName)
        {
            System.Xml.XmlNodeList node = Utility.GetXmlDocumentByDataString(RosterTableGroupExtendData).GetElementsByTagName(FieldName);
            if (node.Count > 0)
                return node[0].InnerText;
            else
                return string.Empty;

        }

        public void SetRosterTableGroupExtendData(string FieldName, string Value)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (string.IsNullOrEmpty(RosterTableGroupExtendData))
            {
                System.Xml.XmlElement rootNode = xmlDoc.CreateElement("RosterTableGroupExtendData");
                xmlDoc.AppendChild(rootNode);

            }
            else
                xmlDoc.LoadXml(RosterTableGroupExtendData);

            if (!Value.Equals(string.Empty))
            {

                System.Xml.XmlElement xmlElement = xmlDoc.CreateElement(FieldName);
                xmlElement.InnerText = Value.Trim();
                xmlDoc.DocumentElement.AppendChild(xmlElement);
                RosterTableGroupExtendData = xmlDoc.InnerXml;
            }
        }

    }
}
