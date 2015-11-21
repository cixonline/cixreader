namespace CIXReader.SubViews
{
    sealed partial class InboxView
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
            this.inboxSplitview = new System.Windows.Forms.SplitContainer();
            this.inboxConversations = new CIXReader.Controls.CRListView();
            this.singleColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inboxMessagePane = new CIXReader.Canvas.Canvas();
            ((System.ComponentModel.ISupportInitialize)(this.inboxSplitview)).BeginInit();
            this.inboxSplitview.Panel1.SuspendLayout();
            this.inboxSplitview.Panel2.SuspendLayout();
            this.inboxSplitview.SuspendLayout();
            this.SuspendLayout();
            // 
            // inboxSplitview
            // 
            this.inboxSplitview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inboxSplitview.Location = new System.Drawing.Point(0, 0);
            this.inboxSplitview.Name = "inboxSplitview";
            this.inboxSplitview.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // inboxSplitview.Panel1
            // 
            this.inboxSplitview.Panel1.Controls.Add(this.inboxConversations);
            // 
            // inboxSplitview.Panel2
            // 
            this.inboxSplitview.Panel2.Controls.Add(this.inboxMessagePane);
            this.inboxSplitview.Size = new System.Drawing.Size(574, 452);
            this.inboxSplitview.SplitterDistance = 190;
            this.inboxSplitview.TabIndex = 0;
            this.inboxSplitview.SizeChanged += new System.EventHandler(this.inboxSplitContainer_SizeChanged);
            // 
            // inboxConversations
            // 
            this.inboxConversations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inboxConversations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.singleColumn});
            this.inboxConversations.FullRowSelect = true;
            this.inboxConversations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.inboxConversations.Location = new System.Drawing.Point(0, 0);
            this.inboxConversations.MultiSelect = false;
            this.inboxConversations.Name = "inboxConversations";
            this.inboxConversations.OwnerDraw = true;
            this.inboxConversations.Size = new System.Drawing.Size(574, 190);
            this.inboxConversations.TabIndex = 0;
            this.inboxConversations.UseCompatibleStateImageBehavior = false;
            this.inboxConversations.View = System.Windows.Forms.View.Details;
            this.inboxConversations.VirtualMode = true;
            this.inboxConversations.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.DrawSubItem);
            this.inboxConversations.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.RetrieveVirtualItem);
            this.inboxConversations.SelectedIndexChanged += new System.EventHandler(this.OnSelectionChanged);
            this.inboxConversations.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // singleColumn
            // 
            this.singleColumn.Width = 530;
            // 
            // inboxMessagePane
            // 
            this.inboxMessagePane.AllowSelection = false;
            this.inboxMessagePane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inboxMessagePane.AutoScroll = true;
            this.inboxMessagePane.AutoScrollMinSize = new System.Drawing.Size(75, 0);
            this.inboxMessagePane.BackColor = System.Drawing.SystemColors.Window;
            this.inboxMessagePane.CommentColour = System.Drawing.Color.Empty;
            this.inboxMessagePane.DisableMarkup = false;
            this.inboxMessagePane.ExpandInlineImages = false;
            this.inboxMessagePane.HasImages = true;
            this.inboxMessagePane.IndentationOffset = 10;
            this.inboxMessagePane.IsLayoutSuspended = false;
            this.inboxMessagePane.Location = new System.Drawing.Point(0, 0);
            this.inboxMessagePane.Name = "inboxMessagePane";
            this.inboxMessagePane.SelectedItem = null;
            this.inboxMessagePane.SelectionColour = System.Drawing.SystemColors.Highlight;
            this.inboxMessagePane.SeparatorColour = System.Drawing.SystemColors.GrayText;
            this.inboxMessagePane.Size = new System.Drawing.Size(574, 258);
            this.inboxMessagePane.TabIndex = 0;
            // 
            // InboxView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.inboxSplitview);
            this.Name = "InboxView";
            this.Size = new System.Drawing.Size(574, 452);
            this.Load += new System.EventHandler(this.InboxView_Load);
            this.Resize += new System.EventHandler(this.InboxView_Resize);
            this.inboxSplitview.Panel1.ResumeLayout(false);
            this.inboxSplitview.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxSplitview)).EndInit();
            this.inboxSplitview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer inboxSplitview;
        private Controls.CRListView inboxConversations;
        private Canvas.Canvas inboxMessagePane;
        private System.Windows.Forms.ColumnHeader singleColumn;
    }
}