// *****************************************************
// CIXReader
// CanvasHoverArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/02/2016 12:13
//  
// Copyright (C) 2013-2016 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;

namespace CIXReader.Canvas
{
    public sealed class CanvasHoverArgs : EventArgs
    {
        /// <summary>
        /// The link text.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// The item being laid out
        /// </summary>
        public CanvasElementBase Component { get; set; }

        /// <summary>
        /// The tag on the item.
        /// </summary>
        public Point Location { get; set; }
    }
}