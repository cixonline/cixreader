namespace CIXReader.Forms
{
    sealed partial class Authenticate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Authenticate));
            this.fldLogo = new System.Windows.Forms.PictureBox();
            this.fldPanel = new CIXReader.Controls.CRPanel();
            this.fldForgotPassword = new System.Windows.Forms.LinkLabel();
            this.fldDescription = new System.Windows.Forms.Label();
            this.fldTitle = new System.Windows.Forms.Label();
            this.fldPassword = new System.Windows.Forms.TextBox();
            this.fldUsername = new System.Windows.Forms.TextBox();
            this.fldPLabel = new System.Windows.Forms.Label();
            this.fldULabel = new System.Windows.Forms.Label();
            this.fldOK = new System.Windows.Forms.Button();
            this.fldCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fldLogo)).BeginInit();
            this.fldPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fldLogo
            // 
            this.fldLogo.BackColor = System.Drawing.SystemColors.Window;
            this.fldLogo.Image = global::CIXReader.Properties.Resources.CIXReaderLogo;
            this.fldLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("fldLogo.InitialImage")));
            this.fldLogo.Location = new System.Drawing.Point(13, 13);
            this.fldLogo.Name = "fldLogo";
            this.fldLogo.Size = new System.Drawing.Size(100, 100);
            this.fldLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fldLogo.TabIndex = 12;
            this.fldLogo.TabStop = false;
            // 
            // fldPanel
            // 
            this.fldPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldPanel.BackColor = System.Drawing.SystemColors.Window;
            this.fldPanel.BottomBorderWidth = 1;
            this.fldPanel.Controls.Add(this.fldForgotPassword);
            this.fldPanel.Controls.Add(this.fldDescription);
            this.fldPanel.Controls.Add(this.fldTitle);
            this.fldPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.fldPanel.Gradient = false;
            this.fldPanel.LeftBorderWidth = 0;
            this.fldPanel.Location = new System.Drawing.Point(0, 0);
            this.fldPanel.Name = "fldPanel";
            this.fldPanel.RightBorderWidth = 0;
            this.fldPanel.Size = new System.Drawing.Size(392, 223);
            this.fldPanel.TabIndex = 17;
            this.fldPanel.TopBorderWidth = 0;
            // 
            // fldForgotPassword
            // 
            this.fldForgotPassword.AutoSize = true;
            this.fldForgotPassword.Location = new System.Drawing.Point(126, 186);
            this.fldForgotPassword.Name = "fldForgotPassword";
            this.fldForgotPassword.Size = new System.Drawing.Size(106, 13);
            this.fldForgotPassword.TabIndex = 3;
            this.fldForgotPassword.TabStop = true;
            this.fldForgotPassword.Text = "Forgotten password?";
            this.fldForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.fldForgotPassword_LinkClicked);
            // 
            // fldDescription
            // 
            this.fldDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fldDescription.Location = new System.Drawing.Point(126, 56);
            this.fldDescription.Name = "fldDescription";
            this.fldDescription.Size = new System.Drawing.Size(248, 51);
            this.fldDescription.TabIndex = 2;
            this.fldDescription.Text = "Your username or password were rejected by CIX. Please enter the correct username" +
    " and password below:";
            // 
            // fldTitle
            // 
            this.fldTitle.AutoSize = true;
            this.fldTitle.BackColor = System.Drawing.SystemColors.Window;
            this.fldTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fldTitle.Location = new System.Drawing.Point(126, 13);
            this.fldTitle.Name = "fldTitle";
            this.fldTitle.Size = new System.Drawing.Size(181, 20);
            this.fldTitle.TabIndex = 1;
            this.fldTitle.Text = "Authentication Required";
            // 
            // fldPassword
            // 
            this.fldPassword.Location = new System.Drawing.Point(194, 149);
            this.fldPassword.Name = "fldPassword";
            this.fldPassword.PasswordChar = '*';
            this.fldPassword.Size = new System.Drawing.Size(183, 20);
            this.fldPassword.TabIndex = 16;
            this.fldPassword.TextChanged += new System.EventHandler(this.fldPassword_TextChanged);
            // 
            // fldUsername
            // 
            this.fldUsername.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.fldUsername.Location = new System.Drawing.Point(194, 122);
            this.fldUsername.Name = "fldUsername";
            this.fldUsername.Size = new System.Drawing.Size(183, 20);
            this.fldUsername.TabIndex = 15;
            this.fldUsername.TextChanged += new System.EventHandler(this.fldUsername_TextChanged);
            // 
            // fldPLabel
            // 
            this.fldPLabel.AutoSize = true;
            this.fldPLabel.BackColor = System.Drawing.SystemColors.Window;
            this.fldPLabel.Location = new System.Drawing.Point(126, 152);
            this.fldPLabel.Name = "fldPLabel";
            this.fldPLabel.Size = new System.Drawing.Size(56, 13);
            this.fldPLabel.TabIndex = 14;
            this.fldPLabel.Text = "&Password:";
            // 
            // fldULabel
            // 
            this.fldULabel.AutoSize = true;
            this.fldULabel.BackColor = System.Drawing.SystemColors.Window;
            this.fldULabel.Location = new System.Drawing.Point(126, 125);
            this.fldULabel.Name = "fldULabel";
            this.fldULabel.Size = new System.Drawing.Size(61, 13);
            this.fldULabel.TabIndex = 13;
            this.fldULabel.Text = "&User name:";
            // 
            // fldOK
            // 
            this.fldOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.fldOK.Font = new System.Drawing.Font("Arial", 8F);
            this.fldOK.Location = new System.Drawing.Point(219, 232);
            this.fldOK.Name = "fldOK";
            this.fldOK.Size = new System.Drawing.Size(75, 23);
            this.fldOK.TabIndex = 18;
            this.fldOK.Text = "&Login";
            this.fldOK.UseVisualStyleBackColor = true;
            this.fldOK.Click += new System.EventHandler(this.fldOK_Click);
            // 
            // fldCancel
            // 
            this.fldCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.fldCancel.Font = new System.Drawing.Font("Arial", 8F);
            this.fldCancel.Location = new System.Drawing.Point(302, 232);
            this.fldCancel.Name = "fldCancel";
            this.fldCancel.Size = new System.Drawing.Size(75, 23);
            this.fldCancel.TabIndex = 19;
            this.fldCancel.Text = "Cancel";
            this.fldCancel.UseVisualStyleBackColor = true;
            // 
            // Authenticate
            // 
            this.AcceptButton = this.fldOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 264);
            this.Controls.Add(this.fldLogo);
            this.Controls.Add(this.fldPassword);
            this.Controls.Add(this.fldUsername);
            this.Controls.Add(this.fldPLabel);
            this.Controls.Add(this.fldULabel);
            this.Controls.Add(this.fldOK);
            this.Controls.Add(this.fldCancel);
            this.Controls.Add(this.fldPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Authenticate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authenticate";
            this.Load += new System.EventHandler(this.Authenticate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fldLogo)).EndInit();
            this.fldPanel.ResumeLayout(false);
            this.fldPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox fldLogo;
        private Controls.CRPanel fldPanel;
        private System.Windows.Forms.Label fldTitle;
        private System.Windows.Forms.TextBox fldPassword;
        private System.Windows.Forms.TextBox fldUsername;
        private System.Windows.Forms.Label fldPLabel;
        private System.Windows.Forms.Label fldULabel;
        private System.Windows.Forms.Button fldOK;
        private System.Windows.Forms.Button fldCancel;
        private System.Windows.Forms.LinkLabel fldForgotPassword;
        private System.Windows.Forms.Label fldDescription;
    }
}