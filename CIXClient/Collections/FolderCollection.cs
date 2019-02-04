// *****************************************************
// CIXReader
// FolderCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 02/10/2013 3:19 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using CIXClient.Database;
using CIXClient.Models;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// The FolderCollection class manages a collection of folders.
    /// </summary>
    public sealed class FolderCollection : IEnumerable<Folder>
    {
        /// <summary>
        /// Topic is read-only
        /// </summary>
        public const int UserForumTopicFlagsReadOnly = 0x0002;

        /// <summary>
        /// Forum cannot be resigned
        /// </summary>
        public const int UserForumTopicFlagsCannotResign = 0x0008;

        /// <summary>
        /// Forum is a noticeboard type
        /// </summary>
        public const int UserForumTopicFlagsNoticeboard = 0x0010;

        // Used to lock access to the internal ID
        private readonly object idLock = new object();

        private Dictionary<int, Folder> _allFolders;
        private bool _isInRefresh;
        private int _nextId = 1;

        /// <summary>
        /// Defines the delegate for FolderUpdated event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional folder update data</param>
        public delegate void FolderUpdatedHandler(object sender, FolderEventArgs e);

        /// <summary>
        /// Defines the delegate for FolderAdded event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional folder update data</param>
        public delegate void FoldersAddedHandler(object sender, FoldersAddedEventArgs e);

        /// <summary>
        /// Defines the delegate for FolderDeleted event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="folder">The folder object</param>
        public delegate void FolderDeletedHandler(object sender, Folder folder);

        /// <summary>
        /// Defines the delegate for FolderRefreshed event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional folder update data</param>
        public delegate void FolderRefreshedHandler(object sender, FolderEventArgs e);

        /// <summary>
        /// Defines the delegate for ForumUpdateStartedHandler event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional forum update data</param>
        public delegate void ForumUpdateStartedHandler(object sender, EventArgs e);

        /// <summary>
        /// Defines the delegate for ForumUpdateCompleted event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional forum update data</param>
        public delegate void ForumUpdateCompletedHandler(object sender, EventArgs e);

        /// <summary>
        /// Defines the delegate for TopicUpdateStartedHandler event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional forum update data</param>
        public delegate void TopicUpdateStartedHandler(object sender, FolderEventArgs e);

        /// <summary>
        /// Defines the delegate for TopicUpdateCompletedHandler event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional forum update data</param>
        public delegate void TopicUpdateCompletedHandler(object sender, FolderEventArgs e);

        /// <summary>
        /// Defines the delegate for MessagePostStarted and MessagePostCompleted event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Additional forum update data</param>
        public delegate void MessagePostHandler(object sender, MessagePostEventArgs e);

        /// <summary>
        /// Defines the delegate for the MessageDeleted event notification
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="message">The message that was deleted</param>
        public delegate void MessageDeletedHandler(object sender, CIXMessage message);

        /// <summary>
        /// Defines the delegate for the MessageChanged event notification
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="message">The message that changed</param>
        public delegate void MessageChangedHandler(object sender, CIXMessage message);

        /// <summary>
        /// Defines the delegate for the ThreadChanged event notification
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="message">The root message of the thread that changed</param>
        public delegate void ThreadChangedHandler(object sender, CIXMessage message);

        /// <summary>
        /// Defines the delegate for AccountUpdated event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Account event arguments</param>
        public delegate void AccountUpdatedHandler(object sender, AccountEventArgs e);

        /// <summary>
        /// Defines the delegate for OnlineUsersUpdated event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Online users event arguments</param>
        public delegate void OnlineUsersUpdatedHandler(object sender, OnlineUsersEventArgs e);

        /// <summary>
        /// Defines the delegate for InterestingThreadsUpdated event notifications.
        /// </summary>
        /// <param name="sender">The FolderCollection object</param>
        /// <param name="e">Interesting threads event arguments</param>
        public delegate void InterestingThreadsUpdatedHandler(object sender, InterestingThreadsEventArgs e);

        /// <summary>
        /// Event handler for notifying a delegate that a folder has been updated.
        /// </summary>
        public event FolderUpdatedHandler FolderUpdated;

        /// <summary>
        /// Event handler for notifying a delegate that a folder has been added.
        /// </summary>
        public event FoldersAddedHandler FoldersAdded;

        /// <summary>
        /// Event handler for notifying a delegate that a folder has been deleted.
        /// </summary>
        public event FolderDeletedHandler FolderDeleted;

        /// <summary>
        /// Event handler for notifying a delegate that multiple folders have been updated.
        /// </summary>
        public event FolderRefreshedHandler FolderRefreshed;

        /// <summary>
        /// Event handler for notifying a delegate that message has been deleted.
        /// </summary>
        public event MessageDeletedHandler MessageDeleted;

        /// <summary>
        /// Event handler for notifying a delegate that a message has been added.
        /// </summary>
        public event MessageChangedHandler MessageAdded;

        /// <summary>
        /// Event handler for notifying a delegate that a message has been changed.
        /// </summary>
        public event MessageChangedHandler MessageChanged;

        /// <summary>
        /// Event handler for notifying a delegate that a message has been changed.
        /// </summary>
        public event ThreadChangedHandler ThreadChanged;

        /// <summary>
        /// Event handler for notifying a delegate that an topic update has started.
        /// </summary>
        public event TopicUpdateStartedHandler TopicUpdateStarted;

        /// <summary>
        /// Event handler for notifying a delegate that an topic update has completed.
        /// </summary>
        public event MessagePostHandler MessagePostStarted;

        /// <summary>
        /// Event handler for notifying a delegate that an topic update has started.
        /// </summary>
        public event MessagePostHandler MessagePostCompleted;

        /// <summary>
        /// Event handler for notifying a delegate that an topic update has completed.
        /// </summary>
        public event TopicUpdateCompletedHandler TopicUpdateCompleted;

        /// <summary>
        /// Event handler for notifying a delegate that an forum list update has started.
        /// </summary>
        public event ForumUpdateStartedHandler ForumUpdateStarted;

        /// <summary>
        /// Event handler for notifying a delegate that an forum list update has completed.
        /// </summary>
        public event ForumUpdateCompletedHandler ForumUpdateCompleted;

        /// <summary>
        /// Event handler for notifying a delegate that the user account details have been updated.
        /// </summary>
        public event AccountUpdatedHandler AccountUpdated;

        /// <summary>
        /// Event handler for notifying a delegate that the list of online users has been updated.
        /// </summary>
        public event OnlineUsersUpdatedHandler OnlineUsersUpdated;

        /// <summary>
        /// Event handler for notifying a delegate that the list of interesting threads has changed.
        /// </summary>
        public event InterestingThreadsUpdatedHandler InterestingThreadsUpdated;

        /// <summary>
        /// Gets the collection of forum folders.
        /// </summary>
        public IEnumerable<Folder> Folders
        {
            get
            {
                return AllFolders.Values;
            }
        }

        /// <summary>
        /// Gets the total number of unread messages in all folders.
        /// </summary>
        public int TotalUnread
        {
            get { return Folders.Where(fld => fld.ParentID != -1).Sum(fld => fld.Unread); }
        }

        /// <summary>
        /// Gets the total number of unread priority messages in all folders.
        /// </summary>
        public int TotalUnreadPriority
        {
            get { return Folders.Where(fld => fld.ParentID != -1).Sum(fld => fld.UnreadPriority); }
        }

        /// <summary>
        /// Gets a dictionary of folders, mapped by folder ID.
        /// </summary>
        private Dictionary<int, Folder> AllFolders
        {
            get
            {
                if (_allFolders == null)
                {
                    lock (idLock)
                    {
                        // ReSharper disable once PossibleMultipleEnumeration
                        Folder[] folders = CIX.DB.Table<Folder>().ToArray();
                        _allFolders = folders.ToDictionary(msg => msg.ID);

                        // ReSharper disable once PossibleMultipleEnumeration
                        if (_allFolders.Count > 0)
                        {
                            _nextId = folders.Max(fld => fld.ID) + 1;
                        }
                    }
                }
                return _allFolders;
            }
        }

        /// <summary>
        /// Gets the Folder indexed by the specified ID, or null if no
        /// folder with that ID exists.
        /// </summary>
        /// <param name="id">Folder ID</param>
        /// <returns>A Folder, or null</returns>
        public Folder this[int id]
        {
            get
            {
                Folder folder;
                return AllFolders.TryGetValue(id, out folder) ? folder : null;
            }
        }

        /// <summary>
        /// Encode the given forum name for safe transmission in a URL path.
        /// </summary>
        /// <param name="forumName">Raw forum name to encode</param>
        /// <returns>Encoded forum name</returns>
        public static string EncodeForumName(string forumName)
        {
            return forumName.Replace(".", "~");
        }

        /// <summary>
        /// Return all messages from the database that match the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria to match</param>
        /// <returns>A CIXMessageCollection containing messages from the database that match the criteria</returns>
        public static CIXMessageCollection MessagesWithCriteria(Expression<Func<CIXMessage, bool>> criteria)
        {
            return new CIXMessageCollection(CIX.DB.Table<CIXMessage>().Where(criteria));
        }

        /// <summary>
        /// Apply the specified rule to all messages
        /// </summary>
        /// <param name="ruleGroup">Rule group to be applied</param>
        public static void ApplyRules(RuleGroup ruleGroup)
        {
            Thread t = new Thread(() =>
            {
                CIXMessageCollection messages = MessagesWithCriteria(ruleGroup.Criteria);
                List<Folder> modifiedFolders = new List<Folder>();
                foreach (CIXMessage message in messages)
                {
                    CIXMessage realMessage = message.RealMessage;
                    if (RuleCollection.ApplyRule(ruleGroup, realMessage))
                    {
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(realMessage);
                        }
                        if (!modifiedFolders.Contains(realMessage.Topic))
                        {
                            modifiedFolders.Add(realMessage.Topic);
                        }
                    }
                }
                foreach (Folder folder in modifiedFolders)
                {
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(folder);
                    }
                    CIX.FolderCollection.NotifyFolderUpdated(folder);
                }
            });
            t.Start();
        }

        /// <summary>
        /// Return a list of interesting threads. An event is fired at the 
        /// end to alert the caller that the list is available.
        /// </summary>
        public static void RefreshInterestingThreads()
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    LogFile.WriteLine("Refreshing latest list of interesting threads");

                    HttpWebRequest wrGeturl = APIRequest.Get("forums/interestingthreads", APIRequest.APIFormat.XML);
                    Stream objStream = APIRequest.ReadResponse(wrGeturl);
                    if (objStream != null)
                    {
                        using (XmlReader reader = XmlReader.Create(objStream))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(InterestingThreadSet));
                            InterestingThreadSet threads = (InterestingThreadSet)serializer.Deserialize(reader);

                            // Notify about the new threads.
                            List<CIXThread> messages = threads.Messages.Select(thread => new CIXThread
                            {
                                Author = thread.Author,
                                Body = thread.Body,
                                Date = DateTimeFromCIXDate(thread.DateTime),
                                RemoteID = thread.RootID,
                                Topic = thread.Topic,
                                Forum = thread.Forum
                            }).ToList();
                            CIX.FolderCollection.NotifyInterestingThreadsUpdated(messages);
                        }
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("ForumCollection.RefreshInterestingThreads", e);
                }
            });
            t.Start();
        }

        /// <summary>
        /// Given a Message2 structure array, this method adds all messages within to the database.
        /// </summary>
        /// <param name="objStream">A data stream containing the XML to parse</param>
        /// <param name="sinceDate">A reference to a DateTime structure that is set to the latest message retrieved</param>
        /// <param name="fastSync">Specifies whether or not the messages were retrieved with fast sync</param>
        /// <param name="fixup">Specifies whether these messages were retrieved as part of a folder fixup call</param>
        /// <returns>The total number of new messages added</returns>
        public static int AddMessages(Stream objStream, ref DateTime sinceDate, bool fastSync, bool fixup)
        {
            // Since the results are quite large, use the fast XML parser instead of
            // XMLSerializer to construct the message objects.
            XmlTextReader reader = new XmlTextReader(objStream)
            {
                WhitespaceHandling = WhitespaceHandling.Significant
            };

            List<Folder> changedFolders = new List<Folder>();
            List<Folder> foldersToRefresh = new List<Folder>();

            Message2 message = new Message2();
            Folder previousTopic = null;
            int countOfNewMessages = 0;
            bool needFullSync = false;
            string lastUpdate = string.Empty;

            lock (CIX.DBLock)
            {
                DateTime localSinceDate = sinceDate;
                CIX.DB.RunInTransaction(() =>
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "Author":
                                    reader.Read();
                                    message.Author = reader.ReadString();
                                    break;

                                case "Body":
                                    reader.Read();
                                    message.Body = reader.ReadString();
                                    break;

                                case "DateTime":
                                    reader.Read();
                                    message.DateTime = reader.ReadString();
                                    break;

                                case "Forum":
                                    reader.Read();
                                    message.Forum = reader.ReadString();
                                    break;

                                case "ID":
                                    reader.Read();
                                    message.ID = reader.ReadContentAsInt();
                                    break;

                                case "Priority":
                                    reader.Read();
                                    message.Priority = reader.ReadContentAsBoolean();
                                    break;

                                case "ReplyTo":
                                    reader.Read();
                                    message.ReplyTo = reader.ReadContentAsInt();
                                    break;

                                case "RootID":
                                    reader.Read();
                                    message.RootID = reader.ReadContentAsInt();
                                    break;

                                case "Starred":
                                    reader.Read();
                                    message.Starred = reader.ReadContentAsBoolean();
                                    break;

                                case "LastUpdate":
                                    reader.Read();
                                    lastUpdate = reader.ReadString();
                                    break;

                                case "Topic":
                                    reader.Read();
                                    message.Topic = reader.ReadString();
                                    break;

                                case "Unread":
                                    reader.Read();
                                    message.Unread = reader.ReadContentAsBoolean();
                                    break;
                            }
                        }
                        else if (reader.Name == "Message2")
                        {
                            Folder forum = CIX.FolderCollection.Get(-1, message.Forum);
                            Folder topic = null;

                            if (forum != null)
                            {
                                topic = CIX.FolderCollection.Get(forum.ID, message.Topic);
                            }
                            if (topic == null)
                            {
                                needFullSync = true;
                                continue;
                            }
                            if (topic.Messages.Count == 0 && fastSync)
                            {
                                if (!foldersToRefresh.Contains(topic))
                                {
                                    foldersToRefresh.Add(topic);
                                }
                                continue;
                            }

                            CIXMessage cixMessage = topic.Messages.MessageByID(message.ID);
                            if (cixMessage != null)
                            {
                                bool oldState = cixMessage.Unread;

                                if (!cixMessage.ReadPending)
                                {
                                    cixMessage.Unread = message.Unread;
                                }
                                if (cixMessage.IsWithdrawn && cixMessage.Unread)
                                {
                                    cixMessage.Unread = false;
                                    cixMessage.ReadPending = true;
                                }
                                cixMessage.Starred = message.Starred;

                                if (oldState != cixMessage.Unread && !cixMessage.ReadLocked)
                                {
                                    topic.Unread += cixMessage.Unread ? 1 : -1;
                                    if (cixMessage.Priority)
                                    {
                                        topic.UnreadPriority += cixMessage.Unread ? 1 : -1;
                                        if (topic.UnreadPriority < 0)
                                        {
                                            topic.UnreadPriority = 0;
                                        }
                                    }
                                }

                                cixMessage.Body = message.Body;
                                CIX.DB.Update(cixMessage);
                            }
                            else
                            {
                                cixMessage = CIXMessageFromMessage(topic, message);
                                if (cixMessage != null)
                                {
                                    if (cixMessage.Unread)
                                    {
                                        ++topic.Unread;
                                        if (cixMessage.Priority)
                                        {
                                            ++topic.UnreadPriority;
                                        }
                                    }
                                    CIX.RuleCollection.ApplyRules(cixMessage);
                                    topic.Messages.AddInternal(cixMessage);

                                    if (cixMessage.Ignored)
                                    {
                                        IEnumerable<CIXMessage> children = topic.Messages.Children(cixMessage);
                                        foreach (CIXMessage child in children.Where(child => !child.Ignored))
                                        {
                                            child.InnerSetIgnore();
                                        }
                                    }

                                    if (cixMessage.Priority)
                                    {
                                        IEnumerable<CIXMessage> children = topic.Messages.Children(cixMessage);
                                        foreach (CIXMessage child in children.Where(child => !child.Priority))
                                        {
                                            child.InnerSetPriority();
                                        }
                                    }
                                }
                                ++countOfNewMessages;
                            }

                            DateTime lastUpdateDateTime;
                            if (DateTime.TryParseExact(lastUpdate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastUpdateDateTime))
                            {
                                if (lastUpdateDateTime > localSinceDate)
                                {
                                    localSinceDate = lastUpdateDateTime;
                                }
                            }

                            // Save the last topic if we switched
                            if (previousTopic != null && previousTopic != topic)
                            {
                                CIX.DB.Update(previousTopic);
                                changedFolders.Add(previousTopic);
                            }
                            previousTopic = topic;
                        }
                    }

                    // Save the final topic
                    if (previousTopic != null)
                    {
                        CIX.DB.Update(previousTopic);
                        changedFolders.Add(previousTopic);
                    }
                });

                sinceDate = localSinceDate;
            }

            // Set the last sync date to be one second past the most recent
            // message.
            CIX.LastSyncDate = needFullSync ? default(DateTime) : sinceDate.AddSeconds(1.0);

            // Finally, force a refresh on all folders
            foreach (Folder topic in foldersToRefresh)
            {
                topic.InternalRefresh();
            }

            // Bulk add the new and updated messages. No need to run these in
            // a transaction.
            foreach (Folder folder in changedFolders)
            {
                CIX.FolderCollection.NotifyFolderRefreshed(new FolderEventArgs { Fixup = fixup, Folder = folder });
            }
            return countOfNewMessages;
        }

        /// <summary>
        /// Returns a folder which matches the given path or null.
        /// </summary>
        /// <param name="name">The path name</param>
        /// <returns>The folder if it exists, null otherwise</returns>
        public Folder Get(string name)
        {
            string[] paths = name.Split('/');
            Folder folder = null;
            int parentID = -1;

            foreach (string path in paths)
            {
                folder = Folders.SingleOrDefault(p => p.Name == path && p.ParentID == parentID);
                if (folder == null)
                {
                    break;
                }
                parentID = folder.ID;
            }
            return folder;
        }

        /// <summary>
        /// Returns a folder which matches the given name under the specified parent, or null.
        /// </summary>
        /// <param name="parentID">The ID of the parent folder</param>
        /// <param name="name">The folder name</param>
        /// <returns>The folder if it exists, null otherwise</returns>
        public Folder Get(int parentID, string name)
        {
            return Folders.SingleOrDefault(p => p.Name == name && p.ParentID == parentID);
        }

        /// <summary>
        /// Returns an enumerator for iterating over the folder collection.
        /// </summary>
        /// <returns>An enumerator for Folder</returns>
        public IEnumerator<Folder> GetEnumerator()
        {
            return Folders.GetEnumerator();
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
        /// Return the parent folder for this folder
        /// </summary>
        /// <param name="thisFolder">A Folder</param>
        /// <returns>The parent folder, or NULL</returns>
        public Folder Parent(Folder thisFolder)
        {
            return (thisFolder.ParentID >= 0) ? AllFolders[thisFolder.ParentID] : null;
        }

        /// <summary>
        /// Return all child folders for the specified parent.
        /// </summary>
        /// <param name="thisFolder">A Folder</param>
        /// <returns>The parent folder, or NULL</returns>
        public IEnumerable<Folder> Children(Folder thisFolder)
        {
            IEnumerable<Folder> children;
            lock (idLock)
            {
                children = Folders.Where(fld => fld.ParentID == thisFolder.ID).ToList();
            }
            return children;
        }

        /// <summary>
        /// Add the specified folder to the collection and add to the database if it
        /// is not already present.
        /// </summary>
        /// <param name="folder">Folder to add</param>
        public void Add(Folder folder)
        {
            if (!Contains(folder.ParentID, folder.Name))
            {
                folder.ID = NextID();
                lock (CIX.DBLock)
                {
                    CIX.DB.Insert(folder);
                }
                AllFolders[folder.ID] = folder;
            }
        }

        /// <summary>
        /// Mark messages read or unread on the server based on their local state
        /// </summary>
        public static void SynchronizeReads()
        {
            MarkRangeRead("read");
            MarkRangeRead("unread");
        }

        /// <summary>
        /// Mark all messages in the database as read.
        /// </summary>
        public void MarkAllRead()
        {
            foreach (Folder folder in Folders.Where(folder => folder.Unread > 0))
            {
                folder.MarkAllRead();
            }
        }

        /// <summary>
        /// Call the synchronisation on the directory.
        /// </summary>
        internal void Sync()
        {
            if (CIX.CanRunTasks)
            {
                try
                {
                    foreach (Folder forum in Folders.Where(forum => forum.HasPending))
                    {
                        forum.Sync();
                    }
                    PostMessages();
                    StarMessages();
                    WithdrawMessages();
                    Refresh(true);
                    SynchronizeReads();
                    SynchronizeStars();
                    FlushFolders();
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("FolderCollection.Sync", e);
                }
            }
        }

        /// <summary>
        /// Send a notification that a thread has been changed
        /// </summary>
        /// <param name="message">The root message of the thread that has changed</param>
        internal void NotifyThreadChanged(CIXMessage message)
        {
            ThreadChanged?.Invoke(this, message);
        }

        /// <summary>
        /// Send a notification that a message has been changed
        /// </summary>
        /// <param name="message">The CIXMessage that has been changed</param>
        internal void NotifyMessageChanged(CIXMessage message)
        {
            MessageChanged?.Invoke(this, message);
        }

        /// <summary>
        /// Send a notification that a message has been added
        /// </summary>
        /// <param name="message">The CIXMessage that has been added</param>
        internal void NotifyMessageAdded(CIXMessage message)
        {
            MessageAdded?.Invoke(this, message);
        }

        /// <summary>
        /// Send a notification that a message has been deleted
        /// </summary>
        /// <param name="message">The CIXMessage that has been deleted</param>
        internal void NotifyMessageDeleted(CIXMessage message)
        {
            MessageDeleted?.Invoke(this, message);
        }

        /// <summary>
        /// Send a notification that a folder has been updated
        /// </summary>
        /// <param name="folder">The folder that has been updated</param>
        internal void NotifyFolderUpdated(Folder folder)
        {
            FolderUpdated?.Invoke(this, new FolderEventArgs { Folder = folder });
        }

        /// <summary>
        /// Send a notification that a folder has been refreshed
        /// </summary>
        /// <param name="args">A FolderEventArgs structure to be passed to the delegates</param>
        internal void NotifyFolderRefreshed(FolderEventArgs args)
        {
            FolderRefreshed?.Invoke(this, args);
        }

        /// <summary>
        /// Send a notification that one or more folder have been added
        /// </summary>
        /// <param name="folders">A list of folders that have been added</param>
        internal void NotifyFoldersAdded(List<Folder> folders)
        {
            FoldersAdded?.Invoke(this, new FoldersAddedEventArgs { Folders = folders });
        }

        /// <summary>
        /// Send a notification that a folder has been deleted
        /// </summary>
        /// <param name="folder">The folder that has been deleted</param>
        internal void NotifyFolderDeleted(Folder folder)
        {
            FolderDeleted?.Invoke(this, folder);
        }

        /// <summary>
        /// Send a notification that account details have been retrieved.
        /// </summary>
        /// <param name="accountDetails">An Account object that contains the account details</param>
        internal void NotifyAccountUpdated(Account accountDetails)
        {
            AccountUpdated?.Invoke(this, new AccountEventArgs { Account = accountDetails });
        }

        /// <summary>
        /// Send a notification that the list of online users has been refreshed.
        /// </summary>
        /// <param name="onlineUsers">A list of online users</param>
        internal void NotifyOnlineUsersUpdated(Whos onlineUsers)
        {
            OnlineUsersUpdated?.Invoke(this, new OnlineUsersEventArgs { Users = onlineUsers });
        }

        /// <summary>
        /// Send a notification that the list of interesting threads has changed.
        /// </summary>
        /// <param name="threads">A list of interesting threads</param>
        internal void NotifyInterestingThreadsUpdated(List<CIXThread> threads)
        {
            InterestingThreadsUpdated?.Invoke(this, new InterestingThreadsEventArgs { Threads = threads });
        }

        /// <summary>
        /// Send a notification that a topic update has commenced.
        /// </summary>
        /// <param name="folder">The folder that will be updated</param>
        internal void NotifyTopicUpdateStarted(Folder folder)
        {
            TopicUpdateStarted?.Invoke(this, new FolderEventArgs { Folder = folder });
        }

        /// <summary>
        /// Send a notification that a topic update has completed.
        /// </summary>
        /// <param name="folder">The folder which has been updated</param>
        internal void NotifyTopicUpdateCompleted(Folder folder)
        {
            TopicUpdateCompleted?.Invoke(this, new FolderEventArgs { Folder = folder });
        }

        /// <summary>
        /// Send a notification that a forum update has commenced.
        /// </summary>
        internal void NotifyForumUpdateStarted()
        {
            ForumUpdateStarted?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Send a notification that a forum update has completed.
        /// </summary>
        internal void NotifyForumUpdateCompleted()
        {
            ForumUpdateCompleted?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Send a notification that a message is going to be posted.
        /// </summary>
        /// <param name="message">The CIXMessage that will be posted</param>
        internal void NotifyMessagePostStarted(CIXMessage message)
        {
            MessagePostStarted?.Invoke(this, new MessagePostEventArgs { Message = message });
        }

        /// <summary>
        /// Send a notification that a message has been posted.
        /// </summary>
        /// <param name="message">The CIXMessage that has been posted</param>
        internal void NotifyMessagePostCompleted(CIXMessage message)
        {
            MessagePostCompleted?.Invoke(this, new MessagePostEventArgs { Message = message });
        }

        /// <summary>
        /// Refresh the local list of active forums to which the user is joined and for all
        /// the ones which have unread messages, simultaneously refresh those topics so they
        /// are downloaded as soon as the user navigates to them.
        /// </summary>
        /// <param name="useFastSync">A flag that specifies whether or not to use fast sync</param>
        internal void Refresh(bool useFastSync)
        {
            if (_isInRefresh)
            {
                return;
            }

            if (useFastSync && RefreshWithFastSync())
            {
                _isInRefresh = false;
                return;
            }

            _isInRefresh = true;
            CIX.FolderCollection.NotifyForumUpdateStarted();

            try
            {
                LogFile.WriteLine("Start refreshing all forums");

                CIX.LastSyncDate = DateTime.Now;

                // Only sync if we didn't do one earlier as part of
                // first-time initialisation.
                CIX.FolderCollection.RefreshWithSlowSync();

                // Force a refresh on all folders
                List<Folder> refreshList = CIX.FolderCollection.Where(fld => fld.RefreshRequired).ToList();
                foreach (Folder topic in refreshList)
                {
                    topic.InternalRefresh();
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("FolderCollection.Refresh", e);
            }

            LogFile.WriteLine("Finished refreshing all forums");

            CIX.FolderCollection.NotifyForumUpdateCompleted();
            _isInRefresh = false;
        }

        /// <summary>
        /// Synchronise the folder collection from the server.
        /// </summary>
        internal void RefreshWithSlowSync()
        {
            HttpWebRequest request = APIRequest.GetWithQuery("user/alltopics", APIRequest.APIFormat.XML, "maxresults=5000");
            Stream objStream = APIRequest.ReadResponse(request);
            if (objStream != null)
            {
                using (XmlReader reader = XmlReader.Create(objStream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(UserForumTopicResultSet2));
                    UserForumTopicResultSet2 listOfForums = (UserForumTopicResultSet2)serializer.Deserialize(reader);

                    List<Folder> modifiedFolders = new List<Folder>();
                    List<Folder> foldersToAdd = new List<Folder>();

                    StringCollection allForums = new StringCollection();

                    foreach (UserForumTopic2 forumTopic in listOfForums.UserTopics)
                    {
                        Folder forum = Get(-1, forumTopic.Forum);
                        FolderFlags folderFlags = FolderFlags.Recent; // Force all forums to show up for now

                        if ((forumTopic.Flags & UserForumTopicFlagsCannotResign) != 0)
                        {
                            folderFlags |= FolderFlags.CannotResign;
                        }

                        if (forum == null)
                        {
                            forum = new Folder
                            {
                                Name = forumTopic.Forum,
                                Flags = folderFlags,
                                ParentID = -1
                            };
                            AddInternal(forum);
                            foldersToAdd.Add(forum);
                        }
                        else if (forum.Flags != folderFlags || forum.Name != forumTopic.Forum)
                        {
                            forum.Flags = folderFlags;
                            forum.Name = forumTopic.Forum;
                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(forum);
                            }
                        }

                        allForums.Add(forum.Name);

                        Folder topic = Get(forum.ID, forumTopic.Topic);
                        folderFlags = 0;

                        if ((forumTopic.Flags & UserForumTopicFlagsReadOnly) != 0)
                        {
                            folderFlags |= FolderFlags.ReadOnly;
                        }
                        if (forumTopic.Latest == "Y")
                        {
                            folderFlags |= FolderFlags.Recent;
                        }
                        if ((forumTopic.Flags & UserForumTopicFlagsNoticeboard) != 0)
                        {
                            folderFlags |= FolderFlags.OwnerCommentsOnly;
                        }

                        if (topic == null)
                        {
                            topic = new Folder
                            {
                                Name = forumTopic.Topic,
                                ParentID = forum.ID,
                                Flags = folderFlags
                            };
                            AddInternal(topic);
                            foldersToAdd.Add(topic);
                        }
                        else if (topic.Flags != folderFlags || forumTopic.Topic != topic.Name)
                        {
                            topic.Name = forumTopic.Topic;
                            topic.Flags = folderFlags;
                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(topic);
                            }
                            modifiedFolders.Add(topic);
                        }

                        // Has the unread count changed on this topic?
                        if (topic.Unread != forumTopic.UnRead)
                        {
                            topic.RefreshRequired = true;
                        }
                    }

                    // Do a sanity pass - mark as resigned any forums we have locally which aren't
                    // resigned but aren't in the list we just got.
                    foreach (Folder folder in this.Where(fld => fld.IsRootFolder).Where(folder => !allForums.Contains(folder.Name)))
                    {
                        if (!folder.IsResigned)
                        {
                            LogFile.WriteLine("Forum {0} marked resigned locally as you're not joined to it on the server", folder.Name);
                            folder.Flags |= FolderFlags.Resigned;

                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(folder);
                            }
                            modifiedFolders.Add(folder);
                        }
                    }

                    // Bulk add new folders now
                    lock (CIX.DBLock)
                    {
                        CIX.DB.InsertAll(foldersToAdd);
                    }

                    if (foldersToAdd.Count > 0)
                    {
                        NotifyFoldersAdded(foldersToAdd);
                        LogFile.WriteLine("New forums added to local database");
                    }
                    if (modifiedFolders.Count > 0)
                    {
                        foreach (Folder folder in modifiedFolders)
                        {
                            NotifyFolderUpdated(folder);
                        }
                        LogFile.WriteLine("All forums updated");
                    }
                }
            }
        }

        /// <summary>
        /// Add the specified folder to the collection but not the database.
        /// </summary>
        /// <param name="folder">Folder to add</param>
        internal void AddInternal(Folder folder)
        {
            if (!Contains(folder.ParentID, folder.Name))
            {
                folder.ID = NextID();
                AllFolders[folder.ID] = folder;
            }
        }

        /// <summary>
        /// Deletes the specified folder from the database and the collection.
        /// </summary>
        /// <param name="folder">Folder to add</param>
        internal void Delete(Folder folder)
        {
            lock (CIX.DBLock)
            {
                CIX.DB.Delete(folder);
            }
            AllFolders.Remove(folder.ID);
        }

        /// <summary>
        /// Returns whether the specified folder exists in the collection.
        /// </summary>
        /// <param name="parentID">The ID of the parent folder</param>
        /// <param name="name">The folder name</param>
        /// <returns>True if the message exists, false otherwise</returns>
        internal bool Contains(int parentID, string name)
        {
            return Get(parentID, name) != null;
        }

        /// <summary>
        /// Set or remove flag on messages.
        /// </summary>
        private static void StarMessages()
        {
            TableQuery<CIXMessage> pending = CIX.DB.Table<CIXMessage>().Where(msg => msg.StarPending);
            foreach (CIXMessage message in pending)
            {
                message.RealMessage.Sync();
            }
        }

        /// <summary>
        /// Return a DateTime object representing the specified CIX date and time string.
        /// </summary>
        /// <param name="cixDate">A CIX date string</param>
        /// <returns>A DateTime object that represents the given date</returns>
        private static DateTime DateTimeFromCIXDate(string cixDate)
        {
            DateTime dateTime;
            if (!DateTime.TryParseExact(cixDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                LogFile.WriteLine("Found malformed date {0} in feed", cixDate);
            }
            return dateTime;
        }

        /// <summary>
        /// Construct a CIXMessage from a raw server Message object.
        /// </summary>
        /// <param name="topic">The topic for the message</param>
        /// <param name="message">The raw Message object from the server</param>
        /// <returns>A CIXMessage, or null if the message object was malformed.</returns>
        private static CIXMessage CIXMessageFromMessage(Folder topic, Message2 message)
        {
            CIXMessage cixMessage = new CIXMessage
            {
                Body = message.Body.TrimEnd(),
                Author = message.Author,
                Date = DateTimeFromCIXDate(message.DateTime),
                CommentID = message.ReplyTo,
                RootID = message.RootID,
                RemoteID = message.ID,
                Unread = message.Unread,
                Starred = message.Starred,
                Priority = message.Priority,
                TopicID = topic.ID
            };
            return cixMessage;
        }

        /// <summary>
        /// Refresh using Fast Sync.
        /// </summary>
        /// <returns>True if Fast Sync completed, False if we need to do a slow sync</returns>
        private static bool RefreshWithFastSync()
        {
            DateTime sinceDate = CIX.LastSyncDate;
            if (sinceDate == default(DateTime) || sinceDate < DateTime.Now.AddDays(-30.0))
            {
                return false;
            }
            try
            {
                HttpWebRequest request = APIRequest.GetWithQuery("user/sync", APIRequest.APIFormat.XML, "maxresults=5000&since=" + sinceDate.ToString("yyyy-MM-dd HH:mm:ss"));

                LogFile.WriteLine("Sync all forums started");

                Stream objStream = APIRequest.ReadResponse(request);
                if (objStream != null)
                {
                    int countOfNewMessages = AddMessages(objStream, ref sinceDate, true, false);
                    LogFile.WriteLine("Sync completed with {0} new messages", countOfNewMessages);
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("FolderCollection.RefreshWithFastSync", e);
            }
            return true;
        }

        /// <summary>
        /// Post any new messages to the server. New messages are those where RemoteID is 0, and we
        /// use the CommentID to determine whether this is a new message or a comment.
        /// </summary>
        private static void PostMessages()
        {
            TableQuery<CIXMessage> cixMessages = CIX.DB.Table<CIXMessage>().Where(fld => fld.PostPending);
            foreach (CIXMessage message in cixMessages)
            {
                message.RealMessage.Sync();
            }
        }

        /// <summary>
        /// Withdraw all messages.
        /// </summary>
        private static void WithdrawMessages()
        {
            TableQuery<CIXMessage> cixMessages = CIX.DB.Table<CIXMessage>().Where(fld => fld.WithdrawPending);
            foreach (CIXMessage message in cixMessages)
            {
                message.RealMessage.Sync();
            }
        }

        /// <summary>
        /// Mark a range of messages read or unread depending on the range type string.
        /// </summary>
        /// <param name="rangeType">A string set to either "read" or "unread"</param>
        private static void MarkRangeRead(string rangeType)
        {
            bool markAsRead = rangeType == "read";
            List<CIXMessage> cixMessages = CIX.DB.Table<CIXMessage>().Where(fld => fld.ReadPending && fld.Unread != markAsRead).ToList();
            IOrderedEnumerable<CIXMessage> orderedTopics = cixMessages.OrderBy(fld => fld.TopicID).ThenBy(fld => fld.RemoteID);

            List<Range> listOfRanges = new List<Range>();
            Range currentRange = null;
            int lastTopicID = -1;
            int lastRemoteID = -1;

            foreach (CIXMessage message in orderedTopics)
            {
                if (message.TopicID != lastTopicID)
                {
                    currentRange = null;
                }
                if (message.RemoteID != lastRemoteID + 1)
                {
                    currentRange = null;
                }
                if (currentRange == null)
                {
                    currentRange = new Range
                    {
                        TopicName = message.Topic.Name,
                        ForumName = message.Topic.ParentFolder.Name,
                        Start = message.RemoteID,
                        End = message.RemoteID
                    };
                    listOfRanges.Add(currentRange);
                    lastTopicID = message.TopicID;
                }
                currentRange.End = message.RemoteID;
                lastRemoteID = message.RemoteID;
            }
            if (listOfRanges.Count > 0)
            {
                try
                {
                    string url = string.Format("forums/{0}/markreadrange", markAsRead ? "true" : "false");
                    HttpWebRequest request = APIRequest.Post(url, APIRequest.APIFormat.XML, listOfRanges);
                    string responseString = APIRequest.ReadResponseString(request);
                    if (responseString != null && responseString == "Success")
                    {
                        int readCount = 0;
                        List<CIXMessage> messagesToUpdate = new List<CIXMessage>();
                        foreach (CIXMessage message in cixMessages)
                        {
                            CIXMessage realMessage = message.RealMessage;
                            realMessage.ReadPending = false;
                            messagesToUpdate.Add(realMessage);
                            ++readCount;
                        }
                        lock (CIX.DBLock)
                        {
                            CIX.DB.UpdateAll(messagesToUpdate);
                        }
                        LogFile.WriteLine("Marked {0} messages {1}", readCount, rangeType);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("FolderCollection.MarkRangeRead", e);
                }
            }
        }

        /// <summary>
        /// Refresh the starred message state from the server.
        /// </summary>
        private void SynchronizeStars()
        {
            try
            {
                HttpWebRequest request = APIRequest.Get("starred", APIRequest.APIFormat.XML);
                Stream objStream = APIRequest.ReadResponse(request);
                if (objStream != null)
                {
                    using (XmlReader reader = XmlReader.Create(objStream))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(StarSet));
                        StarSet allStars = (StarSet)serializer.Deserialize(reader);

                        List<Folder> topicsToRefresh = new List<Folder>();
                        List<CIXMessage> updatedMessages = new List<CIXMessage>();

                        foreach (StarSetStarsStar oneStar in allStars.Stars)
                        {
                            Folder forum = CIX.FolderCollection.Get(-1, oneStar.Conf);
                            if (forum != null)
                            {
                                Folder topic = CIX.FolderCollection.Get(forum.ID, oneStar.Topic);
                                if (topic != null)
                                {
                                    CIXMessage message = topic.Messages.MessageByID(oneStar.MsgID);
                                    if (message == null)
                                    {
                                        // If this message isn't local, retrieve it. The star will be
                                        // set on the message when it comes from the server.
                                        if (!topicsToRefresh.Contains(topic))
                                        {
                                            topicsToRefresh.Add(topic);
                                        }
                                    }
                                    else if (!message.Starred)
                                    {
                                        message.Starred = true;
                                        message.StarPending = false;
                                        updatedMessages.Add(message);
                                    }
                                }
                            }
                        }

                        // Batch commit updated messages
                        lock (CIX.DBLock)
                        {
                            CIX.DB.UpdateAll(updatedMessages);
                        }

                        // For topics which contain stars but which are empty, refresh them to
                        // retrieve the starred messages.
                        foreach (Folder topic in topicsToRefresh)
                        {
                            topic.InternalRefresh();
                        }

                        // Fire the event to notify of updated messages. Again, we need to run this
                        // outside of the transaction.
                        if (updatedMessages.Count > 0)
                        {
                            LogFile.WriteLine("Flag added to {0} messages", updatedMessages.Count);
                            foreach (CIXMessage message in updatedMessages)
                            {
                                CIX.FolderCollection.NotifyMessageChanged(message);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("FolderCollection.SynchronizeStars", e);
            }
        }

        /// <summary>
        /// Return the next available folder ID.
        /// </summary>
        /// <returns>The next folder ID value</returns>
        private int NextID()
        {
            lock (idLock)
            {
                return _nextId++;
            }
        }

        /// <summary>
        /// Flush all modified folders back to the database.
        /// </summary>
        private static void FlushFolders()
        {
            Folder[] modifiedFolders = CIX.FolderCollection.Where(fld => fld.IsModified).ToArray();
            if (modifiedFolders.Length > 0)
            {
                lock (CIX.DBLock)
                {
                    CIX.DB.UpdateAll(modifiedFolders);

                    // Clear the flag asap. There could be a race condition here
                    // unfortunately. Fix this.
                    foreach (Folder folder in modifiedFolders)
                    {
                        folder.IsModified = false;
                    }
                }
            }
        }
    }
}