// *****************************************************
// CIXReader
// MessageItem.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using System.Globalization;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.Properties;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.CanvasItems
{
    /// <summary>
    /// Implements a specialisation of a CanvasItem that displays a CIX message
    /// </summary>
    public sealed partial class MessageItem : CanvasItem
    {
        private int _compactHeight;
        private Font _headerFont;
        private Font _font;
        private Font _footerFont;
        private int _footerLineHeight;
        private int _headerLineHeight;
        private bool _isMinimal;
        private bool _showFolder;
        private CIXMessage _message;

        /// <summary>
        /// Initialises a new instance of the <see cref="MessageItem"/> class
        /// with the specified Canvas.
        /// </summary>
        public MessageItem(Canvas.Canvas view)
            : base(view, false)
        {
            IsExpandable = true;
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the CIXMessage associated with this item.
        /// </summary>
        public CIXMessage Message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    InvalidateItem();
                }
            }
        }

        /// <summary>
        /// Get or set a flag which indicates whether this message can be expanded when
        /// selected.
        /// </summary>
        public bool IsExpandable { get; set; }

        /// <summary>
        /// Get or set the text to highlight in the item body.
        /// </summary>
        public string Highlight { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether this message is drawn flat without
        /// any root indicator.
        /// </summary>
        public bool Flat { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether this message is drawn in full for
        /// display outside of a threaded listing.
        /// </summary>
        public bool Full { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether the full date is displayed
        /// rather than the friendly date format.
        /// </summary>
        public bool FullDateString { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether this message is rendered collapsed.
        /// </summary>
        public bool Collapsed { get; set; }

        /// <summary>
        /// Get or set a flag which indicates whether the message's folder should be
        /// displayed.
        /// </summary>
        public bool ShowFolder
        {
            get { return _showFolder; }
            set
            {
                if (value != _showFolder)
                {
                    _showFolder = value;
                    InvalidateItem();
                }
            }
        }

        /// <summary>
        /// Get or set a flag which indicates whether this message is minimally
        /// rendered.
        /// </summary>
        public bool Minimal
        {
            get { return _isMinimal; }
            set
            {
                if (value != _isMinimal)
                {
                    _isMinimal = value;
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

                _footerFont = UI.GetFont(_font.FontFamily.Name, _font.Size - 1, _font.Style);
                _headerFont = UI.GetFont(_font.FontFamily.Name, _font.Size - 1, FontStyle.Bold);

                _compactHeight = (_font.Height + 1)*2;
                _headerLineHeight = (int) (_compactHeight/1.5);
                _footerLineHeight = _compactHeight/2;

                InvalidateItem();
            }
        }

        /// <summary>
        /// Overrides the base selection property so that if the item has
        /// Minimal set, we clear it and expand the message when it is selected.
        /// </summary>
        public override bool Selected
        {
            get { return base.Selected; }
            set
            {
                base.Selected = value;
                if (base.Selected && Minimal && IsExpandable)
                {
                    Minimal = false;
                }
                InvalidateItem();
            }
        }

        /// <summary>
        /// Specifies whether or not the message item shows tooltips.
        /// </summary>
        public bool ShowTooltips { get; set; }

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
                Update();
            }
        }

        /// <summary>
        /// Return the message date
        /// </summary>
        private string DateForMessage()
        {
            if (FullDateString)
            {
                return Message.Date.ToString("d MMM yyyy") + " " + Message.Date.ToShortTimeString();
            }
            return Message.Date.FriendlyString(true);
        }

        /// <summary>
        /// Return the text colour for this item.
        /// </summary>
        private Color HeaderFooterColour()
        {
            return Message.Ignored ? UI.Forums.IgnoredColour : Selected ? UI.System.SelectionTextColour : UI.Forums.HeaderFooterColour;
        }

        /// <summary>
        /// Return the appropriate read icon for this message based on its unread and priority state
        /// </summary>
        private Image ReadImageForMessage()
        {
            return (Message.ReadLocked) ? Resources.ReadLock :
                   (!Message.Unread) ?    Resources.ReadMessage :
                   (Message.Priority) ?   Resources.UnreadPriorityMessage : 
                                          Resources.UnreadMessage;
        }

        /// <summary>
        /// Return the appropriate colour in which to paint the author name.
        /// </summary>
        private Color ReadColourForMessage()
        {
            return (Message.Ignored) ? UI.Forums.IgnoredColour : (Message.Priority) ? UI.Forums.PriorityColour : Selected ? UI.System.SelectionTextColour : UI.Forums.HeaderFooterColour;
        }

        /// <summary>
        /// Build the layout for this item from the message.
        /// </summary>
        protected override CanvasItemLayout BuildLayout()
        {
            CanvasItemLayout newLayout = new CanvasItemLayout(this, DrawRectangle);
            if (Message != null)
            {
                if (Full)
                {
                    return BuildFullLayout(newLayout, false);
                }

                if (Minimal)
                {
                    return BuildMinimalLayout(newLayout);
                }

                // Handle collapsed messages especially
                if (Collapsed)
                {
                    return BuildCollapsedLayout(newLayout);
                }

                // Need to reset the item colour because the item may have changed from being local
                ItemColour = (Message.IsDraft)
                    ? UI.Forums.DraftColour
                    : (!Message.IsRoot) ? UI.Forums.CommentColour : UI.Forums.RootColour;

                newLayout = BuildFullLayout(newLayout, true);
            }
            return newLayout;
        }

        /// <summary>
        /// Build the layout for a full message.
        /// </summary>
        private CanvasItemLayout BuildFullLayout(CanvasItemLayout newLayout, bool canExpandRoot)
        {
            bool isWithdrawn = Message.IsWithdrawn;

            if (Image != null)
            {
                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.AuthorImage,
                    Image = Image,
                    ImageWidth = 50,
                    ImageHeight = 50,
                    Margin = new Rectangle(0, 0, 7, 0)
                });
                newLayout.AddNewColumn();
            }

            if (canExpandRoot)
            {
                if (Message.IsRoot && Message.ChildCount > 0)
                {
                    newLayout.Add(new CanvasImage
                    {
                        ID = ActionID.Expand,
                        Image = Resources.CollapseArrow
                    });
                }

                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.Read,
                    Image = ReadImageForMessage(),
                    LineHeight = _headerLineHeight
                });
            }

            newLayout.Add(new CanvasText
            {
                ID = ActionID.Profile,
                Text = Message.Author,
                Font = _headerFont,
                LineHeight = _headerLineHeight,
                ForeColour = ReadColourForMessage(),
            });

            AddSeparatorItem(newLayout, _headerLineHeight);

            newLayout.Add(new CanvasText
            {
                Text = DateForMessage(),
                Font = _headerFont,
                LineHeight = _headerLineHeight,
                ForeColour = HeaderFooterColour(),
            });

            newLayout.Add(new CanvasImage
            {
                ID = ActionID.Star,
                Image = Message.Starred ? Resources.ActiveStar : Resources.InactiveStar,
                LineHeight = _headerLineHeight
            });

            if (Message.IsDraft)
            {
                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.Edit,
                    Image = Resources.Draft,
                    LineHeight = _headerLineHeight
                });
            }

            newLayout.Add(new CanvasText
            {
                ID = ActionID.Link,
                Text = string.Format(" {0}", !Message.IsPseudo ? Message.RemoteID.ToString(CultureInfo.InvariantCulture) : Resources.DraftText),
                Font = _headerFont,
                ToolTipString = (ShowTooltips) ? Resources.LinkTooltip : null,
                LineHeight = _headerLineHeight,
                ForeColour = HeaderFooterColour(),
            });

            if (!Message.IsRoot)
            {
                AddSeparatorItem(newLayout, _headerLineHeight);

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.Original,
                    Text = string.Format(Resources.ReplyToMessage, Message.CommentID),
                    Font = _headerFont,
                    LineHeight = _headerLineHeight,
                    ForeColour = HeaderFooterColour(),
                });
            }

            newLayout.AddNewLine();

            if (ShowFolder)
            {
                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.GoToSource,
                    Image = Resources.Topic,
                    LineHeight = _headerLineHeight
                });

                Folder topic = Message.Topic;
                Folder forum = topic.ParentFolder;

                newLayout.Add(new CanvasText
                {
                    Text = string.Format("{0}/{1}", forum.Name, topic.Name),
                    Font = _font,
                    ID = ActionID.GoToSource,
                    LineHeight = _headerLineHeight,
                    ForeColour = HeaderFooterColour(),
                });

                newLayout.AddNewLine();
            }

            string bodyText = Message.Body ?? string.Empty;
            newLayout.Add(new CanvasHTMLText
            {
                Text = bodyText,
                Font = _font,
                ExpandInlineImages = _view.ExpandInlineImages,
                Highlight = Highlight,
                DisableMarkup = _view.DisableMarkup,
                LineHeight = (Level == 0 || Selected) ? 0 : _compactHeight,
                BackColor = Color.Transparent,
                ForeColour = (Message.Ignored || isWithdrawn) ? HeaderFooterColour() : ForeColor,
                SpaceAfter = 5
            });

            newLayout.AddNewLine();

            // Add an e-mail link to contact the author
            newLayout.Add(new CanvasImage
            {
                ID = ActionID.Email,
                Image = Resources.Mail,
                LineHeight = _footerLineHeight
            });

            // The user can only comment on messages which have been synced with the server.
            // Otherwise for draft messages that are not pending, they can edit them as long
            // as the network is offline as otherwise the transition from draft to sync will
            // be too quick.
            Folder folder = Message.Topic;
            Folder forumFolder = folder.ParentFolder;
            DirForum dirforum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
            bool isModerator = dirforum != null && dirforum.IsModerator;
            bool canReply = !(folder.Flags.HasFlag(FolderFlags.OwnerCommentsOnly) && !Message.IsMine);
            if (folder.IsReadOnly && !isModerator)
            {
                canReply = false;
            }
            if (canReply)
            {
                if (!Message.IsPseudo)
                {
                    AddSeparatorItem(newLayout, _footerLineHeight);

                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.Reply,
                        Text = Resources.Comment,
                        Font = _footerFont,
                        LineHeight = _footerLineHeight,
                        ForeColour = HeaderFooterColour(),
                    });
                }
                else if (Message.IsDraft)
                {
                    AddSeparatorItem(newLayout, _footerLineHeight);
 
                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.Edit,
                        Text = Resources.Edit,
                        Font = _footerFont,
                        LineHeight = _footerLineHeight,
                        ForeColour = HeaderFooterColour(),
                    });
                }
            }

            // Ignore
            if (!Message.IsPseudo)
            {
                AddSeparatorItem(newLayout, _footerLineHeight);

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.Ignore,
                    Text = Message.Ignored ? Resources.Unignore : Resources.Ignore,
                    Font = _footerFont,
                    LineHeight = _footerLineHeight,
                    ForeColour = HeaderFooterColour(),
                });
                AddSeparatorItem(newLayout, _footerLineHeight);

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.Priority,
                    Text = Message.Priority ? Resources.Normal : Resources.Priority,
                    Font = _footerFont,
                    LineHeight = _footerLineHeight,
                    ForeColour = HeaderFooterColour(),
                });
            }

            // Allow withdraw if we own this message or we are a moderator
            // of the forum.
            if ((Message.IsMine || isModerator) && !Message.IsPseudo && !isWithdrawn)
            {
                AddSeparatorItem(newLayout, _footerLineHeight);

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.Withdraw,
                    Text = Resources.Withdraw,
                    Font = _footerFont,
                    LineHeight = _footerLineHeight,
                    ForeColour = HeaderFooterColour(),
                });
            }

            // Allow delete if this is a local draft
            if (Message.IsPseudo)
            {
                AddSeparatorItem(newLayout, _footerLineHeight);

                newLayout.Add(new CanvasText
                {
                    ID = ActionID.Delete,
                    Text = Resources.Delete,
                    Font = _footerFont,
                    LineHeight = _footerLineHeight,
                    ForeColour = HeaderFooterColour(),
                });
            }

            return newLayout;
        }

        /// <summary>
        /// Build the layout for a minimal message.
        /// </summary>
        private CanvasItemLayout BuildMinimalLayout(CanvasItemLayout newLayout)
        {
            ItemColour = Message.IsPseudo ? UI.Forums.DraftColour : Message.IsRoot ? UI.Forums.RootColour : UI.Forums.CommentColour;

            if (Message.IsRoot && Message.ChildCount > 0 && !Flat)
            {
                newLayout.Add(new CanvasImage
                {
                    ID = ActionID.Expand,
                    Image = Collapsed ? Resources.ExpandArrow : Resources.CollapseArrow
                });
            }

            newLayout.Add(new CanvasImage
            {
                ID = ActionID.Read,
                Image = ReadImageForMessage(),
                LineHeight = _headerLineHeight
            });

            newLayout.Add(new CanvasText
            {
                ID = ActionID.Profile,
                Text = Message.Author,
                Font = _footerFont,
                LineHeight = _headerLineHeight,
                ForeColour = ReadColourForMessage(),
            });

            AddSeparatorItem(newLayout, _headerLineHeight);

            newLayout.Add(new CanvasText
            {
                Text = DateForMessage(),
                Font = _footerFont,
                LineHeight = _headerLineHeight,
                ForeColour = HeaderFooterColour(),
            });

            newLayout.Add(new CanvasImage
            {
                ID = ActionID.Star,
                Image = Message.Starred ? Resources.ActiveStar : Resources.InactiveStar,
                LineHeight = _headerLineHeight
            });

            if (!Message.IsPseudo)
            {
                newLayout.Add(new CanvasText
                {
                    Text = Message.RemoteID.ToString(CultureInfo.InvariantCulture),
                    ID = ActionID.Link,
                    Font = _footerFont,
                    ToolTipString = (ShowTooltips) ? Resources.LinkTooltip : null,
                    LineHeight = _headerLineHeight,
                    ForeColour = HeaderFooterColour(),
                });

                AddSeparatorItem(newLayout, _headerLineHeight);
            }

            string bodyText = Message.Body ?? string.Empty;
            newLayout.Add(new CanvasText
            {
                Text = bodyText.FirstLine(),
                Font = _footerFont,
                LineHeight = _headerLineHeight,
                ForeColour = HeaderFooterColour(),
            });

            return newLayout;
        }

        /// <summary>
        /// Build the layout for a collapsed root message.
        /// </summary>
        private CanvasItemLayout BuildCollapsedLayout(CanvasItemLayout newLayout)
        {
            // Need to reset the item colour because the item may have changed from being local
            ItemColour = UI.Forums.CollapsedColour;

            newLayout.Add(new CanvasImage
            {
                ID = ActionID.AuthorImage,
                Image = Image,
                ImageWidth = 50,
                ImageHeight = 50
            });

            newLayout.AddNewColumn();

            newLayout.Add(new CanvasImage
            {
                ID = ActionID.Expand,
                Image = Resources.ExpandArrow,
                LineHeight = _headerLineHeight
            });

            newLayout.Add(new CanvasImage
            {
                ID = ActionID.Read,
                Image = Message.Unread ? Resources.UnreadMessage : Resources.ReadMessage,
                LineHeight = _headerLineHeight
            });

            newLayout.Add(new CanvasText
            {
                ID = ActionID.Profile,
                Text = Message.Author,
                Font = _headerFont,
                LineHeight = _headerLineHeight,
                ForeColour = ReadColourForMessage(),
            });

            AddSeparatorItem(newLayout, _headerLineHeight);

            newLayout.Add(new CanvasText
            {
                Text = DateForMessage(),
                Font = _headerFont,
                LineHeight = _headerLineHeight,
                ForeColour = HeaderFooterColour(),
            });

            newLayout.AddNewLine();

            // First two lines of the message body
            string bodyText = Message.Body ?? string.Empty;
            newLayout.Add(new CanvasText
            {
                Text = bodyText.FirstLine(),
                Font = _headerFont,
                LineHeight = _compactHeight,
                ForeColour = Selected ? UI.System.SelectionTextColour : ForeColor,
            });
            newLayout.AddNewLine();

            // Footer line
            newLayout.Add(new CanvasImage
            {
                ID = ActionID.Expand,
                Image = Resources.Bubble,
                ImageWidth = 14,
                ImageHeight = 14
            });

            newLayout.Add(new CanvasText
            {
                ID = ActionID.Expand,
                Text = (Message.ChildCount > 1) ? string.Format(Resources.ManyReplies, Message.ChildCount) : Resources.OneReply,
                Font = _footerFont,
                LineHeight = _footerLineHeight,
                ForeColour = HeaderFooterColour(),
            });

            AddSeparatorItem(newLayout, _footerLineHeight);

            // Ignore
            newLayout.Add(new CanvasText
            {
                ID = ActionID.Ignore,
                Text = Message.Ignored ? Resources.Unignore : Resources.Ignore,
                Font = _footerFont,
                LineHeight = _footerLineHeight,
                ForeColour = HeaderFooterColour(),
            });

            return newLayout;
        }
    }
}