using CIXReader.Controls;

namespace CIXReader.Forms
{
    internal sealed partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.fldLogo = new System.Windows.Forms.PictureBox();
            this.fldUsernameLabel = new System.Windows.Forms.Label();
            this.fldPasswordLabel = new System.Windows.Forms.Label();
            this.fldUsername = new System.Windows.Forms.TextBox();
            this.fldPassword = new System.Windows.Forms.TextBox();
            this.fldCancel = new System.Windows.Forms.Button();
            this.fldOK = new System.Windows.Forms.Button();
            this.fldPanel = new CIXReader.Controls.CRPanel();
            this.fldError = new System.Windows.Forms.Label();
            this.fldSignupText = new System.Windows.Forms.Label();
            this.fldSignupLink = new System.Windows.Forms.LinkLabel();
            this.fldTitle = new System.Windows.Forms.Label();
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
            this.fldLogo.TabIndex = 0;
            this.fldLogo.TabStop = false;
            // 
            // fldUsernameLabel
            // 
            this.fldUsernameLabel.AutoSize = true;
            this.fldUsernameLabel.BackColor = System.Drawing.SystemColors.Window;
            this.fldUsernameLabel.Location = new System.Drawing.Point(126, 57);
            this.fldUsernameLabel.Name = "fldUsernameLabel";
            this.fldUsernameLabel.Size = new System.Drawing.Size(61, 13);
            this.fldUsernameLabel.TabIndex = 2;
            this.fldUsernameLabel.Text = "&User name:";
            // 
            // fldPasswordLabel
            // 
            this.fldPasswordLabel.AutoSize = true;
            this.fldPasswordLabel.BackColor = System.Drawing.SystemColors.Window;
            this.fldPasswordLabel.Location = new System.Drawing.Point(126, 84);
            this.fldPasswordLabel.Name = "fldPasswordLabel";
            this.fldPasswordLabel.Size = new System.Drawing.Size(56, 13);
            this.fldPasswordLabel.TabIndex = 3;
            this.fldPasswordLabel.Text = "&Password:";
            // 
            // fldUsername
            // 
            this.fldUsername.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.fldUsername.Location = new System.Drawing.Point(194, 54);
            this.fldUsername.Name = "fldUsername";
            this.fldUsername.Size = new System.Drawing.Size(183, 20);
            this.fldUsername.TabIndex = 4;
            this.fldUsername.TextChanged += new System.EventHandler(this.fldUsername_TextChanged);
            // 
            // fldPassword
            // 
            this.fldPassword.Location = new System.Drawing.Point(194, 81);
            this.fldPassword.Name = "fldPassword";
            this.fldPassword.PasswordChar = '*';
            this.fldPassword.Size = new System.Drawing.Size(183, 20);
            this.fldPassword.TabIndex = 5;
            this.fldPassword.TextChanged += new System.EventHandler(this.fldPassword_TextChanged);
            // 
            // fldCancel
            // 
            this.fldCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.fldCancel.Font = new System.Drawing.Font("Arial", 8F);
            this.fldCancel.Location = new System.Drawing.Point(302, 228);
            this.fldCancel.Name = "fldCancel";
            this.fldCancel.Size = new System.Drawing.Size(75, 23);
            this.fldCancel.TabIndex = 11;
            this.fldCancel.Text = "Cancel";
            this.fldCancel.UseVisualStyleBackColor = true;
            this.fldCancel.Click += new System.EventHandler(this.fldCancel_Click);
            // 
            // fldOK
            // 
            this.fldOK.Font = new System.Drawing.Font("Arial", 8F);
            this.fldOK.Location = new System.Drawing.Point(219, 228);
            this.fldOK.Name = "fldOK";
            this.fldOK.Size = new System.Drawing.Size(75, 23);
            this.fldOK.TabIndex = 10;
            this.fldOK.Text = "&Login";
            this.fldOK.UseVisualStyleBackColor = true;
            this.fldOK.Click += new System.EventHandler(this.fldOK_Click);
            // 
            // fldPanel
            // 
            this.fldPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fldPanel.BackColor = System.Drawing.SystemColors.Window;
            this.fldPanel.BottomBorderWidth = 1;
            this.fldPanel.Controls.Add(this.fldError);
            this.fldPanel.Controls.Add(this.fldSignupText);
            this.fldPanel.Controls.Add(this.fldSignupLink);
            this.fldPanel.Controls.Add(this.fldTitle);
            this.fldPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.fldPanel.Gradient = false;
            this.fldPanel.LeftBorderWidth = 0;
            this.fldPanel.Location = new System.Drawing.Point(0, 0);
            this.fldPanel.Name = "fldPanel";
            this.fldPanel.RightBorderWidth = 0;
            this.fldPanel.Size = new System.Drawing.Size(389, 219);
            this.fldPanel.TabIndex = 7;
            this.fldPanel.TopBorderWidth = 0;
            // 
            // fldError
            // 
            this.fldError.AutoSize = true;
            this.fldError.ForeColor = System.Drawing.Color.Red;
            this.fldError.Location = new System.Drawing.Point(126, 115);
            this.fldError.Name = "fldError";
            this.fldError.Size = new System.Drawing.Size(57, 13);
            this.fldError.TabIndex = 11;
            this.fldError.Text = "(Error field)";
            this.fldError.Visible = false;
            // 
            // fldSignupText
            // 
            this.fldSignupText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fldSignupText.Location = new System.Drawing.Point(126, 148);
            this.fldSignupText.Name = "fldSignupText";
            this.fldSignupText.Size = new System.Drawing.Size(251, 31);
            this.fldSignupText.TabIndex = 8;
            this.fldSignupText.Text = "If you do not have an account with CIX, you can sign up for a free Basic Account " +
    "online.";
            // 
            // fldSignupLink
            // 
            this.fldSignupLink.AutoSize = true;
            this.fldSignupLink.Location = new System.Drawing.Point(126, 183);
            this.fldSignupLink.Name = "fldSignupLink";
            this.fldSignupLink.Size = new System.Drawing.Size(75, 13);
            this.fldSignupLink.TabIndex = 9;
            this.fldSignupLink.TabStop = true;
            this.fldSignupLink.Text = "Sign up to CIX";
            this.fldSignupLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.fldSignupLink_LinkClicked);
            // 
            // fldTitle
            // 
            this.fldTitle.AutoSize = true;
            this.fldTitle.BackColor = System.Drawing.SystemColors.Window;
            this.fldTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fldTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fldTitle.Location = new System.Drawing.Point(125, 13);
            this.fldTitle.Name = "fldTitle";
            this.fldTitle.Size = new System.Drawing.Size(48, 20);
            this.fldTitle.TabIndex = 1;
            this.fldTitle.Text = "(Title)";
            // 
            // Login
            // 
            this.AcceptButton = this.fldOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.fldCancel;
            this.ClientSize = new System.Drawing.Size(389, 260);
            this.Controls.Add(this.fldOK);
            this.Controls.Add(this.fldCancel);
            this.Controls.Add(this.fldPassword);
            this.Controls.Add(this.fldUsername);
            this.Controls.Add(this.fldPasswordLabel);
            this.Controls.Add(this.fldUsernameLabel);
            this.Controls.Add(this.fldLogo);
            this.Controls.Add(this.fldPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fldLogo)).EndInit();
            this.fldPanel.ResumeLayout(false);
            this.fldPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox fldLogo;
        private System.Windows.Forms.Label fldTitle;
        private System.Windows.Forms.Label fldUsernameLabel;
        private System.Windows.Forms.Label fldPasswordLabel;
        private System.Windows.Forms.TextBox fldUsername;
        private System.Windows.Forms.TextBox fldPassword;
        private CRPanel fldPanel;
        private System.Windows.Forms.Button fldCancel;
        private System.Windows.Forms.Button fldOK;
        private System.Windows.Forms.Label fldSignupText;
        private System.Windows.Forms.LinkLabel fldSignupLink;
        private System.Windows.Forms.Label fldError;
    }
}