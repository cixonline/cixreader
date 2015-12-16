// *****************************************************
// CIXReader
// CRPanel.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/09/2013 4:29 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CIXReader.Controls
{
    /// <summary>
    /// The CRPanel class defines a type of Panel that has separate borders on the
    /// outside of the client area. Each border can have varying thickness ranging from zero,
    /// indicating no border displayed.
    /// </summary>
    public sealed class CRPanel : Panel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CRPanel"/> class.
        /// </summary>
        public CRPanel()
        {
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// Specifies the width of the left border.
        /// </summary>
        public int LeftBorderWidth { get; set; }

        /// <summary>
        /// Specifies the width of the top border.
        /// </summary>
        public int TopBorderWidth { get; set; }

        /// <summary>
        /// Specifies the width of the right border.
        /// </summary>
        public int RightBorderWidth { get; set; }

        /// <summary>
        /// Specifies the width of the bottom border.
        /// </summary>
        public int BottomBorderWidth { get; set; }

        /// <summary>
        /// Specifies whether to do gradient painting.
        /// </summary>
        public bool Gradient { get; set; }

        /// <summary>
        /// Handle the control resize to invalidate the whole control so that we
        /// paint over the old bits when the control is resized.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            Invalidate(Bounds);
            base.OnResize(e);
        }

        /// <summary>
        /// Paint the background gradient
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Gradient && ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                using (var brush = new LinearGradientBrush(ClientRectangle, BackColor, ControlPaint.Light(BackColor), LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
            else
            {
                using (var brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
        }

        /// <summary>
        /// Draw borders of the specified width using the control foreground colour. The interior of
        /// the panel is filled with the control background colour.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush foreBrush = new SolidBrush(ForeColor))
            {
                if (TopBorderWidth > 0)
                {
                    e.Graphics.FillRectangle(foreBrush, 0, 0, Width, TopBorderWidth);
                }
                if (RightBorderWidth > 0)
                {
                    e.Graphics.FillRectangle(foreBrush, Width - RightBorderWidth, 0, Width, Height);
                }
                if (BottomBorderWidth > 0)
                {
                    e.Graphics.FillRectangle(foreBrush, 0, Height - BottomBorderWidth, Width, BottomBorderWidth);
                }
                if (LeftBorderWidth > 0)
                {
                    e.Graphics.FillRectangle(foreBrush, 0, 0, LeftBorderWidth, Height);
                }
            }
        }
    }
}