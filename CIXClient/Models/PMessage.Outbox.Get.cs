// *****************************************************
// CIXReader
// PMessage.Outbox.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 11/06/2014 22:02
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.CodeDom.Compiler;
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
    public sealed class ConversationOutboxSet
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
        /// An array of CIXOutboxItem that represent messages in the outbox.
        /// </summary>
        [XmlArrayItem("ConversationOutbox", typeof(CIXOutboxItem), IsNullable = false)]
        public CIXOutboxItem[] Conversations { get; set; }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class CIXOutboxItem
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
        /// Nickname of the recipient.
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// The message subject line.
        /// </summary>
        public string Subject { get; set; }
    }
}