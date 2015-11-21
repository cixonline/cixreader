// *****************************************************
// CIXReader
// Forum.Message.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 07/10/2013 1:35 PM
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
    public sealed class Message
    {
        private string authorField;

        private string bodyField;

        private string dateTimeField;

        private string depthField;

        private string forumField;

        private int idField;

        private string priorityField;

        private int replyToField;

        private int rootIDField;

        private string starredField;

        private string statusField;

        private string topicField;

        /// <remarks />
        public string Author
        {
            get { return authorField; }
            set { authorField = value; }
        }

        /// <remarks />
        public string Body
        {
            get { return bodyField; }
            set { bodyField = value; }
        }

        /// <remarks />
        public string DateTime
        {
            get { return dateTimeField; }
            set { dateTimeField = value; }
        }

        /// <remarks />
        public string Depth
        {
            get { return depthField; }
            set { depthField = value; }
        }

        /// <remarks />
        public string Forum
        {
            get { return forumField; }
            set { forumField = value; }
        }

        /// <remarks />
        public int ID
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks />
        public string Priority
        {
            get { return priorityField; }
            set { priorityField = value; }
        }

        /// <remarks />
        public int ReplyTo
        {
            get { return replyToField; }
            set { replyToField = value; }
        }

        /// <remarks />
        public int RootID
        {
            get { return rootIDField; }
            set { rootIDField = value; }
        }

        /// <remarks />
        public string Starred
        {
            get { return starredField; }
            set { starredField = value; }
        }

        /// <remarks />
        public string Status
        {
            get { return statusField; }
            set { statusField = value; }
        }

        /// <remarks />
        public string Topic
        {
            get { return topicField; }
            set { topicField = value; }
        }
    }
}