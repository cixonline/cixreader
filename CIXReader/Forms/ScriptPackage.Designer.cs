namespace CIXReader.Forms
{
    sealed partial class ScriptPackage
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
            this.instPanel = new CIXReader.Controls.CRPanel();
            this.instToolbar = new System.Windows.Forms.Label();
            this.instAuthor = new System.Windows.Forms.Label();
            this.instDescription = new System.Windows.Forms.Label();
            this.instName = new System.Windows.Forms.Label();
            this.instToolbarLabel = new System.Windows.Forms.Label();
            this.instAuthorLabel = new System.Windows.Forms.Label();
            this.instDescriptionLabel = new System.Windows.Forms.Label();
            this.instNameLabel = new System.Windows.Forms.Label();
            this.instIntro = new System.Windows.Forms.Label();
            this.instIcon = new System.Windows.Forms.PictureBox();
            this.instOK = new System.Windows.Forms.Button();
            this.instCancel = new System.Windows.Forms.Button();
            this.instPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // instPanel
            // 
            this.instPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instPanel.BackColor = System.Drawing.SystemColors.Window;
            this.instPanel.BottomBorderWidth = 1;
            this.instPanel.Controls.Add(this.instToolbar);
            this.instPanel.Controls.Add(this.instAuthor);
            this.instPanel.Controls.Add(this.instDescription);
            this.instPanel.Controls.Add(this.instName);
            this.instPanel.Controls.Add(this.instToolbarLabel);
            this.instPanel.Controls.Add(this.instAuthorLabel);
            this.instPanel.Controls.Add(this.instDescriptionLabel);
            this.instPanel.Controls.Add(this.instNameLabel);
            this.instPanel.Controls.Add(this.instIntro);
            this.instPanel.Controls.Add(this.instIcon);
            this.instPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.instPanel.LeftBorderWidth = 0;
            this.instPanel.Location = new System.Drawing.Point(0, 0);
            this.instPanel.Name = "instPanel";
            this.instPanel.RightBorderWidth = 0;
            this.instPanel.Size = new System.Drawing.Size(431, 215);
            this.instPanel.TabIndex = 0;
            this.instPanel.TopBorderWidth = 0;
            // 
            // instToolbar
            // 
            this.instToolbar.AutoSize = true;
            this.instToolbar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instToolbar.Location = new System.Drawing.Point(182, 171);
            this.instToolbar.Name = "instToolbar";
            this.instToolbar.Size = new System.Drawing.Size(23, 13);
            this.instToolbar.TabIndex = 3;
            this.instToolbar.Text = "NO";
            // 
            // instAuthor
            // 
            this.instAuthor.AutoSize = true;
            this.instAuthor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instAuthor.Location = new System.Drawing.Point(182, 141);
            this.instAuthor.Name = "instAuthor";
            this.instAuthor.Size = new System.Drawing.Size(54, 13);
            this.instAuthor.TabIndex = 3;
            this.instAuthor.Text = "No author";
            // 
            // instDescription
            // 
            this.instDescription.AutoSize = true;
            this.instDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instDescription.Location = new System.Drawing.Point(182, 111);
            this.instDescription.Name = "instDescription";
            this.instDescription.Size = new System.Drawing.Size(75, 13);
            this.instDescription.TabIndex = 3;
            this.instDescription.Text = "No description";
            // 
            // instName
            // 
            this.instName.AutoSize = true;
            this.instName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instName.Location = new System.Drawing.Point(182, 81);
            this.instName.Name = "instName";
            this.instName.Size = new System.Drawing.Size(41, 13);
            this.instName.TabIndex = 3;
            this.instName.Text = "(Name)";
            // 
            // instToolbarLabel
            // 
            this.instToolbarLabel.AutoSize = true;
            this.instToolbarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instToolbarLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instToolbarLabel.Location = new System.Drawing.Point(87, 171);
            this.instToolbarLabel.Name = "instToolbarLabel";
            this.instToolbarLabel.Size = new System.Drawing.Size(91, 13);
            this.instToolbarLabel.TabIndex = 2;
            this.instToolbarLabel.Text = "Add to toolbar:";
            // 
            // instAuthorLabel
            // 
            this.instAuthorLabel.AutoSize = true;
            this.instAuthorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instAuthorLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instAuthorLabel.Location = new System.Drawing.Point(87, 141);
            this.instAuthorLabel.Name = "instAuthorLabel";
            this.instAuthorLabel.Size = new System.Drawing.Size(48, 13);
            this.instAuthorLabel.TabIndex = 2;
            this.instAuthorLabel.Text = "Author:";
            // 
            // instDescriptionLabel
            // 
            this.instDescriptionLabel.AutoSize = true;
            this.instDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instDescriptionLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instDescriptionLabel.Location = new System.Drawing.Point(87, 111);
            this.instDescriptionLabel.Name = "instDescriptionLabel";
            this.instDescriptionLabel.Size = new System.Drawing.Size(75, 13);
            this.instDescriptionLabel.TabIndex = 2;
            this.instDescriptionLabel.Text = "Description:";
            // 
            // instNameLabel
            // 
            this.instNameLabel.AutoSize = true;
            this.instNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instNameLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instNameLabel.Location = new System.Drawing.Point(87, 81);
            this.instNameLabel.Name = "instNameLabel";
            this.instNameLabel.Size = new System.Drawing.Size(43, 13);
            this.instNameLabel.TabIndex = 2;
            this.instNameLabel.Text = "Name:";
            // 
            // instIntro
            // 
            this.instIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instIntro.ForeColor = System.Drawing.SystemColors.ControlText;
            this.instIntro.Location = new System.Drawing.Point(84, 13);
            this.instIntro.Name = "instIntro";
            this.instIntro.Size = new System.Drawing.Size(336, 50);
            this.instIntro.TabIndex = 1;
            this.instIntro.Text = "You are about to install a script package. Review the details below before clicki" +
    "ng Install, or click Cancel to cancel installation.";
            // 
            // instIcon
            // 
            this.instIcon.Image = global::CIXReader.Properties.Resources.Script;
            this.instIcon.Location = new System.Drawing.Point(13, 13);
            this.instIcon.Name = "instIcon";
            this.instIcon.Size = new System.Drawing.Size(50, 50);
            this.instIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.instIcon.TabIndex = 0;
            this.instIcon.TabStop = false;
            // 
            // instOK
            // 
            this.instOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.instOK.Location = new System.Drawing.Point(265, 224);
            this.instOK.Name = "instOK";
            this.instOK.Size = new System.Drawing.Size(75, 23);
            this.instOK.TabIndex = 1;
            this.instOK.Text = "&Install";
            this.instOK.UseVisualStyleBackColor = true;
            this.instOK.Click += new System.EventHandler(this.instOK_Click);
            // 
            // instCancel
            // 
            this.instCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.instCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.instCancel.Location = new System.Drawing.Point(346, 224);
            this.instCancel.Name = "instCancel";
            this.instCancel.Size = new System.Drawing.Size(75, 23);
            this.instCancel.TabIndex = 1;
            this.instCancel.Text = "Cancel";
            this.instCancel.UseVisualStyleBackColor = true;
            this.instCancel.Click += new System.EventHandler(this.instCancel_Click);
            // 
            // ScriptPackage
            // 
            this.AcceptButton = this.instOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CancelButton = this.instCancel;
            this.ClientSize = new System.Drawing.Size(432, 256);
            this.Controls.Add(this.instCancel);
            this.Controls.Add(this.instOK);
            this.Controls.Add(this.instPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScriptPackage";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Install Script Package";
            this.Load += new System.EventHandler(this.InstallScriptPackage_Load);
            this.instPanel.ResumeLayout(false);
            this.instPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel instPanel;
        private System.Windows.Forms.Button instOK;
        private System.Windows.Forms.Button instCancel;
        private System.Windows.Forms.Label instToolbarLabel;
        private System.Windows.Forms.Label instAuthorLabel;
        private System.Windows.Forms.Label instDescriptionLabel;
        private System.Windows.Forms.Label instNameLabel;
        private System.Windows.Forms.Label instIntro;
        private System.Windows.Forms.PictureBox instIcon;
        private System.Windows.Forms.Label instToolbar;
        private System.Windows.Forms.Label instAuthor;
        private System.Windows.Forms.Label instDescription;
        private System.Windows.Forms.Label instName;
    }
}