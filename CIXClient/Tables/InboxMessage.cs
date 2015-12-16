// *****************************************************
// CIXReader
// InboxMessage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/08/2013 2:27 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXClient.Database;

namespace CIXClient.Tables
{
    /// <summary>
    /// A single InboxMessage represents one message in a
    /// conversation.
    /// </summary>
    public sealed class InboxMessage
    {
        /// <summary>
        /// Gets or sets an unique local ID that identifies this message.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the inbox message ID that corresponds to this message.
        /// This can be 0 if the message is not yet synced with the server.
        /// </summary>
        public int RemoteID { get; set; }

        /// <summary>
        /// Gets or sets the author of this message.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the body of the message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this message was posted.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the ID of the conversation to which this message belongs.
        /// </summary>
        public int ConversationID { get; set; }

        /// <summary>
        /// Gets a value indicating whether this message is synced
        /// with the server.
        /// </summary>
        [Ignore]
        public bool IsDraft
        {
            get { return RemoteID == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the authenticated user was the author
        /// of this message.
        /// </summary>
        [Ignore]
        public bool IsMine
        {
            get { return Author == CIX.Username; }
        }
    }
}
