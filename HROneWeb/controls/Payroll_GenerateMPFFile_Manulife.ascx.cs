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

public partial class Payroll_GenerateMPFFile_Manulife : HROneWebControl, MPFFileControlInterface
{
    private const string MPF_PLAN_XML_SEQUENCE_NODE_NAME = ManulifeMPFFile.MPF_PLAN_XML_SEQUENCE_NODE_NAME;

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
    //public event EventHandler ParameterChange;
    //private const string NEXT_SEQ_NUM = "BOCIMPF_NEXT_SEQ_NUM";
    protected void Page_Load(object sender, EventArgs e)
    {
        //PaymentMethod.SelectedIndexChanged += ParameterChange;

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
        ManulifeMPFFile mpfFile = new ManulifeMPFFile(dbConn);
        int iSequenceNum = 1;
        if (!int.TryParse(SequenceNum.Text, out iSequenceNum))
        {
            SequenceNum.Text = "1";
            mpfFile.SequenceNo = 1;
        }
        else
            mpfFile.SequenceNo = iSequenceNum;

        return mpfFile;
    }

}
