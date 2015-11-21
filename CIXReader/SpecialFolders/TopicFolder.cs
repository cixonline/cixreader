// *****************************************************
// CIXReader
// TopicFolder.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/12/2013 14:40
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Linq;
using CIXClient.Collections;
using CIXClient.Tables;
using CIXReader.Properties;

namespace CIXReader.SpecialFolders
{
    /// <summary>
    /// Folder instance that defines a topic folder
    /// </summary>
    public sealed class TopicFolder : FolderBase
    {
        /// <summary>
        /// Constructs a single TopicFolder instance using the given folder
        /// as the data source.
        /// </summary>
        /// <param name="folder">A folder</param>
        public TopicFolder(Folder folder)
        {
            Folder = folder;
        }

        /// <summary>
        /// This is a topic or forum folder.
        /// </summary>
        public override AppView ViewForFolder
        {
            get { return Folder.IsRootFolder ? AppView.AppViewForum : AppView.AppViewTopic; }
        }

        /// <summary>
        /// Returns the folder data source.
        /// </summary>
        public Folder Folder { get; set; }

        /// <summary>
        /// Return the physical folder ID.
        /// </summary>
        public override int ID
        {
            get { return Folder.ID; }
        }

        /// <summary>
        /// Return the icon for this starred folder.
        /// </summary>
        public override Image Icon
        {
            get
            {
                if (Folder.IsRootFolder)
                {
                    return Resources.SmallFolder;
                }
                return Folder.IsReadOnly ? Resources.ROTopic : Resources.Topic;
            }
        }

        /// <summary>
        /// Return the addressable location of this folder.
        /// </summary>
        public override string Address
        {
            get
            {
                if (Folder.ParentID == -1)
                {
                    return String.Format("cix:{0}", Name);
                }
                Folder parentFolder = Folder.ParentFolder;
                return (parentFolder != null) ? String.Format("cix:{0}/{1}", parentFolder.Name, Name) : null;
            }
        }

        /// <summary>
        /// Return the full name of this folder
        /// </summary>
        public override string FullName 
        {
            get
            {
                if (Folder.ParentID == -1)
                {
                    return Name;
                }
                Folder forum = Folder.ParentFolder;
                string lockSymbol = Folder.IsReadOnly ? " \uD83D\uDD12" : "";
                return String.Format("{0} • {1}{2}", forum.Name, Name, lockSymbol);
            }
        }

        /// <summary>
        /// Return all starred messages.
        /// </summary>
        public override CIXMessageCollection Items
        {
            get
            {
                return Folder.Messages;
            }
        }

        /// <summary>
        /// Return the count of unread messages on this folder.
        /// </summary>
        public override int Unread
        {
            get
            {
                return Folder.ParentID == -1 ? Folder.Children.Sum(child => child.Unread) : Folder.Unread;
            }
        }

        /// <summary>
        /// Return the count of unread priority messages on this folder.
        /// </summary>
        public override int UnreadPriority
        {
            get
            {
                return Folder.ParentID == -1 ? Folder.Children.Sum(child => child.UnreadPriority) : Folder.UnreadPriority;
            }
        }

        /// <summary>
        /// Return the count of messages on this folder.
        /// </summary>
        public override int Count
        {
            get
            {
                return Unread;
            }
        }

        /// <summary>
        /// Return the folder flags
        /// </summary>
        public override FolderFlags Flags
        {
            get { return Folder.Flags; }
        }

        /// <summary>
        /// Topic folders can contain all messages.
        /// </summary>
        public override bool CanContain(CIXMessage message)
        {
            return message.TopicID == ID;
        }

        /// <summary>
        /// Refresh the folder.
        /// </summary>
        public override void Refresh()
        {
            if (Folder.ParentID == -1)
            {
                foreach (Folder topic in Folder.Children)
                {
                    topic.Refresh();
                }
            }
            else
            {
                Folder.Refresh();
            }
        }

        /// <summary>
        /// Topic folders allow scoped searches
        /// </summary>
        public override bool AllowsScopedSearch
        {
            get { return true; }
        }
    }
}