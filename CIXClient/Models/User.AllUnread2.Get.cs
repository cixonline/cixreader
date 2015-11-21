// *****************************************************
// CIXReader
// User.AllUnread2.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 30/01/2015 12:10
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
    public sealed class Message2
    {
        private string authorField;

        private string bodyField;

        private string dateTimeField;

        private string flagField;

        private string forumField;

        private int idField;

        private bool priorityField;

        private int replyToField;

        private int rootIDField;

        private bool starredField;

        private string topicField;

        private bool unreadField;

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
        public string Flag
        {
            get { return flagField; }
            set { flagField = value; }
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
        public bool Priority
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
        public bool Starred
        {
            get { return starredField; }
            set { starredField = value; }
        }

        /// <remarks />
        public string Topic
        {
            get { return topicField; }
            set { topicField = value; }
        }

        /// <remarks />
        public bool Unread
        {
            get { return unreadField; }
            set { unreadField = value; }
        }
    }
}