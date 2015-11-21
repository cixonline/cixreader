// *****************************************************
// CIXReader
// ProfileItem.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.Properties;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.CanvasItems
{
    /// <summary>
    /// Implements a specialisation of CanvasItem that displays a user's profile.
    /// </summary>
    public sealed partial class ProfileItem : CanvasItem
    {
        private int _compactHeight;
        private Font _font;
        private Font _nameFont;
        private Profile _profileData;

        /// <summary>
        /// Construct a ProfileItem object
        /// </summary>
        public ProfileItem(Canvas.Canvas view, bool separator)
            : base(view, separator)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the profile associated with this item.
        /// </summary>
        public Profile ProfileData
        {
            get { return _profileData; }
            set
            {
                _profileData = value;
                InvalidateItem();
            }
        }

        /// <summary>
        /// Colour of the name field.
        /// </summary>
        public Color NameColour { get; set; }

        /// <summary>
        /// Get or set the font used to render this item.
        /// </summary>
        public override Font Font
        {
            get { return _font; }
            set
            {
                _font = value;

                _nameFont = new Font(_font.FontFamily, 16, FontStyle.Bold);
                _compactHeight = _font.Height*2;
            }
        }

        /// <summary>
        /// Update the read state of this item from the message.
        /// </summary>
        public void UpdateImage()
        {
            CanvasImage imageItem = (CanvasImage) CanvasItemLayout[ActionID.AuthorImage];
            if (imageItem != null)
            {
                imageItem.Image = Image;
                Invalidate();
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
                if (_nameFont != null)
                {
                    _nameFont.Dispose();
                }
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Build the layout for this item from the profile.
        /// </summary>
        protected override CanvasItemLayout BuildLayout()
        {
            CanvasItemLayout newLayout = new CanvasItemLayout(this, DrawRectangle);
            if (ProfileData != null)
            {
                // Show mugshot
                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.AuthorImage,
                    Image = Image,
                    ImageWidth = 80,
                    ImageHeight = 80,
                    Margin = new Rectangle(0, 0, 10, 0)
                });
                newLayout.AddNewColumn();

                // Full name, if we have it
                newLayout.Add(new CanvasText
                {
                    ID = ActionID.None,
                    Font = _nameFont,
                    ForeColour = NameColour,
                    Alignment = CanvasTextAlign.Top,
                    LineHeight = _compactHeight,
                    Text = ProfileData.FriendlyName
                });
                newLayout.AddNewLine();

                // User's nickname
                if (!string.IsNullOrWhiteSpace(ProfileData.FullName))
                {
                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.Profile,
                        Font = Font,
                        ForeColour = UI.System.ForegroundColour,
                        Alignment = CanvasTextAlign.Top,
                        LineHeight = _compactHeight,
                        Text = ProfileData.Username
                    });
                    newLayout.AddNewLine();
                }

                // If there's a location set, show that too.
                if (!string.IsNullOrWhiteSpace(ProfileData.Location) && ProfileData.Location != "N/A")
                {
                    string locationString = string.Format(Resources.LocationText, ProfileData.Location);
                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.Location,
                        Font = Font,
                        ForeColour = UI.System.ForegroundColour,
                        Alignment = CanvasTextAlign.Top,
                        LineHeight = _compactHeight,
                        Text = locationString
                    });
                    newLayout.Add(new CanvasImage
                    {
                        ID = ActionID.Location,
                        Image = Resources.Map,
                        ImageWidth = 16,
                        ImageHeight = 16
                    });
                    newLayout.AddNewLine();
                }

                // Show the last on date.
                if (ProfileData.LastOn != new DateTime())
                {
                    string lastOnString = string.Format(Resources.LastOnText, ProfileData.LastOn.FriendlyString(false));
                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.None,
                        Font = Font,
                        ForeColour = UI.System.ForegroundColour,
                        Alignment = CanvasTextAlign.Top,
                        LineHeight = _compactHeight,
                        Text = lastOnString
                    });
                    newLayout.AddNewLine();
                }

                // Show the e-mail address.
                if (!string.IsNullOrEmpty(ProfileData.EMailAddress))
                {
                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.Email,
                        Font = Font,
                        ForeColour = UI.System.LinkColour,
                        Alignment = CanvasTextAlign.Top,
                        LineHeight = _compactHeight,
                        Text = ProfileData.EMailAddress
                    });
                    newLayout.AddNewLine();
                }

                // Show the profile text
                newLayout.Add(new CanvasHTMLText
                {
                    Text = ProfileData.About,
                    Font = Font
                });
            }
            return newLayout;
        }
    }
}