namespace CIXReader.Forms
{
    sealed partial class SettingsRules
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
            this.settingsDeleteRule = new System.Windows.Forms.Button();
            this.settingsEditRule = new System.Windows.Forms.Button();
            this.settingsNewRule = new System.Windows.Forms.Button();
            this.settingsResetRules = new System.Windows.Forms.Button();
            this.settingsRulesList = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // settingsDeleteRule
            // 
            this.settingsDeleteRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsDeleteRule.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsDeleteRule.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsDeleteRule.Location = new System.Drawing.Point(299, 66);
            this.settingsDeleteRule.Name = "settingsDeleteRule";
            this.settingsDeleteRule.Size = new System.Drawing.Size(76, 23);
            this.settingsDeleteRule.TabIndex = 9;
            this.settingsDeleteRule.Text = "De&lete";
            this.settingsDeleteRule.UseVisualStyleBackColor = false;
            this.settingsDeleteRule.Click += new System.EventHandler(this.settingsDeleteRule_Click);
            // 
            // settingsEditRule
            // 
            this.settingsEditRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsEditRule.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsEditRule.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsEditRule.Location = new System.Drawing.Point(299, 39);
            this.settingsEditRule.Name = "settingsEditRule";
            this.settingsEditRule.Size = new System.Drawing.Size(76, 23);
            this.settingsEditRule.TabIndex = 8;
            this.settingsEditRule.Text = "&Edit...";
            this.settingsEditRule.UseVisualStyleBackColor = false;
            this.settingsEditRule.Click += new System.EventHandler(this.settingsEditRule_Click);
            // 
            // settingsNewRule
            // 
            this.settingsNewRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsNewRule.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsNewRule.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsNewRule.Location = new System.Drawing.Point(299, 12);
            this.settingsNewRule.Name = "settingsNewRule";
            this.settingsNewRule.Size = new System.Drawing.Size(76, 23);
            this.settingsNewRule.TabIndex = 7;
            this.settingsNewRule.Text = "&New...";
            this.settingsNewRule.UseVisualStyleBackColor = false;
            this.settingsNewRule.Click += new System.EventHandler(this.settingsNewRule_Click);
            // 
            // settingsResetRules
            // 
            this.settingsResetRules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsResetRules.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsResetRules.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsResetRules.Location = new System.Drawing.Point(299, 93);
            this.settingsResetRules.Name = "settingsResetRules";
            this.settingsResetRules.Size = new System.Drawing.Size(76, 23);
            this.settingsResetRules.TabIndex = 10;
            this.settingsResetRules.Text = "&Reset";
            this.settingsResetRules.UseVisualStyleBackColor = false;
            this.settingsResetRules.Click += new System.EventHandler(this.settingsResetRules_Click);
            // 
            // settingsRulesList
            // 
            this.settingsRulesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsRulesList.FormattingEnabled = true;
            this.settingsRulesList.IntegralHeight = false;
            this.settingsRulesList.Location = new System.Drawing.Point(13, 12);
            this.settingsRulesList.Name = "settingsRulesList";
            this.settingsRulesList.Size = new System.Drawing.Size(280, 289);
            this.settingsRulesList.TabIndex = 11;
            this.settingsRulesList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.settingsRulesList_ItemCheck);
            this.settingsRulesList.SelectedIndexChanged += new System.EventHandler(this.settingsRulesList_SelectedIndexChanged);
            this.settingsRulesList.DoubleClick += new System.EventHandler(this.settingsRules_DoubleClick);
            // 
            // SettingsRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 310);
            this.ControlBox = false;
            this.Controls.Add(this.settingsRulesList);
            this.Controls.Add(this.settingsResetRules);
            this.Controls.Add(this.settingsDeleteRule);
            this.Controls.Add(this.settingsEditRule);
            this.Controls.Add(this.settingsNewRule);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsRules";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SettingsRules";
            this.Load += new System.EventHandler(this.SettingsRules_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button settingsDeleteRule;
        private System.Windows.Forms.Button settingsEditRule;
        private System.Windows.Forms.Button settingsNewRule;
        private System.Windows.Forms.Button settingsResetRules;
        private System.Windows.Forms.CheckedListBox settingsRulesList;
    }
}