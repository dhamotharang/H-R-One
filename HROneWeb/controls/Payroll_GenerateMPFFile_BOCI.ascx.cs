using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.MPFFile;
using HROne.Lib.Entities;

public partial class Payroll_GenerateMPFFile_BOCI : HROneWebControl, MPFFileControlInterface
{
    private const string MPF_PLAN_XML_SEQUENCE_NODE_NAME = "MPFPlanBOCISequenceNo";

    //public int MPFPlanID
    //{
    //    set
    //    {
    //        if (!CurMPFPlanID.Value.Equals(value.ToString()))
    //        {
    //            CurMPFPlanID.Value = value.ToString();
    //            EMPFPlan mpfPlan = new EMPFPlan();
    //            mpfPlan.MPFPlanID = value;
    //            if (EMPFPlan.db.select(dbConn, mpfPlan))
    //            {
    //                System.Xml.XmlNodeList nodeList = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfPlan.MPFPlanExtendData).GetElementsByTagName(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
    //                if (nodeList.Count > 0)
    //                    SequenceNum.Text = nodeList[0].InnerText;
    //                else
    //                    SequenceNum.Text = "1";

    //            }
    //        }
    //    }
    //}
    public event EventHandler ParameterChange;
    //private const string NEXT_SEQ_NUM = "BOCIMPF_NEXT_SEQ_NUM";
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentMethod.SelectedIndexChanged += ParameterChange;

        //if (!Page.IsPostBack)
        //{
        //    SequenceNum.Text = HROne.Lib.Entities.ESystemParameter.getParameter(NEXT_SEQ_NUM);
        //    if (string.IsNullOrEmpty(SequenceNum.Text))
        //    {
        //        SequenceNum.Text = "1";
        //    }
        //}
    }

    public GenericMPFFile CreateMPFFileObject()
    {
        BOCIPMPFFile mpfFile = new BOCIPMPFFile(dbConn);
        if (this.PaymentMethod.SelectedValue.Equals("A"))
            mpfFile.PaymentMethod = BOCIPMPFFile.PaymentMethodEnum.AUTOPAY;
        else if (this.PaymentMethod.SelectedValue.Equals("C"))
            mpfFile.PaymentMethod = BOCIPMPFFile.PaymentMethodEnum.CASH;
        else if (this.PaymentMethod.SelectedValue.Equals("Q"))
        {
            mpfFile.PaymentMethod = BOCIPMPFFile.PaymentMethodEnum.CHEQUE;
            mpfFile.ChequeNum = ChequeNum.Text;
        }
        //int iSequenceNum = 1;
        //if (!int.TryParse(SequenceNum.Text, out iSequenceNum))
        //{
        //    SequenceNum.Text = "1";
        //    mpfFile.SequenceNo = 1;
        //}
        //else
        //    mpfFile.SequenceNo = iSequenceNum;

        //Save();
        return mpfFile;
    }
    protected void PaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PaymentMethod.SelectedValue.Equals("Q"))
            ChequePanel.Visible = true;
        else
            ChequePanel.Visible = false;

    }

    //private void Save()
    //{
    //    int intNextSequenceNum = 1;
    //    if (int.TryParse(SequenceNum.Text, out intNextSequenceNum))
    //        intNextSequenceNum++;
    //    else
    //        intNextSequenceNum = 1;

    //    int MPFPlanID = 0;
    //    if (int.TryParse(CurMPFPlanID.Value, out MPFPlanID))
    //    {
    //        EMPFPlan mpfPlan = new EMPFPlan();
    //        mpfPlan.MPFPlanID = MPFPlanID;
    //        if (EMPFPlan.db.select(dbConn, mpfPlan))
    //        {
    //            System.Xml.XmlDocument xmlDoc = HROne.CommonLib.Utility.GetXmlDocumentByDataString(mpfPlan.MPFPlanExtendData);
    //            System.Xml.XmlNodeList nodeList = xmlDoc.GetElementsByTagName(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
    //            System.Xml.XmlNode node;
    //            if (nodeList.Count > 0)
    //                node = nodeList[0];
    //            else
    //            {
    //                node = xmlDoc.CreateElement(MPF_PLAN_XML_SEQUENCE_NODE_NAME);
    //                xmlDoc.DocumentElement.AppendChild(node);
    //            }

    //            node.InnerText = intNextSequenceNum.ToString();
    //            mpfPlan.MPFPlanExtendData = xmlDoc.InnerXml;
    //            EMPFPlan.db.update(dbConn, mpfPlan);
    //        }
    //    }

    //    //int iSequenceNum = 1;



    //    //if (int.TryParse(SequenceNum.Text, out iSequenceNum))
    //    //{
    //    //    HROne.Lib.Entities.ESystemParameter.setParameter(NEXT_SEQ_NUM, ((int)++iSequenceNum).ToString());
    //    //}
    //    //else
    //    //    HROne.Lib.Entities.ESystemParameter.setParameter(NEXT_SEQ_NUM, "1");
    //}
}
