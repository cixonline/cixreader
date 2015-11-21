// *****************************************************
// CIXReader
// InboxConversation.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/10/2013 8:18 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using CIXClient.Collections;
using CIXClient.Database;
using CIXClient.Models;

namespace CIXClient.Tables
{
    /// <summary>
    /// The Inbox conversation table lists all inbox message conversations.
    /// </summary>
    public sealed class InboxConversation
    {
        private MailCollection _messages;

            /// <summary>
        /// An unique local ID that identifies this conversation.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// The inbox message ID that corresponds to this conversation.
        /// This can be 0 if the message is not yet synced with the server.
        /// </summary>
        public int RemoteID { get; set; }

        /// <summary>
        /// The CIX nickname of the author of this conversation.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The date and time when the conversation was started.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The subject line of the conversation.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The total number of unread messages in the conversation.
        /// </summary>
        public int UnreadCount { get; set; }

        /// <summary>
        /// A set of flags that specify the message state.
        /// </summary>
        public InboxConversationFlags Flags { get; set; }

        /// <summary>
        /// Return whether this is a draft conversation. A draft
        /// conversation is one which has no remote ID yet and thus
        /// has not yet been posted.
        /// </summary>
        [Ignore]
        public bool IsDraft
        {
            get { return RemoteID == 0; }
        }

        /// <summary>
        /// Return whether this conversation has an error.
        /// </summary>
        [Ignore]
        public bool LastError
        {
            get { return Flags.HasFlag(InboxConversationFlags.Error); }
        }

        /// <summary>
        /// Return all the messages of this conversation
        /// </summary>
        [Ignore]
        public MailCollection Messages
        {
            get {
                return _messages ?? (_messages = new MailCollection(CIX.DB.Table<InboxMessage>().Where(msg => msg.ConversationID == ID).ToList()));
            }
        }

        /// <summary>
        /// Mark this conversation as read.
        /// </summary>
        public void MarkRead()
        {
            if (UnreadCount > 0)
            {
                UnreadCount = 0;
                Flags = InboxConversationFlags.MarkRead;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
                CIX.ConversationCollection.NotifyConversationChanged(this);
            }
        }

        /// <summary>
        /// Mark this conversation as read.
        /// </summary>
        public void MarkUnread()
        {
            if (UnreadCount == 0)
            {
                UnreadCount = 1;
                Flags = InboxConversationFlags.MarkRead;
                lock (CIX.DBLock)
                {
                    CIX.DB.Update(this);
                }
                CIX.ConversationCollection.NotifyConversationChanged(this);
            }
        }

        /// <summary>
        /// Mark this conversation as deleted.
        /// </summary>
        public void MarkDelete()
        {
            Flags = InboxConversationFlags.Deleted;
            UnreadCount = 0;
            lock (CIX.DBLock)
            {
                CIX.DB.Update(this);
            }
            CIX.ConversationCollection.NotifyConversationDeleted(this);
        }

        /// <summary>
        /// Sync this conversation with the server.
        /// </summary>
        public void Sync()
        {
            if (Flags.HasFlag(InboxConversationFlags.Deleted))
            {
                DeleteConversation();
                return;
            }
            if (Flags.HasFlag(InboxConversationFlags.MarkRead))
            {
                MarkReadConversation();
            }

            if (Flags.HasFlag(InboxConversationFlags.Error))
            {
                return;
            }

            int conversationID = RemoteID;
            foreach (InboxMessage message in Messages)
            {
                if (message.IsDraft)
                {
                    if (conversationID > 0)
                    {
                        Reply(message);
                    }
                    else
                    {
                        bool hasError = false;

                        try
                        {
                            PMessageAdd newMessage = new PMessageAdd
                            {
                                Body = message.Body.EscapeXml(),
                                Recipient = message.Author,
                                Subject = Subject
                            };

                            WebRequest wrPosturl = APIRequest.Post("personalmessage/add", APIRequest.APIFormat.XML, newMessage);
                            Stream objStream = APIRequest.ReadResponse(wrPosturl);
                            if (objStream != null)
                            {
                                using (TextReader reader = new StreamReader(objStream))
                                {
                                    XmlDocument doc = new XmlDocument {InnerXml = reader.ReadLine()};

                                    if (doc.DocumentElement != null)
                                    {
                                        string responseString = doc.DocumentElement.InnerText;
                                        string[] splitStrings = responseString.Split(new[] {','});

                                        if (splitStrings.Length == 2)
                                        {
                                            int messageID;

                                            if (Int32.TryParse(splitStrings[0], out conversationID) &&
                                                Int32.TryParse(splitStrings[1], out messageID))
                                            {
                                                lock (CIX.DBLock)
                                                {
                                                    RemoteID = conversationID;
                                                    CIX.DB.Update(this);

                                                    message.RemoteID = messageID;
                                                    CIX.DB.Update(message);
                                                }
                                                LogFile.WriteLine("New message {0} in conversation {1} posted to server and updated locally", message.RemoteID, RemoteID);
                                            }
                                        }
                                        else
                                        {
                                            hasError = true;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            CIX.ReportServerExceptions("InboxConversation.Sync", e);
                            hasError = true;
                        }

                        // Flag if we hit an error
                        if (hasError)
                        {
                            Flags |= InboxConversationFlags.Error;
                            lock (CIX.DBLock)
                            {
                                CIX.DB.Update(this);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Post a reply message to the server. The ConversationID field of the message must be set to
        /// a valid conversation ID for the thread to which the reply is being posted.
        /// </summary>
        /// <param name="message">The draft InboxMessage to be posted</param>
        private void Reply(InboxMessage message)
        {
            try
            {
                PMessageReply reply = new PMessageReply
                {
                    Body = message.Body.EscapeXml(),
                    ConID = RemoteID
                };

                WebRequest wrPosturl = APIRequest.Post("personalmessage/reply", APIRequest.APIFormat.XML, reply);
                Stream objStream = APIRequest.ReadResponse(wrPosturl);
                if (objStream != null)
                {
                    using (TextReader reader = new StreamReader(objStream))
                    {
                        XmlDocument doc = new XmlDocument { InnerXml = reader.ReadLine() };

                        if (doc.DocumentElement != null)
                        {
                            string responseString = doc.DocumentElement.InnerText;
                            int messageID;

                            if (Int32.TryParse(responseString, out messageID))
                            {
                                message.RemoteID = messageID;
                                lock (CIX.DBLock)
                                {
                                    CIX.DB.Update(message);
                                }
                                LogFile.WriteLine("Reply {0} posted to server and updated locally", message.RemoteID);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CIX.ReportServerExceptions("InboxConversation.Reply", e);
            }
        }

        /// <summary>
        /// Delete this conversation from the server.
        /// </summary>
        private void DeleteConversation()
        {
            if (CIX.Online)
            {
                try
                {
                    // If draft, then just delete
                    if (RemoteID == 0)
                    {
                        CIX.ConversationCollection.Remove(this);
                        return;
                    }

                    HttpWebRequest wrGeturl = APIRequest.Get("personalmessage/inbox/" + RemoteID + "/rem", APIRequest.APIFormat.XML);
                    string responseString = APIRequest.ReadResponseString(wrGeturl);
                    if (responseString == "Success")
                    {
                        LogFile.WriteLine("Conversation {0} deleted from inbox", RemoteID);
                        CIX.ConversationCollection.Remove(this);
                    }

                    wrGeturl = APIRequest.Get("personalmessage/outbox/" + RemoteID + "/rem", APIRequest.APIFormat.XML);
                    responseString = APIRequest.ReadResponseString(wrGeturl);

                    if (responseString == "Success")
                    {
                        LogFile.WriteLine("Conversation {0} deleted from outbox", RemoteID);
                        CIX.ConversationCollection.Remove(this);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("InboxConversation.DeleteConversations", e);
                }
            }
        }

        /// <summary>
        /// Mark this conversation as read or unread on the server.
        /// </summary>
        private void MarkReadConversation()
        {
            if (CIX.Online)
            {
                try
                {
                    string url = string.Format("personalmessage/{0}/{1}/toggleread", RemoteID, UnreadCount > 0);
                    HttpWebRequest wrGeturl = APIRequest.Get(url, APIRequest.APIFormat.XML);
                    string responseString = APIRequest.ReadResponseString(wrGeturl);
                    if (responseString == "Success")
                    {
                        Flags &= ~InboxConversationFlags.MarkRead;
                        lock (CIX.DBLock)
                        {
                            CIX.DB.Update(this);
                        }
                        LogFile.WriteLine("Conversation {0} marked as read on server", RemoteID);
                    }
                }
                catch (Exception e)
                {
                    CIX.ReportServerExceptions("InboxConversation.MarkReadConversation", e);
                }
            }
        }
    }

    /// <summary>
    /// Inbox Conversation flags.
    /// </summary>
    [Flags]
    public enum InboxConversationFlags
    {
        /// <summary>
        /// Indicates that this message should be marked read on the server.
        /// </summary>
        MarkRead = 1,

        /// <summary>
        /// Indicates that this conversation should be deleted from the server.
        /// </summary>
        Deleted = 2,

        /// <summary>
        /// Indicates that the conversation could not be posted.
        /// </summary>
        Error = 4
    }
}