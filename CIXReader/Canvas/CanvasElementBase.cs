// *****************************************************
// CIXReader
// CanvasElementBase.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 8:22 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Canvas
{
    /// <summary>
    /// Base class for component type canvas item layout elements.
    /// </summary>
    public class CanvasElementBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CanvasElementBase"/> class and assigns
        /// this component the default ID of ComponentTypes.None.
        /// </summary>
        public CanvasElementBase(CanvasItemLayout.ItemType type)
        {
            Type = type;
            Visible = true;
            ID = ActionID.None;
        }

        /// <summary>
        /// The type of this element.
        /// </summary>
        public CanvasItemLayout.ItemType Type { get; private set; }

        /// <summary>
        /// Get or set the margin around this component.
        /// </summary>
        public Rectangle Margin { protected get; set; }

        /// <summary>
        /// Specifies whether or not this element is visible.
        /// </summary>
        public bool Visible { get; protected set; }

        /// <summary>
        /// Get or set the ID of this component
        /// </summary>
        public ActionID ID { get; set; }

        /// <summary>
        /// Get or set the background colour of the component
        /// </summary>
        public virtual Color BackColor { get; set; }

        /// <summary>
        /// Get or set component specific tag data.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Get or set the tooltip string associated with the item.
        /// </summary>
        public string ToolTipString { get; set; }

        /// <summary>
        /// Return the width, in pixels, of this component.
        /// </summary>
        public int Width
        {
            get { return Bounds.Width; }
        }

        /// <summary>
        /// Get or set the height of this component.
        /// </summary>
        public int Height
        {
            get { return Bounds.Height; }
        }

        /// <summary>
        /// Override to handle removing this component from the layout.
        /// </summary>
        public void Remove()
        {
            if (Control != null)
            {
                Control parentControl = Control.Parent;
                parentControl.Controls.Remove(Control);
            }
        }

        /// <summary>
        /// Enable or disable the component.
        /// </summary>
        public bool Enabled
        {
            get { return (Control == null) || Control.Enabled; }
            set
            {
                if (Control != null)
                {
                    Control.Enabled = value;
                }
            }
        }

        /// <summary>
        /// Returns whether or not this item has an associated tooltip.
        /// </summary>
        public bool HasToolTip
        {
            get { return ToolTipString != null; }
        }

        /// <summary>
        /// Determine if the two coordinates are within the bounds of this component.
        /// </summary>
        /// <returns>True if we're within bounds, false otherwise</returns>
        public bool IsInBounds(int x, int y)
        {
            return Bounds.Contains(x, y);
        }

        /// <summary>
        /// Override to add the given component to the layout using the specified client bounds and
        /// position within the client.
        /// </summary>
        /// <param name="layout">The containing canvas item layout</param>
        /// <param name="clientBounds">The client bounds for the layout, relative to the canvas item top left corner</param>
        /// <param name="position">The current position within the layout</param>
        public virtual void Add(CanvasItemLayout layout, Rectangle clientBounds, Point position)
        {
        }

        /// <summary>
        /// Override to draw the given component in the layout.
        /// </summary>
        /// <param name="graphics">The graphics context for drawing.</param>
        public virtual void Draw(Graphics graphics)
        {
        }

        /// <summary>
        /// Adjusts the position of the component to the given position, constrained by
        /// the given client boundary.
        /// </summary>
        /// <param name="position">New position of the component</param>
        /// <param name="clientBounds">Client boundary</param>
        public virtual void SetPosition(Point position, Rectangle clientBounds)
        {
        }

        /// <summary>
        /// Return the selection in the element.
        /// </summary>
        public virtual string GetSelection()
        {
            return string.Empty;
        }

        /// <summary>
        /// Clears any existing selection in the element.
        /// </summary>
        public virtual void ClearSelection()
        {
        }

        /// <summary>
        /// Select all text
        /// </summary>
        public virtual void SelectAll()
        {
        }

        /// <summary>
        /// If the component is implemented as a .NET control, get or set
        /// the actual embedded control.
        /// </summary>
        protected Control Control { get; set; }

        /// <summary>
        /// Get or set the bounds of this component. This can only be
        /// accessed and modified by the derived classes. Use the Width and
        /// Height properties to access the component bounds.
        /// </summary>
        protected Rectangle Bounds { get; set; }
    }
}