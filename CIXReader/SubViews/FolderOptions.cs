// *****************************************************
// CIXReader
// FolderOptions.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 12/06/2015 21:55
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;

namespace CIXReader.SubViews
{
    /// <summary>
    /// Controls behaviour when a folder is first displayed
    /// </summary>
    [Flags]
    public enum FolderOptions
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,

        /// <summary>
        /// Display the first unread message (priority or otherwise)
        /// </summary>
        NextUnread = 1,

        /// <summary>
        /// Display the first unread priority message
        /// </summary>
        Priority = 2,

        /// <summary>
        /// Clear the search filter
        /// </summary>
        ClearFilter = 4,

        /// <summary>
        /// Go to the root of the next thread with unread messages
        /// </summary>
        Root = 8,

        /// <summary>
        /// When entering a folder, search for unread from the top rather than the current selection
        /// </summary>
        Reset = 16
    }
}