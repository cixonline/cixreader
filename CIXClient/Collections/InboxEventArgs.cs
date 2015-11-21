// *****************************************************
// CIXReader
// InboxEventArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/06/2015 12:20
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that encapsulates the inbox event arguments
    /// </summary>
    public sealed class InboxEventArgs : EventArgs
    {
        /// <summary>
        /// A copy of the message that was recently added to the inbox.
        /// </summary>
        public InboxConversation Conversation { get; set; }
    }
}