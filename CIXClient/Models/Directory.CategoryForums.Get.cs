// *****************************************************
// CIXReader
// Directory.CategoryForums.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/09/2013 9:58 AM
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
    public sealed class DirListings
    {
        private string countField;

        private string startField;

        private DirListing[] forumsField;

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
        [XmlArrayItem("Listing", typeof (DirListing), IsNullable = false)]
        public DirListing[] Forums
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
    public sealed class DirListing
    {
        private string catField;

        private string forumField;

        private int recentField;

        private string subField;

        private string titleField;

        private string typeField;

        /// <remarks />
        public string Cat
        {
            get { return catField; }
            set { catField = value; }
        }

        /// <remarks />
        public string Forum
        {
            get { return forumField; }
            set { forumField = value; }
        }

        /// <remarks />
        public int Recent
        {
            get { return recentField; }
            set { recentField = value; }
        }

        /// <remarks />
        public string Sub
        {
            get { return subField; }
            set { subField = value; }
        }

        /// <remarks />
        public string Title
        {
            get { return titleField; }
            set { titleField = value; }
        }

        /// <remarks />
        public string Type
        {
            get { return typeField; }
            set { typeField = value; }
        }
    }
}