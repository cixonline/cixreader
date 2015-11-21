// *****************************************************
// CIXReader
// CRTreeView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 21/10/2013 8:15 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CIXReader.Controls
{
    internal sealed class CRTreeView : TreeView
    {
        // Context icon variables
        private Timer _contextIconTimer;

        protected override void OnHandleCreated(EventArgs e)
        {
            if (!SystemInformation.TerminalServerSession)
            {
                DoubleBuffered = true;
            }
            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Get or set the context icon attached to this tree control.
        /// </summary>
        public Image ContextIcon { get; set; }

        /// <summary>
        /// Get or set the selected context icon attached to this tree control.
        /// </summary>
        public Image SelectedContextIcon { get; set; }

        /// <summary>
        /// Get or set the node in which the context icon is active.
        /// </summary>
        public TreeNode ContextNode { get; set; }

        /// <summary>
        /// Return the bounds of the context icon for the specified node.
        /// </summary>
        /// <param name="node">The tree node</param>
        /// <returns>A rectangle representing the context icon bounds</returns>
        public Rectangle ContextIconBounds(TreeNode node)
        {
            if (ContextIcon == null)
            {
                return Rectangle.Empty;
            }

            int height = ContextIcon.Height;
            int width = ContextIcon.Width;

            return new Rectangle
            {
                X = ClientRectangle.Width - (width + 2),
                Y = node.Bounds.Top + (node.Bounds.Height - height) / 2,
                Width = width,
                Height = height
            };
        }

        /// <summary>
        /// Invalidate the specified node and force it to be redrawn.
        /// </summary>
        public void InvalidateNode(TreeNode node)
        {
            Rectangle bounds = new Rectangle(0, node.Bounds.Y, ClientRectangle.Width, node.Bounds.Height);
            Invalidate(bounds);
            Update();
        }

        /// <summary>
        /// Handle mouse move events when a context icon is specified to support firing off
        /// update and click events for the context icon.
        /// </summary>
        /// <param name="e">The mouse move event</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            TreeNode node = GetNodeAt(e.Location);

            if (node != ContextNode && ContextIcon != null)
            {
                if (_contextIconTimer != null)
                {
                    _contextIconTimer.Stop();
                }

                if (ContextNode != null)
                {
                    TreeNode lastHoverNode = ContextNode;
                    ContextNode = null;
                    InvalidateNode(lastHoverNode);
                }

                // Start a timer to only show the context icon after 500ms as
                // long as we're still on the same node.
                _contextIconTimer = new Timer { Interval = 500 };
                _contextIconTimer.Tick += ContextMenuShow;
                _contextIconTimer.Tag = node;
                _contextIconTimer.Start();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Override the mouse leave event for when we have a context icon so
        /// that we hide the context icon.
        /// </summary>
        /// <param name="e">The mouse leave event</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (_contextIconTimer != null)
            {
                _contextIconTimer.Stop();
            }
            if (ContextNode != null)
            {
                TreeNode lastHoverNode = ContextNode;
                ContextNode = null;
                InvalidateNode(lastHoverNode);
            }
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Trigger the update to show the context icon when the hover timer
        /// expires.
        /// </summary>
        private void ContextMenuShow(object obj, EventArgs e)
        {
            _contextIconTimer.Stop();

            ContextNode = (TreeNode)_contextIconTimer.Tag;
            if (ContextNode != null)
            {
                InvalidateNode(ContextNode);
            }
        }
    }
}