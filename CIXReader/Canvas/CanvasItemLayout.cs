// *****************************************************
// CIXReader
// CanvasItemLayout.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Canvas
{
    /// <summary>
    /// The CanvasItemLayout class tracks layouts of canvas items
    /// </summary>
    public sealed class CanvasItemLayout
    {
        /// <summary>
        /// Specifies the type of the element
        /// </summary>
        public enum ItemType
        {
            /// <summary>
            /// A newline after the last element
            /// </summary>
            NewLine,

            /// <summary>
            /// A new column after the last element
            /// </summary>
            NewColumn,

            /// <summary>
            /// Start a new section
            /// </summary>
            BeginSection,

            /// <summary>
            /// End a section
            /// </summary>
            EndSection,

            /// <summary>
            /// A canvas item element.
            /// </summary>
            Component
        }

        private static Font _defaultFont;

        private readonly List<CanvasElementBase> _components = new List<CanvasElementBase>();

        /// <summary>
        /// Get and set the current write position.
        /// </summary>
        public Point Position;

        private Rectangle _bounds;
        private Rectangle _clientBounds;
        private int _columnWidth;
        private int _lineHeight;
        private int _lineStartX;
        private readonly Stack<Point> _sectionStack = new Stack<Point>(); 

        private Color _backColor;

        /// <summary>
        /// Instantiates a new CanvasItemLayout instance for the specified container and using
        /// the given client bounds.
        /// </summary>
        /// <param name="container">The CanvasItem to which this layout belongs</param>
        /// <param name="clientBounds">A rectangle that indicates the client boundaries</param>
        public CanvasItemLayout(CanvasItem container, Rectangle clientBounds)
        {
            _backColor = SystemColors.Control;

            Container = container;
            ResetPosition(clientBounds);
            Padding = new Size(4, 4);
        }

        /// <summary>
        /// Return the default stock font
        /// </summary>
        public static Font DefaultFont
        {
            get { return _defaultFont ?? (_defaultFont = UI.System.Font); }
        }

        /// <summary>
        /// Return the entire bounds of the whole layout
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set
            {
                ResetPosition(value);
                foreach (CanvasElementBase component in _components)
                {
                    switch (component.Type)
                    {
                        case ItemType.Component:
                            component.SetPosition(Position, _clientBounds);
                            if (component.Visible)
                            {
                                Forward(component.Width, component.Height);
                            }
                            break;

                        case ItemType.NewColumn:
                            NewColumn();
                            break;

                        case ItemType.BeginSection:
                            BeginSection();
                            break;

                        case ItemType.EndSection:
                            EndSection();
                            break;

                        case ItemType.NewLine:
                            Newline();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the component with the given ID. If more than one component shares
        /// the same ID, the first one is returned.
        /// </summary>
        /// <param name="id">ID of the componet</param>
        /// <returns>A CanvasElementBase, or null</returns>
        public CanvasElementBase this[ActionID id]
        {
            get { return _components.FirstOrDefault(cmn => cmn.ID == id); }
        }

        /// <summary>
        /// Get and set the inter-component padding.
        /// </summary>
        public Size Padding { get; set; }

        /// <summary>
        /// Get and set the container canvas item.
        /// </summary>
        public CanvasItem Container { get; set; }

        /// <summary>
        /// Return a list of all components.
        /// </summary>
        public IEnumerable<CanvasElementBase> Items
        {
            get { return _components; }
        }

        /// <summary>
        /// Set the background colour of this item.
        /// </summary>
        public Color BackColor
        {
            set
            {
                _backColor = value;
                foreach (CanvasElementBase component in _components)
                {
                    component.BackColor = _backColor;
                }
            }
        }

        /// <summary>
        /// Return any selection in the item.
        /// </summary>
        public string SelectedText
        {
            get
            {
                StringBuilder str = new StringBuilder();
                foreach (CanvasElementBase component in _components)
                {
                    str.Append(component.GetSelection());
                }
                return str.ToString();
            }
        }

        /// <summary>
        /// Return any selection in the item.
        /// </summary>
        public void SelectAll()
        {
            foreach (CanvasElementBase component in _components)
            {
                component.SelectAll();
            }
        }

        /// <summary>
        /// Clear the active selection.
        /// </summary>
        public void ClearSelection()
        {
            foreach (CanvasElementBase component in _components)
            {
                component.ClearSelection();
            }
        }

        /// <summary>
        /// Reset the layout to empty.
        /// </summary>
        public void Clear()
        {
            foreach (CanvasElementBase component in _components)
            {
                component.Remove();
            }
            _components.Clear();
        }

        /// <summary>
        /// Add a new component and advance forward on the current line.
        /// </summary>
        public void Add(CanvasElementBase newElement)
        {
            _components.Add(newElement);
            newElement.Add(this, _clientBounds, Position);
            Forward(newElement.Width, newElement.Height);
        }

        /// <summary>
        /// Move to the start of the next line.
        /// </summary>
        public void AddNewLine()
        {
            _components.Add(new CanvasElementBase(ItemType.NewLine));
            Newline();
        }

        /// <summary>
        /// Move to the start of the next column.
        /// </summary>
        public void AddNewColumn()
        {
            _components.Add(new CanvasElementBase(ItemType.NewColumn));
            NewColumn();
        }

        /// <summary>
        /// Define a new section.
        /// </summary>
        public void AddBeginSection()
        {
            _components.Add(new CanvasElementBase(ItemType.BeginSection));
            BeginSection();
        }

        /// <summary>
        /// Move to the start of the next line.
        /// </summary>
        public void AddEndSection()
        {
            _components.Add(new CanvasElementBase(ItemType.EndSection));
            EndSection();
        }

        /// <summary>
        /// Move to the start of the next line.
        /// </summary>
        public void SoftNewLine()
        {
            Newline();
        }

        /// <summary>
        /// Determine the item at the specified coordinates.
        /// </summary>
        /// <param name="pt">Coordinates, relative to top left of the item control</param>
        /// <returns>The item at the coordinates</returns>
        public CanvasElementBase ItemFromPosition(Point pt)
        {
            return _components.FirstOrDefault(component => component.IsInBounds(pt.X, pt.Y));
        }

        /// <summary>
        /// Render the entire layout.
        /// </summary>
        /// <param name="graphics">The graphics item to use</param>
        public void Draw(Graphics graphics)
        {
            foreach (CanvasElementBase component in _components.Where(component => component.Visible))
            {
                component.Draw(graphics);
            }
        }

        /// <summary>
        /// Explicitly advance forward by the given increments vertically and horizontally
        /// </summary>
        private void Forward(int width, int height)
        {
            Point lastPosition = _sectionStack.Peek();
            Position.X += width + Padding.Width;
            if (Position.X > _bounds.Width)
            {
                _bounds.Width = Position.X - lastPosition.X;
            }
            if (Position.Y + height > _bounds.Height)
            {
                _bounds.Height = Position.Y + height;
            }
            if (_lineHeight < height)
            {
                _lineHeight = height;
            }
            if (_columnWidth < width)
            {
                _columnWidth = width;
            }
        }

        /// <summary>
        /// Reset the position to the start for use when recalculating the layout.
        /// </summary>
        /// <param name="bounds">Client bounds rectangle</param>
        private void ResetPosition(Rectangle bounds)
        {
            _sectionStack.Clear();
            _clientBounds = bounds;
            _columnWidth = 0;
            _lineHeight = 0;
            Position = new Point(_clientBounds.X, _clientBounds.Y);
            _lineStartX = Position.X;

            BeginSection();
        }

        /// <summary>
        /// Move to the start of the next line.
        /// </summary>
        private void Newline()
        {
            _bounds.Width -= Padding.Width;
            Position.X = _lineStartX;
            Position.Y += _lineHeight;
            _lineHeight = 0;
        }

        /// <summary>
        /// Move to the start of the next line.
        /// </summary>
        private void NewColumn()
        {
            Point lastPosition = _sectionStack.Peek();
            Position.X = lastPosition.X + _bounds.Width;
            _lineStartX = Position.X;
            _lineHeight = 0;
            _columnWidth = 0;
        }

        /// <summary>
        /// Define the start of a new section.
        /// </summary>
        private void BeginSection()
        {
            _sectionStack.Push(Position);
            _bounds = new Rectangle();
        }

        /// <summary>
        /// Define the end of a section.
        /// </summary>
        private void EndSection()
        {
            Point lastPosition = _sectionStack.Pop();
            _lineStartX = lastPosition.X;
            Newline();
        }
    }
}