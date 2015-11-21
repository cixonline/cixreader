// *****************************************************
// CIXReader
// DrawRectElements.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/06/2015 12:36
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;

namespace CIXReader.Utilities
{
    /// <summary>
    /// Structure that records the client position of the various thread row elements
    /// computed by ComputeDrawRectElements.
    /// </summary>
    public struct DrawRectElements
    {
        /// <summary>
        /// Output rectangle of the selection and focus
        /// </summary>
        public Rectangle BoundaryRect { get; set; }

        /// <summary>
        /// Output rectangle of the expander rectangle if present. This may be empty.
        /// </summary>
        public Rectangle ExpanderRect { get; set; }

        /// <summary>
        /// Output rectangle of the Read/Unread icon.
        /// </summary>
        public Rectangle ReadRect { get; set; }

        /// <summary>
        /// Output rectangle of the Flag icon.
        /// </summary>
        public Rectangle StarRect { get; set; }

        /// <summary>
        /// Output rectangle of the date element.
        /// </summary>
        public Rectangle DateRect { get; set; }

        /// <summary>
        /// Output rectangle of the author name element.
        /// </summary>
        public Rectangle AuthorRect { get; set; }

        /// <summary>
        /// Output rectangle of the ID field element.
        /// </summary>
        public Rectangle IDRect { get; set; }

        /// <summary>
        /// Output rectangle of the subject line element.
        /// </summary>
        public Rectangle SubjectRect { get; set; }

        /// <summary>
        /// Output rectangle of the first separator.
        /// </summary>
        public Rectangle Separator1Rect { get; set; }

        /// <summary>
        /// Output rectangle of the second separator.
        /// </summary>
        public Rectangle Separator2Rect { get; set; }
    }
}