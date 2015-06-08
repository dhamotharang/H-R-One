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


    public partial class DetailToolBar : System.Web.UI.UserControl
    {
        public event EventHandler BackButton_Click;
        public event EventHandler NewButton_Click;
        public event EventHandler CopyButton_Click;
        public event EventHandler EditButton_Click;
        public event EventHandler SaveButton_Click;
        public event EventHandler DeleteButton_Click;
        public event EventHandler CustomButton1_Click;
        public event EventHandler CustomButton2_Click;
        public event EventHandler CustomButton3_Click;

        private string m_FunctionCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (BackButton_Click != null)
                BackButton.Click += BackButton_Click;
            if (NewButton_Click != null)
                NewButton.Click += NewButton_Click;
            if (CopyButton_Click != null)
                CopyButton.Click += CopyButton_Click;
            if (EditButton_Click != null)
                EditButton.Click += EditButton_Click;
            if (SaveButton_Click != null)
                SaveButton.Click += SaveButton_Click;
            if (DeleteButton_Click != null)
                DeleteButton.Click += DeleteButton_Click;
            if (CustomButton1_Click != null)
                CustomButton1.Click += CustomButton1_Click;
            if (CustomButton2_Click != null)
                CustomButton2.Click += CustomButton2_Click;
            if (CustomButton3_Click != null)
                CustomButton3.Click += CustomButton3_Click;

            //DeleteButton.OnClientClick = HROne.Translation.PromptMessage.DELETE_GENERIC_JAVASCRIPT;
            DeleteButton.OnClientClick = HROne.Translation.PromptMessage.CreateDeleteConfirmDialogJavascript(DeleteButton);
            RefreshSecurity();
        }

        private void RefreshSecurity()
        {
            if (!string.IsNullOrEmpty(m_FunctionCode))

                if (!WebUtils.CheckPermission(Session, m_FunctionCode, WebUtils.AccessLevel.ReadWrite))
                {
                    NewButton_Visible = false;
                    EditButton_Visible = false;
                    SaveButton_Visible = false;
                    DeleteButton_Visible = false;
                }
        }

        public string FunctionCode
        {
            get { return m_FunctionCode; }
            set { m_FunctionCode = value; RefreshSecurity(); }
        }

        public bool BackButton_Visible
        {
            get { return BackButton.Visible; }
            set { BackButton.Visible = value; }
        }
        public string BackButton_ClientClick
        {
            get { return BackButton.OnClientClick; }
            set { BackButton.OnClientClick = value; }
        }
        public bool NewButton_Visible
        {
            get { return NewButton.Visible; }
            set { NewButton.Visible = value; }
        }
        public string NewButton_ClientClick
        {
            get { return NewButton.OnClientClick; }
            set { NewButton.OnClientClick = value; }
        }
        public bool CopyButton_Visible
        {
            get { return CopyButton.Visible; }
            set { CopyButton.Visible = value; }
        }
        public string CopyButton_ClientClick
        {
            get { return CopyButton.OnClientClick; }
            set { CopyButton.OnClientClick = value; }
        }

        public bool EditButton_Visible
        {
            get { return EditButton.Visible; }
            set { EditButton.Visible = value; }
        }
        public string EditButton_ClientClick
        {
            get { return EditButton.OnClientClick; }
            set { EditButton.OnClientClick = value; }
        }

        public bool SaveButton_Visible
        {
            get { return SaveButton.Visible; }
            set { SaveButton.Visible = value; }
        }
        public string SaveButton_ClientClick
        {
            get { return SaveButton.OnClientClick; }
            set { SaveButton.OnClientClick = value; }
        }

        public bool DeleteButton_Visible
        {
            get { return DeleteButton.Visible; }
            set { DeleteButton.Visible = value; }
        }
        public string DeleteButton_ClientClick
        {
            get { return DeleteButton.OnClientClick; }
            set { DeleteButton.OnClientClick = value; }
        }

        public string CustomButton1_Name
        {
            get { return CustomButton1.Text; }
            set { CustomButton1.Text = value; }
        }
        public bool CustomButton1_Visible
        {
            get { return CustomButton1.Visible; }
            set { CustomButton1.Visible = value; }
        }
        public string CustomButton1_ClientClick
        {
            get { return CustomButton1.OnClientClick; }
            set { CustomButton1.OnClientClick = value; }
        }

        public string CustomButton2_Name
        {
            get { return CustomButton2.Text; }
            set { CustomButton2.Text = value; }
        }
        public bool CustomButton2_Visible
        {
            get { return CustomButton2.Visible; }
            set { CustomButton2.Visible = value; }
        }
        public string CustomButton2_ClientClick
        {
            get { return CustomButton2.OnClientClick; }
            set { CustomButton2.OnClientClick = value; }
        }

        public string CustomButton3_Name
        {
            get { return CustomButton3.Text; }
            set { CustomButton3.Text = value; }
        }
        public bool CustomButton3_Visible
        {
            get { return CustomButton3.Visible; }
            set { CustomButton3.Visible = value; }
        }
        public string CustomButton3_ClientClick
        {
            get { return CustomButton3.OnClientClick; }
            set { CustomButton3.OnClientClick = value; }
        }
    }
