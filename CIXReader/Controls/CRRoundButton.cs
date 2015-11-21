// *****************************************************
// CIXReader
// CRRoundButton.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 26/03/2014 13:47
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using CIXClient.Collections;
using CIXReader.Utilities;

namespace CIXReader.Controls
{
    /// <summary>
    /// Implements a themeable button with round corners
    /// </summary>
    internal sealed partial class CRRoundButton : Button
    {
        private bool _autosize;
        private bool _hasMouse;
        private string _text;
        private bool _isActive;
        private bool _isEnabled;

        /// <summary>
        /// Creates an instances of a CRRoundButton
        /// </summary>
        public CRRoundButton()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            BackColor = SystemColors.ControlLightLight;

            ParentChanged += OnParentChanged;

            CanHaveFocus = true;
        }

        /// <summary>
        /// Redraw the control if the text value is changed
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                CalcAutoSize();
                Invalidate();
            }
        }

        /// <summary>
        /// Extra data associated with the button as defined by the user.
        /// </summary>
        public object ExtraData { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether the user can click on
        /// this label to select it.
        /// </summary>
        public bool CanBeSelected { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether images are stretched to
        /// fit.
        /// </summary>
        public bool ImageScaling { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether the control shows the focus
        /// when the user mouses over it.
        /// </summary>
        public bool CanHaveFocus { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether the control is activated.
        /// </summary>
        public bool Active
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Override Enabled behaviour so we can interpret the behaviour functionally
        /// but behave as always enabled so that tooltips continue to work over buttons
        /// that are locally disabled.
        /// </summary>
        public new bool Enabled
        {
            set
            {
                base.Enabled = true;
                _isEnabled = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Override the mouse down so that we only pass it on if the local
        /// enable flag is set. This suppresses OnClick events for local disable
        /// even though the underlying control Enabled flag is true.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (_isEnabled)
            {
                base.OnMouseDown(mevent);
            }
        }

        /// <summary>
        /// Change the mouse cursor to a hand when we're over the label since
        /// we're clickable by default.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (CanBeSelected)
            {
                Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Called when the mouse enters the control.
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            if (CanHaveFocus)
            {
                _hasMouse = true;
                Invalidate();
            }
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Called when the mouse leaves the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (CanHaveFocus)
            {
                _hasMouse = false;
                Invalidate();
            }
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Draw a button with rounded edges.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush backBrush = new SolidBrush(Parent.BackColor))
            {
                e.Graphics.FillRectangle(backBrush, ClientRectangle);
            }

            Color backColor = Active ? SystemColors.Window : _hasMouse && _isEnabled ? SystemColors.ButtonHighlight : BackColor;
            Color textColor = Active ? SystemColors.WindowText : ForeColor;
            using (Brush brush = new SolidBrush(backColor))
            {
                RectangleF rectangle = new RectangleF(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                using (Pen pen = new Pen(textColor))
                {
                    e.Graphics.FillRoundedRectangle(pen, brush, rectangle);
                    if (Text != null)
                    {
                        e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        SizeF sizeF = e.Graphics.MeasureString(Text, Font);
                        Rectangle textRectangle = new Rectangle(
                            (int)(ClientRectangle.Width - sizeF.Width)/2,
                            (int)(ClientRectangle.Height - sizeF.Height) / 2,
                            (int)sizeF.Width + 5,
                            (int)sizeF.Height);

                        const TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter;
                        TextRenderer.DrawText(e.Graphics, Text, Font, textRectangle, textColor, flags);
                    }
                }

                Image imageToUse = Image;
                if (Image != null)
                {
                    if (ImageScaling)
                    {
                        imageToUse = ImageRequestorTask.ScaleImage(imageToUse, ClientRectangle.Width - 8, ClientRectangle.Height - 8);
                    }
                    rectangle = new RectangleF(
                            ((float)ClientRectangle.Width - imageToUse.Width) / 2,
                            ((float)ClientRectangle.Height - imageToUse.Height) / 2,
                            imageToUse.Width,
                            imageToUse.Height);
                    if (_isEnabled)
                    {
                        e.Graphics.DrawImage(imageToUse, rectangle);
                    }
                    else
                    {
                        ControlPaint.DrawImageDisabled(e.Graphics, imageToUse, (int)rectangle.X, (int)rectangle.Y, backColor);
                    }
                }
            }
        }

        /// <summary>
        /// Set the automatic sizing of the button by computing the internal dimensions.
        /// </summary>
        public override bool AutoSize
        {
            get { return _autosize; }
            set
            {
                if (_autosize != value)
                {
                    SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
                    base.AutoSize = value;
                    _autosize = value;
                    CalcAutoSize();
                    Invalidate();

                    OnAutoSizeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Return a default size for the button.
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(50, 23); }
        }

        /// <summary>
        /// If allowed, perform automatic size calculation of the button bounds based on
        /// the text value.
        /// </summary>
        private void CalcAutoSize()
        {
            if (!AutoSize)
                return;

            Size s = InternalGetPreferredSize();
            SetBounds(Left, Top, s.Width, s.Height, BoundsSpecified.Size);
        }

        /// <summary>
        /// Determine the button size based on the text and font plus any requested
        /// padding.
        /// </summary>
        internal Size InternalGetPreferredSize()
        {
            Size size;

            if (Text == string.Empty)
            {
                size = new Size(0, Font.Height);
            }
            else
            {
                using (Graphics graphics = CreateGraphics())
                {
                    SizeF sizeF = graphics.MeasureString(Text, Font);
                    size = new Size((int)sizeF.Width + 10, (int)sizeF.Height + 10);
                }
            }
            return size;
        }

        /// <summary>
        /// Return the preferred size of the button using the proposed size
        /// as the basis.
        /// </summary>
        public override Size GetPreferredSize(Size proposedSize)
        {
            return InternalGetPreferredSize();
        }

        /// <summary>
        /// If the parent is changed, hook into the parent's background colour change
        /// notification.
        /// </summary>
        private void OnParentChanged(object sender, EventArgs eventArgs)
        {
            if (Parent != null)
            {
                Parent.BackColorChanged += ParentOnBackColorChanged;
            }
        }

        /// <summary>
        /// Handle the parent background colour changed so we repaint the areas under the
        /// rounded edges.
        /// </summary>
        private void ParentOnBackColorChanged(object sender, EventArgs eventArgs)
        {
            Invalidate();
        }
    }
}