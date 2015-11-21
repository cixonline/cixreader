// *****************************************************
// CIXReader
// SearchEngines.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/01/2015 11:44
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CIXClient;
using CIXReader.Properties;

namespace CIXReader.Utilities
{
    internal static class SearchEngines
    {
        private static SearchEnginesFile _searchEngines;

        public static IEnumerable<string> AllSites
        {
            get
            {
                if (_searchEngines == null)
                {
                    LoadSearchEngines();
                }
                return _searchEngines != null ? _searchEngines.Items.Select(entry => entry.name).ToArray() : new String[0];
            }
        }

        /// <summary>
        /// Return the link for the specified search engine
        /// </summary>
        /// <param name="name">The search engine name</param>
        /// <returns>A template URL for the search engine</returns>
        public static string LinkForSearchEngine(string name)
        {
            if (_searchEngines == null)
            {
                LoadSearchEngines();
            }
            if (_searchEngines != null)
            {
                foreach (SearchEngineEntry entry in _searchEngines.Items.Where(entry => entry.name == name))
                {
                    return entry.link;
                }
            }
            return "";
        }

        /// <summary>
        /// Load the list of search engines from the resource.
        /// </summary>
        private static void LoadSearchEngines()
        {
            try
            {
                string searchEnginesXML = Resources.SearchEngines;
                using (XmlReader reader = XmlReader.Create(new StringReader(searchEnginesXML)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SearchEnginesFile));
                    _searchEngines = (SearchEnginesFile)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Error parsing search engines {0} : ", e.Message);
            }
        }
    }

    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public sealed class SearchEnginesFile
    {
        private SearchEngineEntry[] itemsField;

        /// <remarks />
        [XmlElement("Site", Form = XmlSchemaForm.Unqualified)]
        public SearchEngineEntry[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public sealed class SearchEngineEntry
    {
        private string linkField;
        private string nameField;

        /// <remarks />
        [XmlAttributeAttribute]
        public string link
        {
            get { return linkField; }
            set { linkField = value; }
        }

        /// <remarks />
        [XmlAttributeAttribute]
        public string name
        {
            get { return nameField; }
            set { nameField = value; }
        }
    }
}