// *****************************************************
// CIXReader
// Extensions.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 30/08/2013 4:40 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace CIXClient
{
    /// <summary>
    /// The Extensions class provides extensions to other, generally .NET Framework, classes
    /// to provide additional functionality.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Return the friendly name for the specified date/time. The algorithm used
        /// here is as follows:
        /// 
        /// If the date is today, we just return the current time in short format.
        /// 
        /// Otherwise if the date was yesterday, we return "Yesterday".
        /// 
        /// Otherwise if the date was within a week, we return the weekday name
        /// in long format ("Monday", "Tuesday", etc).
        /// 
        /// Otherwise if the date was more than a week, we return the date in
        /// short format.
        /// </summary>
        /// <param name="dateTime">A date/time structure</param>
        /// <param name="withTime">True if the time should always be included</param>
        /// <returns>The friendly name for the given date and time</returns>
        public static string FriendlyString(this DateTime dateTime, bool withTime)
        {
            DateTime now = DateTime.Now;
            DateTime endOfDay = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            TimeSpan deltaTime = endOfDay - dateTime;

            if (deltaTime.Days > 7 && withTime)
            {
                return dateTime.ToString("d MMMM yyyy") + " at " + dateTime.ToShortTimeString();
            }
            if (deltaTime.Days > 7)
            {
                return dateTime.ToString("d MMMM yyyy");
            }
            if (deltaTime.Days > 0)
            {
                return dateTime.ToString("ddd") + " " + dateTime.ToShortTimeString();
            }
            return dateTime.ToShortTimeString();
        }

        /// <summary>
        /// Fix MS-Word style smart quotes by converting them to normal quotes.
        /// </summary>
        internal static string FixQuotes(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Replace('\u2018', '\'')
                    .Replace('\u2019', '\'')
                    .Replace('\u201c', '\"')
                    .Replace('\u201d', '\"');
            }
            return str;
        }

        /// <summary>
        /// Return the first line of the given string. A line is assumed to be terminated
        /// by either a carriage return or newline, or by the end of the string itself.
        /// </summary>
        /// <param name="str">A string with optional newlines</param>
        /// <returns>The first line of the above string</returns>
        public static string FirstLine(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException();
            }

            str = str.TrimStart();

            int lineIndex = str.IndexOfAny(new [] { '\r', '\n' });
            return lineIndex < 0 ? str : str.Substring(0, lineIndex);
        }

        /// <summary>
        /// Truncate the string by the specified limit.
        /// </summary>
        /// <param name="str">The string to truncate</param>
        /// <param name="limit">The limit of the string</param>
        /// <returns>Either the string or the string truncated to the limit with ellipsis appended</returns>
        public static string TruncateByWordWithLimit(this string str, int limit)
        {
            if (str == null || str.Length < limit)
            {
                return str;
            }
            int iNextSpace = str.LastIndexOf(" ", limit, StringComparison.Ordinal);
            return string.Format("{0}...", str.Substring(0, (iNextSpace > 0) ? iNextSpace : limit).Trim());
        }

        /// <summary>
        /// Returns the first line of the string that isn't entirely spaces or tabs. If all lines in the string are
        /// empty, we return an empty string.
        /// </summary>
        /// <param name="str">A string with optional newlines</param>
        /// <returns>The first non-empty line of the above string</returns>
        public static string FirstNonBlankLine(this string str)
        {
            bool hasNonEmptyChars = false;
            int indexOfFirstChr = 0;
            int indexOfLastChr = 0;
    
            int indexOfChr = 0;
            int length = str.Length;
            while (indexOfChr < length)
            {
                char ch = str[indexOfChr];
                if (ch == '\r' || ch == '\n')
                {
                    if (hasNonEmptyChars)
                    {
                        break;
                    }
                }
                else
                {
                    if (ch != ' ' && ch != '\t')
                    {
                        if (!hasNonEmptyChars)
                        {
                            hasNonEmptyChars = true;
                            indexOfFirstChr = indexOfChr;
                        }
                        indexOfLastChr = indexOfChr;
                    }
                }
                ++indexOfChr;
            }
            return hasNonEmptyChars ? str.Substring(indexOfFirstChr, 1 + (indexOfLastChr - indexOfFirstChr)) : string.Empty;
        }

        /// <summary>
        /// Return the first line of the given string that doesn't begin with a quote delimiter.
        /// A line is assumed to be terminated by either a carriage return or newline, or by
        /// the end of the string itself.
        /// </summary>
        /// <param name="str">A string with optional newlines</param>
        /// <returns>The first line of the above string</returns>
        public static string FirstUnquotedLine(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException();
            }

            int lineIndex;
            while (str.TrimStart().StartsWith(">", StringComparison.Ordinal))
            {
                lineIndex = str.IndexOfAny(new[] { '\r', '\n' });
                if (lineIndex < 0)
                {
                    return string.Empty;
                }
                str = str.Substring(lineIndex + 1);
            }

            str = str.TrimStart();
            lineIndex = str.IndexOfAny(new[] { '\r', '\n' });
            return lineIndex < 0 ? str : str.Substring(0, lineIndex);
        }

        /// <summary>
        /// Normalise a string by converting all line breaks to Windows style line
        /// breaks.
        /// </summary>
        /// <param name="text">String to normalise</param>
        /// <returns>The normalised string</returns>
        public static string FixNewlines(this string text)
        {
            string [] splitArray = text.Split(new [] { "\n", "r\n" }, StringSplitOptions.None);
            return string.Join("\r\n", splitArray);
        }

        /// <summary>
        /// Return the specified text wrapped in quotes
        /// </summary>
        /// <param name="text">The raw text to be quoted</param>
        /// <returns>The text wrapped in quotes</returns>
        public static string Quoted(this string text)
        {
            StringBuilder quotedText = new StringBuilder();

            const int wrapColumn = 74;

            int lastSpaceIndex = -1;
            int lastSpaceColumnIndex = 0;
            int columnIndex = 0;
            int startIndex = 0;
            int currentIndex = 0;

            while (currentIndex < text.Length)
            {
                char ch = text[currentIndex++];
                if (ch == ' ')
                {
                    lastSpaceIndex = currentIndex;
                    lastSpaceColumnIndex = columnIndex;

                    ++columnIndex;
                }
                else if (ch == '\r' || ch == '\n')
                {
                    quotedText.AppendFormat("> {0}\r\n", text.Substring(startIndex, columnIndex));

                    while (currentIndex < text.Length && Char.IsWhiteSpace(text[currentIndex]))
                    {
                        ++currentIndex;
                    }

                    startIndex = currentIndex;
                    columnIndex = 0;
                }
                else if (columnIndex >= wrapColumn)
                {
                    if (lastSpaceIndex == -1) // Line with no spaces!
                    {
                        lastSpaceIndex = currentIndex;
                        lastSpaceColumnIndex = columnIndex;
                    }

                    quotedText.AppendFormat("> {0}\r\n", text.Substring(startIndex, lastSpaceColumnIndex + 1));

                    while (lastSpaceIndex < text.Length && Char.IsWhiteSpace(text[lastSpaceIndex]))
                    {
                        ++lastSpaceIndex;
                    }

                    currentIndex = lastSpaceIndex;
                    startIndex = currentIndex;
                    lastSpaceIndex = -1;
                    columnIndex = 0;
                }
                else
                {
                    ++columnIndex;
                }
            }

            // Last line
            if (columnIndex > 0)
            {
                quotedText.AppendFormat("> {0}\r\n", text.Substring(startIndex));
            }

            return quotedText.ToString();
        }

        /// <summary>
        /// Escape any illegal XML characters in the given string by replacing them with their
        /// equivalent.
        /// </summary>
        /// <param name="s">The string to escape</param>
        /// <returns>The string but with any illegal XML characters escaped.</returns>
        public static string EscapeXml(this string s)
        {
            string xml = s;
            if (!string.IsNullOrEmpty(xml))
            {
                xml = xml.Replace("&", "&amp;");
                xml = xml.Replace("<", "&lt;");
                xml = xml.Replace(">", "&gt;");
                xml = xml.Replace("\"", "&quot;");
                xml = xml.Replace("'", "&apos;");
            }
            return xml;
        }

        /// <summary>
        /// Unescape any XML entity characters back to their raw equivalents.
        /// </summary>
        /// <param name="s">The string to unescape</param>
        /// <returns>The string with XML entity codes unescaped.</returns>
        internal static string UnescapeXml(this string s)
        {
            string xml = s;
            if (!string.IsNullOrEmpty(xml))
            {
                xml = xml.Replace("&lt;", "<");
                xml = xml.Replace("&gt;", ">");
                xml = xml.Replace("&quot;", "\"");
                xml = xml.Replace("&apos;", "'");
                xml = xml.Replace("&amp;", "&");
            }
            return xml;
        }

        /// <summary>
        /// Resize the image.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">Maximum width</param>
        /// <param name="height">Maximum height</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);

            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                Rectangle destImage = new Rectangle(0, 0, result.Width, result.Height);
                Rectangle sourceImage;

                // For non-square source images we crop and center.
                if (image.Width > image.Height)
                {
                    int xOffset = (image.Width - image.Height) / 2;
                    sourceImage = new Rectangle(xOffset, 0, image.Height, image.Height);
                }
                else if (image.Width < image.Height)
                {
                    int yOffset = (image.Height - image.Width) / 2;
                    sourceImage = new Rectangle(0, yOffset, image.Width, image.Width);
                }
                else
                {
                    sourceImage = new Rectangle(0, 0, image.Width, image.Height);
                }
                graphics.DrawImage(image, destImage, sourceImage, GraphicsUnit.Pixel);
            }
            return result;
        }
    }
}