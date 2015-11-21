using CIXReader.Controls;

namespace CIXReader.Forms
{
    sealed partial class Participants
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
            this.parViewProfile = new System.Windows.Forms.Button();
            this.parClose = new System.Windows.Forms.Button();
            this.parCount = new System.Windows.Forms.Label();
            this.parList = new CIXReader.Controls.CRListView();
            this.parListImage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.parListName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // parViewProfile
            // 
            this.parViewProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.parViewProfile.Font = new System.Drawing.Font("Arial", 8F);
            this.parViewProfile.Location = new System.Drawing.Point(305, 13);
            this.parViewProfile.Name = "parViewProfile";
            this.parViewProfile.Size = new System.Drawing.Size(102, 23);
            this.parViewProfile.TabIndex = 1;
            this.parViewProfile.Text = "&View Profile";
            this.parViewProfile.UseVisualStyleBackColor = true;
            this.parViewProfile.Click += new System.EventHandler(this.parViewProfile_Click);
            // 
            // parClose
            // 
            this.parClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.parClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.parClose.Font = new System.Drawing.Font("Arial", 8F);
            this.parClose.Location = new System.Drawing.Point(305, 43);
            this.parClose.Name = "parClose";
            this.parClose.Size = new System.Drawing.Size(102, 23);
            this.parClose.TabIndex = 2;
            this.parClose.Text = "&Close";
            this.parClose.UseVisualStyleBackColor = true;
            this.parClose.Click += new System.EventHandler(this.parClose_Click);
            // 
            // parCount
            // 
            this.parCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.parCount.Location = new System.Drawing.Point(13, 521);
            this.parCount.Name = "parCount";
            this.parCount.Size = new System.Drawing.Size(285, 15);
            this.parCount.TabIndex = 3;
            // 
            // parList
            // 
            this.parList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.parListImage,
            this.parListName});
            this.parList.FullRowSelect = true;
            this.parList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.parList.HideSelection = false;
            this.parList.Location = new System.Drawing.Point(13, 13);
            this.parList.MultiSelect = false;
            this.parList.Name = "parList";
            this.parList.ShowGroups = false;
            this.parList.Size = new System.Drawing.Size(285, 501);
            this.parList.TabIndex = 0;
            this.parList.UseCompatibleStateImageBehavior = false;
            this.parList.View = System.Windows.Forms.View.Details;
            this.parList.VirtualMode = true;
            this.parList.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.parList_RetrieveVirtualItem);
            this.parList.SelectedIndexChanged += new System.EventHandler(this.parList_SelectedIndexChanged);
            this.parList.DoubleClick += new System.EventHandler(this.parList_DoubleClick);
            this.parList.Resize += new System.EventHandler(this.parList_Resize);
            // 
            // parListImage
            // 
            this.parListImage.Width = 30;
            // 
            // parListName
            // 
            this.parListName.Width = 281;
            // 
            // Participants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.parClose;
            this.ClientSize = new System.Drawing.Size(416, 545);
            this.ControlBox = false;
            this.Controls.Add(this.parCount);
            this.Controls.Add(this.parClose);
            this.Controls.Add(this.parViewProfile);
            this.Controls.Add(this.parList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Participants";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Participants";
            this.Load += new System.EventHandler(this.Participants_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CRListView parList;
        private System.Windows.Forms.Button parViewProfile;
        private System.Windows.Forms.Button parClose;
        private System.Windows.Forms.ColumnHeader parListName;
        private System.Windows.Forms.Label parCount;
        private System.Windows.Forms.ColumnHeader parListImage;
    }
}