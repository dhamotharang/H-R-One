namespace HROne.MigrationTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTargetDBVersion = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmdApplyDBScript = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.cmdEncrypt = new System.Windows.Forms.Button();
            this.txtNewKey = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdDecrypt = new System.Windows.Forms.Button();
            this.txtCurrentKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdDecrypt2 = new System.Windows.Forms.Button();
            this.cmdEncrypt2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(741, 64);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdEncrypt2);
            this.groupBox1.Controls.Add(this.cmdDecrypt2);
            this.groupBox1.Controls.Add(this.txtTargetDBVersion);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmdApplyDBScript);
            this.groupBox1.Controls.Add(this.txtResult);
            this.groupBox1.Controls.Add(this.cmdEncrypt);
            this.groupBox1.Controls.Add(this.txtNewKey);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtDatabase);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmdDecrypt);
            this.groupBox1.Controls.Add(this.txtCurrentKey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(15, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(819, 389);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtTargetDBVersion
            // 
            this.txtTargetDBVersion.Location = new System.Drawing.Point(477, 30);
            this.txtTargetDBVersion.Name = "txtTargetDBVersion";
            this.txtTargetDBVersion.Size = new System.Drawing.Size(110, 22);
            this.txtTargetDBVersion.TabIndex = 4;
            this.txtTargetDBVersion.Text = "2.3.588";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(348, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "Target DB Version: ";
            // 
            // cmdApplyDBScript
            // 
            this.cmdApplyDBScript.Location = new System.Drawing.Point(621, 30);
            this.cmdApplyDBScript.Name = "cmdApplyDBScript";
            this.cmdApplyDBScript.Size = new System.Drawing.Size(179, 27);
            this.cmdApplyDBScript.TabIndex = 7;
            this.cmdApplyDBScript.Text = "Apply DB Upgrade Script";
            this.cmdApplyDBScript.UseVisualStyleBackColor = true;
            this.cmdApplyDBScript.Click += new System.EventHandler(this.cmdApplyDBScript_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(11, 207);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(789, 162);
            this.txtResult.TabIndex = 9;
            // 
            // cmdEncrypt
            // 
            this.cmdEncrypt.Location = new System.Drawing.Point(621, 130);
            this.cmdEncrypt.Name = "cmdEncrypt";
            this.cmdEncrypt.Size = new System.Drawing.Size(179, 27);
            this.cmdEncrypt.TabIndex = 10;
            this.cmdEncrypt.Text = "Encrypt data";
            this.cmdEncrypt.UseVisualStyleBackColor = true;
            this.cmdEncrypt.Click += new System.EventHandler(this.cmdEncrypt_Click);
            // 
            // txtNewKey
            // 
            this.txtNewKey.Location = new System.Drawing.Point(477, 134);
            this.txtNewKey.Name = "txtNewKey";
            this.txtNewKey.Size = new System.Drawing.Size(110, 22);
            this.txtNewKey.TabIndex = 6;
            this.txtNewKey.Text = "HROneHROne";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(348, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "New Encryption Key:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(138, 140);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(110, 22);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.Text = "hrone";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "Password:";
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(138, 101);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(110, 22);
            this.txtUserID.TabIndex = 2;
            this.txtUserID.Text = "hrone";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "User ID:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(138, 64);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(175, 22);
            this.txtDatabase.TabIndex = 1;
            this.txtDatabase.Text = "HROneForConversion";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Database:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(138, 30);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(175, 22);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "localhost\\sqlexpress";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Server:";
            // 
            // cmdDecrypt
            // 
            this.cmdDecrypt.Location = new System.Drawing.Point(621, 63);
            this.cmdDecrypt.Name = "cmdDecrypt";
            this.cmdDecrypt.Size = new System.Drawing.Size(179, 27);
            this.cmdDecrypt.TabIndex = 8;
            this.cmdDecrypt.Text = "Decrypt existing data";
            this.cmdDecrypt.UseVisualStyleBackColor = true;
            this.cmdDecrypt.Click += new System.EventHandler(this.cmdDecrypt_Click);
            // 
            // txtCurrentKey
            // 
            this.txtCurrentKey.Location = new System.Drawing.Point(477, 67);
            this.txtCurrentKey.Name = "txtCurrentKey";
            this.txtCurrentKey.Size = new System.Drawing.Size(110, 22);
            this.txtCurrentKey.TabIndex = 5;
            this.txtCurrentKey.Text = "HRPlusHRExpress";
            this.txtCurrentKey.TextChanged += new System.EventHandler(this.txtCurrentKey_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Existing Encryption Key:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cmdDecrypt2
            // 
            this.cmdDecrypt2.Location = new System.Drawing.Point(621, 97);
            this.cmdDecrypt2.Name = "cmdDecrypt2";
            this.cmdDecrypt2.Size = new System.Drawing.Size(179, 27);
            this.cmdDecrypt2.TabIndex = 9;
            this.cmdDecrypt2.Text = "Decrypt existing data 2";
            this.cmdDecrypt2.UseVisualStyleBackColor = true;
            this.cmdDecrypt2.Click += new System.EventHandler(this.cmdDecrypt2_Click);
            // 
            // cmdEncrypt2
            // 
            this.cmdEncrypt2.Location = new System.Drawing.Point(621, 165);
            this.cmdEncrypt2.Name = "cmdEncrypt2";
            this.cmdEncrypt2.Size = new System.Drawing.Size(179, 27);
            this.cmdEncrypt2.TabIndex = 11;
            this.cmdEncrypt2.Text = "Encrypt data 2";
            this.cmdEncrypt2.UseVisualStyleBackColor = true;
            this.cmdEncrypt2.Click += new System.EventHandler(this.cmdEncrypt2_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 497);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "frmMain";
            this.Text = "HROne Migration Tool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCurrentKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdDecrypt;
        private System.Windows.Forms.TextBox txtNewKey;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdEncrypt;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button cmdApplyDBScript;
        private System.Windows.Forms.TextBox txtTargetDBVersion;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button cmdEncrypt2;
        private System.Windows.Forms.Button cmdDecrypt2;
    }
}

