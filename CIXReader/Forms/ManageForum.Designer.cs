using System.Windows.Forms;

namespace CIXReader.Forms
{
    partial class ManageForum
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
            this.managePanel = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.manageToolbar = new CIXReader.Controls.CRPanel();
            this.manageParts = new System.Windows.Forms.Button();
            this.manageMods = new System.Windows.Forms.Button();
            this.manageGeneral = new System.Windows.Forms.Button();
            this.manageToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // managePanel
            // 
            this.managePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.managePanel.Location = new System.Drawing.Point(0, 57);
            this.managePanel.Name = "managePanel";
            this.managePanel.Size = new System.Drawing.Size(493, 341);
            this.managePanel.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Font = new System.Drawing.Font("Arial", 8F);
            this.saveButton.Location = new System.Drawing.Point(326, 399);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Font = new System.Drawing.Font("Arial", 8F);
            this.cancelButton.Location = new System.Drawing.Point(407, 399);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // manageToolbar
            // 
            this.manageToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.manageToolbar.BackColor = System.Drawing.SystemColors.Window;
            this.manageToolbar.BottomBorderWidth = 1;
            this.manageToolbar.Controls.Add(this.manageParts);
            this.manageToolbar.Controls.Add(this.manageMods);
            this.manageToolbar.Controls.Add(this.manageGeneral);
            this.manageToolbar.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.manageToolbar.Gradient = false;
            this.manageToolbar.LeftBorderWidth = 0;
            this.manageToolbar.Location = new System.Drawing.Point(0, 0);
            this.manageToolbar.Name = "manageToolbar";
            this.manageToolbar.RightBorderWidth = 0;
            this.manageToolbar.Size = new System.Drawing.Size(493, 57);
            this.manageToolbar.TabIndex = 0;
            this.manageToolbar.TopBorderWidth = 0;
            // 
            // manageParts
            // 
            this.manageParts.FlatAppearance.BorderSize = 0;
            this.manageParts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manageParts.Font = new System.Drawing.Font("Arial", 8F);
            this.manageParts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.manageParts.Image = global::CIXReader.Properties.Resources.ManageParts;
            this.manageParts.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.manageParts.Location = new System.Drawing.Point(152, 0);
            this.manageParts.Name = "manageParts";
            this.manageParts.Size = new System.Drawing.Size(76, 56);
            this.manageParts.TabIndex = 2;
            this.manageParts.Text = "Participants";
            this.manageParts.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.manageParts.UseVisualStyleBackColor = true;
            this.manageParts.Click += new System.EventHandler(this.manageParts_Click);
            // 
            // manageMods
            // 
            this.manageMods.FlatAppearance.BorderSize = 0;
            this.manageMods.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manageMods.Font = new System.Drawing.Font("Arial", 8F);
            this.manageMods.ForeColor = System.Drawing.SystemColors.ControlText;
            this.manageMods.Image = global::CIXReader.Properties.Resources.ManageMods;
            this.manageMods.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.manageMods.Location = new System.Drawing.Point(76, 0);
            this.manageMods.Name = "manageMods";
            this.manageMods.Size = new System.Drawing.Size(76, 56);
            this.manageMods.TabIndex = 1;
            this.manageMods.Text = "Moderators";
            this.manageMods.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.manageMods.UseVisualStyleBackColor = true;
            this.manageMods.Click += new System.EventHandler(this.manageMods_Click);
            // 
            // manageGeneral
            // 
            this.manageGeneral.FlatAppearance.BorderSize = 0;
            this.manageGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manageGeneral.Font = new System.Drawing.Font("Arial", 8F);
            this.manageGeneral.ForeColor = System.Drawing.SystemColors.ControlText;
            this.manageGeneral.Image = global::CIXReader.Properties.Resources.ManageGeneral;
            this.manageGeneral.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.manageGeneral.Location = new System.Drawing.Point(0, 0);
            this.manageGeneral.Name = "manageGeneral";
            this.manageGeneral.Size = new System.Drawing.Size(76, 56);
            this.manageGeneral.TabIndex = 0;
            this.manageGeneral.Text = "General";
            this.manageGeneral.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.manageGeneral.UseVisualStyleBackColor = true;
            this.manageGeneral.Click += new System.EventHandler(this.manageGeneral_Click);
            // 
            // ManageForum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 431);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.managePanel);
            this.Controls.Add(this.manageToolbar);
            this.Name = "ManageForum";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Forum";
            this.Load += new System.EventHandler(this.ManageForum_Load);
            this.Resize += new System.EventHandler(this.ManageForum_Resize);
            this.manageToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel manageToolbar;
        private System.Windows.Forms.Button manageGeneral;
        private System.Windows.Forms.Panel managePanel;
        private System.Windows.Forms.Button manageParts;
        private System.Windows.Forms.Button manageMods;
        private Button saveButton;
        private Button cancelButton;
    }
}