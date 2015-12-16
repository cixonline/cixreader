// *****************************************************
// CIXReader
// OldCIXMessage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 16/02/2015 1:17 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Database;

namespace CIXClient.Tables
{
    /// <summary>
    /// Old CIXMessage table structure used to migrate data across to the
    /// new CIXMessage table.
    /// </summary>
    public sealed class OldCIXMessage
    {
        /// <summary>
        /// Gets or sets an unique local ID that identifies this message across all folders.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the CIX message ID that corresponds to this message. This can be
        /// 0 if the message is not yet synced with CIX.
        /// </summary>
        public int RemoteID { get; set; }

        /// <summary>
        /// Gets or sets the CIX author nickname.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the body of the message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the message was posted.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the CIX ID of the message to which this message is a reply.
        /// </summary>
        public int CommentID { get; set; }

        /// <summary>
        /// Gets or sets the CIX ID of the root message.
        /// </summary>
        public int RootID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the folder containing this message.
        /// </summary>
        public int TopicID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is unread.
        /// </summary>
        public bool Unread { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is priority.
        /// </summary>
        public bool Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is starred.
        /// </summary>
        public bool Starred { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is read locked.
        /// </summary>
        public bool ReadLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is ignored.
        /// </summary>
        public bool Ignored { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message read status change is pending.
        /// </summary>
        public bool ReadPending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is pending posting.
        /// </summary>
        public bool PostPending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message star status change is pending.
        /// </summary>
        public bool StarPending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message withdraw status change is pending.
        /// </summary>
        public bool WithdrawPending { get; set; }

        /// <summary>
        /// Gets or sets the flags that specify the message state.
        /// </summary>
        public CIXMessageFlags Flags { get; set; }
    }

    /// <summary>
    /// CIX Message flags.
    /// </summary>
    [Flags]
    public enum CIXMessageFlags
    {
        /// <summary>
        ///     Indicates that this message is unread.
        /// </summary>
        Unread = 1,

        /// <summary>
        ///     Indicates that this is a priority message.
        /// </summary>
        Priority = 2,

        /// <summary>
        ///     Indicates that this message has been starred.
        /// </summary>
        Starred = 4,

        /// <summary>
        ///     Indicates that this message is part of an ignored conversation.
        /// </summary>
        Ignored = 8,

        /// <summary>
        ///     Indicates that this message is a local draft that is ready to be
        ///     updated on the server.
        /// </summary>
        PostPending = 16,

        /// <summary>
        ///     Indicates that the star state of this message needs to be synchronised with
        ///     the server.
        /// </summary>
        StarPending = 128,

        /// <summary>
        ///     Indicates that this message's read state needs to be synchronized with
        ///     the server
        /// </summary>
        ReadPending = 256,

        /// <summary>
        ///     Indicates that this message is to be withdrawn.
        /// </summary>
        WithdrawPending = 512,

        /// <summary>
        ///     Indicates that the message is read locked
        /// </summary>
        ReadLocked = 8192
    }
}