// *****************************************************
// CIXReader
// ForumPage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 14/11/2013 7:52 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Drawing;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.Properties;
using CIXReader.SpecialFolders;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.CanvasItems
{
    /// <summary>
    /// Implements a specialisation of a CanvasItem that displays a forum summary
    /// </summary>
    public sealed partial class ForumPage : CanvasItem
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ForumPage"/> class with
        /// the specified Canvas and optional separator.
        /// </summary>
        public ForumPage(Canvas.Canvas view, bool separator) : base(view, separator)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the Folder associated with this item.
        /// </summary>
        public Folder Folder { get; set; }

        /// <summary>
        /// Get or set the Forum associated with this item.
        /// </summary>
        public DirForum Forum { get; set; }

        /// <summary>
        /// Get or set the font used to render the folder name.
        /// </summary>
        public Font NameFont { get; set; }

        /// <summary>
        /// Get or set the font used to render the title of the folder.
        /// </summary>
        public Font TitleFont { get; set; }

        /// <summary>
        /// Get or set the font used to render the description of the folder.
        /// </summary>
        public Font DescriptionFont { get; set; }

        /// <summary>
        /// Build the layout for this item from the message.
        /// </summary>
        protected override CanvasItemLayout BuildLayout()
        {
            CanvasItemLayout newLayout = new CanvasItemLayout(this, DrawRectangle);
            if (Folder != null)
            {
                // Forum category icon, if one is present. At some point we should
                // replace this with an actual forum icon.
                Image categoryImage = null;

                if (Forum != null)
                {
                    categoryImage = CategoryFolder.IconForCategory(Forum.Cat);
                }
                if (categoryImage == null)
                {
                    categoryImage = Resources.Categories;
                }
                if (categoryImage != null)
                {
                    newLayout.Add(new CanvasImage
                    {
                        ID = ActionID.None,
                        Image = categoryImage,
                        ImageWidth = 50,
                        ImageHeight = 50
                    });
                    newLayout.AddNewColumn();
                }

                // Forum name
                newLayout.Add(new CanvasText
                {
                    ID = ActionID.None,
                    Text = Folder.Name,
                    Font = NameFont,
                    LineHeight = NameFont.Height + 10,
                    ForeColour = UI.System.ForegroundColour
                });
                newLayout.AddNewLine();

                // Forum title.
                if (Forum != null && !string.IsNullOrEmpty(Forum.Title))
                {
                    newLayout.Add(new CanvasHTMLText
                    {
                        ID = ActionID.None,
                        Text = Forum.Title,
                        Font = TitleFont,
                        ForeColour = UI.System.ForegroundColour,
                        SpaceAfter = 15
                    });
                    newLayout.AddNewLine();
                }

                // Forum description.
                if (Forum != null && !string.IsNullOrEmpty(Forum.Desc))
                {
                    newLayout.Add(new CanvasHTMLText
                    {
                        ID = ActionID.None,
                        HTMLText = Forum.Desc,
                        Font = DescriptionFont,
                        ForeColour = UI.System.ForegroundColour,
                        SpaceAfter = 15
                    });
                    newLayout.AddNewLine();
                }

                // If the Join is pending, say so.
                if (Forum != null && Forum.JoinPending)
                {
                    newLayout.Add(new CanvasImage
                    {
                        Image = Resources.Warning,
                        ImageWidth = 16,
                        ImageHeight = 16
                    });

                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.None,
                        Text = Resources.JoinPending,
                        LineHeight = 16,
                        Margin = new Rectangle(0, 0, 0, 15),
                        Font = Font
                    });
                    newLayout.AddNewLine();
                }

                // If a Resign is pending, say so.
                if (Folder.ResignPending)
                {
                    newLayout.Add(new CanvasImage
                    {
                        Image = Resources.Warning,
                        ImageWidth = 16,
                        ImageHeight = 16
                    });

                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.None,
                        Text = Resources.ResignPending,
                        LineHeight = 16,
                        Margin = new Rectangle(0, 0, 0, 15),
                        Font = Font,
                    });
                    newLayout.AddNewLine();
                }

                // If the Join failed, say so too.
                if (Folder.Flags.HasFlag(FolderFlags.JoinFailed))
                {
                    newLayout.Add(new CanvasImage
                    {
                        Image = Resources.Error1,
                        ImageWidth = 16,
                        ImageHeight = 16
                    });

                    newLayout.Add(new CanvasText
                    {
                        ID = ActionID.None,
                        Text = Resources.JoinFailure,
                        LineHeight = 16,
                        Font = Font,
                        Margin = new Rectangle(0, 0, 0, 15)
                    });
                    newLayout.AddNewLine();
                }

                // Resign/Join button
                // Don't display if the forum doesn't permit the user to resign it.
                if (Folder.CanResign)
                {
                    if (Folder.IsResigned || 
                        Folder.Flags.HasFlag(FolderFlags.JoinFailed) ||
                        Folder.ResignPending)
                    {
                        newLayout.Add(new CanvasButton
                        {
                            ID = ActionID.JoinForum,
                            Text = Resources.JoinForum,
                            Font = DescriptionFont,
                            ForeColour = UI.System.ForegroundColour,
                            SpaceAfter = 15
                        });
                    }
                    else
                    {
                        newLayout.Add(new CanvasButton
                        {
                            ID = ActionID.ResignForum,
                            Text = Resources.ResignForum,
                            Font = DescriptionFont,
                            ForeColour = UI.System.ForegroundColour,
                            SpaceAfter = 15
                        });
                    }
                }

                // Delete button
                // Offer the option to delete the folder if we're not joined to it
                if (Folder.IsResigned || Folder.Flags.HasFlag(FolderFlags.JoinFailed))
                {
                    newLayout.Add(new CanvasButton
                    {
                        ID = ActionID.Delete,
                        Text = Resources.DeleteFolder,
                        Font = DescriptionFont,
                        ForeColour = UI.System.ForegroundColour,
                        SpaceAfter = 15
                    });
                }

                // Add Participants button
                newLayout.Add(new CanvasButton
                {
                    ID = ActionID.Participants,
                    Text = Resources.Participants,
                    Font = DescriptionFont,
                    ForeColour = UI.System.ForegroundColour,
                    SpaceAfter = 15
                });

                // Refresh button
                newLayout.Add(new CanvasButton
                {
                    ID = ActionID.Refresh,
                    Text = Resources.RefreshText,
                    Font = DescriptionFont,
                    ForeColour = UI.System.ForegroundColour,
                    SpaceAfter = 15
                });

                // Manage Forum button
                if (Forum != null && Forum.IsModerator)
                {
                    newLayout.Add(new CanvasButton
                    {
                        ID = ActionID.ManageForum,
                        Text = Resources.ManageForum,
                        Font = DescriptionFont,
                        ForeColour = UI.System.ForegroundColour,
                        SpaceAfter = 15
                    });
                }
            }
            return newLayout;
        }
    }
}