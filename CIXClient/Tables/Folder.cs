// *****************************************************
// CIXReader
// Folder.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/09/2013 10:57 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CIXClient.Collections;
using CIXClient.Database;
using CIXClient.Models;

namespace CIXClient.Tables
{
    /// <summary>
    /// The Folder class represents a single folder on the remote system.
    /// </summary>
    public sealed class Folder : IComparable
    {
        private CIXMessageCollection _allMessages;
        private Folder _parentFolder;
        private int _parentID;
        private bool _isFolderRefreshing;
        private readonly Object _allMessagesLock = new Object();

        /// <summary>
        /// An unique ID that identifies this folder.
        /// </summary>
        [PrimaryKey]
        public int ID { get; set; }

        /// <summary>
        /// The local name of the folder.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The ID of the parent folder. If this is a root folder, the
        /// parent ID value will be -1.
        /// </summary>
        public int ParentID
        {
            get { return _parentID;  }
            set { 
                _parentID = value;
                _parentFolder = null;
            }
        }

        /// <summary>
        /// The folder flags.
        /// </summary>
        public FolderFlags Flags { get; set; }

        /// <summary>
        /// Index of the item within the parent collection.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The number of unread messages in the folder
        /// </summary>
        public int Unread { get; set; }

        /// <summary>
        /// The number of unread priority messages in the folder
        /// </summary>
        public int UnreadPriority { get; set; }

        /// <summary>
        /// A resign action is pending.
        /// </summary>
        public bool ResignPending { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether this folder
        /// has been modified.
        /// </summary>
        public bool MarkReadRangePending { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether this folder
        /// has been modified.
        /// </summary>
        [Ignore]
        public bool IsModified { get; set; }

        /// <summary>
        /// Return the parent of this folder
        /// </summary>
        [Ignore]
        public Folder ParentFolder
        {
            get { return _parentFolder ?? (_parentFolder = CIX.FolderCollection.Parent(this)); }
        }

        /// <summary>
        /// Return the list of child folders of this folder
        /// </summary>
        [Ignore]
        public IEnumerable<Folder> Children
        {
            get
            {
                return CIX.FolderCollection.Children(this);
            }
        }

        /// <summary>
        /// Flag that indicates whether this folder needs to be refreshed
        /// </summary>
        [Ignore]
        internal bool RefreshRequired { get; set; }

        /// <summary>
        /// Return whether or not this folder has been resigned.
        /// </summary>
        [Ignore]
        public bool IsResigned
        {
            get { return Flags.HasFlag(FolderFlags.Resigned); }
        }

        /// <summary>
        /// Return whether the user can resign this folder.
        /// </summary>
        [Ignore]
        public bool CanResign
        {
            get { return !Flags.HasFlag(FolderFlags.CannotResign); }
        }

        /// <summary>
        /// True if the messages for this folder have been loaded
        /// </summary>
        [Ignore]
        public bool HasMessages
        {
            get { return _allMessages != null; }
        }

        /// <summary>
        /// Gets a collection of all messages in this folder. Messages are retrieved from
        /// the database and cached locally.
        /// </summary>
        [Ignore]
        public CIXMessageCollection Messages
        {
            get
            {
                if (_allMessages == null)
                {
                    lock (_allMessagesLock)
                    {
                        if (!IsRootFolder)
                        {
                            _allMessages = new CIXMessageCollection(CIX.DB.Table<CIXMessage>().Where(msg => msg.TopicID == ID).ToArray());

                            // Fix up unread and unread priority counts
                            int unreadCount = 0;
                            int unreadPriorityCount = 0;

                            foreach (CIXMessage message in _allMessages)
                            {
                                if (message.Unread)
                                {
                                    ++unreadCount;
                                    if (message.Priority)
                                    {
                                        ++unreadPriorityCount;
                                    }
                                }
                            }
                            if (unreadCount != Unread || unreadPriorityCount != UnreadPriority)
                            {
                                Unread = unreadCount;
                                UnreadPriority = unreadPriorityCount;
                                lock (CIX.DBLock)
                                {
                                    CIX.DB.Update(this);
                                }
                                CIX.FolderCollection.NotifyFolderUpdated(this);
                            }
                        }
                        else
                        {
                            // All messages in all topics within this forum.
                            int[] subFolders = SubFolderIDs();
                            _allMessages = new CIXMessageCollection(CIX.DB.Table<CIXMessage>().Where(msg => subFolders.Contains(msg.TopicID)).ToArray());
                        }
                    }
                }
                return _allMessages;
            }
        }

        /// <summary>
        /// Do a fixup scan of the messages in the folder and make a request to
        /// retrieve any missing ones.
        /// </summary>
        public void Fixup()
        {
            List<Range> listOfRanges = new List<Range>();
            int lastMessageID = 0;

            foreach (CIXMessage message in _allMessages.OrderedMessages)
            {
                if (!message.IsPseudo && message.RemoteID != lastMessageID + 1)
                {
                    Range newRange = new Range
                    {
                        TopicName = Name,
                        ForumName = ParentFolder.Name,
                        Start = lastMessageID + 1,
                        End = message.RemoteID - 1
                    };
                    listOfRanges.Add(newRange);
                }
                lastMessageID = message.RemoteID;
            }
            if (listOfRanges.Count > 0)
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        HttpWebRequest wrGeturl = APIRequest.Post("forums/messagerange", APIRequest.APIFormat.XML, listOfRanges);
                        Stream objStream = APIRequest.ReadResponse(wrGeturl);
                        if (objStream != null)
                        {
                            DateTime sinceDate = CIX.LastSyncDate;
                            int newMessages = FolderCollection.AddMessages(objStream, ref sinceDate, true, true);

                            if (newMessages > 0)
                            {
                                LogFile.WriteLine("{0}/{1} refreshed with {2} new messages", ParentFolder.Name, Name, newMessages);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CIX.ReportServerExceptions("Folder.Fixup", e);
                    }
                });
                t.Start();
            }
        }

        /// <summary>
        /// Delete this folder.
        /// </summary>
        public void Delete()
        {
            DeleteAll();

            IEnumerable<Folder> topicsList = CIX.FolderCollection.Where(fld => fld.ParentID == ID).ToList();
            foreach (Folder topic in topicsList)
            {
                CIX.FolderCollection.Delete(topic);
            }
            CIX.FolderCollection.Delete(this);

            CIX.LastSyncDate = default(DateTime);

            LogFile.WriteLine("Folder {0} deleted", Name);

            CIX.FolderCollection.NotifyFolderDeleted(this);
        }

        /// <summary>
        /// Return whether this folder has a pending action
        /// </summary>
        public bool HasPending
        {
            get { return ResignPending; }
        }

        /// <summary>
        /// Sync this folder with the server.
        /// </summary>
        public void Sync()
        {
            if (ResignPending)
            {
                ResignFolder();
            }
        }

        /// <summary>
        /// Refresh this topic from the server.
        /// </summary>
        public void Refresh()
        {
            Thread t = new Thread(InternalRefresh);
            t.Start();
        }

        /// <summary>
        /// Resign this folder on the server.
        /// </summary>
        public void Resign()
        {
            if (CanResign)
            {
                ResignFolder();
            }
        }

        /// <summary>
        /// Mark all messages read in the specified folder and any sub-folders.
        /// </summary>
        public void MarkAllRead()
        {
            Thread t = new Thread(() =>
            {
                List<Folder> foldersUpdated = new List<Folder>();
                lock (CIX.DBLock)
                {
                    CIX.DB.RunInTransaction(() =>
                    {
                        if (!IsRootFolder)
                        {
                            if (InternalMarkAllRead())
                            {
                                foldersUpdated.Add(this);
                            }
                        }
                        else
                        {
                            foldersUpdated.AddRange(
                                Children.Where(topic => topic.Unread > 0 && topic.InternalMarkAllRead()));
                        }
                    });
                }
                foreach (Folder folder in foldersUpdated)
                {
                    CIX.FolderCollection.NotifyFolderRefreshed(new FolderEventArgs {Folder = folder});
                }
            });
            t.Start();
        }

        /// <summary>
        /// Gets a value that indicates whether this is a top level folder.
        /// </summary>
        [Ignore]
        public bool IsRootFolder { get { return ParentID == -1; } }

        /// <summary>
        /// Gets whether this folder is read-only.
        /// </summary>
        [Ignore]
        public bool IsReadOnly { get { return Flags.HasFlag(FolderFlags.ReadOnly); } }

        /// <summary>
        /// Returns whether or not this is a recent folder
        /// </summary>
        [Ignore]
        public bool IsRecent
        {
            get { return (Flags & FolderFlags.Recent) == FolderFlags.Recent; }
            set
            {
                if (value)
                {
                    Flags |= FolderFlags.Recent;
                }
                else
                {
                    Flags &= ~FolderFlags.Recent;
                }
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
        }

        /// <summary>
        /// Compare two folder names.
        /// </summary>
        public int CompareTo(object obj)
        {
            Folder otherFolder = (Folder) obj;
            return String.Compare(Name, otherFolder.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns an array of IDs of sub-folders of which this folder is the parent.
        /// </summary>
        private int[] SubFolderIDs()
        {
            List<int> subFolders = new List<int>();

            // Disable ReSharper's suggestion to make this into a Linq query because that actually
            // does NOT work and returns an array of zero integers.
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Folder fld in CIX.DB.Table<Folder>().Where(fld => fld.ParentID == ID))
            {
                subFolders.Add(fld.ID);
            }
            return subFolders.ToArray();
        }

        /// <summary>
        /// Delete all messages from this folder.
        /// </summary>
        internal void DeleteAll()
        {
            TableMapping map = CIX.DB.GetMapping<CIXMessage>();
            if (map != null)
            {
                TableMapping.Column column = map.FindColumnWithPropertyName("TopicID");
                StringBuilder deleteCmd = new StringBuilder();
                if (IsRootFolder)
                {
                    int[] subFolders = SubFolderIDs();
                    if (subFolders.Count() == 0)
                    {
                        return;
                    }
                    deleteCmd.AppendFormat("delete from {0} where ", map.TableName);
                    for (int c = 0; c < subFolders.Count(); ++c)
                    {
                        if (c > 0)
                        {
                            deleteCmd.Append(" or ");
                        }
                        deleteCmd.AppendFormat("{0}={1}", column.Name, subFolders[c]);
                    }
                }
                else
                {
                    deleteCmd.AppendFormat("delete from {0} where {1}={2}", map.TableName, column.Name, ID);
                }
                lock (CIX.DBLock)
                {
                    CIX.DB.Execute(deleteCmd.ToString());
                }
            }
            Invalidate();
        }

        /// <summary>
        /// Refresh this topic from the server
        /// </summary>
        internal void InternalRefresh()
        {
            if (CIX.Online & !_isFolderRefreshing)
            {
                _isFolderRefreshing = true;
                CIX.FolderCollection.NotifyTopicUpdateStarted(this);

                try
                {
                    // When refreshing, get the last 30 days worth unless the topic is empty in which
                    // case we get everything. The assumption is that if any changes occur with the
                    // read/unread status on the server then 30 days back is enough to sync those
                    // with the local database.
                    DateTime latestReply = new DateTime(1900, 1, 1);
                    if (Messages.Count > 0)
                    {
                        latestReply = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
                    }

                    // First clear the flag so we don't over-refresh
                    RefreshRequired = false;

                    Folder forum = ParentFolder;
                    int countOfNewMessages = 0;

                    string urlFormat = string.Format("forums/{0}/{1}/allmessages",
                        FolderCollection.EncodeForumName(forum.Name), Name);

                    HttpWebRequest wrGeturl = APIRequest.GetWithQuery(urlFormat, APIRequest.APIFormat.XML,
                        "maxresults=5000&since=" + latestReply.ToString("yyyy-MM-dd HH:mm:ss"));
                    using (Stream objStream = APIRequest.ReadResponse(wrGeturl))
                    {
                        if (objStream != null)
                        {
                            countOfNewMessages = FolderCollection.AddMessages(objStream, ref latestReply, false, false);
                        }
                    }
                    if (countOfNewMessages > 0)
                    {
                        LogFile.WriteLine("{0}/{1} refreshed with {2} new messages", forum.Name, Name,
                            countOfNewMessages);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("Folder.Refresh", this, e);
                }

                CIX.FolderCollection.NotifyTopicUpdateCompleted(this);
                _isFolderRefreshing = false;
            }
        }


        /// <summary>
        /// Invalidate the folder cache, forcing it to be reloaded on the reference.
        /// </summary>
        private void Invalidate()
        {
            _allMessages = null;
        }

        /// <summary>
        /// Mark all messages read in this topic.
        /// Must be called while holding a lock on CIX.DBLock!
        /// </summary>
        private bool InternalMarkAllRead()
        {
            List<CIXMessage> messagesUpdated = new List<CIXMessage>();

            int countMarkedRead = 0;
            int countMarkedPriorityRead = 0;

            foreach (CIXMessage message in Messages.Where(msg => msg.Unread))
            {
                if (!message.ReadLocked)
                {
                    message.Unread = false;
                    message.ReadPending = true;
                    ++countMarkedRead;
                    if (message.Priority)
                    {
                        ++countMarkedPriorityRead;
                    }
                    messagesUpdated.Add(message);
                }
            }
            CIX.DB.UpdateAll(messagesUpdated);

            Unread -= countMarkedRead;
            UnreadPriority -= countMarkedPriorityRead;

            if (countMarkedRead > 0)
            {
                MarkReadRangePending = true;
                CIX.DB.Update(this);
            }
            return countMarkedRead > 0;
        }

        /// <summary>
        /// Resign this folder on the server.
        /// </summary>
        private void ResignFolder()
        {
            if (!CIX.Online)
            {
                ResignPending = true;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
                return;
            }

            try
            {
                string url;
                if (IsRootFolder)
                {
                    LogFile.WriteLine("Resigning forum {0}", Name);
                    url = "forums/" + FolderCollection.EncodeForumName(Name) + "/resign";
                }
                else
                {
                    Folder forum = ParentFolder;
                    LogFile.WriteLine("Resigning topic {0}/{1}", forum.Name, Name);
                    url = "forums/" + FolderCollection.EncodeForumName(forum.Name) + "/" + FolderCollection.EncodeForumName(Name) + "/resigntopic";
                }

                HttpWebRequest wrGeturl = APIRequest.Get(url, APIRequest.APIFormat.XML);

                // Whatever happens, clear the pending action so we don't keep trying to
                // resign the forum repeatedly.
                ResignPending = false;
                Flags |= FolderFlags.Resigned;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }

                string responseString = APIRequest.ReadResponseString(wrGeturl);
                if (responseString == "Success")
                {
                    LogFile.WriteLine("Successfully resigned from {0}", Name);

                    CIX.FolderCollection.NotifyFolderUpdated(this);
                }
                else
                {
                    LogFile.WriteLine("Error resigning {0}. Response is: {1}", Name, responseString);
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("Folder.ResignFolder", this, e);
            }
        }

    }

    /// <summary>
    /// Folder flags
    /// </summary>
    [Flags]
    public enum FolderFlags
    {
        /// <summary>
        /// Indicates that this is a read-only folder.
        /// </summary>
        ReadOnly = 1,

        /// <summary>
        /// Indicates that the user resigned this forum.
        /// </summary>
        Resigned = 2,

        /// <summary>
        /// Indicates that the user cannot resign from this forum.
        /// </summary>
        CannotResign = 4,

        /// <summary>
        /// Indicates that the user cannot comment to messages in this
        /// topic unless to a message they posted.
        /// </summary>
        OwnerCommentsOnly = 8,

        /// <summary>
        /// Indicates that the last attempt to join the forum failed due
        /// to an unspecified error.
        /// </summary>
        JoinFailed = 16,

        /// <summary>
        /// Indicates that, for a folder, this is a recent folder displayed
        /// when filtering by recent only.
        /// </summary>
        Recent = 32
    }
}