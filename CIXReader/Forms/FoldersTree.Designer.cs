using System.Drawing;
using CIXReader.Controls;

namespace CIXReader.Forms
{
    sealed partial class FoldersTree
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
            this.components = new System.ComponentModel.Container();
            this.frmSplitContainer = new System.Windows.Forms.SplitContainer();
            this.frmContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.frmAllTopics = new System.Windows.Forms.ToolStripMenuItem();
            this.frmRecentTopics = new System.Windows.Forms.ToolStripMenuItem();
            this.frmMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.frmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.frmResign = new System.Windows.Forms.ToolStripMenuItem();
            this.frmManage = new System.Windows.Forms.ToolStripMenuItem();
            this.frmParticipants = new System.Windows.Forms.ToolStripMenuItem();
            this.frmMarkAllRead = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.frmRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.frmListPanel = new CIXReader.Controls.CRPanel();
            this.frmMessage = new System.Windows.Forms.Label();
            this.frmProgress = new CIXReader.Controls.CRProgress();
            this.frmList = new CIXReader.Controls.CRTreeView();
            this.frmInfoBar = new CIXReader.Controls.CRInfoBar();
            ((System.ComponentModel.ISupportInitialize)(this.frmSplitContainer)).BeginInit();
            this.frmSplitContainer.Panel1.SuspendLayout();
            this.frmSplitContainer.Panel2.SuspendLayout();
            this.frmSplitContainer.SuspendLayout();
            this.frmContextMenu.SuspendLayout();
            this.frmListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // frmSplitContainer
            // 
            this.frmSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmSplitContainer.BackColor = System.Drawing.SystemColors.GrayText;
            this.frmSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.frmSplitContainer.Name = "frmSplitContainer";
            // 
            // frmSplitContainer.Panel1
            // 
            this.frmSplitContainer.Panel1.Controls.Add(this.frmListPanel);
            // 
            // frmSplitContainer.Panel2
            // 
            this.frmSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.frmSplitContainer.Panel2.Controls.Add(this.frmInfoBar);
            this.frmSplitContainer.Panel2.SizeChanged += new System.EventHandler(this.frmSplitContainer_Panel2_SizeChanged);
            this.frmSplitContainer.Size = new System.Drawing.Size(680, 449);
            this.frmSplitContainer.SplitterDistance = 120;
            this.frmSplitContainer.TabIndex = 0;
            this.frmSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.frmSplitContainer_SplitterMoved);
            // 
            // frmContextMenu
            // 
            this.frmContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frmAllTopics,
            this.frmRecentTopics,
            this.frmMenuSeparator,
            this.frmDelete,
            this.frmResign,
            this.frmManage,
            this.frmParticipants,
            this.frmMarkAllRead,
            this.toolStripMenuItem1,
            this.frmRefresh});
            this.frmContextMenu.Name = "frmContextMenu";
            this.frmContextMenu.Size = new System.Drawing.Size(157, 192);
            // 
            // frmAllTopics
            // 
            this.frmAllTopics.Name = "frmAllTopics";
            this.frmAllTopics.Size = new System.Drawing.Size(156, 22);
            this.frmAllTopics.Text = "All Topics";
            this.frmAllTopics.Click += new System.EventHandler(this.frmAllTopics_Click);
            // 
            // frmRecentTopics
            // 
            this.frmRecentTopics.Name = "frmRecentTopics";
            this.frmRecentTopics.Size = new System.Drawing.Size(156, 22);
            this.frmRecentTopics.Text = "Recent Topics";
            this.frmRecentTopics.Click += new System.EventHandler(this.frmRecentTopics_Click);
            // 
            // frmMenuSeparator
            // 
            this.frmMenuSeparator.Name = "frmMenuSeparator";
            this.frmMenuSeparator.Size = new System.Drawing.Size(153, 6);
            // 
            // frmDelete
            // 
            this.frmDelete.Name = "frmDelete";
            this.frmDelete.Size = new System.Drawing.Size(156, 22);
            this.frmDelete.Text = "Delete...";
            this.frmDelete.Click += new System.EventHandler(this.frmDelete_Click);
            // 
            // frmResign
            // 
            this.frmResign.Name = "frmResign";
            this.frmResign.Size = new System.Drawing.Size(156, 22);
            this.frmResign.Text = "Resign...";
            this.frmResign.Click += new System.EventHandler(this.frmResign_Click);
            // 
            // frmManage
            // 
            this.frmManage.Name = "frmManage";
            this.frmManage.Size = new System.Drawing.Size(156, 22);
            this.frmManage.Text = "Manage...";
            this.frmManage.Click += new System.EventHandler(this.frmManage_Click);
            // 
            // frmParticipants
            // 
            this.frmParticipants.Name = "frmParticipants";
            this.frmParticipants.Size = new System.Drawing.Size(156, 22);
            this.frmParticipants.Text = "Participants";
            this.frmParticipants.Click += new System.EventHandler(this.frmParticipants_Click);
            // 
            // frmMarkAllRead
            // 
            this.frmMarkAllRead.Name = "frmMarkAllRead";
            this.frmMarkAllRead.Size = new System.Drawing.Size(156, 22);
            this.frmMarkAllRead.Text = "Mark All Read...";
            this.frmMarkAllRead.Click += new System.EventHandler(this.frmMarkAllRead_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 6);
            // 
            // frmRefresh
            // 
            this.frmRefresh.Name = "frmRefresh";
            this.frmRefresh.Size = new System.Drawing.Size(156, 22);
            this.frmRefresh.Text = "Refresh";
            this.frmRefresh.Click += new System.EventHandler(this.frmRefresh_Click);
            // 
            // frmListPanel
            // 
            this.frmListPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmListPanel.BottomBorderWidth = 0;
            this.frmListPanel.Controls.Add(this.frmMessage);
            this.frmListPanel.Controls.Add(this.frmProgress);
            this.frmListPanel.Controls.Add(this.frmList);
            this.frmListPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.frmListPanel.Gradient = false;
            this.frmListPanel.LeftBorderWidth = 0;
            this.frmListPanel.Location = new System.Drawing.Point(0, 0);
            this.frmListPanel.Name = "frmListPanel";
            this.frmListPanel.RightBorderWidth = 1;
            this.frmListPanel.Size = new System.Drawing.Size(120, 449);
            this.frmListPanel.TabIndex = 2;
            this.frmListPanel.TopBorderWidth = 0;
            // 
            // frmMessage
            // 
            this.frmMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmMessage.AutoSize = true;
            this.frmMessage.BackColor = System.Drawing.SystemColors.Window;
            this.frmMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frmMessage.Location = new System.Drawing.Point(26, 216);
            this.frmMessage.Name = "frmMessage";
            this.frmMessage.Size = new System.Drawing.Size(69, 16);
            this.frmMessage.TabIndex = 16;
            this.frmMessage.Text = "No forums";
            this.frmMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.frmMessage.Visible = false;
            // 
            // frmProgress
            // 
            this.frmProgress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.frmProgress.BackColor = System.Drawing.SystemColors.Window;
            this.frmProgress.Location = new System.Drawing.Point(40, 222);
            this.frmProgress.Name = "frmProgress";
            this.frmProgress.Percentage = 0F;
            this.frmProgress.Size = new System.Drawing.Size(40, 40);
            this.frmProgress.TabIndex = 15;
            this.frmProgress.Visible = false;
            // 
            // frmList
            // 
            this.frmList.AllowDrop = true;
            this.frmList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.frmList.ContextIcon = global::CIXReader.Properties.Resources.MenuDropBlue;
            this.frmList.ContextNode = null;
            this.frmList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.frmList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frmList.HideSelection = false;
            this.frmList.ItemHeight = 24;
            this.frmList.Location = new System.Drawing.Point(0, 0);
            this.frmList.Name = "frmList";
            this.frmList.SelectedContextIcon = global::CIXReader.Properties.Resources.MenuDropWhite;
            this.frmList.ShowLines = false;
            this.frmList.Size = new System.Drawing.Size(119, 449);
            this.frmList.TabIndex = 3;
            this.frmList.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.frmList_DrawNode);
            this.frmList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.frmList_ItemDrag);
            this.frmList.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.frmList_BeforeSelect);
            this.frmList.SizeChanged += new System.EventHandler(this.frmList_SizeChanged);
            this.frmList.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmList_DragDrop);
            this.frmList.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmList_DragEnter);
            this.frmList.DragOver += new System.Windows.Forms.DragEventHandler(this.frmList_DragOver);
            this.frmList.DragLeave += new System.EventHandler(this.frmList_DragLeave);
            this.frmList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmList_KeyPress);
            this.frmList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmList_MouseClick);
            this.frmList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmList_MouseDown);
            this.frmList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmList_MouseUp);
            // 
            // frmInfoBar
            // 
            this.frmInfoBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmInfoBar.BackColor = System.Drawing.Color.Cornsilk;
            this.frmInfoBar.InfoIcon = CIXReader.Controls.CRInfoBarIcon.None;
            this.frmInfoBar.InfoText = "";
            this.frmInfoBar.Location = new System.Drawing.Point(0, 0);
            this.frmInfoBar.Name = "frmInfoBar";
            this.frmInfoBar.Size = new System.Drawing.Size(554, 28);
            this.frmInfoBar.TabIndex = 0;
            this.frmInfoBar.Visible = false;
            // 
            // FoldersTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(680, 449);
            this.ControlBox = false;
            this.Controls.Add(this.frmSplitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FoldersTree";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FoldersTree_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FoldersTree_KeyDown);
            this.frmSplitContainer.Panel1.ResumeLayout(false);
            this.frmSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frmSplitContainer)).EndInit();
            this.frmSplitContainer.ResumeLayout(false);
            this.frmContextMenu.ResumeLayout(false);
            this.frmListPanel.ResumeLayout(false);
            this.frmListPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer frmSplitContainer;
        private CRPanel frmListPanel;
        private CRTreeView frmList;
        private System.Windows.Forms.Label frmMessage;
        private CRProgress frmProgress;
        private System.Windows.Forms.ContextMenuStrip frmContextMenu;
        private System.Windows.Forms.ToolStripMenuItem frmRecentTopics;
        private System.Windows.Forms.ToolStripMenuItem frmAllTopics;
        private System.Windows.Forms.ToolStripMenuItem frmMarkAllRead;
        private System.Windows.Forms.ToolStripMenuItem frmResign;
        private CRInfoBar frmInfoBar;
        private System.Windows.Forms.ToolStripMenuItem frmRefresh;
        private System.Windows.Forms.ToolStripSeparator frmMenuSeparator;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem frmDelete;
        private System.Windows.Forms.ToolStripMenuItem frmParticipants;
        private System.Windows.Forms.ToolStripMenuItem frmManage;
    }
}