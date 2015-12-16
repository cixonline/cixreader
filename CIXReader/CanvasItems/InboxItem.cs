// *****************************************************
// CIXReader
// InboxItem.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.CanvasItems
{
    /// <summary>
    /// Implements a specialisation of a CanvasItem that displays Inbox messages
    /// </summary>
    public sealed partial class InboxItem : CanvasItem
    {
        private Font _authorFont;
        private Font _font;
        private int _headerLineHeight;

        /// <summary>
        /// Initialises a new instance of the <see cref="InboxItem"/> class with the
        /// specified Canvas and optional separator.
        /// </summary>
        public InboxItem(Canvas.Canvas view, bool separator)
            : base(view, separator)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the InboxMessage associated with this item.
        /// </summary>
        public InboxMessage Message { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether the full date is displayed
        /// rather than the friendly date format.
        /// </summary>
        public bool FullDateString { get; set; }

        /// <summary>
        /// Get or set the font used to render this item.
        /// </summary>
        public override Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                _authorFont = new Font(_font, FontStyle.Bold);
                _headerLineHeight = (int) (_font.Height*1.5);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (_authorFont != null)
                {
                    _authorFont.Dispose();
                }
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Build the layout for this item from the message.
        /// </summary>
        protected override CanvasItemLayout BuildLayout()
        {
            CanvasItemLayout newLayout = new CanvasItemLayout(this, DrawRectangle);
            if (Message != null)
            {
                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.AuthorImage,
                    Image = Image,
                    ImageWidth = 50,
                    ImageHeight = 50
                });
                newLayout.AddNewColumn();

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.Profile,
                    Text = Message.Author,
                    Font = _authorFont,
                    LineHeight = _headerLineHeight,
                    ForeColour = UI.Forums.HeaderFooterColour,
                });

                AddSeparatorItem(newLayout, _headerLineHeight);

                newLayout.Add(new CanvasText
                {
                    Text = FullDateString ? Message.Date.ToString("d MMM yyyy") + " " + Message.Date.ToShortTimeString() : Message.Date.FriendlyString(true),
                    Font = _font,
                    LineHeight = _headerLineHeight,
                    ForeColour = UI.Forums.HeaderFooterColour,
                });
                newLayout.AddNewLine();

                newLayout.Add(new CanvasHTMLText
                {
                    Text = Message.Body,
                    Font = _font,
                    ExpandInlineImages = _view.ExpandInlineImages,
                    DisableMarkup = _view.DisableMarkup,
                    ForeColour = ForeColor
                });
                newLayout.AddNewLine();
            }
            return newLayout;
        }
    }
}