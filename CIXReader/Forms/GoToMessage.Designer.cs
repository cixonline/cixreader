using System.Windows.Forms;

namespace CIXReader.Forms
{
    sealed partial class GoToMessage
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
            this.gotoPanel = new CIXReader.Controls.CRPanel();
            this.gotoInput = new System.Windows.Forms.TextBox();
            this.gotoLabel = new System.Windows.Forms.Label();
            this.gotoOK = new System.Windows.Forms.Button();
            this.gotoCancel = new System.Windows.Forms.Button();
            this.gotoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gotoPanel
            // 
            this.gotoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gotoPanel.BackColor = System.Drawing.SystemColors.Window;
            this.gotoPanel.BottomBorderWidth = 1;
            this.gotoPanel.Controls.Add(this.gotoInput);
            this.gotoPanel.Controls.Add(this.gotoLabel);
            this.gotoPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.gotoPanel.Gradient = false;
            this.gotoPanel.LeftBorderWidth = 0;
            this.gotoPanel.Location = new System.Drawing.Point(0, 0);
            this.gotoPanel.Name = "gotoPanel";
            this.gotoPanel.RightBorderWidth = 0;
            this.gotoPanel.Size = new System.Drawing.Size(290, 60);
            this.gotoPanel.TabIndex = 0;
            this.gotoPanel.TopBorderWidth = 0;
            // 
            // gotoInput
            // 
            this.gotoInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gotoInput.Location = new System.Drawing.Point(155, 19);
            this.gotoInput.MaxLength = 5;
            this.gotoInput.Name = "gotoInput";
            this.gotoInput.Size = new System.Drawing.Size(100, 20);
            this.gotoInput.TabIndex = 1;
            this.gotoInput.TextChanged += new System.EventHandler(this.gotoInput_TextChanged);
            this.gotoInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gotoInput_KeyPress);
            // 
            // gotoLabel
            // 
            this.gotoLabel.AutoSize = true;
            this.gotoLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gotoLabel.Location = new System.Drawing.Point(30, 22);
            this.gotoLabel.Name = "gotoLabel";
            this.gotoLabel.Size = new System.Drawing.Size(119, 13);
            this.gotoLabel.TabIndex = 0;
            this.gotoLabel.Text = "&Go to message number:";
            // 
            // gotoOK
            // 
            this.gotoOK.Font = new System.Drawing.Font("Arial", 8F);
            this.gotoOK.Location = new System.Drawing.Point(119, 69);
            this.gotoOK.Name = "gotoOK";
            this.gotoOK.Size = new System.Drawing.Size(75, 23);
            this.gotoOK.TabIndex = 1;
            this.gotoOK.Text = "OK";
            this.gotoOK.UseVisualStyleBackColor = true;
            this.gotoOK.Click += new System.EventHandler(this.gotoOK_Click);
            // 
            // gotoCancel
            // 
            this.gotoCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.gotoCancel.Font = new System.Drawing.Font("Arial", 8F);
            this.gotoCancel.Location = new System.Drawing.Point(203, 69);
            this.gotoCancel.Name = "gotoCancel";
            this.gotoCancel.Size = new System.Drawing.Size(75, 23);
            this.gotoCancel.TabIndex = 1;
            this.gotoCancel.Text = "Cancel";
            this.gotoCancel.UseVisualStyleBackColor = true;
            this.gotoCancel.Click += new System.EventHandler(this.gotoCancel_Click);
            // 
            // GoToMessage
            // 
            this.AcceptButton = this.gotoOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.gotoCancel;
            this.ClientSize = new System.Drawing.Size(290, 101);
            this.Controls.Add(this.gotoCancel);
            this.Controls.Add(this.gotoOK);
            this.Controls.Add(this.gotoPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GoToMessage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Go To...";
            this.Load += new System.EventHandler(this.GoToMessage_Load);
            this.gotoPanel.ResumeLayout(false);
            this.gotoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel gotoPanel;
        private Button gotoOK;
        private Button gotoCancel;
        private System.Windows.Forms.TextBox gotoInput;
        private System.Windows.Forms.Label gotoLabel;
    }
}