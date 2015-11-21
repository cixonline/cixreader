// *****************************************************
// CIXReader
// Forum.Details.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 13/10/2013 7:24 PM
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
    public sealed class ForumDetails
    {
        private string categoryField;

        private string descriptionField;

        private string firstPostField;

        private string lastPostField;

        private string nameField;

        private int recentField;

        private string subCategoryField;

        private string titleField;

        private string topicsField;

        private string typeField;

        /// <remarks />
        public string Category
        {
            get { return categoryField; }
            set { categoryField = value; }
        }

        /// <remarks />
        public string Description
        {
            get { return descriptionField; }
            set { descriptionField = value; }
        }

        /// <remarks />
        public string FirstPost
        {
            get { return firstPostField; }
            set { firstPostField = value; }
        }

        /// <remarks />
        public string LastPost
        {
            get { return lastPostField; }
            set { lastPostField = value; }
        }

        /// <remarks />
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks />
        public int Recent
        {
            get { return recentField; }
            set { recentField = value; }
        }

        /// <remarks />
        public string SubCategory
        {
            get { return subCategoryField; }
            set { subCategoryField = value; }
        }

        /// <remarks />
        public string Title
        {
            get { return titleField; }
            set { titleField = value; }
        }

        /// <remarks />
        public string Topics
        {
            get { return topicsField; }
            set { topicsField = value; }
        }

        /// <remarks />
        public string Type
        {
            get { return typeField; }
            set { typeField = value; }
        }
    }
}