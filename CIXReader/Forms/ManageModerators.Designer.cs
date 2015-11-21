namespace CIXReader.Forms
{
    sealed partial class ManageModerators
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
            this.forumAddMod = new System.Windows.Forms.Button();
            this.forumRemoveMod = new System.Windows.Forms.Button();
            this.forumModList = new CIXReader.Controls.CRListView();
            this.forumModListImage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.forumModListName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // modAddPart
            // 
            this.forumAddMod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forumAddMod.Location = new System.Drawing.Point(324, 13);
            this.forumAddMod.Name = "modAddPart";
            this.forumAddMod.Size = new System.Drawing.Size(75, 23);
            this.forumAddMod.TabIndex = 1;
            this.forumAddMod.Text = "&Add...";
            this.forumAddMod.UseVisualStyleBackColor = true;
            // 
            // modRemovePart
            // 
            this.forumRemoveMod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forumRemoveMod.Location = new System.Drawing.Point(325, 43);
            this.forumRemoveMod.Name = "modRemovePart";
            this.forumRemoveMod.Size = new System.Drawing.Size(75, 23);
            this.forumRemoveMod.TabIndex = 2;
            this.forumRemoveMod.Text = "&Remove";
            this.forumRemoveMod.UseVisualStyleBackColor = true;
            // 
            // forumModList
            // 
            this.forumModList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forumModList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.forumModListImage,
            this.forumModListName});
            this.forumModList.FullRowSelect = true;
            this.forumModList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.forumModList.HideSelection = false;
            this.forumModList.Location = new System.Drawing.Point(12, 13);
            this.forumModList.MultiSelect = false;
            this.forumModList.Name = "forumModList";
            this.forumModList.ShowGroups = false;
            this.forumModList.Size = new System.Drawing.Size(307, 380);
            this.forumModList.TabIndex = 3;
            this.forumModList.UseCompatibleStateImageBehavior = false;
            this.forumModList.View = System.Windows.Forms.View.Details;
            this.forumModList.VirtualMode = true;
            // 
            // forumModListImage
            // 
            this.forumModListImage.Width = 30;
            // 
            // forumModListName
            // 
            this.forumModListName.Width = 277;
            // 
            // ManageModerators
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 405);
            this.ControlBox = false;
            this.Controls.Add(this.forumModList);
            this.Controls.Add(this.forumRemoveMod);
            this.Controls.Add(this.forumAddMod);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageModerators";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Manage Participants";
            this.Load += new System.EventHandler(this.ManageModerators_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button forumAddMod;
        private System.Windows.Forms.Button forumRemoveMod;
        private Controls.CRListView forumModList;
        private System.Windows.Forms.ColumnHeader forumModListImage;
        private System.Windows.Forms.ColumnHeader forumModListName;
    }
}