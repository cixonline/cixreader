namespace CIXReader.Forms
{
    sealed partial class SettingsViewing
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
            this.settingsShowToolbar = new System.Windows.Forms.CheckBox();
            this.settingsShowFullDate = new System.Windows.Forms.CheckBox();
            this.settingsSeparator1 = new System.Windows.Forms.GroupBox();
            this.settingsOptionsLabel = new System.Windows.Forms.Label();
            this.settingsShowMenuBar = new System.Windows.Forms.CheckBox();
            this.settingsDontDownloadImages = new System.Windows.Forms.CheckBox();
            this.settingsSpellAsYouType = new System.Windows.Forms.CheckBox();
            this.settingsViewStatusBar = new System.Windows.Forms.CheckBox();
            this.settingsDisableMarkup = new System.Windows.Forms.CheckBox();
            this.settingsTheme = new System.Windows.Forms.ComboBox();
            this.settingsThemeLabel = new System.Windows.Forms.Label();
            this.settingsCustomiseTheme = new System.Windows.Forms.Button();
            this.settingsSeparator2 = new System.Windows.Forms.GroupBox();
            this.settingsEditorLabel = new System.Windows.Forms.Label();
            this.settingsShowCounts = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // settingsShowToolbar
            // 
            this.settingsShowToolbar.AutoSize = true;
            this.settingsShowToolbar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsShowToolbar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsShowToolbar.Location = new System.Drawing.Point(116, 171);
            this.settingsShowToolbar.Name = "settingsShowToolbar";
            this.settingsShowToolbar.Size = new System.Drawing.Size(88, 17);
            this.settingsShowToolbar.TabIndex = 9;
            this.settingsShowToolbar.Text = "Sho&w toolbar";
            this.settingsShowToolbar.UseVisualStyleBackColor = true;
            this.settingsShowToolbar.CheckedChanged += new System.EventHandler(this.settingsShowToolbar_CheckedChanged);
            // 
            // settingsShowFullDate
            // 
            this.settingsShowFullDate.AutoSize = true;
            this.settingsShowFullDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.settingsShowFullDate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsShowFullDate.Location = new System.Drawing.Point(116, 191);
            this.settingsShowFullDate.Name = "settingsShowFullDate";
            this.settingsShowFullDate.Size = new System.Drawing.Size(154, 17);
            this.settingsShowFullDate.TabIndex = 10;
            this.settingsShowFullDate.Text = "Show &full date in messages";
            this.settingsShowFullDate.UseVisualStyleBackColor = true;
            this.settingsShowFullDate.CheckedChanged += new System.EventHandler(this.settingsShowFullDate_CheckedChanged);
            // 
            // settingsSeparator1
            // 
            this.settingsSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsSeparator1.Location = new System.Drawing.Point(4, 74);
            this.settingsSeparator1.Name = "settingsSeparator1";
            this.settingsSeparator1.Size = new System.Drawing.Size(376, 2);
            this.settingsSeparator1.TabIndex = 3;
            this.settingsSeparator1.TabStop = false;
            // 
            // settingsOptionsLabel
            // 
            this.settingsOptionsLabel.AutoSize = true;
            this.settingsOptionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsOptionsLabel.Location = new System.Drawing.Point(53, 92);
            this.settingsOptionsLabel.Name = "settingsOptionsLabel";
            this.settingsOptionsLabel.Size = new System.Drawing.Size(54, 13);
            this.settingsOptionsLabel.TabIndex = 4;
            this.settingsOptionsLabel.Text = "Options:";
            // 
            // settingsShowMenuBar
            // 
            this.settingsShowMenuBar.AutoSize = true;
            this.settingsShowMenuBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsShowMenuBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsShowMenuBar.Location = new System.Drawing.Point(116, 151);
            this.settingsShowMenuBar.Name = "settingsShowMenuBar";
            this.settingsShowMenuBar.Size = new System.Drawing.Size(100, 17);
            this.settingsShowMenuBar.TabIndex = 8;
            this.settingsShowMenuBar.Text = "Show men&u bar";
            this.settingsShowMenuBar.UseVisualStyleBackColor = true;
            this.settingsShowMenuBar.CheckedChanged += new System.EventHandler(this.settingsShowMenuBar_CheckedChanged);
            // 
            // settingsDontDownloadImages
            // 
            this.settingsDontDownloadImages.AutoSize = true;
            this.settingsDontDownloadImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsDontDownloadImages.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsDontDownloadImages.Location = new System.Drawing.Point(116, 91);
            this.settingsDontDownloadImages.Name = "settingsDontDownloadImages";
            this.settingsDontDownloadImages.Size = new System.Drawing.Size(142, 17);
            this.settingsDontDownloadImages.TabIndex = 5;
            this.settingsDontDownloadImages.Text = "&Don\'t show inline images";
            this.settingsDontDownloadImages.UseVisualStyleBackColor = true;
            this.settingsDontDownloadImages.CheckedChanged += new System.EventHandler(this.settingsDontDownloadImages_CheckedChanged);
            // 
            // settingsSpellAsYouType
            // 
            this.settingsSpellAsYouType.AutoSize = true;
            this.settingsSpellAsYouType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsSpellAsYouType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsSpellAsYouType.Location = new System.Drawing.Point(116, 254);
            this.settingsSpellAsYouType.Name = "settingsSpellAsYouType";
            this.settingsSpellAsYouType.Size = new System.Drawing.Size(152, 17);
            this.settingsSpellAsYouType.TabIndex = 14;
            this.settingsSpellAsYouType.Text = "Check spelling as you ty&pe";
            this.settingsSpellAsYouType.UseVisualStyleBackColor = true;
            this.settingsSpellAsYouType.CheckedChanged += new System.EventHandler(this.settingsSpellAsYouType_CheckedChanged);
            // 
            // settingsViewStatusBar
            // 
            this.settingsViewStatusBar.AutoSize = true;
            this.settingsViewStatusBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsViewStatusBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsViewStatusBar.Location = new System.Drawing.Point(116, 131);
            this.settingsViewStatusBar.Name = "settingsViewStatusBar";
            this.settingsViewStatusBar.Size = new System.Drawing.Size(102, 17);
            this.settingsViewStatusBar.TabIndex = 7;
            this.settingsViewStatusBar.Text = "Sho&w status bar";
            this.settingsViewStatusBar.UseVisualStyleBackColor = true;
            this.settingsViewStatusBar.CheckedChanged += new System.EventHandler(this.settingsViewStatusBar_CheckedChanged);
            // 
            // settingsDisableMarkup
            // 
            this.settingsDisableMarkup.AutoSize = true;
            this.settingsDisableMarkup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsDisableMarkup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsDisableMarkup.Location = new System.Drawing.Point(116, 111);
            this.settingsDisableMarkup.Name = "settingsDisableMarkup";
            this.settingsDisableMarkup.Size = new System.Drawing.Size(166, 17);
            this.settingsDisableMarkup.TabIndex = 6;
            this.settingsDisableMarkup.Text = "Displ&ay messages in plain text";
            this.settingsDisableMarkup.UseVisualStyleBackColor = true;
            this.settingsDisableMarkup.CheckedChanged += new System.EventHandler(this.settingsDisableMarkup_CheckedChanged);
            // 
            // settingsTheme
            // 
            this.settingsTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsTheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsTheme.FormattingEnabled = true;
            this.settingsTheme.Location = new System.Drawing.Point(116, 12);
            this.settingsTheme.Name = "settingsTheme";
            this.settingsTheme.Size = new System.Drawing.Size(121, 21);
            this.settingsTheme.Sorted = true;
            this.settingsTheme.TabIndex = 1;
            this.settingsTheme.SelectedIndexChanged += new System.EventHandler(this.settingsTheme_SelectedIndexChanged);
            // 
            // settingsThemeLabel
            // 
            this.settingsThemeLabel.AutoSize = true;
            this.settingsThemeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsThemeLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsThemeLabel.Location = new System.Drawing.Point(59, 15);
            this.settingsThemeLabel.Name = "settingsThemeLabel";
            this.settingsThemeLabel.Size = new System.Drawing.Size(49, 13);
            this.settingsThemeLabel.TabIndex = 0;
            this.settingsThemeLabel.Text = "T&heme:";
            // 
            // settingsCustomiseTheme
            // 
            this.settingsCustomiseTheme.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsCustomiseTheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsCustomiseTheme.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsCustomiseTheme.Location = new System.Drawing.Point(116, 40);
            this.settingsCustomiseTheme.Name = "settingsCustomiseTheme";
            this.settingsCustomiseTheme.Size = new System.Drawing.Size(124, 23);
            this.settingsCustomiseTheme.TabIndex = 2;
            this.settingsCustomiseTheme.Text = "&Custom Theme...";
            this.settingsCustomiseTheme.UseVisualStyleBackColor = false;
            this.settingsCustomiseTheme.Click += new System.EventHandler(this.settingsCustomiseTheme_Click);
            // 
            // settingsSeparator2
            // 
            this.settingsSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsSeparator2.Location = new System.Drawing.Point(0, 239);
            this.settingsSeparator2.Name = "settingsSeparator2";
            this.settingsSeparator2.Size = new System.Drawing.Size(376, 2);
            this.settingsSeparator2.TabIndex = 12;
            this.settingsSeparator2.TabStop = false;
            // 
            // settingsEditorLabel
            // 
            this.settingsEditorLabel.AutoSize = true;
            this.settingsEditorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsEditorLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsEditorLabel.Location = new System.Drawing.Point(63, 255);
            this.settingsEditorLabel.Name = "settingsEditorLabel";
            this.settingsEditorLabel.Size = new System.Drawing.Size(44, 13);
            this.settingsEditorLabel.TabIndex = 13;
            this.settingsEditorLabel.Text = "Editor:";
            // 
            // settingsShowCounts
            // 
            this.settingsShowCounts.AutoSize = true;
            this.settingsShowCounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.settingsShowCounts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsShowCounts.Location = new System.Drawing.Point(116, 211);
            this.settingsShowCounts.Name = "settingsShowCounts";
            this.settingsShowCounts.Size = new System.Drawing.Size(165, 17);
            this.settingsShowCounts.TabIndex = 11;
            this.settingsShowCounts.Text = "Show counts on s&mart folders";
            this.settingsShowCounts.UseVisualStyleBackColor = true;
            this.settingsShowCounts.CheckedChanged += new System.EventHandler(this.settingsShowCounts_CheckedChanged);
            // 
            // SettingsViewing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 297);
            this.ControlBox = false;
            this.Controls.Add(this.settingsShowCounts);
            this.Controls.Add(this.settingsEditorLabel);
            this.Controls.Add(this.settingsShowToolbar);
            this.Controls.Add(this.settingsShowFullDate);
            this.Controls.Add(this.settingsSeparator1);
            this.Controls.Add(this.settingsOptionsLabel);
            this.Controls.Add(this.settingsShowMenuBar);
            this.Controls.Add(this.settingsDontDownloadImages);
            this.Controls.Add(this.settingsSpellAsYouType);
            this.Controls.Add(this.settingsViewStatusBar);
            this.Controls.Add(this.settingsDisableMarkup);
            this.Controls.Add(this.settingsTheme);
            this.Controls.Add(this.settingsThemeLabel);
            this.Controls.Add(this.settingsCustomiseTheme);
            this.Controls.Add(this.settingsSeparator2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsViewing";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SettingsViewing";
            this.Load += new System.EventHandler(this.SettingsViewing_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox settingsShowToolbar;
        private System.Windows.Forms.CheckBox settingsShowFullDate;
        private System.Windows.Forms.GroupBox settingsSeparator1;
        private System.Windows.Forms.Label settingsOptionsLabel;
        private System.Windows.Forms.CheckBox settingsShowMenuBar;
        private System.Windows.Forms.CheckBox settingsDontDownloadImages;
        private System.Windows.Forms.CheckBox settingsSpellAsYouType;
        private System.Windows.Forms.CheckBox settingsViewStatusBar;
        private System.Windows.Forms.CheckBox settingsDisableMarkup;
        private System.Windows.Forms.ComboBox settingsTheme;
        private System.Windows.Forms.Label settingsThemeLabel;
        private System.Windows.Forms.Button settingsCustomiseTheme;
        private System.Windows.Forms.GroupBox settingsSeparator2;
        private System.Windows.Forms.Label settingsEditorLabel;
        private System.Windows.Forms.CheckBox settingsShowCounts;
    }
}