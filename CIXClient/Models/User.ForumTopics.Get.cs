// *****************************************************
// CIXReader
// User.ForumTopics.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 16/09/2013 7:37 AM
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
    [GeneratedCode("xsd", "4.0.30319.17626")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    [XmlRoot(Namespace = "http://cixonline.com", IsNullable = false)]
    public sealed class UserTopicResultSet
    {
        private string countField;

        private string startField;

        private UserTopic[] userTopicsField;

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
        [XmlArrayItem("UserTopic", typeof (UserTopic),
            IsNullable = false)]
        public UserTopic[] UserTopics
        {
            get { return userTopicsField; }
            set { userTopicsField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17626")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class UserTopic
    {
        private string flagField;

        private string msgsField;

        private string nameField;

        private string statusField;

        private string unReadField;

        /// <remarks />
        public string Flag
        {
            get { return flagField; }
            set { flagField = value; }
        }

        /// <remarks />
        public string Msgs
        {
            get { return msgsField; }
            set { msgsField = value; }
        }

        /// <remarks />
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks />
        public string Status
        {
            get { return statusField; }
            set { statusField = value; }
        }

        /// <remarks />
        public string UnRead
        {
            get { return unReadField; }
            set { unReadField = value; }
        }
    }
}