// *****************************************************
// CIXReader
// KeyboardHelp.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/11/2013 8:09
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Windows.Forms;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// Declares a class that displays the Keyboard Help dialog.
    /// </summary>
    internal sealed partial class KeyboardHelp : Form
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="KeyboardHelp"/> class.
        /// </summary>
        public KeyboardHelp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the Keyboard Help dialog.
        /// </summary>
        private void kybdClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Fill out the Help with the current defined key map.
        /// </summary>
        private void KeyboardHelp_Load(object sender, EventArgs e)
        {
            kybdNextUnread.Text = UI.MapActionToKeyString(ActionID.NextUnread);
            kybdPrevious.Text = UI.MapActionToKeyString(ActionID.BackTrack);
            kybdPreviousThread.Text = UI.MapActionToKeyString(ActionID.Root);
            kybdNextThread.Text = UI.MapActionToKeyString(ActionID.NextRoot);
            kybdOriginal.Text = UI.MapActionToKeyString(ActionID.Original);
            kybdNextPriority.Text = UI.MapActionToKeyString(ActionID.NextPriorityUnread);
            kybdExpandThread.Text = UI.MapActionToKeyString(ActionID.Expand);
            kybdScrollNextUnread.Text = UI.MapActionToKeyString(ActionID.PageMessage);
            kybdGoto.Text = UI.MapActionToKeyString(ActionID.GoTo);

            kybdSay.Text = UI.MapActionToKeyString(ActionID.NewMessage);
            kybdComment.Text = UI.MapActionToKeyString(ActionID.Edit);
            kybdQuote.Text = UI.MapActionToKeyString(ActionID.Quote);
            kybdDel.Text = UI.MapActionToKeyString(ActionID.Delete);

            kybdMarkRead.Text = UI.MapActionToKeyString(ActionID.Read);
            kybdMarkStar.Text = UI.MapActionToKeyString(ActionID.Star);
            kybdMarkPriority.Text = UI.MapActionToKeyString(ActionID.Priority);
            kybdMarkThreadRead.Text = UI.MapActionToKeyString(ActionID.MarkThreadRead);
            kybdMarkIgnore.Text = UI.MapActionToKeyString(ActionID.Ignore);
            kybdWithdraw.Text = UI.MapActionToKeyString(ActionID.Withdraw);
            kybdMarkReadLock.Text = UI.MapActionToKeyString(ActionID.ReadLock);
            kybdMarkThreadRead.Text = UI.MapActionToKeyString(ActionID.MarkThreadRead);
            kybdTogglePlainText.Text = UI.MapActionToKeyString(ActionID.Markdown);

            kybdSearch.Text = UI.MapActionToKeyString(ActionID.Search);
            kybdHelp.Text = UI.MapActionToKeyString(ActionID.KeyboardHelp);
            kybdSystemMenu.Text = UI.MapActionToKeyString(ActionID.SystemMenu);
        }

        /// <summary>
        /// If the user closes the Keyboard Help window, just hide it rather than
        /// dispose of it.
        /// </summary>
        private void KeyboardHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}