// *****************************************************
// CIXReader
// CRToolbarItemCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/05/2014 23:14
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CIXClient;
using CIXReader.Utilities;

namespace CIXReader.Controls
{
    /// <summary>
    /// Defines the CRToolbar item collection class
    /// </summary>
    public sealed class CRToolbarItemCollection
    {
        private toolbar _allButtons;
        private ArrayList _customButtons;
        private List<CRToolbarItem> _buttons;

        private static readonly CRToolbarItemCollection _defaultCollection = new CRToolbarItemCollection();

        /// <summary>
        /// Prevents a default instance of the <see cref="CRToolbarItemCollection"/> class
        /// from being created.
        /// </summary>
        private CRToolbarItemCollection()
        {
        }

        /// <summary>
        /// Return the default toolbar item collection.
        /// </summary>
        public static CRToolbarItemCollection DefaultCollection
        {
            get { return _defaultCollection; }
        }

        /// <summary>
        /// Return the default collection of all toolbar buttons.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ToolbarDataItem> AllButtons
        {
            get
            {
                if (_allButtons == null)
                {
                    string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                    string toolbarFile = Path.Combine(appPath, "Toolbar", "toolbar_all.xml");
                    _allButtons = LoadToolbarFromFile(toolbarFile);
                }
                if (_allButtons != null)
                {
                    ArrayList items = new ArrayList(_allButtons.Items);
                    if (_customButtons != null)
                    {
                        items.AddRange(_customButtons);
                    }
                    return items.ToArray(typeof (ToolbarDataItem)) as ToolbarDataItem[];
                }
                return null;
            }
        }

        /// <summary>
        /// Add a custom button to the default set.
        /// </summary>
        public void Add(ToolbarDataItem newItem)
        {
            if (_customButtons == null)
            {
                _customButtons = new ArrayList();
            }
            _customButtons.Add(newItem);
        }

        /// <summary>
        /// Return the mutable list of all toolbar buttons.
        /// </summary>
        public List<CRToolbarItem> Buttons
        {
            get
            {
                if (_buttons == null)
                {
                    _buttons = new List<CRToolbarItem>();
                    foreach (CRToolbarItem tbItem in from item in Load select new CRToolbarItem(item))
                    {
                        _buttons.Add(tbItem);
                    }
                }
                return _buttons;
            }
        }

        /// <summary>
        /// Remove the specified item from the collection.
        /// </summary>
        public void Remove(CRToolbarItem button)
        {
            _buttons.Remove(button);
        }

        /// <summary>
        /// Insert the item at the specified index.
        /// </summary>
        public void Insert(int index, CRToolbarItem button)
        {
            _buttons.Insert(index, button);
        }

        /// <summary>
        /// Delete the current toolbar.
        /// </summary>
        public void Clear()
        {
            _buttons = null;
        }

        /// <summary>
        /// Reset the toolbar and reload it from the default. Delete both
        /// customised toolbars.
        /// </summary>
        public void Reset()
        {
            string toolbarFile = Path.Combine(CIX.HomeFolder, CIX.Username);
            toolbarFile = Path.ChangeExtension(toolbarFile, "toolbar.xml");

            if (File.Exists(toolbarFile))
            {
                File.Delete(toolbarFile);
            }

            toolbarFile = Path.Combine(CIX.HomeFolder, "allusers");
            toolbarFile = Path.ChangeExtension(toolbarFile, "toolbar.xml");

            if (File.Exists(toolbarFile))
            {
                File.Delete(toolbarFile);
            }

            _buttons = null;
        }

        /// <summary>
        /// Save the toolbar state.
        /// </summary>
        public void Save()
        {
            StreamWriter fileStream = null;
            string toolbarFile = Path.Combine(CIX.HomeFolder, CIX.Username);
            toolbarFile = Path.ChangeExtension(toolbarFile, "toolbar.xml");

            try
            {
                fileStream = new StreamWriter(toolbarFile, false);
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true,
                    NewLineOnAttributes = true
                };

                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof(toolbar));

                    toolbar newToolbar = new toolbar {Items = _buttons.Select(button => button.DataItem).ToArray()};
                    serializer.Serialize(writer, newToolbar);

                    LogFile.WriteLine("Saved toolbar {0}", toolbarFile);
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Cannot save toolbar to {0} : {1}", toolbarFile, e.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Attempt to load the user's custom toolbar.
        /// </summary>
        private static toolbar LoadUserToolbar()
        {
            string toolbarFile = Path.Combine(CIX.HomeFolder, CIX.Username);
            toolbarFile = Path.ChangeExtension(toolbarFile, "toolbar.xml");
            return File.Exists(toolbarFile) ? LoadToolbarFromFile(toolbarFile) : null;
        }

        /// <summary>
        /// Load the toolbar by first checking the user's configuration and if no
        /// custom toolbar is found, fall back on the built-in one.
        /// </summary>
        private static IEnumerable<ToolbarDataItem> Load
        {
            get
            {
                toolbar currentToolbar = LoadUserToolbar() ?? (LoadAllUserToolbar() ?? LoadDefaultToolbar());
                ArrayList items = new ArrayList(currentToolbar.Items);
                return items.ToArray(typeof(ToolbarDataItem)) as ToolbarDataItem[];
            }
        }

        /// <summary>
        /// Attempt to load the custom toolbar for all users.
        /// </summary>
        private static toolbar LoadAllUserToolbar()
        {
            string toolbarFile = Path.Combine(CIX.HomeFolder, "allusers");
            toolbarFile = Path.ChangeExtension(toolbarFile, "toolbar.xml");
            return File.Exists(toolbarFile) ? LoadToolbarFromFile(toolbarFile) : null;
        }

        /// <summary>
        /// Attempt to load the built-in toolbar.
        /// </summary>
        private static toolbar LoadDefaultToolbar()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string toolbarFile = Path.Combine(appPath, "Toolbar", "toolbar.xml");
            return LoadToolbarFromFile(toolbarFile);
        }

        /// <summary>
        /// Load a toolbar XML file from the given path.
        /// </summary>
        /// <param name="toolbarFile">Path to the toolbar file</param>
        private static toolbar LoadToolbarFromFile(string toolbarFile)
        {
            StreamReader fileStream = null;
            toolbar toolbar;

            try
            {
                fileStream = new StreamReader(toolbarFile);
                using (XmlReader reader = XmlReader.Create(fileStream))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof(toolbar));
                    toolbar = (toolbar)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Error parsing toolbar file {0} : {1}", toolbarFile, e.Message);
                toolbar = null;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
            return toolbar;
        }
    }

    /// <remarks/>
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public sealed class toolbar
    {
        private ToolbarDataItem[] itemsField;

        /// <remarks/>
        [XmlElement("button", Form = XmlSchemaForm.Unqualified)]
        public ToolbarDataItem[] Items
        {
            get
            {
                return itemsField;
            }
            set
            {
                itemsField = value;
            }
        }
    }

    /// <remarks/>
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public sealed class ToolbarDataItem
    {
        private string typeField;
        private string nameField;
        private string labelField;
        private string imageField;
        private string dataField;
        private string tooltipField;

        /// <summary>
        /// Return the item Action ID.
        /// </summary>
        public ActionID ID
        {
            get
            {
                ActionID tagId = ActionID.None;
                if (Type == CRToolbarItemType.Button)
                {
                    if (!Enum.TryParse(name, true, out tagId))
                    {
                        return ActionID.None;
                    }
                }
                else if (Type == CRToolbarItemType.Search)
                {
                    tagId = ActionID.Search;
                }
                else if (Type == CRToolbarItemType.FlexibleSpace)
                {
                    tagId = ActionID.FlexibleSpace;
                }
                else if (Type == CRToolbarItemType.Space)
                {
                    tagId = ActionID.Space;
                }
                return tagId;
            }
        }

        /// <summary>
        /// Return the type equivalent of the item type
        /// </summary>
        internal CRToolbarItemType Type
        {
            get
            {
                switch (typeField)
                {
                    default:
                        return CRToolbarItemType.None;

                    case "button":
                        return CRToolbarItemType.Button;

                    case "space":
                        return CRToolbarItemType.Space;

                    case "search":
                        return CRToolbarItemType.Search;

                    case "flexible_space":
                        return CRToolbarItemType.FlexibleSpace;
                }
            }
        }

        /// <summary>
        /// Return an Image object for the path specified by the image attribute
        /// for this item, or null if there is no image attribute, or the path
        /// cannot be successfully resolved.
        /// </summary>
        public Image Image
        {
            get
            {
                string appPath;
                if (imageField != null && imageField.StartsWith("app:", StringComparison.Ordinal))
                {
                    appPath = Path.GetDirectoryName(Application.ExecutablePath);
                    appPath = Path.Combine(appPath, "Images", imageField.Substring(4));
                }
                else if (imageField != null && imageField.StartsWith("usr:", StringComparison.Ordinal))
                {
                    appPath = Path.Combine(CIX.HomeFolder, imageField.Substring(4));
                }
                else
                {
                    appPath = imageField;
                }
                if (appPath != null && File.Exists(appPath))
                {
                    return new Bitmap(appPath);
                }
                return null;
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string label
        {
            get
            {
                return labelField;
            }
            set
            {
                labelField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string image
        {
            get
            {
                return imageField;
            }
            set
            {
                imageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string data
        {
            get
            {
                return dataField;
            }
            set
            {
                dataField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute]
        public string tooltip
        {
            get
            {
                return tooltipField;
            }
            set
            {
                tooltipField = value;
            }
        }
    }
}