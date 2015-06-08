using System;
using System.Collections.Generic;
using System.Text;
using HROne.DataAccess;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("MPFSchemeCessationReason")]
    public class EMPFSchemeCessationReason : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EMPFSchemeCessationReason));

        protected int m_MPFSchemeCessationReasonID;
        [DBField("MPFSchemeCessationReasonID", true, true), TextSearch, Export(false)]
        public int MPFSchemeCessationReasonID
        {
            get { return m_MPFSchemeCessationReasonID; }
            set { m_MPFSchemeCessationReasonID = value; modify("MPFSchemeCessationReasonID"); }
        }
        protected int m_MPFSchemeID;
        [DBField("MPFSchemeID"), TextSearch, Export(false), Required]
        public int MPFSchemeID
        {
            get { return m_MPFSchemeID; }
            set { m_MPFSchemeID = value; modify("MPFSchemeID"); }
        }
        protected string m_MPFSchemeCessationReasonCode;
        [DBField("MPFSchemeCessationReasonCode"), MaxLength(10), TextSearch, Export(false), Required]
        public string MPFSchemeCessationReasonCode
        {
            get { return m_MPFSchemeCessationReasonCode; }
            set { m_MPFSchemeCessationReasonCode = value; modify("MPFSchemeCessationReasonCode"); }
        }
        protected string m_MPFSchemeCessationReasonDesc;
        [DBField("MPFSchemeCessationReasonDesc"), MaxLength(100), TextSearch, Export(false), Required]
        public string MPFSchemeCessationReasonDesc
        {
            get { return m_MPFSchemeCessationReasonDesc; }
            set { m_MPFSchemeCessationReasonDesc = value; modify("MPFSchemeCessationReasonDesc"); }
        }


    }
}
