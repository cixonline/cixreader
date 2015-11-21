// *****************************************************
// CIXReader
// Directory.Categories.Get.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/09/2013 9:57 AM
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
    public sealed class CategoryResultSet
    {
        private string countField;

        private string startField;

        private CategoryResult[] categoriesField;

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
        [XmlArrayItem("Category", typeof (CategoryResult), IsNullable = false)]
        public CategoryResult[] Categories
        {
            get { return categoriesField; }
            set { categoriesField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17626")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true, Namespace = "http://cixonline.com")]
    public sealed class CategoryResult
    {
        private string nameField;

        private string subField;

        /// <remarks />
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks />
        public string Sub
        {
            get { return subField; }
            set { subField = value; }
        }
    }
}
