namespace CIXReader.Forms
{
    sealed partial class JoinForumInput
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
            this.inputCancelButton = new System.Windows.Forms.Button();
            this.inputJoinButton = new System.Windows.Forms.Button();
            this.inputPanel = new CIXReader.Controls.CRPanel();
            this.inputText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.inputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputCancelButton
            // 
            this.inputCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.inputCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.inputCancelButton.Font = new System.Drawing.Font("Arial", 8F);
            this.inputCancelButton.Location = new System.Drawing.Point(221, 78);
            this.inputCancelButton.Name = "inputCancelButton";
            this.inputCancelButton.Size = new System.Drawing.Size(75, 23);
            this.inputCancelButton.TabIndex = 2;
            this.inputCancelButton.Text = "Cancel";
            this.inputCancelButton.UseVisualStyleBackColor = true;
            this.inputCancelButton.Click += new System.EventHandler(this.inputCancelButton_Click);
            // 
            // inputJoinButton
            // 
            this.inputJoinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.inputJoinButton.Font = new System.Drawing.Font("Arial", 8F);
            this.inputJoinButton.Location = new System.Drawing.Point(140, 78);
            this.inputJoinButton.Name = "inputJoinButton";
            this.inputJoinButton.Size = new System.Drawing.Size(75, 23);
            this.inputJoinButton.TabIndex = 1;
            this.inputJoinButton.Text = "&Join";
            this.inputJoinButton.UseVisualStyleBackColor = true;
            this.inputJoinButton.Click += new System.EventHandler(this.inputJoinButton_Click);
            // 
            // inputPanel
            // 
            this.inputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputPanel.BackColor = System.Drawing.SystemColors.Window;
            this.inputPanel.BottomBorderWidth = 1;
            this.inputPanel.Controls.Add(this.inputText);
            this.inputPanel.Controls.Add(this.label1);
            this.inputPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.inputPanel.Gradient = false;
            this.inputPanel.LeftBorderWidth = 0;
            this.inputPanel.Location = new System.Drawing.Point(0, 0);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.RightBorderWidth = 0;
            this.inputPanel.Size = new System.Drawing.Size(308, 69);
            this.inputPanel.TabIndex = 0;
            this.inputPanel.TopBorderWidth = 0;
            // 
            // inputText
            // 
            this.inputText.Location = new System.Drawing.Point(13, 31);
            this.inputText.Name = "inputText";
            this.inputText.Size = new System.Drawing.Size(281, 20);
            this.inputText.TabIndex = 1;
            this.inputText.TextChanged += new System.EventHandler(this.inputText_TextChanged);
            this.inputText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputText_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Enter the name of the forum to join:";
            // 
            // JoinForumInput
            // 
            this.AcceptButton = this.inputJoinButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.inputCancelButton;
            this.ClientSize = new System.Drawing.Size(308, 110);
            this.Controls.Add(this.inputJoinButton);
            this.Controls.Add(this.inputCancelButton);
            this.Controls.Add(this.inputPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JoinForumInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Join Forum";
            this.Load += new System.EventHandler(this.JoinForumInput_Load);
            this.inputPanel.ResumeLayout(false);
            this.inputPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel inputPanel;
        private System.Windows.Forms.TextBox inputText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button inputCancelButton;
        private System.Windows.Forms.Button inputJoinButton;
    }
}