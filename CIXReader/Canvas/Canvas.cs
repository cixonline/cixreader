// *****************************************************
// CIXReader
// Canvas.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Canvas
{
    /// <summary>
    /// The Canvas control implements a variation of a ScrollableControl that displays
    /// canvas item controls.
    /// </summary>
    public sealed class Canvas : ScrollableControl
    {
        private bool _enableScrollIntoView;
        private bool _inLayout;
        private int _selectedItemIndex = -1;
        private bool _displayRectUpdate = true;

        /// <summary>
        /// Defines the delegate for CanvasItemAction event notifications.
        /// </summary>
        /// <param name="sender">The Canvas object</param>
        /// <param name="e">Additional event data for item</param>
        public delegate void CanvasItemActionHandler(object sender, CanvasItemArgs e);

        /// <summary>
        /// Defines the delegate for CanvasItemClick event notifications.
        /// </summary>
        /// <param name="sender">The Canvas object</param>
        /// <param name="e">Additional event data for item</param>
        public delegate void CanvasItemClickHandler(object sender, CanvasItemArgs e);

        /// <summary>
        /// Defines the delegate for LinkClicked event notifications.
        /// </summary>
        /// <param name="sender">The Canvas object</param>
        /// <param name="e">Additional event data for the layout</param>
        public delegate void LinkClickedHandler(object sender, LinkClickedEventArgs e);

        /// <summary>
        /// Defines the delegate for KeyPress event notifications.
        /// </summary>
        /// <param name="sender">The canvas object</param>
        /// <param name="e">Additional event data for the layout</param>
        public delegate void KeyPressedHandler(object sender, KeyEventArgs e);

        /// <summary>
        /// Event handler for notifying a delegate that an action occured in a component on
        /// a canvas item.
        /// </summary>
        public event CanvasItemActionHandler CanvasItemAction;

        /// <summary>
        /// Event handler for notifying a delegate that a link was clicked within an item
        /// in the list.
        /// </summary>
        public event LinkClickedHandler LinkClicked;

        /// <summary>
        /// Event handler for notifying a delegate that a key was pressed.
        /// </summary>
        public event KeyPressedHandler KeyPressed;

        /// <summary>
        /// Instantiates a new, empty, Canvas control.
        /// </summary>
        public Canvas()
        {
            Items = new ObservableCollection<CanvasItem>();
            Items.CollectionChanged += ItemsOnCollectionChanged;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, true);

            SelectionColour = SystemColors.Highlight;
            SeparatorColour = SystemColors.GrayText;

            HasImages = true;
            IndentationOffset = 10;
        }

        /// <summary>
        /// A collection of all the items to be rendered in the canvas.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableCollection<CanvasItem> Items { get; set; }

        /// <summary>
        /// Specifies whether selections are permitted.
        /// </summary>
        public bool AllowSelection { get; set; }

        /// <summary>
        /// Return the selection colour.
        /// </summary>
        public Color SelectionColour { get; set; }

        /// <summary>
        /// Return the separator colour.
        /// </summary>
        public Color SeparatorColour { get; set; }

        /// <summary>
        /// Get or set the colour to use for indented blocks
        /// </summary>
        public Color CommentColour { get; set; }

        /// <summary>
        /// Get or set a flag which tracks whether layout is suspended.
        /// </summary>
        public bool IsLayoutSuspended { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether inline images are expanded.
        /// </summary>
        public bool ExpandInlineImages { get; set; }

        /// <summary>
        /// Return the indentation offset.
        /// </summary>
        public int IndentationOffset { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether markup codes are interpreted.
        /// </summary>
        public bool DisableMarkup { get; set; }

        /// <summary>
        /// Set or get the currently selected item. Only one item can be
        /// selected in the Canvas control. Returns null if no item is
        /// currently selected.
        /// </summary>
        public CanvasItem SelectedItem
        {
            get { return (_selectedItemIndex >= 0 && _selectedItemIndex < Controls.Count) ? (CanvasItem)Controls[_selectedItemIndex] : null; }
            set
            {
                int indexOfItem = Controls.IndexOf(value);
                if (indexOfItem >= 0)
                {
                    SelectItem(indexOfItem, true);
                }
            }
        }

        /// <summary>
        /// Returns the element that currently has a selection.
        /// </summary>
        public CanvasItem TextSelectedItem { get; private set; }

        /// <summary>
        /// Get or set a flag which indicates whether the canvas items can have images
        /// </summary>
        public bool HasImages { get; set; }

        /// <summary>
        /// Start a batch update of items and disable rendering until the corresponding
        /// EndUpdate is called.
        /// </summary>
        public void BeginUpdate()
        {
            IsLayoutSuspended = true;
            SuspendLayout();
        }

        /// <summary>
        /// Marks the end of batch update of items and proceeds to render.
        /// </summary>
        public void EndUpdate(CanvasItem rootItem)
        {
            IsLayoutSuspended = false;
            LayoutControls(rootItem);
            ResumeLayout(true);
        }

        /// <summary>
        /// Called when the Items collection is changed. We make the necessary modifications to the
        /// Controls collection.
        /// </summary>
        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    _selectedItemIndex = -1;
                    TextSelectedItem = null;
                    Controls.Clear();
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (CanvasItem item in e.OldItems)
                    {
                        if (item.Selected)
                        {
                            _selectedItemIndex = -1;
                        }
                        if (_selectedItemIndex >= e.OldStartingIndex)
                        {
                            --_selectedItemIndex;
                        }
                        Controls.Remove(item);
                    }
                    _displayRectUpdate = true;
                    if (!IsLayoutSuspended)
                    {
                        LayoutControls(null);
                    }
                    break;

                case NotifyCollectionChangedAction.Add:
                    int index = e.NewStartingIndex;
                    foreach (CanvasItem item in e.NewItems)
                    {
                        Controls.Add(item);
                        Controls.SetChildIndex(item, index);
                        if (_selectedItemIndex >= index)
                        {
                            ++_selectedItemIndex;
                        }
                        ++index;
                    }
                    _displayRectUpdate = true;
                    if (!IsLayoutSuspended)
                    {
                        LayoutControls((CanvasItem) e.NewItems[0]);
                    }
                    break;
            }
        }

        /// <summary>
        /// Implement our own invalidate.
        /// </summary>
        public new void Invalidate()
        {
            foreach (CanvasItem item in Items)
            {
                item.CanvasItemLayout = null;
                item.Height = -1;
            }
            LayoutControls(null);
        }

        /// <summary>
        /// Handle the IsInputKey property on behalf of the canvas item
        /// since the actual key processing is done by the canvas.
        /// </summary>
        /// <param name="keyCode">Key code</param>
        /// <returns>True if we handle the key, false otherwise</returns>
        public bool HandleIsInputKey(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                case Keys.Home:
                case Keys.End:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Handle keys. For input navigation keys, only those recognised by
        /// HandleIsInputKey will arrive here.
        /// </summary>
        public void HandleOnKeyDown(KeyEventArgs e)
        {
            if (KeyPressed != null)
            {
                KeyPressed(this, e);
            }
            if (AllowSelection && !e.Handled)
            {
                int nextSelectedItem;
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        nextSelectedItem = _selectedItemIndex - 1;
                        break;
                    case Keys.Down:
                        nextSelectedItem = _selectedItemIndex + 1;
                        break;
                    case Keys.End:
                        nextSelectedItem = Controls.Count - 1;
                        break;
                    case Keys.Home:
                        nextSelectedItem = 0;
                        break;
                    default:
                        return;
                }
                SelectItem(nextSelectedItem, true);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Respond to a link being clicked in a static text item.
        /// </summary>
        /// <param name="item">The item that raised this event</param>
        /// <param name="link">The URL</param>
        public void HandleLink(CanvasItem item, string link)
        {
            if (LinkClicked != null)
            {
                LinkClicked(item, new LinkClickedEventArgs(link));
            }
        }

        /// <summary>
        /// A selection was made on an item. We keep track of the item with the selection
        /// separately from the selected item.
        /// </summary>
        internal void HandleSelection(CanvasItem canvasItem)
        {
            if (TextSelectedItem != null && canvasItem != TextSelectedItem)
            {
                TextSelectedItem.ClearSelection();
            }
            TextSelectedItem = canvasItem;
        }

        /// <summary>
        /// Respond to a canvas item component being selected. ComponentID is the ID
        /// of the actual component under the mouse cursor.
        /// </summary>
        /// <param name="item">The item that raised this select</param>
        /// <param name="component">The component selected</param>
        internal void HandleSelect(CanvasItem item, CanvasElementBase component)
        {
            if (component == null || component.ID == ActionID.None)
            {
                if (AllowSelection)
                {
                    Focus();

                    int selectedItem = Controls.IndexOf(item);
                    if (selectedItem != _selectedItemIndex)
                    {
                        SelectItem(selectedItem, true);
                    }
                }
            }
            if (CanvasItemAction != null)
            {
                CanvasItemAction(this, new CanvasItemArgs
                {
                    Item = (component != null) ? component.ID : ActionID.None,
                    Tag = (component != null) ? component.Tag : null,
                    Control = item
                });
            }
        }

        /// <summary>
        /// Layout all the canvas items from either the given item or from
        /// the start if null is specified.
        /// </summary>
        public void LayoutControls(CanvasItem firstControl)
        {
            if (!_inLayout)
            {
                bool layoutAll = (firstControl == null);

                _inLayout = true;
                int index = (layoutAll) ? 0 : Controls.IndexOf(firstControl);
                if (index >= 0)
                {
                    Point point;

                    if (index > 0)
                    {
                        point = Controls[index - 1].Location;
                        point.Y += Controls[index - 1].Height;
                    }
                    else
                    {
                        point = AutoScrollPosition;
                    }

                    int topOffset = Math.Abs(point.Y);
                    int yCount = 0;

                    while (index < Controls.Count && (layoutAll || yCount < ClientRectangle.Bottom * 2))
                    {
                        CanvasItem item = (CanvasItem) Controls[index];
                        BoundsSpecified flags = BoundsSpecified.None;

                        if (item.Bounds.X != point.X)
                        {
                            flags |= BoundsSpecified.X;
                        }
                        if (item.Bounds.Y != point.Y)
                        {
                            flags |= BoundsSpecified.Y;
                        }
                        if (item.Bounds.Width != ClientRectangle.Width)
                        {
                            flags |= BoundsSpecified.Width;
                        }
                        if (item.Bounds.Height != item.Height)
                        {
                            flags |= BoundsSpecified.Height;
                        }
                        item.SetBounds(point.X, point.Y, ClientRectangle.Width, item.Height, flags);
                        
                        point.Y += item.Height;
                        yCount += item.Height - topOffset;

                        topOffset = 0;
                        ++index;
                    }

                    // If the collection changed then we need to update the virtual display
                    // rectangle bounds too.
                    if (_displayRectUpdate)
                    {
                        while (index < Controls.Count)
                        {
                            CanvasItem item = (CanvasItem) Controls[index++];
                            point.Y += item.Height;
                        }
                        AutoScrollMinSize = new Size(DisplayRectangle.Width, point.Y);

                        _displayRectUpdate = false;
                    }
                }
                _inLayout = false;
            }
        }

        /// <summary>
        /// On each scroll, update the bounds of the ones we're going to show.
        /// </summary>
        /// <param name="se"></param>
        protected override void OnScroll(ScrollEventArgs se)
        {
            CanvasItem topItem = (CanvasItem) GetChildAtPoint(new Point(0, 0));
            LayoutControls(topItem);
            base.OnScroll(se);
        }

        /// <summary>
        /// Mouse wheel scrolls don't cause the OnScroll event to be hit so we need
        /// to handle that separately.
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            CanvasItem topItem = (CanvasItem)GetChildAtPoint(new Point(0, 0));
            LayoutControls(topItem);
            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Select the item at the specified index, removing any existing selection.
        /// </summary>
        private void SelectItem(int newSelection, bool scrollToView)
        {
            if (newSelection >= 0 && newSelection < Controls.Count && newSelection != _selectedItemIndex)
            {
                CanvasItem previousItem = SelectedItem;
                CanvasItem item = (CanvasItem)Controls[newSelection];

                if (AllowSelection)
                {
                    ClearSelection();
                    _selectedItemIndex = newSelection;
                    item.Selected = true;
                }

                if (scrollToView)
                {
                    _enableScrollIntoView = true;

                    int midzoneTop = (Height/2) - 30;
                    int midzoneBase = (Height/2) + 30;
                    if (item.Top >= 0 && item.Top < Height && previousItem != null && previousItem.Top > midzoneTop && previousItem.Top < midzoneBase)
                    {
                        int delta = item.Top - (Height / 2);
                        AutoScrollPosition = new Point(AutoScrollPosition.X, -(AutoScrollPosition.Y - delta));
                    }
                    else
                    {
                        ScrollControlIntoView(item);
                    }

                    _enableScrollIntoView = false;
                }
            }
        }

        /// <summary>
        /// Remove the selection from the specified item.
        /// </summary>
        private void ClearSelection()
        {
            if (_selectedItemIndex >= 0)
            {
                CanvasItem item = (CanvasItem) Controls[_selectedItemIndex];
                item.Selected = false;
                _selectedItemIndex = -1;
            }
        }

        /// <summary>
        /// Override ScrollToControl to stop the scrollable control container
        /// scrolling us around whenever it gets focus or is resized.
        /// </summary>
        protected override Point ScrollToControl(Control activeControl)
        {
            return (_enableScrollIntoView) ? base.ScrollToControl(activeControl) : DisplayRectangle.Location;
        }

        /// <summary>
        /// If the container control gets focus, set the focus explicitly on the selected
        /// item. Otherwise set it on the first visible item.
        /// </summary>
        /// <param name="e">Focus event arguments</param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (_selectedItemIndex >= 0)
            {
                CanvasItem item = (CanvasItem) Controls[_selectedItemIndex];
                item.Focus();
            }
            else
            {
                CanvasItem item = (CanvasItem) GetChildAtPoint(new Point(0, 0));
                if (item != null)
                {
                    item.Focus();
                }
            }
        }

        /// <summary>
        /// Override the resize event to force a re-layout of the content.
        /// </summary>
        /// <param name="e">Resize event arguments</param>
        protected override void OnResize(EventArgs e)
        {
            if (!_inLayout)
            {
                int index = 0;
                while (index < Controls.Count)
                {
                    CanvasItem item = (CanvasItem) Controls[index];
                    item.SetBounds(0, 0, ClientRectangle.Width, 0, BoundsSpecified.Width);
                    ++index;
                }

                LayoutControls(null);
            }
        }
    }
}