// *****************************************************
// CIXReader
// InboxMessageCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/10/2013 8:05 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// The MailCollection class manages a collection of inbox messages
    /// </summary>
    public sealed class MailCollection : IEnumerable<InboxMessage>
    {
        private readonly List<InboxMessage> _messages;

        /// <summary>
        /// Initialises a MessageCollection with a range of messages.
        /// </summary>
        /// <param name="messages">An array of messages</param>
        internal MailCollection(IEnumerable<InboxMessage> messages)
        {
            _messages = new List<InboxMessage>(messages);
        }

        /// <summary>
        /// Returns an enumerator for iterating over the message collection.
        /// </summary>
        /// <returns>An enumerator for InboxMessage</returns>
        public IEnumerator<InboxMessage> GetEnumerator()
        {
            return (_messages != null) ? _messages.GetEnumerator() : new List<InboxMessage>.Enumerator();
        }

        /// <summary>
        /// Returns whether the collection contains a message with the given remote ID.
        /// </summary>
        /// <param name="messageID">The message ID to check for</param>
        /// <returns>True if the message with that remote ID exists, false otherwise</returns>
        public bool Contains(int messageID)
        {
            return _messages.Any(msg => msg.RemoteID == messageID);
        }

        /// <summary>
        /// Returns an enumerator for iterating over the message collection.
        /// </summary>
        /// <returns>A generic enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add a new message to the inbox message collection.
        /// </summary>
        /// <param name="newMessage">The new InboxMessage to add</param>
        internal void AddInternal(InboxMessage newMessage)
        {
            lock (CIX.DBLock)
            {
                CIX.DB.Insert(newMessage);
            }
            _messages.Add(newMessage);
        }
    }
}