// *****************************************************
// CIXReader
// CanvasHTMLText.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 8:31 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CIXClient;
using CIXReader.Properties;
using CIXReader.UIConfig;
using CIXReader.Utilities;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace CIXReader.Canvas
{
    /// <summary>
    /// Canvas item element for an HTML text static control.
    /// </summary>
    public sealed class CanvasHTMLText : CanvasElementBase
    {
        // Smiley emoticons as constants
        private const string emoSmiley = ":-)";
        private const string emoLaugh = ":-D";
        private const string emoFrown = ":-(";
        private const string emoWink = ";-)";
        private const string emoShades = "8-)";
        private Dictionary<string, string> _emoticonDictionary;
        private bool _isMouseDown;
        private CanvasItemLayout _layout;
        private StringBuilder _text;
        private bool _wasDrag;

        /// <summary>
        /// Initialises a new instance of the <see cref="CanvasHTMLText"/> class
        /// </summary>
        public CanvasHTMLText() : base(CanvasItemLayout.ItemType.Component)
        {
            Font = CanvasItemLayout.DefaultFont;
            ExpandInlineImages = true;
        }

        /// <summary>
        /// Get or set the font used by the label text.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Get or set the foreground colour for the label text.
        /// </summary>
        public Color ForeColour { get; set; }

        /// <summary>
        /// Get or set the foreground colour for the label text.
        /// </summary>
        public override Color BackColor
        {
            get { return Control.BackColor; }
            set
            {
                if (Control != null)
                {
                    Control.BackColor = value;
                }
            }
        }

        /// <summary>
        /// Get or set a flag which indicates whether or not we expand inline images.
        /// </summary>
        public bool ExpandInlineImages { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether or not markup codes are interpreted.
        /// </summary>
        public bool DisableMarkup { get; set; }

        /// <summary>
        /// Get or set the text displayed in the label. The text will be converted to HTML automatically.
        /// To use prepared HTML text, use HTMLText instead.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Get or set the text to be highlighted.
        /// </summary>
        public string Highlight { get; set; }

        /// <summary>
        /// Get or set the prepared HTML text displayed in the label. If this is set, it overrides
        /// what is set in Text.
        /// </summary>
        public string HTMLText { get; set; }

        /// <summary>
        /// Get or set the maximum height that the text must occupy. The text will be vertically
        /// centred within this height if it is less than the maximum height.
        /// </summary>
        public int LineHeight { get; set; }

        /// <summary>
        /// Get or set the amount of spacing after the text, in pixels.
        /// </summary>
        public int SpaceAfter { get; set; }

        /// <summary>
        /// Maximum possible height of the control.
        /// </summary>
        private static int MaxHeight { get { return 20000; } }

        /// <summary>
        /// Add a new HTML label within the layout at the given position. The client bounds determine the
        /// maximum extent of the text.
        /// </summary>
        public override void Add(CanvasItemLayout layout, Rectangle clientBounds, Point position)
        {
            int width = clientBounds.Width - (position.X - clientBounds.X);

            if (_text == null)
            {
                _text = new StringBuilder((HTMLText != null) ? HTMLWrap(HTMLText) : HTML(Text));
            }

            // Do Smiley replacement
            if (!DisableMarkup)
            {
                ReplaceEmoticon(_text, emoSmiley);
                ReplaceEmoticon(_text, emoFrown);
                ReplaceEmoticon(_text, emoLaugh);
                ReplaceEmoticon(_text, emoWink);
                ReplaceEmoticon(_text, emoShades);
            }

            string stylesheet = string.Format("pre {{ white-space: pre-wrap; margin-top: 0px }}\n" +
                                              "div {{ color: rgb({0}) }}\n" +
                                              "::selection {{ color: rgb({1}); background-color: rgb({2}) }}\n" +
                                              "blockquote {{ color: rgb({3}) }}\n" +
                                              "blockquote blockquote {{ color: rgb({4}) }}\n" +
                                              "blockquote blockquote blockquote {{ color: rgb({5}) }}",
                UI.ToString(layout.Container.Selected ? UI.Forums.SelectionTextColour : ForeColour),
                UI.ToString(layout.Container.Selected ? UI.Forums.SelectionColour : UI.Forums.SelectionTextColour),
                UI.ToString(layout.Container.Selected ? UI.Forums.SelectionTextColour : UI.Forums.SelectionColour),
                UI.ToString(layout.Container.Selected ? UI.Forums.SelectionTextColour : UI.Forums.Level1QuoteColour),
                UI.ToString(layout.Container.Selected ? UI.Forums.SelectionTextColour : UI.Forums.Level2QuoteColour),
                UI.ToString(layout.Container.Selected ? UI.Forums.SelectionTextColour : UI.Forums.Level3QuoteColour));

            Size size = new Size(width, MaxHeight);
            HtmlLabel label = new HtmlLabel
            {
                AutoSizeHeightOnly = false,
                AutoSize = true,
                Size = size,
                MaximumSize = size,
                Location = position,
                ForeColor = ForeColour,
                BaseStylesheet = stylesheet,
                BackColor = Color.Transparent,
                IsSelectionEnabled = true,
                Visible = true
            };

            Control = label;

            label.LinkClicked += OnLinkClicked;
            label.Click += OnClick;
            label.MouseDown += OnMouseDown;
            label.MouseMove += OnMouseMove;
            label.ImageLoad += LabelOnImageLoad;
            label.PreviewKeyDown += OnPreviewKeyDown;
            label.KeyDown += OnKeyDown;
            label.ContextMenuInvoked += OnContextMenuInvoked;

            try
            {
                label.Text = _text.ToString();
            }
            catch (Exception)
            {
                // This can crash in HtmlRenderer so we need this safeguard.
                label.Text = string.Empty;
            }

            layout.Container.Controls.Add(Control);
            Bounds = new Rectangle(position.X, position.Y, Control.Size.Width, Control.Size.Height + SpaceAfter);

            _layout = layout;
        }

        /// <summary>
        /// Add items to the context menu.
        /// </summary>
        private void OnContextMenuInvoked(object sender, HtmlContextMenuEventArgs eventArgs)
        {
            RContextMenu contextMenu = eventArgs.ContextMenu;
            string searchEngineTitle = Preferences.StandardPreferences.DefaultSearchEngine;
            contextMenu.AddItem(string.Format(Resources.SearchWith, searchEngineTitle), !string.IsNullOrEmpty(GetSelection()), OnContextMenuClick);
        }

        /// <summary>
        /// Do a search of the selected text with Google.
        /// </summary>
        private void OnContextMenuClick(object sender, EventArgs eventArgs)
        {
            string textToSearch = GetSelection();

            // Remove any trailing '.' at the end of the string
            while (textToSearch.Length > 0 && textToSearch.EndsWith(".", StringComparison.Ordinal))
            {
                textToSearch = textToSearch.Remove(textToSearch.Length - 1, 1);
            }

            // Double up any quotes in the string
            textToSearch = textToSearch.Replace("\"", "\"\"");

            // Escape spaces
            textToSearch = textToSearch.Replace(" ", "%20");

            string linkTemplate = SearchEngines.LinkForSearchEngine(Preferences.StandardPreferences.DefaultSearchEngine);
            string searchUrl = string.Format(linkTemplate, textToSearch.Trim());
            Process.Start(searchUrl);
        }

        /// <summary>
        /// Propagate keys up to the parent.
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            _layout.Container.RaiseKeyDown(keyEventArgs);
        }

        /// <summary>
        /// Make sure that arrow keys are caught so we can propagate those up to the parent.
        /// </summary>
        private static void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs args)
        {
            switch (args.KeyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                case Keys.Home:
                case Keys.End:
                    args.IsInputKey = true;
                    break;
            }
        }

        /// <summary>
        /// Adjust the position of this item.
        /// </summary>
        public override void SetPosition(Point position, Rectangle clientBounds)
        {
            Control.Location = position;

            int width = clientBounds.Width - (position.X - clientBounds.X);
            Control.MaximumSize = new Size(width, MaxHeight);
            Bounds = new Rectangle(position.X, position.Y, Control.Size.Width, Control.Size.Height + SpaceAfter);
        }

        /// <summary>
        /// Retrieve the selected text, or an empty string if no selection.
        /// </summary>
        public override string GetSelection()
        {
            HtmlLabel label = (HtmlLabel) Control;
            return label.SelectedText;
        }

        /// <summary>
        /// Select all text in the HTML label
        /// </summary>
        public override void SelectAll()
        {
            HtmlLabel label = (HtmlLabel)Control;
            label.SelectAll();
        }

        /// <summary>
        /// Clears any existing selection
        /// </summary>
        public override void ClearSelection()
        {
            HtmlLabel label = (HtmlLabel) Control;
            label.ClearSelection();
        }

        /// <summary>
        /// This callback is invoked when a remote image is requested. These are identified by image: prefixes on the img
        /// tag source and are assumed to be http based. We request the image from the ImageRequestorTask in CIXClient and
        /// provide a callback to be invoked when the image is retrieved.
        /// </summary>
        private void LabelOnImageLoad(object sender, HtmlImageLoadEventArgs htmlImageLoadEventArgs)
        {
            string realUrl = htmlImageLoadEventArgs.Src;
            if (realUrl.StartsWith("image:", StringComparison.Ordinal))
            {
                realUrl = "http://" + realUrl.Substring(6);
                Image image = CIX.ImageRequestorTask.NewRequest(realUrl, Control.MaximumSize.Width/3, MaxHeight, ImageRetrieved, htmlImageLoadEventArgs);
                if (image != null)
                {
                    htmlImageLoadEventArgs.Callback(image);
                }
            }
            else if (realUrl.StartsWith("images:", StringComparison.Ordinal))
            {
                realUrl = "https://" + realUrl.Substring(7);
                Image image = CIX.ImageRequestorTask.NewRequest(realUrl, Control.MaximumSize.Width / 3, MaxHeight, ImageRetrieved, htmlImageLoadEventArgs);
                if (image != null)
                {
                    htmlImageLoadEventArgs.Callback(image);
                }
            }
        }

        /// <summary>
        /// Callback from ImageRequestorTask for when an image is retrieved.
        /// </summary>
        /// <param name="image">The image retrieved (may be null)</param>
        /// <param name="parameter">The HtmlImageLoadEventArgs argument</param>
        private void ImageRetrieved(Image image, object parameter)
        {
            HtmlImageLoadEventArgs args = parameter as HtmlImageLoadEventArgs;
            if (args != null)
            {
                args.Callback(image);

                Bounds = new Rectangle(Bounds.X, Bounds.Y, Control.Size.Width, Control.Size.Height + SpaceAfter);

                // Now we have an image, we need to adjust the control height to accommodate the
                // image size the label height will likely have changed.
                _layout.Container.RaiseRelayout();
                _layout.Container.Invalidate();
            }
        }

        /// <summary>
        /// Replace all occurrences of the specified emoticon in the string with an HTML img tag
        /// referencing the corresponding image in the Emoticons folder.
        /// </summary>
        /// <param name="source">StringBuilder with the text to replace</param>
        /// <param name="emoticon">The emoticon</param>
        private void ReplaceEmoticon(StringBuilder source, string emoticon)
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
            if (assemblyPath != null)
            {
                string emoticonPath = Path.Combine(assemblyPath, "Emoticons", _emoticonDictionary[emoticon]);
                source.Replace(emoticon, "<img alt=\"" + emoticon + "\" src=\"" + emoticonPath + ".png\" />");
            }
        }

        /// <summary>
        /// Trigger when the mouse moves. Set a flag if a drag event occurred so that we don't fire
        /// off a RaiseEvent later and de-select the control at the end of the drag.
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_isMouseDown)
            {
                if (!_wasDrag)
                {
                    _layout.Container.RaiseSelection();
                }
                _wasDrag = true;
            }
        }

        /// <summary>
        /// Trigger on the mouse down button and set a flag for use when we distinguish between a
        /// click drag and a click selection.
        /// </summary>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDown = true;
            _wasDrag = false;
        }

        /// <summary>
        /// Handle a click on the label and pass up the event to the parent.
        /// </summary>
        private void OnClick(object sender, EventArgs eventArgs)
        {
            if (!_wasDrag)
            {
                _layout.Container.RaiseEvent(this);
            }
        }

        /// <summary>
        /// Handle links clicked in the label.
        /// </summary>
        /// <param name="sender">The control</param>
        /// <param name="args">The link click arguments</param>
        private void OnLinkClicked(object sender, HtmlLinkClickedEventArgs args)
        {
            _layout.Container.RaiseLink(args.Link);
            args.Handled = true;
        }

        /// <summary>
        /// Convert a raw text string to HTML complete with font and markup text
        /// converted to formatting. This is actually quite expensive code so it should
        /// be used sparingly and the results cached.
        /// </summary>
        /// <param name="s">Raw string to convert</param>
        /// <returns>The raw string in HTML format</returns>
        public string HTML(string s)
        {
            string text = s;

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
                    r = new Regex(string.Format("({0})", Highlight), RegexOptions.Multiline|RegexOptions.IgnoreCase);
                    text = r.Replace(text, "<font style=\"BACKGROUND-COLOR: yellow\">$1</font>");
                }

                // Replace hard newlines with break tags.
                text = text.TrimEnd().Replace("\r", "");
                text = localDisableMarkup ? text.Replace("\n", "<br />") : CIXMarkup.CIXMarkup.MarkupToHTML(text);

                // Convert HTTP:// (etc) and www / ftp (etc) links to actual links.
                const string regexlink = @"(((http|https|ftp|news|file)+://|www\.|ftp\.)[&#95;.a-z0-9]+[a-z0-9/&#95;:@=.+?,#!$_%&()~-]*[^.|\'|# |!|\(|?|,| |>|<|;|)])";
                r = new Regex(regexlink, RegexOptions.IgnoreCase);
                text = r.Replace(text, "<a href=\"$1\">$1</a>").Replace("href=\"www", "href=\"http://www");

                // Make cix: links into links. These have slightly odder syntax than http
                // so we handle these especially.
                const string cixRegex = @"(cix:[a-z0-9._\-:/]*[a-z0-9]+)";
                r = new Regex(cixRegex, RegexOptions.IgnoreCase);
                text = r.Replace(text, "<a href=\"$1\">$1</a>");

                // Handle mailto links
                const string mailtoRegex = @"(mailto:\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b)";
                r = new Regex(mailtoRegex, RegexOptions.IgnoreCase);
                text = r.Replace(text, "<a href=\"$1\">$1</a>");

                // Look for image links and replace with img tags
                // Only HTTP links are used for now.
                if (ExpandInlineImages)
                {
                    const string imgRegex = "(<a href=\"(http|https):.*?.(?:jpe?g|gif|png)\">)(.*?)(</a>)";
                    r = new Regex(imgRegex, RegexOptions.IgnoreCase);
                    text = r.Replace(text, "$1<img border=\"0\" src=\"image:$3\" />$4");
                    text = text.Replace("image:http://", "image:");
                    text = text.Replace("image:https://", "images:");
                }

                // Make cixfile: links into links. These are converted to:
                //
                //  http://forums.cix.co.uk/secure/cixfile.aspx?forum={forum}&topic={topic}&file={filename}
                //
                const string cixfileRegex = @"cixfile\:([a-z0-9._\-]+)/([a-z0-9._\-]+):([a-z0-9\/&#95;:@=.+?,#_%&~-]+)";
                r = new Regex(cixfileRegex, RegexOptions.IgnoreCase);
                text = r.Replace(text, "<a href=\"http://forums.cix.co.uk/secure/cixfile.aspx?forum=$1&topic=$2&file=$3\">cixfile:$1/$2:$3</a>");

                // Wrap the results
                Font font = Font ?? CanvasItemLayout.DefaultFont;
                float htmlFontSize = font.Size / (72f / 96f);
                text = String.Format("<font face=\"{0}\" size=\"{1}px\"><div><p>{2}</p></div></font>", font.FontFamily.Name, htmlFontSize, text);
            }
            return text;
        }

        /// <summary>
        /// Wrap the given string in an HTML div tag.
        /// </summary>
        /// <param name="s">String to wrap</param>
        /// <returns>Wrapped string</returns>
        public string HTMLWrap(string s)
        {
            string text = s;

            if (!String.IsNullOrEmpty(text) && text[0] != '<')
            {
                Font font = Font ?? CanvasItemLayout.DefaultFont;
                float htmlFontSize = font.Size / (72f / 96f);
                text = String.Format("<font face=\"{0}\" size=\"{1}px\"><div>{2}</div></font>", font.FontFamily.Name, htmlFontSize, text);
            }

            // Replace hard newlines with break tags.
            return (text != null) ? text.Replace("\n", "<br>") : String.Empty;
        }
    }
}