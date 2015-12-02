// *****************************************************
// CIXReader
// FoldersTree.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 31/08/2013 9:08 AM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Tables;
using CIXReader.Controls;
using CIXReader.Properties;
using CIXReader.SpecialFolders;
using CIXReader.SubViews;
using CIXReader.UIConfig;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// FoldersTree defines the folder tree view.
    /// </summary>
    public sealed partial class FoldersTree : Form
    {
        private TreeNode _selectedNode;
        private TreeNode _nodeAtDragPoint;
        private TreeNode _nodeBeingDragged;
        private TreeNode _contextMenuNode;

        private TreeNode _forumsTree;
        private TreeNode _homePage;
        private TreeNode _messagesTree;
        private TreeNode _directoryTree;

        private readonly Dictionary<AppView, ViewBaseView> _allViews;
        private ViewBaseView _currentView;

        private bool _blockKeyDown;
        private Font _topicNameFont;
        private Font _boldTopicNameFont;
        private Font _unreadFont;
        private bool _initialising;

        private CRPanel _searchBar;
        private bool _searchBarVisible;
        private readonly Timer _searchBarTimer;
        private readonly System.Timers.Timer _smartFolderRefresh;
        private int _searchBarIncrement;
        private int _searchBarIteration;
        private int _searchBarHeight;
        private string _currentSearchText;
        private Address _lastAddress;
        private bool _isRBut;
        private List<TreeNode> _allSmartFolders;

        // Search button names.
        private const string _allForumsSearchButton = "allForumsSearchButton";
        private const string _currentForumSearchButton = "currentForumSearchButton";
        private const string _currentTopicSearchButton = "currentTopicSearchButton";

        private enum SearchBarVisibility { Show, FastHide };

        /// <summary>
        /// Constructs a FoldersTree instance.
        /// </summary>
        public FoldersTree(MainForm mainForm)
        {
            InitializeComponent();

            MainForm = mainForm;

            _initialising = true;
            _searchBarTimer = new Timer
            {
                Enabled = false,
                Interval = 5
            };
            _searchBarTimer.Tick += SearchBarTimerOnTick;

            _smartFolderRefresh = new System.Timers.Timer(500);
            _smartFolderRefresh.Elapsed += SmartFolderRefresh;
            _smartFolderRefresh.AutoReset = false;

            _allViews = new Dictionary<AppView, ViewBaseView>();
            _allViews[AppView.AppViewWelcome] = new WelcomeView(this);
            _allViews[AppView.AppViewMail] = new InboxView(this);
            _allViews[AppView.AppViewTopic] = new TopicView(this);
            _allViews[AppView.AppViewDirectory] = new DirectoryView(this);
            _allViews[AppView.AppViewForum] = new ForumsView(this);
        }

        /// <summary>
        /// Override to provide the sort menu for this view
        /// </summary>
        public ContextMenuStrip SortMenu
        {
            get { return _currentView.SortMenu; }
        }

        /// <summary>
        /// Return the sort ordering for the view being
        /// displayed.
        /// </summary>
        public CRSortOrder.SortOrder SortOrder
        {
            get { return _currentView.SortOrderBase.Order; }
            set { _currentView.SortOrderBase.Order = value; }
        }

        /// <summary>
        /// Return the sort direction for the view
        /// being displayed.
        /// </summary>
        public bool Ascending
        {
            get { return _currentView.SortOrderBase.Ascending; }
            set { _currentView.SortOrderBase.Ascending = value; }
        }

        /// <summary>
        /// Scripting: Expose the sort ordering as an integer.
        /// </summary>
        // ReSharper disable UnusedMember.Global
        public int Order
        // ReSharper restore UnusedMember.Global
        {
            get { return (int)_currentView.SortOrderBase.Order; }
            set { _currentView.SortOrderBase.Order = (CRSortOrder.SortOrder)value; }
        }

        /// <summary>
        /// Return the address of the selected item in the current view.
        /// </summary>
        public string Address
        {
            get { return _currentView != null ? _currentView.Address : string.Empty; }
        }

        /// <summary>
        /// Return the address of the selected item in the specified view.
        /// </summary>
        public Address AddressFromView(AppView view)
        {
            return new Address(_allViews[view].Address);
        }

        /// <summary>
        /// Set the specified address by navigating to the folder referenced by
        /// the address.
        /// </summary>
        /// <param name="address">The address to be set</param>
        public void SetAddress(Address address)
        {
            if (SelectAddress(address))
            {
                return;
            }
            if (JoinMissingForum(address))
            {
                return;
            }
            Process.Start(address.SchemeAndQuery);
        }

        /// <summary>
        /// Get or set the main form to which this view belongs.
        /// </summary>
        public MainForm MainForm { get; set; }

        /// <summary>
        /// Return the selected tree node
        /// </summary>
        public TreeNode SelectedNode
        {
            get { return frmList.SelectedNode; }
        }

        /// <summary>
        /// Scripting: Return the currently selected CIXMessage or null.
        /// </summary>
        // ReSharper disable UnusedMember.Global
        public CIXMessage CurrentMessage
        // ReSharper enable UnusedMember.Global
        {
            get { return (_currentView != null && _currentView.DisplayedItem != null) ? _currentView.DisplayedItem.Message : null; }
        }

        /// <summary>
        /// Scripting: Actions the specified action ID. Note that this is called
        /// from a script and thus the action type must be an integer.
        /// </summary>
        public void Action(int id)
        {
            Action((ActionID) id);
        }

        /// <summary>
        /// Actions the specified action ID.
        /// </summary>
        public void Action(ActionID id)
        {
            FolderBase folderBase = (FolderBase)(_contextMenuNode ?? _selectedNode).Tag;
            switch (id)
            {
                case ActionID.Refresh:
                {
                    if (folderBase.ID > 0)
                    {
                        TopicFolder topicFolder = (TopicFolder)folderBase;
                        Folder folder = topicFolder.Folder;
                        if (folder.IsRootFolder)
                        {
                            DirForum thisForum = CIX.DirectoryCollection.ForumByName(folder.Name);
                            thisForum.Refresh();
                        }
                    }
                    folderBase.Refresh();
                    _contextMenuNode = null;
                    return;
                }

                case ActionID.ManageForum:
                {
                    if (folderBase.ID > 0)
                    {
                        TopicFolder topicFolder = (TopicFolder) folderBase;
                        Folder folder = topicFolder.Folder;
                        if (!folder.IsRootFolder)
                        {
                            folder = folder.ParentFolder;
                        }
                        DirForum thisForum = CIX.DirectoryCollection.ForumByName(folder.Name);
                        ManageForum(thisForum);
                    }
                    _contextMenuNode = null;
                    return;
                }

                case ActionID.Participants:
                {
                    if (folderBase.ID > 0)
                    {
                        TopicFolder topicFolder = (TopicFolder) folderBase;
                        Folder folder = topicFolder.Folder;
                        if (!folder.IsRootFolder)
                        {
                            folder = folder.ParentFolder;
                        }
                        DisplayParticipants(folder.Name);
                    }
                    _contextMenuNode = null;
                    return;
                }

                case ActionID.Delete:
                {
                    if (folderBase.ID > 0)
                    {
                        TopicFolder folder = (TopicFolder)folderBase;
                        string promptString = string.Format(Resources.ConfirmDelete, folder.Name);
                        if (MessageBox.Show(promptString, Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            folder.Folder.Delete();
                        }
                    }
                    _contextMenuNode = null;
                    return;
                }

                case ActionID.ResignForum:
                {
                    if (folderBase.ID > 0)
                    {
                        TopicFolder folder = (TopicFolder)folderBase;
                        string promptString = String.Format(Resources.ConfirmResign, folder.Name);
                        if (MessageBox.Show(promptString, Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            folder.Folder.Resign();
                        }
                    }
                    _contextMenuNode = null;
                    return;
                }

                case ActionID.MarkTopicRead:
                {
                    if (folderBase.ID > 0)
                    {
                        TopicFolder folder = (TopicFolder)folderBase;
                        HandleMarkFolderRead(folder.Folder);
                    }
                    _contextMenuNode = null;
                    return;
                }
            }
            if (_currentView != null)
            {
                _currentView.Action(id);
            }
        }

        /// <summary>
        /// Return the title for the specified action.
        /// </summary>
        public string TitleForAction(ActionID id)
        {
            return _currentView.TitleForAction(id);
        }

        /// <summary>
        /// Return whether the specified action ID can be handled.
        /// </summary>
        internal bool CanAction(ActionID id)
        {
            TreeNode node = _contextMenuNode ?? _selectedNode;
            FolderBase folderBase = node != null ? (FolderBase)node.Tag : null;
            switch (id)
            {
                case ActionID.ManageForum:
                {
                    DirForum thisForum = null;
                    if (folderBase != null && folderBase.ID > 0)
                    {
                        TopicFolder topicFolder = (TopicFolder) folderBase;
                        Folder folder = topicFolder.Folder;
                        if (!folder.IsRootFolder)
                        {
                            folder = folder.ParentFolder;
                        }
                        thisForum = CIX.DirectoryCollection.ForumByName(folder.Name);
                    }
                    return thisForum != null && thisForum.IsModerator;
                }

                case ActionID.ResignForum:
                {
                    Folder folder = folderBase != null && folderBase.ID > 0 ? ((TopicFolder)folderBase).Folder : null;
                    return folder != null && folder.CanResign && !folder.IsResigned;
                }

                case ActionID.Delete:
                case ActionID.Participants:
                case ActionID.MarkTopicRead:
                {
                    return folderBase != null && folderBase.ID > 0;
                }
            }
            return (_currentView != null) && _currentView.CanAction(id);
        }

        /// <summary>
        /// Locate and display the next folder, after the selected one, that contains
        /// unread messages. Then select the first message in that folder that is unread.
        ///
        /// Options are a combination of:
        /// FolderOption.Priority - only display the next unread priority message
        /// FolderOption.Reset - when displaying the next unread in a folder, start scan
        ///                      from the top of the list rather than from the current
        ///                      selection in that folder.
        /// 
        /// </summary>
        /// <param name="options">Flags that control the next unread behaviour</param>
        public void NextUnread(FolderOptions options)
        {
            if (frmList.Nodes.Count == 0)
            {
                return;
            }
            TreeNode currentNode = frmList.SelectedNode ?? frmList.Nodes[0];
            TreeNode startingNode = currentNode;
            bool resetLoop = false;

            bool acceptSmartFolders = currentNode.Tag is SmartFolder;
            bool priorityOnly = options.HasFlag(FolderOptions.Priority);

            do
            {
                FolderBase folder = (FolderBase)currentNode.Tag;
                if (folder.ViewForFolder == AppView.AppViewForum)
                {
                    TopicFolder topicFolder = (TopicFolder)folder;
                    if (priorityOnly ? topicFolder.UnreadPriority > 0 : topicFolder.Unread > 0)
                    {
                        frmList.BeginUpdate();
                        if (startingNode.Tag is TopicFolder)
                        {
                            TreeNode parentNode = startingNode.Parent;
                            if (parentNode != null && parentNode.Parent != null)
                            {
                                parentNode.Collapse();
                            }
                        }
                        currentNode.ExpandAll();
                        frmList.EndUpdate();
                    }
                }
                else if (folder is MailGroup && !priorityOnly)
                {
                    MailGroup group = (MailGroup)folder;
                    if (group.Unread > 0)
                    {
                        currentNode.Expand();
                    }
                }
                else if (folder is ForumGroup)
                {
                    ForumGroup group = (ForumGroup)folder;
                    if (group.Unread > 0)
                    {
                        currentNode.Expand();
                    }
                }
                else
                {
                    if ((acceptSmartFolders && folder is SmartFolder) || (priorityOnly ? folder.UnreadPriority > 0 : folder.Unread > 0))
                    {
                        if (SelectViewForFolder(currentNode, null, options))
                        {
                            MoveSelection(currentNode);
                            return;
                        }
                    }
                }

                if (currentNode.Nodes.Count > 0)
                {
                    currentNode = currentNode.Nodes[0];
                }
                else if (currentNode.NextNode != null)
                {
                    currentNode = currentNode.NextNode;
                }
                else if (currentNode.Parent != null)
                {
                    currentNode = currentNode.Parent;
                    currentNode = currentNode.NextNode;
                    if (currentNode == null)
                    {
                        if (resetLoop)
                        {
                            break;
                        }
                        currentNode = frmList.Nodes[0];
                        resetLoop = true;
                    }
                }
                else
                {
                    break;
                }
                acceptSmartFolders = false;
                options |= FolderOptions.Reset;
            } while (true);
        }

        /// <summary>
        /// Set the topic name on the main window title bar.
        /// </summary>
        public void SetTopicName(string format)
        {
            MainForm.UserTitleString = format;
        }

        /// <summary>
        /// Show the info bar with the specified message for 2.5 seconds then
        /// automatically hide it.
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void ShowInfoBar(string message)
        {
            frmInfoBar.InfoText = message;
            frmInfoBar.InfoIcon = CRInfoBarIcon.None;
            frmInfoBar.Show(2500);
        }

        /// <summary>
        /// Invoke the forum management dialog.
        /// </summary>
        public void ManageForum(DirForum forum)
        {
            ManageForum manageForumWindow = new ManageForum(forum);
            manageForumWindow.ShowDialog();
        }

        /// <summary>
        /// Display the participants list for the specified forum.
        /// </summary>
        public void DisplayParticipants(string forumName)
        {
            Participants parDialog = new Participants(MainForm, forumName);
            parDialog.ShowDialog();
        }

        /// <summary>
        /// Prompt and then mark the specified folder read.
        /// </summary>
        public void HandleMarkFolderRead(Folder folder)
        {
            string promptString = String.Format(Resources.MarkFolderRead, folder.Name);
            if (MessageBox.Show(promptString, Resources.Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                folder.MarkAllRead();
            }
        }

        /// <summary>
        /// Display the folder referenced by the given node, passing through the address and options.
        /// </summary>
        public bool SelectViewForFolder(TreeNode node, Address address, FolderOptions options)
        {
            FolderBase folder = (FolderBase)node.Tag;
            return SelectView(folder.ViewForFolder, folder, address, options);
        }

        /// <summary>
        /// First time load of the forums view, add all folders we know about to the list.
        /// </summary>
        private void FoldersTree_Load(object sender, EventArgs args)
        {
            _initialising = true;

            RefreshTheme();

            // If the directory is empty, use this as an opportunity to refresh it from CIX.
            int categoryCount = CIX.DirectoryCollection.Categories.Count();
            if (categoryCount == 0)
            {
                CIX.DirectoryCollection.Refresh();
            }

            float splitPositionPercent = Preferences.StandardPreferences.FolderSplitPosition;
            if (splitPositionPercent <= 0 || splitPositionPercent > 100)
            {
                splitPositionPercent = 25.0f;
            }
            int splitDistance = (int)(frmSplitContainer.Width / (100.0 / splitPositionPercent));
            if (splitDistance >= frmSplitContainer.Panel1MinSize && splitDistance <= frmSplitContainer.Width - frmSplitContainer.Panel2MinSize)
            {
                frmSplitContainer.SplitterDistance = splitDistance;
            }

            CIX.FolderCollection.ForumUpdateStarted += OnForumUpdateStarted;
            CIX.FolderCollection.ForumUpdateCompleted += OnForumUpdateCompleted;
            CIX.FolderCollection.FolderRefreshed += OnFolderRefreshed;
            CIX.FolderCollection.FolderUpdated += OnFolderUpdated;
            CIX.FolderCollection.FoldersAdded += OnFoldersAdded;
            CIX.FolderCollection.FolderDeleted += OnFolderDeleted;

            CIX.DirectoryCollection.ForumJoined += OnForumJoined;
            CIX.DirectoryCollection.DirectoryChanged += OnDirectoryChanged;

            CIX.ConversationCollection.ConversationChanged += OnConversationChanged;

            // Get notified of theme changes.
            UI.ThemeChanged += OnThemeChanged;

            // Get notified of settings changes.
            Preferences.PreferencesChanged += OnPreferencesChanged;

            Update();

            KeyPreview = true;

            LoadTree();
            MainForm.UpdateTotalUnreadCount();

            _initialising = false;
        }

        /// <summary>
        /// Invoke the Join dialog to join the forum specified by the address.
        /// This only works for cix: addresses. Others are ignored.
        /// </summary>
        /// <param name="address">The forum or forum/topic address</param>
        /// <returns>True if we displayed the Join Forum dialog, false otherwise</returns>
        private static bool JoinMissingForum(Address address)
        {
            if (address.Scheme == "cix")
            {
                string[] splitAddress = address.Query.Split(new[] {'/'});
                if (splitAddress.Length > 0)
                {
                    JoinForum joinForum = new JoinForum(splitAddress[0]);
                    joinForum.ShowDialog();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Handle changes to the forum directory by rebuilding the
        /// directory tree.
        /// </summary>
        private void OnDirectoryChanged(object sender, DirectoryEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                _directoryTree.Nodes.Clear();
                LoadDirectoryTree(_directoryTree);
            });
        }

        /// <summary>
        /// Inbox has updated, so redraw the node and the count on the node, and also
        /// let the main form update the count on the shortcut bar.
        /// </summary>
        private void OnConversationChanged(object sender, InboxEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                frmList.InvalidateNode(_messagesTree.Nodes[0]);
                MainForm.UpdateTotalUnreadCount();
            });
        }

        /// <summary>
        /// Handle preferences change events.
        /// </summary>
        private void OnPreferencesChanged(object sender, PreferencesChangedEventArgs e)
        {
            if (e.Name == Preferences.MAPref_ShowAllTopics)
            {
                LoadTree();
            }
            if (e.Name == Preferences.MAPref_EnableSmartFolderCounts)
            {
                SmartFolderRefresh(this, null);
            }
        }

        /// <summary>
        /// Handle Search by showing or hiding the search bar (if we're currently
        /// in a topic folder, otherwise do nothing) then trigger a search in the
        /// current view.
        /// </summary>
        /// <param name="searchText">The text to search</param>
        public void Search(string searchText)
        {
            if (searchText == string.Empty)
            {
                if (_searchBarVisible)
                {
                    ShowSearchBar(SearchBarVisibility.FastHide);
                    RefreshFolder(_selectedNode, FolderOptions.None);
                }
                return;
            }

            FolderBase folder = (FolderBase) _selectedNode.Tag;
            if (folder is TopicFolder)
            {
                ShowSearchBar(SearchBarVisibility.Show);
            }

            if (_selectedNode != null)
            {
                _currentSearchText = searchText;
                SearchForStringInFolder(folder, searchText);
            }
        }

        /// <summary>
        /// Trigger on a search button click.
        /// </summary>
        private void SearchButtonOnClick(object sender, EventArgs e)
        {
            CRRoundButton button = (CRRoundButton) sender;
            if (button != null)
            {
                if (!string.IsNullOrWhiteSpace(_currentSearchText))
                {
                    SearchForStringInFolder((FolderBase)button.ExtraData, _currentSearchText);
                }
            }
        }

        /// <summary>
        /// Search for the given text in the selected folder and update the UI to reflect
        /// the search scope.
        /// </summary>
        private void SearchForStringInFolder(FolderBase folder, string searchText)
        {
            string scopeName = ScopeFromFolder(folder);
            foreach (var roundButton in _searchBar.Controls.OfType<CRRoundButton>())
            {
                roundButton.Active = (roundButton.Name == scopeName);
            }
            if (folder != null)
            {
                _currentView.FilterViewByString(searchText);
            }
        }

        /// <summary>
        /// Invoked when a forum update is started.
        /// </summary>
        private void OnForumUpdateStarted(object sender, EventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                if (frmList.Nodes.Count == 0)
                {
                    StartProgressBar();
                }
            });
        }

        /// <summary>
        /// Invoked when a forum update completes.
        /// </summary>
        private void OnForumUpdateCompleted(object sender, EventArgs args)
        {
            Platform.UIThread(this, StopProgressBar);
        }

        /// <summary>
        /// Handle theme changed events to redraw the folder tree elements
        /// in the new style.
        /// </summary>
        private void OnThemeChanged(object sender, EventArgs args)
        {
            RefreshTheme();

            // Force redraw of any affected controls
            frmList.Invalidate();
        }

        /// <summary>
        /// Update our copy of the font and font metrics when the theme changes.
        /// </summary>
        private void RefreshTheme()
        {
            _topicNameFont = UI.Forums.ListFont;
            _unreadFont = UI.GetFont(UI.System.font, 8);
            _boldTopicNameFont = UI.GetFont(UI.Forums.listfont, UI.Forums.listfontsize, FontStyle.Bold);

            // Determine the node item heights based on the font and size
            SizeF textSize = CreateGraphics().MeasureString("M", _topicNameFont);
            frmList.ItemHeight = (int) textSize.Height + 7;

            // Splitter bar colour
            frmSplitContainer.BackColor = UI.System.SplitterBarColour;
        }

        /// <summary>
        /// Toggle the folder list between showing all and recent topics.
        /// </summary>
        /// <param name="showAllTopics"></param>
        private static void ChangeFolderListDisplay(bool showAllTopics)
        {
            Preferences.StandardPreferences.ShowAllTopics = showAllTopics;
        }

        /// <summary>
        /// Build the folder list with all folders which are valid in the specified view.
        /// </summary>
        private void LoadTree()
        {
            frmList.BeginUpdate();
            frmList.Nodes.Clear();

            // Home page category
            _homePage = new TreeNode
            {
                Name = Resources.Home,
                Tag = new FolderBase()
            };
            frmList.Nodes.Add(_homePage);

            // Directory category
            _directoryTree = new TreeNode
            {
                Name = Resources.Directory,
                Tag = new FolderBase()
            };
            frmList.Nodes.Add(_directoryTree);
            LoadDirectoryTree(_directoryTree);

            // Messages category
            _messagesTree = new TreeNode
            {
                Name = Resources.PMessages,
                Tag = new MailGroup()
            };
            frmList.Nodes.Add(_messagesTree);
            LoadMessagesTree(_messagesTree);

            // Forums category
            _forumsTree = new TreeNode
            {
                Name = Resources.Forums,
                Tag = new ForumGroup()
            };
            frmList.Nodes.Add(_forumsTree);
            LoadForumsTree(_forumsTree);

            frmList.TopNode = _homePage;
            _forumsTree.Expand();
            _messagesTree.Expand();

            frmList.EndUpdate();
        }

        /// <summary>
        /// Load the directory tree.
        /// </summary>
        private static void LoadDirectoryTree(TreeNode parent)
        {
            foreach (string categoryName in CIX.DirectoryCollection.Categories)
            {
                CategoryFolder categoryFolder = new CategoryFolder {Name = categoryName};
                parent.Nodes.Add(new TreeNode
                {
                    Name = categoryName,
                    Tag = categoryFolder
                });
            }
            CategoryFolder allCategoryFolder = new CategoryFolder {Name = CategoryFolder.AllCategoriesName};
            parent.Nodes.Insert(0, new TreeNode
            {
                Name = CategoryFolder.AllCategoriesName,
                Tag = allCategoryFolder
            });
        }

        /// <summary>
        /// Reload the list of topics under the given forum folder.
        /// </summary>
        private void ReloadForumTreeFromNode(FolderBase forumFolder)
        {
            TreeNode parent = FindFolder(_forumsTree.Nodes, forumFolder.Name);
            bool showAllTopics = Preferences.StandardPreferences.ShowAllTopics;

            if (parent != null)
            {
                TopicFolder folder = (TopicFolder) forumFolder;
                parent.Nodes.Clear();
                foreach (Folder topic in folder.Folder.Children.OrderBy(nm => nm.Index))
                {
                    bool showArchivedTopic = showAllTopics || !topic.IsRecent && topic.Unread > 0;
                    if (showArchivedTopic || topic.IsRecent)
                    {
                        TopicFolder topicFolder = new TopicFolder(topic) {Name = topic.Name};
                        TreeNode subNode = new TreeNode(topicFolder.Name) {Tag = topicFolder};

                        parent.Nodes.Add(subNode);
                    }
                }
            }
        }

        /// <summary>
        /// Load the PMessages tree.
        /// </summary>
        private static void LoadMessagesTree(TreeNode parent)
        {
            MailFolder mailFolder = new MailFolder { Name = Resources.InboxName };
            parent.Nodes.Insert(0, new TreeNode
            {
                Name = Resources.InboxName,
                Tag = mailFolder
            });
        }

        /// <summary>
        /// Load the entire forums tree.
        /// </summary>
        private void LoadForumsTree(TreeNode parent)
        {
            bool showAllTopics = Preferences.StandardPreferences.ShowAllTopics;
            foreach (Folder folder in CIX.FolderCollection.Where(fld => fld.IsRootFolder).OrderBy(fld => fld.Index))
            {
                TopicFolder forumFolder = new TopicFolder(folder) { Name = folder.Name };
                TreeNode node = new TreeNode(folder.Name) { Tag = forumFolder };

                int folderID = folder.ID;
                foreach (Folder topic in CIX.FolderCollection.Where(fld => fld.ParentID == folderID).OrderBy(nm => nm.Index))
                {
                    bool showArchivedTopic = showAllTopics || !topic.IsRecent && topic.Unread > 0;
                    if (showArchivedTopic || topic.IsRecent)
                    {
                        TopicFolder topicFolder = new TopicFolder(topic) { Name = topic.Name };
                        TreeNode subNode = new TreeNode(topicFolder.Name) { Tag = topicFolder };

                        node.Nodes.Add(subNode);
                    }
                }
                parent.Nodes.Add(node);
            }

            // Group special folders at the top
            SmartFolder starredFolder = new SmartFolder
            {
                Name = Resources.Starred,
                Comparator = message => message.Starred,
                Criteria = PredicateBuilder.GetExpression<CIXMessage>(new List<PredicateBuilder.Filter>
                    {
                        new PredicateBuilder.Filter { 
                            PropertyName = "Starred" , 
                            Operation = PredicateBuilder.Op.Equals, 
                            Value = true }
                    })
            };
            parent.Nodes.Insert(0, new TreeNode
            {
                Name = Resources.Starred,
                Tag = starredFolder
            });

            SmartFolder draftsFolder = new SmartFolder
            {
                Name = Resources.DraftText,
                Comparator = message => message.IsDraft,
                Criteria = PredicateBuilder.GetExpression<CIXMessage>(new List<PredicateBuilder.Filter>
                    {
                        new PredicateBuilder.Filter { 
                            PropertyName = "RemoteID" , 
                            Operation = PredicateBuilder.Op.GreaterThanOrEqual, 
                            Value = int.MaxValue / 2 },
                        new PredicateBuilder.Filter { 
                            PropertyName = "PostPending" , 
                            Operation = PredicateBuilder.Op.Equals, 
                            Value = false }
                    })
            };
            parent.Nodes.Insert(1, new TreeNode
            {
                Name = Resources.DraftText,
                Tag = draftsFolder
            });

            SmartFolder outboxFolder = new SmartFolder
            {
                Name = Resources.OutboxText,
                Comparator = message => message.IsPending,
                Criteria = PredicateBuilder.GetExpression<CIXMessage>(new List<PredicateBuilder.Filter>
                    {
                        new PredicateBuilder.Filter { 
                            PropertyName = "PostPending" , 
                            Operation = PredicateBuilder.Op.Equals, 
                            Value = true }
                    })
            };
            parent.Nodes.Insert(2, new TreeNode
            {
                Name = Resources.OutboxText,
                Tag = outboxFolder
            });

            _allSmartFolders = new List<TreeNode>
            {
                parent.Nodes[0], parent.Nodes[1], parent.Nodes[2]
            };
        }

        /// <summary>
        /// Handle the folder update event to redraw the folder's node in the tree
        /// and then update the counts on the shortcut bar.
        /// </summary>
        /// <param name="folder">The folder that was updated</param>
        private void OnFolderUpdated(Folder folder)
        {
            Platform.UIThread(this, delegate
            {
                UpdateFolder(folder);
                MainForm.UpdateTotalUnreadCount();
            });
        }

        /// <summary>
        /// Handle the folder refresh event to redraw the folder's node in the tree
        /// and then update the counts on the shortcut bar.
        /// </summary>
        private void OnFolderRefreshed(object sender, FolderEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                UpdateFolder(args.Folder);
                MainForm.UpdateTotalUnreadCount();
            });
        }

        /// <summary>
        /// Handle the folders added event.
        /// </summary>
        private void OnFoldersAdded(object sender, FoldersAddedEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                frmList.BeginUpdate();
                foreach (Folder folder in args.Folders)
                {
                    if (Preferences.StandardPreferences.ShowAllTopics || folder.IsRootFolder || folder.IsRecent)
                    {
                        InsertFolder(folder, false);
                    }
                }
                frmList.EndUpdate();
            });
        }

        /// <summary>
        /// Handle the folder deletion event.
        /// </summary>
        /// <param name="folder">The folder that was deleted</param>
        private void OnFolderDeleted(Folder folder)
        {
            Platform.UIThread(this, delegate
            {
                TreeNode nodeToSelect = null;
                TreeNode node;
                if (folder.IsRootFolder)
                {
                    node = FindFolder(_forumsTree.Nodes, folder.Name);
                }
                else
                {
                    node = FindFolder(_forumsTree.Nodes, folder.ParentFolder.Name);
                    if (node == null)
                    {
                        return;
                    }
                    node = FindFolder(node.Nodes, folder.Name);
                }
                if (node != null)
                {
                    if (node.IsSelected)
                    {
                        nodeToSelect = node.PrevNode ?? node.NextNode;
                    }
                    node.Remove();
                }
                if (nodeToSelect != null)
                {
                    SelectFolder(nodeToSelect, FolderOptions.None);
                }
                MainForm.UpdateTotalUnreadCount();
            });
        }

        /// <summary>
        /// Handle the forum joined event.
        /// </summary>
        /// <param name="folder">The folder for the forum that was joined</param>
        private void OnForumJoined(Folder folder)
        {
            Platform.UIThread(this, delegate
            {
                TreeNode node = FindFolder(_forumsTree.Nodes, folder.Name);
                if (node == null)
                {
                    node = InsertFolder(folder, false);
                }
                else
                {
                    UpdateFolder(folder);
                }
                SelectFolder(node, FolderOptions.None);
            });
        }

        /// <summary>
        /// Locate and update the specified folder in the tree.
        /// </summary>
        private void UpdateFolder(Folder folderToUpdate)
        {
            int itemIndex = 0;
            TreeNodeCollection containerNodes = _forumsTree.Nodes;
            bool foundNode = false;

            // Find the top level folder to which this belongs.
            while (itemIndex < containerNodes.Count)
            {
                TreeNode node = containerNodes[itemIndex];
                FolderBase folder = (FolderBase)node.Tag;
                if (folderToUpdate.ParentID >= 0 && folder.ID == folderToUpdate.ParentID)
                {
                    containerNodes = node.Nodes;
                    itemIndex = 0;
                    continue;
                }
                if (folder.ID == folderToUpdate.ID)
                {
                    // Force an invalidate so we redraw the node with the
                    // changed information.
                    if (node.IsVisible)
                    {
                        frmList.InvalidateNode(node);
                    }
                    else if (node.Parent != null && node.Parent.IsVisible)
                    {
                        frmList.InvalidateNode(node.Parent);
                    }
                    foundNode = true;
                    break;
                }
                ++itemIndex;
            }

            // If we didn't find the node but the folder has unread messages AND
            // we're only showing recent then a non-recent folder got new messages.
            // So put that folder back in the list
            if (!foundNode && folderToUpdate.Unread > 0)
            {
                InsertFolder(folderToUpdate, false);
            }

            // Trigger an update to smart folders
            TriggerSmartFolderRefresh();
        }

        /// <summary>
        /// Insert the specified folder into the tree if it isn't already present. The folder's
        /// parents are also added if they are missing in order to preserve the tree integrity.
        /// Subview filtering is respected so if the folder doesn't belong to the current subview
        /// then it is not added. If the folder is already present, we force a refresh on it to
        /// update it to show any modified data.
        /// </summary>
        private TreeNode InsertFolder(Folder newFolder, bool addChildren)
        {
            if (newFolder.ParentID != -1)
            {
                Folder parentFolder = newFolder.ParentFolder;
                if (parentFolder != null)
                {
                    InsertFolder(parentFolder, addChildren);
                }
            }

            int insertIndex = 0;
            int itemIndex = 0;
            TreeNode thisNode = null;
            TreeNodeCollection containerNodes = _forumsTree.Nodes;
            bool foundPosition = false;

            // Find the top level folder to which this belongs.
            while (itemIndex < containerNodes.Count)
            {
                TreeNode node = containerNodes[itemIndex];
                FolderBase folder = (FolderBase)node.Tag;
                if (folder != null)
                {
                    if (newFolder.ParentID >= 0 && folder.ID == newFolder.ParentID)
                    {
                        containerNodes = node.Nodes;
                        insertIndex = 0;
                        itemIndex = 0;
                        foundPosition = false;
                        continue;
                    }
                    if (folder.ID == newFolder.ID)
                    {
                        // Force an invalidate so we redraw the node with the
                        // changed information.
                        frmList.InvalidateNode(node);

                        thisNode = node;
                        break;
                    }
                    if (String.Compare(newFolder.Name, folder.Name, StringComparison.Ordinal) < 0 && folder.ID > 0 && !foundPosition)
                    {
                        insertIndex = itemIndex;
                        foundPosition = true;
                    }
                }
                ++itemIndex;
                if (folder != null && folder.ID < 0)
                {
                    // Skip special folders
                    insertIndex = itemIndex;
                }
            }
            if (!foundPosition)
            {
                insertIndex = itemIndex;
            }

            // This is an insertion task. At this point containerNodes will reference
            // the container nodes for the new folder and insertIndex will be the index
            // within that at which the new folder is added.
            if (thisNode == null)
            {
                thisNode = new TreeNode(newFolder.Name)
                {
                    Tag = new TopicFolder(newFolder) { Name = newFolder.Name }
                };
                if (newFolder.IsRootFolder && addChildren)
                {
                    foreach (Folder topic in newFolder.Children)
                    {
                        TreeNode subNode = new TreeNode(topic.Name)
                        {
                            Tag = new TopicFolder(topic) { Name = topic.Name }
                        };
                        thisNode.Nodes.Add(subNode);
                    }
                }

                containerNodes.Insert(insertIndex, thisNode);

                // Persist the index to the database.
                FixupNodeIndexes(_forumsTree.Nodes);
            }
            return thisNode;
        }

        /// <summary>
        /// Find the tree node in the node collection which matches the folder name. Only the
        /// given level is searched and child nodes are disregarded.
        /// </summary>
        /// <param name="nodes">Nodes to search</param>
        /// <param name="folderName">Name of folder requested</param>
        /// <returns>The FolderBase for the matching folder, or null</returns>
        private static TreeNode FindFolder(IEnumerable nodes, string folderName)
        {
            return (from TreeNode node in nodes let folder = (FolderBase)node.Tag where folder.Name == folderName select node).FirstOrDefault();
        }

        private void TriggerSmartFolderRefresh()
        {
            if (Preferences.StandardPreferences.EnableSmartFolderCounts)
            {
                _smartFolderRefresh.Enabled = true;
                _smartFolderRefresh.Stop();
                _smartFolderRefresh.Start();
            }
        }

        private void SmartFolderRefresh(object obj, EventArgs args)
        {
            _smartFolderRefresh.Stop();
            foreach (TreeNode node in _allSmartFolders)
            {
                SmartFolder folder = node.Tag as SmartFolder;
                if (folder != null)
                {
                    folder.RefreshCount();
                    TreeNode node1 = node;
                    Platform.UIThread(this, () => frmList.InvalidateNode(node1));
                }
            }
        }

        /// <summary>
        /// Select a folder based on the address of the node.
        /// </summary>
        /// <param name="address">Address of the node</param>
        /// <returns>True if we found and selected the folder, false if it was not found</returns>
        private bool SelectAddress(Address address)
        {
            FolderBase selection = FolderFromAddress(address.SchemeAndQuery);
            if (selection != null)
            {
                SetSelection(selection, address);
            }
            return selection != null;
        }

        /// <summary>
        /// Find the specified folder in the tree and if found, select it. If it is
        /// already selected, invoke the view with the specified address.
        /// </summary>
        /// <param name="folder">The folder to select</param>
        /// <param name="address">The address to pass to the view</param>
        private void SetSelection(FolderBase folder, Address address)
        {
            if (folder != null)
            {
                TreeNode node = NodeForFolder(folder, frmList.Nodes);
                if (node != null && node.IsSelected)
                {
                    SelectViewForFolder(node, address, 0);
                }
                else
                {
                    _lastAddress = address;
                    SelectFolder(node, 0);
                }
            }
        }

        /// <summary>
        /// Walks the TreeView looking for the node which has the specified folder
        /// associated with it.
        /// </summary>
        /// <param name="folder">The folder to locate</param>
        /// <param name="nodes">The nodes to search</param>
        /// <returns>The TreeNode of the folder, or null if not found</returns>
        private static TreeNode NodeForFolder(FolderBase folder, IEnumerable nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == folder)
                {
                    return node;
                }
                if (node.Nodes.Count > 0)
                {
                    TreeNode subNode = NodeForFolder(folder, node.Nodes);
                    if (subNode != null)
                    {
                        return subNode;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the folder whose address corresponds to the given address.
        /// </summary>
        /// <param name="address">Address of the folder to locate</param>
        /// <returns>The FolderBase of the folder at the address or null if it was not found</returns>
        private FolderBase FolderFromAddress(string address)
        {
            return FolderFromAddress(address, frmList.Nodes);
        }

        /// <summary>
        /// Recursively locate the folder whose ID corresponds to the given ID
        /// under the specified tree node collection.
        /// </summary>
        /// <param name="id">ID to match</param>
        /// <param name="nodes">Tree nodes to search</param>
        /// <returns>The FolderBase of the folder with the given ID, or null if it was not found</returns>
        private static FolderBase FolderWithID(int id, IEnumerable nodes)
        {
            foreach (TreeNode node in nodes)
            {
                FolderBase folder = node.Tag as FolderBase;
                if (folder != null && folder.ID == id)
                {
                    return folder;
                }
                if (node.Nodes.Count > 0)
                {
                    FolderBase subFolder = FolderWithID(id, node.Nodes);
                    if (subFolder != null)
                    {
                        return subFolder;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Recursively locate the folder whose address corresponds to the given address
        /// under the specified tree node collection.
        /// </summary>
        /// <param name="address">Address to match</param>
        /// <param name="nodes">Tree nodes to search</param>
        /// <returns>The FolderBase of the folder with the given address, or null if it was not found</returns>
        private FolderBase FolderFromAddress(string address, IEnumerable nodes)
        {
            foreach (TreeNode node in nodes)
            {
                FolderBase folder = node.Tag as FolderBase;
                if (folder != null && folder.Address == address)
                {
                    return folder;
                }
                if (node.Nodes.Count > 0)
                {
                    FolderBase subFolder = FolderFromAddress(address, node.Nodes);
                    if (subFolder != null)
                    {
                        return subFolder;
                    }
                }
            }

            // Not found but possibly hidden if we're filtering by recent so parse off
            // the parent folder and look for that, then unhide the topic.
            Address addr = new Address(address);
            if (addr.Scheme == "cix" && addr.Query != null)
            {
                string[] splitAddress = addr.Query.Split(new[] {'\\'});
                if (splitAddress.Length == 2)
                {
                    Folder forum = CIX.FolderCollection.Get(-1, splitAddress[0]);
                    Folder topic = CIX.FolderCollection.Get(forum.ID, splitAddress[1]);

                    if (topic != null && !topic.IsRecent)
                    {
                        topic.IsRecent = true;
                        FolderBase forumFolder = FolderWithID(forum.ID, _forumsTree.Nodes);
                        ReloadForumTreeFromNode(forumFolder);

                        return FolderWithID(topic.ID, _forumsTree.Nodes);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Move the selection to the specified node and redraw both the previous
        /// selected node and this new one. Does nothing if the node is null or
        /// already selected.
        /// </summary>
        /// <param name="node">The TreeNode to be selected</param>
        private void MoveSelection(TreeNode node)
        {
            _selectedNode = node;
            if (_selectedNode != null && _selectedNode != frmList.SelectedNode)
            {
                TreeNode lastSelectedNode = frmList.SelectedNode;

                frmList.SelectedNode = _selectedNode;

                if (lastSelectedNode != null)
                {
                    frmList.InvalidateNode(lastSelectedNode);
                }
                frmList.InvalidateNode(_selectedNode);
                UpdateContextMenu();
            }
        }

        /// <summary>
        /// Select a folder and display all messages in that folder.
        /// TODO: make this call MoveSelection to remove the duplicate logic! 
        /// </summary>
        private void SelectFolder(TreeNode node, FolderOptions flags)
        {
            _selectedNode = node;
            if (_selectedNode != null)
            {
                TreeNode lastSelectedNode = frmList.SelectedNode;
                frmList.SelectedNode = _selectedNode;

                if (lastSelectedNode != null)
                {
                    frmList.InvalidateNode(lastSelectedNode);
                }
                frmList.InvalidateNode(_selectedNode);
                RefreshFolder(node, flags);

                UpdateContextMenu();
            }
        }

        /// <summary>
        /// Refresh the view for the folder at the given node.
        /// </summary>
        /// <param name="node">The node of the folder to refresh</param>
        /// <param name="options">Options to be passed to the view</param>
        private void RefreshFolder(TreeNode node, FolderOptions options)
        {
            if (node != null)
            {
                SelectViewForFolder(node, _lastAddress, options);
                _lastAddress = null;
            }
        }

        /// <summary>
        /// Display the selected view with the given folder, passing through the address and options.
        /// </summary>
        /// <param name="requestedView">The ID of the type of the view requested</param>
        /// <param name="folder">The folder to be displayed in the view</param>
        /// <param name="address">The address of the item within the folder to be selected</param>
        /// <param name="options">Options controlling the folder selection</param>
        /// <returns>True if the view was successfully selected, false otherwise</returns>
        private bool SelectView(AppView requestedView, FolderBase folder, Address address, FolderOptions options)
        {
            ViewBaseView newView = _allViews[requestedView];

            if (newView != _currentView)
            {
                if (_currentView != null)
                {
                    _currentView.Visible = false;
                    frmSplitContainer.Panel2.Controls.Remove(_currentView);
                }
                if (newView != null)
                {
                    frmSplitContainer.Panel2.Controls.Add(newView);

                    _currentView = newView;
                    SetSubviewSize();
                    newView.Visible = true;
                    newView.Update();
                }
            }

            ShowSearchBar(SearchBarVisibility.FastHide);

            if (folder != null)
            {
                string placeholder = folder.AllowsScopedSearch
                    ? string.Format(Resources.SearchForTextIn, folder.Name)
                    : Resources.Search;
                MainForm.SetSearchFieldPlaceholder(placeholder);

                SetTopicName(folder.FullName);
            }
            return (_currentView != null) && _currentView.ViewFromFolder(folder, address, options);
        }

        /// <summary>
        /// Update the context menu when the selection changes.
        /// </summary>
        private void UpdateContextMenu()
        {
            frmRecentTopics.Checked = !Preferences.StandardPreferences.ShowAllTopics;
            frmAllTopics.Checked = Preferences.StandardPreferences.ShowAllTopics;

            frmResign.Enabled = CanAction(ActionID.ResignForum);
            frmDelete.Enabled = CanAction(ActionID.Delete);
            frmMarkAllRead.Enabled = CanAction(ActionID.MarkTopicRead);
            frmParticipants.Enabled = CanAction(ActionID.Participants);
            frmManage.Enabled = CanAction(ActionID.ManageForum);
            frmRefresh.Enabled = CanAction(ActionID.Refresh);
        }

        /// <summary>
        /// Show or hide the search bar.
        /// </summary>
        /// <param name="v">Visibility</param>
        private void ShowSearchBar(SearchBarVisibility v)
        {
            _searchBarHeight = 32;
            if (_searchBar == null)
            {
                _searchBar = new CRPanel
                {
                    Anchor = (AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right,
                    BackColor = UI.System.InfoBarColour,
                    BottomBorderWidth = 1,
                    ForeColor = UI.System.EdgeColour,
                    Name = "sbPanel",
                    Parent = this,
                    Location = new Point(0, 0),
                    Size = new Size(575, 0)
                };

                Controls.Add(_searchBar);
            }
            if (v == SearchBarVisibility.Show && !_searchBarVisible)
            {
                _searchBarIncrement = 4;
                _searchBarIteration = 0;
                _searchBarTimer.Enabled = true;
                _searchBarVisible = true;
            }
            if (v == SearchBarVisibility.FastHide && _searchBarVisible)
            {
                _searchBar.Size = new Size(frmSplitContainer.Width, _searchBar.Height - _searchBarHeight);
                frmSplitContainer.Location = new Point(0, frmSplitContainer.Top - _searchBarHeight);
                frmSplitContainer.Size = new Size(frmSplitContainer.Width, frmSplitContainer.Height + _searchBarHeight);
                _searchBarVisible = false;
            }
            UpdateSearchBarButtons();
        }

        /// <summary>
        /// Update all the search bar buttons when the selected forum or
        /// topic changes.
        /// </summary>
        private void UpdateSearchBarButtons()
        {
            if (_searchBar != null && _searchBarVisible)
            {
                const int yPosition = 4;
                int xPosition = 4;

                _searchBar.Controls.Clear();
                _searchBar.SuspendLayout();

                string topicName = null;
                string forumName;

                FolderBase forumFolder;
                FolderBase topicFolder;

                TreeNode currentNode = frmList.SelectedNode;
                if (currentNode.Parent != null)
                {
                    forumName = currentNode.Parent.Text;
                    topicName = currentNode.Text;

                    forumFolder = currentNode.Parent.Tag as FolderBase;
                    topicFolder = currentNode.Tag as FolderBase;
                }
                else
                {
                    forumName = currentNode.Text;

                    forumFolder = currentNode.Tag as FolderBase;
                    topicFolder = null;
                }

                CRRoundButton currentForumSearchButton = new CRRoundButton
                {
                    Name = _currentForumSearchButton,
                    Text = forumName,
                    CanHaveFocus = true,
                    AutoSize = true,
                    Parent = _searchBar,
                    ExtraData = forumFolder,
                    Location = new Point(xPosition, yPosition)
                };
                currentForumSearchButton.Click += SearchButtonOnClick;
                _searchBar.Controls.Add(currentForumSearchButton);

                ToolTip newToolTip = new ToolTip();
                newToolTip.SetToolTip(currentForumSearchButton, Resources.CurrentForumSearch);

                if (topicName != null)
                {
                    xPosition += currentForumSearchButton.Width + 8;

                    CRRoundButton currentTopicSearchButton = new CRRoundButton
                    {
                        Name = _currentTopicSearchButton,
                        Text = topicName,
                        CanHaveFocus = true,
                        AutoSize = true,
                        Parent = _searchBar,
                        ExtraData = topicFolder,
                        Location = new Point(xPosition, yPosition)
                    };
                    currentTopicSearchButton.Click += SearchButtonOnClick;
                    _searchBar.Controls.Add(currentTopicSearchButton);

                    newToolTip = new ToolTip();
                    newToolTip.SetToolTip(currentTopicSearchButton, Resources.CurrentTopicSearch);
                }

                PictureBox sbClose = new PictureBox
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Image = Resources.CloseButton,
                    Location = new Point(_searchBar.Right - 36, 4),
                    Name = "sbClose",
                    Parent = _searchBar,
                    Size = new Size(24, 24),
                    TabIndex = 4,
                    TabStop = false
                };
                sbClose.Click += OnSearchBarClose;
                _searchBar.Controls.Add(sbClose);

                newToolTip = new ToolTip();
                newToolTip.SetToolTip(sbClose, Resources.CloseSearchBar);

                _searchBar.ResumeLayout(false);
            }
        }

        /// <summary>
        /// Given a folder, return the name of the scope of this folder in a
        /// search.
        /// </summary>
        private static string ScopeFromFolder(FolderBase folder)
        {
            if (folder == null)
            {
                return _allForumsSearchButton;
            }
            if (folder is TopicFolder && ((TopicFolder)folder).Folder.IsRootFolder)
            {
                return _currentForumSearchButton;
            }
            return _currentTopicSearchButton;
        }

        /// <summary>
        /// Close the search bar.
        /// </summary>
        private void OnSearchBarClose(object sender, EventArgs eventArgs)
        {
            ShowSearchBar(SearchBarVisibility.FastHide);
            RefreshFolder(_selectedNode, FolderOptions.ClearFilter);
        }

        /// <summary>
        /// Timer tick to animate scrolling the search bar in and out of view.
        /// </summary>
        private void SearchBarTimerOnTick(object sender, EventArgs eventArgs)
        {
            _searchBar.Size = new Size(frmSplitContainer.Width, _searchBar.Height + _searchBarIncrement);
            _searchBar.Invalidate();

            frmSplitContainer.Location = new Point(0, frmSplitContainer.Top + _searchBarIncrement);
            frmSplitContainer.Size = new Size(frmSplitContainer.Width, frmSplitContainer.Height - _searchBarIncrement);

            if (++_searchBarIteration == (_searchBarHeight / 4))
            {
                _searchBarTimer.Stop();
            }
        }

        /// <summary>
        /// Handle keyboard interaction at the form level.
        /// </summary>
        private void FoldersTree_KeyDown(object sender, KeyEventArgs args)
        {
            Control activeControl = Platform.GetFocusedControl();
            bool passToCurrentView = _currentView != null && !(ActiveControl is CRSearchField) && activeControl != frmList;
            if (activeControl == frmList && args.KeyCode == Keys.Right)
            {
                // A bit of a hack. Whatever the right arrow key is assigned to, let the subview take it if
                // it is a no-op in the forums list.
                TreeNode currentNode = frmList.SelectedNode;
                if (currentNode != null && (currentNode.IsExpanded || currentNode.Nodes.Count == 0))
                {
                    passToCurrentView = true;
                }
            }
            if (passToCurrentView)
            {
                if (MainForm.HandleFormKeyDown(args.KeyData))
                {
                    args.Handled = true;
                    _blockKeyDown = true;
                }
            }
        }

        private void frmList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = _blockKeyDown;
            _blockKeyDown = false;
        }

        /// <summary>
        /// Draw the treeview node.
        /// </summary>
        private void frmList_DrawNode(object sender, DrawTreeNodeEventArgs args)
        {
            if (args.Node.IsVisible && !args.Bounds.IsEmpty)
            {
                bool isFocused = (args.State & TreeNodeStates.Focused) != 0;
                bool isSelected = (args.State & TreeNodeStates.Selected) != 0;

                Color colour;
                Color backColour;

                FolderBase folder = (FolderBase)args.Node.Tag;

                if (isFocused || isSelected)
                {
                    colour = UI.System.SelectionTextColour;
                    backColour = UI.System.SelectionColour;
                }
                else if (folder is TopicFolder && ((TopicFolder)folder).Folder.IsResigned)
                {
                    colour = UI.Forums.IgnoredColour;
                    backColour = BackColor;
                }
                else
                {
                    colour = UI.System.ForegroundColour;
                    backColour = BackColor;
                }

                // Blow away the background. If we're drawing the drag item then
                // render it differently so it stands out.
                using (Brush backBrush = new SolidBrush(backColour))
                {
                    if (args.Node == _nodeAtDragPoint)
                    {
                        Rectangle rect = args.Bounds;
                        const int selectionWidth = 3;

                        // Draw selection focus.
                        using (Pen linePen = new Pen(UI.System.SelectionColour, selectionWidth))
                        {
                            args.Graphics.DrawLine(linePen, rect.X, rect.Y, rect.X + rect.Width - 1, rect.Y);
                            args.Graphics.DrawLine(linePen, rect.X + rect.Width - 1, rect.Y, rect.X + rect.Width - 1, rect.Y + rect.Height - 1);
                            args.Graphics.DrawLine(linePen, rect.X + rect.Width - 1, rect.Y + rect.Height - 1, rect.X, rect.Y + rect.Height - 1);
                            args.Graphics.DrawLine(linePen, rect.X, rect.Y + rect.Height - 1, rect.X, rect.Y);
                        }
                        colour = UI.System.ForegroundColour;
                    }
                    else
                    {
                        args.Graphics.FillRectangle(backBrush, args.Bounds);
                    }

                    if (isFocused)
                    {
                        ControlPaint.DrawFocusRectangle(args.Graphics, args.Bounds);
                    }

                    // Draw the top level category name
                    Rectangle drawRect = args.Bounds;
                    if (args.Node.Level == 0)
                    {
                        string nodeName = args.Node.Name;
                        SizeF textSize = args.Graphics.MeasureString(nodeName, _boldTopicNameFont);
                        Rectangle textRect = new Rectangle
                        {
                            X = drawRect.X + 2,
                            Y = (drawRect.Height - (int)textSize.Height) / 2 + drawRect.Y,
                            Width = (int)textSize.Width + 1,
                            Height = (int)textSize.Height + 1
                        };

                        using (SolidBrush textBrush = new SolidBrush(colour))
                        {
                            args.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                            args.Graphics.DrawString(nodeName, _boldTopicNameFont, textBrush, textRect);
                        }
                    }
                    else
                    {
                        // Draw the expander if this is a root
                        Image imageToDraw;
                        int imageX = (16 * args.Node.Level) + 2;
                        if (args.Node.Nodes.Count > 0)
                        {
                            imageToDraw = args.Node.IsExpanded ? Resources.downArrow : Resources.rightArrow;
                            int imageY = drawRect.Y + (drawRect.Height - imageToDraw.Height) / 2;
                            args.Graphics.DrawImage(imageToDraw, imageX, imageY, imageToDraw.Width, imageToDraw.Height);
                        }
                        drawRect.X += 18;
                        imageX = 18;

                        // Draw the image that appears to the left of the folder name.
                        if (folder.Flags.HasFlag(FolderFlags.JoinFailed))
                        {
                            // Show a warning icon next to folders where the join failed.
                            imageToDraw = Resources.Warning;
                            drawRect.X += imageToDraw.Width + 2;
                        }
                        else if (args.Node.Level == 0)
                        {
                            imageToDraw = folder.Icon;
                            drawRect.X += imageToDraw.Width + 2;
                        }
                        else
                        {
                            drawRect.X += (16*args.Node.Level);
                            imageX = drawRect.X + 2;
                            imageToDraw = folder.Icon;
                            drawRect.X += imageToDraw.Width + 2;
                        }
                        if (imageToDraw != null)
                        {
                            int imageY = drawRect.Y + (drawRect.Height - imageToDraw.Height) / 2;
                            args.Graphics.DrawImage(imageToDraw, imageX, imageY, 12, 12);
                        }

                        // Draw the folder name.
                        SizeF textSize = args.Graphics.MeasureString(folder.Name, _topicNameFont);
                        Rectangle textRect = new Rectangle
                        {
                            X = drawRect.X + 2,
                            Y = (drawRect.Height - (int)textSize.Height) / 2 + drawRect.Y,
                            Width = (int)textSize.Width + 1,
                            Height = (int)textSize.Height + 1
                        };

                        using (SolidBrush textBrush = new SolidBrush(colour))
                        {
                            args.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                            args.Graphics.DrawString(folder.Name, _topicNameFont, textBrush, textRect);
                        }

                        drawRect.X += (int)textSize.Width + 2;

                        bool isEndNode = (!args.Node.IsExpanded) || (args.Node.Nodes.Count == 0);

                        // Draw the count of unread only if we're at zero
                        if (isEndNode)
                        {
                            int folderCount = folder.Count;
                            if (folderCount > 0)
                            {
                                args.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                                string unreadCount = folderCount.ToString(CultureInfo.InvariantCulture);
                                textSize = args.Graphics.MeasureString(unreadCount, _unreadFont);

                                int height = (int) textSize.Height + 2;
                                int width = Math.Max(height, (int) textSize.Width + 2);
                                Rectangle countButton = new Rectangle
                                {
                                    X = drawRect.X,
                                    Y = drawRect.Y + (drawRect.Height - height)/2,
                                    Width = width,
                                    Height = height
                                };

                                Color buttonColour = (isFocused || isSelected)
                                    ? BackColor
                                    : UI.System.SelectionColour;
                                using (SolidBrush circleBrush = new SolidBrush(buttonColour))
                                {
                                    args.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    args.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    args.Graphics.FillEllipse(circleBrush, countButton);
                                }

                                int x = countButton.X + (countButton.Width - (int) textSize.Width)/2;
                                int y = countButton.Y + (countButton.Height - (int) textSize.Height)/2;
                                args.Graphics.DrawString(unreadCount, _unreadFont, backBrush, x + 1, y);
                            }
                        }

                        // Draw the context menu if applicable
                        if (args.Node == frmList.ContextNode)
                        {
                            args.Graphics.DrawImage(isSelected ? frmList.SelectedContextIcon : frmList.ContextIcon, frmList.ContextIconBounds(args.Node));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Persist the forums list splitter position when it is changed.
        /// </summary>
        private void frmSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!_initialising)
            {
                int messagePaneHeight = frmSplitContainer.Width;
                Preferences.StandardPreferences.FolderSplitPosition = (float)((100.0 / messagePaneHeight) * frmSplitContainer.SplitterDistance);
            }
        }

        /// <summary>
        /// Resize the sub-view when the parent view changes size.
        /// </summary>
        private void frmSplitContainer_Panel2_SizeChanged(object sender, EventArgs e)
        {
            if (_currentView != null)
            {
                SetSubviewSize();
            }
        }

        /// <summary>
        /// Resize the subview to fill the subview area in the forum view.
        /// </summary>
        private void SetSubviewSize()
        {
            Rectangle clientBounds = frmSplitContainer.Panel2.Bounds;
            _currentView.SetBounds(0, 0, clientBounds.Width, clientBounds.Height);
        }

        /// <summary>
        /// Handle the event raised when the folders tree view size is changed. We
        /// hook into this to reposition the progress control's position so that it
        /// remains centered within the folders tree control.
        /// </summary>
        private void frmList_SizeChanged(object sender, EventArgs e)
        {
            frmProgress.Left = (frmList.Width - frmProgress.Width) / 2;
            frmProgress.Top = (frmList.Height - frmProgress.Height) / 2;
        }

        /// <summary>
        /// Start the progress spinner only if the root list is empty.
        /// </summary>
        private void StartProgressBar()
        {
            if (frmList.Nodes.Count == 0)
            {
                frmMessage.Hide();
                frmProgress.Start();
                frmProgress.Visible = true;
            }
        }

        /// <summary>
        /// Stop the progress spinner.
        /// </summary>
        private void StopProgressBar()
        {
            frmProgress.Stop();
            frmProgress.Visible = false;
        }

        /// <summary>
        /// Start a drag and drop action.
        /// </summary>
        private void frmList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _nodeBeingDragged = (TreeNode)e.Item;
            DoDragDrop(_nodeBeingDragged, DragDropEffects.Move);
        }

        /// <summary>
        /// Handle a drag enter on the treeview. Indicate that the action is a move.
        /// </summary>
        private void frmList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Handle a drop on the node. Two types of drops are handled depending on
        /// the type of the data in e.Data:
        /// 
        /// If the item is a ListViewItem then it is assumed to be a drag from the
        /// thread list to another topic. In this case, create a new message in the
        /// target topic which indicates that it is a copy (the user can edit the
        /// "***COPIED FROM" header out anyway.
        /// 
        /// If the item is a TreeNode then it is assumed to be a drag and drop
        /// re-arrangement of the nodes in the tree.
        /// </summary>
        private void frmList_DragDrop(object sender, DragEventArgs e)
        {
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = ((TreeView)sender).GetNodeAt(pt);

            if (targetNode != null)
            {
                if (e.Data.GetDataPresent(typeof (ListViewItem)))
                {
                    ListViewItem item = e.Data.GetData(typeof (ListViewItem)) as ListViewItem;
                    if (item != null)
                    {
                        if (targetNode == _nodeAtDragPoint)
                        {
                            _nodeAtDragPoint = null;
                            frmList.InvalidateNode(targetNode);
                        }
                        FolderBase folderBase = (FolderBase)targetNode.Tag;
                        if (folderBase.ID > 0)
                        {
                            string body = BodyFromDropSource(item.Tag);

                            TopicFolder topicFolder = (TopicFolder)folderBase;
                            CIXMessage message = new CIXMessage
                            {
                                Author = CIX.Username,
                                RemoteID = 0,
                                Priority = true,
                                Date = DateTime.Now,
                                Body = body,
                                TopicID = topicFolder.ID,
                                RootID = 0,
                                CommentID = 0
                            };

                            CIXMessageEditor editor = MessageEditorCollection.Get(message) ?? new CIXMessageEditor(message, false);
                            editor.Show();
                            editor.BringToFront();
                        }
                    }
                    return;
                }
                if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
                {
                    TreeNode newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                    _nodeAtDragPoint = null;

                    if (targetNode == _nodeBeingDragged)
                    {
                        frmList.InvalidateNode(targetNode);
                    }
                    else
                    {
                        TreeNodeCollection parentNodes;
                        int insertIndex;

                        if (targetNode.Parent == _nodeBeingDragged.Parent)
                        {
                            parentNodes = (targetNode.Parent != null) ? targetNode.Parent.Nodes : _forumsTree.Nodes;
                            insertIndex = parentNodes.IndexOf(targetNode) + 1;
                        }
                        else
                        {
                            parentNodes = targetNode.Nodes;
                            insertIndex = 0;
                        }

                        TreeNode insertedNode = (TreeNode) newNode.Clone();
                        parentNodes.Insert(insertIndex, insertedNode);
                        SelectFolder(insertedNode, FolderOptions.None);
                        newNode.Remove();

                        // Fix up the indexes for all subsequent items
                        FixupNodeIndexes(_forumsTree.Nodes);
                    }
                }
            }
        }

        /// <summary>
        /// Given a drop source, create the body of a new message to be posted to
        /// a topic using the contents of the original message.
        /// </summary>
        /// <param name="tag">The drop source object</param>
        /// <returns>A string containing the body of the new message</returns>
        private static string BodyFromDropSource(object tag)
        {
            StringBuilder body = new StringBuilder();
            if (tag is CIXMessage)
            {
                CIXMessage sourceMessage = (CIXMessage)tag;
                Folder topic = sourceMessage.Topic;
                Folder forum = topic.ParentFolder;

                body.AppendFormat("***COPIED FROM: >>>{0}/{1} {2} ", forum.Name, topic.Name,
                    sourceMessage.RemoteID);
                body.AppendFormat("{0}({1})", sourceMessage.Author, sourceMessage.Body.Length);
                body.AppendFormat("{0} ", sourceMessage.Date.ToString("ddMMMyyyy HH:MM"));

                if (sourceMessage.CommentID > 0)
                    body.AppendFormat("c{0}", sourceMessage.CommentID);

                body.AppendLine();
                body.AppendLine(sourceMessage.Body.FixNewlines());
            }
            else if (tag is InboxConversation)
            {
                InboxConversation conv = (InboxConversation) tag;

                body.AppendFormat("***COPIED FROM PMESSAGE: >>>{0} ", conv.Author);
                body.AppendFormat("{0}({1})", conv.Author, conv.Subject);
                body.AppendFormat("{0} ", conv.Date.ToString("ddMMMyyyy HH:MM"));

                body.AppendLine();

                foreach (InboxMessage message in conv.Messages)
                {
                    body.AppendLine(message.Body.FixNewlines());
                }
            }
            return body.ToString();
        }

        /// <summary>
        /// Handle the drag over an item and update _dragNode so the paint logic can highlight
        /// it in a distinct colour. Also handle movement off the top or bottom of the folder
        /// list to scroll the list down or up.
        /// </summary>
        private void frmList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof (ListViewItem)))
            {
                HandleMessageDragAndDrop(e);
                return;
            }

            Point pt = frmList.PointToClient(new Point(e.X, e.Y));
            if ((pt.Y + 20) > frmList.Height)
            {
                Platform.ScrollWindow(frmList.Handle, 1);
            }
            else if (pt.Y < 20)
            {
                Platform.ScrollWindow(frmList.Handle, 0);
            }
            
            TreeNode node = frmList.GetNodeAt(pt);
            if (node != _nodeAtDragPoint)
            {
                TreeNode currentDragNode = _nodeAtDragPoint;
                _nodeAtDragPoint = null;

                if (currentDragNode != null)
                {
                    frmList.InvalidateNode(currentDragNode);
                }

                // Make sure the node under the cursor is legit for a drop
                if (node == null)
                {
                    e.Effect = DragDropEffects.None;
                }
                else
                {
                    FolderBase folder = (FolderBase)node.Tag;
                    if ((node.Parent == _nodeBeingDragged.Parent || node == _nodeBeingDragged.Parent) && folder.ID >= -1)
                    {
                        _nodeAtDragPoint = node;
                        frmList.InvalidateNode(_nodeAtDragPoint);
                        e.Effect = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    node.EnsureVisible();
                }
            }
        }

        /// <summary>
        /// Scan the current organisation of nodes and fix up the indexes so we can
        /// persist the arrangement to the database and restore it later.
        /// </summary>
        private static void FixupNodeIndexes(IEnumerable nodes)
        {
            int index = 0;
            foreach (TreeNode node in nodes)
            {
                FolderBase folderBase = (FolderBase)node.Tag;
                if (folderBase.ID > 0)
                {
                    TopicFolder folder = (TopicFolder)folderBase;
                    if (folder.Folder.Index != index)
                    {
                        folder.Folder.Index = index;
                        folder.Folder.IsModified = true;
                    }
                    ++index;
                }

                if (node.Nodes.Count > 0)
                {
                    FixupNodeIndexes(node.Nodes);
                }
            }
        }

        /// <summary>
        /// Handle a drag from the canvas control. Only permit a copy if the drag is over a
        /// topic folder.
        /// 
        /// TODO: handle scrolling the folder list if we go off the top or bottom!
        /// </summary>
        /// <param name="e">Drag event arguments</param>
        private void HandleMessageDragAndDrop(DragEventArgs e)
        {
            TreeNode node = frmList.GetNodeAt(frmList.PointToClient(new Point(e.X, e.Y)));
            if (node != null && node != _nodeAtDragPoint)
            {
                TreeNode currentDragNode = _nodeAtDragPoint;
                _nodeAtDragPoint = null;

                if (currentDragNode != null)
                {
                    frmList.InvalidateNode(currentDragNode);
                }

                FolderBase folderBase = (FolderBase)node.Tag;
                if (folderBase.ID > 0)
                {
                    TopicFolder folder = (TopicFolder)folderBase;
                    if (!folder.Folder.IsRootFolder)
                    {
                        _nodeAtDragPoint = node;
                        frmList.InvalidateNode(_nodeAtDragPoint);

                        e.Effect = DragDropEffects.Link;
                        return;
                    }
                }

                _nodeAtDragPoint = null;
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Prohibit selection of category nodes.
        /// </summary>
        private void frmList_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node != null && (e.Node.Level == 0 || _isRBut))
            {
                e.Cancel = true;
            }
        }

        private void frmList_MouseDown(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo hti = frmList.HitTest(e.Location);

            _isRBut = e.Button == MouseButtons.Right;
            if (_isRBut)
            {
                if (hti.Location == TreeViewHitTestLocations.Label || hti.Location == TreeViewHitTestLocations.RightOfLabel)
                {
                    _contextMenuNode = hti.Node;
                    UpdateContextMenu();
                    frmContextMenu.Show(frmList, e.Location);
                }
            }
            else if (hti.Location == TreeViewHitTestLocations.PlusMinus)
            {
                _isRBut = true;
            }
    }

        private void frmList_MouseUp(object sender, MouseEventArgs e)
        {
            _isRBut = e.Button == MouseButtons.Right;
        }

        /// <summary>
        /// Handle a click to determine if we clicked on the context icon and, if so,
        /// handle the context icon action.
        /// </summary>
        private void frmList_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNode node = frmList.GetNodeAt(e.Location);
            Rectangle nodeRect = frmList.ContextIconBounds(node);

            var hitTest = node.TreeView.HitTest(e.Location);
            if (hitTest.Location == TreeViewHitTestLocations.PlusMinus)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (node.Level == 0 && node.Nodes.Count > 0)
            {
                // Click to expand/collapse category headers which have
                // child items.
                if (node.IsExpanded)
                {
                    node.Collapse();
                }
                else
                {
                    node.Expand();
                }
            }
            else if (nodeRect.Contains(e.Location))
            {
                _contextMenuNode = node;
                UpdateContextMenu();
                frmContextMenu.Show(frmList, new Point(nodeRect.Left, nodeRect.Bottom), ToolStripDropDownDirection.BelowRight);
            }
            else if (node != _selectedNode && !_isRBut)
            {
                _contextMenuNode = null;
                FolderBase folderBase = (FolderBase)node.Tag;
                if (folderBase.ID > 0)
                {
                    TopicFolder topicFolder = (TopicFolder)folderBase;
                    if (topicFolder.Folder.IsRootFolder && e.X < 16)
                    {
                        return; // Clicking on the expand arrow.
                    }
                }

                SelectFolder(node, FolderOptions.None);

                // Bit of a hack to switch the focus AFTER the mouse click has finished
                // processing.
                Delay(100, (o, a) => _currentView.SetFocus());
            }
        }

        /// <summary>
        /// Executes the specified action after the given delay.
        /// </summary>
        static void Delay(int ms, EventHandler action)
        {
            var tmp = new Timer { Interval = ms };
            tmp.Tick += (o, e) => tmp.Enabled = false;
            tmp.Tick += action;
            tmp.Enabled = true;
        }

        /// <summary>
        /// Handle the case of the drag cursor being moved outside the client area
        /// in which case we need to cancel the drag node as otherwise we won't get
        /// a dragdrop event when the drag ends.
        /// </summary>
        private void frmList_DragLeave(object sender, EventArgs e)
        {
            TreeNode currentDragNode = _nodeAtDragPoint;
            _nodeAtDragPoint = null;

            if (currentDragNode != null)
            {
                frmList.InvalidateNode(currentDragNode);
            }
        }

        /// <summary>
        /// Handle the Mark Everything Read context menu item.
        /// </summary>
        private void frmMarkAllRead_Click(object sender, EventArgs e)
        {
            Action(ActionID.MarkTopicRead);
        }

        /// <summary>
        /// Switch the view to show only recent topics.
        /// </summary>
        private void frmRecentTopics_Click(object sender, EventArgs e)
        {
            ChangeFolderListDisplay(false);
        }

        /// <summary>
        /// Switch the view to show all topics.
        /// </summary>
        private void frmAllTopics_Click(object sender, EventArgs e)
        {
            ChangeFolderListDisplay(true);
        }

        /// <summary>
        /// Resign the selected forum or topic.
        /// </summary>
        private void frmResign_Click(object sender, EventArgs e)
        {
            MainForm.Action(ActionID.ResignForum);
        }

        /// <summary>
        /// Refresh the selected topic or forum.
        /// </summary>
        private void frmRefresh_Click(object sender, EventArgs e)
        {
            MainForm.Action(ActionID.Refresh);
        }

        /// <summary>
        /// Delete the selected topic or forum.
        /// </summary>
        private void frmDelete_Click(object sender, EventArgs e)
        {
            MainForm.Action(ActionID.Delete);
        }

        /// <summary>
        /// Display the participants list for the selected topic or forum.
        /// </summary>
        private void frmParticipants_Click(object sender, EventArgs e)
        {
            MainForm.Action(ActionID.Participants);
        }

        /// <summary>
        /// Manage the active forum.
        /// </summary>
        private void frmManage_Click(object sender, EventArgs e)
        {
            MainForm.Action(ActionID.ManageForum);
        }
    }
}