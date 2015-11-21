namespace CIXReader.Forms
{
    sealed partial class SettingsUpdates
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
            this.settingsUpdatesWarning = new System.Windows.Forms.Label();
            this.settingsUpdatesLabel = new System.Windows.Forms.Label();
            this.settingsCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.settingsUseBeta = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // settingsUpdatesWarning
            // 
            this.settingsUpdatesWarning.ForeColor = System.Drawing.Color.DimGray;
            this.settingsUpdatesWarning.Location = new System.Drawing.Point(98, 56);
            this.settingsUpdatesWarning.Name = "settingsUpdatesWarning";
            this.settingsUpdatesWarning.Size = new System.Drawing.Size(235, 79);
            this.settingsUpdatesWarning.TabIndex = 24;
            this.settingsUpdatesWarning.Text = "Beta releases are untested builds that are provided for people who like living at" +
    " the bleeding edge. Don\'t install unless you know what you\'re doing and are will" +
    "ing to test and provide feedback.";
            // 
            // settingsUpdatesLabel
            // 
            this.settingsUpdatesLabel.AutoSize = true;
            this.settingsUpdatesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsUpdatesLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsUpdatesLabel.Location = new System.Drawing.Point(16, 17);
            this.settingsUpdatesLabel.Name = "settingsUpdatesLabel";
            this.settingsUpdatesLabel.Size = new System.Drawing.Size(58, 13);
            this.settingsUpdatesLabel.TabIndex = 23;
            this.settingsUpdatesLabel.Text = "Updates:";
            // 
            // settingsCheckForUpdates
            // 
            this.settingsCheckForUpdates.AutoSize = true;
            this.settingsCheckForUpdates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsCheckForUpdates.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsCheckForUpdates.Location = new System.Drawing.Point(81, 16);
            this.settingsCheckForUpdates.Name = "settingsCheckForUpdates";
            this.settingsCheckForUpdates.Size = new System.Drawing.Size(177, 17);
            this.settingsCheckForUpdates.TabIndex = 21;
            this.settingsCheckForUpdates.Text = "Automatically chec&k for updates";
            this.settingsCheckForUpdates.UseVisualStyleBackColor = true;
            this.settingsCheckForUpdates.CheckedChanged += new System.EventHandler(this.settingsCheckForUpdates_CheckedChanged);
            // 
            // settingsUseBeta
            // 
            this.settingsUseBeta.AutoSize = true;
            this.settingsUseBeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsUseBeta.ForeColor = System.Drawing.SystemColors.ControlText;
            this.settingsUseBeta.Location = new System.Drawing.Point(81, 36);
            this.settingsUseBeta.Name = "settingsUseBeta";
            this.settingsUseBeta.Size = new System.Drawing.Size(232, 17);
            this.settingsUseBeta.TabIndex = 22;
            this.settingsUseBeta.Text = "Include &beta releases in checks for updates";
            this.settingsUseBeta.UseVisualStyleBackColor = true;
            this.settingsUseBeta.CheckedChanged += new System.EventHandler(this.settingsUseBeta_CheckedChanged);
            // 
            // SettingsUpdates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 146);
            this.ControlBox = false;
            this.Controls.Add(this.settingsUpdatesWarning);
            this.Controls.Add(this.settingsUpdatesLabel);
            this.Controls.Add(this.settingsCheckForUpdates);
            this.Controls.Add(this.settingsUseBeta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsUpdates";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SettingsUpdates";
            this.Load += new System.EventHandler(this.SettingsUpdates_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label settingsUpdatesWarning;
        private System.Windows.Forms.Label settingsUpdatesLabel;
        private System.Windows.Forms.CheckBox settingsCheckForUpdates;
        private System.Windows.Forms.CheckBox settingsUseBeta;
    }
}