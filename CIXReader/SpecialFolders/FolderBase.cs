// *****************************************************
// CIXReader
// FolderBase.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/12/2013 13:37
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using CIXClient.Collections;
using CIXClient.Tables;

namespace CIXReader.SpecialFolders
{
    /// <summary>
    /// Defines the view used to display the folder
    /// </summary>
    public enum AppView
    {
        AppViewMail,
        AppViewForum,
        AppViewTopic,
        AppViewDirectory,
        AppViewWelcome    
    }

    /// <summary>
    /// Base class for the UI representation of a Folder item
    /// </summary>
    public class FolderBase
    {
        /// <summary>
        /// Return the folder ID.
        /// </summary>
        public virtual int ID
        {
            get { return 0; }
        }

        /// <summary>
        /// Specifies the view that displays this folder.
        /// </summary>
        public virtual AppView ViewForFolder
        {
            get { return AppView.AppViewWelcome; }
        }

        /// <summary>
        /// Folder name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return the icon for this folder.
        /// </summary>
        public virtual Image Icon
        {
            get { return null; }
        }

        /// <summary>
        /// Return all messages in this folder.
        /// </summary>
        // ReSharper disable ReturnTypeCanBeEnumerable.Global
        public virtual CIXMessageCollection Items
        {
            get { return null; }
        }

        /// <summary>
        /// Returns the number of unread messages in the folder.
        /// </summary>
        public virtual int Unread
        {
            get { return 0;  }
        }

        /// <summary>
        /// Returns the number of messages in the folder.
        /// </summary>
        public virtual int Count
        {
            get { return 0; }
        }

        /// <summary>
        /// Returns the number of unread priority messages in the folder.
        /// </summary>
        public virtual int UnreadPriority
        {
            get { return 0; }
        }

        /// <summary>
        /// Return the folder flags
        /// </summary>
        public virtual FolderFlags Flags
        {
            get { return 0; }
        }

        /// <summary>
        /// Return the folder address
        /// </summary>
        /// <summary>
        /// Return the address of this folder
        /// </summary>
        public virtual string Address
        {
            get { return "welcome"; }
        }

        /// <summary>
        /// Return the full name of the folder.
        /// </summary>
        public virtual string FullName
        {
            get { return Name; }
        }

        /// <summary>
        /// Get or set the RemoteID of the most recently selected message in
        /// the folder.
        /// </summary>
        public int RecentMessage { get; set; }

        /// <summary>
        /// Returns whether the folder can contain the specified message.
        /// </summary>
        public virtual bool CanContain(CIXMessage message)
        {
            return true;
        }

        /// <summary>
        /// Refreshes the folder
        /// </summary>
        public virtual void Refresh() {}

        /// <summary>
        /// Override to indicate whether this folder allows scoped searches
        /// </summary>
        public virtual bool AllowsScopedSearch
        {
            get { return false; }
        }
    }
}