using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HROne.CommonLib;

namespace ProductKeyGenerator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            this.cbxProduct.Items.Clear();
            foreach(HRPlus.ProductKey.ProductLicenseType value in Enum.GetValues(typeof(HRPlus.ProductKey.ProductLicenseType)))
                this.cbxProduct.Items.Add(value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ushort iSerialNo;
            if (!ushort.TryParse(txtSerialNo.Text, out iSerialNo))
            {
                MessageBox.Show(this, "Serial No must be numeric!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(cbxProduct.Text))
            {
                MessageBox.Show(this, "Please select the Product Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            HRPlus.ProductKey key= new HRPlus.ProductKey();
            key.SerialNo=iSerialNo;
            key.ProductType = (HRPlus.ProductKey.ProductLicenseType)Enum.Parse(typeof(HRPlus.ProductKey.ProductLicenseType), cbxProduct.Text);
            key.NumOfCompanies=Convert.ToUInt16(numCompany.Value);
            key.NumOfUsers = Convert.ToUInt16(numUser.Value);
            key.IsAttendance=chkModuleAttendance.Checked;
            key.IsESS=chkModuleESS.Checked;
            key.IsLeaveManagement=chkModuleLeave.Checked;
            key.IsPayroll=chkModulePayroll.Checked;
            key.IsTaxation=chkModuleTaxation.Checked;
            key.IsCostCenter = chkModuleCostCenter.Checked;
            key.IsTraining = chkModuleTraining.Checked;

            txtProductKey.Text=key.GetProductKey();
            return;


        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(cbxProduct.Text))
            {
                MessageBox.Show(this, "Please select the Product Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            HRPlus.ProductKey key = new HRPlus.ProductKey();
            //key.ProductType = (HRPlus.ProductKey.ProductLicenseType)Enum.Parse(typeof(HRPlus.ProductKey.ProductLicenseType), cbxProduct.Text);
            key.LoadProductKey(txtProductKey.Text);
            cbxProduct.Text = key.ProductType.ToString();
            txtSerialNo.Text = key.SerialNo.ToString();
            numCompany.Value = key.NumOfCompanies;
            numUser.Value = key.NumOfUsers;
            chkModuleAttendance.Checked = key.IsAttendance;
            chkModuleESS.Checked = key.IsESS;
            chkModuleLeave.Checked = key.IsLeaveManagement;
            chkModulePayroll.Checked = key.IsPayroll;
            chkModuleTaxation.Checked = key.IsTaxation;
            chkModuleCostCenter.Checked = key.IsCostCenter;
            chkModuleTraining.Checked = key.IsTraining;
            return;
        }

        private void btnTrialKey_Click(object sender, EventArgs e)
        {
            txtTrialKey.Text = GetTrialKey(dtpLastTrialDate.Value);
        }
        public string GetTrialKey(DateTime lastTrialDate)
        {
            //if (string.IsNullOrEmpty(ESystemParameter.getParameter(ESystemParameter.PARAM_CODE_TRIALKEY)))
            //{
            Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);
            string trialKey = string.Empty;
            trialKey = crypto.Encrypting(lastTrialDate.ToString("yyyyMMdd"), (string)cbxProduct.Text);

            return HRPlus.base32.ConvertBase64ToBase32(trialKey);
            //}
        }

        private void btnGenerateAuthorizationCode_Click(object sender, EventArgs e)
        {
            Crypto crypto = new Crypto(Crypto.SymmProvEnum.DES);
            txtAuthorizationCode.Text = HRPlus.base32.ConvertBase64ToBase32(crypto.Encrypting((string)cbxProduct.Text, txtRequestCode.Text));

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}