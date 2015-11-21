using System.ComponentModel;
using System.Security.AccessControl;
using System.Windows.Forms;
using CIXReader.Controls;

namespace CIXReader.SubViews
{
    sealed partial class TopicView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.tsvMessages = new CIXReader.Controls.CRListView();
            this.singleColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tsvStatusPanel = new System.Windows.Forms.Panel();
            this.tsvStatusMessage = new System.Windows.Forms.Label();
            this.tsvProgress = new CIXReader.Controls.CRProgress();
            this.tsvThreadPane = new System.Windows.Forms.Panel();
            this.tsvSplitview = new System.Windows.Forms.SplitContainer();
            this.tsvMessagePane = new CIXReader.Canvas.Canvas();
            this.tsvStatusPanel.SuspendLayout();
            this.tsvThreadPane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tsvSplitview)).BeginInit();
            this.tsvSplitview.Panel1.SuspendLayout();
            this.tsvSplitview.Panel2.SuspendLayout();
            this.tsvSplitview.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsvMessages
            // 
            this.tsvMessages.BackColor = System.Drawing.SystemColors.Window;
            this.tsvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tsvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.singleColumn});
            this.tsvMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsvMessages.FullRowSelect = true;
            this.tsvMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.tsvMessages.Location = new System.Drawing.Point(0, 0);
            this.tsvMessages.MultiSelect = false;
            this.tsvMessages.Name = "tsvMessages";
            this.tsvMessages.OwnerDraw = true;
            this.tsvMessages.ShowGroups = false;
            this.tsvMessages.Size = new System.Drawing.Size(530, 215);
            this.tsvMessages.TabIndex = 0;
            this.tsvMessages.UseCompatibleStateImageBehavior = false;
            this.tsvMessages.View = System.Windows.Forms.View.Details;
            this.tsvMessages.VirtualMode = true;
            this.tsvMessages.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.tsvMessages_DrawSubItem);
            this.tsvMessages.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.tsvMessages_RetrieveVirtualItem);
            this.tsvMessages.SizeChanged += new System.EventHandler(this.tsvMessages_SizeChanged);
            this.tsvMessages.DoubleClick += new System.EventHandler(this.tsvMessages_DoubleClick);
            // 
            // singleColumn
            // 
            this.singleColumn.Text = "";
            this.singleColumn.Width = 530;
            // 
            // tsvStatusPanel
            // 
            this.tsvStatusPanel.BackColor = System.Drawing.SystemColors.Window;
            this.tsvStatusPanel.Controls.Add(this.tsvStatusMessage);
            this.tsvStatusPanel.Controls.Add(this.tsvProgress);
            this.tsvStatusPanel.Location = new System.Drawing.Point(113, 130);
            this.tsvStatusPanel.Name = "tsvStatusPanel";
            this.tsvStatusPanel.Size = new System.Drawing.Size(200, 100);
            this.tsvStatusPanel.TabIndex = 0;
            this.tsvStatusPanel.Visible = false;
            // 
            // tsvStatusMessage
            // 
            this.tsvStatusMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsvStatusMessage.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tsvStatusMessage.Location = new System.Drawing.Point(3, 74);
            this.tsvStatusMessage.Name = "tsvStatusMessage";
            this.tsvStatusMessage.Size = new System.Drawing.Size(194, 23);
            this.tsvStatusMessage.TabIndex = 1;
            this.tsvStatusMessage.Text = "Loading...";
            this.tsvStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tsvProgress
            // 
            this.tsvProgress.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tsvProgress.Location = new System.Drawing.Point(71, 8);
            this.tsvProgress.Name = "tsvProgress";
            this.tsvProgress.Percentage = 0F;
            this.tsvProgress.Size = new System.Drawing.Size(58, 58);
            this.tsvProgress.TabIndex = 0;
            // 
            // tsvThreadPane
            // 
            this.tsvThreadPane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsvThreadPane.Controls.Add(this.tsvMessages);
            this.tsvThreadPane.Location = new System.Drawing.Point(0, 0);
            this.tsvThreadPane.Name = "tsvThreadPane";
            this.tsvThreadPane.Size = new System.Drawing.Size(530, 215);
            this.tsvThreadPane.TabIndex = 0;
            // 
            // tsvSplitview
            // 
            this.tsvSplitview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsvSplitview.BackColor = System.Drawing.SystemColors.GrayText;
            this.tsvSplitview.Location = new System.Drawing.Point(0, 0);
            this.tsvSplitview.Name = "tsvSplitview";
            this.tsvSplitview.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // tsvSplitview.Panel1
            // 
            this.tsvSplitview.Panel1.Controls.Add(this.tsvThreadPane);
            // 
            // tsvSplitview.Panel2
            // 
            this.tsvSplitview.Panel2.Controls.Add(this.tsvMessagePane);
            this.tsvSplitview.Size = new System.Drawing.Size(530, 512);
            this.tsvSplitview.SplitterDistance = 215;
            this.tsvSplitview.TabIndex = 0;
            this.tsvSplitview.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.tsvSplitview_SplitterMoved);
            this.tsvSplitview.SizeChanged += new System.EventHandler(this.tsvSplitview_SizeChanged);
            // 
            // tsvMessagePane
            // 
            this.tsvMessagePane.AllowSelection = false;
            this.tsvMessagePane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsvMessagePane.AutoScroll = true;
            this.tsvMessagePane.AutoScrollMinSize = new System.Drawing.Size(75, 0);
            this.tsvMessagePane.BackColor = System.Drawing.SystemColors.Window;
            this.tsvMessagePane.CommentColour = System.Drawing.Color.Empty;
            this.tsvMessagePane.DisableMarkup = false;
            this.tsvMessagePane.ExpandInlineImages = true;
            this.tsvMessagePane.HasImages = true;
            this.tsvMessagePane.IndentationOffset = 10;
            this.tsvMessagePane.IsLayoutSuspended = false;
            this.tsvMessagePane.Location = new System.Drawing.Point(0, 0);
            this.tsvMessagePane.Name = "tsvMessagePane";
            this.tsvMessagePane.SelectedItem = null;
            this.tsvMessagePane.SelectionColour = System.Drawing.SystemColors.Highlight;
            this.tsvMessagePane.SeparatorColour = System.Drawing.SystemColors.GrayText;
            this.tsvMessagePane.Size = new System.Drawing.Size(530, 293);
            this.tsvMessagePane.TabIndex = 5;
            // 
            // TopicView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.tsvStatusPanel);
            this.Controls.Add(this.tsvSplitview);
            this.Name = "TopicView";
            this.Size = new System.Drawing.Size(530, 512);
            this.Load += new System.EventHandler(this.TopicThread_Load);
            this.Resize += new System.EventHandler(this.TopicView_Resize);
            this.tsvStatusPanel.ResumeLayout(false);
            this.tsvThreadPane.ResumeLayout(false);
            this.tsvSplitview.Panel1.ResumeLayout(false);
            this.tsvSplitview.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tsvSplitview)).EndInit();
            this.tsvSplitview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CRListView tsvMessages;
        private Panel tsvStatusPanel;
        private Label tsvStatusMessage;
        private CRProgress tsvProgress;
        private Panel tsvThreadPane;
        private SplitContainer tsvSplitview;
        private Canvas.Canvas tsvMessagePane;
        private ColumnHeader singleColumn;
    }
}
