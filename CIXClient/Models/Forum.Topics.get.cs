// *****************************************************
// CIXReader
// Forum.Topics.get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 28/10/2013 9:50 PM
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
    public sealed class TopicResultSet
    {
        private string countField;

        private string startField;

        private TopicResult[] topicsField;

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
        [XmlArrayItem("Topic", typeof (TopicResult), IsNullable = false)]
        public TopicResult[] Topics
        {
            get { return topicsField; }
            set { topicsField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class TopicResult
    {
        private string descField;

        private string filesField;

        private string flagField;

        private string nameField;

        /// <remarks />
        public string Desc
        {
            get { return descField; }
            set { descField = value; }
        }

        /// <remarks />
        public string Files
        {
            get { return filesField; }
            set { filesField = value; }
        }

        /// <remarks />
        public string Flag
        {
            get { return flagField; }
            set { flagField = value; }
        }

        /// <remarks />
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }
    }
}