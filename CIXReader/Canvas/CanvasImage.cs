// *****************************************************
// CIXReader
// CanvasImage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 8:32 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using System.Windows.Forms;

namespace CIXReader.Canvas
{
    /// <summary>
    /// Canvas item element for an image.
    /// </summary>
    public sealed class CanvasImage : CanvasElementBase
    {
        private CanvasItemLayout _layout;

        /// <summary>
        /// Initialises a new instance of the <see cref="CanvasImage"/> class
        /// </summary>
        public CanvasImage() : base(CanvasItemLayout.ItemType.Component)
        {
            Font = CanvasItemLayout.DefaultFont;
        }

        /// <summary>
        /// Get or set the image displayed.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Get or set the flag which specifies whether we wrap images.
        /// </summary>
        public bool NoWrap { get; set; }

        /// <summary>
        /// Get or set the optional text displayed with the image.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Get or set the foreground colour of the text.
        /// </summary>
        public Color ForeColour { get; set; }

        /// <summary>
        /// Get or set the font used for rendering text.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Get or set the requested image width which overrides the actual
        /// image size. The image will be scaled to this width.
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// Get or set the requested image height which overrides the actual
        /// image size. The image will be scaled to this height.
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// Get or set the height of the line on which this image is drawn. This is used
        /// to centre the image vertically when drawn.
        /// </summary>
        public int LineHeight { get; set; }

        /// <summary>
        /// Add a new image control to the layout.
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
            int width = (ImageWidth == 0 && Image != null) ? Image.Width : ImageWidth;
            int height = (ImageHeight == 0 && Image != null) ? Image.Height : ImageHeight;
            int lineHeight = (LineHeight == 0) ? height : LineHeight;

            if (position.X + width > clientBounds.Right)
            {
                if (NoWrap)
                {
                    Visible = false;
                    return;
                }

                _layout.SoftNewLine();
                position = _layout.Position;
            }

            // In case we got turned off by NoWrap
            Visible = true;

            if (Text != null)
            {
                Graphics graphics = _layout.Container.CreateGraphics();
                SizeF sizeF = TextRenderer.MeasureText(graphics, Text, Font ?? CanvasItemLayout.DefaultFont, new Size(width, 9999), TextFormatFlags.SingleLine);

                height += (int)sizeF.Height + 6;
                if (height > lineHeight)
                {
                    lineHeight = height;
                }
            }

            int marginExtraX = Margin.Left + Margin.Right;
            int marginExtraY = Margin.Top + Margin.Bottom;

            Bounds = new Rectangle(position.X, position.Y + (lineHeight - height)/2, width + marginExtraX, height + marginExtraY);
        }

        /// <summary>
        /// Draw this image on the layout.
        /// </summary>
        public override void Draw(Graphics graphics)
        {
            if (Image != null)
            {
                int width = (ImageWidth == 0) ? Image.Width : ImageWidth;
                int height = (ImageHeight == 0) ? Image.Height : ImageHeight;
                graphics.DrawImage(Image, Bounds.X + Margin.Left, Bounds.Y + Margin.Top, width, height);
            }

            if (Text != null)
            {
                SizeF sizeF = TextRenderer.MeasureText(graphics, Text, Font ?? CanvasItemLayout.DefaultFont, new Size(Bounds.Width, 9999), TextFormatFlags.SingleLine);

                int x = Bounds.X + (Bounds.Width - (int)sizeF.Width) / 2;
                int y = Bounds.Y + (Bounds.Height - (int) sizeF.Height) - 3;

                Rectangle textBounds = new Rectangle(x, y, Bounds.Width, (int)sizeF.Height);
                TextRenderer.DrawText(graphics, Text, Font ?? CanvasItemLayout.DefaultFont, textBounds, ForeColour, TextFormatFlags.SingleLine);
            }
        }
    }
}