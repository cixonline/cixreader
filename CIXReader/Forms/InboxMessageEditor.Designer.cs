using CIXReader.Controls;

namespace CIXReader.Forms
{
    internal sealed partial class InboxMessageEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InboxMessageEditor));
            this.nmRecipientLabel = new System.Windows.Forms.Label();
            this.nmRecipients = new CIXReader.Controls.CRTextBox();
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
            this.nmMainPanel = new CIXReader.Controls.CRPanel();
            this.nmSignatureList = new System.Windows.Forms.ComboBox();
            this.nmSignatureLabel = new System.Windows.Forms.Label();
            this.nmReplyPanel = new CIXReader.Controls.CRPanel();
            this.nmReplyUsername = new System.Windows.Forms.Label();
            this.nmSubject = new CIXReader.Controls.CRTextBox();
            this.nmSubjectLabel = new System.Windows.Forms.Label();
            this.nmSend = new System.Windows.Forms.Button();
            this.nmCancel = new System.Windows.Forms.Button();
            this.nmContextMenu.SuspendLayout();
            this.nmMainPanel.SuspendLayout();
            this.nmReplyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // nmRecipientLabel
            // 
            this.nmRecipientLabel.AutoSize = true;
            this.nmRecipientLabel.BackColor = System.Drawing.SystemColors.Control;
            this.nmRecipientLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmRecipientLabel.Location = new System.Drawing.Point(13, 16);
            this.nmRecipientLabel.Name = "nmRecipientLabel";
            this.nmRecipientLabel.Size = new System.Drawing.Size(23, 13);
            this.nmRecipientLabel.TabIndex = 1;
            this.nmRecipientLabel.Text = "&To:";
            // 
            // nmRecipients
            // 
            this.nmRecipients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmRecipients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmRecipients.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmRecipients.Location = new System.Drawing.Point(43, 13);
            this.nmRecipients.Name = "nmRecipients";
            this.nmRecipients.Size = new System.Drawing.Size(387, 20);
            this.nmRecipients.TabIndex = 2;
            this.nmRecipients.TextChanged += new System.EventHandler(this.nmRecipients_TextChanged);
            this.nmRecipients.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nmRecipients_KeyPress);
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
            this.nmMessage.Size = new System.Drawing.Size(417, 231);
            this.nmMessage.TabIndex = 5;
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
            // nmMainPanel
            // 
            this.nmMainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmMainPanel.BackColor = System.Drawing.SystemColors.Window;
            this.nmMainPanel.BottomBorderWidth = 1;
            this.nmMainPanel.Controls.Add(this.nmSignatureList);
            this.nmMainPanel.Controls.Add(this.nmSignatureLabel);
            this.nmMainPanel.Controls.Add(this.nmReplyPanel);
            this.nmMainPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.nmMainPanel.Gradient = false;
            this.nmMainPanel.LeftBorderWidth = 0;
            this.nmMainPanel.Location = new System.Drawing.Point(0, 0);
            this.nmMainPanel.Name = "nmMainPanel";
            this.nmMainPanel.RightBorderWidth = 0;
            this.nmMainPanel.Size = new System.Drawing.Size(442, 377);
            this.nmMainPanel.TabIndex = 5;
            this.nmMainPanel.TopBorderWidth = 0;
            // 
            // nmSignatureList
            // 
            this.nmSignatureList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmSignatureList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nmSignatureList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.nmSignatureList.FormattingEnabled = true;
            this.nmSignatureList.Location = new System.Drawing.Point(75, 339);
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
            this.nmSignatureLabel.Location = new System.Drawing.Point(13, 342);
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
            this.nmReplyPanel.Controls.Add(this.nmReplyUsername);
            this.nmReplyPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.nmReplyPanel.Gradient = false;
            this.nmReplyPanel.LeftBorderWidth = 0;
            this.nmReplyPanel.Location = new System.Drawing.Point(0, 0);
            this.nmReplyPanel.Name = "nmReplyPanel";
            this.nmReplyPanel.RightBorderWidth = 0;
            this.nmReplyPanel.Size = new System.Drawing.Size(442, 81);
            this.nmReplyPanel.TabIndex = 0;
            this.nmReplyPanel.TopBorderWidth = 0;
            // 
            // nmReplyUsername
            // 
            this.nmReplyUsername.AutoSize = true;
            this.nmReplyUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmReplyUsername.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.nmReplyUsername.Location = new System.Drawing.Point(69, 13);
            this.nmReplyUsername.Name = "nmReplyUsername";
            this.nmReplyUsername.Size = new System.Drawing.Size(0, 13);
            this.nmReplyUsername.TabIndex = 0;
            // 
            // nmSubject
            // 
            this.nmSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmSubject.Location = new System.Drawing.Point(78, 45);
            this.nmSubject.Name = "nmSubject";
            this.nmSubject.Size = new System.Drawing.Size(351, 20);
            this.nmSubject.TabIndex = 4;
            this.nmSubject.TextChanged += new System.EventHandler(this.nmSubject_TextChanged);
            // 
            // nmSubjectLabel
            // 
            this.nmSubjectLabel.AutoSize = true;
            this.nmSubjectLabel.BackColor = System.Drawing.SystemColors.Control;
            this.nmSubjectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmSubjectLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nmSubjectLabel.Location = new System.Drawing.Point(13, 47);
            this.nmSubjectLabel.Name = "nmSubjectLabel";
            this.nmSubjectLabel.Size = new System.Drawing.Size(46, 13);
            this.nmSubjectLabel.TabIndex = 3;
            this.nmSubjectLabel.Text = "Su&bject:";
            // 
            // nmSend
            // 
            this.nmSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nmSend.Font = new System.Drawing.Font("Arial", 8F);
            this.nmSend.Location = new System.Drawing.Point(274, 386);
            this.nmSend.Name = "nmSend";
            this.nmSend.Size = new System.Drawing.Size(75, 23);
            this.nmSend.TabIndex = 6;
            this.nmSend.Text = "&Send";
            this.nmSend.UseVisualStyleBackColor = true;
            this.nmSend.Click += new System.EventHandler(this.nmSend_Click);
            // 
            // nmCancel
            // 
            this.nmCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nmCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.nmCancel.Font = new System.Drawing.Font("Arial", 8F);
            this.nmCancel.Location = new System.Drawing.Point(355, 386);
            this.nmCancel.Name = "nmCancel";
            this.nmCancel.Size = new System.Drawing.Size(75, 23);
            this.nmCancel.TabIndex = 7;
            this.nmCancel.Text = "Cancel";
            this.nmCancel.UseVisualStyleBackColor = true;
            this.nmCancel.Click += new System.EventHandler(this.nmCancel_Click);
            // 
            // InboxMessageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.nmCancel;
            this.ClientSize = new System.Drawing.Size(442, 418);
            this.Controls.Add(this.nmCancel);
            this.Controls.Add(this.nmSend);
            this.Controls.Add(this.nmMessage);
            this.Controls.Add(this.nmRecipients);
            this.Controls.Add(this.nmRecipientLabel);
            this.Controls.Add(this.nmSubject);
            this.Controls.Add(this.nmSubjectLabel);
            this.Controls.Add(this.nmMainPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "InboxMessageEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Message";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InboxMessageEditor_FormClosing);
            this.Load += new System.EventHandler(this.InboxMessageEditor_Load);
            this.nmContextMenu.ResumeLayout(false);
            this.nmMainPanel.ResumeLayout(false);
            this.nmMainPanel.PerformLayout();
            this.nmReplyPanel.ResumeLayout(false);
            this.nmReplyPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nmRecipientLabel;
        private CRTextBox nmRecipients;
        private System.Windows.Forms.Label nmSubjectLabel;
        private CRTextBox nmSubject;
        private CRTextBox nmMessage;
        private System.Windows.Forms.Button nmSend;
        private System.Windows.Forms.Button nmCancel;
        private System.Windows.Forms.ContextMenuStrip nmContextMenu;
        private System.Windows.Forms.ToolStripMenuItem nmUndo;
        private System.Windows.Forms.ToolStripSeparator nmSeparator1;
        private System.Windows.Forms.ToolStripMenuItem nmCut;
        private System.Windows.Forms.ToolStripMenuItem nmCopy;
        private System.Windows.Forms.ToolStripMenuItem nmPaste;
        private System.Windows.Forms.ToolStripMenuItem nmDelete;
        private System.Windows.Forms.ToolStripSeparator nmSeparator2;
        private System.Windows.Forms.ToolStripMenuItem nmSelectAll;
        private CRPanel nmMainPanel;
        private System.Windows.Forms.ComboBox nmSignatureList;
        private System.Windows.Forms.Label nmSignatureLabel;
        private CRPanel nmReplyPanel;
        private System.Windows.Forms.Label nmReplyUsername;
    }
}