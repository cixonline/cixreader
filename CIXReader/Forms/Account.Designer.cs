namespace CIXReader.Forms
{
    sealed partial class Account
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
            this.actCancel = new System.Windows.Forms.Button();
            this.actSave = new System.Windows.Forms.Button();
            this.accountPanel = new CIXReader.Controls.CRPanel();
            this.actNotifyTag = new System.Windows.Forms.CheckBox();
            this.actNotifyPM = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.actSeparator = new System.Windows.Forms.GroupBox();
            this.actAccountType = new System.Windows.Forms.Label();
            this.actAccountUpgrade = new System.Windows.Forms.LinkLabel();
            this.actAbout = new System.Windows.Forms.TextBox();
            this.actFullname = new System.Windows.Forms.TextBox();
            this.actAboutLabel = new System.Windows.Forms.Label();
            this.actUsername = new System.Windows.Forms.Label();
            this.actFullNameLabel = new System.Windows.Forms.Label();
            this.actAccountTypeLabel = new System.Windows.Forms.Label();
            this.actLocationLabel = new System.Windows.Forms.Label();
            this.actUserNameLabel = new System.Windows.Forms.Label();
            this.actLocation = new System.Windows.Forms.TextBox();
            this.actEmailLabel = new System.Windows.Forms.Label();
            this.actEmail = new System.Windows.Forms.TextBox();
            this.actSexLabel = new System.Windows.Forms.Label();
            this.actSexFemale = new System.Windows.Forms.RadioButton();
            this.actRemove = new System.Windows.Forms.Button();
            this.actUpload = new System.Windows.Forms.Button();
            this.actSexMale = new System.Windows.Forms.RadioButton();
            this.actSexDontSay = new System.Windows.Forms.RadioButton();
            this.actMugshot = new System.Windows.Forms.PictureBox();
            this.accountPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actMugshot)).BeginInit();
            this.SuspendLayout();
            // 
            // actCancel
            // 
            this.actCancel.Font = new System.Drawing.Font("Arial", 8F);
            this.actCancel.Location = new System.Drawing.Point(427, 458);
            this.actCancel.Name = "actCancel";
            this.actCancel.Size = new System.Drawing.Size(75, 23);
            this.actCancel.TabIndex = 2;
            this.actCancel.Text = "Cancel";
            this.actCancel.UseVisualStyleBackColor = true;
            this.actCancel.Click += new System.EventHandler(this.actCancel_Click);
            // 
            // actSave
            // 
            this.actSave.Font = new System.Drawing.Font("Arial", 8F);
            this.actSave.Location = new System.Drawing.Point(346, 458);
            this.actSave.Name = "actSave";
            this.actSave.Size = new System.Drawing.Size(75, 23);
            this.actSave.TabIndex = 1;
            this.actSave.Text = "&Save";
            this.actSave.UseVisualStyleBackColor = true;
            this.actSave.Click += new System.EventHandler(this.actSave_Click);
            // 
            // accountPanel
            // 
            this.accountPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountPanel.BackColor = System.Drawing.SystemColors.Window;
            this.accountPanel.BottomBorderWidth = 1;
            this.accountPanel.Controls.Add(this.actNotifyTag);
            this.accountPanel.Controls.Add(this.actNotifyPM);
            this.accountPanel.Controls.Add(this.label1);
            this.accountPanel.Controls.Add(this.actSeparator);
            this.accountPanel.Controls.Add(this.actAccountType);
            this.accountPanel.Controls.Add(this.actAccountUpgrade);
            this.accountPanel.Controls.Add(this.actAbout);
            this.accountPanel.Controls.Add(this.actFullname);
            this.accountPanel.Controls.Add(this.actAboutLabel);
            this.accountPanel.Controls.Add(this.actUsername);
            this.accountPanel.Controls.Add(this.actFullNameLabel);
            this.accountPanel.Controls.Add(this.actAccountTypeLabel);
            this.accountPanel.Controls.Add(this.actLocationLabel);
            this.accountPanel.Controls.Add(this.actUserNameLabel);
            this.accountPanel.Controls.Add(this.actLocation);
            this.accountPanel.Controls.Add(this.actEmailLabel);
            this.accountPanel.Controls.Add(this.actEmail);
            this.accountPanel.Controls.Add(this.actSexLabel);
            this.accountPanel.Controls.Add(this.actSexFemale);
            this.accountPanel.Controls.Add(this.actRemove);
            this.accountPanel.Controls.Add(this.actUpload);
            this.accountPanel.Controls.Add(this.actSexMale);
            this.accountPanel.Controls.Add(this.actSexDontSay);
            this.accountPanel.Controls.Add(this.actMugshot);
            this.accountPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.accountPanel.Gradient = false;
            this.accountPanel.LeftBorderWidth = 0;
            this.accountPanel.Location = new System.Drawing.Point(0, 0);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.RightBorderWidth = 0;
            this.accountPanel.Size = new System.Drawing.Size(512, 449);
            this.accountPanel.TabIndex = 0;
            this.accountPanel.TopBorderWidth = 0;
            // 
            // actNotifyTag
            // 
            this.actNotifyTag.AutoSize = true;
            this.actNotifyTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actNotifyTag.Location = new System.Drawing.Point(346, 225);
            this.actNotifyTag.Name = "actNotifyTag";
            this.actNotifyTag.Size = new System.Drawing.Size(65, 17);
            this.actNotifyTag.TabIndex = 12;
            this.actNotifyTag.Text = "Ta&gging";
            this.actNotifyTag.UseVisualStyleBackColor = true;
            this.actNotifyTag.Click += new System.EventHandler(this.OnAccountChanged);
            // 
            // actNotifyPM
            // 
            this.actNotifyPM.AutoSize = true;
            this.actNotifyPM.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actNotifyPM.Location = new System.Drawing.Point(225, 225);
            this.actNotifyPM.Name = "actNotifyPM";
            this.actNotifyPM.Size = new System.Drawing.Size(110, 17);
            this.actNotifyPM.TabIndex = 11;
            this.actNotifyPM.Text = "Pri&vate Messages";
            this.actNotifyPM.UseVisualStyleBackColor = true;
            this.actNotifyPM.Click += new System.EventHandler(this.OnAccountChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(140, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Notify:";
            // 
            // actSeparator
            // 
            this.actSeparator.Location = new System.Drawing.Point(143, 83);
            this.actSeparator.Name = "actSeparator";
            this.actSeparator.Size = new System.Drawing.Size(343, 2);
            this.actSeparator.TabIndex = 22;
            this.actSeparator.TabStop = false;
            // 
            // actAccountType
            // 
            this.actAccountType.AutoSize = true;
            this.actAccountType.BackColor = System.Drawing.SystemColors.Window;
            this.actAccountType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actAccountType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actAccountType.Location = new System.Drawing.Point(252, 49);
            this.actAccountType.Name = "actAccountType";
            this.actAccountType.Size = new System.Drawing.Size(52, 13);
            this.actAccountType.TabIndex = 20;
            this.actAccountType.Text = "(typefield)";
            // 
            // actAccountUpgrade
            // 
            this.actAccountUpgrade.AutoSize = true;
            this.actAccountUpgrade.BackColor = System.Drawing.SystemColors.Window;
            this.actAccountUpgrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actAccountUpgrade.Location = new System.Drawing.Point(370, 49);
            this.actAccountUpgrade.Name = "actAccountUpgrade";
            this.actAccountUpgrade.Size = new System.Drawing.Size(122, 13);
            this.actAccountUpgrade.TabIndex = 21;
            this.actAccountUpgrade.TabStop = true;
            this.actAccountUpgrade.Text = "U&pgrade to Full Account";
            this.actAccountUpgrade.Visible = false;
            this.actAccountUpgrade.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.actAccountUpgrade_LinkClicked);
            // 
            // actAbout
            // 
            this.actAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actAbout.Location = new System.Drawing.Point(143, 277);
            this.actAbout.Multiline = true;
            this.actAbout.Name = "actAbout";
            this.actAbout.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.actAbout.Size = new System.Drawing.Size(344, 155);
            this.actAbout.TabIndex = 14;
            this.actAbout.TextChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // actFullname
            // 
            this.actFullname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actFullname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actFullname.Location = new System.Drawing.Point(225, 103);
            this.actFullname.Name = "actFullname";
            this.actFullname.Size = new System.Drawing.Size(261, 20);
            this.actFullname.TabIndex = 1;
            this.actFullname.TextChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // actAboutLabel
            // 
            this.actAboutLabel.AutoSize = true;
            this.actAboutLabel.BackColor = System.Drawing.SystemColors.Window;
            this.actAboutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actAboutLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actAboutLabel.Location = new System.Drawing.Point(140, 261);
            this.actAboutLabel.Name = "actAboutLabel";
            this.actAboutLabel.Size = new System.Drawing.Size(70, 13);
            this.actAboutLabel.TabIndex = 13;
            this.actAboutLabel.Text = "&About You:";
            // 
            // actUsername
            // 
            this.actUsername.AutoSize = true;
            this.actUsername.BackColor = System.Drawing.SystemColors.Window;
            this.actUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actUsername.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actUsername.Location = new System.Drawing.Point(252, 19);
            this.actUsername.Name = "actUsername";
            this.actUsername.Size = new System.Drawing.Size(61, 13);
            this.actUsername.TabIndex = 18;
            this.actUsername.Text = "(Username)";
            // 
            // actFullNameLabel
            // 
            this.actFullNameLabel.AutoSize = true;
            this.actFullNameLabel.BackColor = System.Drawing.SystemColors.Window;
            this.actFullNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actFullNameLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actFullNameLabel.Location = new System.Drawing.Point(140, 106);
            this.actFullNameLabel.Name = "actFullNameLabel";
            this.actFullNameLabel.Size = new System.Drawing.Size(67, 13);
            this.actFullNameLabel.TabIndex = 0;
            this.actFullNameLabel.Text = "Full &Name:";
            // 
            // actAccountTypeLabel
            // 
            this.actAccountTypeLabel.AutoSize = true;
            this.actAccountTypeLabel.BackColor = System.Drawing.SystemColors.Window;
            this.actAccountTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actAccountTypeLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actAccountTypeLabel.Location = new System.Drawing.Point(140, 49);
            this.actAccountTypeLabel.Name = "actAccountTypeLabel";
            this.actAccountTypeLabel.Size = new System.Drawing.Size(90, 13);
            this.actAccountTypeLabel.TabIndex = 19;
            this.actAccountTypeLabel.Text = "Account Type:";
            // 
            // actLocationLabel
            // 
            this.actLocationLabel.AutoSize = true;
            this.actLocationLabel.BackColor = System.Drawing.SystemColors.Window;
            this.actLocationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLocationLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actLocationLabel.Location = new System.Drawing.Point(140, 136);
            this.actLocationLabel.Name = "actLocationLabel";
            this.actLocationLabel.Size = new System.Drawing.Size(60, 13);
            this.actLocationLabel.TabIndex = 2;
            this.actLocationLabel.Text = "&Location:";
            // 
            // actUserNameLabel
            // 
            this.actUserNameLabel.AutoSize = true;
            this.actUserNameLabel.BackColor = System.Drawing.SystemColors.Window;
            this.actUserNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actUserNameLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actUserNameLabel.Location = new System.Drawing.Point(140, 19);
            this.actUserNameLabel.Name = "actUserNameLabel";
            this.actUserNameLabel.Size = new System.Drawing.Size(73, 13);
            this.actUserNameLabel.TabIndex = 17;
            this.actUserNameLabel.Text = "User Name:";
            // 
            // actLocation
            // 
            this.actLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLocation.Location = new System.Drawing.Point(225, 133);
            this.actLocation.Name = "actLocation";
            this.actLocation.Size = new System.Drawing.Size(261, 20);
            this.actLocation.TabIndex = 3;
            this.actLocation.TextChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // actEmailLabel
            // 
            this.actEmailLabel.AutoSize = true;
            this.actEmailLabel.BackColor = System.Drawing.SystemColors.Window;
            this.actEmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actEmailLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actEmailLabel.Location = new System.Drawing.Point(140, 166);
            this.actEmailLabel.Name = "actEmailLabel";
            this.actEmailLabel.Size = new System.Drawing.Size(46, 13);
            this.actEmailLabel.TabIndex = 4;
            this.actEmailLabel.Text = "&E-Mail:";
            // 
            // actEmail
            // 
            this.actEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actEmail.Location = new System.Drawing.Point(225, 163);
            this.actEmail.Name = "actEmail";
            this.actEmail.Size = new System.Drawing.Size(261, 20);
            this.actEmail.TabIndex = 5;
            this.actEmail.TextChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // actSexLabel
            // 
            this.actSexLabel.AutoSize = true;
            this.actSexLabel.BackColor = System.Drawing.SystemColors.Window;
            this.actSexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actSexLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actSexLabel.Location = new System.Drawing.Point(140, 196);
            this.actSexLabel.Name = "actSexLabel";
            this.actSexLabel.Size = new System.Drawing.Size(32, 13);
            this.actSexLabel.TabIndex = 6;
            this.actSexLabel.Text = "Sex:";
            // 
            // actSexFemale
            // 
            this.actSexFemale.AutoSize = true;
            this.actSexFemale.BackColor = System.Drawing.SystemColors.Window;
            this.actSexFemale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actSexFemale.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actSexFemale.Location = new System.Drawing.Point(225, 193);
            this.actSexFemale.Name = "actSexFemale";
            this.actSexFemale.Size = new System.Drawing.Size(59, 17);
            this.actSexFemale.TabIndex = 7;
            this.actSexFemale.TabStop = true;
            this.actSexFemale.Text = "&Female";
            this.actSexFemale.UseVisualStyleBackColor = false;
            this.actSexFemale.CheckedChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // actRemove
            // 
            this.actRemove.Font = new System.Drawing.Font("Arial", 8F);
            this.actRemove.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actRemove.Location = new System.Drawing.Point(29, 151);
            this.actRemove.Name = "actRemove";
            this.actRemove.Size = new System.Drawing.Size(75, 23);
            this.actRemove.TabIndex = 16;
            this.actRemove.Text = "&Remove";
            this.actRemove.UseVisualStyleBackColor = true;
            this.actRemove.Click += new System.EventHandler(this.actRemove_Click);
            // 
            // actUpload
            // 
            this.actUpload.Font = new System.Drawing.Font("Arial", 8F);
            this.actUpload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actUpload.Location = new System.Drawing.Point(29, 122);
            this.actUpload.Name = "actUpload";
            this.actUpload.Size = new System.Drawing.Size(75, 23);
            this.actUpload.TabIndex = 15;
            this.actUpload.Text = "&Browse...";
            this.actUpload.UseVisualStyleBackColor = true;
            this.actUpload.Click += new System.EventHandler(this.actUpload_Click);
            // 
            // actSexMale
            // 
            this.actSexMale.AutoSize = true;
            this.actSexMale.BackColor = System.Drawing.SystemColors.Window;
            this.actSexMale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actSexMale.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actSexMale.Location = new System.Drawing.Point(291, 193);
            this.actSexMale.Name = "actSexMale";
            this.actSexMale.Size = new System.Drawing.Size(48, 17);
            this.actSexMale.TabIndex = 8;
            this.actSexMale.TabStop = true;
            this.actSexMale.Text = "&Male";
            this.actSexMale.UseVisualStyleBackColor = false;
            this.actSexMale.CheckedChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // actSexDontSay
            // 
            this.actSexDontSay.AutoSize = true;
            this.actSexDontSay.BackColor = System.Drawing.SystemColors.Window;
            this.actSexDontSay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actSexDontSay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.actSexDontSay.Location = new System.Drawing.Point(346, 193);
            this.actSexDontSay.Name = "actSexDontSay";
            this.actSexDontSay.Size = new System.Drawing.Size(71, 17);
            this.actSexDontSay.TabIndex = 9;
            this.actSexDontSay.TabStop = true;
            this.actSexDontSay.Text = "&Don\'t Say";
            this.actSexDontSay.UseVisualStyleBackColor = false;
            this.actSexDontSay.CheckedChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // actMugshot
            // 
            this.actMugshot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actMugshot.Location = new System.Drawing.Point(16, 12);
            this.actMugshot.Name = "actMugshot";
            this.actMugshot.Size = new System.Drawing.Size(100, 100);
            this.actMugshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.actMugshot.TabIndex = 41;
            this.actMugshot.TabStop = false;
            this.actMugshot.DragDrop += new System.Windows.Forms.DragEventHandler(this.actMugshot_DragDrop);
            this.actMugshot.DragEnter += new System.Windows.Forms.DragEventHandler(this.actMugshot_DragEnter);
            this.actMugshot.DragOver += new System.Windows.Forms.DragEventHandler(this.actMugshot_DragOver);
            this.actMugshot.DragLeave += new System.EventHandler(this.actMugshot_DragLeave);
            // 
            // Account
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 490);
            this.Controls.Add(this.actCancel);
            this.Controls.Add(this.actSave);
            this.Controls.Add(this.accountPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Account";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account";
            this.Load += new System.EventHandler(this.Account_Load);
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actMugshot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel actAccountUpgrade;
        private System.Windows.Forms.Label actAccountType;
        private System.Windows.Forms.Label actUsername;
        private System.Windows.Forms.Label actAccountTypeLabel;
        private System.Windows.Forms.Label actUserNameLabel;
        private System.Windows.Forms.Button actCancel;
        private System.Windows.Forms.Button actSave;
        private System.Windows.Forms.TextBox actAbout;
        private System.Windows.Forms.Button actUpload;
        private System.Windows.Forms.Button actRemove;
        private System.Windows.Forms.PictureBox actMugshot;
        private System.Windows.Forms.RadioButton actSexDontSay;
        private System.Windows.Forms.RadioButton actSexMale;
        private System.Windows.Forms.RadioButton actSexFemale;
        private System.Windows.Forms.Label actAboutLabel;
        private System.Windows.Forms.Label actSexLabel;
        private System.Windows.Forms.TextBox actEmail;
        private System.Windows.Forms.Label actEmailLabel;
        private System.Windows.Forms.TextBox actLocation;
        private System.Windows.Forms.Label actLocationLabel;
        private System.Windows.Forms.TextBox actFullname;
        private System.Windows.Forms.Label actFullNameLabel;
        private Controls.CRPanel accountPanel;
        private System.Windows.Forms.GroupBox actSeparator;
        private System.Windows.Forms.CheckBox actNotifyTag;
        private System.Windows.Forms.CheckBox actNotifyPM;
        private System.Windows.Forms.Label label1;
    }
}