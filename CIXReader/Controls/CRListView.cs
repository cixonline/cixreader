// *****************************************************
// CIXReader
// CRListView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/12/2014 13:56
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using System.Windows.Forms;

namespace CIXReader.Controls
{
    /// <summary>
    /// Contains helper methods to change extended styles on ListView, including enabling double buffering.
    /// Based on Giovanni Montrone's article on <see cref="http://www.codeproject.com/KB/list/listviewxp.aspx" />
    /// </summary>
    public sealed partial class CRListView : ListView
    {
        public void SetHeight(int height)
        {
            ImageList imgList = new ImageList {ImageSize = new Size(1, height)};
            SmallImageList = imgList;
        }

        public int SearchRow { get; set; }

        /// <summary>
        /// Start a drag/drop operation.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.All | DragDropEffects.Link);
        }
    }
}