// *****************************************************
// CIXReader
// GraphicsExtensions.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 19/06/2014 18:56
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CIXReader.Utilities
{
    /// <summary>
    /// Extensions for the Graphics class
    /// </summary>
    public static class GraphicsExtensions
    {
        /// <summary>
        /// Draw a rounded rectangle with the specified pen as the edge and the brush as the fill.
        /// </summary>
        /// <param name="graphics">Graphics object</param>
        /// <param name="borderPen"></param>
        /// <param name="fillBrush"></param>
        /// <param name="fillRectangle"></param>
        public static void FillRoundedRectangle(this Graphics graphics, Pen borderPen, Brush fillBrush, RectangleF fillRectangle)
        {
            FillRoundedRectangle(graphics, 4.0f, borderPen, fillBrush, fillRectangle);
        }

        /// <summary>
        /// Draw a rounded rectangle with the specified pen as the edge and the brush as the fill.
        /// </summary>
        /// <param name="graphics">Graphics object</param>
        /// <param name="borderRadius"></param>
        /// <param name="borderPen"></param>
        /// <param name="fillBrush"></param>
        /// <param name="fillRectangle"></param>
        public static void FillRoundedRectangle(this Graphics graphics, float borderRadius, Pen borderPen, Brush fillBrush, RectangleF fillRectangle)
        {
            GraphicsPath path = GenerateRoundedRectangle(fillRectangle, borderRadius, RectangleEdgeFilter.All);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(borderPen, path);
            graphics.FillPath(fillBrush, path);
            graphics.SmoothingMode = old;
        }

        private static GraphicsPath GenerateCapsule(RectangleF rectangle)
        {
            GraphicsPath path = new GraphicsPath();
            try
            {
                float diameter;
                RectangleF arc;
                if (rectangle.Width > rectangle.Height)
                {
                    diameter = rectangle.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rectangle.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = rectangle.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (rectangle.Width < rectangle.Height)
                {
                    diameter = rectangle.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rectangle.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = rectangle.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else path.AddEllipse(rectangle);
            }
            catch
            {
                path.AddEllipse(rectangle);
            }
            finally
            {
                path.CloseFigure();
            }
            return path;
        }

        private static GraphicsPath GenerateRoundedRectangle(RectangleF rectangle, float radius,
            RectangleEdgeFilter filter)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.0F || filter == RectangleEdgeFilter.None)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }
            if (radius >= (Math.Min(rectangle.Width, rectangle.Height))/2.0)
            {
                return GenerateCapsule(rectangle);
            }
            float diameter = radius*2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(rectangle.Location, sizeF);
            if ((RectangleEdgeFilter.TopLeft & filter) == RectangleEdgeFilter.TopLeft)
            {
                path.AddArc(arc, 180, 90);
            }
            else
            {
                path.AddLine(arc.X, arc.Y + arc.Height, arc.X, arc.Y);
                path.AddLine(arc.X, arc.Y, arc.X + arc.Width, arc.Y);
            }
            arc.X = rectangle.Right - diameter;
            if ((RectangleEdgeFilter.TopRight & filter) == RectangleEdgeFilter.TopRight)
            {
                path.AddArc(arc, 270, 90);
            }
            else
            {
                path.AddLine(arc.X, arc.Y, arc.X + arc.Width, arc.Y);
                path.AddLine(arc.X + arc.Width, arc.Y + arc.Height, arc.X + arc.Width, arc.Y);
            }
            arc.Y = rectangle.Bottom - diameter;
            if ((RectangleEdgeFilter.BottomRight & filter) == RectangleEdgeFilter.BottomRight)
            {
                path.AddArc(arc, 0, 90);
            }
            else
            {
                path.AddLine(arc.X + arc.Width, arc.Y, arc.X + arc.Width, arc.Y + arc.Height);
                path.AddLine(arc.X, arc.Y + arc.Height, arc.X + arc.Width, arc.Y + arc.Height);
            }
            arc.X = rectangle.Left;
            if ((RectangleEdgeFilter.BottomLeft & filter) == RectangleEdgeFilter.BottomLeft)
            {
                path.AddArc(arc, 90, 90);
            }
            else
            {
                path.AddLine(arc.X + arc.Width, arc.Y + arc.Height, arc.X, arc.Y + arc.Height);
                path.AddLine(arc.X, arc.Y + arc.Height, arc.X, arc.Y);
            }
            path.CloseFigure();
            return path;
        }

        [Flags]
        private enum RectangleEdgeFilter
        {
            None = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomLeft = 4,
            BottomRight = 8,
            All = TopLeft | TopRight | BottomLeft | BottomRight
        }
    }
}