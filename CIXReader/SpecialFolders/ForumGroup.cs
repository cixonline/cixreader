// *****************************************************
// CIXReader
// ForumGroup.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/06/2015 15:06
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXClient;

namespace CIXReader.SpecialFolders
{
    /// <summary>
    /// Implements a folder tree item for grouping forums
    /// </summary>
    public sealed class ForumGroup : FolderBase
    {
        public override int Unread
        {
            get { return CIX.FolderCollection.TotalUnread; }
        }
    }
}