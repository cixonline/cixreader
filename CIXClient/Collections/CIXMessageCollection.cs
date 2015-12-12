// *****************************************************
// CIXReader
// MessageCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 02/10/2013 1:49 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// The CIXMessageCollection class manages a collection of CIX messages.
    /// </summary>
    public sealed class CIXMessageCollection : IEnumerable<CIXMessage>
    {
        private List<CIXMessage> _messages;
        private List<CIXMessage> _threadedMessages;
        private bool _isOrdered;

        // Used to guard access to updates to the list
        private readonly Object _changeLock = new Object();

        /// <summary>
        /// Initialises a CIXMessageCollection with a range of messages.
        /// </summary>
        /// <param name="messages">An array of messages</param>
        public CIXMessageCollection(IEnumerable<CIXMessage> messages)
        {
            lock (_changeLock)
            {
                _messages = new List<CIXMessage>(messages);
                _isOrdered = false;
            }
        }

        /// <summary>
        /// Return the number of messages in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _messages.Count;
            }
        }

        /// <summary>
        /// Return all messages ordered by date
        /// </summary>
        public IEnumerable<CIXMessage> OrderedMessages
        {
            get {
                if (!_isOrdered)
                {
                    lock (_changeLock)
                    {
                        _messages = _messages.OrderBy(msg => msg.RemoteID).ToList();

                        // Sanity check!
                        int lastMessageID = -1;
                        for (int i = 0; i < _messages.Count; ++i)
                        {
                            CIXMessage message = _messages[i];
                            if (message.RemoteID == lastMessageID)
                            {
                                lock (CIX.DBLock)
                                {
                                    CIX.DB.Delete(message);
                                }
                                _messages.RemoveAt(i--);
                                LogFile.WriteLine("Removed duplicated record {0}", message.ID);
                            }
                            lastMessageID = message.RemoteID;
                        }

                        _isOrdered = true;
                    }
                }
                return _messages;
            }
        }

        /// <summary>
        /// Return all messages ordered by conversation.
        /// </summary>
        public List<CIXMessage> AllMessagesByConversation
        {
            get
            {
                if (_threadedMessages == null)
                {
                    lock (_changeLock)
                    {
                        int index = 0;

                        _threadedMessages = new List<CIXMessage>(OrderedMessages);
                        int count = _threadedMessages.Count;
                        while (index < count)
                        {
                            CIXMessage message = _threadedMessages[index];
                            message.Level = 0;
                            message.LastChildMessage = message;
                            message.LatestDate = message.Date;
                            if (message.CommentID > 0)
                            {
                                int parentIndex = index - 1;
                                while (parentIndex >= 0)
                                {
                                    CIXMessage parentMessage = _threadedMessages[parentIndex];
                                    if (parentMessage.RemoteID == message.CommentID)
                                    {
                                        CIXMessage lastChild = parentMessage.LastChildMessage;

                                        while (lastChild != lastChild.LastChildMessage)
                                        {
                                            lastChild = lastChild.LastChildMessage;
                                        }
                                        int insertIndex = _threadedMessages.IndexOf(lastChild) + 1;
                                        if (insertIndex > 0 && insertIndex != index)
                                        {
                                            _threadedMessages.RemoveAt(index);
                                            _threadedMessages.Insert(insertIndex, message);
                                        }
                                        parentMessage.LastChildMessage = message;
                                        message.Level = parentMessage.Level + 1;
                                        message.Parent = parentMessage;

                                        parentMessage.LatestDate = LatestOf(parentMessage.LatestDate, message.Date);
                                        while (parentMessage.Parent != null)
                                        {
                                            parentMessage = parentMessage.Parent;
                                            parentMessage.LatestDate = LatestOf(parentMessage.LatestDate, message.Date);
                                        }
                                        break;
                                    }
                                    --parentIndex;
                                }
                            }
                            ++index;
                        }
                    }
                }
                return _threadedMessages;
            }
        }

        /// <summary>
        /// Return all root messages.
        /// </summary>
        public IEnumerable<CIXMessage> Roots
        {
            get
            {
                List<CIXMessage> conversations = AllMessagesByConversation;
                lock (_changeLock)
                {
                    return conversations.Where(msg => msg.CommentID == 0).ToList();
                }
            }
        }

        /// <summary>
        /// Return the child messages of the specified message.
        /// </summary>
        /// <param name="message">The parent message</param>
        /// <returns>A collection of child messages ordered by thread.</returns>
        public IEnumerable<CIXMessage> Children(CIXMessage message)
        {
            List<CIXMessage> conversations = AllMessagesByConversation;
            lock (_changeLock)
            {
                int index = conversations.IndexOf(message);
                List<CIXMessage> children = new List<CIXMessage>();

                while (++index < conversations.Count)
                {
                    CIXMessage child = conversations[index];
                    if (child.Level <= message.Level)
                        break;
                    children.Add(child);
                }
            }
            return children;
        }

        /// <summary>
        /// Returns an enumerator for iterating over the message collection.
        /// </summary>
        /// <returns>An enumerator for CIXMessages</returns>
        public IEnumerator<CIXMessage> GetEnumerator()
        {
            return _messages.GetEnumerator();
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
        /// Adds the specified message to the collection.
        /// </summary>
        /// <param name="message">A CIXMessage to add</param>
        public void Add(CIXMessage message)
        {
            bool isNew = AddInternal(message);

            if (message.Unread)
            {
                Folder topic = message.Topic;
                topic.Unread += 1;
                if (message.Priority)
                {
                    topic.UnreadPriority += 1;
                }
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(topic);
                }
            }
            if (isNew)
            {
                CIX.FolderCollection.NotifyMessageAdded(message);
            }
            else
            {
                CIX.FolderCollection.NotifyMessageChanged(message);
            }
            CIX.FolderCollection.NotifyFolderUpdated(message.Topic);
        }

        /// <summary>
        /// Delete the specified message from the database and the list.
        /// </summary>
        /// <param name="message">The CIXMessage to delete</param>
        public void Delete(CIXMessage message)
        {
            CIXMessage realMessage = message.RealMessage;
            if (_messages.Contains(realMessage))
            {
                lock (_changeLock)
                {
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Delete(realMessage);
                    }
                    _messages.Remove(realMessage);
                    CIX.FolderCollection.NotifyMessageDeleted(message);
                    CIX.FolderCollection.NotifyFolderUpdated(message.Topic);
                }
            }
        }

        /// <summary>
        /// Return the cached message that corresponds to the given message ID.
        /// </summary>
        /// <param name="messageID">The ID of the message to locate</param>
        /// <returns>The corresponding message, or null</returns>
        public CIXMessage MessageByID(int messageID)
        {
            foreach (CIXMessage message in OrderedMessages)
            {
                if (message.RemoteID == messageID)
                {
                    return message;
                }
                if (!message.IsPseudo && message.RemoteID > messageID)
                {
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds the specified message to the collection.
        /// </summary>
        /// <param name="message">A CIXMessage to add</param>
        internal bool AddInternal(CIXMessage message)
        {
            lock (_changeLock)
            {
                bool isNew = false;
                if (message.RemoteID == 0)
                {
                    message.RemoteID = GetPseudoID();
                }
                if (message.ID > 0)
                {
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(message);
                    }
                }
                else
                {
                    if (MessageByID(message.RemoteID) != null)
                    {
                        Debug.Fail("Duplicate messages!");
                    }
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Insert(message);
                    }
                    _messages.Add(message);

                    int lastRemoteID = _messages.Count > 0 ? _messages[_messages.Count - 1].RemoteID : 0;
                    _isOrdered = message.RemoteID > lastRemoteID;
                    _threadedMessages = null;

                    isNew = true;
                }
                return isNew;
            }
        }

        /// <summary>
        /// Return a pseudo ID value to assign to this new message.
        /// </summary>
        /// <returns>A new pseudo ID</returns>
        private int GetPseudoID()
        {
            if (_messages.Count == 0)
            {
                return int.MaxValue / 2;
            }
            int pseudoID = _messages[_messages.Count - 1].RemoteID + 1;
            return Math.Max(pseudoID, int.MaxValue / 2);
        }

        /// <summary>
        /// Return the latest of either the date object or the specified new date.
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="newDate"></param>
        /// <returns></returns>
        private static DateTime LatestOf(DateTime datetime, DateTime newDate)
        {
            return (datetime < newDate) ? newDate : datetime;
        }
    }
}