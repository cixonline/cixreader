// *****************************************************
// CIXReader
// SmartFolder.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/06/2015 19:54
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Linq.Expressions;
using CIXClient.Collections;
using CIXClient.Tables;
using CIXReader.Properties;
using CIXReader.Utilities;

namespace CIXReader.SpecialFolders
{
    /// <summary>
    /// Implements a folder tree icon for smart folders
    /// </summary>
    public sealed class SmartFolder : FolderBase
    {
        private int _count;

        /// <summary>
        /// Identify this folder as a special folder.
        /// </summary>
        public override int ID
        {
            get { return -1; }
        }

        /// <summary>
        /// This is a topic folder.
        /// </summary>
        public override AppView ViewForFolder
        {
            get { return AppView.AppViewTopic; }
        }

        /// <summary>
        /// Return the icon for this draft folder.
        /// </summary>
        public override Image Icon
        {
            get { return Resources.SmartFolder; }
        }

        /// <summary>
        /// Store the criteria that returns the results from this
        /// smart folder.
        /// </summary>
        public Expression<Func<CIXMessage, bool>> Criteria { get; set; }

        /// <summary>
        /// Store the criteria that returns the results from this
        /// smart folder.
        /// </summary>
        public delegate bool ContainComparator(CIXMessage message);

        public ContainComparator Comparator { get; set; }

        /// <summary>
        /// Return all starred messages.
        /// </summary>
        public override CIXMessageCollection Items
        {
            get { 
                CIXMessageCollection col = FolderCollection.MessagesWithCriteria(Criteria);
                _count = col.Count;
                return col;
            }
        }

        /// <summary>
        /// Update the count of messages in this folder.
        /// </summary>
        public void RefreshCount()
        {
            if (Preferences.StandardPreferences.EnableSmartFolderCounts)
            {
                _count = Items.Count;
            }
        }

        /// <summary>
        /// Return the count of messages in this folder.
        /// </summary>
        public override int Count
        {
            get { return Preferences.StandardPreferences.EnableSmartFolderCounts ? _count : 0; }
        }

        /// <summary>
        /// Return true if the message is a draft.
        /// </summary>
        public override bool CanContain(CIXMessage message)
        {
            return Comparator(message);
        }
    }
}