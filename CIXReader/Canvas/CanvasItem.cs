// *****************************************************
// CIXReader
// CanvasItem.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Canvas
{
    /// <summary>
    /// A CanvasItem defines the base class for a single canvas item.
    /// </summary>
    public partial class CanvasItem : UserControl
    {
        private const int _separatorBarHeight = 2;
        private const int _firstIndentOffset = 60;
        private const int _interItemSpacing = 4;

        /// <summary>
        /// The parent thread view
        /// </summary>
        protected readonly Canvas _view;

        private bool _autosize;
        private readonly bool _hasSeparator;
        private int _height;

        // Flags
        private bool _isSelected;

        // Parent view controller.
        private CanvasItemLayout _layout;
        private int _level;
        private bool _suspendPaint;
        private Point _lastMousePosition;
        private readonly ToolTip _tooltip;

        private Rectangle _dragBoxFromMouseDown;

        /// <summary>
        /// Initialises a new instance of the <see cref="CanvasItem"/> class with the
        /// specified Canvas and optional separator.
        /// </summary>
        protected CanvasItem(Canvas view, bool hasSeparator)
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, false);

            _view = view;
            _height = -1;
            _hasSeparator = hasSeparator;

            _tooltip = new ToolTip();
            _lastMousePosition = new Point(0, 0);
        }

        /// <summary>
        /// Get or set the ID of this canvas item.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get or set the ID of the parent item.
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// Get or set the image associated with this item.
        /// </summary>
        public Image Image { protected get; set; }

        /// <summary>
        /// Get or set the active canvas item layout.
        /// </summary>
        public CanvasItemLayout CanvasItemLayout
        {
            get { 
                if (_layout == null)
                {
                    _suspendPaint = true;
                    _layout = BuildLayout(); 
                    _suspendPaint = false;
                }
                return _layout;
            }
            set
            {
                if (_layout != null)
                {
                    _layout.Clear();
                }
                _layout = value;
            }
        }

        /// <summary>
        /// Return the item height, which is computed on demand.
        /// </summary>
        public new int Height
        {
            set { _height = value; }
            get
            {
                if (_height < 0)
                {
                    ComputeHeight();
                }
                return _height;
            }
        }

        /// <summary>
        /// Set the indent level of this canvas item. Because changing the indent
        /// level affects the height, we need to force a computation of the item
        /// size in response.
        /// </summary>
        public int Level
        {
            protected get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    Height = -1;
                }
            }
        }

        /// <summary>
        /// Get or set whether this canvas item is selected. Selected items are
        /// shown with the selection colour.
        /// </summary>
        public virtual bool Selected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Automatically compute the size of this canvas item.
        /// </summary>
        public override bool AutoSize
        {
            get { return _autosize; }
            set
            {
                if (_autosize != value)
                {
                    _autosize = value;

                    SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
                    base.AutoSize = value;
                    ComputeHeight();
                    Invalidate();

                    OnAutoSizeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Get or set the value that indicates whether or not this item is preceded
        /// by a separator bar.
        /// </summary>
        public bool Separator
        {
            get { return _hasSeparator; }
        }

        /// <summary>
        /// Override the default padding to give us a border around the edges.
        /// </summary>
        protected override Padding DefaultPadding
        {
            get { return new Padding(4); }
        }

        /// <summary>
        /// Override the default margin to give us inter-item spacing.
        /// </summary>
        protected override Padding DefaultMargin
        {
            get { return new Padding(1); }
        }

        /// <summary>
        /// Override the default size for the item. This can't be determined until
        /// we have actual content so this is slightly spurious.
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(90, 90); }
        }

        /// <summary>
        /// Return the selection colour.
        /// </summary>
        public Color SelectionColour
        {
            get { return _view.SelectionColour; }
        }

        /// <summary>
        /// Return the separator bar colour.
        /// </summary>
        public Color SeparatorColour
        {
            get { return _view.SeparatorColour; }
        }

        /// <summary>
        /// Get or set the item background colour
        /// </summary>
        public Color ItemColour { get; set; }

        /// <summary>
        /// Return the separator bar colour.
        /// </summary>
        public int IndentationOffset
        {
            get { return _view.IndentationOffset; }
        }

        /// <summary>
        /// Returns the selection in the item.
        /// </summary>
        public string Selection
        {
            get { return CanvasItemLayout.SelectedText; }
        }

        /// <summary>
        /// Select all text in the item.
        /// </summary>
        public void SelectAll()
        {
            CanvasItemLayout.SelectAll();
        }

        /// <summary>
        /// Pass the clear selection through to the canvas item.
        /// </summary>
        public void ClearSelection()
        {
            CanvasItemLayout.ClearSelection();
        }

        /// <summary>
        /// Return the client rectangle for this item.
        /// </summary>
        protected Rectangle DrawRectangle
        {
            get
            {
                Rectangle drawRect = new Rectangle(new Point(0, 0), _view.ClientSize);

                // Allow space for the separator
                if (Separator)
                {
                    drawRect.Y += _separatorBarHeight;
                    drawRect.Height -= _separatorBarHeight;
                }

                // Indent for canvas item level.
                Reduce(ref drawRect, GetIndent(), _interItemSpacing);
                Reduce(ref drawRect, Margin);
                Reduce(ref drawRect, Padding);

                return drawRect;
            }
        }

        /// <summary>
        /// Invalidate this item contents, forcing it to be re-laid out. This is
        /// accomplished by deleting the existing layout.
        /// </summary>
        public void InvalidateItem()
        {
            CanvasItemLayout = null;
            RaiseRelayout();
            Invalidate();
        }

        /// <summary>
        /// Insert a separator item in the layout.
        /// </summary>
        /// <param name="layout">A canvas layout object</param>
        /// <param name="height">Height of line</param>
        protected void AddSeparatorItem(CanvasItemLayout layout, int height)
        {
            const string _separatorChar = "•";

            layout.Add(new CanvasText
            {
                Text = _separatorChar,
                Font = Font,
                LineHeight = height,
                ForeColour = Selected ? UI.System.SelectionTextColour : UI.Forums.HeaderFooterColour,
            });
        }

        /// <summary>
        /// This must be overridden by the inherited classes to build the layout
        /// on demand.
        /// </summary>
        protected virtual CanvasItemLayout BuildLayout()
        {
            return null;
        }

        /// <summary>
        /// Compute the size of this item based on its layout plus margin and padding. The
        /// item width is always inherited from the parent, whereas the height is layout
        /// dependent and requires we do an update.
        /// </summary>
        private void ComputeHeight()
        {
            int height = CanvasItemLayout.Bounds.Height + Margin.Bottom + Padding.Bottom;
            if (Separator)
            {
                height += _separatorBarHeight;
            }
            Height = height;
        }

        /// <summary>
        /// When the item size is changed, reset the bounds on the item which
        /// causes the layout to be recalculated.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (Bounds.Y < _view.ClientRectangle.Bottom)
            {
                CanvasItemLayout.Bounds = DrawRectangle;
            }
        }

        /// <summary>
        /// Handles the key down event when the control has focus and passes it up
        /// to the parent canvas.
        /// </summary>
        /// <param name="e">Key event arguments</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            _view.HandleOnKeyDown(e);
        }

        /// <summary>
        /// Handles the IsInputKey event and passes it up to the parent canvas
        /// to respond.
        /// </summary>
        /// <param name="keyData">The key being tested</param>
        /// <returns>True if we need to handle this key, false otherwise</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            return _view.HandleIsInputKey(keyData);
        }

        /// <summary>
        /// Handle a keyboard down event from a component.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs keyEventArgs)
        {
            _view.HandleOnKeyDown(keyEventArgs);
        }

        /// <summary>
        /// Raises an event on a component in the canvas item. The event is passed up to the
        /// parent canvas to notify the owner.
        /// </summary>
        /// <param name="item">The item that raised the event</param>
        public void RaiseEvent(CanvasElementBase item)
        {
            _view.HandleSelect(this, item);
        }

        /// <summary>
        /// Raises a selection event on a component in the canvas item. The event is passed
        /// up to the parent canvas so it can keep track of which items have selection.
        /// </summary>
        public void RaiseSelection()
        {
            _view.HandleSelection(this);
        }

        /// <summary>
        /// Raises an link on a component in the canvas item. The link is passed up to the
        /// parent canvas to notify the owner.
        /// </summary>
        /// <param name="link">A URL link</param>
        public void RaiseLink(string link)
        {
            _view.HandleLink(this, link);
        }

        /// <summary>
        /// Raises a hover on a component in the canvas item.
        /// </summary>
        /// <param name="args">Link hover arguments</param>
        public void RaiseHover(CanvasHoverArgs args)
        {
            _view.HandleHover(this, args);
        }

        /// <summary>
        /// Force a re-layout of the view starting with this control. This is raised when
        /// the layout determines that the control size has changed asynchronously.
        /// </summary>
        public void RaiseRelayout()
        {
            if (InvokeRequired)
            {
                // Make sure UI code is run on the main thread in the case where we're fired
                // from within a different thread.
                Invoke((MethodInvoker) (RaiseRelayout));
                return;
            }

            _height = -1;
            if (!_view.IsLayoutSuspended)
            {
                CanvasItemLayout.Bounds = DrawRectangle;
                _view.LayoutControls(this);
            }
        }

        /// <summary>
        /// Handle mouse clicks on the control.
        /// </summary>
        /// <param name="e">Mouse event argument</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // In case this is a drag, remember where we started
            Size dragSize = SystemInformation.DragSize;
            _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
        }

        /// <summary>
        /// Clear the drag source when the mouse goes up.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _dragBoxFromMouseDown = Rectangle.Empty;
        }

        /// <summary>
        /// Handle mouse clicks on the control.
        /// </summary>
        /// <param name="e">Mouse event argument</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            _view.HandleSelect(this, CanvasItemLayout.ItemFromPosition(e.Location));
        }

        /// <summary>
        /// Handle mouse hover over an item to display a tooltip.
        /// </summary>
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            CanvasElementBase item = CanvasItemLayout.ItemFromPosition(_lastMousePosition);
            if (item != null && item.HasToolTip)
            {
                _tooltip.Show(item.ToolTipString, this, _lastMousePosition.X + 16, _lastMousePosition.Y, 2500);
            }
        }

        /// <summary>
        /// Handle mouse movement to set the cursor.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // OK - it is definitely a drag so handle it as such.
                    DoDragDrop(this, DragDropEffects.All | DragDropEffects.Link);
                    return;
                }
            }

            base.OnMouseMove(e);
            CanvasElementBase item = CanvasItemLayout.ItemFromPosition(e.Location);
            Cursor = (item != null && item.ID != ActionID.None) ? Cursors.Hand : Cursors.Default;
            _lastMousePosition = e.Location;
        }

        /// <summary>
        /// Reduce a rectangle by the specified margin on each side.
        /// </summary>
        /// <param name="rect">The rectangle to reduce</param>
        /// <param name="margin">Reduction margin</param>
        public void Reduce(ref Rectangle rect, Padding margin)
        {
            rect.X += margin.Left;
            rect.Y += margin.Top;
            rect.Width -= margin.Horizontal;
            rect.Height -= margin.Vertical;
        }

        /// <summary>
        /// Reduce a rectangle by the specified margin on each side.
        /// </summary>
        /// <param name="rect">The rectangle to reduce</param>
        /// <param name="width">Width by which to reduce</param>
        /// <param name="height">Height by which to reduce</param>
        public void Reduce(ref Rectangle rect, int width, int height)
        {
            rect.X += width;
            rect.Y += height;
            rect.Width -= width;
            rect.Height -= height;
        }

        /// <summary>
        /// Paint this item. We handle the painting for the background, separator, selection
        /// and the interior. The layout handles the painting for the components.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_suspendPaint)
            {
                base.OnPaint(e);

                Rectangle drawRect = ClientRectangle;

                // Draw the top border for all except the first item
                if (Separator)
                {
                    using (Pen separatorPen = new Pen(SeparatorColour))
                    {
                        e.Graphics.DrawLine(separatorPen, drawRect.X, drawRect.Y, drawRect.X + drawRect.Width, drawRect.Y);
                    }
                    drawRect.Y += _separatorBarHeight;
                    drawRect.Height -= _separatorBarHeight;
                }

                // Indent for canvas item level.
                Reduce(ref drawRect, GetIndent(), _interItemSpacing);

                // Draw the selection around the control.
                using (Pen edgePen = new Pen(Selected ? SelectionColour : BackColor))
                {
                    Color fillColour = Selected ? SelectionColour : ItemColour;
                    using (Brush backBrush = new SolidBrush(fillColour))
                    {
                        RectangleF rectangle = new RectangleF(drawRect.Left, drawRect.Top, drawRect.Width - 1, drawRect.Height - 1);
                        e.Graphics.FillRoundedRectangle(edgePen, backBrush, rectangle);
                    }
                }
                CanvasItemLayout.BackColor = Selected ? SelectionColour : ItemColour;

                // Draw the inner item contents.
                CanvasItemLayout.Draw(e.Graphics);
            }
        }

        /// <summary>
        /// Return the constrained item indent. The indent is always limited to half
        /// the view size maximum.
        /// </summary>
        private int GetIndent()
        {
            int firstIndent = _view.HasImages ? _firstIndentOffset : IndentationOffset;
            int indent = ((Level == 0) ? 0 : firstIndent) + Level * IndentationOffset;
            if (indent > _view.ClientSize.Width / 2)
            {
                indent = _view.ClientSize.Width / 2;
            }
            return indent;
        }
    }
}