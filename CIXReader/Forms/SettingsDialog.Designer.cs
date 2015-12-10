namespace CIXReader.Forms
{
    sealed partial class SettingsDialog
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
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.settingsToolbar = new CIXReader.Controls.CRPanel();
            this.settingsRules = new System.Windows.Forms.Button();
            this.settingsUpdates = new System.Windows.Forms.Button();
            this.settingsSignatures = new System.Windows.Forms.Button();
            this.settingsViewing = new System.Windows.Forms.Button();
            this.settingsGeneral = new System.Windows.Forms.Button();
            this.settingsToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsPanel.Location = new System.Drawing.Point(0, 57);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(427, 324);
            this.settingsPanel.TabIndex = 5;
            // 
            // settingsToolbar
            // 
            this.settingsToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsToolbar.BackColor = System.Drawing.SystemColors.Window;
            this.settingsToolbar.BottomBorderWidth = 1;
            this.settingsToolbar.Controls.Add(this.settingsRules);
            this.settingsToolbar.Controls.Add(this.settingsUpdates);
            this.settingsToolbar.Controls.Add(this.settingsSignatures);
            this.settingsToolbar.Controls.Add(this.settingsViewing);
            this.settingsToolbar.Controls.Add(this.settingsGeneral);
            this.settingsToolbar.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.settingsToolbar.Gradient = false;
            this.settingsToolbar.LeftBorderWidth = 0;
            this.settingsToolbar.Location = new System.Drawing.Point(0, 0);
            this.settingsToolbar.Name = "settingsToolbar";
            this.settingsToolbar.RightBorderWidth = 0;
            this.settingsToolbar.Size = new System.Drawing.Size(427, 57);
            this.settingsToolbar.TabIndex = 4;
            this.settingsToolbar.TopBorderWidth = 0;
            // 
            // settingsRules
            // 
            this.settingsRules.FlatAppearance.BorderSize = 0;
            this.settingsRules.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsRules.Font = new System.Drawing.Font("Arial", 8F);
            this.settingsRules.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsRules.Image = global::CIXReader.Properties.Resources.rulesPreferences;
            this.settingsRules.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.settingsRules.Location = new System.Drawing.Point(202, 0);
            this.settingsRules.Name = "settingsRules";
            this.settingsRules.Size = new System.Drawing.Size(66, 56);
            this.settingsRules.TabIndex = 4;
            this.settingsRules.Text = "Rules";
            this.settingsRules.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.settingsRules.UseVisualStyleBackColor = true;
            this.settingsRules.Click += new System.EventHandler(this.settingsRules_Click);
            // 
            // settingsUpdates
            // 
            this.settingsUpdates.FlatAppearance.BorderSize = 0;
            this.settingsUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsUpdates.Font = new System.Drawing.Font("Arial", 8F);
            this.settingsUpdates.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsUpdates.Image = global::CIXReader.Properties.Resources.UpdatePreferences;
            this.settingsUpdates.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.settingsUpdates.Location = new System.Drawing.Point(274, 0);
            this.settingsUpdates.Name = "settingsUpdates";
            this.settingsUpdates.Size = new System.Drawing.Size(66, 56);
            this.settingsUpdates.TabIndex = 3;
            this.settingsUpdates.Text = "Updates";
            this.settingsUpdates.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.settingsUpdates.UseVisualStyleBackColor = true;
            this.settingsUpdates.Click += new System.EventHandler(this.settingsUpdates_Click);
            // 
            // settingsSignatures
            // 
            this.settingsSignatures.FlatAppearance.BorderSize = 0;
            this.settingsSignatures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsSignatures.Font = new System.Drawing.Font("Arial", 8F);
            this.settingsSignatures.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsSignatures.Image = global::CIXReader.Properties.Resources.SignaturePreferences;
            this.settingsSignatures.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.settingsSignatures.Location = new System.Drawing.Point(132, 0);
            this.settingsSignatures.Name = "settingsSignatures";
            this.settingsSignatures.Size = new System.Drawing.Size(70, 56);
            this.settingsSignatures.TabIndex = 2;
            this.settingsSignatures.Text = "Signatures";
            this.settingsSignatures.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.settingsSignatures.UseVisualStyleBackColor = true;
            this.settingsSignatures.Click += new System.EventHandler(this.settingsSignatures_Click);
            // 
            // settingsViewing
            // 
            this.settingsViewing.FlatAppearance.BorderSize = 0;
            this.settingsViewing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsViewing.Font = new System.Drawing.Font("Arial", 8F);
            this.settingsViewing.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsViewing.Image = global::CIXReader.Properties.Resources.ViewingPrefImage;
            this.settingsViewing.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.settingsViewing.Location = new System.Drawing.Point(66, 0);
            this.settingsViewing.Name = "settingsViewing";
            this.settingsViewing.Size = new System.Drawing.Size(66, 56);
            this.settingsViewing.TabIndex = 1;
            this.settingsViewing.Text = "Viewing";
            this.settingsViewing.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.settingsViewing.UseVisualStyleBackColor = true;
            this.settingsViewing.Click += new System.EventHandler(this.settingsViewing_Click);
            // 
            // settingsGeneral
            // 
            this.settingsGeneral.FlatAppearance.BorderSize = 0;
            this.settingsGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsGeneral.Font = new System.Drawing.Font("Arial", 8F);
            this.settingsGeneral.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsGeneral.Image = global::CIXReader.Properties.Resources.GeneralPrefImage;
            this.settingsGeneral.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.settingsGeneral.Location = new System.Drawing.Point(0, 0);
            this.settingsGeneral.Name = "settingsGeneral";
            this.settingsGeneral.Size = new System.Drawing.Size(66, 56);
            this.settingsGeneral.TabIndex = 0;
            this.settingsGeneral.Text = "General";
            this.settingsGeneral.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.settingsGeneral.UseVisualStyleBackColor = true;
            this.settingsGeneral.Click += new System.EventHandler(this.settingsGeneral_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 382);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.settingsToolbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsDialog_FormClosing);
            this.Load += new System.EventHandler(this.NewSettingsDialog_Load);
            this.Resize += new System.EventHandler(this.NewSettingsDialog_Resize);
            this.settingsToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel settingsPanel;
        private CIXReader.Controls.CRPanel settingsToolbar;
        private System.Windows.Forms.Button settingsSignatures;
        private System.Windows.Forms.Button settingsViewing;
        private System.Windows.Forms.Button settingsGeneral;
        private System.Windows.Forms.Button settingsUpdates;
        private System.Windows.Forms.Button settingsRules;
    }
}