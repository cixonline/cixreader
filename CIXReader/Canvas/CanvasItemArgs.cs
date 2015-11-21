// *****************************************************
// CIXReader
// CanvasItemArgs.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 25/06/2015 17:36
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXReader.Utilities;

namespace CIXReader.Canvas
{
    /// <summary>
    /// Class that encapsulates the arguments for a canvas item event
    /// </summary>
    public sealed class CanvasItemArgs : EventArgs
    {
        /// <summary>
        /// The item.
        /// </summary>
        public ActionID Item { get; set; }

        /// <summary>
        /// The item being laid out
        /// </summary>
        public CanvasItem Control { get; set; }

        /// <summary>
        /// The tag on the item.
        /// </summary>
        public object Tag { get; set; }
    }
}