namespace CIXReader.Forms
{
    sealed partial class Diagnostics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Diagnostics));
            this.diagPanel = new CIXReader.Controls.CRPanel();
            this.diagText = new System.Windows.Forms.TextBox();
            this.diagClose = new System.Windows.Forms.Button();
            this.diagCopy = new System.Windows.Forms.Button();
            this.diagPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // diagPanel
            // 
            this.diagPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diagPanel.BackColor = System.Drawing.SystemColors.Window;
            this.diagPanel.BottomBorderWidth = 1;
            this.diagPanel.Controls.Add(this.diagText);
            this.diagPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.diagPanel.Gradient = false;
            this.diagPanel.LeftBorderWidth = 0;
            this.diagPanel.Location = new System.Drawing.Point(0, 0);
            this.diagPanel.Name = "diagPanel";
            this.diagPanel.RightBorderWidth = 0;
            this.diagPanel.Size = new System.Drawing.Size(480, 369);
            this.diagPanel.TabIndex = 0;
            this.diagPanel.TopBorderWidth = 0;
            // 
            // diagText
            // 
            this.diagText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diagText.BackColor = System.Drawing.SystemColors.Window;
            this.diagText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.diagText.Location = new System.Drawing.Point(13, 13);
            this.diagText.Multiline = true;
            this.diagText.Name = "diagText";
            this.diagText.ReadOnly = true;
            this.diagText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.diagText.Size = new System.Drawing.Size(457, 341);
            this.diagText.TabIndex = 0;
            // 
            // diagClose
            // 
            this.diagClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.diagClose.Font = new System.Drawing.Font("Arial", 8F);
            this.diagClose.Location = new System.Drawing.Point(393, 378);
            this.diagClose.Name = "diagClose";
            this.diagClose.Size = new System.Drawing.Size(75, 23);
            this.diagClose.TabIndex = 1;
            this.diagClose.Text = "&Close";
            this.diagClose.UseVisualStyleBackColor = true;
            this.diagClose.Click += new System.EventHandler(this.diagClose_Click);
            // 
            // diagCopy
            // 
            this.diagCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.diagCopy.Font = new System.Drawing.Font("Arial", 8F);
            this.diagCopy.Location = new System.Drawing.Point(312, 378);
            this.diagCopy.Name = "diagCopy";
            this.diagCopy.Size = new System.Drawing.Size(75, 23);
            this.diagCopy.TabIndex = 1;
            this.diagCopy.Text = "Cop&y";
            this.diagCopy.UseVisualStyleBackColor = true;
            this.diagCopy.Click += new System.EventHandler(this.diagCopy_Click);
            // 
            // Diagnostics
            // 
            this.AcceptButton = this.diagClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(480, 410);
            this.Controls.Add(this.diagCopy);
            this.Controls.Add(this.diagClose);
            this.Controls.Add(this.diagPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "Diagnostics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Diagnostics";
            this.Load += new System.EventHandler(this.Diagnostics_Load);
            this.diagPanel.ResumeLayout(false);
            this.diagPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel diagPanel;
        private System.Windows.Forms.Button diagClose;
        private System.Windows.Forms.Button diagCopy;
        private System.Windows.Forms.TextBox diagText;
    }
}