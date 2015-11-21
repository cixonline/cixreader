// *****************************************************
// CIXReader
// StyleController.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 15/05/2015 16:44
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Tables;
using CommonMark;

namespace CIXReader.Utilities
{
    public sealed class StyleController
    {
        // Styles path mappings is global across all instances
        private static Dictionary<string, string> _stylePathMappings;

        private readonly string _styleName;

        private bool _inited;
        private string _htmlTemplate;
        private string _cssStyleSheet;
        private string _jsScript;

        // Smiley emoticons as constants
        private const string emoSmiley = ":-)";
        private const string emoLaugh = ":-D";
        private const string emoFrown = ":-(";
        private const string emoWink = ";-)";
        private const string emoShades = "8-)";
        private Dictionary<string, string> _emoticonDictionary;

        /// <summary>
        /// Initialise the template and stylesheet for the specified display style if it can be
        /// found. Otherwise the existing template and stylesheet are not changed.
        /// </summary>
        /// <param name="styleName">Name of the style to use</param>
        public StyleController(string styleName)
        {
            _styleName = styleName;
            _inited = false;
        }

        /// <summary>
        /// Load the styles map from the styles folder.
        /// </summary>
        public static void LoadStyleMap()
        {
            if (_stylePathMappings == null)
            {
                _stylePathMappings = new Dictionary<string, string>();
        
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                if (appPath != null)
                {
                    string stylesFolder = Path.Combine(appPath, "Styles");
                    LoadMapFromPath(stylesFolder, _stylePathMappings);
                }

                if (CIX.HomeFolder != null)
                {
                    string stylesFolder = Path.Combine(CIX.HomeFolder, "Styles");
                    if (Directory.Exists(stylesFolder))
                    {
                        LoadMapFromPath(stylesFolder, _stylePathMappings);
                    }
                }
            }
        }

        /// <summary>
        /// Return a list of all available styles.
        /// </summary>
        public static IEnumerable<string> AllStyles
        {
            get
            {
                LoadStyleMap();
                return _stylePathMappings.Keys.ToList();
            }
        }

        /// <summary>
        /// Initialise the template and stylesheet for the specified display style if it can be
        /// found. Otherwise the existing template and stylesheet are not changed.
        /// </summary>
        public void LoadStyle()
        {
            LoadStyleMap();
            if (!_inited)
            {
                string path;
                if (_stylePathMappings.TryGetValue(_styleName, out path))
                {
                    try
                    {
                        string filePath = Path.Combine(path, "template.html");
                        string templateString = File.ReadAllText(filePath);

                        if (!string.IsNullOrWhiteSpace(templateString))
                        {
                            _htmlTemplate = templateString;
                            _cssStyleSheet = Path.Combine(path, "stylesheet.css");

                            string javaScriptPath = Path.Combine(path, "script.js");
                            if (File.Exists(javaScriptPath))
                            {
                                _jsScript = javaScriptPath;
                            }

                            // Make sure template is valid
                            string firstLine = _htmlTemplate.FirstNonBlankLine().ToLower();
                            if (firstLine.StartsWith("<html>") || firstLine.StartsWith("<!doctype"))
                            {
                                _htmlTemplate = null;
                                _cssStyleSheet = null;
                                _jsScript = null;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogFile.WriteLine("Failed to read style {0} : {1}", _styleName, e.Message);
                    }
                }
                _inited = true;
            }
        }

        /// <summary>
        /// Get or set the text that is highlighted in the body of the message
        /// </summary>
        public string Highlight { get; set; }

        /// <summary>
        /// Get or set a flag which specifies whether or not markup is enabled.
        /// </summary>
        public bool DisableMarkup { get; set; }

        /// <summary>
        /// Get or set a flag which specifies whether inline images are expanded.
        /// </summary>
        public bool ExpandInlineImages { get; set; }

        public string ImageFromTag(string tag)
        {
            return string.Format("mugshot:{0}", tag);
        }

        public string StringFromTag(string tag)
        {
            return string.IsNullOrEmpty(tag) ? string.Empty : tag;
        }

        public string DateFromTag(DateTime tag)
        {
            return tag.FriendlyString(true);
        }

        public string HtmlFromTag(string tag)
        {
            string text = tag;
            if (!String.IsNullOrEmpty(text))
            {
                Regex r;

                // If text contains explicit XML, temporarily disable markup for this message
                bool localDisableMarkup = DisableMarkup || (text.Contains("?xml"));

                // Make sure all invalid HTML characters are converted to entities.
                if (localDisableMarkup)
                {
                    text = text.EscapeXml();
                }

                if (Highlight != null)
                {
                    r = new Regex(string.Format("({0})", Highlight), RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    text = r.Replace(text, "<font style=\"BACKGROUND-COLOR: yellow\">$1</font>");
                }

                // Replace hard newlines with break tags.
                text = localDisableMarkup ? text.Replace("\n", "<br />") : CommonMarkConverter.Convert(text);

                // Hack to convert <ul><p> and </p></ul> because HtmlRenderer doesn't yet render these properly.
                text = text.Replace("<li>\r\n<p>", "<li>");
                text = text.Replace("</p>\r\n</li>", "</li>");

                // Convert HTTP:// (etc) and www / ftp (etc) links to actual links.
                const string regexlink = @"(((http|https|ftp|news|file)+\:\/\/|www\.|ftp\.)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,#!_%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
                r = new Regex(regexlink, RegexOptions.IgnoreCase);
                text = r.Replace(text, "<a href=\"$1\" title=\"Click to open in a new window or tab\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");

                // Make cix: links into links. These have slightly odder syntax than http
                // so we handle these especially.
                const string cixRegex = @"(cix\:[a-z0-9._\-:/]*[a-z0-9]+)";
                r = new Regex(cixRegex, RegexOptions.IgnoreCase);
                text = r.Replace(text, "<a href=\"$1\">$1</a>");

                // Look for image links and replace with img tags
                // Only HTTP links are used for now.
                if (ExpandInlineImages)
                {
                    const string imgRegex = "(<a href=\"http:.*.(?:jpe?g|gif|png)\".*>)(.*)(</a>)";
                    r = new Regex(imgRegex, RegexOptions.IgnoreCase);
                    text = r.Replace(text, "$1<img border=\"0\" src=\"image:$2\" />$3");
                    text = text.Replace("image:http://", "image:");
                }

                // Make cixfile: links into links. These are converted to:
                //
                //  http://forums.cix.co.uk/secure/cixfile.aspx?forum={forum}&topic={topic}&file={filename}
                //
                const string cixfileRegex = @"cixfile\:([a-z0-9._\-]+)/([a-z0-9._\-]+):([a-z0-9\/&#95;:@=.+?,#_%&~-]+)";
                r = new Regex(cixfileRegex, RegexOptions.IgnoreCase);
                text = r.Replace(text, "<a href=\"http://forums.cix.co.uk/secure/cixfile.aspx?forum=$1&topic=$2&file=$3\">cixfile:$1/$2:$3</a>");

                // Do Smiley replacement
                if (!DisableMarkup)
                {
                    text = ReplaceEmoticons(text);
                }
            }
            return text;
        }

        /// <summary>
        /// Convert any emoticons in the string to references to their actual file in the resources.
        /// </summary>
        public string ReplaceEmoticons(string source)
        {
            if (_emoticonDictionary == null)
            {
                _emoticonDictionary = new Dictionary<string, string>();
                _emoticonDictionary[emoSmiley] = "smiley";
                _emoticonDictionary[emoLaugh] = "laugh";
                _emoticonDictionary[emoFrown] = "frown";
                _emoticonDictionary[emoShades] = "shades";
                _emoticonDictionary[emoWink] = "wink";
            }

            // Always assume that emoticon is in the dictionary. It is an error otherwise.
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return _emoticonDictionary.Keys.Aggregate(source, (current, emoticon) => 
                current.Replace(emoticon, "<img alt=\"" + emoticon + "\" src=\"" + assemblyPath + "\\Emoticons\\" + _emoticonDictionary[emoticon] + ".png\" />"));
        }

        /// <summary>
        /// Return an NSString containing the tagged item collection formatted as HTML
        ///
        /// This method iterates over the items in the collection and creates an NSString that contains
        /// the items using the specified style template if one was specified.
        /// </summary>
        public string StyledTextForCollection(IEnumerable<CIXMessage> array)
        {
            LoadStyle();

            StringBuilder htmlText = new StringBuilder("<!DOCTYPE html><html><head><meta charset=\"UTF-8\" />");
            if (_cssStyleSheet != null)
            {
                htmlText.Append("<link rel=\"stylesheet\" type=\"text/css\" href=\"");
                htmlText.Append(_cssStyleSheet);
                htmlText.Append("\"/>");
            }
            if (_jsScript != null)
            {
                htmlText.Append("<script type=\"text/javascript\" src=\"");
                htmlText.Append(_jsScript);
                htmlText.Append("\"/></script>");
            }
            htmlText.Append("<meta http-equiv=\"Pragma\" content=\"no-cache\">");
            htmlText.Append("</head><body>");

            int index = 0;
            foreach (CIXMessage message in array)
            {
                StringBuilder htmlMessage = new StringBuilder();

                if (_htmlTemplate == null)
                {
                    htmlMessage.Append(message.UnformattedText());
                }
                else
                {
                    Scanner scanner = new Scanner(_htmlTemplate);
                    bool stripIfEmpty = false;
                    string theString = null;

                    while (!scanner.IsAtEnd)
                    {
                        if (scanner.ScanUpToString("<!--", ref theString))
                        {
                            htmlMessage.Append(ExpandTags(theString, message, stripIfEmpty));
                        }
                        if (scanner.ScanString("<!--", ref theString))
                        {
                            string commentTag = null;

                            if (scanner.ScanUpToString("-->", ref commentTag) && commentTag != null)
                            {
                                commentTag = commentTag.Trim();
                                if (commentTag == "cond:noblank")
                                {
                                    stripIfEmpty = true;
                                }
                                if (commentTag == "end")
                                {
                                    stripIfEmpty = false;
                                }
                                scanner.ScanString("-->", ref theString);
                            }
                        }
                    }
                }

                // Separate each message with a horizontal divider line
                if (index > 0)
                {
                    htmlText.Append("<hr><br />");
                }
                htmlText.Append(htmlMessage);
                ++index;
            }

            htmlText.Append("</body></html>");
            return htmlText.ToString();
        }

        /// <summary>
        /// Expands recognised tags in theString based on the object values.
        /// </summary>
        private string ExpandTags(string theString, CIXMessage message, bool cond)
        {
            bool hasOneTag = false;
            int tagStartIndex = 0;

            while ((tagStartIndex = theString.IndexOf('$', tagStartIndex)) >= 0)
            {
                int tagEndIndex = theString.IndexOf('$', tagStartIndex + 1);
                if (tagEndIndex < 0)
                {
                    break;
                }
                int tagLength = (tagEndIndex - tagStartIndex) + 1;
                string tagName = theString.Substring(tagStartIndex + 1, tagLength - 2);
                string tagSelName = string.Format("tag{0}", tagName);

                Type messageType = typeof(TaggedMessage);
                MethodInfo method = messageType.GetMethod(tagSelName);
                string replacementString = (string) method.Invoke(message, new object[]{ message, this });

                if (replacementString == null)
                {
                    theString = theString.Substring(0, tagStartIndex) + theString.Substring(tagEndIndex + 1);
                }
                else
                {
                    theString = theString.Substring(0, tagStartIndex) + replacementString + theString.Substring(tagEndIndex + 1);
                    hasOneTag = true;

                    if (!string.IsNullOrEmpty(replacementString))
                    {
                        cond = false;
                    }

                    tagStartIndex += replacementString.Length;
                }
            }
            return (cond && hasOneTag) ? string.Empty : theString;
        }

        /// <summary>
        /// Read all files from the specified path and construct a mapping of name to
        /// the actual paths and store in the mappings dictionary supplied.
        /// </summary>
        private static void LoadMapFromPath(string path, Dictionary<string, string> mappings)
        {
            IEnumerable<string> directories = Directory.EnumerateDirectories(path);
            foreach (string directory in directories)
            {
                string name = Path.GetFileNameWithoutExtension(directory);
                if (name != null)
                {
                    mappings[name] = directory;
                }
            }
        }
    }
}