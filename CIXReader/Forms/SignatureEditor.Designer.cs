namespace CIXReader.Forms
{
    sealed partial class SignatureEditor
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
            this.sigPanel = new CIXReader.Controls.CRPanel();
            this.sigTitleLabel = new System.Windows.Forms.Label();
            this.sigText = new System.Windows.Forms.TextBox();
            this.sigTitle = new System.Windows.Forms.TextBox();
            this.sigEditorCancel = new System.Windows.Forms.Button();
            this.sigEditorSave = new System.Windows.Forms.Button();
            this.sigPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sigPanel
            // 
            this.sigPanel.BackColor = System.Drawing.SystemColors.Window;
            this.sigPanel.BottomBorderWidth = 1;
            this.sigPanel.Controls.Add(this.sigTitleLabel);
            this.sigPanel.Controls.Add(this.sigText);
            this.sigPanel.Controls.Add(this.sigTitle);
            this.sigPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.sigPanel.LeftBorderWidth = 0;
            this.sigPanel.Location = new System.Drawing.Point(0, 0);
            this.sigPanel.Name = "sigPanel";
            this.sigPanel.RightBorderWidth = 0;
            this.sigPanel.Size = new System.Drawing.Size(392, 247);
            this.sigPanel.TabIndex = 0;
            this.sigPanel.TopBorderWidth = 0;
            // 
            // sigTitleLabel
            // 
            this.sigTitleLabel.AutoSize = true;
            this.sigTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sigTitleLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sigTitleLabel.Location = new System.Drawing.Point(15, 17);
            this.sigTitleLabel.Name = "sigTitleLabel";
            this.sigTitleLabel.Size = new System.Drawing.Size(30, 13);
            this.sigTitleLabel.TabIndex = 0;
            this.sigTitleLabel.Text = "&Title:";
            // 
            // sigText
            // 
            this.sigText.AcceptsReturn = true;
            this.sigText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sigText.Location = new System.Drawing.Point(15, 43);
            this.sigText.Multiline = true;
            this.sigText.Name = "sigText";
            this.sigText.Size = new System.Drawing.Size(362, 192);
            this.sigText.TabIndex = 2;
            this.sigText.TextChanged += new System.EventHandler(this.sigText_TextChanged);
            // 
            // sigTitle
            // 
            this.sigTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sigTitle.Location = new System.Drawing.Point(60, 14);
            this.sigTitle.Name = "sigTitle";
            this.sigTitle.Size = new System.Drawing.Size(317, 20);
            this.sigTitle.TabIndex = 1;
            this.sigTitle.TextChanged += new System.EventHandler(this.sigTitle_TextChanged);
            // 
            // sigEditorCancel
            // 
            this.sigEditorCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sigEditorCancel.Font = new System.Drawing.Font("Arial", 8F);
            this.sigEditorCancel.Location = new System.Drawing.Point(305, 256);
            this.sigEditorCancel.Name = "sigEditorCancel";
            this.sigEditorCancel.Size = new System.Drawing.Size(75, 23);
            this.sigEditorCancel.TabIndex = 2;
            this.sigEditorCancel.Text = "Cancel";
            this.sigEditorCancel.UseVisualStyleBackColor = true;
            // 
            // sigEditorSave
            // 
            this.sigEditorSave.Font = new System.Drawing.Font("Arial", 8F);
            this.sigEditorSave.Location = new System.Drawing.Point(224, 256);
            this.sigEditorSave.Name = "sigEditorSave";
            this.sigEditorSave.Size = new System.Drawing.Size(75, 23);
            this.sigEditorSave.TabIndex = 1;
            this.sigEditorSave.Text = "Save";
            this.sigEditorSave.UseVisualStyleBackColor = true;
            this.sigEditorSave.Click += new System.EventHandler(this.sigEditorSave_Click);
            // 
            // SignatureEditor
            // 
            this.AcceptButton = this.sigEditorSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.sigEditorCancel;
            this.ClientSize = new System.Drawing.Size(392, 288);
            this.Controls.Add(this.sigEditorSave);
            this.Controls.Add(this.sigEditorCancel);
            this.Controls.Add(this.sigPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SignatureEditor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Signature Editor";
            this.Load += new System.EventHandler(this.SignatureEditor_Load);
            this.sigPanel.ResumeLayout(false);
            this.sigPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel sigPanel;
        private System.Windows.Forms.TextBox sigText;
        private System.Windows.Forms.TextBox sigTitle;
        private System.Windows.Forms.Label sigTitleLabel;
        private System.Windows.Forms.Button sigEditorCancel;
        private System.Windows.Forms.Button sigEditorSave;
    }
}