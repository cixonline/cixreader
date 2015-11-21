// *****************************************************
// CIXReader
// User.AllTopics.get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 23/10/2013 3:40 PM
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
    public sealed class UserForumTopicResultSet2
    {
        private string countField;

        private string startField;

        private UserForumTopic2[] userTopicsField;

        /// <remarks />
        public string Count
        {
            get { return countField; }
            set { countField = value; }
        }

        /// <remarks />
        public string Start
        {
            get { return startField; }
            set { startField = value; }
        }

        /// <remarks />
        [XmlArrayItem("UserForumTopic2", typeof (UserForumTopic2), IsNullable = false)]
        public UserForumTopic2[] UserTopics
        {
            get { return userTopicsField; }
            set { userTopicsField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class UserForumTopic2
    {
        private int flagsField;

        private string forumField;

        private string msgsField;

        private string priorityField;

        private string topicField;

        private int unReadField;

        private string recentField;

        private string nameField;

        private string latestField;

        /// <remarks />
        public int Flags
        {
            get { return flagsField; }
            set { flagsField = value; }
        }

        /// <remarks />
        public string Forum
        {
            get { return forumField; }
            set { forumField = value; }
        }

        /// <remarks />
        public string Msgs
        {
            get { return msgsField; }
            set { msgsField = value; }
        }

        /// <remarks />
        public string Priority
        {
            get { return priorityField; }
            set { priorityField = value; }
        }

        /// <remarks />
        public string Topic
        {
            get { return topicField; }
            set { topicField = value; }
        }

        /// <remarks />
        public int UnRead
        {
            get { return unReadField; }
            set { unReadField = value; }
        }

        /// <remarks />
        public string Recent
        {
            get { return recentField; }
            set { recentField = value; }
        }

        /// <remarks />
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks />
        public string Latest
        {
            get { return latestField; }
            set { latestField = value; }
        }
    }
}