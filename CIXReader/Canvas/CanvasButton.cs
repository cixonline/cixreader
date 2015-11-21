// *****************************************************
// CIXReader
// CanvasButton.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 8:29 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CIXReader.Canvas
{
    /// <summary>
    /// Canvas item element for a button.
    /// </summary>
    public sealed class CanvasButton : CanvasElementBase
    {
        private CanvasItemLayout _layout;

        /// <summary>
        /// Instantiates an instance of the CanvasButton.
        /// </summary>
        public CanvasButton() : base(CanvasItemLayout.ItemType.Component)
        {
        }

        /// <summary>
        /// Get or set the text displayed in the button.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Get or set the font used to render the text.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Get or set the foreground colour of the text.
        /// </summary>
        public Color ForeColour { get; set; }

        /// <summary>
        /// Get or set the height of the line on which this button appears.
        /// </summary>
        public int LineHeight { get; set; }

        /// <summary>
        /// Get or set the amount of spacing after the text, in pixels.
        /// </summary>
        public int SpaceAfter { get; set; }

        /// <summary>
        /// Add a new button control to the layout.
        /// </summary>
        public override void Add(CanvasItemLayout layout, Rectangle clientBounds, Point position)
        {
            Control = new Button
            {
                Text = Text,
                Font = Font,
                AutoSize = true,
                ForeColor = ForeColour,
                Location = position
            };

            Control.Click += ButtonOnClick;

            layout.Container.Controls.Add(Control);
            Bounds = new Rectangle(position.X, position.Y, Control.Size.Width, Math.Max(Control.Size.Height, LineHeight) + SpaceAfter);

            _layout = layout;
        }

        /// <summary>
        /// Handle a click on the button.
        /// </summary>
        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            _layout.Container.RaiseEvent(this);
        }

        /// <summary>
        /// Adjust the position of this item.
        /// </summary>
        public override void SetPosition(Point position, Rectangle clientBounds)
        {
            Control.Location = position;
            Bounds = new Rectangle(position.X, position.Y, Control.Size.Width, Math.Max(Control.Size.Height, LineHeight) + SpaceAfter);
        }
    }
}