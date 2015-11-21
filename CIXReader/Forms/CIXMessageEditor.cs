// *****************************************************
// CIXReader
// CIXMessageEditor.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/09/2013 8:12 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Properties;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// The CIXMessageEditor form displays an editor for new and existing CIX messages.
    /// </summary>
    internal sealed partial class CIXMessageEditor : SpellEditorBase
    {
        private readonly CIXMessage _message;
        private bool _wasPending;

        /// <summary>
        /// Constructs a CIXMessageEditor instance.
        /// </summary>
        /// <param name="message">The CIXMessage to display in the editor</param>
        /// <param name="addSignature">True if a signature should be added</param>
        public CIXMessageEditor(CIXMessage message, bool addSignature)
        {
            InitializeComponent();
            AddSignature = addSignature;
            _message = message;
        }

        /// <summary>
        /// Get or set a flag which indicates if this form was closed.
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Check whether the form can be closed.
        /// </summary>
        public bool PreClose()
        {
            return CheckForClosing();
        }

        /// <summary>
        /// Display the message editor window.
        /// </summary>
        private void CIXMessageEditor_Load(object sender, EventArgs e)
        {
            bool isReply = _message.CommentID > 0;
            string forumName = string.Empty;
            string topicName = string.Empty;

            Editor = nmMessage;

            // Restore the form width, height and state from the last time it
            // was invoked.
            this.RestoreFromSettings();

            Folder folder = _message.Topic;
            if (folder != null)
            {
                Folder parentFolder = folder.ParentFolder;
                if (parentFolder != null)
                {
                    forumName = parentFolder.Name;
                }
                topicName = folder.Name;
            }

            if (isReply && folder != null)
            {
                string replyName = string.Empty;
                isReply = false;

                CIXMessage parentMessage = folder.Messages.MessageByID(_message.CommentID);
                if (parentMessage != null)
                {
                    replyName = parentMessage.Author;

                    Mugshot mugshot = Mugshot.MugshotForUser(replyName, true);

                    nmReplyImage.Image = mugshot.RealImage;
                    nmReplyUsername.Text = string.Format(Resources.ReplyEditorTitle, replyName);
                    nmReplyText.Text = parentMessage.Body;

                    // Make the full body available as a tooltip.
                    nmBodyTooltip.SetToolTip(nmReplyText, parentMessage.Body);

                    isReply = true;
                }

                Text = string.Format(Resources.ReplyTitle, replyName, forumName, topicName);
            }
            else
            {
                Text = string.Format(Resources.NewMessageTitle, forumName, topicName);
            }
            if (!isReply)
            {
                nmReplyPanel.Visible = false;
                nmMessage.Top -= nmReplyPanel.Height;
                nmMessage.Height += nmReplyPanel.Height;
            }

            // Load signature list
            ReloadSignaturesList(nmSignatureList);

            // Add any existing message with signature.
            LoadMessage(forumName, _message.Body);

            nmMessage.Font = UI.GetFont(UI.Forums.font, UI.Forums.fontsize);

            _wasPending = _message.IsPending;
            _message.PostPending = false;

            MessageEditorCollection.Add(this);

            EnableSendButton();
        }

        /// <summary>
        /// Handle the Cancel button. Display a prompt if the message actually has some content.
        /// </summary>
        /// <param name="sender">The message editor form</param>
        /// <param name="e">Event arguments</param>
        private void nmCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            if (_wasPending)
            {
                _message.PostPending = true;
            }

            Close();
        }

        /// <summary>
        /// Handle the Send button.
        /// </summary>
        /// <param name="sender">The message editor form</param>
        /// <param name="e">Event arguments</param>
        private void nmSend_Click(object sender, EventArgs e)
        {
            _message.Body = nmMessage.Text;
            _message.Date = DateTime.Now;

            _message.Topic.Messages.Add(_message);
            _message.Post();

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Handle the Save As Draft button.
        /// </summary>
        /// <param name="sender">The message editor form</param>
        /// <param name="e">Event arguments</param>
        private void nmSaveAsDraft_Click(object sender, EventArgs e)
        {
            _message.Body = nmMessage.Text;
            _message.Date = DateTime.Now;

            _message.Topic.Messages.Add(_message);

            DialogResult = DialogResult.Retry;
            Close();
        }

        /// <summary>
        /// Respond to changes in the message body and enable or disable the Send button as
        /// appropriate.
        /// </summary>
        /// <param name="sender">The message body control</param>
        /// <param name="e">Event arguments</param>
        private void nmMessage_TextChanged(object sender, EventArgs e)
        {
            EnableSendButton();
        }

        /// <summary>
        /// Handle the context menu Copy command.
        /// </summary>
        private void nmMessage_Copy(object sender, EventArgs e)
        {
            Copy();
        }

        /// <summary>
        /// Handle the context menu Cut command.
        /// </summary>
        private void nmMessage_Cut(object sender, EventArgs e)
        {
            Cut();
        }

        /// <summary>
        /// Handle the context menu Delete command.
        /// </summary>
        private void nmMessage_Delete(object sender, EventArgs e)
        {
            Delete();
        }

        /// <summary>
        /// Handle the context menu Paste command.
        /// </summary>
        private void nmMessage_Paste(object sender, EventArgs e)
        {
            Paste();
        }

        /// <summary>
        /// Handle the context menu Select All command.
        /// </summary>
        private void nmMessage_SelectAll(object sender, EventArgs e)
        {
            SelectAll();
        }

        /// <summary>
        /// Handle the context menu Undo command.
        /// </summary>
        private void nmMessage_Undo(object sender, EventArgs e)
        {
            Undo();
        }

        /// <summary>
        /// Enable or disable the Send button depending on whether there's any content
        /// in the message body.
        /// </summary>
        private void EnableSendButton()
        {
            nmSend.Enabled = MessageHasContent();
            nmSaveAsDraft.Enabled = MessageHasContent();
        }

        /// <summary>
        /// Returns whether the message body is empty or not. A message consisting of just whitespace
        /// (spaces, tabs, newlines) is considered empty.
        /// </summary>
        /// <returns>True if the message body has content, false if it is empty</returns>
        private bool MessageHasContent()
        {
            return !string.IsNullOrWhiteSpace(nmMessage.Text);
        }

        /// <summary>
        /// Save window state when the form is closing.
        /// </summary>
        private void CIXMessageEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel || DialogResult == DialogResult.None)
            {
                if (!CheckForClosing())
                {
                    e.Cancel = true;
                    return;
                }
            }

            MessageEditorCollection.Remove(this);

            this.SaveToSettings();

            IsClosed = true;
        }

        /// <summary>
        /// Check whether the form has been modified and prompt whether to save if so.
        /// </summary>
        /// <returns>True if we can close, false otherwise</returns>
        private bool CheckForClosing()
        {
            if (nmMessage.Modified && nmMessage.TextLength > 0)
            {
                BringToFront();
                if (MessageBox.Show(Resources.CancelNewMessagePrompt, Resources.CancelNewMessageTitle, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns whether the message being edited matches the specified CIXMessage.
        /// </summary>
        public bool Matches(CIXMessage message)
        {
            if (_message == null || message == null)
            {
                return false;
            }
            return _message.CommentID == message.CommentID && _message.TopicID == message.TopicID;
        }

        /// <summary>
        /// Handle the context menu opening by initialising the item states.
        /// </summary>
        private void nmContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            nmUndo.Enabled = nmMessage.CanUndo;
            nmPaste.Enabled = Clipboard.ContainsText();
            nmCut.Enabled = nmMessage.SelectionLength > 0;
            nmCopy.Enabled = nmMessage.SelectionLength > 0;
            nmSelectAll.Enabled = nmMessage.Text.Length > 0;
            nmDelete.Enabled = nmMessage.SelectionLength > 0;
        }

        /// <summary>
        /// Change the signature
        /// </summary>
        private void nmSignatureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string signature = (string)nmSignatureList.SelectedItem;
            InsertSignature(signature);
        }

        /// <summary>
        /// Suspend sync when the editor has focus.
        /// </summary>
        private void CIXMessageEditor_Activated(object sender, EventArgs e)
        {
            CIX.SuspendTasks();
        }

        /// <summary>
        /// Resume sync when the editor loses focus.
        /// </summary>
        private void CIXMessageEditor_Deactivate(object sender, EventArgs e)
        {
            CIX.ResumeTasks();
        }
    }
}