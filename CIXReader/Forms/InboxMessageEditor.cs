// *****************************************************
// CIXReader
// InboxMessageEditor.cs
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
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// The InboxMessageEditor form displays an editor for inbox messages.
    /// </summary>
    internal sealed partial class InboxMessageEditor : SpellEditorBase
    {
        private InboxConversation _currentConversation;
        private InboxMessage _currentMessage;

        private readonly CIXMessage _messageToUse;
        private readonly string _defaultRecipient;

        /// <summary>
        /// Initialises a new instance of the <see cref="InboxMessageEditor"/> class.
        /// </summary>
        public InboxMessageEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InboxMessageEditor"/> class and
        /// populates it with data from the specified conversation.
        /// </summary>
        public InboxMessageEditor(InboxConversation conversation)
        {
            InitializeComponent();
            _currentConversation = conversation;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InboxMessageEditor"/> class and
        /// sets the recipient field to the specified recipient name.
        /// </summary>
        public InboxMessageEditor(string recipient)
        {
            InitializeComponent();
            _defaultRecipient = recipient;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InboxMessageEditor"/> class and
        /// populates it with data from the specified CIX message.
        /// </summary>
        public InboxMessageEditor(CIXMessage message)
        {
            InitializeComponent();
            _messageToUse = message;
        }

        /// <summary>
        /// Load the form and pre-fill as required.
        /// </summary>
        private void InboxMessageEditor_Load(object sender, EventArgs e)
        {
            Editor = nmMessage;

            // Restore the form width, height and state from the last time it
            // was invoked.
            this.RestoreFromSettings();

            // Load signature list
            ReloadSignaturesList(nmSignatureList);

            string body = string.Empty;

            if (_messageToUse != null)
            {
                nmRecipients.Text = _messageToUse.Author;
                nmSubject.Text = string.Format("Re: {0}", _messageToUse.Body.FirstNonBlankLine().TruncateByWordWithLimit(80));
                body = _messageToUse.Body.Quoted();
                ActiveControl = nmMessage;
            }

            if (!string.IsNullOrEmpty(_defaultRecipient))
            {
                nmRecipients.Text = _defaultRecipient;
                ActiveControl = nmSubject;
            }

            if (_currentConversation != null)
            {
                nmSubject.Enabled = false;
                nmRecipients.Enabled = false;

                string subjectString = _currentConversation.Subject;
                if (string.IsNullOrEmpty(subjectString))
                {
                    subjectString = Resources.NoSubject;
                }
                nmSubject.Text = subjectString;
                nmRecipients.Text = _currentConversation.Author;

                ActiveControl = nmMessage;
            }

            AddSignature = true;
            LoadMessage(string.Empty, body);

            nmSignatureList.SelectedItem = CurrentSignatureTitle;

            UpdateTitle();
            EnableSendButton();
        }

        /// <summary>
        /// Handle the Cancel button. Display a prompt if the message actually has some content.
        /// </summary>
        /// <param name="sender">The new message form</param>
        /// <param name="e">Event arguments</param>
        private void nmCancel_Click(object sender, EventArgs e)
        {
            if (CheckForClosing())
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        /// <summary>
        /// Handle the Send button. At this point we assume there are valid recipients and something
        /// in the message body. We really ought to do some validation of the recipient names here.
        /// </summary>
        /// <param name="sender">The new message form</param>
        /// <param name="e">Event arguments</param>
        private void nmSend_Click(object sender, EventArgs e)
        {
            if (_currentConversation == null)
            {
                _currentConversation = new InboxConversation
                {
                    Subject = nmSubject.Text,
                    Author = CIX.Username,
                    Date = DateTime.Now
                };
            }
            if (_currentMessage == null)
            {
                _currentMessage = new InboxMessage
                {
                    Author = (_currentConversation.ID > 0) ? CIX.Username : nmRecipients.Text,
                    Body = nmMessage.Text,
                    ConversationID = _currentConversation.ID,
                    Date = DateTime.Now
                };

                CIX.ConversationCollection.Add(_currentConversation, _currentMessage);
            }

            DialogResult = DialogResult.OK;
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
        /// Respond to changes in the recipient list and enable or disable the Send button as
        /// appropriate.
        /// </summary>
        /// <param name="sender">The recipient list control</param>
        /// <param name="e">Event arguments</param>
        private void nmRecipients_TextChanged(object sender, EventArgs e)
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
        /// in the recipients or message body.
        /// </summary>
        private void EnableSendButton()
        {
            nmSend.Enabled = RecipientsHasContent() && MessageHasContent();
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
        /// Returns whether the recipient field is empty or not. A field consisting of just whitespace
        /// (spaces, tabs, newlines) is considered empty.
        /// </summary>
        /// <returns>True if the recipient field has content, false if it is empty</returns>
        private bool RecipientsHasContent()
        {
            return !string.IsNullOrWhiteSpace(nmRecipients.Text);
        }

        /// <summary>
        /// Save window state when the form is closing.
        /// </summary>
        private void InboxMessageEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.None)
            {
                if (!CheckForClosing())
                {
                    e.Cancel = true;
                    return;
                }
            }
            this.SaveToSettings();
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
        /// Ensure the recipient name is a valid CIX nickname.
        /// </summary>
        private void nmRecipients_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && (e.KeyChar != '_'))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Update the signature in the text body when the user selects a new one.
        /// </summary>
        private void nmSignatureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string signature = (string)nmSignatureList.SelectedItem;
            InsertSignature(signature);
        }

        /// <summary>
        /// Update the window caption with details of this message.
        /// </summary>
        private void UpdateTitle()
        {
            if (_currentConversation != null)
            {
                string title = string.Format(Resources.ReplyEditorTitle, nmSubject.Text);
                Text = title;
            }
            else
            {
                bool hasSubject = !string.IsNullOrWhiteSpace(nmSubject.Text);
                Text = hasSubject ? nmSubject.Text : Resources.NewPMessage;
            }
        }

        /// <summary>
        /// Update the caption when the subject changes.
        /// </summary>
        private void nmSubject_TextChanged(object sender, EventArgs e)
        {
            UpdateTitle();
        }
    }
}