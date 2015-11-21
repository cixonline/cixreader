using CIXReader.Controls;

namespace CIXReader.Forms
{
    internal sealed partial class CIXMessageEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CIXMessageEditor));
            this.nmMessage = new CIXReader.Controls.CRTextBox();
            this.nmContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nmUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.nmSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.nmCut = new System.Windows.Forms.ToolStripMenuItem();
            this.nmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.nmPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.nmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.nmSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.nmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.nmPanel = new CIXReader.Controls.CRPanel();
            this.nmSignatureList = new System.Windows.Forms.ComboBox();
            this.nmSignatureLabel = new System.Windows.Forms.Label();
            this.nmReplyPanel = new CIXReader.Controls.CRPanel();
            this.nmReplyText = new System.Windows.Forms.Label();
            this.nmReplyUsername = new System.Windows.Forms.Label();
            this.nmReplyImage = new System.Windows.Forms.PictureBox();
            this.nmEditPanel = new System.Windows.Forms.Panel();
            this.nmSend = new System.Windows.Forms.Button();
            this.nmCancel = new System.Windows.Forms.Button();
            this.nmSaveAsDraft = new System.Windows.Forms.Button();
            this.nmBodyTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.nmContextMenu.SuspendLayout();
            this.nmPanel.SuspendLayout();
            this.nmReplyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmReplyImage)).BeginInit();
            this.SuspendLayout();
            // 
            // nmMessage
            // 
            this.nmMessage.AcceptsReturn = true;
            this.nmMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmMessage.ContextMenuStrip = this.nmContextMenu;
            this.nmMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmMessage.Location = new System.Drawing.Point(13, 95);
            this.nmMessage.Multiline = true;
            this.nmMessage.Name = "nmMessage";
            this.nmMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.nmMessage.Size = new System.Drawing.Size(425, 240);
            this.nmMessage.TabIndex = 0;
            this.nmMessage.TextChanged += new System.EventHandler(this.nmMessage_TextChanged);
            // 
            // nmContextMenu
            // 
            this.nmContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nmUndo,
            this.nmSeparator1,
            this.nmCut,
            this.nmCopy,
            this.nmPaste,
            this.nmDelete,
            this.nmSeparator2,
            this.nmSelectAll});
            this.nmContextMenu.Name = "nmContextMenu";
            this.nmContextMenu.Size = new System.Drawing.Size(123, 148);
            this.nmContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.nmContextMenu_Opening);
            // 
            // nmUndo
            // 
            this.nmUndo.Name = "nmUndo";
            this.nmUndo.Size = new System.Drawing.Size(122, 22);
            this.nmUndo.Text = "&Undo";
            this.nmUndo.Click += new System.EventHandler(this.nmMessage_Undo);
            // 
            // nmSeparator1
            // 
            this.nmSeparator1.Name = "nmSeparator1";
            this.nmSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // nmCut
            // 
            this.nmCut.Name = "nmCut";
            this.nmCut.Size = new System.Drawing.Size(122, 22);
            this.nmCut.Text = "Cut";
            this.nmCut.Click += new System.EventHandler(this.nmMessage_Cut);
            // 
            // nmCopy
            // 
            this.nmCopy.Name = "nmCopy";
            this.nmCopy.Size = new System.Drawing.Size(122, 22);
            this.nmCopy.Text = "Copy";
            this.nmCopy.Click += new System.EventHandler(this.nmMessage_Copy);
            // 
            // nmPaste
            // 
            this.nmPaste.Name = "nmPaste";
            this.nmPaste.Size = new System.Drawing.Size(122, 22);
            this.nmPaste.Text = "Paste";
            this.nmPaste.Click += new System.EventHandler(this.nmMessage_Paste);
            // 
            // nmDelete
            // 
            this.nmDelete.Name = "nmDelete";
            this.nmDelete.Size = new System.Drawing.Size(122, 22);
            this.nmDelete.Text = "Delete";
            this.nmDelete.Click += new System.EventHandler(this.nmMessage_Delete);
            // 
            // nmSeparator2
            // 
            this.nmSeparator2.Name = "nmSeparator2";
            this.nmSeparator2.Size = new System.Drawing.Size(119, 6);
            // 
            // nmSelectAll
            // 
            this.nmSelectAll.Name = "nmSelectAll";
            this.nmSelectAll.Size = new System.Drawing.Size(122, 22);
            this.nmSelectAll.Text = "Select All";
            this.nmSelectAll.Click += new System.EventHandler(this.nmMessage_SelectAll);
            // 
            // nmPanel
            // 
            this.nmPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmPanel.BackColor = System.Drawing.SystemColors.Window;
            this.nmPanel.BottomBorderWidth = 1;
            this.nmPanel.Controls.Add(this.nmSignatureList);
            this.nmPanel.Controls.Add(this.nmSignatureLabel);
            this.nmPanel.Controls.Add(this.nmReplyPanel);
            this.nmPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.nmPanel.Gradient = false;
            this.nmPanel.LeftBorderWidth = 0;
            this.nmPanel.Location = new System.Drawing.Point(0, 0);
            this.nmPanel.Name = "nmPanel";
            this.nmPanel.RightBorderWidth = 0;
            this.nmPanel.Size = new System.Drawing.Size(450, 386);
            this.nmPanel.TabIndex = 4;
            this.nmPanel.TopBorderWidth = 0;
            // 
            // nmSignatureList
            // 
            this.nmSignatureList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmSignatureList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nmSignatureList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.nmSignatureList.FormattingEnabled = true;
            this.nmSignatureList.Location = new System.Drawing.Point(75, 348);
            this.nmSignatureList.Name = "nmSignatureList";
            this.nmSignatureList.Size = new System.Drawing.Size(166, 21);
            this.nmSignatureList.TabIndex = 2;
            this.nmSignatureList.SelectedIndexChanged += new System.EventHandler(this.nmSignatureList_SelectedIndexChanged);
            // 
            // nmSignatureLabel
            // 
            this.nmSignatureLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmSignatureLabel.AutoSize = true;
            this.nmSignatureLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nmSignatureLabel.Location = new System.Drawing.Point(13, 351);
            this.nmSignatureLabel.Name = "nmSignatureLabel";
            this.nmSignatureLabel.Size = new System.Drawing.Size(55, 13);
            this.nmSignatureLabel.TabIndex = 1;
            this.nmSignatureLabel.Text = "Si&gnature:";
            // 
            // nmReplyPanel
            // 
            this.nmReplyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmReplyPanel.BackColor = System.Drawing.SystemColors.Control;
            this.nmReplyPanel.BottomBorderWidth = 1;
            this.nmReplyPanel.Controls.Add(this.nmReplyText);
            this.nmReplyPanel.Controls.Add(this.nmReplyUsername);
            this.nmReplyPanel.Controls.Add(this.nmReplyImage);
            this.nmReplyPanel.Gradient = false;
            this.nmReplyPanel.LeftBorderWidth = 0;
            this.nmReplyPanel.Location = new System.Drawing.Point(0, 0);
            this.nmReplyPanel.Name = "nmReplyPanel";
            this.nmReplyPanel.RightBorderWidth = 0;
            this.nmReplyPanel.Size = new System.Drawing.Size(450, 81);
            this.nmReplyPanel.TabIndex = 0;
            this.nmReplyPanel.TopBorderWidth = 0;
            // 
            // nmReplyText
            // 
            this.nmReplyText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmReplyText.AutoEllipsis = true;
            this.nmReplyText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nmReplyText.Location = new System.Drawing.Point(72, 30);
            this.nmReplyText.Name = "nmReplyText";
            this.nmReplyText.Size = new System.Drawing.Size(365, 40);
            this.nmReplyText.TabIndex = 1;
            // 
            // nmReplyUsername
            // 
            this.nmReplyUsername.AutoSize = true;
            this.nmReplyUsername.BackColor = System.Drawing.SystemColors.Control;
            this.nmReplyUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmReplyUsername.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nmReplyUsername.Location = new System.Drawing.Point(69, 13);
            this.nmReplyUsername.Name = "nmReplyUsername";
            this.nmReplyUsername.Size = new System.Drawing.Size(0, 13);
            this.nmReplyUsername.TabIndex = 0;
            // 
            // nmReplyImage
            // 
            this.nmReplyImage.Location = new System.Drawing.Point(12, 13);
            this.nmReplyImage.Name = "nmReplyImage";
            this.nmReplyImage.Size = new System.Drawing.Size(50, 50);
            this.nmReplyImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.nmReplyImage.TabIndex = 0;
            this.nmReplyImage.TabStop = false;
            // 
            // nmEditPanel
            // 
            this.nmEditPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmEditPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmEditPanel.Location = new System.Drawing.Point(13, 95);
            this.nmEditPanel.Name = "nmEditPanel";
            this.nmEditPanel.Size = new System.Drawing.Size(425, 240);
            this.nmEditPanel.TabIndex = 1;
            // 
            // nmSend
            // 
            this.nmSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nmSend.Font = new System.Drawing.Font("Arial", 8F);
            this.nmSend.Location = new System.Drawing.Point(282, 395);
            this.nmSend.Name = "nmSend";
            this.nmSend.Size = new System.Drawing.Size(75, 23);
            this.nmSend.TabIndex = 2;
            this.nmSend.Text = "&Post";
            this.nmBodyTooltip.SetToolTip(this.nmSend, "Post the message");
            this.nmSend.UseVisualStyleBackColor = true;
            this.nmSend.Click += new System.EventHandler(this.nmSend_Click);
            // 
            // nmCancel
            // 
            this.nmCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nmCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.nmCancel.Font = new System.Drawing.Font("Arial", 8F);
            this.nmCancel.Location = new System.Drawing.Point(363, 395);
            this.nmCancel.Name = "nmCancel";
            this.nmCancel.Size = new System.Drawing.Size(75, 23);
            this.nmCancel.TabIndex = 3;
            this.nmCancel.Text = "Cancel";
            this.nmCancel.UseVisualStyleBackColor = true;
            this.nmCancel.Click += new System.EventHandler(this.nmCancel_Click);
            // 
            // nmSaveAsDraft
            // 
            this.nmSaveAsDraft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmSaveAsDraft.Font = new System.Drawing.Font("Arial", 8F);
            this.nmSaveAsDraft.Location = new System.Drawing.Point(12, 395);
            this.nmSaveAsDraft.Name = "nmSaveAsDraft";
            this.nmSaveAsDraft.Size = new System.Drawing.Size(100, 23);
            this.nmSaveAsDraft.TabIndex = 1;
            this.nmSaveAsDraft.Text = "&Save As Draft";
            this.nmBodyTooltip.SetToolTip(this.nmSaveAsDraft, "Save the message as a draft");
            this.nmSaveAsDraft.UseVisualStyleBackColor = true;
            this.nmSaveAsDraft.Click += new System.EventHandler(this.nmSaveAsDraft_Click);
            // 
            // CIXMessageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.nmCancel;
            this.ClientSize = new System.Drawing.Size(450, 427);
            this.Controls.Add(this.nmMessage);
            this.Controls.Add(this.nmEditPanel);
            this.Controls.Add(this.nmSaveAsDraft);
            this.Controls.Add(this.nmCancel);
            this.Controls.Add(this.nmSend);
            this.Controls.Add(this.nmPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "CIXMessageEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.CIXMessageEditor_Activated);
            this.Deactivate += new System.EventHandler(this.CIXMessageEditor_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CIXMessageEditor_FormClosing);
            this.Load += new System.EventHandler(this.CIXMessageEditor_Load);
            this.nmContextMenu.ResumeLayout(false);
            this.nmPanel.ResumeLayout(false);
            this.nmPanel.PerformLayout();
            this.nmReplyPanel.ResumeLayout(false);
            this.nmReplyPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmReplyImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CRTextBox nmMessage;
        private CRPanel nmPanel;
        private System.Windows.Forms.Button nmSend;
        private System.Windows.Forms.Button nmCancel;
        private System.Windows.Forms.Button nmSaveAsDraft;
        private System.Windows.Forms.ContextMenuStrip nmContextMenu;
        private System.Windows.Forms.ToolStripMenuItem nmUndo;
        private System.Windows.Forms.ToolStripSeparator nmSeparator1;
        private System.Windows.Forms.ToolStripMenuItem nmCut;
        private System.Windows.Forms.ToolStripMenuItem nmCopy;
        private System.Windows.Forms.ToolStripMenuItem nmPaste;
        private System.Windows.Forms.ToolStripMenuItem nmDelete;
        private System.Windows.Forms.ToolStripSeparator nmSeparator2;
        private System.Windows.Forms.ToolStripMenuItem nmSelectAll;
        private CRPanel nmReplyPanel;
        private System.Windows.Forms.Label nmReplyUsername;
        private System.Windows.Forms.PictureBox nmReplyImage;
        private System.Windows.Forms.Label nmReplyText;
        private System.Windows.Forms.ToolTip nmBodyTooltip;
        private System.Windows.Forms.Panel nmEditPanel;
        private System.Windows.Forms.ComboBox nmSignatureList;
        private System.Windows.Forms.Label nmSignatureLabel;
    }
}