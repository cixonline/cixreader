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
            this.settingsRulesList = new System.Windows.Forms.ListView();
            this.settingsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            // settingsRulesList
            // 
            this.settingsRulesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsRulesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.settingsColumnHeader});
            this.settingsRulesList.FullRowSelect = true;
            this.settingsRulesList.Location = new System.Drawing.Point(12, 12);
            this.settingsRulesList.MultiSelect = false;
            this.settingsRulesList.Name = "settingsRulesList";
            this.settingsRulesList.ShowGroups = false;
            this.settingsRulesList.Size = new System.Drawing.Size(281, 251);
            this.settingsRulesList.TabIndex = 6;
            this.settingsRulesList.UseCompatibleStateImageBehavior = false;
            this.settingsRulesList.View = System.Windows.Forms.View.Details;
            this.settingsRulesList.SelectedIndexChanged += new System.EventHandler(this.settingsRules_SelectedIndexChanged);
            this.settingsRulesList.DoubleClick += new System.EventHandler(this.settingsRules_DoubleClick);
            // 
            // settingsColumnHeader
            // 
            this.settingsColumnHeader.Text = "Title";
            this.settingsColumnHeader.Width = 276;
            // 
            // SettingsRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 310);
            this.ControlBox = false;
            this.Controls.Add(this.settingsDeleteRule);
            this.Controls.Add(this.settingsEditRule);
            this.Controls.Add(this.settingsNewRule);
            this.Controls.Add(this.settingsRulesList);
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button settingsDeleteRule;
        private System.Windows.Forms.Button settingsEditRule;
        private System.Windows.Forms.Button settingsNewRule;
        private System.Windows.Forms.ListView settingsRulesList;
        private System.Windows.Forms.ColumnHeader settingsColumnHeader;
    }
}