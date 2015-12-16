// *****************************************************
// CIXReader
// CIXMessage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/09/2013 10:51 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using CIXClient.Collections;
using CIXClient.Database;
using CIXClient.Models;
using CIXClient.Properties;

namespace CIXClient.Tables
{
    /// <summary>
    /// The CIXMessage class encapsulates data about a single CIX message.
    /// </summary>
    public sealed class CIXMessage
    {
        private string _subject;
        private string _body;
        private CIXMessage _parent;
        private Folder _topic;

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
        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                _subject = null;
            }
        }

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
        /// Gets a subject from the first line, limited to 80 characters.
        /// </summary>
        [Ignore]
        public string Subject
        {
            get { return _subject ?? (_subject = (Body != null) ? Body.FirstNonBlankLine().TruncateByWordWithLimit(80) : string.Empty); }
        }

        /// <summary>
        /// Gets or sets the indent level of this message from the conversation root.
        /// </summary>
        [Ignore]
        public int Level { get; set; }

        /// <summary>
        /// Gets a value indicating whether the authenticated user was the author
        /// of this message.
        /// </summary>
        [Ignore]
        public bool IsMine
        {
            get { return Author == CIX.Username; }
        }

        /// <summary>
        /// Gets a value indicating whether this message is a local draft and not
        /// yet synced with the CIX server.
        /// </summary>
        [Ignore]
        public bool IsDraft
        {
            get { return IsPseudo && !IsPending; }
        }

        /// <summary>
        /// Gets a value indicating whether this message is does not yet have a
        /// server assigned ID value.
        /// </summary>
        [Ignore]
        public bool IsPseudo
        {
            get { return RemoteID >= int.MaxValue / 2; }
        }

        /// <summary>
        /// Gets a value indicating whether this message is pending a sync with
        /// the CIX server. If IsDraft is true but IsPending is false, the message is in
        /// the process of being edited and not ready to be synced.
        /// </summary>
        [Ignore]
        public bool IsPending
        {
            get { return PostPending; }
        }

        /// <summary>
        /// Gets a value indicating whether this message is a root message.
        /// </summary>
        [Ignore]
        public bool IsRoot
        {
            get { return CommentID == 0; }
        }

        /// <summary>
        /// Gets or sets the count of child messages to this thread.
        /// </summary>
        [Ignore]
        public int ChildCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this is a withdrawn message.
        /// </summary>
        [Ignore]
        public bool IsWithdrawn
        {
            get { return Body == Resources.WithdrawnByAuthor || Body == Resources.WithdrawnByModerator; }
        }

        /// <summary>
        /// Gets or sets the parent of this message
        /// </summary>
        [Ignore]
        public CIXMessage Parent
        {
            set { _parent = value; }
            get { return _parent ?? (_parent = CommentID > 0 ? Topic.Messages.MessageByID(CommentID) : null); }
        }

        /// <summary>
        /// Gets or sets the parent of this message
        /// </summary>
        [Ignore]
        public CIXMessage SafeParent
        {
            set { _parent = value; }
            get
            {
                return Parent ?? new CIXMessage();
            }
        }

        /// <summary>
        /// Gets the root message
        /// </summary>
        [Ignore]
        public CIXMessage Root
        {
            get
            {
                CIXMessage message = this;
                while (message.Level > 0)
                {
                    message = message.Parent;
                }
                return message;
            }
        }

        /// <summary>
        /// Gets the forum folder to which this message belongs.
        /// </summary>
        [Ignore]
        public Folder Forum
        {
            get { return Topic.ParentFolder; }
        }

        /// <summary>
        /// Gets the topic folder to which this message belongs.
        /// </summary>
        [Ignore]
        public Folder Topic
        {
            get { return _topic ?? (_topic = CIX.FolderCollection[TopicID]); }
        }

        /// <summary>
        /// Gets or sets the last message that is a child of this message
        /// </summary>
        [Ignore]
        public CIXMessage LastChildMessage { get; set; }

        /// <summary>
        /// Gets or sets the date of the latest message in this thread
        /// </summary>
        [Ignore]
        public DateTime LatestDate { get; set; }

        /// <summary>
        /// Gets the cached version of the specified message.
        /// </summary>
        [Ignore]
        public CIXMessage RealMessage
        {
            get
            {
                return (Topic != null && Topic.HasMessages) ? Topic.Messages.MessageByID(RemoteID) : this;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this message any child messages
        /// </summary>
        [Ignore]
        public bool HasChildren
        {
            get { return LastChildMessage != this; }
        }

        /// <summary>
        /// Gets the total number of unread child messages.
        /// </summary>
        [Ignore]
        public int UnreadChildren
        {
            get
            {
                return Topic.Messages.Children(this).Count(message => message.Unread);
            }
        }

        /// <summary>
        /// Post this message to the server.
        /// </summary>
        public void Post()
        {
            if (CIX.Online)
            {
                Thread t = new Thread(PostMessage);
                t.Start();
            }
            else
            {
                PostPending = true;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
        }

        /// <summary>
        /// Withdraw this message from the server.
        /// </summary>
        public void Withdraw()
        {
            if (CIX.Online)
            {
                Thread t = new Thread(WithdrawMessage);
                t.Start();
            }
            else
            {
                WithdrawPending = true;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
        }

        /// <summary>
        /// Mark this message as unread.
        /// </summary>
        public void MarkUnread()
        {
            if (!ReadLocked)
            {
                InnerMarkUnread();
            }
        }

        /// <summary>
        /// Mark this message as read.
        /// </summary>
        public void MarkRead()
        {
            if (!ReadLocked)
            {
                InnerMarkRead();
            }
        }

        /// <summary>
        /// Remove star from this message.
        /// </summary>
        public void RemoveStar()
        {
            Starred = false;
            StarPending = true;
            lock (CIX.DBLock)
            {
                CIX.DB.Update(this);
            }
            CIX.FolderCollection.NotifyMessageChanged(this);
            CIX.FolderCollection.NotifyFolderUpdated(Topic);
        }

        /// <summary>
        /// Add star to this message
        /// </summary>
        public void AddStar()
        {
            Starred = true;
            StarPending = true;
            lock (CIX.DBLock)
            {
                CIX.DB.Update(this);
            }
            CIX.FolderCollection.NotifyMessageChanged(this);
            CIX.FolderCollection.NotifyFolderUpdated(Topic);
        }

        /// <summary>
        /// Remove the read-lock flag on the message and mark it
        /// as read.
        /// </summary>
        public void ClearReadLock()
        {
            if (ReadLocked)
            {
                ReadLocked = false;
                InnerMarkRead();
            }
        }

        /// <summary>
        /// Set the read-lock flag on the message and mark it
        /// as unread.
        /// </summary>
        public void MarkReadLock()
        {
            if (!ReadLocked)
            {
                ReadLocked = true;
                InnerMarkUnread();
            }
        }

        /// <summary>
        /// Remove the Ignore flag from the message
        /// </summary>
        public void RemoveIgnore()
        {
            Folder topic = Topic;
            IEnumerable<CIXMessage> childMessages = topic.Messages.Children(this);

            lock (CIX.DBLock)
            {
                CIX.DB.RunInTransaction(() =>
                {
                    Ignored = false;
                    CIX.DB.Update(this);

                    foreach (CIXMessage child in childMessages)
                    {
                        child.Ignored = false;
                        CIX.DB.Update(child);
                    }
                });
            }
            CIX.FolderCollection.NotifyThreadChanged(this);
        }

        /// <summary>
        /// Set the Ignore flag on the message.
        /// </summary>
        public void SetIgnore()
        {
            lock (CIX.DBLock)
            {
                int unreadCount = 0;
                CIX.DB.RunInTransaction(() =>
                {
                    unreadCount = InnerSetIgnore();
                });
                if (unreadCount > 0)
                {
                    CIX.FolderCollection.NotifyFolderUpdated(Topic);
                }
            }
            CIX.FolderCollection.NotifyThreadChanged(this);
        }

        /// <summary>
        /// Set the Priority flag on the message
        /// </summary>
        public void SetPriority()
        {
            lock (CIX.DBLock)
            {
                CIX.DB.RunInTransaction(InnerSetPriority);
            }
            CIX.FolderCollection.NotifyThreadChanged(this);
        }

        /// <summary>
        /// Clear the priority flag from the message.
        /// </summary>
        public void ClearPriority()
        {
            Folder topic = Topic;
            IEnumerable<CIXMessage> childMessages = topic.Messages.Children(this);
            bool folderChanged = false;

            lock (CIX.DBLock)
            {
                CIX.DB.RunInTransaction(() =>
                {
                    Priority = false;
                    if (Unread && topic.UnreadPriority > 0)
                    {
                        topic.UnreadPriority -= 1;
                        folderChanged = true;
                    }
                    CIX.DB.Update(this);
                    foreach (CIXMessage child in childMessages)
                    {
                        child.Priority = false;
                        if (child.Unread && topic.UnreadPriority > 0)
                        {
                            topic.UnreadPriority -= 1;
                            folderChanged = true;
                        }
                        CIX.DB.Update(child);
                    }
                    if (folderChanged)
                    {
                        CIX.DB.Update(topic);
                    }
                });
            }
            CIX.FolderCollection.NotifyThreadChanged(this);
        }

        /// <summary>
        /// Mark the thread from the specified root as read.
        /// </summary>
        public void MarkThreadRead()
        {
            List<CIXMessage> messagesUpdated = new List<CIXMessage>();
            Folder topic = Topic;

            int countMarkedRead = 0;
            int countMarkedPriorityRead = 0;

            if (!ReadLocked && Unread)
            {
                Unread = false;
                ReadPending = true;
                ++countMarkedRead;
                if (Priority)
                {
                    ++countMarkedPriorityRead;
                }
                messagesUpdated.Add(this);
            }

            IEnumerable<CIXMessage> childMessages = topic.Messages.Children(this);
            foreach (CIXMessage child in childMessages)
            {
                if (!child.ReadLocked && child.Unread)
                {
                    child.Unread = false;
                    child.ReadPending = true;
                    ++countMarkedRead;
                    if (child.Priority)
                    {
                        ++countMarkedPriorityRead;
                    }
                    messagesUpdated.Add(child);
                }
            }

            lock (CIX.DBLock)
            {
                CIX.DB.UpdateAll(messagesUpdated);
            }

            if (countMarkedRead > 0)
            {
                topic.Unread -= countMarkedRead;
                if (topic.Unread < 0)
                {
                    topic.Unread = 0;
                }
                topic.UnreadPriority -= countMarkedPriorityRead;
                if (topic.UnreadPriority < 0)
                {
                    topic.UnreadPriority = 0;
                }
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(topic);
                }
                CIX.FolderCollection.NotifyFolderUpdated(topic);
            }
            CIX.FolderCollection.NotifyThreadChanged(this);
        }

        /// <summary>
        /// Mark the thread from the specified root as unread.
        /// </summary>
        public void MarkThreadUnread()
        {
            List<CIXMessage> messagesUpdated = new List<CIXMessage>();
            Folder topic = Topic;

            int countMarkedUnread = 0;
            int countMarkedPriorityUnread = 0;

            if (!ReadLocked && !Unread)
            {
                Unread = true;
                ReadPending = true;
                ++countMarkedUnread;
                if (Priority)
                {
                    ++countMarkedPriorityUnread;
                }
                messagesUpdated.Add(this);
            }

            IEnumerable<CIXMessage> childMessages = topic.Messages.Children(this);
            foreach (CIXMessage child in childMessages)
            {
                if (!child.ReadLocked && !child.Unread)
                {
                    child.Unread = true;
                    child.ReadPending = true;
                    ++countMarkedUnread;
                    if (child.Priority)
                    {
                        ++countMarkedPriorityUnread;
                    }
                    messagesUpdated.Add(child);
                }
            }

            lock (CIX.DBLock)
            {
                CIX.DB.UpdateAll(messagesUpdated);
            }

            if (countMarkedUnread > 0)
            {
                topic.Unread += countMarkedUnread;
                topic.UnreadPriority += countMarkedPriorityUnread;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(topic);
                }
                CIX.FolderCollection.NotifyFolderUpdated(topic);
            }
            CIX.FolderCollection.NotifyThreadChanged(this);
        }

        /// <summary>
        /// Perform synchronisation on this message
        /// </summary>
        public void Sync()
        {
            if (CIX.Online)
            {
                if (PostPending)
                {
                    PostMessage();
                }
                if (StarPending)
                {
                    StarMessage();
                }
                if (WithdrawPending)
                {
                    WithdrawMessage();
                }
            }
        }

        /// <summary>
        /// Mark this message as unread and update the corresponding folder
        /// unread counts.
        /// </summary>
        private void InnerMarkUnread()
        {
            bool stateChanged = !Unread;
            if (stateChanged)
            {
                Unread = true;
                ReadPending = true;
            }
            lock (CIX.DBLock)
            {
                CIX.DB.Update(this);
            }
            CIX.FolderCollection.NotifyMessageChanged(this);
            if (stateChanged)
            {
                Folder topic = Topic;
                topic.Unread += 1;
                if (Priority)
                {
                    topic.UnreadPriority += 1;
                }
                if (!IsRoot)
                {
                    CIXMessage root = topic.Messages.MessageByID(RootID);
                    if (root != null)
                    {
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(root);
                        }
                        CIX.FolderCollection.NotifyMessageChanged(root);
                    }
                }
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(topic);
                }
                CIX.FolderCollection.NotifyFolderUpdated(topic);
            }
        }

        /// <summary>
        /// Internal method to set the ignore flag on the message and simultaneously
        /// mark the messages as read.
        /// </summary>
        /// <returns>The number of unread messages marked read</returns>
        internal int InnerSetIgnore()
        {
            Folder topic = Topic;
            IEnumerable<CIXMessage> childMessages = topic.Messages.Children(this);
            int unreadCount = 0;

            Ignored = true;
            bool hasPriority = Priority;
            if (!ReadLocked && Unread)
            {
                Unread = false;
                ReadPending = true;
                ++unreadCount;
            }
            CIX.DB.Update(this);

            foreach (CIXMessage child in childMessages)
            {
                child.Ignored = true;
                if (!child.ReadLocked && child.Unread)
                {
                    child.Unread = false;
                    child.ReadPending = true;
                    if (child.Priority)
                    {
                        hasPriority = true;
                    }
                    ++unreadCount;
                }
                CIX.DB.Update(child);
            }
            if (topic.Unread >= unreadCount)
            {
                topic.Unread -= unreadCount;
                if (hasPriority && topic.UnreadPriority >= unreadCount)
                {
                    topic.UnreadPriority -= unreadCount;
                }
                CIX.DB.Update(topic);
            }
            return unreadCount;
        }

        /// <summary>
        /// Internal method to set the priority flag on the message.
        /// </summary>
        internal void InnerSetPriority()
        {
            Folder topic = Topic;
            IEnumerable<CIXMessage> childMessages = topic.Messages.Children(this);
            bool folderChanged = false;

            Priority = true;
            if (Unread)
            {
                topic.UnreadPriority += 1;
                folderChanged = true;
            }
            CIX.DB.Update(this);
            foreach (CIXMessage child in childMessages)
            {
                child.Priority = true;
                if (child.Unread)
                {
                    topic.UnreadPriority += 1;
                    folderChanged = true;
                }
                CIX.DB.Update(child);
            }
            if (folderChanged)
            {
                CIX.DB.Update(topic);
            }
        }

        /// <summary>
        /// Mark this message as unread and update the corresponding folder
        /// unread counts.
        /// </summary>
        private void InnerMarkRead()
        {
            bool stateChanged = Unread;
            if (stateChanged)
            {
                Unread = false;
                ReadPending = true;
            }
            lock (CIX.DBLock)
            {
                CIX.DB.Update(this);
            }
            CIX.FolderCollection.NotifyMessageChanged(this);
            if (stateChanged)
            {
                Folder topic = Topic;
                topic.Unread -= 1;
                if (Priority && topic.UnreadPriority > 0)
                {
                    topic.UnreadPriority -= 1;
                }
                if (!IsRoot)
                {
                    CIXMessage root = topic.Messages.MessageByID(RootID);
                    if (root != null)
                    {
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(root);
                        }
                        CIX.FolderCollection.NotifyMessageChanged(root);
                    }
                }
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(topic);
                }
                CIX.FolderCollection.NotifyFolderUpdated(topic);
            }
        }

        /// <summary>
        /// Add or remove the star from this message on the server.
        /// </summary>
        private void StarMessage()
        {
            Folder folder = Topic;

            if (Starred)
            {
                StarAdd addStar = new StarAdd
                {
                    Forum = Topic.ParentFolder.Name,
                    Topic = folder.Name,
                    MsgID = RemoteID.ToString(CultureInfo.InvariantCulture)
                };
                LogFile.WriteLine("Adding star to message {0} in {1}/{2}", addStar.MsgID, addStar.Forum, addStar.Topic);

                try
                {
                    HttpWebRequest postUrl = APIRequest.Post("starred/add", APIRequest.APIFormat.XML, addStar);
                    string responseString = APIRequest.ReadResponseString(postUrl);

                    if (responseString.Contains("Success"))
                    {
                        LogFile.WriteLine("Star successfully added");
                    }
                    else
                    {
                        LogFile.WriteLine("Failed to add star to message {0} : {1}", RemoteID, responseString);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("CIXMessage.StarMessage", e);
                }
                StarPending = false;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
            }
            else
            {
                string forumName = FolderCollection.EncodeForumName(Topic.ParentFolder.Name);
                string starUrl = string.Format("starred/{0}/{1}/{2}/rem", forumName, Topic.Name, RemoteID);

                LogFile.WriteLine("Removing star from message {0} in {1}/{2}", RemoteID, forumName, Topic.Name);

                try
                {
                    HttpWebRequest getRequest = APIRequest.Get(starUrl, APIRequest.APIFormat.XML);
                    string responseString = APIRequest.ReadResponseString(getRequest);

                    if (responseString.Contains("Success"))
                    {
                        LogFile.WriteLine("Star successfully removed");
                    }
                    else
                    {
                        LogFile.WriteLine("Failed to remove star for message {0} : {1}", RemoteID, responseString);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("CIXMessage.StarMessage", e);
                }
                if (StarPending)
                {
                    StarPending = false;
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(this);
                    }
                }
            }
        }

        /// <summary>
        /// Withdraw this message from the server.
        /// </summary>
        private void WithdrawMessage()
        {
            try
            {
                LogFile.WriteLine("Withdrawing message {0}", RemoteID);

                Folder topic = Topic;
                Folder forum = topic.ParentFolder;
                string urlFormat = string.Format("forums/{0}/{1}/{2}/withdraw", FolderCollection.EncodeForumName(forum.Name), topic.Name, RemoteID);
                HttpWebRequest request = APIRequest.Get(urlFormat, APIRequest.APIFormat.XML);

                // Don't try and withdraw again because this was an error.
                WithdrawPending = false;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
                string responseString = APIRequest.ReadResponseString(request);

                if (responseString == "Success")
                {
                    LogFile.WriteLine("Successfully withdrawn message {0}", RemoteID);

                    // Replace the message text with the one that we'd get from the server
                    // and save us a round-trip.
                    Body = IsMine ? Resources.WithdrawnByAuthor : Resources.WithdrawnByModerator;
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(this);
                    }
                    CIX.FolderCollection.NotifyMessageChanged(this);
                }
                else
                {
                    LogFile.WriteLine("Error withdrawing message. Response is: {1}", forum.Name, responseString);
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("CIXMessage.WithdrawMessage", e);
            }
        }

        /// <summary>
        /// Post this message to the server
        /// </summary>
        private void PostMessage()
        {
            CIX.FolderCollection.NotifyMessagePostStarted(this);
            PostPending = false;
            try
            {
                Folder folder = Topic;
                PostMessage postMessage = new PostMessage
                {
                    Body = Body.FixQuotes(),
                    Forum = folder.ParentFolder.Name,
                    Topic = folder.Name,
                    WrapColumn = "0",
                    MarkRead = "1",
                    MsgID = CommentID.ToString(CultureInfo.InvariantCulture)
                };

                HttpWebRequest postUrl = APIRequest.Post("forums/post", APIRequest.APIFormat.XML, postMessage);
                string responseString = APIRequest.ReadResponseString(postUrl);

                int newRemoteID;
                if (int.TryParse(responseString, out newRemoteID))
                {
                    RemoteID = newRemoteID;
                    Date = DateTime.Now;
                    lock (CIX.DBLock)
                    {
                        CIX.DB.Update(this);
                    }
                    if (CommentID == 0)
                    {
                        LogFile.WriteLine("Posted new thread \"{0}\" as message {1}", Body.FirstLine(), RemoteID);
                    }
                    else
                    {
                        LogFile.WriteLine("Posted new reply to message {0} as message {1}", CommentID, RemoteID);
                    }
                    CIX.FolderCollection.NotifyMessageChanged(this);
                }
                else
                {
                    LogFile.WriteLine("Failed to post message {0} : {1}", ID, responseString);
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("CIXMessage.PostMessage", e);
            }
            CIX.FolderCollection.NotifyMessagePostCompleted(this);
        }
    }
}