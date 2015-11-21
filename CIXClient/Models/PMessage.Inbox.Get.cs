// *****************************************************
// CIXReader
// PMessage.Inbox.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/08/2013 5:15 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CIXClient.Models
{
    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    [XmlRoot(Namespace = "http://cixonline.com", IsNullable = false)]
    public sealed class ConversationInboxSet
    {
        /// <summary>
        /// Total count of messages
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// Index of the Start message.
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// An array of CIXInboxItems that represent messages in the inbox.
        /// </summary>
        [XmlArrayItem("ConversationInbox", typeof (CIXInboxItem), IsNullable = false)]
        public CIXInboxItem[] Conversations { get; set; }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class CIXInboxItem
    {
        /// <summary>
        /// Body of the message
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Date of the message in ISO format.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// ID of the message.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Actually, not quite sure what this is.
        /// </summary>
        public string LastMsgBy { get; set; }

        /// <summary>
        /// Nickname of the sender.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// The message subject line.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// true or false value which indicates whether or not
        /// this message has been read.
        /// </summary>
        public string Unread { get; set; }
    }
}