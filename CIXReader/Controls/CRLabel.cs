// *****************************************************
// CIXReader
// CRLabel.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 24/09/2013 12:20 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Controls
{
    /// <summary>
    /// The CRLabel control implements a simple Label control that has an
    /// optional count value attached to it which is drawn at the bottom right of
    /// the label.
    /// </summary>
    public sealed partial class CRLabel : Control
    {
        private bool _autosize;
        private int _count;
        private bool _selected;
        private readonly Font _countFont;
        private Color _inactiveColour;
        private Color _countColour;
        private Color _countTextColour;

        /// <summary>
        /// Instantiates an instance of the CRLabel control.
        /// </summary>
        public CRLabel()
        {
            InitializeComponent();

            _inactiveColour = SystemColors.GrayText;
            MaxCount = 99;

            _countFont = UI.GetFont(UI.System.font, 8, FontStyle.Bold);

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        /// <summary>
        /// Set the count on the label.
        /// </summary>
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                CalcAutoSize();
                Invalidate();
            }
        }

        /// <summary>
        /// Get or set the maximum count shown on the label
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether the user can click on
        /// this label to select it.
        /// </summary>
        public bool CanBeSelected { get; set; }

        /// <summary>
        /// Get or set a flag which indicates if this label is selected.
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Get or set the colour of inactive menu labels
        /// </summary>
        public Color InactiveColour
        {
            get { return _inactiveColour; }
            set
            {
                if (value != _inactiveColour)
                {
                    _inactiveColour = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Get or set the colour of the count button
        /// </summary>
        public Color CountColour
        {
            get { return _countColour; }
            set
            {
                if (value != _countColour)
                {
                    _countColour = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Get or set the colour of the count button text
        /// </summary>
        public Color CountTextColour
        {
            get { return _countTextColour; }
            set
            {
                if (value != _countTextColour)
                {
                    _countTextColour = value;
                    Invalidate();
                }
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
        /// Draw a label with a count overlaid if the count value is non-zero.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle drawRect = ClientRectangle;

            using (Brush textBrush = new SolidBrush(Selected ? ForeColor : InactiveColour))
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                e.Graphics.DrawString(Text, Font, textBrush, drawRect);
            }

            if (Count > 0)
            {
                string unreadCount = (Count <= MaxCount) ? Count.ToString(CultureInfo.InvariantCulture) : string.Format("{0}+", MaxCount);
                SizeF stringSize = e.Graphics.MeasureString(unreadCount, _countFont, 100);

                int width = Math.Max(18, (int) stringSize.Width + 4);
                const int height = 18;
                Rectangle countRect = new Rectangle(drawRect.Right - (width + 1), 
                                                    drawRect.Top + (drawRect.Height - height) / 2, 
                                                    width,
                                                    height);

                using (Brush circleBrush = new SolidBrush(CountColour))
                {
                    using (Pen circlePen = new Pen(CountColour))
                    {
                        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.FillRoundedRectangle(8.0f, circlePen, circleBrush, countRect);
                    }
                }

                using (Brush countBrush = new SolidBrush(CountTextColour))
                {
                    int x = countRect.X + (countRect.Width - (int)stringSize.Width) / 2;
                    int y = countRect.Y + (countRect.Height - (int)stringSize.Height) / 2;

                    e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
                    e.Graphics.DrawString(unreadCount, _countFont, countBrush, x, y);
                }
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Set the automatic sizing of the label by computing the internal dimensions.
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
        /// Return a default size for the label.
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(90, 90); }
        }

        /// <summary>
        /// If allowed, perform automatic size calculation of the label bounds based on
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
        /// Determine the label size based on the text and font plus any requested
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
                    size = new Size((int)sizeF.Width + 3, (int)sizeF.Height);
                }
            }

            if (Count > 0)
            {
                using (Graphics graphics = CreateGraphics())
                {
                    string unreadCount = (Count <= MaxCount)
                        ? Count.ToString(CultureInfo.InvariantCulture)
                        : string.Format("{0}+", MaxCount);
                    SizeF stringSize = graphics.MeasureString(unreadCount, _countFont, 100);
                    int width = Math.Max(18, (int) stringSize.Width + 4);
                    size.Width += width;
                }
            }

            size.Width += Padding.Horizontal;
            size.Height += Padding.Vertical;
            return size;
        }

        /// <summary>
        /// Return the preferred size of the label control using the proposed size
        /// as the basis.
        /// </summary>
        public override Size GetPreferredSize(Size proposedSize)
        {
            return InternalGetPreferredSize();
        }
    }
}