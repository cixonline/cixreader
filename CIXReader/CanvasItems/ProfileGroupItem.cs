// *****************************************************
// CIXReader
// ProfileGroupItem.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections.Specialized;
using System.Linq;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.CanvasItems
{
    /// <summary>
    /// Implements a specialisation of CanvasItem that shows a list of profile groups
    /// </summary>
    public sealed partial class ProfileGroupItem : CanvasItem
    {
        /// <summary>
        /// Construct a ModeratorsItem object
        /// </summary>
        public ProfileGroupItem(Canvas.Canvas view, bool separator)
            : base(view, separator)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the StringCollection for the profiles.
        /// </summary>
        public StringCollection Items { get; set; }

        /// <summary>
        /// Get or set the title to be displayed above the group.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the flag which indicates whether we render in a
        /// single line or wrap at the edge.
        /// </summary>
        public bool SingleLine { get; set; }

        /// <summary>
        /// Update the mugshot for the given user.
        /// </summary>
        public void Refresh(string username)
        {
            CanvasItemLayout layout = CanvasItemLayout;

            Mugshot mugshot = Mugshot.MugshotForUser(username, true);

            foreach (CanvasImage imageItem in layout.Items.
                Where(component => component.ID == ActionID.AuthorImage && (string) component.Tag == username).
                Cast<CanvasImage>())
            {
                imageItem.Image = mugshot.RealImage;
                Invalidate();
                break;
            }
        }

        /// <summary>
        /// Build the layout for this item.
        /// </summary>
        protected override CanvasItemLayout BuildLayout()
        {
            CanvasItemLayout newLayout = new CanvasItemLayout(this, DrawRectangle);
            if (Items != null)
            {
                // Title
                newLayout.Add(new CanvasText
                {
                    ID = ActionID.None,
                    ForeColour = UI.System.ForegroundColour,
                    Alignment = CanvasTextAlign.Top,
                    LineHeight = 30,
                    Font = UI.GetFont(UI.System.font, 12),
                    Text = Title
                });
                newLayout.AddNewLine();

                // Iterate and show an image for every profile.
                foreach (string username in Items)
                {
                    newLayout.Add(new CanvasImage
                    {
                        ID = ActionID.AuthorImage,
                        Image = Mugshot.MugshotForUser(username, true).RealImage,
                        Text = username,
                        Font = UI.GetFont(UI.System.font, 8),
                        Tag = username,
                        NoWrap = SingleLine,
                        ForeColour = UI.System.ForegroundColour,
                        ImageWidth = 80,
                        ImageHeight = 80
                    });
                }
                newLayout.AddNewLine();
            }
            return newLayout;
        }
    }
}