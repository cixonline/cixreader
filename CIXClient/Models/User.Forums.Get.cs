// *****************************************************
// CIXReader
// User.Forums.Get.cs
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
    public sealed class ForumResultSet
    {
        private string countField;

        private ForumResultSetRow[] forumsField;
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
        [XmlArrayItem("ForumRow", typeof (ForumResultSetRow), IsNullable = false)]
        public ForumResultSetRow[] Forums
        {
            get { return forumsField; }
            set { forumsField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17626")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class ForumResultSetRow
    {
        private string flagsField;

        private string nameField;

        private string priorityField;

        private string unreadField;

        /// <remarks />
        public string Flags
        {
            get { return flagsField; }
            set { flagsField = value; }
        }

        /// <remarks />
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks />
        public string Priority
        {
            get { return priorityField; }
            set { priorityField = value; }
        }

        /// <remarks />
        public string Unread
        {
            get { return unreadField; }
            set { unreadField = value; }
        }
    }
}