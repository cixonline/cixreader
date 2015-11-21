// *****************************************************
// CIXReader
// Forum.InterestingThreads.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 30/06/2015 09:57
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
    public sealed class InterestingThreadSet
    {
        private string countField;

        private InterestingThread[] messagesField;
        private string startField;

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
        [XmlArrayItem("InterestingThread", typeof (InterestingThread), IsNullable = false)]
        public InterestingThread[] Messages
        {
            get { return messagesField; }
            set { messagesField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class InterestingThread
    {
        private string authorField;

        private string bodyField;

        private string dateTimeField;

        private string forumField;

        private int rootIDField;

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
        public string Forum
        {
            get { return forumField; }
            set { forumField = value; }
        }

        /// <remarks />
        public int RootID
        {
            get { return rootIDField; }
            set { rootIDField = value; }
        }

        /// <remarks />
        public string Topic
        {
            get { return topicField; }
            set { topicField = value; }
        }
    }
}