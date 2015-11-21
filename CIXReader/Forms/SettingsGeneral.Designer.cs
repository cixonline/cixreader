namespace CIXReader.Forms
{
    sealed partial class SettingsGeneral
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
            this.settingsSearchEngines = new System.Windows.Forms.ComboBox();
            this.settingsSearchLabel = new System.Windows.Forms.Label();
            this.settingsSeparator2 = new System.Windows.Forms.GroupBox();
            this.settingsSeparator1 = new System.Windows.Forms.GroupBox();
            this.settingsArchiveLogs = new System.Windows.Forms.CheckBox();
            this.settingsDebugLog = new System.Windows.Forms.CheckBox();
            this.settingsOpenLogFile = new System.Windows.Forms.Button();
            this.settingsLoggingLabel = new System.Windows.Forms.Label();
            this.settingsStartupLabel = new System.Windows.Forms.Label();
            this.settingsStartOffline = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.settingsCleanUpCacheList = new System.Windows.Forms.ComboBox();
            this.settingsStartInHomePage = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // settingsSearchEngines
            // 
            this.settingsSearchEngines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsSearchEngines.FormattingEnabled = true;
            this.settingsSearchEngines.Location = new System.Drawing.Point(117, 192);
            this.settingsSearchEngines.Name = "settingsSearchEngines";
            this.settingsSearchEngines.Size = new System.Drawing.Size(145, 21);
            this.settingsSearchEngines.Sorted = true;
            this.settingsSearchEngines.TabIndex = 12;
            this.settingsSearchEngines.SelectedIndexChanged += new System.EventHandler(this.settingsSearchEngines_SelectedIndexChanged);
            // 
            // settingsSearchLabel
            // 
            this.settingsSearchLabel.AutoSize = true;
            this.settingsSearchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsSearchLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsSearchLabel.Location = new System.Drawing.Point(55, 195);
            this.settingsSearchLabel.Name = "settingsSearchLabel";
            this.settingsSearchLabel.Size = new System.Drawing.Size(51, 13);
            this.settingsSearchLabel.TabIndex = 11;
            this.settingsSearchLabel.Text = "Searc&h:";
            // 
            // settingsSeparator2
            // 
            this.settingsSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsSeparator2.Location = new System.Drawing.Point(5, 179);
            this.settingsSeparator2.Name = "settingsSeparator2";
            this.settingsSeparator2.Size = new System.Drawing.Size(372, 2);
            this.settingsSeparator2.TabIndex = 10;
            this.settingsSeparator2.TabStop = false;
            // 
            // settingsSeparator1
            // 
            this.settingsSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsSeparator1.Location = new System.Drawing.Point(5, 90);
            this.settingsSeparator1.Name = "settingsSeparator1";
            this.settingsSeparator1.Size = new System.Drawing.Size(372, 2);
            this.settingsSeparator1.TabIndex = 5;
            this.settingsSeparator1.TabStop = false;
            // 
            // settingsArchiveLogs
            // 
            this.settingsArchiveLogs.AutoSize = true;
            this.settingsArchiveLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsArchiveLogs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsArchiveLogs.Location = new System.Drawing.Point(117, 126);
            this.settingsArchiveLogs.Name = "settingsArchiveLogs";
            this.settingsArchiveLogs.Size = new System.Drawing.Size(134, 17);
            this.settingsArchiveLogs.TabIndex = 8;
            this.settingsArchiveLogs.Text = "Archive o&ld debug logs";
            this.settingsArchiveLogs.UseVisualStyleBackColor = true;
            this.settingsArchiveLogs.CheckedChanged += new System.EventHandler(this.settingsArchiveLogs_CheckedChanged);
            // 
            // settingsDebugLog
            // 
            this.settingsDebugLog.AutoSize = true;
            this.settingsDebugLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsDebugLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsDebugLog.Location = new System.Drawing.Point(117, 106);
            this.settingsDebugLog.Name = "settingsDebugLog";
            this.settingsDebugLog.Size = new System.Drawing.Size(145, 17);
            this.settingsDebugLog.TabIndex = 7;
            this.settingsDebugLog.Text = "Create &session debug log";
            this.settingsDebugLog.UseVisualStyleBackColor = true;
            this.settingsDebugLog.CheckedChanged += new System.EventHandler(this.settingsDebugLog_CheckedChanged);
            // 
            // settingsOpenLogFile
            // 
            this.settingsOpenLogFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsOpenLogFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsOpenLogFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsOpenLogFile.Location = new System.Drawing.Point(130, 150);
            this.settingsOpenLogFile.Name = "settingsOpenLogFile";
            this.settingsOpenLogFile.Size = new System.Drawing.Size(106, 23);
            this.settingsOpenLogFile.TabIndex = 9;
            this.settingsOpenLogFile.Text = "&Open Log File";
            this.settingsOpenLogFile.UseVisualStyleBackColor = false;
            this.settingsOpenLogFile.Click += new System.EventHandler(this.settingsOpenLogFile_Click);
            // 
            // settingsLoggingLabel
            // 
            this.settingsLoggingLabel.AutoSize = true;
            this.settingsLoggingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsLoggingLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsLoggingLabel.Location = new System.Drawing.Point(55, 107);
            this.settingsLoggingLabel.Name = "settingsLoggingLabel";
            this.settingsLoggingLabel.Size = new System.Drawing.Size(56, 13);
            this.settingsLoggingLabel.TabIndex = 6;
            this.settingsLoggingLabel.Text = "Logging:";
            // 
            // settingsStartupLabel
            // 
            this.settingsStartupLabel.AutoSize = true;
            this.settingsStartupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsStartupLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsStartupLabel.Location = new System.Drawing.Point(59, 13);
            this.settingsStartupLabel.Name = "settingsStartupLabel";
            this.settingsStartupLabel.Size = new System.Drawing.Size(52, 13);
            this.settingsStartupLabel.TabIndex = 0;
            this.settingsStartupLabel.Text = "Startup:";
            // 
            // settingsStartOffline
            // 
            this.settingsStartOffline.AutoSize = true;
            this.settingsStartOffline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsStartOffline.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsStartOffline.Location = new System.Drawing.Point(117, 12);
            this.settingsStartOffline.Name = "settingsStartOffline";
            this.settingsStartOffline.Size = new System.Drawing.Size(119, 17);
            this.settingsStartOffline.TabIndex = 1;
            this.settingsStartOffline.Text = "S&tart in offline mode";
            this.settingsStartOffline.UseVisualStyleBackColor = true;
            this.settingsStartOffline.CheckedChanged += new System.EventHandler(this.settingsStartOffline_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "&Clean up cache:";
            // 
            // settingsCleanUpCacheList
            // 
            this.settingsCleanUpCacheList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsCleanUpCacheList.FormattingEnabled = true;
            this.settingsCleanUpCacheList.Location = new System.Drawing.Point(209, 54);
            this.settingsCleanUpCacheList.Name = "settingsCleanUpCacheList";
            this.settingsCleanUpCacheList.Size = new System.Drawing.Size(121, 21);
            this.settingsCleanUpCacheList.TabIndex = 4;
            this.settingsCleanUpCacheList.SelectedIndexChanged += new System.EventHandler(this.settingsCleanUpCacheList_SelectedIndexChanged);
            // 
            // settingsStartInHomePage
            // 
            this.settingsStartInHomePage.AutoSize = true;
            this.settingsStartInHomePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsStartInHomePage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsStartInHomePage.Location = new System.Drawing.Point(117, 32);
            this.settingsStartInHomePage.Name = "settingsStartInHomePage";
            this.settingsStartInHomePage.Size = new System.Drawing.Size(115, 17);
            this.settingsStartInHomePage.TabIndex = 2;
            this.settingsStartInHomePage.Text = "Start in home &page";
            this.settingsStartInHomePage.UseVisualStyleBackColor = true;
            this.settingsStartInHomePage.CheckedChanged += new System.EventHandler(this.settingsStartInHomePage_CheckedChanged);
            // 
            // SettingsGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 235);
            this.ControlBox = false;
            this.Controls.Add(this.settingsStartInHomePage);
            this.Controls.Add(this.settingsCleanUpCacheList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.settingsSearchEngines);
            this.Controls.Add(this.settingsSearchLabel);
            this.Controls.Add(this.settingsSeparator2);
            this.Controls.Add(this.settingsSeparator1);
            this.Controls.Add(this.settingsArchiveLogs);
            this.Controls.Add(this.settingsDebugLog);
            this.Controls.Add(this.settingsOpenLogFile);
            this.Controls.Add(this.settingsLoggingLabel);
            this.Controls.Add(this.settingsStartupLabel);
            this.Controls.Add(this.settingsStartOffline);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsGeneral";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SettingsGeneral";
            this.Load += new System.EventHandler(this.SettingsGeneral_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox settingsSearchEngines;
        private System.Windows.Forms.Label settingsSearchLabel;
        private System.Windows.Forms.GroupBox settingsSeparator2;
        private System.Windows.Forms.GroupBox settingsSeparator1;
        private System.Windows.Forms.CheckBox settingsArchiveLogs;
        private System.Windows.Forms.CheckBox settingsDebugLog;
        private System.Windows.Forms.Button settingsOpenLogFile;
        private System.Windows.Forms.Label settingsLoggingLabel;
        private System.Windows.Forms.Label settingsStartupLabel;
        private System.Windows.Forms.CheckBox settingsStartOffline;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox settingsCleanUpCacheList;
        private System.Windows.Forms.CheckBox settingsStartInHomePage;
    }
}