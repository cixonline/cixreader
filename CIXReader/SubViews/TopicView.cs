// *****************************************************
// CIXReader
// TopicView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 10/10/2013 12:13 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Tables;
using CIXReader.Canvas;
using CIXReader.CanvasItems;
using CIXReader.Controls;
using CIXReader.Forms;
using CIXReader.Properties;
using CIXReader.SpecialFolders;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.SubViews
{
    /// <summary>
    /// A TopicView implements the subview for displaying details of a selected topic.
    /// </summary>
    public sealed partial class TopicView : ViewBaseView
    {
        private Font _font;
        private Font _rootFont;
        private FolderBase _currentFolder;
        private bool _showFullDate;
        private bool _initialising;
        private bool _isTopicFolder;
        private bool _showIgnored;
        private int _rowHeight;
        private bool _hasMouseDown;
        private bool _groupByConv;
        private bool _collapseConv;
        private bool _isFiltering;
        private int _lastIndex;
        private bool _suspendFixup;
        private string _currentFilterString;
        private ListViewItem[] _itemCache;
        private List<CIXMessage> _messages;

        private const int MaximumIndent = 10000;

        /// <summary>
        /// Specifies which root message SelectRoot should select.
        /// </summary>
        public enum RootToSelect
        {
            /// <summary>
            /// The previous root message
            /// </summary>
            PreviousRoot,

            /// <summary>
            /// The next root message
            /// </summary>
            NextRoot
        }

        /// <summary>
        /// Creates a TopicView instance with the given ForumView parent.
        /// </summary>
        /// <param name="foldersTree">The parent forum view</param>
        public TopicView(FoldersTree foldersTree) : base("Forums")
        {
            InitializeComponent();

            FoldersTree = foldersTree;
            SortOrderBase.DefaultSortOrder = CRSortOrder.SortOrder.Date;
        }

        /// <summary>
        /// Display the specified topic folder in the thread list.
        /// </summary>
        /// <param name="folder">The folder whose topic is to be displayed</param>
        /// <param name="address"></param>
        /// <param name="options">Folder display flags</param>
        public override bool ViewFromFolder(FolderBase folder, Address address, FolderOptions options)
        {
            if (folder.ViewForFolder == AppView.AppViewTopic)
            {
                if ((_currentFolder != folder) || options.HasFlag(FolderOptions.ClearFilter))
                {
                    _currentFolder = folder;
                    _currentFilterString = null;
                    _isFiltering = false;
                    _isTopicFolder = IsTopicFolder();

                    tsvMessages.SelectedIndices.Clear();
                    ShowMessage(null);

                    if (FoldersTree.MainForm.RunEvent(EventID.FolderSelected,
                        _isTopicFolder ? ((TopicFolder) folder).Folder : null))
                    {
                        if (_isTopicFolder)
                        {
                            // Attempt to make topic switches cleaner by removing the old messages from display
                            // when switching to a new topic will involve a delay caused by loading the messages
                            // from the DB, sorting, etc.
                            if (!((TopicFolder)_currentFolder).Folder.HasMessages && _messages != null && _messages.Count > 0)
                            {
                                _messages = new List<CIXMessage>();
                                InitialiseList();
                                RedrawAllItems();
                            }
                        }
                        SortConversations();
                    }

                    UpdateFromFlags();
                }

                // Load this topic from the server only if we're empty.
                if (_messages != null && _messages.Count == 0)
                {
                    if (address != null && address.Scheme == "cix")
                    {
                        Int32.TryParse(address.Data, out _lastIndex);
                    }
                    folder.Refresh();
                    return false;
                }

                if (options.HasFlag(FolderOptions.ClearFilter))
                {
                    options &= ~FolderOptions.ClearFilter;
                    _currentFilterString = null;
                    _isFiltering = false;
                }

                if (address != null && address.Scheme == "cix")
                {
                    int selectedID;
                    Int32.TryParse(address.Data, out selectedID);
                    if (!GoToMessage(selectedID))
                    {
                        SetInitialSelection();
                    }
                    if (address.Unread)
                    {
                        SelectedMessage.MarkUnread();
                    }
                }
                else if (options == 0)
                {
                    SetInitialSelection();
                }
                else
                {
                    int row = SelectedRow;
                    if (row < 0 || options.HasFlag(FolderOptions.Reset))
                    {
                        row = -1;
                    }
                    else
                    {
                        CIXMessage selectedMessage = SelectedMessage;
                        if (selectedMessage != null && selectedMessage.Unread)
                        {
                            selectedMessage.MarkRead();
                        }
                    }
                    if (!FirstUnreadAfterRow(row, options))
                    {
                        return false;
                    }
                }

                ActiveControl = tsvMessages;
            }
            return true;
        }

        /// <summary>
        /// Display a directory of forums filtered by the given search string. If the search string is
        /// empty then all forums are displayed.
        /// </summary>
        /// <param name="searchString">A search string</param>
        public override void FilterViewByString(string searchString)
        {
            _currentFilterString = searchString.Trim().ToLower();
            CIXMessage selectedMessage = SelectedMessage;

            tsvMessages.SelectedIndices.Clear();
            AssignArrayOfMessages();

            _isFiltering = !string.IsNullOrEmpty(_currentFilterString);
            if (_isFiltering)
            {
                _messages = _messages.Where(msg => 
                    msg.Author.IndexOf(_currentFilterString, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    msg.Body.IndexOf(_currentFilterString, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
            }

            InitialiseList();
            RedrawAllItems();

            RestoreSelection(selectedMessage);

            if (SelectedRow == -1)
            {
                SetInitialSelection();
            }
        }

        /// <summary>
        /// Set the focus to the what is the primary control in the view
        /// </summary>
        public override void SetFocus()
        {
            ActiveControl = tsvMessages;
        }

        /// <summary>
        /// Override to return the URL of the view being displayed.
        /// </summary>
        public override string Address
        {
            get
            {
                int currentSelectedRow = SelectedRow;
    
                if (currentSelectedRow > -1)
                {
                    CIXMessage message = _messages[currentSelectedRow];
                    return AddressFromMessage(message);
                }
                return _currentFolder != null ? _currentFolder.Address : string.Empty;
            }
        }

        /// <summary>
        /// Indicate that we handle the cix scheme.
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public override bool Handles(string scheme)
        {
            return scheme == "cix";
        }

        /// <summary>
        /// Return the sort menu for this view.
        /// </summary>
        public override ContextMenuStrip SortMenu
        {
            get { return FoldersTree.MainForm.TopicSortMenu; }
        }

        /// <summary>
        /// Return the text string for the specified action.
        /// </summary>
        public override string TitleForAction(ActionID id)
        {
            if (SelectedMessage != null)
            {
                CIXMessage message = SelectedMessage;
                switch (id)
                {
                    case ActionID.ReadLock:
                        return message.ReadLocked ? Resources.ClearReadLock : Resources.SetReadLock;

                    case ActionID.Ignore:
                        return message.Ignored ? Resources.AsUnignored : Resources.AsIgnored;

                    case ActionID.Priority:
                        return message.Priority ? Resources.AsNormal : Resources.AsPriority;

                    case ActionID.Star:
                        return message.Starred ? Resources.ClearFlag : Resources.Flag;

                    case ActionID.Read:
                        return message.Unread ? Resources.AsRead : Resources.AsUnread;

                    case ActionID.Withdraw:
                        return message.IsPseudo ? Resources.DeleteOnMenu : Resources.WithdrawOnMenu;
                }
            }
            return null;
        }

        /// <summary>
        /// Return whether the specified action can be carried out.
        /// </summary>
        /// <param name="id">An action ID</param>
        public override bool CanAction(ActionID id)
        {
            switch (id)
            {
                case ActionID.Profile:
                case ActionID.MarkThreadReadThenRoot:
                case ActionID.MarkThreadRead:
                case ActionID.ReadLock:
                case ActionID.Ignore:
                case ActionID.Star:
                case ActionID.Priority:
                case ActionID.Link:
                case ActionID.SelectAll:
                case ActionID.Print:
                case ActionID.ReplyByMail:
                case ActionID.Block:
                    if (SelectedMessage != null)
                    {
                        CIXMessage message = SelectedMessage;
                        return !message.IsPseudo;
                    }
                    return false;

                case ActionID.ManageForum:
                    if (_currentFolder.ID > 0)
                    {
                        TopicFolder topicFolder = (TopicFolder)_currentFolder;
                        Folder forumFolder = topicFolder.Folder.ParentFolder;
                        DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                        return forum != null && forum.IsModerator;
                    }
                    return false;

                case ActionID.Expand:
                    return SelectedMessage != null && IsExpandable(SelectedMessage);

                case ActionID.Copy:
                    return DisplayedItem != null && !string.IsNullOrWhiteSpace(DisplayedItem.Selection);

                case ActionID.GoTo:
                    return _currentFolder.ID > 0 && _messages.Count > 0;

                case ActionID.Original:
                    if (SelectedMessage != null)
                    {
                        CIXMessage message = SelectedMessage;
                        return message.CommentID != 0;
                    }
                    return false;

                case ActionID.Refresh:
                    return _currentFolder.ID > 0;

                case ActionID.Participants:
                    return true;

                case ActionID.Read:
                    if (SelectedMessage != null)
                    {
                        CIXMessage message = SelectedMessage;
                        return !message.IsPseudo && !message.ReadLocked;
                    }
                    return false;

                case ActionID.NextRoot:
                {
                    int selectedIndex = SelectedRow;
                    return selectedIndex < _messages.Count - 1;
                }

                case ActionID.Root:
                    return SelectedRow > 0;

                case ActionID.NewMessage:
                {
                    if (_currentFolder.ID > 0)
                    {
                        TopicFolder topicFolder = (TopicFolder)_currentFolder;
                        Folder forumFolder = topicFolder.Folder.IsRootFolder ? topicFolder.Folder : topicFolder.Folder.ParentFolder;
                        DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                        bool isModerator = forum != null && forum.IsModerator;
                        bool isReadOnly = topicFolder.Folder.IsReadOnly;
                        return isModerator || !isReadOnly;
                    }
                    return false;
                }

                case ActionID.Delete:
                case ActionID.Withdraw:
                    if (SelectedMessage != null)
                    {
                        CIXMessage message = SelectedMessage;
                        Folder topic = message.Topic;
                        Folder forumFolder = topic.ParentFolder;
                        DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                        bool isModerator = forum != null && forum.IsModerator;

                        return isModerator || message.IsPseudo || message.IsMine;
                    }
                    return false;

                case ActionID.Edit:
                    if (SelectedMessage != null)
                    {
                        CIXMessage message = SelectedMessage;
                        return message.IsDraft;
                    }
                    return false;

                case ActionID.Reply:
                case ActionID.Quote:
                    if (SelectedMessage != null)
                    {
                        CIXMessage message = SelectedMessage;
                        Folder topicFolder = message.Topic;
                        if (topicFolder != null)
                        {
                            Folder forumFolder = topicFolder.ParentFolder;
                            DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                            bool isModerator = forum != null && forum.IsModerator;

                            if (topicFolder.IsReadOnly && !isModerator)
                            {
                                return false;
                            }
                            return !message.IsPseudo;
                        }
                    }
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Action the specified ID.
        /// </summary>
        /// <param name="id">An action ID</param>
        public override void Action(ActionID id)
        {
            Action(id, SelectedMessage);
        }

        /// <summary>
        /// Return the MessageItem for the actual displayed message. This must always be
        /// overridden.
        /// </summary>
        public override MessageItem DisplayedItem
        {
            get
            {
                return (tsvMessagePane.Items.Count > 0) ? tsvMessagePane.Items[0] as MessageItem : null;
            }
        }

        /// <summary>
        /// Add a new message.
        /// </summary>
        private void NewMessage(string body)
        {
            if (_currentFolder.ID > 0)
            {
                TopicFolder topicFolder = (TopicFolder)_currentFolder;
                Folder forumFolder = topicFolder.Folder.ParentFolder;
                DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                bool isModerator = forum != null && forum.IsModerator;

                // Can't post to read-only folders
                if (topicFolder.Folder.IsReadOnly && !isModerator)
                {
                    return;
                }

                CIXMessage message = new CIXMessage
                {
                    Author = CIX.Username,
                    RemoteID = 0,
                    Priority = true,
                    Date = DateTime.Now,
                    Body = body,
                    TopicID = _currentFolder.ID,
                    RootID = 0,
                    CommentID = 0
                };

                EditMessage(message, true);
            }
        }

        /// <summary>
        /// Initialise the topic thread UI.
        /// </summary>
        private void TopicThread_Load(object sender, EventArgs args)
        {
            _initialising = true;

            RefreshTheme();

            ShowMessagePane();

            _showFullDate = Preferences.StandardPreferences.ShowFullDate;
            _groupByConv = Preferences.StandardPreferences.GroupByConv;
            _collapseConv = Preferences.StandardPreferences.CollapseConv;

            CIX.FolderCollection.MessageDeleted += OnMessageDeleted;
            CIX.FolderCollection.MessageChanged += OnMessageChanged;
            CIX.FolderCollection.MessageAdded += OnMessageAdded;

            CIX.FolderCollection.FolderRefreshed += OnFolderRefreshed;

            CIX.FolderCollection.ThreadChanged += OnThreadChanged;

            CIX.FolderCollection.TopicUpdateStarted += OnTopicUpdateStarted;
            CIX.FolderCollection.TopicUpdateCompleted += OnTopicUpdateCompleted;

            CIX.FolderCollection.MessagePostStarted += OnMessagePostStarted;
            CIX.FolderCollection.MessagePostCompleted += OnMessagePostCompleted;

            CIX.MugshotUpdated += OnMugshotUpdated;

            SortOrderBase.OrderingChanged += OnOrderingChanged;

            tsvMessages.SelectedIndexChanged += OnSelectionChanged;
            tsvMessages.MouseDown += OnMouseDown;

            tsvMessagePane.CanvasItemAction += OnItemAction;
            tsvMessagePane.LinkClicked += OnLinkClicked;

            tsvMessagePane.ExpandInlineImages = Preferences.StandardPreferences.DownloadInlineImages;
            tsvMessagePane.DisableMarkup = Preferences.StandardPreferences.IgnoreMarkup;

            _showIgnored = Preferences.StandardPreferences.ShowIgnored;

            // Get notified of preferences changes
            Preferences.PreferencesChanged += OnPreferencesChanged;

            // Get notified of theme changes.
            UI.ThemeChanged += OnThemeChanged;

            _initialising = false;
        }

        /// <summary>
        /// Handle click on read or star icons on an item to change the read or star state.
        /// </summary>
        private void OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            ListViewItem item = tsvMessages.GetItemAt(mouseEventArgs.Location.X, mouseEventArgs.Location.Y);
            if (item != null)
            {
                if (item.Index >= 0 && item.Index < _messages.Count)
                {
                    CIXMessage message = _messages[item.Index];
                    DrawRectElements elements = ComputeDrawRectElements(tsvMessages.CreateGraphics(), message, item.Bounds);

                    if (elements.ExpanderRect.Contains(mouseEventArgs.Location))
                    {
                        Action(ActionID.Expand, message);
                    }
                    else if (elements.ReadRect.Contains(mouseEventArgs.Location))
                    {
                        Action(ActionID.Read, message);
                    }
                    else if (elements.StarRect.Contains(mouseEventArgs.Location))
                    {
                        Action(ActionID.Star, message);
                    }
                }
            }
            _hasMouseDown = true;
        }

        /// <summary>
        /// Return the CIX address of the specified message.
        /// </summary>
        private static string AddressFromMessage(CIXMessage message)
        {
            Folder topic = CIX.FolderCollection[message.TopicID];
            Folder forum = topic.ParentFolder;
            return string.Format(@"cix:{0}/{1}:{2}", forum.Name, topic.Name, message.RemoteID);
        }

        /// <summary>
        /// Respond to changes in the sort order.
        /// </summary>
        private void OnOrderingChanged(object sender, EventArgs args)
        {
            SortConversations();
            ShowMessage(SelectedMessage);
        }

        /// <summary>
        /// Handle preferences change events.
        /// </summary>
        private void OnPreferencesChanged(object sender, PreferencesChangedEventArgs args)
        {
            if (args.Name == Preferences.MAPref_DownloadInlineImages)
            {
                tsvMessagePane.ExpandInlineImages = Preferences.StandardPreferences.DownloadInlineImages;
                RefreshMessagePane();
            }
            if (args.Name == Preferences.MAPref_IgnoreMarkup)
            {
                tsvMessagePane.DisableMarkup = Preferences.StandardPreferences.IgnoreMarkup;
                RefreshMessagePane();
            }
            if (args.Name == Preferences.MAPref_ShowIgnored)
            {
                _showIgnored = Preferences.StandardPreferences.ShowIgnored;
                ReloadList();
            }
            if (args.Name == Preferences.MAPref_ShowFullDate)
            {
                _showFullDate = Preferences.StandardPreferences.ShowFullDate;
                RefreshList();
                RefreshMessagePane();
            }
            if (args.Name == Preferences.MAPref_DefaultSearchEngine)
            {
                RefreshList();
            }
            if (args.Name == Preferences.MAPref_GroupByConv)
            {
                _groupByConv = Preferences.StandardPreferences.GroupByConv;
                ReloadList();
            }
            if (args.Name == Preferences.MAPref_CollapseConv)
            {
                _collapseConv = Preferences.StandardPreferences.CollapseConv;
                ReloadList();
            }
        }

        /// <summary>
        /// Update the UI based on the newly selected folder.
        /// </summary>
        private void UpdateFromFlags()
        {
            if (IsTopicFolder())
            {
                TopicFolder topicFolder = (TopicFolder)_currentFolder;
                Folder forumFolder = topicFolder.Folder.ParentFolder;
                DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                bool isModerator = forum != null && forum.IsModerator;

                bool isReadOnly = topicFolder.Folder.IsReadOnly;

                if (!isModerator)
                {
                    if (isReadOnly)
                    {
                        FoldersTree.ShowInfoBar(Resources.FolderIsReadOnly);
                    }
                    else if (topicFolder.Flags.HasFlag(FolderFlags.OwnerCommentsOnly))
                    {
                        FoldersTree.ShowInfoBar(Resources.OwnerPostingOnly);
                    }
                }
            }
        }

        /// <summary>
        /// Display the UI for when a topic update has started
        /// </summary>
        private void OnTopicUpdateStarted(object sender, EventArgs args)
        {
            Platform.UIThread(this, StartProgressBar);
        }

        /// <summary>
        /// Display the UI for when a topic update has completed.
        /// </summary>
        private void OnTopicUpdateCompleted(object sender, FolderEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                StopProgressBar();

                // If a message was requested but not selected because the topic wasn't filled
                // at the time then select it now.
                if (IsTopicFolder())
                {
                    TopicFolder topicFolder = (TopicFolder) _currentFolder;
                    if (args.Folder == topicFolder.Folder)
                    {
                        ReloadList();

                        if (_lastIndex != 0)
                        {
                            GoToMessage(_lastIndex);
                            _lastIndex = 0;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Handle folder being refreshed.
        /// </summary>
        private void OnFolderRefreshed(object sender, FolderEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                if (_currentFolder.ID == args.Folder.ID)
                {
                    _suspendFixup = args.Fixup;
                    ReloadList();
                    _suspendFixup = false;
                    if (_messages.Count == 0)
                    {
                        ShowEmptyMessage();
                    }
                    if (SelectedRow == -1)
                    {
                        SetInitialSelection();
                    }
                }
            });
        }

        /// <summary>
        /// Handle theme changed events to change the forums theme.
        /// </summary>
        private void OnThemeChanged(object sender, EventArgs args)
        {
            RefreshTheme();
            RefreshList();
            RefreshMessagePane();
        }

        /// <summary>
        /// Return the appropriate body colour for the given message.
        /// </summary>
        private static Color BodyColourForItem(CIXMessage message)
        {
            return message.Ignored ? UI.Forums.IgnoredColour : UI.Forums.BodyColour;
        }

        /// <summary>
        /// Reload the list
        /// </summary>
        private void ReloadList()
        {
            CIXMessage message = SelectedMessage;
            SortConversations();
            RestoreSelection(message);
        }

        /// <summary>
        /// Loop over all messages and force a refresh on them.
        /// </summary>
        private void RefreshList()
        {
            if (_messages.Count > 0)
            {
                CIXMessage savedMessage = SelectedMessage;
                RedrawAllItems();
                RestoreSelection(savedMessage);
            }
        }

        /// <summary>
        /// Refresh the message pane.
        /// </summary>
        private void RefreshMessagePane()
        {
            if (tsvMessagePane.Items.Count > 0)
            {
                MessageItem messageItem = tsvMessagePane.Items[0] as MessageItem;
                if (messageItem != null)
                {
                    messageItem.Font = UI.Forums.MessageFont;
                    messageItem.ForeColor = UI.Forums.BodyColour;
                    messageItem.FullDateString = _showFullDate;
                    messageItem.Highlight = _currentFilterString;
                    messageItem.InvalidateItem();
                    messageItem.Update();
                }
            }
        }

        /// <summary>
        /// Return the appropriate font for this item.
        /// </summary>
        private Font FontForItem(CIXMessage message)
        {
            return message.IsRoot ? _rootFont : _font;
        }

        /// <summary>
        /// Update our copy of the font and font metrics when the theme changes.
        /// </summary>
        private void RefreshTheme()
        {
            _font = UI.GetFont(UI.Forums.font, UI.Forums.fontsize);
            _rootFont = UI.GetFont(UI.Forums.rootfont, UI.Forums.rootfontsize, FontStyle.Bold);
            _rowHeight = (_font.Height + 4)*2;

            tsvMessages.Font = _font;
            tsvMessages.SetHeight(_rowHeight);

            tsvSplitview.BackColor = UI.System.SplitterBarColour;
        }

        /// <summary>
        /// Scroll to and display the message whose remote ID is the given value.
        /// </summary>
        private bool GoToMessage(int value)
        {
            int selectedIndex;
            for (selectedIndex = 0; selectedIndex < _messages.Count; ++selectedIndex)
            {
                CIXMessage message = _messages[selectedIndex];
                if (message.RemoteID == value)
                {
                    break;
                }
            }
            if (selectedIndex < _messages.Count)
            {
                SelectedRow = selectedIndex;
                return true;
            }

            // Possibly collapsed? Look for the root
            if (_currentFolder.ID >= 0 && _collapseConv)
            {
                TopicFolder topicFolder = (TopicFolder) _currentFolder;
                CIXMessage message = topicFolder.Folder.Messages.MessageByID(value);

                if (message != null)
                {
                    if (IsCollapsed(message.Root))
                    {
                        ExpandThread(message.Root);
                        SelectedRow = _messages.IndexOf(message);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This notification is triggered if a mugshot is updated from the server. We use this
        /// to refresh the images shown in the thread.
        /// </summary>
        /// <param name="mugshot">The mugshot object</param>
        private void OnMugshotUpdated(Mugshot mugshot)
        {
            Platform.UIThread(this, delegate
            {
                RefreshList();

                // Update any mugshot in the message pane too
                if (tsvMessagePane.Items.Count > 0)
                {
                    MessageItem control = tsvMessagePane.Items[0] as MessageItem;
                    if (control != null)
                    {
                        CIXMessage message = control.Message;
                        if (message.Author == mugshot.Username)
                        {
                            control.Image = mugshot.RealImage;
                            control.UpdateImage();
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Delegate invoked when a message has been posted.
        /// </summary>
        private void OnMessagePostStarted(object sender, MessagePostEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                if (args.Message != null && args.Message.Body != null)
                {
                    string statusText = string.Format(Resources.StatusMessagePost, args.Message.Body.FirstLine());
                    FoldersTree.MainForm.StartStatusProgressSpinner(statusText);
                }
            });
        }

        /// <summary>
        /// Delegate invoked when a message is being posted.
        /// </summary>
        private void OnMessagePostCompleted(object sender, MessagePostEventArgs args)
        {
            Platform.UIThread(this, () => FoldersTree.MainForm.StopStatusProgressSpinner());
        }

        /// <summary>
        /// Respond to thread change events
        /// </summary>
        /// <param name="message">The root message of the thread that changed</param>
        private void OnThreadChanged(CIXMessage message)
        {
            Platform.UIThread(this, delegate
            {
                int startIndex = _messages.IndexOf(message);
                if (startIndex != -1)
                {
                    int rowIndex = startIndex;
                    int countOfRemoved = 0;
                    if (!CanShowMessage(message))
                    {
                        _messages.Remove(message);
                        ++countOfRemoved;
                    }
                    else
                    {
                        ++rowIndex;
                    }
                    while (rowIndex < _messages.Count)
                    {
                        CIXMessage childMessage = _messages[rowIndex];
                        if (childMessage.Level <= message.Level)
                        {
                            break;
                        }
                        if (!CanShowMessage(childMessage))
                        {
                            _messages.Remove(childMessage);
                            ++countOfRemoved;
                        }
                        else
                        {
                            ++rowIndex;
                        }
                    }
                    if (countOfRemoved > 0)
                    {
                        InitialiseList();
                        rowIndex = _messages.Count;
                    }
                    if (startIndex >= 0 && rowIndex > startIndex && rowIndex <= _messages.Count)
                    {
                        tsvMessages.RedrawItems(startIndex, rowIndex - 1, false);
                    }
                    if (startIndex >= _messages.Count)
                    {
                        startIndex = _messages.Count - 1;
                    }
                    SelectedRow = startIndex;
                    DisplaySelectedRow(true);
                }
            });
        }

        /// <summary>
        /// Initialise the virtual list view records with the capacity from the
        /// _messages list.
        /// </summary>
        private void InitialiseList()
        {
            _itemCache = new ListViewItem[_messages.Count];
            tsvMessages.VirtualListSize = _messages.Count;
        }

        /// <summary>
        /// Respond to message change events
        /// </summary>
        /// <param name="message">The CIXMessage that changed</param>
        private void OnMessageChanged(CIXMessage message)
        {
            Platform.UIThread(this, delegate
            {
                int rowIndex = _messages.IndexOf(message);
                if (rowIndex != -1)
                {
                    if (!CanShowMessage(message))
                    {
                        RemoveMessage(message);
                        return;
                    }
                    tsvMessages.RedrawItems(rowIndex, rowIndex, false);
                }
                if (message == SelectedMessage)
                {
                    RefreshMessagePane();
                }
            });
        }

        /// <summary>
        /// This event is raised when a message is added on the database.
        /// </summary>
        /// <param name="message">Message being added</param>
        private void OnMessageAdded(CIXMessage message)
        {
            Platform.UIThread(this, delegate
            {
                if (CanShowMessage(message))
                {
                    ReloadList();
                }
            });
        }

        /// <summary>
        /// This event is raised when a message is deleted from the database.
        /// </summary>
        private void OnMessageDeleted(CIXMessage message)
        {
            Platform.UIThread(this, delegate
            {
                if (message.TopicID == _currentFolder.ID || _currentFolder is SmartFolder)
                {
                    RemoveMessage(message);
                }
            });
        }

        /// <summary>
        /// Remove the specified message from the list.
        /// </summary>
        /// <param name="message">The message to remove</param>
        private void RemoveMessage(CIXMessage message)
        {
            int selectedRow = SelectedRow;
            bool deletingCurrent = (selectedRow != -1) && message == _messages[selectedRow];

            _messages.Remove(message);
            
            InitialiseList();

            if (selectedRow == _messages.Count())
            {
                --selectedRow;
            }
            RedrawAllItems();
            if (deletingCurrent)
            {
                if (selectedRow < 0)
                {
                    ShowEmptyMessage();
                }
                else
                {
                    SelectedRow = selectedRow;
                }
            }                
        }

        /// <summary>
        /// Returns whether or not this folder is a topic folder.
        /// </summary>
        private bool IsTopicFolder()
        {
            if (_currentFolder.ID > 0)
            {
                TopicFolder topicFolder = _currentFolder as TopicFolder;
                if (topicFolder != null && !topicFolder.Folder.IsRootFolder)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Display the first unread message after the specified row.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private bool FirstUnreadAfterRow(int row, FolderOptions options)
        {
            while (++row < _messages.Count)
            {
                CIXMessage message = _messages[row];

                if (IsCollapsed(message) && message.UnreadChildren > 0)
                {
                    ExpandThread(message);
                }
                if (options.HasFlag(FolderOptions.Priority))
                {
                    if (message.Priority && message.Unread)
                    {
                        SelectedRow = row;
                        break;
                    }
                }
                else if (options.HasFlag(FolderOptions.NextUnread) && message.Unread)
                {
                    if (options.HasFlag(FolderOptions.Root))
                    {
                        while (message.CommentID > 0 && row > 0)
                        {
                            message = _messages[--row];
                        }
                    }
                    SelectedRow = row;
                    break;
                }
            }
            return (row != _messages.Count);   
        }

        /// <summary>
        /// Put the focus on the first message in the folder, if there is one.
        /// </summary>
        private void SetInitialSelection()
        {
            if (!FirstUnreadAfterRow(-1, FolderOptions.NextUnread))
            {
                if (_currentFolder.RecentMessage == 0 || !GoToMessage(_currentFolder.RecentMessage))
                {
                    SelectedRow = SortOrderBase.Ascending ? _messages.Count - 1 : 0;
                }
            }
        }

        /// <summary>
        /// Initial load of the thread of all messages currently in the database for the given
        /// folder, up to a maximum of _maxRootsPerPage root messages.
        /// </summary>
        private void SortConversations()
        {
            AssignArrayOfMessages();

            InitialiseList();
            RedrawAllItems();
        }

        /// <summary>
        ///  Redraw all items on view
        /// </summary>
        private void RedrawAllItems()
        {
            if (_messages.Count > 0)
            {
                int topIndex = tsvMessages.TopItem.Index;
                int itemsOnPage = tsvMessages.ClientRectangle.Height / _rowHeight;
                tsvMessages.RedrawItems(topIndex, Math.Min(topIndex + itemsOnPage, _messages.Count - 1), false);
            }
            else
            {
                tsvMessages.Update();
            }
        }

        /// <summary>
        /// Reload the array of messages.
        /// </summary>
        private void AssignArrayOfMessages()
        {
            _messages = new List<CIXMessage>(MessagesByOrder(_currentFolder));
            if (!_showIgnored)
            {
                _messages.RemoveAll(msg => msg.Ignored);
            }
            if (_currentFolder.ID > 0 && !_suspendFixup)
            {
                TopicFolder topicFolder = (TopicFolder) _currentFolder;
                topicFolder.Folder.Fixup();
            }
            tsvMessages.VirtualMode = true;
        }

        /// <summary>
        /// Return whether we can display and render by conversation grouping. For this to be
        /// allowed, we need to have conversation grouping enabled, not be in a smart folder
        /// and not be filtering by a search string.
        /// </summary>
        private bool CanGroupByConversation()
        {
            return _groupByConv && _currentFolder.ID > 0 && !_isFiltering;
        }

        /// <summary>
        /// Return a list of all messages ordered by the current folder ordering.
        /// </summary>
        private IEnumerable<CIXMessage> MessagesByOrder(FolderBase folder)
        {
            CRSortOrder.SortOrder sortOrder = OrderForFolder();

            if (CanGroupByConversation())
            {
                TopicFolder topicFolder = (TopicFolder)folder;
                List<CIXMessage> messages;

                switch (sortOrder)
                {
                    case CRSortOrder.SortOrder.Author:
                        messages = SortOrderBase.Ascending 
                            ? topicFolder.Folder.Messages.Roots.OrderBy(rt => rt.Author).ToList()
                            : topicFolder.Folder.Messages.Roots.OrderByDescending(rt => rt.Author).ToList();
                        break;

                    case CRSortOrder.SortOrder.Subject:
                        messages = SortOrderBase.Ascending 
                            ? topicFolder.Folder.Messages.Roots.OrderBy(rt => rt.Subject).ToList()
                            : topicFolder.Folder.Messages.Roots.OrderByDescending(rt => rt.Subject).ToList();
                        break;

                    case CRSortOrder.SortOrder.Date:
                        messages = SortOrderBase.Ascending
                            ? topicFolder.Folder.Messages.Roots.OrderBy(rt => rt.LatestDate).ToList()
                            : topicFolder.Folder.Messages.Roots.OrderByDescending(rt => rt.LatestDate).ToList();
                        break;

                    default:
                        return null;
                }

                if (!_collapseConv)
                {
                    for (int index = messages.Count - 1; index >= 0; index--)
                    {
                        CIXMessage message = messages[index];
                        messages.InsertRange(index + 1, topicFolder.Folder.Messages.Children(message));
                    }
                }
                return messages;
            }
            switch (sortOrder)
            {
                case CRSortOrder.SortOrder.Author:
                    return SortOrderBase.Ascending
                        ? folder.Items.OrderBy(rt => rt.Author).ToList()
                        : folder.Items.OrderByDescending(rt => rt.Author).ToList();

                case CRSortOrder.SortOrder.Subject:
                    return SortOrderBase.Ascending
                        ? folder.Items.OrderBy(rt => rt.Body.FirstUnquotedLine()).ToList()
                        : folder.Items.OrderByDescending(rt => rt.Body.FirstUnquotedLine()).ToList();

                case CRSortOrder.SortOrder.Date:
                    return SortOrderBase.Ascending
                        ? folder.Items.OrderBy(rt => rt.Date).ToList()
                        : folder.Items.OrderByDescending(rt => rt.Date).ToList();
            }
            return null;
        }

        /// <summary>
        /// Return whether or not the specified message can be displayed.
        /// </summary>
        private bool CanShowMessage(CIXMessage cixMessage)
        {
            if (_currentFolder == null || !_currentFolder.CanContain(cixMessage))
            {
                return false;
            }

            return !cixMessage.Ignored || _showIgnored;
        }

        /// <summary>
        /// Select the original message for this if there is one. Download it
        /// if it is missing.
        /// </summary>
        private void SelectOriginal(CIXMessage message)
        {
            if (message != null && message.CommentID > 0)
            {
                Folder topic = message.Topic;
                Folder forum = topic.ParentFolder;
                FoldersTree.MainForm.Address = String.Format("cix:{0}/{1}:{2}", forum.Name, topic.Name, message.CommentID);
            }
        }

        /// <summary>
        /// Mark the entire thread, of which the specified message is part, as
        /// read.
        /// </summary>
        private static void MarkThreadRead(CIXMessage message)
        {
            if (message != null)
            {
                message.MarkThreadRead();
            }
        }

        /// <summary>
        /// Mark the entire topic read.
        /// </summary>
        private void MarkTopicRead()
        {
            if (_currentFolder.ID > 0)
            {
                TopicFolder topicFolder = (TopicFolder) _currentFolder;
                FoldersTree.HandleMarkFolderRead(topicFolder.Folder);
            }
        }

        /// <summary>
        /// The selection has changed, so start a timer to mark the selected item as
        /// read if it is currently unread.
        /// </summary>
        private void OnSelectionChanged(object sender, EventArgs args)
        {
            if (SelectedRow >= 0)
            {
                DisplaySelectedRow(!_hasMouseDown);
                _hasMouseDown = false;
            }
        }

        /// <summary>
        /// Redisplay the selected message.
        /// </summary>
        private void DisplaySelectedRow(bool center)
        {
            int newSelection = SelectedRow;
            if (newSelection >= 0)
            {
                if (center)
                {
                    CentreSelection();
                }

                CIXMessage message = _messages[newSelection];
                Folder topic = message.Topic;
                Folder forum = topic.ParentFolder;

                string messageAddress = String.Format("cix:{0}/{1}:{2}", forum.Name, topic.Name, message.RemoteID);
                FoldersTree.MainForm.AddBacktrack(messageAddress, message.Unread);

                ShowMessage(message);
                FoldersTree.MainForm.RunEvent(EventID.MessageSelected, message);
                tsvMessages.Focus();
            }
        }

        /// <summary>
        /// Select the current root message or the next root message.
        /// </summary>
        private void SelectRoot(CIXMessage message, RootToSelect value)
        {
            if (message != null)
            {
                switch (value)
                {
                    case RootToSelect.PreviousRoot:
                    case RootToSelect.NextRoot:
                    {
                        int direction = (value == RootToSelect.NextRoot) ? 1 : -1;
                        int indexOfMessage = _messages.IndexOf(message) + direction;
                        while (indexOfMessage >= 0 && indexOfMessage < _messages.Count)
                        {
                            message = _messages[indexOfMessage];
                            if (message.IsRoot)
                            {
                                SelectedRow = indexOfMessage;
                                return;
                            }
                            indexOfMessage += direction;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get or set the index of the selected row.
        /// </summary>
        private int SelectedRow
        {
            get
            {
                ListView.SelectedIndexCollection selectedItems = tsvMessages.SelectedIndices;
                if (selectedItems.Count == 1)
                {
                    return selectedItems[0];
                }
                return -1;
            }
            set
            {
                if (value >= 0 && value < _messages.Count)
                {
                    if (!tsvMessages.Items[value].Selected)
                    {
                        tsvMessages.Items[value].Selected = true;
                        tsvMessages.Items[value].Focused = true;
                        tsvMessages.EnsureVisible(value);
                        CentreSelection();
                    }
                }
                else
                {
                    ShowEmptyMessage();
                }
                tsvMessages.Select();
            }
        }

        /// <summary>
        /// Ensure the selection is centered.
        /// </summary>
        private void CentreSelection()
        {
            int selectedRow = SelectedRow;

            if (selectedRow >= 0)
            {
                int itemsOnPage = tsvMessages.ClientRectangle.Height / _rowHeight;
                int topItem = Math.Max(0, selectedRow - itemsOnPage/2);
                tsvMessages.TopItem = tsvMessages.Items[topItem];

                int indent = IndentForItem(_messages[selectedRow]);
                if (indent > tsvMessages.ClientRectangle.Width/2)
                {
                    tsvMessages.ScrollHorizontal(indent - tsvMessages.ClientRectangle.Width/2);
                }
                else
                {
                    tsvMessages.ScrollHorizontal(0);
                }
            }
        }

        /// <summary>
        /// Determine if the message specified needs to be scrolled
        /// up because it is too long to fit in the thread window. If so
        /// then we scroll it up and return true, otherwise we return
        /// false.
        /// </summary>
        private bool ScrollMessageUp()
        {
            MessageItem item = tsvMessagePane.Items[0] as MessageItem;
            if (item != null && item.Bounds.Height + item.Bounds.Top > tsvMessagePane.Height)
            {
                int delta = -(tsvMessagePane.Height - (item.Bounds.Height + item.Bounds.Top));
                if (delta > 0)
                {
                    int X = -tsvMessagePane.AutoScrollPosition.X;
                    int Y = -tsvMessagePane.AutoScrollPosition.Y + tsvMessagePane.VerticalScroll.LargeChange;
                    tsvMessagePane.AutoScrollPosition = new Point(X, Y);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Return the current selected message item.
        /// </summary>
        private CIXMessage SelectedMessage
        {
            get
            {
                int selectedRow = SelectedRow;
                return selectedRow >= 0 && selectedRow < _messages.Count ? _messages[selectedRow] : null;
            }
        }

        /// <summary>
        /// Put the selection back on the specified message.
        /// </summary>
        /// <param name="message">The message to select</param>
        private void RestoreSelection(CIXMessage message)
        {
            if (_messages.Count == 0)
            {
                ShowEmptyMessage();
            }
            else if (message != null)
            {
                int row = _messages.IndexOf(message);
                if (row == -1 && IsCollapsed(message.Root))
                {
                    ExpandThread(message.Root);
                    row = _messages.IndexOf(message);
                }
                _hasMouseDown = true;
                SelectedRow = row;
            }
        }

        /// <summary>
        /// Get or set the associated folders tree.
        /// </summary>
        private FoldersTree FoldersTree { get; set; }

        /// <summary>
        /// Copy the selected text from the current message to the clipboard.
        /// </summary>
        private void CopySelection()
        {
            CanvasItem item = DisplayedItem;
            if (item != null)
            {
                string selectedText = item.Selection;
                if (!String.IsNullOrEmpty(selectedText))
                {
                    Clipboard.SetDataObject(selectedText);
                }
            }
        }

        /// <summary>
        /// Expand or collapse the current thread.
        /// </summary>
        private void ExpandCollapseThread(CIXMessage message)
        {
            if (message != null)
            {
                if (IsExpanded(message.Root))
                {
                    CollapseThread(message);
                }
                else
                {
                    ExpandThread(message);
                }
            }
        }

        /// <summary>
        /// Returns whether the specified message can be expanded. It must be a root
        /// message with children and we should be sorting by conversation in the current
        /// folder view.
        /// </summary>
        private bool IsExpandable(CIXMessage message)
        {
            return message.Root.HasChildren && _groupByConv;
        }

        /// <summary>
        /// Return whether the current messge is collapsed.
        /// </summary>
        private bool IsCollapsed(CIXMessage message)
        {
            return message.IsRoot && message.HasChildren && _groupByConv && !IsExpanded(message);
        }

        /// <summary>
        /// Return whether the current message is expanded by consulting the next message
        /// in the _messages list to determine if its level is greater than the the root
        /// level (i.e. 0). This is cruder than having a flag on the message itself but
        /// it really isn't very expensive if only checked infrequently.
        /// </summary>
        private bool IsExpanded(CIXMessage message)
        {
            if (message.IsRoot)
            {
                int index = _messages.IndexOf(message);
                int count = _messages.Count;
                if (index >= 0 && index < count - 1)
                {
                    CIXMessage nextMessage = _messages[++index];
                    if (nextMessage.Level > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Collapse a thread from the root by removing all the root's child messages
        /// from the _messages array
        /// array.
        /// </summary>
        private void CollapseThread(CIXMessage message)
        {
            CIXMessage root = message.Root;
            int index = _messages.IndexOf(root);
            int count = _messages.Count;
            if (index >= 0 && index < count - 1 && _currentFolder.ID > 0)
            {
                ++index;
                while (index < _messages.Count && _messages[index].Level > 0)
                {
                    _messages.RemoveAt(index);
                }
            }
            if (count != _messages.Count)
            {
                InitialiseList();
                RefreshList();
                GoToMessage(root.RemoteID);
            }
        }

        /// <summary>
        /// Expand a thread by inserting all child messages. The message must be
        /// a root message.
        /// </summary>
        private void ExpandThread(CIXMessage message)
        {
            if (message.IsRoot)
            {
                int index = _messages.IndexOf(message);
                int count = _messages.Count;
                if (index >= 0 && _currentFolder.ID > 0)
                {
                    TopicFolder topicFolder = (TopicFolder) _currentFolder;
                    _messages.InsertRange(index + 1, topicFolder.Folder.Messages.Children(message));
                }
                if (count != _messages.Count)
                {
                    InitialiseList();
                    RefreshList();
                }
            }
        }

        /// <summary>
        /// Select all text in the text item that has the focus.
        /// </summary>
        private void SelectAll()
        {
            CanvasItem item = DisplayedItem;
            if (item != null)
            {
                item.SelectAll();
            }
        }

        /// <summary>
        /// Keep the progress status panel centered within the thread view.
        /// </summary>
        private void tsvMessages_SizeChanged(object sender, EventArgs args)
        {
            tsvStatusPanel.Left = (tsvMessages.Width - tsvStatusPanel.Width) / 2;
            tsvStatusPanel.Top = (tsvMessages.Height - tsvStatusPanel.Height) / 2;
        }

        /// <summary>
        /// Fill a listview item with an entry for the _message at the given index.
        /// </summary>
        private void tsvMessages_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs args)
        {
            ListViewItem itm = _itemCache[args.ItemIndex];
            if (itm == null)
            {
                itm = new ListViewItem("") { Tag = _messages[args.ItemIndex] };
                _itemCache[args.ItemIndex] = itm;
            }
            args.Item = itm;
        }

        /// <summary>
        /// Return the indent for the specified message.
        /// For Mono, we constrain the indent to half the client width because there's no mechanism to
        /// set the horizontal scroll position of a ListView without P/Invoke.
        /// </summary>
        private int IndentForItem(CIXMessage message)
        {
            int indent = 0;
            if (CanGroupByConversation() && message.Level > 0)
            {
                #if __MonoCS__
                int maximumIndent = tsvMessagePane.ClientRectangle.Width / 2;
                #else
                const int maximumIndent = MaximumIndent;
                #endif
                indent = Math.Min(7 + 14*message.Level, maximumIndent);
            }
            return indent;
        }

        /// <summary>
        /// Compute the display rectangles for the various field elements of the given message
        /// </summary>
        private DrawRectElements ComputeDrawRectElements(Graphics graphics, CIXMessage message, Rectangle bounds)
        {
            DrawRectElements drawRectElements = new DrawRectElements();
            Rectangle drawRect = bounds;

            // Indent for conversations
            int indent = IndentForItem(message);
            drawRect.X += indent;
            drawRect.Width -= indent;
            drawRect.Inflate(-2, -2);

            // The rectangle for the selection and focus
            drawRectElements.BoundaryRect = drawRect;

            Font headerFont = FontForItem(message);
            Rectangle imageRect;

            drawRect.Y = drawRect.Y + (drawRect.Height - headerFont.Height)/2;
            drawRect.X += 4;
            drawRect.Width -= 4;

            // Compute the optional expander rectangle
            if (message.IsRoot && message.HasChildren && CanGroupByConversation())
            {
                Image arrowImage = IsExpanded(message) ? Resources.CollapseArrow : Resources.ExpandArrow;
                imageRect = new Rectangle(drawRect.X, bounds.Y + (bounds.Height - arrowImage.Height) / 2, arrowImage.Width, arrowImage.Height);

                drawRectElements.ExpanderRect = imageRect;

                drawRect.X += arrowImage.Width + 4;
                drawRect.Width -= arrowImage.Width + 4;
            }

            // Compute Read image rectangle
            Image readImage = ReadImageForMessage(message);
            imageRect = new Rectangle(drawRect.X, bounds.Y + (bounds.Height - readImage.Height) / 2, readImage.Width, readImage.Height);

            drawRectElements.ReadRect = imageRect;

            drawRect.X += readImage.Width + 4;
            drawRect.Width -= readImage.Width + 4;

            // Compute author name rectangle
            SizeF idSize = graphics.MeasureString(message.Author, headerFont);

            drawRectElements.AuthorRect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute first separator rectangle
            const string separatorChar = "•";
            idSize = graphics.MeasureString(separatorChar, headerFont);

            drawRectElements.Separator1Rect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute date field rectangle
            string dateString = (_showFullDate)
                ? message.Date.ToString("d MMM yyyy") + " " + message.Date.ToShortTimeString()
                : message.Date.FriendlyString(true);
            idSize = graphics.MeasureString(dateString, headerFont);

            drawRectElements.DateRect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute Star field rectangle
            Image starIcon = message.Starred ? Resources.ActiveStar : Resources.InactiveStar;
            imageRect = new Rectangle(drawRect.X, bounds.Y + (bounds.Height - starIcon.Height) / 2,
                starIcon.Width, starIcon.Height);

            drawRectElements.StarRect = imageRect;

            drawRect.X += starIcon.Width + 4;
            drawRect.Width -= starIcon.Width + 4;

            // Compute ID field rectangle
            string idString = message.IsPseudo
                ? "Draft"
                : message.RemoteID.ToString(CultureInfo.InvariantCulture);
            idSize = graphics.MeasureString(idString, headerFont);

            drawRectElements.IDRect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute second separator rectangle
            idSize = graphics.MeasureString(separatorChar, headerFont);
            drawRectElements.Separator2Rect = drawRect;

            // Compute subject line rectangle
            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 8;

            drawRectElements.SubjectRect = drawRect;

            return drawRectElements;
        }

        /// <summary>
        /// Draw the row.
        /// </summary>
        private void tsvMessages_DrawSubItem(object sender, DrawListViewSubItemEventArgs args)
        {
            CIXMessage message = args.Item.Tag as CIXMessage;
            if (message != null)
            {
                DrawRectElements elements = ComputeDrawRectElements(args.Graphics, message, args.Bounds);
                     
                bool canThread = CanGroupByConversation();

                Color textColor = UI.Forums.HeaderFooterColour;
                if (args.Item.Selected)
                {
                    args.DrawFocusRectangle(elements.BoundaryRect);
                    textColor = UI.Forums.SelectionTextColour;
                }

                // Draw the selection around the control.
                using (Pen edgePen = new Pen(args.Item.Selected ? UI.Forums.SelectionColour : BackColor))
                {
                    Color fillColour = args.Item.Selected ? UI.Forums.SelectionColour : (message.IsRoot ? UI.Forums.RootColour : UI.Forums.CommentColour);
                    using (Brush backBrush = new SolidBrush(fillColour))
                    {
                        args.Graphics.FillRoundedRectangle(edgePen, backBrush, elements.BoundaryRect);
                    }
                }

                Font headerFont = FontForItem(message);
                using (Brush textBrush = new SolidBrush(message.Ignored ? UI.Forums.IgnoredColour : textColor))
                {
                    using (Brush priorityBrush = new SolidBrush(message.Ignored ? UI.Forums.IgnoredColour : UI.Forums.PriorityColour))
                    {
                        // Draw arrows if message can be expanded or collapsed
                        if (message.IsRoot && message.HasChildren && canThread)
                        {
                            Image arrowImage = IsExpanded(message) ? Resources.CollapseArrow : Resources.ExpandArrow;
                            args.Graphics.DrawImage(arrowImage, elements.ExpanderRect);
                        }

                        // Draw read image
                        Image readImage = ReadImageForMessage(message);
                        args.Graphics.DrawImage(readImage, elements.ReadRect);

                        // Draw author name
                        args.Graphics.DrawString(message.Author, headerFont, message.Priority ? priorityBrush : textBrush, elements.AuthorRect);

                        // Draw separator
                        const string separatorChar = "•";
                        args.Graphics.DrawString(separatorChar, headerFont, textBrush, elements.Separator1Rect);

                        // Draw date
                        string dateString = (_showFullDate)
                            ? message.Date.ToString("d MMM yyyy") + " " + message.Date.ToShortTimeString()
                            : message.Date.FriendlyString(true);
                        args.Graphics.DrawString(dateString, headerFont, textBrush, elements.DateRect);

                        // Draw star icon
                        Image starIcon = message.Starred ? Resources.ActiveStar : Resources.InactiveStar;
                        args.Graphics.DrawImage(starIcon, elements.StarRect);

                        // Draw ID field
                        string idString = message.IsPseudo
                            ? "Draft"
                            : message.RemoteID.ToString(CultureInfo.InvariantCulture);
                        args.Graphics.DrawString(idString, headerFont, textBrush, elements.IDRect);

                        // Another separator
                        args.Graphics.DrawString(separatorChar, headerFont, textBrush, elements.Separator2Rect);

                        // Draw subject line
                        string subjectLine = message.Subject;
                        args.Graphics.DrawString(subjectLine, headerFont, textBrush, elements.SubjectRect, new StringFormat(StringFormatFlags.NoWrap));
                    }
                }
            }
        }


        /// <summary>
        /// Return the appropriate read icon for this message based on its unread and priority state
        /// </summary>
        private static Image ReadImageForMessage(CIXMessage message)
        {
            return (message.ReadLocked) ? Resources.ReadLock :
                   (!message.Unread) ? Resources.ReadMessage :
                   (message.Priority) ? Resources.UnreadPriorityMessage :
                                          Resources.UnreadMessage;
        }

        /// <summary>
        /// Start the progress spinner only if the root list is empty.
        /// </summary>
        private void StartProgressBar()
        {
            if (_messages.Count == 0)
            {
                tsvStatusPanel.Visible = true;
                tsvProgress.Start();
                tsvStatusPanel.Update();
            }
            FoldersTree.MainForm.StartStatusProgressSpinner(Resources.StatusTopicRefresh);
        }

        /// <summary>
        /// Stop the progress spinner.
        /// </summary>
        private void StopProgressBar()
        {
            tsvProgress.Stop();
            tsvStatusPanel.Visible = false;
            FoldersTree.MainForm.StopStatusProgressSpinner();
        }

        /// <summary>
        /// Show or hide the message pane at the bottom of the window.
        /// </summary>
        private void ShowMessagePane()
        {
            int messagePaneHeight = tsvSplitview.Height;
            float splitPositionPercent = Preferences.StandardPreferences.ThreadSplitPosition;
            if (splitPositionPercent <= 0 || splitPositionPercent > 100)
            {
                splitPositionPercent = 50.0f;
            }
            tsvThreadPane.Parent = tsvSplitview.Panel1;
            tsvSplitview.Visible = true;
            tsvSplitview.SplitterDistance = (int)(messagePaneHeight / (100.0 / splitPositionPercent));
            tsvThreadPane.Size = new Size(tsvSplitview.Panel1.Width, tsvSplitview.Panel1.Height);
        }

        /// <summary>
        /// Save splitter bar percent position if it is moved.
        /// </summary>
        private void tsvSplitview_SplitterMoved(object sender, SplitterEventArgs args)
        {
            if (!_initialising)
            {
                int messagePaneHeight = tsvSplitview.Height;
                Preferences.StandardPreferences.ThreadSplitPosition = (float)((100.0 / messagePaneHeight) * tsvSplitview.SplitterDistance);
            }
            ResizeMessagePanel();
        }

        /// <summary>
        /// When split view is resized, make sure thread pane fills the 1st panel area.
        /// </summary>
        private void tsvSplitview_SizeChanged(object sender, EventArgs args)
        {
            ResizeMessagePanel();
        }

        /// <summary>
        /// Resize the thread pane to fill the 1st splitview panel area.
        /// There seems to be some issues with autolayout resizing so this needs to be done manually.
        /// </summary>
        private void ResizeMessagePanel()
        {
            tsvMessages.Height = tsvSplitview.Panel1.Height;
            tsvMessages.Width = tsvSplitview.Panel1.Width;
            tsvMessages.Columns[0].Width = MaximumIndent;
        }

        /// <summary>
        /// Display a blank message pane and a button to create a new thread
        /// </summary>
        private void ShowEmptyMessage()
        {
            ShowMessage(null);
        }

        /// <summary>
        /// Show the currently selected message.
        /// </summary>
        private void ShowMessage(CIXMessage cixMessage)
        {
            tsvMessagePane.BeginUpdate();
            tsvMessagePane.Items.Clear();
            if (cixMessage != null && (!cixMessage.Ignored || _showIgnored))
            {
                MessageItem messageItem = MessageItemFromMessage(cixMessage, false);
                messageItem.Level = 0;
                messageItem.Full = true;
                messageItem.IsExpandable = true;
                messageItem.ShowFolder = !_isTopicFolder || _isFiltering;
                messageItem.Image = Mugshot.MugshotForUser(cixMessage.Author, true).RealImage;
                messageItem.Font = UI.Forums.MessageFont;
                messageItem.ShowTooltips = true;
                messageItem.ItemColour = tsvMessagePane.BackColor;
                messageItem.ForeColor = BodyColourForItem(cixMessage);
                if (tsvMessagePane.Items.Count == 1)
                {
                    return;
                }
                tsvMessagePane.Items.Insert(0, messageItem);
            }
            tsvMessagePane.AutoScrollPosition = new Point(0, 0);
            tsvMessagePane.EndUpdate(null);
            if (cixMessage != null)
            {
                _currentFolder.RecentMessage = cixMessage.RemoteID;
            }
        }

        /// <summary>
        /// Construct a MessageItem for the thread pane from the given cixMessage
        /// </summary>
        /// <param name="cixMessage">A CIX message</param>
        /// <param name="isMinimal">Whether the message is rendered in minimal format</param>
        /// <returns>A fully initialised MessageItem</returns>
        private MessageItem MessageItemFromMessage(CIXMessage cixMessage, bool isMinimal)
        {
            return new MessageItem(tsvMessagePane)
            {
                ID = cixMessage.RemoteID,
                Minimal = isMinimal,
                Highlight = _currentFilterString,
                FullDateString = _showFullDate,
                ParentID = cixMessage.CommentID,
                Level = cixMessage.Level,
                Message = cixMessage,
                ForeColor = BodyColourForItem(cixMessage),
                Font = FontForItem(cixMessage)
            };
        }

        /// <summary>
        /// Return the specific sort order for this folder. Smart folders override conversation
        /// view to enforce date view instead.
        /// </summary>
        private CRSortOrder.SortOrder OrderForFolder()
        {
            CRSortOrder.SortOrder order = SortOrderBase.Order;
            if (_groupByConv)
            {
                if (_currentFolder.ID <= 0 || _isFiltering)
                {
                    return CRSortOrder.SortOrder.Date;
                }
            }
            return order;
        }

        /// <summary>
        /// Prompt for and go to the specified message in the current topic.
        /// </summary>
        private void GoToMessageDialog()
        {
            GoToMessage gotoDialog = new GoToMessage();
            if (gotoDialog.ShowDialog() == DialogResult.OK)
            {
                if (!GoToMessage(gotoDialog.MessageNumber))
                {
                    string message = string.Format(Resources.GotoError, gotoDialog.MessageNumber);
                    MessageBox.Show(message, Resources.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// This event is triggered when the user performs an action on a thread item.
        /// </summary>
        /// <param name="sender">The thread view control</param>
        /// <param name="args">A CanvasItemArgs object that contains details of the action</param>
        private void OnItemAction(object sender, CanvasItemArgs args)
        {
            MessageItem messageItem = (MessageItem)args.Control;
            Action(args.Item, messageItem.Message);
        }

        /// <summary>
        /// Respond to links being clicked by sending them to the mainform to be processed.
        /// </summary>
        private void OnLinkClicked(object sender, LinkClickedEventArgs args)
        {
            if (args.LinkText.StartsWith("cix", StringComparison.OrdinalIgnoreCase))
            {
                FoldersTree.MainForm.Address = args.LinkText;
            }
            else
            {
                FoldersTree.MainForm.LaunchURL(args.LinkText);
            }
        }

        /// <summary>
        /// Handle double-click on an item.
        /// </summary>
        private void tsvMessages_DoubleClick(object sender, EventArgs args)
        {
            int row = SelectedRow;
            if (row >= 0 && CanAction(ActionID.Edit))
            {
                Action(ActionID.Edit, _messages[row]);
            }
        }

        /// <summary>
        /// Action the specified ID with the given message item.
        /// </summary>
        /// <param name="id">An action ID</param>
        /// <param name="message">A selected message, or null</param>
        private void Action(ActionID id, CIXMessage message)
        {
            switch (id)
            {
                case ActionID.Chat:
                    Chat(message);
                    break;

                case ActionID.Block:
                    if (message != null)
                    {
                        string titleString = string.Format(Resources.BlockTitle, message.Author);
                        string promptString = string.Format(Resources.BlockPrompt, message.Author);
                        if (MessageBox.Show(promptString, titleString, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CIX.RuleCollection.Block(message.Author);
                            OnMessageChanged(message);
                        }
                    }
                    break;

                case ActionID.Participants:
                    if (_currentFolder.ID > 0)
                    {
                        TopicFolder topic = (TopicFolder) _currentFolder;
                        Folder forum = topic.Folder.ParentFolder;
                        Participants parDialog = new Participants(FoldersTree.MainForm, forum.Name);
                        parDialog.ShowDialog();
                    }
                    break;

                case ActionID.ManageForum:
                    if (_currentFolder.ID > 0)
                    {
                        TopicFolder topicFolder = (TopicFolder) _currentFolder;
                        Folder forumFolder = topicFolder.Folder.ParentFolder;
                        DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                        FoldersTree.ManageForum(forum);
                    }
                    break;

                case ActionID.Email:
                    Email(message);
                    break;

                case ActionID.Profile:
                case ActionID.AuthorImage:
                    FoldersTree.MainForm.Address = string.Format("cixuser:{0}", message.Author);
                    break;

                case ActionID.Priority:
                    PriorityThread(message);
                    break;

                case ActionID.Ignore:
                    IgnoreThread(message);
                    break;

                case ActionID.ReadLock:
                    ToggleReadLock(message);
                    break;

                case ActionID.Quote:
                    if (message != null)
                    {
                        Comment(message, message.Body.Quoted());
                    }
                    break;

                case ActionID.Edit:
                case ActionID.Reply:
                    if (message != null)
                    {
                        if (message.Topic.Flags.HasFlag(FolderFlags.OwnerCommentsOnly) && !message.IsMine)
                        {
                            Action(ActionID.ReplyByMail);
                        }
                        else
                        {
                            Comment(message, null);
                        }
                    }
                    break;

                case ActionID.ReplyByMail:
                    {
                        InboxMessageEditor newMessageWnd = new InboxMessageEditor(message);
                        newMessageWnd.Show();
                    }
                    break;

                case ActionID.Print:
                    Print(message);
                    break;

                case ActionID.Read:
                    ToggleRead(message);
                    break;

                case ActionID.Star:
                    ToggleStar(message);
                    break;

                case ActionID.Withdraw:
                    WithdrawMessage(message);
                    break;

                case ActionID.Delete:
                    DeleteMessage(message);
                    break;

                case ActionID.NextUnread:
                    GoToNextUnread(message);
                    break;

                case ActionID.NextPriorityUnread:
                    GoToNextPriorityUnread(message);
                    break;

                case ActionID.GoToSource:
                    GoToSourceThread(message);
                    break;

                case ActionID.Link:
                    CopyLinkToClipboard(message);
                    break;

                case ActionID.PageMessage:
                    if (tsvMessagePane.Items.Count > 0)
                    {
                        MessageItem messageItem = tsvMessagePane.Items[0] as MessageItem;
                        if (messageItem != null)
                        {
                            if (ScrollMessageUp())
                            {
                                break;
                            }
                            if (messageItem.Message.Unread)
                            {
                                MarkAsRead(messageItem.Message);
                            }
                            FoldersTree.NextUnread(FolderOptions.NextUnread);

                            // Put focus back on thread
                            ActiveControl = tsvMessages;
                        }
                    }
                    break;

                case ActionID.GoTo:
                    if (CanAction(ActionID.GoTo))
                    {
                        GoToMessageDialog();
                    }
                    break;

                case ActionID.MarkThreadRead:
                    MarkThreadRead(message);
                    FoldersTree.NextUnread(FolderOptions.NextUnread);
                    break;

                case ActionID.MarkThreadReadThenRoot:
                    MarkThreadRead(message);
                    FoldersTree.NextUnread(FolderOptions.NextUnread | FolderOptions.Root);
                    break;

                case ActionID.MarkTopicRead:
                    MarkTopicRead();
                    FoldersTree.NextUnread(FolderOptions.NextUnread);
                    break;

                case ActionID.NextRoot:
                    SelectRoot(message, RootToSelect.NextRoot);
                    break;

                case ActionID.Root:
                    SelectRoot(message, RootToSelect.PreviousRoot);
                    break;

                case ActionID.Original:
                    SelectOriginal(message);
                    break;

                case ActionID.NewMessage:
                    NewMessage(string.Empty);
                    break;

                case ActionID.SelectAll:
                    SelectAll();
                    break;

                case ActionID.Expand:
                    ExpandCollapseThread(message);
                    break;

                case ActionID.Copy:
                    CopySelection();
                    break;
            }
        }

        /// <summary>
        /// Add a new comment to the specified item.
        /// </summary>
        private void Comment(CIXMessage message, string quotedText)
        {
            if (message == null)
            {
                return;
            }
            if (message.IsDraft)
            {
                EditMessage(message, false);
            }
            else
            {
                Folder folder = message.Topic;
                Folder forumFolder = folder.ParentFolder;
                DirForum forum = CIX.DirectoryCollection.ForumByName(forumFolder.Name);
                bool isModerator = forum != null && forum.IsModerator;

                // Can't post to read-only folders
                if (folder.IsReadOnly && !isModerator)
                {
                    return;
                }

                // Can't post if the folder is OwnerPostOnly and we weren't the author
                // of the parent message
                if (folder.Flags.HasFlag(FolderFlags.OwnerCommentsOnly) && !message.IsMine)
                {
                    return;
                }

                CIXMessage newMessage = new CIXMessage
                {
                    Author = CIX.Username,
                    RemoteID = 0,
                    Priority = true,
                    Body = quotedText ?? GetQuotedText(DisplayedItem),
                    Date = DateTime.Now,
                    TopicID = message.TopicID,
                    RootID = message.IsRoot ? message.RemoteID : message.RootID,
                    CommentID = message.RemoteID
                };

                // If we comment on an unread message, we assume it is read
                // at this point.
                MarkAsRead(message);

                EditMessage(newMessage, true);
            }
        }

        /// <summary>
        /// Print the specified message.
        /// </summary>
        /// <param name="message">Message to print</param>
        private void Print(CIXMessage message)
        {
            if (message != null)
            {
                PrintDocument printDoc = new PrintDocument
                {
                    PrinterSettings = FoldersTree.MainForm.PrintDocument.PrinterSettings,
                    DefaultPageSettings = FoldersTree.MainForm.PrintDocument.DefaultPageSettings
                };
                Font printFont = new Font("Arial", 10);

                PrintDialog pdi = new PrintDialog
                {
                    Document = printDoc,
                    UseEXDialog = true
                };
                if (pdi.ShowDialog() == DialogResult.OK)
                {
                    string[] lines = message.Body.Split(new[] { '\n' });
                    int lineIndex = 0;
                    try
                    {
                        printDoc.PrintPage += (sender, ev) =>
                        {
                            float leftMargin = ev.MarginBounds.Left;
                            float topMargin = ev.MarginBounds.Top;

                            // Print each line of the file.
                            float yPos = topMargin;
                            while (lineIndex < lines.Count())
                            {
                                string line = lines[lineIndex];
                                SizeF sf = ev.Graphics.MeasureString(line, printFont, ev.MarginBounds.Width);
                                if (yPos + sf.Height > ev.MarginBounds.Bottom)
                                {
                                    break;
                                }
                                using (Brush textBrush = new SolidBrush(SystemColors.ControlText))
                                {
                                    ev.Graphics.DrawString(line, printFont, textBrush,
                                        new RectangleF(new PointF(leftMargin, yPos), sf), new StringFormat());
                                }
                                yPos += (sf.Height > 0) ? sf.Height : printFont.GetHeight(ev.Graphics);
                                ++lineIndex;
                            }

                            // If more lines exist, print another page. 
                            ev.HasMorePages = lineIndex < lines.Count();
                        };
                        printDoc.Print();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(string.Format(Resources.PrintError, e.Message), Resources.Error, MessageBoxButtons.OK);
                    }
                }
            }
        }

        /// <summary>
        /// Invoke the message editor on the specified message
        /// </summary>
        /// <param name="message">The message to edit</param>
        /// <param name="addSignature">True if signature is to be added</param>
        private static void EditMessage(CIXMessage message, bool addSignature)
        {
            CIXMessageEditor editor = MessageEditorCollection.Get(message) ?? new CIXMessageEditor(message, addSignature);
            editor.Show();
            editor.BringToFront();
        }

        /// <summary>
        /// If the item has selected text, extract that and use it as quotation
        /// in the message body.
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>Quoted text or an empty string</returns>
        private static string GetQuotedText(CanvasItem item)
        {
            if (item != null)
            {
                string selectedText = item.Selection;
                if (!string.IsNullOrWhiteSpace(selectedText))
                {
                    return selectedText.Quoted();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Start a chat session with the author of the specified message using the
        /// XMPP protocol to initiate the client.
        /// </summary>
        private void Chat(CIXMessage item)
        {
            if (item != null)
            {
                string chatUrl = String.Format(Resources.XMPPLink, item.Author);
                FoldersTree.MainForm.Address = chatUrl;
            }
        }

        /// <summary>
        /// E-mail the author of the selected item.
        /// </summary>
        private void Email(CIXMessage item)
        {
            if (item != null)
            {
                Profile profile = Profile.ProfileForUser(item.Author);
                if (profile != null && !String.IsNullOrEmpty(profile.EMailAddress))
                {
                    FoldersTree.MainForm.Address = String.Format("mailto:{0}", profile.EMailAddress);
                }
                else
                {
                    FoldersTree.MainForm.Address = String.Format("cixmail:{0}", item.Author);
                }
            }
        }

        /// <summary>
        /// Go to the next unread message.
        /// </summary>
        private void GoToNextUnread(CIXMessage message)
        {
            if (message != null && message.Unread)
            {
                MarkAsRead(message);
            }
            FoldersTree.NextUnread(FolderOptions.NextUnread);
        }

        /// <summary>
        /// Go to the next priority unread message.
        /// </summary>
        private void GoToNextPriorityUnread(CIXMessage message)
        {
            if (message != null && message.Unread)
            {
                MarkAsRead(message);
            }
            FoldersTree.NextUnread(FolderOptions.NextUnread|FolderOptions.Priority);
        }

        /// <summary>
        /// Mark the message referenced by the specified thread item as read.
        /// </summary>
        private static void MarkAsRead(CIXMessage message)
        {
            if (message != null && message.Unread && !message.ReadLocked)
            {
                message.MarkRead();
            }
        }

        /// <summary>
        /// Toggle the read or unread state of the message referenced by the specified thread item.
        /// If the item is a root message and is collapsed then mark the whole thread instead.
        /// </summary>
        private void ToggleRead(CIXMessage message)
        {
            if (message != null && !message.ReadLocked)
            {
                if (IsCollapsed(message))
                {
                    if (message.Unread)
                    {
                        message.MarkThreadRead();
                    }
                    else
                    {
                        message.MarkThreadUnread();
                    }
                }
                else
                {
                    if (message.Unread)
                    {
                        message.MarkRead();
                    }
                    else
                    {
                        message.MarkUnread();
                    }
                }
            }
        }

        /// <summary>
        /// Add or remove a star from the specified message.
        /// </summary>
        private static void ToggleStar(CIXMessage item)
        {
            if (item != null)
            {
                if (item.Starred)
                {
                    item.RemoveStar();
                }
                else
                {
                    item.AddStar();
                }
            }
        }

        /// <summary>
        /// Toggle read-lock state of a message. Also force the message unread
        /// if not already.
        /// </summary>
        private static void ToggleReadLock(CIXMessage item)
        {
            if (item != null)
            {
                if (item.ReadLocked)
                {
                    item.ClearReadLock();
                }
                else
                {
                    item.MarkReadLock();
                }
            }
        }

        /// <summary>
        /// Withdraw the selected message on the server and replace it with a withdrawn message.
        /// </summary>
        /// <param name="message">The message to be withdrawn</param>
        private static void WithdrawMessage(CIXMessage message)
        {
            if (message != null)
            {
                if (message.IsPseudo)
                {
                    DeleteMessage(message);
                    return;
                }
                if (MessageBox.Show(Resources.ConfirmWithdraw, Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    message.Withdraw();
                }
            }
        }

        /// <summary>
        /// Toggle ignore the current message and all subsequent replies.
        /// </summary>
        /// <param name="item">The root of the thread to ignore</param>
        private static void IgnoreThread(CIXMessage item)
        {
            if (item != null)
            {
                if (item.Ignored)
                {
                    item.RemoveIgnore();
                }
                else
                {
                    item.SetIgnore();
                }
            }
        }

        /// <summary>
        /// Toggle as priority the current message and all subsequent replies.
        /// </summary>
        /// <param name="item">The root of the thread to toggle priority</param>
        private static void PriorityThread(CIXMessage item)
        {
            if (item != null)
            {
                if (item.Priority)
                {
                    item.ClearPriority();
                }
                else
                {
                    item.SetPriority();
                }
            }
        }

        /// <summary>
        /// Delete the specified message from the database
        /// </summary>
        /// <param name="message">The message to be withdrawn</param>
        private static void DeleteMessage(CIXMessage message)
        {
            if (message != null && message.IsPseudo)
            {
                Folder topic = message.Topic;
                topic.Messages.Delete(message);
            }
        }

        /// <summary>
        /// Navigate to the thread in the original forum in which this message appears.
        /// </summary>
        private void GoToSourceThread(CIXMessage message)
        {
            if (message != null)
            {
                Folder topic = message.Topic;
                Folder forum = topic.ParentFolder;
                FoldersTree.MainForm.Address = String.Format("cix:{0}/{1}:{2}", forum.Name, topic.Name, message.RemoteID);
            }
        }

        /// <summary>
        /// Copy a link to the message to the clipboard.
        /// </summary>
        private void CopyLinkToClipboard(CIXMessage message)
        {
            if (message != null)
            {
                Folder topic = message.Topic;
                Folder forum = topic.ParentFolder;
                Clipboard.SetDataObject(String.Format("cix:{0}/{1}:{2}", forum.Name, topic.Name, message.RemoteID));

                FoldersTree.ShowInfoBar(Resources.CopyLinkInfoMessage);
            }
        }

        /// <summary>
        /// Handle the main topic view being resized.
        /// </summary>
        private void TopicView_Resize(object sender, EventArgs args)
        {
            ResizeMessagePanel();
        }
    }
}