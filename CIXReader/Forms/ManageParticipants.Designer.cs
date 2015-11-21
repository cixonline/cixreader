namespace CIXReader.Forms
{
    sealed partial class ManageParticipants
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
            this.forumAddPart = new System.Windows.Forms.Button();
            this.forumRemovePart = new System.Windows.Forms.Button();
            this.forumParList = new CIXReader.Controls.CRListView();
            this.parListImage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.parListName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // forumAddPart
            // 
            this.forumAddPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forumAddPart.Location = new System.Drawing.Point(324, 13);
            this.forumAddPart.Name = "forumAddPart";
            this.forumAddPart.Size = new System.Drawing.Size(75, 23);
            this.forumAddPart.TabIndex = 1;
            this.forumAddPart.Text = "&Add...";
            this.forumAddPart.UseVisualStyleBackColor = true;
            // 
            // forumRemovePart
            // 
            this.forumRemovePart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forumRemovePart.Location = new System.Drawing.Point(325, 43);
            this.forumRemovePart.Name = "forumRemovePart";
            this.forumRemovePart.Size = new System.Drawing.Size(75, 23);
            this.forumRemovePart.TabIndex = 2;
            this.forumRemovePart.Text = "&Remove";
            this.forumRemovePart.UseVisualStyleBackColor = true;
            // 
            // forumParList
            // 
            this.forumParList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forumParList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.parListImage,
            this.parListName});
            this.forumParList.FullRowSelect = true;
            this.forumParList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.forumParList.HideSelection = false;
            this.forumParList.Location = new System.Drawing.Point(12, 13);
            this.forumParList.MultiSelect = false;
            this.forumParList.Name = "forumParList";
            this.forumParList.ShowGroups = false;
            this.forumParList.Size = new System.Drawing.Size(307, 380);
            this.forumParList.TabIndex = 3;
            this.forumParList.UseCompatibleStateImageBehavior = false;
            this.forumParList.View = System.Windows.Forms.View.Details;
            this.forumParList.VirtualMode = true;
            // 
            // parListImage
            // 
            this.parListImage.Width = 30;
            // 
            // parListName
            // 
            this.parListName.Width = 277;
            // 
            // ManageParticipants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 405);
            this.ControlBox = false;
            this.Controls.Add(this.forumParList);
            this.Controls.Add(this.forumRemovePart);
            this.Controls.Add(this.forumAddPart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageParticipants";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Manage Participants";
            this.Load += new System.EventHandler(this.ManageParticipants_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button forumAddPart;
        private System.Windows.Forms.Button forumRemovePart;
        private Controls.CRListView forumParList;
        private System.Windows.Forms.ColumnHeader parListImage;
        private System.Windows.Forms.ColumnHeader parListName;
    }
}