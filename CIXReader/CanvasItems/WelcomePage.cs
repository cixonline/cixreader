// *****************************************************
// CIXReader
// WelcomePage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 30/06/2015 18:07
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CIXClient;
using CIXClient.Models;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.Properties;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.CanvasItems
{
    public sealed partial class WelcomePage : CanvasItem
    {
        private List<CIXThread> _thread;
        private Font _font;
        private Font _headerFont;
        private int _headerLineHeight;
        private Whos _onlineUsers;

        /// <summary>
        /// Construct a WelcomePage object
        /// </summary>
        public WelcomePage(Canvas.Canvas view) : base(view, false)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the list of CIXThreads to display on the page.
        /// </summary>
        public List<CIXThread> Threads
        {
            get { return _thread; }
            set
            {
                if (value != _thread)
                {
                    _thread = value;
                    InvalidateItem();
                }
            }
        }

        /// <summary>
        /// Get or set the list of online users.
        /// </summary>
        public Whos OnlineUsers
        {
            get { return _onlineUsers; }
            set
            {
                if (value != _onlineUsers)
                {
                    _onlineUsers = value;
                    InvalidateItem();
                }
            }
        }

        /// <summary>
        /// Get or set the font used to render this item.
        /// </summary>
        public override Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                _headerFont = UI.GetFont(_font.FontFamily.Name, _font.Size + 2, _font.Style);

                int compactHeight = (_font.Height + 1) * 2;
                _headerLineHeight = (int)(compactHeight / 1.5);

                InvalidateItem();
            }
        }

        /// <summary>
        /// Update the mugshot for the given user.
        /// </summary>
        public void Refresh(string username)
        {
            CanvasItemLayout layout = CanvasItemLayout;

            Mugshot mugshot = Mugshot.MugshotForUser(username, true);

            foreach (CanvasImage imageItem in layout.Items.
                Where(component => component.ID == ActionID.AuthorImage && (string)component.Tag == username).
                Cast<CanvasImage>())
            {
                imageItem.Image = mugshot.RealImage;
                Invalidate();
                break;
            }
        }

        /// <summary>
        /// Build the layout for this item from the message.
        /// </summary>
        protected override CanvasItemLayout BuildLayout()
        {
            CanvasItemLayout newLayout = new CanvasItemLayout(this, DrawRectangle);

            newLayout.Add(new CanvasImage
            {
                ImageWidth = 100,
                ImageHeight = 100,
                Image = Resources.CIXReaderLogo,
                Margin = new Rectangle(0, 0, 10, 0)
            });

            newLayout.AddNewColumn();

            newLayout.Add(new CanvasText
            {
                Text = Resources.WelcomeTitle,
                Font = UI.GetFont(UI.System.font, 20),
                ForeColour = UI.Forums.BodyColour
            });

            newLayout.AddNewLine();

            newLayout.Add(new CanvasHTMLText
            {
                Text = Resources.WelcomeSubtitle,
                Font = _font,
                ForeColour = UI.Forums.BodyColour
            });

            newLayout.AddNewLine();

            if (Threads.Count > 0)
            {
                newLayout.Add(new CanvasText
                {
                    Text = Resources.TopThreads,
                    Font = _headerFont,
                    ForeColour = UI.Forums.BodyColour,
                    Margin = new Rectangle(0, 0, 0, 15)
                });

                newLayout.AddNewLine();
            }
            foreach (CIXThread thread in Threads)
            {
                newLayout.AddBeginSection();

                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.AuthorImage,
                    ImageWidth = 40,
                    ImageHeight = 40,
                    Tag = thread.Author,
                    Margin = new Rectangle(0, 0, 5, 0),
                    Image = Mugshot.MugshotForUser(thread.Author, true).RealImage
                });

                newLayout.AddNewColumn();

                string bodyText = thread.Body ?? string.Empty;
                newLayout.Add(new CanvasText
                {
                    ID = ActionID.GoToSource,
                    Text = bodyText.FirstLine().TruncateByWordWithLimit(80),
                    Font = _font,
                    Tag = thread,
                    Alignment = CanvasTextAlign.Top,
                    LineHeight = _headerLineHeight,
                    ForeColour = UI.Forums.BodyColour
                });

                newLayout.AddNewLine();

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.AuthorImage,
                    Text = string.Format(Resources.StartedBy, thread.Author),
                    Font = _font,
                    Alignment = CanvasTextAlign.Top,
                    Tag = thread.Author,
                    LineHeight = _headerLineHeight,
                    ForeColour = UI.Forums.HeaderFooterColour
                });

                AddSeparatorItem(newLayout, _headerLineHeight);

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.GoToSource,
                    Text = string.Format("{0}/{1}", thread.Forum, thread.Topic),
                    Font = _font,
                    Tag = thread,
                    Alignment = CanvasTextAlign.Top,
                    LineHeight = _headerLineHeight,
                    Margin = new Rectangle(0, 0, 0, 15),
                    ForeColour = UI.System.LinkColour
                });

                AddSeparatorItem(newLayout, _headerLineHeight);

                newLayout.Add(new CanvasText
                {
                    Text = string.Format("posted at {0}", thread.Date.FriendlyString(true)),
                    Font = _font,
                    Tag = thread,
                    Alignment = CanvasTextAlign.Top,
                    LineHeight = _headerLineHeight,
                    Margin = new Rectangle(0, 0, 0, 15),
                    ForeColour = UI.Forums.HeaderFooterColour
                });

                newLayout.AddEndSection();
            }

            if (OnlineUsers.Users != null)
            {
                newLayout.Add(new CanvasText
                {
                    Text = Resources.OnlineUsers,
                    Font = _headerFont,
                    ForeColour = UI.Forums.BodyColour,
                    Margin = new Rectangle(0, 0, 0, 15)
                });

                newLayout.AddNewLine();

                foreach (OnlineUser user in OnlineUsers.Users)
                {
                    newLayout.Add(new CanvasImage
                    {
                        ID = ActionID.AuthorImage,
                        Image = Mugshot.MugshotForUser(user.Name, true).RealImage,
                        Text = user.Name,
                        Font = UI.GetFont(UI.System.font, 8),
                        Tag = user.Name,
                        NoWrap = true,
                        ForeColour = UI.System.ForegroundColour,
                        ImageWidth = 80,
                        ImageHeight = 80
                    });

                }
            }
            return newLayout;
        }
    }
}