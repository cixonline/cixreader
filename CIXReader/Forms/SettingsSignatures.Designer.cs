namespace CIXReader.Forms
{
    sealed partial class SettingsSignatures
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
            this.settingsDefaultSignature = new System.Windows.Forms.ComboBox();
            this.settingsDefaultSignatureLabel = new System.Windows.Forms.Label();
            this.settingsDeleteSignature = new System.Windows.Forms.Button();
            this.settingsEditSignature = new System.Windows.Forms.Button();
            this.settingsNewSignature = new System.Windows.Forms.Button();
            this.settingsSignaturesList = new System.Windows.Forms.ListView();
            this.settingsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // settingsDefaultSignature
            // 
            this.settingsDefaultSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settingsDefaultSignature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsDefaultSignature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsDefaultSignature.FormattingEnabled = true;
            this.settingsDefaultSignature.Location = new System.Drawing.Point(110, 272);
            this.settingsDefaultSignature.Name = "settingsDefaultSignature";
            this.settingsDefaultSignature.Size = new System.Drawing.Size(183, 21);
            this.settingsDefaultSignature.TabIndex = 11;
            this.settingsDefaultSignature.SelectedIndexChanged += new System.EventHandler(this.settingsDefaultSignature_SelectedIndexChanged);
            // 
            // settingsDefaultSignatureLabel
            // 
            this.settingsDefaultSignatureLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settingsDefaultSignatureLabel.AutoSize = true;
            this.settingsDefaultSignatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsDefaultSignatureLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsDefaultSignatureLabel.Location = new System.Drawing.Point(12, 275);
            this.settingsDefaultSignatureLabel.Name = "settingsDefaultSignatureLabel";
            this.settingsDefaultSignatureLabel.Size = new System.Drawing.Size(92, 13);
            this.settingsDefaultSignatureLabel.TabIndex = 10;
            this.settingsDefaultSignatureLabel.Text = "&Default Signature:";
            // 
            // settingsDeleteSignature
            // 
            this.settingsDeleteSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsDeleteSignature.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsDeleteSignature.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsDeleteSignature.Location = new System.Drawing.Point(299, 66);
            this.settingsDeleteSignature.Name = "settingsDeleteSignature";
            this.settingsDeleteSignature.Size = new System.Drawing.Size(76, 23);
            this.settingsDeleteSignature.TabIndex = 9;
            this.settingsDeleteSignature.Text = "De&lete";
            this.settingsDeleteSignature.UseVisualStyleBackColor = false;
            this.settingsDeleteSignature.Click += new System.EventHandler(this.settingsDeleteSignature_Click);
            // 
            // settingsEditSignature
            // 
            this.settingsEditSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsEditSignature.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsEditSignature.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsEditSignature.Location = new System.Drawing.Point(299, 39);
            this.settingsEditSignature.Name = "settingsEditSignature";
            this.settingsEditSignature.Size = new System.Drawing.Size(76, 23);
            this.settingsEditSignature.TabIndex = 8;
            this.settingsEditSignature.Text = "&Edit...";
            this.settingsEditSignature.UseVisualStyleBackColor = false;
            this.settingsEditSignature.Click += new System.EventHandler(this.settingsEditSignature_Click);
            // 
            // settingsNewSignature
            // 
            this.settingsNewSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsNewSignature.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsNewSignature.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsNewSignature.Location = new System.Drawing.Point(299, 12);
            this.settingsNewSignature.Name = "settingsNewSignature";
            this.settingsNewSignature.Size = new System.Drawing.Size(76, 23);
            this.settingsNewSignature.TabIndex = 7;
            this.settingsNewSignature.Text = "&New...";
            this.settingsNewSignature.UseVisualStyleBackColor = false;
            this.settingsNewSignature.Click += new System.EventHandler(this.settingsNewSignature_Click);
            // 
            // settingsSignaturesList
            // 
            this.settingsSignaturesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsSignaturesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.settingsColumnHeader});
            this.settingsSignaturesList.FullRowSelect = true;
            this.settingsSignaturesList.Location = new System.Drawing.Point(12, 12);
            this.settingsSignaturesList.MultiSelect = false;
            this.settingsSignaturesList.Name = "settingsSignaturesList";
            this.settingsSignaturesList.ShowGroups = false;
            this.settingsSignaturesList.Size = new System.Drawing.Size(281, 251);
            this.settingsSignaturesList.TabIndex = 6;
            this.settingsSignaturesList.UseCompatibleStateImageBehavior = false;
            this.settingsSignaturesList.View = System.Windows.Forms.View.Details;
            this.settingsSignaturesList.SelectedIndexChanged += new System.EventHandler(this.settingsSignatures_SelectedIndexChanged);
            this.settingsSignaturesList.DoubleClick += new System.EventHandler(this.settingsSignatures_DoubleClick);
            // 
            // settingsColumnHeader
            // 
            this.settingsColumnHeader.Text = "Title";
            this.settingsColumnHeader.Width = 276;
            // 
            // SettingsSignatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 310);
            this.ControlBox = false;
            this.Controls.Add(this.settingsDefaultSignature);
            this.Controls.Add(this.settingsDefaultSignatureLabel);
            this.Controls.Add(this.settingsDeleteSignature);
            this.Controls.Add(this.settingsEditSignature);
            this.Controls.Add(this.settingsNewSignature);
            this.Controls.Add(this.settingsSignaturesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsSignatures";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SettingsSignatures";
            this.Load += new System.EventHandler(this.SettingsSignatures_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox settingsDefaultSignature;
        private System.Windows.Forms.Label settingsDefaultSignatureLabel;
        private System.Windows.Forms.Button settingsDeleteSignature;
        private System.Windows.Forms.Button settingsEditSignature;
        private System.Windows.Forms.Button settingsNewSignature;
        private System.Windows.Forms.ListView settingsSignaturesList;
        private System.Windows.Forms.ColumnHeader settingsColumnHeader;
    }
}