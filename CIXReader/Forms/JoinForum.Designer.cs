using TheArtOfDev.HtmlRenderer.WinForms;

namespace CIXReader.Forms
{
    sealed partial class JoinForum
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.forumPanel = new CIXReader.Controls.CRPanel();
            this.statusField = new System.Windows.Forms.Label();
            this.forumLine = new System.Windows.Forms.GroupBox();
            this.forumDescription = new TheArtOfDev.HtmlRenderer.WinForms.HtmlLabel();
            this.forumTitle = new System.Windows.Forms.Label();
            this.forumName = new System.Windows.Forms.Label();
            this.forumImage = new System.Windows.Forms.PictureBox();
            this.forumPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forumImage)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Arial", 8F);
            this.cancelButton.Location = new System.Drawing.Point(385, 235);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // joinButton
            // 
            this.joinButton.AutoSize = true;
            this.joinButton.Font = new System.Drawing.Font("Arial", 8F);
            this.joinButton.Location = new System.Drawing.Point(12, 235);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(108, 24);
            this.joinButton.TabIndex = 2;
            this.joinButton.Text = "&Join This Forum";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // forumPanel
            // 
            this.forumPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forumPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.forumPanel.BackColor = System.Drawing.SystemColors.Window;
            this.forumPanel.BottomBorderWidth = 1;
            this.forumPanel.Controls.Add(this.statusField);
            this.forumPanel.Controls.Add(this.forumLine);
            this.forumPanel.Controls.Add(this.forumDescription);
            this.forumPanel.Controls.Add(this.forumTitle);
            this.forumPanel.Controls.Add(this.forumName);
            this.forumPanel.Controls.Add(this.forumImage);
            this.forumPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.forumPanel.Gradient = false;
            this.forumPanel.LeftBorderWidth = 0;
            this.forumPanel.Location = new System.Drawing.Point(0, 0);
            this.forumPanel.Name = "forumPanel";
            this.forumPanel.RightBorderWidth = 0;
            this.forumPanel.Size = new System.Drawing.Size(480, 237);
            this.forumPanel.TabIndex = 0;
            this.forumPanel.TopBorderWidth = 0;
            this.forumPanel.Resize += new System.EventHandler(this.crPanel1_Resize);
            // 
            // statusField
            // 
            this.statusField.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.statusField.Location = new System.Drawing.Point(110, 97);
            this.statusField.Name = "statusField";
            this.statusField.Size = new System.Drawing.Size(100, 23);
            this.statusField.TabIndex = 5;
            // 
            // forumLine
            // 
            this.forumLine.Location = new System.Drawing.Point(110, 88);
            this.forumLine.Name = "forumLine";
            this.forumLine.Size = new System.Drawing.Size(200, 2);
            this.forumLine.TabIndex = 4;
            this.forumLine.TabStop = false;
            // 
            // forumDescription
            // 
            this.forumDescription.AutoSize = false;
            this.forumDescription.AutoSizeHeightOnly = true;
            this.forumDescription.BackColor = System.Drawing.SystemColors.Window;
            this.forumDescription.BaseStylesheet = null;
            this.forumDescription.Location = new System.Drawing.Point(112, 61);
            this.forumDescription.Name = "forumDescription";
            this.forumDescription.Size = new System.Drawing.Size(0, 0);
            this.forumDescription.TabIndex = 3;
            this.forumDescription.Text = "";
            // 
            // forumTitle
            // 
            this.forumTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forumTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.forumTitle.Location = new System.Drawing.Point(110, 37);
            this.forumTitle.Name = "forumTitle";
            this.forumTitle.Size = new System.Drawing.Size(346, 20);
            this.forumTitle.TabIndex = 2;
            // 
            // forumName
            // 
            this.forumName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forumName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.forumName.Location = new System.Drawing.Point(110, 13);
            this.forumName.Name = "forumName";
            this.forumName.Size = new System.Drawing.Size(351, 20);
            this.forumName.TabIndex = 1;
            this.forumName.Text = "label1";
            // 
            // forumImage
            // 
            this.forumImage.Location = new System.Drawing.Point(14, 13);
            this.forumImage.Name = "forumImage";
            this.forumImage.Size = new System.Drawing.Size(80, 80);
            this.forumImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.forumImage.TabIndex = 0;
            this.forumImage.TabStop = false;
            // 
            // JoinForum
            // 
            this.AcceptButton = this.joinButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(480, 279);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.forumPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JoinForum";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Join Forum";
            this.Load += new System.EventHandler(this.JoinForum_Load);
            this.forumPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.forumImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.CRPanel forumPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.PictureBox forumImage;
        private System.Windows.Forms.Label forumName;
        private System.Windows.Forms.Label forumTitle;
        private HtmlLabel forumDescription;
        private System.Windows.Forms.GroupBox forumLine;
        private System.Windows.Forms.Label statusField;
    }
}