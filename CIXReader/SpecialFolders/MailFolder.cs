// *****************************************************
// CIXReader
// MailFolder.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/06/2015 19:39
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using CIXClient;
using CIXReader.Properties;

namespace CIXReader.SpecialFolders
{
    /// <summary>
    /// Implements a folder tree item for CIX mail boxes
    /// </summary>
    public sealed class MailFolder : FolderBase
    {
        /// <summary>
        /// This is a directory view folder.
        /// </summary>
        public override AppView ViewForFolder
        {
            get { return AppView.AppViewMail; }
        }

        /// <summary>
        /// Return the Image that corresponds to the specified category name.
        /// </summary>
        public override Image Icon
        {
            get
            {
                return Resources.Inbox;
            }
        }

        /// <summary>
        /// Return the addressable location of this folder.
        /// </summary>
        public override string Address
        {
            get { return "cixmailbox:inbox"; }
        }

        /// <summary>
        /// Return the count of unread messages on this folder.
        /// </summary>
        public override int Unread
        {
            get { return CIX.ConversationCollection.TotalUnread; }
        }

        /// <summary>
        /// Return the count of messages on this folder.
        /// </summary>
        public override int Count
        {
            get { return Unread; }
        }

        /// <summary>
        /// Refresh this folder.
        /// </summary>
        public override void Refresh()
        {
            CIX.ConversationCollection.Refresh();
        }
    }
}