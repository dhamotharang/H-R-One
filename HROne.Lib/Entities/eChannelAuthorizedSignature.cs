using System;
using System.Collections;
using System.Text;
using HROne.DataAccess;
using HROne.Common;
////using perspectivemind.validation;

namespace HROne.Lib.Entities
{
    [DBClass("eChannelAuthorizedSignature")]
    public class EeChannelAuthorizedSignature : BaseObject
    {
        public static DBManager db = new DBManager(typeof(EeChannelAuthorizedSignature));

        protected int m_eChannelAuthorizedSignatureID;
        [DBField("eChannelAuthorizedSignatureID", true, true), TextSearch, Export(false)]
        public int eChannelAuthorizedSignatureID
        {
            get { return m_eChannelAuthorizedSignatureID; }
            set { m_eChannelAuthorizedSignatureID = value; modify("eChannelAuthorizedSignatureID"); }
        }
        protected int m_UserID;
        [DBField("UserID"), TextSearch, Export(false), Required]
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; modify("UserID"); }
        }

    }
}
