// *****************************************************
// CIXReader
// CRToolbarItem.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 20/05/2015 14:16
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using CIXReader.Utilities;
using System.Windows.Forms;

namespace CIXReader.Controls
{
    /// <summary>
    /// Type of the toolbar item
    /// </summary>
    public enum CRToolbarItemType
    {
        /// <summary>
        /// Item is invalid
        /// </summary>
        None,

        /// <summary>
        /// Item is a toolbar button
        /// </summary>
        Button,

        /// <summary>
        /// Item is a search field
        /// </summary>
        Search,

        /// <summary>
        /// Item is a flexible space insert
        /// </summary>
        FlexibleSpace,

        /// <summary>
        /// Item is a fixed width space insert
        /// </summary>
        Space
    };

    public sealed class CRToolbarItem
    {
        public CRToolbarItem(ToolbarDataItem item)
        {
            DataItem = item;
        }

        /// <summary>
        /// Expose the underlying control for this item
        /// </summary>
        public Control Control { get; internal set; }

        /// <summary>
        /// The action ID associated with this item
        /// </summary>
        public ActionID ID
        {
            get { return DataItem.ID; }
        }

        /// <summary>
        /// User supplied extra data associated with the item
        /// </summary>
        public object ExtraData
        {
            get { return DataItem.data; }
        }

        /// <summary>
        /// The button type.
        /// </summary>
        public CRToolbarItemType Type
        {
            get { return DataItem.Type; }
        }

        /// <summary>
        /// The button name
        /// </summary>
        public string Name
        {
            get { return DataItem.name; }
        }

        /// <summary>
        /// The button image
        /// </summary>
        public Image Image
        {
            get { return DataItem.Image;  }
        }

        /// <summary>
        /// The button tooltip
        /// </summary>
        public string Tooltip
        {
            get { return DataItem.tooltip; }
        }

        /// <summary>
        /// Underlying data item.
        /// </summary>
        public ToolbarDataItem DataItem { get; private set; }
    }
}