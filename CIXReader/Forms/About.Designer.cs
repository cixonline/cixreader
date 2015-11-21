using CIXReader.Controls;

namespace CIXReader.Forms
{
    sealed partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.abtLogo = new System.Windows.Forms.PictureBox();
            this.abtPanel = new CIXReader.Controls.CRPanel();
            this.abtLicenseText = new System.Windows.Forms.Label();
            this.abtLinkLabel = new System.Windows.Forms.LinkLabel();
            this.abtCopyright = new System.Windows.Forms.Label();
            this.abtVersion = new System.Windows.Forms.Label();
            this.abtClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.abtLogo)).BeginInit();
            this.abtPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // abtLogo
            // 
            this.abtLogo.BackColor = System.Drawing.SystemColors.Window;
            this.abtLogo.Image = global::CIXReader.Properties.Resources.CIXReaderLogo;
            this.abtLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("abtLogo.InitialImage")));
            this.abtLogo.Location = new System.Drawing.Point(13, 14);
            this.abtLogo.Name = "abtLogo";
            this.abtLogo.Size = new System.Drawing.Size(100, 100);
            this.abtLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.abtLogo.TabIndex = 7;
            this.abtLogo.TabStop = false;
            // 
            // abtPanel
            // 
            this.abtPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.abtPanel.BackColor = System.Drawing.SystemColors.Window;
            this.abtPanel.BottomBorderWidth = 1;
            this.abtPanel.Controls.Add(this.abtLicenseText);
            this.abtPanel.Controls.Add(this.abtLinkLabel);
            this.abtPanel.Controls.Add(this.abtCopyright);
            this.abtPanel.Controls.Add(this.abtVersion);
            this.abtPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.abtPanel.Gradient = false;
            this.abtPanel.LeftBorderWidth = 0;
            this.abtPanel.Location = new System.Drawing.Point(0, 0);
            this.abtPanel.Name = "abtPanel";
            this.abtPanel.RightBorderWidth = 0;
            this.abtPanel.Size = new System.Drawing.Size(389, 149);
            this.abtPanel.TabIndex = 14;
            this.abtPanel.TopBorderWidth = 0;
            // 
            // abtLicenseText
            // 
            this.abtLicenseText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.abtLicenseText.Location = new System.Drawing.Point(125, 60);
            this.abtLicenseText.Name = "abtLicenseText";
            this.abtLicenseText.Size = new System.Drawing.Size(252, 42);
            this.abtLicenseText.TabIndex = 4;
            this.abtLicenseText.Text = "This software uses third party open source libraries. Click this link to view the" +
    " license for these libraries:";
            // 
            // abtLinkLabel
            // 
            this.abtLinkLabel.AutoSize = true;
            this.abtLinkLabel.Location = new System.Drawing.Point(125, 102);
            this.abtLinkLabel.Name = "abtLinkLabel";
            this.abtLinkLabel.Size = new System.Drawing.Size(149, 13);
            this.abtLinkLabel.TabIndex = 3;
            this.abtLinkLabel.TabStop = true;
            this.abtLinkLabel.Text = "Open Source Library Licenses";
            this.abtLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.abtLinkLabel_LinkClicked);
            // 
            // abtCopyright
            // 
            this.abtCopyright.AutoSize = true;
            this.abtCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
            this.abtCopyright.Location = new System.Drawing.Point(125, 37);
            this.abtCopyright.Name = "abtCopyright";
            this.abtCopyright.Size = new System.Drawing.Size(56, 13);
            this.abtCopyright.TabIndex = 2;
            this.abtCopyright.Text = "(copyright)";
            // 
            // abtVersion
            // 
            this.abtVersion.AutoSize = true;
            this.abtVersion.BackColor = System.Drawing.SystemColors.Window;
            this.abtVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abtVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.abtVersion.Location = new System.Drawing.Point(125, 13);
            this.abtVersion.Name = "abtVersion";
            this.abtVersion.Size = new System.Drawing.Size(66, 18);
            this.abtVersion.TabIndex = 1;
            this.abtVersion.Text = "(version)";
            // 
            // abtClose
            // 
            this.abtClose.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.abtClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abtClose.Font = new System.Drawing.Font("Arial", 8F);
            this.abtClose.Location = new System.Drawing.Point(302, 158);
            this.abtClose.Name = "abtClose";
            this.abtClose.Size = new System.Drawing.Size(75, 23);
            this.abtClose.TabIndex = 13;
            this.abtClose.Text = "Close";
            this.abtClose.UseVisualStyleBackColor = false;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(389, 190);
            this.Controls.Add(this.abtLogo);
            this.Controls.Add(this.abtPanel);
            this.Controls.Add(this.abtClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.abtLogo)).EndInit();
            this.abtPanel.ResumeLayout(false);
            this.abtPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox abtLogo;
        private System.Windows.Forms.Label abtVersion;
        private CRPanel abtPanel;
        private System.Windows.Forms.Button abtClose;
        private System.Windows.Forms.Label abtCopyright;
        private System.Windows.Forms.Label abtLicenseText;
        private System.Windows.Forms.LinkLabel abtLinkLabel;
    }
}