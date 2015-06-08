namespace ProductKeyGenerator
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numCompany = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numUser = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkModuleTraining = new System.Windows.Forms.CheckBox();
            this.chkModuleCostCenter = new System.Windows.Forms.CheckBox();
            this.chkModuleAttendance = new System.Windows.Forms.CheckBox();
            this.chkModuleESS = new System.Windows.Forms.CheckBox();
            this.chkModuleTaxation = new System.Windows.Forms.CheckBox();
            this.chkModulePayroll = new System.Windows.Forms.CheckBox();
            this.chkModuleLeave = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtProductKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxProduct = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dtpLastTrialDate = new System.Windows.Forms.DateTimePicker();
            this.btnTrialKey = new System.Windows.Forms.Button();
            this.txtTrialKey = new System.Windows.Forms.TextBox();
            this.lblRequestCode = new System.Windows.Forms.Label();
            this.txtRequestCode = new System.Windows.Forms.TextBox();
            this.lblAuthorizationCode = new System.Windows.Forms.Label();
            this.txtAuthorizationCode = new System.Windows.Forms.TextBox();
            this.btnGenerateAuthorizationCode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUser)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numCompany
            // 
            this.numCompany.Location = new System.Drawing.Point(208, 72);
            this.numCompany.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numCompany.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCompany.Name = "numCompany";
            this.numCompany.Size = new System.Drawing.Size(56, 22);
            this.numCompany.TabIndex = 0;
            this.numCompany.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of Company (99 = unlimited)";
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Location = new System.Drawing.Point(208, 48);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(56, 22);
            this.txtSerialNo.TabIndex = 2;
            this.txtSerialNo.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Serial No";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Number of Users (99 = unlimited)";
            // 
            // numUser
            // 
            this.numUser.Location = new System.Drawing.Point(208, 96);
            this.numUser.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numUser.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUser.Name = "numUser";
            this.numUser.Size = new System.Drawing.Size(56, 22);
            this.numUser.TabIndex = 5;
            this.numUser.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkModuleTraining);
            this.groupBox1.Controls.Add(this.chkModuleCostCenter);
            this.groupBox1.Controls.Add(this.chkModuleAttendance);
            this.groupBox1.Controls.Add(this.chkModuleESS);
            this.groupBox1.Controls.Add(this.chkModuleTaxation);
            this.groupBox1.Controls.Add(this.chkModulePayroll);
            this.groupBox1.Controls.Add(this.chkModuleLeave);
            this.groupBox1.Location = new System.Drawing.Point(304, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 132);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Module";
            // 
            // chkModuleTraining
            // 
            this.chkModuleTraining.AutoSize = true;
            this.chkModuleTraining.Location = new System.Drawing.Point(16, 112);
            this.chkModuleTraining.Name = "chkModuleTraining";
            this.chkModuleTraining.Size = new System.Drawing.Size(64, 16);
            this.chkModuleTraining.TabIndex = 13;
            this.chkModuleTraining.Text = "Training";
            this.chkModuleTraining.UseVisualStyleBackColor = true;
            // 
            // chkModuleCostCenter
            // 
            this.chkModuleCostCenter.AutoSize = true;
            this.chkModuleCostCenter.Location = new System.Drawing.Point(16, 96);
            this.chkModuleCostCenter.Name = "chkModuleCostCenter";
            this.chkModuleCostCenter.Size = new System.Drawing.Size(79, 16);
            this.chkModuleCostCenter.TabIndex = 12;
            this.chkModuleCostCenter.Text = "Cost Center";
            this.chkModuleCostCenter.UseVisualStyleBackColor = true;
            // 
            // chkModuleAttendance
            // 
            this.chkModuleAttendance.AutoSize = true;
            this.chkModuleAttendance.Location = new System.Drawing.Point(16, 80);
            this.chkModuleAttendance.Name = "chkModuleAttendance";
            this.chkModuleAttendance.Size = new System.Drawing.Size(76, 16);
            this.chkModuleAttendance.TabIndex = 11;
            this.chkModuleAttendance.Text = "Attendance";
            this.chkModuleAttendance.UseVisualStyleBackColor = true;
            // 
            // chkModuleESS
            // 
            this.chkModuleESS.AutoSize = true;
            this.chkModuleESS.Location = new System.Drawing.Point(16, 64);
            this.chkModuleESS.Name = "chkModuleESS";
            this.chkModuleESS.Size = new System.Drawing.Size(129, 16);
            this.chkModuleESS.TabIndex = 10;
            this.chkModuleESS.Text = "Employee Self Service";
            this.chkModuleESS.UseVisualStyleBackColor = true;
            // 
            // chkModuleTaxation
            // 
            this.chkModuleTaxation.AutoSize = true;
            this.chkModuleTaxation.Checked = true;
            this.chkModuleTaxation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkModuleTaxation.Location = new System.Drawing.Point(16, 48);
            this.chkModuleTaxation.Name = "chkModuleTaxation";
            this.chkModuleTaxation.Size = new System.Drawing.Size(65, 16);
            this.chkModuleTaxation.TabIndex = 9;
            this.chkModuleTaxation.Text = "Taxation";
            this.chkModuleTaxation.UseVisualStyleBackColor = true;
            // 
            // chkModulePayroll
            // 
            this.chkModulePayroll.AutoSize = true;
            this.chkModulePayroll.Checked = true;
            this.chkModulePayroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkModulePayroll.Location = new System.Drawing.Point(16, 16);
            this.chkModulePayroll.Name = "chkModulePayroll";
            this.chkModulePayroll.Size = new System.Drawing.Size(57, 16);
            this.chkModulePayroll.TabIndex = 8;
            this.chkModulePayroll.Text = "Payroll";
            this.chkModulePayroll.UseVisualStyleBackColor = true;
            // 
            // chkModuleLeave
            // 
            this.chkModuleLeave.AutoSize = true;
            this.chkModuleLeave.Checked = true;
            this.chkModuleLeave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkModuleLeave.Location = new System.Drawing.Point(16, 32);
            this.chkModuleLeave.Name = "chkModuleLeave";
            this.chkModuleLeave.Size = new System.Drawing.Size(115, 16);
            this.chkModuleLeave.TabIndex = 7;
            this.chkModuleLeave.Text = "Leave Management";
            this.chkModuleLeave.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtProductKey
            // 
            this.txtProductKey.Location = new System.Drawing.Point(89, 146);
            this.txtProductKey.Name = "txtProductKey";
            this.txtProductKey.Size = new System.Drawing.Size(416, 22);
            this.txtProductKey.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Product";
            // 
            // cbxProduct
            // 
            this.cbxProduct.FormattingEnabled = true;
            this.cbxProduct.Items.AddRange(new object[] {
            "HROne"});
            this.cbxProduct.Location = new System.Drawing.Point(64, 12);
            this.cbxProduct.Name = "cbxProduct";
            this.cbxProduct.Size = new System.Drawing.Size(200, 20);
            this.cbxProduct.TabIndex = 14;
            this.cbxProduct.Text = "HROne";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(376, 174);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(131, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Load From Key";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dtpLastTrialDate
            // 
            this.dtpLastTrialDate.Location = new System.Drawing.Point(8, 200);
            this.dtpLastTrialDate.Name = "dtpLastTrialDate";
            this.dtpLastTrialDate.Size = new System.Drawing.Size(184, 22);
            this.dtpLastTrialDate.TabIndex = 16;
            // 
            // btnTrialKey
            // 
            this.btnTrialKey.Location = new System.Drawing.Point(192, 200);
            this.btnTrialKey.Name = "btnTrialKey";
            this.btnTrialKey.Size = new System.Drawing.Size(104, 23);
            this.btnTrialKey.TabIndex = 17;
            this.btnTrialKey.Text = "GenerateTrial Key";
            this.btnTrialKey.UseVisualStyleBackColor = true;
            this.btnTrialKey.Click += new System.EventHandler(this.btnTrialKey_Click);
            // 
            // txtTrialKey
            // 
            this.txtTrialKey.Location = new System.Drawing.Point(8, 224);
            this.txtTrialKey.Name = "txtTrialKey";
            this.txtTrialKey.Size = new System.Drawing.Size(496, 22);
            this.txtTrialKey.TabIndex = 18;
            // 
            // lblRequestCode
            // 
            this.lblRequestCode.AutoSize = true;
            this.lblRequestCode.Location = new System.Drawing.Point(8, 272);
            this.lblRequestCode.Name = "lblRequestCode";
            this.lblRequestCode.Size = new System.Drawing.Size(70, 12);
            this.lblRequestCode.TabIndex = 19;
            this.lblRequestCode.Text = "Request Code";
            // 
            // txtRequestCode
            // 
            this.txtRequestCode.Location = new System.Drawing.Point(112, 264);
            this.txtRequestCode.Name = "txtRequestCode";
            this.txtRequestCode.Size = new System.Drawing.Size(208, 22);
            this.txtRequestCode.TabIndex = 20;
            // 
            // lblAuthorizationCode
            // 
            this.lblAuthorizationCode.AutoSize = true;
            this.lblAuthorizationCode.Location = new System.Drawing.Point(8, 296);
            this.lblAuthorizationCode.Name = "lblAuthorizationCode";
            this.lblAuthorizationCode.Size = new System.Drawing.Size(97, 12);
            this.lblAuthorizationCode.TabIndex = 21;
            this.lblAuthorizationCode.Text = "Authorization Code";
            // 
            // txtAuthorizationCode
            // 
            this.txtAuthorizationCode.Location = new System.Drawing.Point(112, 288);
            this.txtAuthorizationCode.Name = "txtAuthorizationCode";
            this.txtAuthorizationCode.Size = new System.Drawing.Size(392, 22);
            this.txtAuthorizationCode.TabIndex = 22;
            // 
            // btnGenerateAuthorizationCode
            // 
            this.btnGenerateAuthorizationCode.Location = new System.Drawing.Point(376, 264);
            this.btnGenerateAuthorizationCode.Name = "btnGenerateAuthorizationCode";
            this.btnGenerateAuthorizationCode.Size = new System.Drawing.Size(128, 23);
            this.btnGenerateAuthorizationCode.TabIndex = 23;
            this.btnGenerateAuthorizationCode.Text = "Generate";
            this.btnGenerateAuthorizationCode.UseVisualStyleBackColor = true;
            this.btnGenerateAuthorizationCode.Click += new System.EventHandler(this.btnGenerateAuthorizationCode_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 344);
            this.Controls.Add(this.btnGenerateAuthorizationCode);
            this.Controls.Add(this.txtAuthorizationCode);
            this.Controls.Add(this.lblAuthorizationCode);
            this.Controls.Add(this.txtRequestCode);
            this.Controls.Add(this.lblRequestCode);
            this.Controls.Add(this.txtTrialKey);
            this.Controls.Add(this.btnTrialKey);
            this.Controls.Add(this.dtpLastTrialDate);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cbxProduct);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtProductKey);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSerialNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numCompany);
            this.Name = "frmMain";
            this.Text = "HROne Product Key Generator";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUser)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numCompany;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numUser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkModuleTaxation;
        private System.Windows.Forms.CheckBox chkModulePayroll;
        private System.Windows.Forms.CheckBox chkModuleLeave;
        private System.Windows.Forms.CheckBox chkModuleAttendance;
        private System.Windows.Forms.CheckBox chkModuleESS;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtProductKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxProduct;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker dtpLastTrialDate;
        private System.Windows.Forms.Button btnTrialKey;
        private System.Windows.Forms.TextBox txtTrialKey;
        private System.Windows.Forms.Label lblRequestCode;
        private System.Windows.Forms.TextBox txtRequestCode;
        private System.Windows.Forms.Label lblAuthorizationCode;
        private System.Windows.Forms.TextBox txtAuthorizationCode;
        private System.Windows.Forms.Button btnGenerateAuthorizationCode;
        private System.Windows.Forms.CheckBox chkModuleCostCenter;
        private System.Windows.Forms.CheckBox chkModuleTraining;
    }
}

