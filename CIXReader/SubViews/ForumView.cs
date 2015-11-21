// *****************************************************
// CIXReader
// ForumsView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/10/2013 1:34 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.CanvasItems;
using CIXReader.Forms;
using CIXReader.Properties;
using CIXReader.SpecialFolders;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.SubViews
{
    /// <summary>
    /// A ForumsView implements the subview for displaying details of a selected forum.
    /// </summary>
    internal sealed partial class ForumsView : ViewBaseView
    {
        // Display fonts
        private Font _nameFont;
        private Font _titleFont;
        private Font _descriptionFont;

        private TopicFolder _currentFolder;
        private DirForum _thisForum;

        /// <summary>
        /// Creates a ForumView instance with the given ForumView parent.
        /// </summary>
        /// <param name="foldersTree">The parent forum view</param>
        public ForumsView(FoldersTree foldersTree) : base("Forums")
        {
            FoldersTree = foldersTree;
            InitializeComponent();
        }

        /// <summary>
        /// Display the page for the specified forum.
        /// </summary>
        public override bool ViewFromFolder(FolderBase folder, Address address, FolderOptions flags)
        {
            if (folder is TopicFolder)
            {
                _currentFolder = folder as TopicFolder;
                _thisForum = CIX.DirectoryCollection.ForumByName(_currentFolder.Name);

                FoldersTree.SetTopicName(folder.Name);

                CIX.DirectoryCollection.RefreshForum(folder.Name);

                FillCanvas();
            }
            return true;
        }

        /// <summary>
        /// Override to return the URL of the view being displayed.
        /// </summary>
        public override string Address
        {
            get { return string.Format("cix:{0}", _currentFolder.Name); }
        }

        /// <summary>
        /// Return whether the specified action can be carried out.
        /// </summary>
        /// <param name="id">An action ID</param>
        public override bool CanAction(ActionID id)
        {
            switch (id)
            {
                case ActionID.ManageForum:
                    return _thisForum != null && _thisForum.IsModerator;

                case ActionID.Participants:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Action the specified ID.
        /// </summary>
        /// <param name="id">An action ID</param>
        public override void Action(ActionID id)
        {
            switch (id)
            {
                case ActionID.Participants:
                    FoldersTree.DisplayParticipants(_thisForum.Name);
                    break;

                case ActionID.ManageForum:
                    FoldersTree.ManageForum(_thisForum);
                    break;

                case ActionID.NextUnread:
                    FoldersTree.NextUnread(FolderOptions.NextUnread);
                    break;

                case ActionID.NextPriorityUnread:
                    FoldersTree.NextUnread(FolderOptions.NextUnread|FolderOptions.Priority);
                    break;

                case ActionID.PageMessage:
                    FoldersTree.NextUnread(FolderOptions.NextUnread);
                    break;
            }
        }

        /// <summary>
        /// Set the focus to the what is the primary control in the view
        /// </summary>
        public override void SetFocus()
        {
            ActiveControl = frmCanvas;
        }

        /// <summary>
        /// Get or set the associated folders tree view.
        /// </summary>
        private FoldersTree FoldersTree { get; set; }

        /// <summary>
        /// Initialise the forum view UI.
        /// </summary>
        private void ForumView_Load(object sender, EventArgs e)
        {
            RefreshTheme();

            CIX.FolderCollection.FolderUpdated += OnFolderUpdated;
            CIX.DirectoryCollection.ForumUpdated += OnDirForumUpdated;
            CIX.DirectoryCollection.ModeratorsUpdated += OnModeratorsUpdated;
            CIX.MugshotUpdated += OnMugshotUpdated;

            frmCanvas.LinkClicked += OnLinkClicked;
            frmCanvas.CanvasItemAction += OnItemAction;

            // Get notified of theme changes.
            UI.ThemeChanged += OnThemeChanged;
        }

        /// <summary>
        /// Update the list of moderators
        /// </summary>
        private void OnModeratorsUpdated(DirForum forum)
        {
            Platform.UIThread(this, delegate
            {
                if (forum == _thisForum && frmCanvas.Items.Count > 1)
                {
                    ForumPage forumFolderItem = (ForumPage) frmCanvas.Items[0];
                    forumFolderItem.Forum = _thisForum;
                    forumFolderItem.InvalidateItem();

                    ProfileGroupItem moderatorsItem = (ProfileGroupItem) frmCanvas.Items[1];
                    moderatorsItem.Items = new StringCollection();
                    if (_thisForum != null)
                    {
                        moderatorsItem.Items.AddRange(_thisForum.Moderators());
                    }
                    moderatorsItem.InvalidateItem();
                }
            });
        }

        /// <summary>
        /// Handle mugshot change events. We pass this through to the moderator component to
        /// refresh any that are affected.
        /// </summary>
        private void OnMugshotUpdated(Mugshot mugshot)
        {
            Platform.UIThread(this, delegate
            {
                if (frmCanvas.Items.Count > 1)
                {
                    ProfileGroupItem moderatorsItem = (ProfileGroupItem) frmCanvas.Items[1];
                    moderatorsItem.Refresh(mugshot.Username);
                }
            });
        }

        /// <summary>
        /// Respond to links being clicked by sending them to the mainform to be processed.
        /// </summary>
        private void OnLinkClicked(object sender, LinkClickedEventArgs args)
        {
            FoldersTree.MainForm.Address = args.LinkText;
        }

        /// <summary>
        /// This event is triggered when the user performs an action on a canvas item.
        /// </summary>
        /// <param name="sender">The thread view control</param>
        /// <param name="args">A CanvasItemArgs object that contains details of the action</param>
        private void OnItemAction(object sender, CanvasItemArgs args)
        {
            switch (args.Item)
            {
                case ActionID.AuthorImage:
                {
                    FoldersTree.MainForm.Address = string.Format("cixuser:{0}", args.Tag);
                    break;
                }

                case ActionID.ManageForum:
                case ActionID.Participants:
                    Action(args.Item);
                    break;

                case ActionID.Delete:
                {
                    CanvasElementBase deleteButton = args.Control.CanvasItemLayout[ActionID.Delete];
                    if (deleteButton.Enabled)
                    {
                        string promptString = string.Format(Resources.ConfirmDelete, _currentFolder.Name);
                        if (MessageBox.Show(promptString, Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            deleteButton.Enabled = false;
                            args.Control.CanvasItemLayout[ActionID.JoinForum].Enabled = false;

                            _currentFolder.Folder.Delete();
                        }
                    }
                    break;
                }

                case ActionID.ResignForum:
                {
                    CanvasElementBase resignButton = args.Control.CanvasItemLayout[ActionID.ResignForum];
                    if (resignButton.Enabled)
                    {
                        string promptString = string.Format(Resources.ConfirmResign, _currentFolder.Name);
                        if (MessageBox.Show(promptString, Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            resignButton.Enabled = false;

                            _currentFolder.Folder.Resign();
                        }
                    }
                    break;
                }

                case ActionID.JoinForum:
                {
                    string promptString = string.Format(Resources.ConfirmJoin, _currentFolder.Name);
                    if (MessageBox.Show(promptString, Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        _thisForum.Join();
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Event invoked when a forum's details are refreshed from the server.
        /// </summary>
        private void OnDirForumUpdated(DirForum forum)
        {
            if (forum == null || forum.Name != _currentFolder.Name)
            {
                return;
            }

            Platform.UIThread(this, delegate
            {
                _thisForum = forum;

                if (frmCanvas.Items.Count > 0)
                {
                    ForumPage forumFolderItem = (ForumPage) frmCanvas.Items[0];
                    forumFolderItem.Forum = _thisForum;
                    forumFolderItem.InvalidateItem();
                }
            });
        }

        /// <summary>
        /// Called when details of a forum are refreshed from the server. This may be the title
        /// or description.
        /// </summary>
        private void OnFolderUpdated(Folder folder)
        {
            Platform.UIThread(this, delegate
            {
                // Handle changes to the topic name or description
                if (folder == _currentFolder.Folder && frmCanvas.Items.Count > 0)
                {
                    ForumPage forumFolderItem = (ForumPage) frmCanvas.Items[0];
                    forumFolderItem.InvalidateItem();
                }
            });
        }

        /// <summary>
        /// Handle theme changed events to change the forums theme.
        /// </summary>
        private void OnThemeChanged(object sender, EventArgs args)
        {
            RefreshTheme();

            // Force redraw of any affected controls
            frmCanvas.Invalidate();
        }

        /// <summary>
        /// Update our copy of the font and font metrics when the theme changes.
        /// </summary>
        private void RefreshTheme()
        {
            frmCanvas.SelectionColour = UI.System.SelectionColour;
            frmCanvas.CommentColour = UI.Forums.CommentColour;
            frmCanvas.SeparatorColour = UI.System.EdgeColour;

            _nameFont = UI.GetFont(UI.System.Font, 26);
            _titleFont = UI.GetFont(UI.System.Font, 14);
            _descriptionFont = UI.GetFont(UI.System.Font, 10);
        }

        /// <summary>
        /// Add all the messages in the specified list to the current thread being displayed.
        /// </summary>
        public void FillCanvas()
        {
            frmCanvas.BeginUpdate();
            frmCanvas.Items.Clear();

            ForumPage folderItem = new ForumPage(frmCanvas, false)
            {
                Folder = _currentFolder.Folder,
                Forum = _thisForum,
                NameFont = _nameFont,
                TitleFont = _titleFont,
                DescriptionFont = _descriptionFont,
                Font = UI.System.Font,
                BackColor = UI.Forums.CommentColour
            };
            frmCanvas.Items.Add(folderItem);

            StringCollection strings = new StringCollection();
            if (_thisForum != null)
            {
                strings.AddRange(_thisForum.Moderators());
            }
            ProfileGroupItem moderatorsItem = new ProfileGroupItem(frmCanvas, true)
            {
                Title = Resources.Moderators,
                Items = strings
            };
            frmCanvas.Items.Add(moderatorsItem);

            frmCanvas.EndUpdate(null);
        }
    }
}