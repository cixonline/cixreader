using CIXReader.Controls;

namespace CIXReader.Forms
{
    sealed partial class EULA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EULA));
            this.eulaAccept = new System.Windows.Forms.Button();
            this.eulaReject = new System.Windows.Forms.Button();
            this.eulaPanel = new CIXReader.Controls.CRPanel();
            this.eulaText = new System.Windows.Forms.RichTextBox();
            this.eulaPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // eulaAccept
            // 
            this.eulaAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.eulaAccept.Font = new System.Drawing.Font("Arial", 8F);
            this.eulaAccept.Location = new System.Drawing.Point(354, 483);
            this.eulaAccept.Name = "eulaAccept";
            this.eulaAccept.Size = new System.Drawing.Size(75, 23);
            this.eulaAccept.TabIndex = 1;
            this.eulaAccept.Text = "&Accept";
            this.eulaAccept.UseVisualStyleBackColor = true;
            this.eulaAccept.Click += new System.EventHandler(this.eulaAccept_Click);
            // 
            // eulaReject
            // 
            this.eulaReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.eulaReject.Font = new System.Drawing.Font("Arial", 8F);
            this.eulaReject.Location = new System.Drawing.Point(435, 483);
            this.eulaReject.Name = "eulaReject";
            this.eulaReject.Size = new System.Drawing.Size(75, 23);
            this.eulaReject.TabIndex = 1;
            this.eulaReject.Text = "&Reject";
            this.eulaReject.UseVisualStyleBackColor = true;
            this.eulaReject.Click += new System.EventHandler(this.eulaReject_Click);
            // 
            // eulaPanel
            // 
            this.eulaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eulaPanel.BackColor = System.Drawing.SystemColors.Window;
            this.eulaPanel.BottomBorderWidth = 1;
            this.eulaPanel.Controls.Add(this.eulaText);
            this.eulaPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.eulaPanel.Gradient = false;
            this.eulaPanel.LeftBorderWidth = 0;
            this.eulaPanel.Location = new System.Drawing.Point(0, 0);
            this.eulaPanel.Name = "eulaPanel";
            this.eulaPanel.RightBorderWidth = 0;
            this.eulaPanel.Size = new System.Drawing.Size(522, 474);
            this.eulaPanel.TabIndex = 0;
            this.eulaPanel.TopBorderWidth = 0;
            // 
            // eulaText
            // 
            this.eulaText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eulaText.BackColor = System.Drawing.SystemColors.Window;
            this.eulaText.Location = new System.Drawing.Point(12, 12);
            this.eulaText.Name = "eulaText";
            this.eulaText.ReadOnly = true;
            this.eulaText.Size = new System.Drawing.Size(498, 448);
            this.eulaText.TabIndex = 1;
            this.eulaText.Text = "";
            // 
            // EULA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(522, 515);
            this.Controls.Add(this.eulaReject);
            this.Controls.Add(this.eulaAccept);
            this.Controls.Add(this.eulaPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "EULA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EULA";
            this.Load += new System.EventHandler(this.EULA_Load);
            this.eulaPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CRPanel eulaPanel;
        private System.Windows.Forms.RichTextBox eulaText;
        private System.Windows.Forms.Button eulaAccept;
        private System.Windows.Forms.Button eulaReject;

    }
}