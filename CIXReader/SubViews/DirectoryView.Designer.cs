namespace CIXReader.SubViews
{
    sealed partial class DirectoryView
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
            this.dvForumsList = new CIXReader.Controls.CRListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.popularityColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.descriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.subCategoryColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // dvForumsList
            // 
            this.dvForumsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dvForumsList.BackColor = System.Drawing.SystemColors.Window;
            this.dvForumsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dvForumsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.popularityColumn,
            this.descriptionColumn,
            this.subCategoryColumn});
            this.dvForumsList.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dvForumsList.FullRowSelect = true;
            this.dvForumsList.Location = new System.Drawing.Point(3, 0);
            this.dvForumsList.MultiSelect = false;
            this.dvForumsList.Name = "dvForumsList";
            this.dvForumsList.SearchRow = 0;
            this.dvForumsList.ShowGroups = false;
            this.dvForumsList.Size = new System.Drawing.Size(527, 466);
            this.dvForumsList.TabIndex = 5;
            this.dvForumsList.UseCompatibleStateImageBehavior = false;
            this.dvForumsList.View = System.Windows.Forms.View.Details;
            this.dvForumsList.VirtualMode = true;
            this.dvForumsList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.dvForumsList_ColumnClick);
            this.dvForumsList.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.dvForumsList_ColumnWidthChanging);
            this.dvForumsList.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.dvForumsList_RetrieveVirtualItem);
            this.dvForumsList.DoubleClick += new System.EventHandler(this.dvForumsList_DoubleClick);
            this.dvForumsList.Resize += new System.EventHandler(this.dvForumsList_Resize);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 125;
            // 
            // popularityColumn
            // 
            this.popularityColumn.Text = "Popularity";
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.Text = "Description";
            this.descriptionColumn.Width = 400;
            // 
            // subCategoryColumn
            // 
            this.subCategoryColumn.Text = "SubCategory";
            this.subCategoryColumn.Width = 100;
            // 
            // DirectoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dvForumsList);
            this.Name = "DirectoryView";
            this.Size = new System.Drawing.Size(527, 466);
            this.Load += new System.EventHandler(this.DirectoryView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRListView dvForumsList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader popularityColumn;
        private System.Windows.Forms.ColumnHeader descriptionColumn;
        private System.Windows.Forms.ColumnHeader subCategoryColumn;
    }
}