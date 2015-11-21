// *****************************************************
// CIXReader
// PMessage.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/09/2013 12:20 PM
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
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    [XmlRoot(Namespace = "http://cixonline.com", IsNullable = false)]
    public sealed class PMessageSet
    {
        private string countField;

        private CIXInboxMessage[] pMessagesField;
        private string subjectField;

        /// <remarks />
        public string Count
        {
            get { return countField; }
            set { countField = value; }
        }

        /// <remarks />
        public string Subject
        {
            get { return subjectField; }
            set { subjectField = value; }
        }

        /// <remarks />
        [XmlArrayItem("PMessage", typeof(CIXInboxMessage), IsNullable = false)]
        public CIXInboxMessage[] PMessages
        {
            get { return pMessagesField; }
            set { pMessagesField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class CIXInboxMessage
    {
        private string bodyField;

        private string dateField;

        private int messageIDField;

        private string recipientField;

        private string senderField;

        /// <remarks />
        public string Body
        {
            get { return bodyField; }
            set { bodyField = value; }
        }

        /// <remarks />
        public string Date
        {
            get { return dateField; }
            set { dateField = value; }
        }

        /// <remarks />
        public int MessageID
        {
            get { return messageIDField; }
            set { messageIDField = value; }
        }

        /// <remarks />
        public string Recipient
        {
            get { return recipientField; }
            set { recipientField = value; }
        }

        /// <remarks />
        public string Sender
        {
            get { return senderField; }
            set { senderField = value; }
        }
    }
}
