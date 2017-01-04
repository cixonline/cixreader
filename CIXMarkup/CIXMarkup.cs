// *****************************************************
// CIXMarkup
// CIXMarkup.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 24/06/2015 18:02
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Linq;
using System.Text;

namespace CIXMarkup
{
    /// <summary>
    /// Class that parses CIX markup to HTML
    /// </summary>
    public static class CIXMarkup
    {
        private static int _blockQuoteDepth;

        /// <summary>
        /// Convert the input string to HTML, parsing all markup codes
        /// to their HTML equivalent.
        /// </summary>
        /// <param name="text">Input string</param>
        /// <returns>HTML converted string</returns>
        public static string MarkupToHTML(string text)
        {
            StringBuilder outputString = new StringBuilder();

            string[] lines = text.Split('\n');
            foreach (string line in lines)
            {
                if (outputString.Length > 0)
                {
                    outputString.Append("<br />");
                }
                outputString.Append(ParseLine(line));
            }
            while (_blockQuoteDepth > 0)
            {
                outputString.Append("</blockquote>");
                --_blockQuoteDepth;
            }

            // Optimise the results before returning
            outputString.Replace("<br /><blockquote>", "<blockquote>");
            outputString.Replace("<br /></blockquote>", "</blockquote>");

            return outputString.ToString();
        }

        /// <summary>
        /// Return whether the specified character legally marks the start of a
        /// style interpretation.
        /// </summary>
        /// <param name="lastChar">Preceding character to test</param>
        /// <param name="thisChar">Style character</param>
        /// <returns>True if character can precede style</returns>
        private static bool CanPrecedeStyle(char lastChar, char thisChar)
        {
            return lastChar != thisChar && (char.IsWhiteSpace(lastChar) || lastChar == '*' || lastChar == '_' || lastChar == '/');
        }

        /// <summary>
        /// Read the potential HTML tag from line at the given index and return
        /// whether it matches a subset of HTML tags that are passed through for
        /// rendering as opposed to being escaped for presentation.
        /// </summary>
        /// <param name="line">The line containing the tag</param>
        /// <param name="index">Index of the first character of the tag</param>
        /// <returns>True if the tag is passed through, false if escaped</returns>
        private static bool IsLegalTag(string line, int index)
        {
            int endIndex = line.IndexOfAny(new[] { '>', ' ' }, index);
            if (endIndex > index)
            {
                string tagName = line.Substring(index, endIndex - index);
                if (tagName.StartsWith("/", StringComparison.Ordinal))
                {
                    tagName = tagName.Substring(1);
                }

                string[] legalTags = { "font", "b", "i", "u" };
                return legalTags.Contains(tagName.ToLower());
            }
            return false;
        }

        /// <summary>
        /// Look for the close tag for the specified style character. To qualify,
        /// the end tag must be followed by whitespace.
        /// </summary>
        /// <param name="line">The line containing the tag</param>
        /// <param name="index">Index of where to search for the close tag</param>
        /// <param name="ch">The tag character</param>
        /// <returns>True if the close tag is found, false otherwise</returns>
        private static bool HasCloseTag(string line, int index, char ch)
        {
            int endIndex = line.IndexOf(ch, index);
            if (endIndex > index)
            {
                if (endIndex + 1 == line.Length)
                {
                    return true;
                }
                char afterTag = line[endIndex + 1];
                return char.IsWhiteSpace(afterTag) || afterTag == '*' || afterTag == '_' || afterTag == '/' || afterTag == '.' || afterTag == ',';
            }
            return false;
        }

        /// <summary>
        /// Parse a single line and return the HTML equivalent.
        /// </summary>
        /// <param name="line">The string containing the line to parse</param>
        /// <returns>HTML version of line</returns>
        private static string ParseLine(string line)
        {
            bool inBold = false;
            bool inUnderline = false;
            bool inItalic = false;
            bool lineStart = true;
            bool absorbTag = false;
            int blockCount = 0;
            char lastChar = '\x0085'; // Whitespace
            int index = 0;

            StringBuilder outputString = new StringBuilder();
            while (index < line.Length)
            {
                char ch = line[index++];
                if (absorbTag)
                {
                    outputString.Append(ch);
                    absorbTag = ch != '>';
                    continue;
                }
                if (lineStart)
                {
                    // Skip initial whitespace
                    if (char.IsWhiteSpace(ch))
                    {
                        continue;
                    }
                    if (ch == '>')
                    {
                        ++blockCount;
                        continue;
                    }
                }
                if (blockCount > _blockQuoteDepth)
                {
                    while (_blockQuoteDepth != blockCount)
                    {
                        outputString.Append("<blockquote>");
                        ++_blockQuoteDepth;
                    }
                }
                else if (blockCount < _blockQuoteDepth)
                {
                    while (_blockQuoteDepth != blockCount)
                    {
                        outputString.Append("</blockquote>");
                        --_blockQuoteDepth;
                    }
                }
                switch (ch)
                {
                    case '<':
                        if (IsLegalTag(line, index))
                        {
                            outputString.Append(ch);
                            absorbTag = true;
                        }
                        else
                        {
                            outputString.Append("&lt;");
                        }
                        break;
                    case '>':
                        outputString.Append("&gt;");
                        break;
                    case '&':
                        outputString.Append("&amp;");
                        break;
                    case '*':
                        if (inBold)
                        {
                            outputString.Append("</b>");
                            inBold = false;
                        }
                        else if (CanPrecedeStyle(lastChar, ch) && HasCloseTag(line, index, ch))
                        {
                            outputString.Append("<b>");
                            inBold = true;
                        }
                        else
                        {
                            outputString.Append(ch);
                        }
                        break;
                    case '_':
                        if (inUnderline)
                        {
                            outputString.Append("</u>");
                            inUnderline = false;
                        }
                        else if (CanPrecedeStyle(lastChar, ch) && HasCloseTag(line, index, ch))
                        {
                            outputString.Append("<u>");
                            inUnderline = true;
                        }
                        else
                        {
                            outputString.Append(ch);
                        }
                        break;
                    case '/':
                        if (inItalic)
                        {
                            outputString.Append("</i>");
                            inItalic = false;
                        }
                        else if (CanPrecedeStyle(lastChar, ch) && HasCloseTag(line, index, ch))
                        {
                            outputString.Append("<i>");
                            inItalic = true;
                        }
                        else
                        {
                            outputString.Append(ch);
                        }
                        break;
                    default:
                        outputString.Append(ch);
                        break;
                }
                lineStart = false;
                lastChar = ch;
            }
            if (inBold)
            {
                outputString.Append("</b>");
            }
            if (inUnderline)
            {
                outputString.Append("</u>");
            }
            if (inItalic)
            {
                outputString.Append("</i>");
            }
            return outputString.ToString();
        }
    }
}