namespace CIXReader.Forms
{
    sealed partial class ManageForumGeneral
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
            this.label1 = new System.Windows.Forms.Label();
            this.forumName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.forumTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.forumDesc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.forumType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.forumCategory = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.forumSubCategory = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // forumName
            // 
            this.forumName.AutoSize = true;
            this.forumName.Location = new System.Drawing.Point(106, 13);
            this.forumName.Name = "forumName";
            this.forumName.Size = new System.Drawing.Size(68, 13);
            this.forumName.TabIndex = 1;
            this.forumName.Text = "(forum name)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label2.Location = new System.Drawing.Point(106, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(301, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "You cannot change the forum name once it has been created.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "&Title:";
            // 
            // forumTitle
            // 
            this.forumTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forumTitle.Location = new System.Drawing.Point(106, 62);
            this.forumTitle.Name = "forumTitle";
            this.forumTitle.Size = new System.Drawing.Size(456, 20);
            this.forumTitle.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "&Description:";
            // 
            // forumDesc
            // 
            this.forumDesc.AcceptsReturn = true;
            this.forumDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forumDesc.Location = new System.Drawing.Point(106, 95);
            this.forumDesc.Multiline = true;
            this.forumDesc.Name = "forumDesc";
            this.forumDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.forumDesc.Size = new System.Drawing.Size(456, 147);
            this.forumDesc.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "T&ype:";
            // 
            // forumType
            // 
            this.forumType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.forumType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forumType.FormattingEnabled = true;
            this.forumType.Items.AddRange(new object[] {
            "Open",
            "Closed",
            "Private"});
            this.forumType.Location = new System.Drawing.Point(106, 257);
            this.forumType.Name = "forumType";
            this.forumType.Size = new System.Drawing.Size(68, 21);
            this.forumType.TabIndex = 8;
            this.forumType.SelectedIndexChanged += new System.EventHandler(this.forumType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "&Category:";
            // 
            // forumCategory
            // 
            this.forumCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.forumCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forumCategory.FormattingEnabled = true;
            this.forumCategory.Location = new System.Drawing.Point(106, 288);
            this.forumCategory.Name = "forumCategory";
            this.forumCategory.Size = new System.Drawing.Size(125, 21);
            this.forumCategory.Sorted = true;
            this.forumCategory.TabIndex = 10;
            this.forumCategory.SelectedIndexChanged += new System.EventHandler(this.forumCategory_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 321);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "&Sub-category:";
            // 
            // forumSubCategory
            // 
            this.forumSubCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.forumSubCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.forumSubCategory.FormattingEnabled = true;
            this.forumSubCategory.Location = new System.Drawing.Point(106, 319);
            this.forumSubCategory.Name = "forumSubCategory";
            this.forumSubCategory.Size = new System.Drawing.Size(125, 21);
            this.forumSubCategory.Sorted = true;
            this.forumSubCategory.TabIndex = 12;
            // 
            // ManageForumGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(574, 357);
            this.ControlBox = false;
            this.Controls.Add(this.forumSubCategory);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.forumCategory);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.forumType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.forumDesc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.forumTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.forumName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageForumGeneral";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.ManageForumGeneral_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label forumName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox forumTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox forumDesc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox forumType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox forumCategory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox forumSubCategory;
    }
}