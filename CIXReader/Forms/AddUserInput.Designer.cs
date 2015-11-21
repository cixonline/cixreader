namespace CIXReader.Forms
{
    sealed partial class AddUserInput
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
            this.addLabel = new System.Windows.Forms.Label();
            this.inputField = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.crPanel1 = new CIXReader.Controls.CRPanel();
            this.crPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // addLabel
            // 
            this.addLabel.AutoSize = true;
            this.addLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.addLabel.Location = new System.Drawing.Point(13, 24);
            this.addLabel.Name = "addLabel";
            this.addLabel.Size = new System.Drawing.Size(61, 13);
            this.addLabel.TabIndex = 0;
            this.addLabel.Text = "&User name:";
            // 
            // inputField
            // 
            this.inputField.ForeColor = System.Drawing.SystemColors.ControlText;
            this.inputField.Location = new System.Drawing.Point(95, 21);
            this.inputField.Name = "inputField";
            this.inputField.Size = new System.Drawing.Size(235, 20);
            this.inputField.TabIndex = 1;
            this.inputField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputField_KeyPress);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(173, 75);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "&Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(255, 75);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // crPanel1
            // 
            this.crPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.crPanel1.BottomBorderWidth = 1;
            this.crPanel1.Controls.Add(this.inputField);
            this.crPanel1.Controls.Add(this.addLabel);
            this.crPanel1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.crPanel1.LeftBorderWidth = 0;
            this.crPanel1.Location = new System.Drawing.Point(0, 0);
            this.crPanel1.Name = "crPanel1";
            this.crPanel1.RightBorderWidth = 0;
            this.crPanel1.Size = new System.Drawing.Size(341, 66);
            this.crPanel1.TabIndex = 0;
            this.crPanel1.TopBorderWidth = 0;
            // 
            // AddUserInput
            // 
            this.AcceptButton = this.addButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(342, 107);
            this.Controls.Add(this.crPanel1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddUserInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add...";
            this.crPanel1.ResumeLayout(false);
            this.crPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label addLabel;
        private System.Windows.Forms.TextBox inputField;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
        private Controls.CRPanel crPanel1;
    }
}