// *****************************************************
// CIXReader
// CanvasText.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 8:31 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace CIXReader.Canvas
{
    /// <summary>
    /// Specifies the alignment of text by the CanvasText component.
    /// </summary>
    public enum CanvasTextAlign
    {
        /// <summary>
        /// The text is top aligned.
        /// </summary>
        Top,

        /// <summary>
        /// The text is bottom aligned.
        /// </summary>
        Bottom,

        /// <summary>
        /// The text is middle aligned
        /// </summary>
        Middle
    }

    /// <summary>
    /// Canvas item element layout element for static text.
    /// </summary>
    public sealed class CanvasText : CanvasElementBase
    {
        private readonly TextFormatFlags _stringFormat;
        private CanvasItemLayout _layout;

        /// <summary>
        /// Instantiates an instance of the CanvasText.
        /// </summary>
        public CanvasText() : base(CanvasItemLayout.ItemType.Component)
        {
            _stringFormat = TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.NoPrefix;
            Alignment = CanvasTextAlign.Middle;
            Font = CanvasItemLayout.DefaultFont;
        }

        /// <summary>
        /// Get or set the font used to render the text.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Get or set the foreground colour of the text.
        /// </summary>
        public Color ForeColour { get; set; }

        /// <summary>
        /// Get or set the text displayed in the layout.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Get or set the maximum height that the static text must occupy. The text will be vertically
        /// centered within this height if it is less than the maximum height.
        /// </summary>
        public int LineHeight { get; set; }

        /// <summary>
        /// Get or set the alignment of the static text within the container.
        /// </summary>
        public CanvasTextAlign Alignment { get; set; }

        /// <summary>
        /// Maximum possible height of the control.
        /// </summary>
        private static int MaxHeight { get { return 20000; } }

        /// <summary>
        /// Add a new static text component in the layout at the given position and wrapped to fit within
        /// the client bounds.
        /// </summary>
        public override void Add(CanvasItemLayout layout, Rectangle clientBounds, Point position)
        {
            _layout = layout;
            SetPosition(position, clientBounds);
        }

        /// <summary>
        /// Adjust the position of this item.
        /// </summary>
        public override void SetPosition(Point position, Rectangle clientBounds)
        {
            int width = clientBounds.Width - (position.X - clientBounds.X);
            int height = Math.Max(LineHeight, MaxHeight);

            Graphics graphics = _layout.Container.CreateGraphics();
            SizeF sizeF = TextRenderer.MeasureText(graphics, Text, Font ?? CanvasItemLayout.DefaultFont, new Size(width, height), _stringFormat);

            if (LineHeight > 0)
            {
                switch (Alignment)
                {
                    default:
                        Bounds = new Rectangle(position.X, position.Y, (int)sizeF.Width + 2, LineHeight + Margin.Bottom);
                        break;

                    case CanvasTextAlign.Middle:
                        Bounds = new Rectangle(position.X, position.Y + Math.Max(0, (LineHeight - (int) sizeF.Height)/2),
                            (int)sizeF.Width + 2, LineHeight + Margin.Bottom);
                        break;

                    case CanvasTextAlign.Bottom:
                        Bounds = new Rectangle(position.X, position.Y + (LineHeight - (int) sizeF.Height),
                            (int)sizeF.Width + 2, LineHeight + Margin.Bottom);
                        break;
                }
            }
            else
            {
                Bounds = new Rectangle(position.X, position.Y, (int)sizeF.Width + 2, (int)sizeF.Height + Margin.Bottom);
            }
        }

        /// <summary>
        /// Draw the text in the layout using the specified graphics context, font, foreground colour
        /// and bounds.
        /// </summary>
        public override void Draw(Graphics graphics)
        {
            if (Text != null)
            {
                graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
                TextRenderer.DrawText(graphics, Text, Font ?? CanvasItemLayout.DefaultFont, Bounds, ForeColour, _stringFormat);
            }
        }
    }
}