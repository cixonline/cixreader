// *****************************************************
// CIXReader
// MailGroup.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/06/2015 15:05
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXClient;

namespace CIXReader.SpecialFolders
{
    public sealed class MailGroup : FolderBase
    {
        public override int Unread
        {
            get { return CIX.ConversationCollection.TotalUnread; }
        }
    }
}