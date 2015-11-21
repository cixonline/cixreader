namespace CIXReader.Controls
{
    sealed partial class CRSearchField
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRSearchField));
            this.searchInputField = new CRTextBox();
            this.searchIcon = new System.Windows.Forms.PictureBox();
            this.searchClose = new System.Windows.Forms.PictureBox();
            this.searchContainer = new CIXReader.Controls.CRRoundButton();
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchClose)).BeginInit();
            this.SuspendLayout();
            // 
            // searchInputField
            // 
            this.searchInputField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchInputField.BackColor = System.Drawing.SystemColors.Control;
            this.searchInputField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchInputField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchInputField.ForeColor = System.Drawing.Color.DarkGray;
            this.searchInputField.Location = new System.Drawing.Point(28, 5);
            this.searchInputField.Margin = new System.Windows.Forms.Padding(0);
            this.searchInputField.Name = "searchInputField";
            this.searchInputField.Size = new System.Drawing.Size(96, 13);
            this.searchInputField.TabIndex = 1;
            this.searchInputField.WordWrap = false;
            this.searchInputField.TextChanged += new System.EventHandler(this.searchInputField_TextChanged);
            this.searchInputField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchInputField_KeyDown);
            // 
            // searchIcon
            // 
            this.searchIcon.BackColor = System.Drawing.SystemColors.Control;
            this.searchIcon.Image = ((System.Drawing.Image)(resources.GetObject("searchIcon.Image")));
            this.searchIcon.Location = new System.Drawing.Point(6, 4);
            this.searchIcon.Name = "searchIcon";
            this.searchIcon.Size = new System.Drawing.Size(16, 16);
            this.searchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.searchIcon.TabIndex = 2;
            this.searchIcon.TabStop = false;
            this.searchIcon.Click += new System.EventHandler(this.searchIcon_Click);
            // 
            // searchClose
            // 
            this.searchClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchClose.BackColor = System.Drawing.SystemColors.Control;
            this.searchClose.Image = ((System.Drawing.Image)(resources.GetObject("searchClose.Image")));
            this.searchClose.Location = new System.Drawing.Point(128, 4);
            this.searchClose.Name = "searchClose";
            this.searchClose.Size = new System.Drawing.Size(16, 16);
            this.searchClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.searchClose.TabIndex = 2;
            this.searchClose.TabStop = false;
            this.searchClose.Visible = false;
            this.searchClose.Click += new System.EventHandler(this.searchClose_Click);
            // 
            // searchContainer
            // 
            this.searchContainer.Active = false;
            this.searchContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchContainer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.searchContainer.CanBeSelected = false;
            this.searchContainer.CanHaveFocus = false;
            this.searchContainer.ExtraData = null;
            this.searchContainer.ImageScaling = false;
            this.searchContainer.Location = new System.Drawing.Point(0, 0);
            this.searchContainer.Name = "searchContainer";
            this.searchContainer.Size = new System.Drawing.Size(147, 23);
            this.searchContainer.TabIndex = 0;
            this.searchContainer.Text = "";
            this.searchContainer.UseVisualStyleBackColor = false;
            // 
            // CRSearchField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchIcon);
            this.Controls.Add(this.searchClose);
            this.Controls.Add(this.searchInputField);
            this.Controls.Add(this.searchContainer);
            this.Name = "CRSearchField";
            this.Size = new System.Drawing.Size(150, 23);
            this.Load += new System.EventHandler(this.CRSearchField_Load);
            this.Enter += new System.EventHandler(this.CRSearchField_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CRRoundButton searchContainer;
        private CRTextBox searchInputField;
        private System.Windows.Forms.PictureBox searchIcon;
        private System.Windows.Forms.PictureBox searchClose;
    }
}
