// *****************************************************
// CIXReader
// MainForm.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 29/08/2013 5:17 PM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using CIXClient;
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
    /// The MainForm is the main program form.
    /// </summary>
    public sealed partial class MainForm : Form
    {
        private readonly CRPanel _shortcutBar;
        private readonly Panel _viewPanel;
        private readonly CRToolbar _toolbar;
        private readonly PictureBox _logonImage;
        private readonly PictureBox _controlMenu;
        private readonly CRLabel _logonName;
        private readonly Panel _mainPanel;
        private readonly FoldersTree _foldersTree;

        private readonly List<CRLabel> _labels;

        private PrintDocument _printDoc;

        private ProfileView _profileViewer;
        private KeyboardHelp _keyHelpDialog;

        private readonly ScriptManager _scriptManager;

        private int _progressCount;
        private string _userTitleString;

        // Maximum size of the backtrack stack
        private const int _backtrackLimit = 20;

        // Backtrack queue
        private bool _isBacktracking;
        private readonly BackTrackArray _backtrackQueue = new BackTrackArray(_backtrackLimit);

        /// <summary>
        /// Custom main menu bar renderer to ensure that top level menu items that
        /// are pressed are rendered clearly when the foreground colour on the
        /// background is the same as the highlight colour.
        /// </summary>
        private sealed class MainMenuBarRenderer : ToolStripProfessionalRenderer
        {
            #if __MonoCS__
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                using (SolidBrush backBrush = new SolidBrush(UI.Menu.BackgroundColour))
                {
                    e.Graphics.FillRectangle(backBrush, e.AffectedBounds);
                }
            }
            #endif
            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                if (!e.Item.IsOnDropDown && e.Item.Pressed)
                {
                    Color pressedColor = SystemColors.MenuText;
                    TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, e.TextRectangle, pressedColor, e.TextFormat);
                }
                else
                {
                    base.OnRenderItemText(e);
                }
            }
        }

        /// <summary>
        /// Constructs a MainForm instance.
        /// </summary>
        internal MainForm()
        {
            InitializeComponent();

            _scriptManager = new ScriptManager(this);

            Initialise();

            _mainPanel = new Panel
            {
                Parent = this,
                Location = new Point(0, 0),
                Size = new Size(ClientRectangle.Width, ClientRectangle.Height),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
            };
            Controls.Add(_mainPanel);

            Image cixLogoImage = Resources.CIXLogo2;

            _isBacktracking = true;

            Font titleFont = UI.Menu.Font;
            int menuHeight = Math.Max(titleFont.Height + 4, 42);

            // Top level menu panel
            _shortcutBar = new CRPanel
            {
                Parent = _mainPanel,
                Location = new Point(0, 0),
                Gradient = true,
                Size = new Size(_mainPanel.Width, menuHeight),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                BackColor = UI.Menu.BackgroundColour
            };
            _mainPanel.Controls.Add(_shortcutBar);

            // Add the CIX logo to the left edge of the shortcut bar
            PictureBox cixLogo = new PictureBox
            {
                Image = cixLogoImage,
                Location = new Point(4, (menuHeight - cixLogoImage.Height) / 2),
                Name = "cixlogo",
                Parent = _shortcutBar,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Visible = true,
                Size = cixLogoImage.Size
            };
            cixLogo.Click += delegate { Address = "welcome"; };
            _shortcutBar.Controls.Add(cixLogo);

            _labels = new List<CRLabel>();

            AddShortcut(Resources.Forums, Resources.ForumsTooltip, delegate { Address = "cix:_lastview"; });
            AddShortcut(Resources.PMessages, Resources.PMessagesTooltip, delegate { Address = "cixmailbox:inbox"; });
            AddShortcut(Resources.Directory, Resources.DirectoryTooltip, delegate { Address = "cixdirectory:all"; });
            AddShortcut(Resources.Support, Resources.SupportTooltip, delegate { Address = "cix:" + Constants.SupportForum; });

            if (Preferences.StandardPreferences.UseBeta)
            {
                AddShortcut(Resources.Beta, Resources.BetaTooltip, delegate { Address = "cix:" + Constants.BetaForum; });
            }

            // Add the control menu to the extreme right edge of the shortcut bar
            Image controlImage = Resources.Control;
            _controlMenu = new PictureBox
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Image = controlImage,
                Location = new Point(_mainPanel.Width - (controlImage.Width + 2), (menuHeight - controlImage.Height) / 2),
                Name = "controlMenu",
                Parent = _shortcutBar,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Visible = true,
                Size = controlImage.Size
            };
            _controlMenu.Click += ControlMenuOnClick;
            _controlMenu.MouseEnter += ControlMenuMouseEnter;
            _controlMenu.MouseLeave += ControlMenuMouseLeave;

            // Add the user's mugshot image. We don't have it yet. We'll
            // get it once the database is ready.
            Size imageSize = new Size(menuHeight - 4, menuHeight - 4);
            _logonImage = new PictureBox
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(_mainPanel.Width - (imageSize.Width + controlImage.Width + 2), 2),
                Name = "logonImage",
                Parent = _shortcutBar,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Visible = true,
                Size = imageSize
            };

            // Add the user's name to the left of the image.
            Font logonNameFont = UI.GetFont(UI.Menu.Font, UIConfigMenu.FontSize - 10);
            string logonText = CIX.Username;
            SizeF nameSize = CreateGraphics().MeasureString(logonText, logonNameFont);

            int xPosition = _labels[_labels.Count - 1].Right;
            _logonName = new CRLabel
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                AutoSize = true,
                Padding = new Padding(0, 1, 0, 0),
                Font = logonNameFont,
                ForeColor = UI.Menu.ForegroundColour,
                InactiveColour = UI.Menu.ForegroundColour,
                BackColor = Color.Transparent,
                Location = new Point(xPosition, (int)(menuHeight - nameSize.Height) / 2),
                Size = new Size(_logonImage.Left - xPosition, (int)nameSize.Height),
                Parent = _shortcutBar,
                Visible = true,
                Text = logonText
            };
            _shortcutBar.Controls.Add(_logonName);

            // Top level menu panel
            int dpi = GetDPI();
            _toolbar = new CRToolbar
            {
                Parent = _mainPanel,
                Location = new Point(0, menuHeight),
                Size = new Size(_mainPanel.Width, Math.Max(dpi / 3, 36)),
                Visible = false
            };

            // Add the panel for the view
            _viewPanel = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(0, menuHeight),
                Size = new Size(ClientRectangle.Width, ClientRectangle.Height - menuHeight),
                Margin = new Padding(0),
                Name = "viewPanel",
                Tag = this,
                Parent = _mainPanel,
                TabIndex = _labels.Count
            };

            // Add the folders tree portion
            _foldersTree = new FoldersTree(this)
                {
                    TopLevel = false,
                    Location = new Point(0, 0),
                    Parent = _viewPanel,
                    Size = _viewPanel.Size
                };

            _mainPanel.Controls.Add(_viewPanel);
            _mainPanel.Controls.Add(_toolbar);

            // Good time to show the status bar if needed.
            mainStatusBar.ForeColor = UI.System.EdgeColour;
            mainStatusBar.BackColor = UI.System.InfoBarColour;

            mainMenubar.BackColor = UI.Menu.BackgroundColour;
            mainMenubar.ForeColor = UI.Menu.ForegroundColour;
            mainMenubar.Renderer = new MainMenuBarRenderer();

            ShowStatusBar(Preferences.StandardPreferences.ViewStatusBar);
            ShowMenuBar(Preferences.StandardPreferences.ViewMenuBar);
            ShowToolbar(Preferences.StandardPreferences.ShowToolBar);

            // Set the service online state now
            ChangeOnlineState(NetworkInterface.GetIsNetworkAvailable() && Program.StartupOnline);
        }

        /// <summary>
        /// Get or set the user title string that displays on the main window title bar
        /// </summary>
        public string UserTitleString
        {
            get { return _userTitleString; }
            set
            {
                if (value != _userTitleString)
                {
                    _userTitleString = value;
                    UpdateCaption();
                }
            }
        }

        /// <summary>
        /// Get or return the active URL.
        /// </summary>
        public string Address
        {
            set
            {
                if (value != null)
                {
                    Address address = new Address(value);

                    // Special case URI to select the last topic displayed in the
                    // topic view or a suitable default.
                    if (address.SchemeAndQuery == "cix:_lastview")
                    {
                        address = _foldersTree.AddressFromView(AppView.AppViewTopic);
                        if (string.IsNullOrEmpty(address.Query))
                        {
                            // Pick a suitable default forum and topic.
                            address = new Address("cix:cixnews/announce");
                        }
                    }

                    if (address.Scheme == "cixmail")
                    {
                        InboxMessageEditor newMessageWnd = new InboxMessageEditor(address.Query);
                        newMessageWnd.Show();
                        return;
                    }

                    if (address.Scheme == "cixuser")
                    {
                        Profile profile = Profile.ProfileForUser(address.Query);
                        ShowProfile(profile);
                        return;
                    }

                    if (address.Scheme != "welcome" && string.IsNullOrEmpty(address.Query))
                    {
                        string oldAddress = address.Data;
                        TreeNode node = _foldersTree.SelectedNode;
                        if (node != null)
                        {
                            FolderBase folder = (FolderBase)node.Tag;
                            string newAddress = folder.Address;
                            address = new Address(newAddress) { Data = oldAddress };
                        }
                    }

                    if (address.Scheme != null)
                    {
                        // Special case the cix:/topic convention here because it requires us to
                        // know the forum from the selection which is only possible when we have
                        // the UI from which to determine the selection.
                        if (address.Query != null && address.Query.StartsWith("/", StringComparison.Ordinal))
                        {
                            TreeNode node = _foldersTree.SelectedNode;
                            if (node != null)
                            {
                                FolderBase folder = (FolderBase)node.Tag;
                                if (folder is TopicFolder && !((TopicFolder)folder).Folder.IsRootFolder)
                                {
                                    Folder forum = ((TopicFolder)folder).Folder.ParentFolder;
                                    address.Query = string.Format("{0}{1}", forum.Name, address.Query);
                                }
                            }
                        }
                    }

                    _foldersTree.SetAddress(address);
                }
            }
        }

        /// <summary>
        /// Scripting: Function to display a message box.
        /// </summary>
        /// <param name="line">Message to display</param>
        // ReSharper disable UnusedMember.Global
        public void Message(string line)
        // ReSharper restore UnusedMember.Global
        {
            MessageBox.Show(line);
        }

        /// <summary>
        /// Scripting: Return the FoldersTree object.
        /// </summary>
        // ReSharper disable UnusedMember.Global
        public FoldersTree FoldersTree
        // ReSharper restore UnusedMember.Global
        {
            get { return _foldersTree; }
        }

        /// <summary>
        /// Return the global PrintDocument object
        /// </summary>
        public PrintDocument PrintDocument
        {
            get { return _printDoc ?? (_printDoc = new PrintDocument()); }
        }

        /// <summary>
        /// Return the Sort By Topic menu
        /// </summary>
        public ContextMenuStrip TopicSortMenu
        {
            get { return menuViewSortByTopic; }
        }

        /// <summary>
        /// Return the Sort By Directory menu
        /// </summary>
        public ContextMenuStrip DirectorySortMenu
        {
            get { return menuViewSortByDirectory; }
        }

        /// <summary>
        /// Show or hide the status bar.
        /// </summary>
        /// <param name="show">True to show, false to hide</param>
        public void ShowStatusBar(bool show)
        {
            bool wasVisible = mainStatusBar.Visible;
            mainStatusBar.Visible = show;
            if (show && !wasVisible)
            {
                mainStatusBar.Location = new Point(0, ClientRectangle.Height - mainStatusBar.Height);
                mainStatusBar.Size = new Size(_mainPanel.Width, mainStatusBar.Height);
                _mainPanel.Height = _mainPanel.Height - mainStatusBar.Height;
            }
            if (!show && wasVisible)
            {
                _mainPanel.Height = _mainPanel.Height + mainStatusBar.Height;
            }
            _foldersTree.Size = _viewPanel.Size;
        }

        /// <summary>
        /// Show or hide the menu bar.
        /// </summary>
        /// <param name="show">True to show, false to hide</param>
        public void ShowMenuBar(bool show)
        {
            bool wasVisible = mainMenubar.Visible;
            mainMenubar.Visible = show;
            if (show && !wasVisible)
            {
                _mainPanel.Top += mainMenubar.Height;
                _mainPanel.Height = _mainPanel.Height - mainMenubar.Height;
            }
            if (!show && wasVisible)
            {
                _mainPanel.Top -= mainMenubar.Height;
                _mainPanel.Height = _mainPanel.Height + mainMenubar.Height;
            }
            _foldersTree.Size = _viewPanel.Size;
        }

        /// <summary>
        /// Show or hide the toolbar
        /// </summary>
        /// <param name="show">True to show, false to hide</param>
        public void ShowToolbar(bool show)
        {
            bool wasVisible = _toolbar.Visible;
            _toolbar.Visible = show;
            if (show && !wasVisible)
            {
                _toolbar.Top = _shortcutBar.Bottom;
                _viewPanel.Top += _toolbar.Height;
                _viewPanel.Height -= _toolbar.Height;
            }
            if (!show && wasVisible)
            {
                _viewPanel.Top -= _toolbar.Height;
                _viewPanel.Height += _toolbar.Height;
            }
            _foldersTree.Size = _viewPanel.Size;
        }

        /// <summary>
        /// Set the search bar placeholder string.
        /// </summary>
        public void SetSearchFieldPlaceholder(string placeholder)
        {
            CRToolbarItem item = _toolbar.ItemWithID(ActionID.Search);
            if (item != null)
            {
                CRSearchField searchField = (CRSearchField)item.Control;

                searchField.PlaceholderText = placeholder;
                searchField.Text = string.Empty;
            }
        }

        /// <summary>
        /// Start the progress spinner on the status bar.
        /// </summary>
        public void StartStatusProgressSpinner(string statusText)
        {
            if (_progressCount == 0)
            {
                mainProgress.Show();
                mainProgress.Start();
                mainStatusText.Show();
            }
            mainStatusText.Text = statusText ?? String.Empty;
            ++_progressCount;
        }

        /// <summary>
        /// Stop the progress spinner on the status bar.
        /// </summary>
        public void StopStatusProgressSpinner()
        {
            if (_progressCount > 0)
            {
                --_progressCount;
                if (_progressCount == 0)
                {
                    mainProgress.Stop();
                    mainProgress.Hide();
                    mainStatusText.Hide();
                }
            }
        }

        /// <summary>
        /// Update the total unread count displayed on labels on the shortcut bar.
        /// </summary>
        public void UpdateTotalUnreadCount()
        {
            Platform.UIThread(this, delegate
            {
                CRLabel forumLabel = LabelByName(Resources.Forums);
                forumLabel.Count = CIX.FolderCollection.TotalUnread;

                CRLabel pMessageLabel = LabelByName(Resources.PMessages);
                pMessageLabel.Count = CIX.ConversationCollection.TotalUnread;

                CRLabel supportLabel = LabelByName(Resources.Support);
                Folder supportTopic = CIX.FolderCollection.Get(Constants.SupportForum);
                if (supportTopic != null)
                {
                    supportLabel.Count = supportTopic.Unread;
                }

                CRLabel betaLabel = LabelByName(Resources.Beta);
                if (betaLabel != null)
                {
                    Folder betaTopic = CIX.FolderCollection.Get(Constants.BetaForum);
                    if (betaTopic != null)
                    {
                        betaLabel.Count = betaTopic.Unread;
                    }
                }

                RefreshShortcutBar();
                _toolbar.RefreshButtons();
            });
        }

        /// <summary>
        /// Run the specified script.
        /// </summary>
        /// <param name="scriptFile">Script file name</param>
        public void RunScript(string scriptFile)
        {
            _scriptManager.RunScript(scriptFile);
        }

        /// <summary>
        /// Invokes the specified event interface in each script passing through the
        /// event data. Each event handler must return true to allow the event to continue.
        /// If any returns false, the event is abandoned.
        /// </summary>
        public bool RunEvent(EventID eventId, object data)
        {
            string eventName = string.Format("on_{0}", eventId);
            return _scriptManager.RunEvents(eventName, data);
        }

        /// <summary>
        /// Push the current selected message onto the backtrack stack, ensuring
        /// the stack remains at the fixed capacity.
        /// </summary>
        public void AddBacktrack(string address, bool unread)
        {
            if (address != null && _isBacktracking)
            {
                _backtrackQueue.AddToQueue(new Address(address) { Unread = unread });
            }
        }

        /// <summary>
        /// Suspend backtracking.
        /// </summary>
        public void SuspendTracking()
        {
            _isBacktracking = false;
        }

        /// <summary>
        /// Resume backtracking.
        /// </summary>
        public void ResumeTracking()
        {
            _isBacktracking = true;
        }

        /// <summary>
        /// Back track to the previously visited message.
        /// </summary>
        public void GoBack()
        {
            Address item = null;

            if (_backtrackQueue.PreviousItemAtQueue(ref item))
            {
                SuspendTracking();
                _foldersTree.SetAddress(item);
                ResumeTracking();
            }
        }

        public bool CanGoBack()
        {
            return !_backtrackQueue.IsAtStartOfQueue;
        }

        /// <summary>
        /// Go forward in the backtrack queue.
        /// </summary>
        public void GoForward()
        {
            Address item = null;

            if (_backtrackQueue.NextItemAtQueue(ref item))
            {
                SuspendTracking();
                _foldersTree.SetAddress(item);
                ResumeTracking();
            }
        }

        public bool CanGoForward()
        {
            return !_backtrackQueue.IsAtEndOfQueue;
        }

        /// <summary>
        /// Handle keyboard interaction.
        /// </summary>
        public bool HandleFormKeyDown(Keys keyData)
        {
            if (ActiveControl is CRSearchField)
            {
                return false;
            }
            ActionID actionID = UI.MapKeyToAction(keyData);
            if (actionID != ActionID.None)
            {
                // Backspace
                switch (actionID)
                {
                    case ActionID.SystemMenu:
                        ControlMenuOnClick(this, null);
                        return true;

                    case ActionID.ViewAccounts:
                        menuCIXReaderAccount_Click(this, null);
                        return true;

                    default:
                        Action(actionID);
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Top level action handler.
        /// </summary>
        public void Action(ActionID id)
        {
            switch (id)
            {
                case ActionID.Offline:
                    ChangeOnlineState(!CIX.Online);
                    LogFile.WriteLine("User changed online state to {0}", CIX.Online);
                    break;

                case ActionID.BackTrack:
                    GoBack();
                    break;

                case ActionID.Search:
                    ActivateSearch();
                    break;

                case ActionID.NextPriorityUnread:
                    _foldersTree.NextUnread(FolderOptions.NextUnread | FolderOptions.Priority);
                    break;

                case ActionID.NextUnread:
                    _foldersTree.NextUnread(FolderOptions.NextUnread);
                    break;

                case ActionID.Markdown:
                    {
                        bool disableMarkup = Preferences.StandardPreferences.IgnoreMarkup;
                        Preferences.StandardPreferences.IgnoreMarkup = !disableMarkup;
                        break;
                    }

                case ActionID.KeyboardHelp:
                    if (_keyHelpDialog == null)
                    {
                        _keyHelpDialog = new KeyboardHelp();
                    }
                    _keyHelpDialog.Show();
                    _keyHelpDialog.BringToFront();
                    break;

                case ActionID.ViewChangeLog:
                    {
                        string releaseOrBeta = Preferences.StandardPreferences.UseBeta ? "beta" : "release";
                        LaunchURL(string.Format(Constants.ChangeLogURL, releaseOrBeta));
                        break;
                    }

                case ActionID.JoinForum:
                    if (_foldersTree.CanAction(ActionID.JoinForum))
                    {
                        _foldersTree.Action(ActionID.JoinForum);
                    }
                    else
                    {
                        JoinForumInput joinInput = new JoinForumInput();
                        if (joinInput.ShowDialog() == DialogResult.OK)
                        {
                            JoinForum joinForum = new JoinForum(joinInput.ForumName);
                            joinForum.ShowDialog();
                        }
                    }
                    break;

                default:
                    _foldersTree.Action(id);
                    break;
            }
            _toolbar.RefreshButtons();
        }

        /// <summary>
        /// Put the focus in the search field if there is one.
        /// </summary>
        public void ActivateSearch()
        {
            CRToolbarItem item = _toolbar.ItemWithID(ActionID.Search);
            if (item != null)
            {
                CRSearchField searchField = (CRSearchField)item.Control;
                if (searchField != null && searchField.Visible)
                {
                    ActiveControl = searchField;
                }
            }
        }

        /// <summary>
        /// Launch the specified URL in the default browser.
        /// </summary>
        /// <param name="url"></param>
        public void LaunchURL(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Win32Exception e)
            {
                string errorString = String.Format(Resources.CannotOpen, url, e.Message);
                MessageBox.Show(errorString, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Process top level keystrokes to handle the view switching keys.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return HandleFormKeyDown(keyData) || base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Add a shortcut to the shortcut bar.
        /// </summary>
        private void AddShortcut(string titleText, string tooltipString, EventHandler handler)
        {
            using (Graphics graphics = CreateGraphics())
            {
                int count = _shortcutBar.Controls.Count;
                int index = count - 1;
                int xPosition = _shortcutBar.Controls[count - 1].Right + 4;
                string name = titleText;

                if (index > 0)
                {
                    titleText = "•  " + titleText;
                }
                Font titleFont = UI.Menu.Font;
                int menuHeight = Math.Max(titleFont.Height + 4, 42);
                SizeF size = graphics.MeasureString(titleText, titleFont);

                CRLabel menuLabel = new CRLabel
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Top,
                    AutoSize = true,
                    Selected = true,
                    CanBeSelected = true,
                    Padding = new Padding(0, 0, 0, 0),
                    Font = titleFont,
                    BackColor = Color.Transparent,
                    ForeColor = UI.Menu.ForegroundColour,
                    InactiveColour = UI.Menu.InactiveColour,
                    CountColour = UI.System.CountColour,
                    CountTextColour = UI.System.CountTextColour,
                    MaxCount = Preferences.StandardPreferences.MaxCount,
                    Location = new Point(xPosition, (int)(menuHeight - size.Height) / 2),
                    Name = name,
                    TabIndex = index,
                    Parent = _shortcutBar,
                    Visible = true,
                    Text = titleText
                };

                menuLabel.Click += handler;

                ToolTip newToolTip = new ToolTip();
                newToolTip.SetToolTip(menuLabel, tooltipString);

                _labels.Add(menuLabel);
            }
        }

        /// <summary>
        /// Return the shortcut label control given its name.
        /// </summary>
        private CRLabel LabelByName(string name)
        {
            return _labels.FirstOrDefault(label => label.Name == name);
        }

        /// <summary>
        /// Return the DPI for the current display.
        /// </summary>
        private int GetDPI()
        {
            float dy;

            Graphics g = CreateGraphics();
            try
            {
                dy = g.DpiY;
            }
            finally
            {
                g.Dispose();
            }
            return (int)dy;
        }

        /// <summary>
        /// Trigger on action in the search field.
        /// </summary>
        private void SearchForString(object sender, EventArgs e)
        {
            CRSearchField searchField = (CRSearchField)sender;
            if (searchField != null)
            {
                string searchText = searchField.Text.Trim();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    _foldersTree.Search(searchText);
                }
            }
        }

        /// <summary>
        /// Clears the search field.
        /// </summary>
        private void ClearSearchField(object sender, EventArgs e)
        {
            TreeNode node = _foldersTree.SelectedNode;
            _foldersTree.SelectViewForFolder(node, null, FolderOptions.ClearFilter);
        }

        /// <summary>
        /// Drop down the system menu when the user clicks on the system control.
        /// </summary>
        private void ControlMenuOnClick(object sender, EventArgs eventArgs)
        {
            mainPopupMenu.Show(_controlMenu, new Point(_controlMenu.Width, _shortcutBar.Height - _controlMenu.Top), ToolStripDropDownDirection.BelowLeft);
        }

        /// <summary>
        /// Show hand cursor when we hover over the control menu.
        /// </summary>
        private void ControlMenuMouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Show the default cursor when we leave the control menu.
        /// </summary>
        private void ControlMenuMouseLeave(object sender, EventArgs eventArgs)
        {
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handle the About menu item.
        /// </summary>
        private void mainAbout_Click(object sender, EventArgs e)
        {
            About aboutDialog = new About();
            aboutDialog.ShowDialog();
        }

        /// <summary>
        /// Handle the Log off menu item.
        /// </summary>
        private void mainLogoff_Click(object sender, EventArgs e)
        {
            Program.ShutdownReason = Program.ShutdownReasonType.Logout;
            Close();
        }

        /// <summary>
        /// Handle the Keyboard Help menu item.
        /// </summary>
        private void mainKeyboardHelp_Click(object sender, EventArgs e)
        {
            Action(ActionID.KeyboardHelp);
        }

        /// <summary>
        /// Handle the Exit menu item.
        /// </summary>
        private void mainExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Display the profile for the specified user in the pop-up profile viewer
        /// window.
        /// </summary>
        /// <param name="profile">Profile to be displayed</param>
        private void ShowProfile(Profile profile)
        {
            if (_profileViewer == null)
            {
                _profileViewer = new ProfileView(this);
            }
            _profileViewer.Profile = profile;
            _profileViewer.Show();

            profile.Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Restore the form width, height and state from the last time it
            // was invoked.
            this.RestoreFromSettings();

            // Get notified if the network goes online or offline
            NetworkChange.NetworkAvailabilityChanged += OnNetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += OnNetworkAddressChanged;

            // We may need to update this later.
            _logonImage.Image = Mugshot.MugshotForUser(CIX.Username, true).RealImage;
            CIX.MugshotUpdated += OnMugshotUpdated;

            // Get notified of authentication errors
            CIX.AuthenticationFailed += OnAuthenticationFailed;

            // Get notified of theme changes.
            UI.ThemeChanged += OnThemeChanged;

            // Load and init the toolbar
            _toolbar.ValidateToolbarItem += OnValidateToolbarItem;
            _toolbar.ActionToolbarItem += OnActionToolbarItem;

            #if __MonoCS__
            _toolbar.CanCustomise = false;
            #else
            _toolbar.CanCustomise = true;
            #endif
            _toolbar.Load();
            _toolbar.RefreshButtons();

            // On Linux, make Ctrl+Q the exit shortcut
            #if __MonoCS__
            menuFileExit.ShortcutKeys = Keys.Q | Keys.Control;
            #endif

            // On Linux, Check for Updates is not supported
            #if __MonoCS__
            toolStripSeparator8.Visible = false;
            menuHelpCheckForUpdates.Visible = false;
            mainCheckForUpdates.Visible = false;
            #endif

            // Search control trigger
            CRToolbarItem item = _toolbar.ItemWithID(ActionID.Search);
            if (item != null)
            {
                CRSearchField searchField = (CRSearchField)item.Control;
                if (searchField != null)
                {
                    searchField.Trigger += SearchForString;
                    searchField.Cleared += ClearSearchField;
                }
            }

            // Get notified of settings changes.
            Preferences.PreferencesChanged += OnPreferencesChanged;

            UpdateCaption();
            Update();

            // At this point, we're sufficiently initialised to be able to load
            // and run scripts.
            _scriptManager.LoadScripts();

            // Make sure folders tree is visible
            _foldersTree.Show();

            // Restore the last address viewed
            if (Preferences.StandardPreferences.StartInHomePage)
            {
                Address = "welcome";
            }
            else
            {
                string lastAddress = Program.StartupAddress;
                if (!String.IsNullOrEmpty(lastAddress))
                {
                    Address = lastAddress;
                }
            }

            // Start tasks running
            CIX.StartTask();
        }

        /// <summary>
        /// Called by the toolbar when the user clicks a button.
        /// </summary>
        private void OnActionToolbarItem(object sender, CRToolbarItemEventArgs args)
        {
            switch (args.Item.ID)
            {
                case ActionID.Script:
                    RunScript((string)args.Item.ExtraData);
                    break;

                default:
                    Action(args.Item.ID);
                    break;
            }
        }

        /// <summary>
        /// Called by the toolbar to update the state of the specified toolbar item.
        /// </summary>
        private void OnValidateToolbarItem(object sender, CRToolbarItemEventArgs args)
        {
            CRRoundButton button = args.Item.Control as CRRoundButton;
            if (button != null)
            {
                button.Enabled = CanAction(args.Item.ID);
            }
            else
            {
                args.Item.Control.Enabled = CanAction(args.Item.ID);
            }
        }

        /// <summary>
        /// Handle preferences change events.
        /// </summary>
        private void OnPreferencesChanged(object sender, PreferencesChangedEventArgs args)
        {
            if (args.Name == Preferences.MAPref_ViewMenuBar)
            {
                ShowMenuBar(Preferences.StandardPreferences.ViewMenuBar);
            }
            if (args.Name == Preferences.MAPref_ShowToolBar)
            {
                ShowToolbar(Preferences.StandardPreferences.ShowToolBar);
            }
            if (args.Name == Preferences.MAPref_ViewStatusBar)
            {
                ShowStatusBar(Preferences.StandardPreferences.ViewStatusBar);
            }
        }

        /// <summary>
        /// Handle theme changed events to change the menu theme.
        /// </summary>
        private void OnThemeChanged(object sender, EventArgs args)
        {
            Font titleFont = UI.Menu.Font;
            Font logonNameFont = UI.GetFont(UI.Menu.Font, UIConfigMenu.FontSize - 10);

            mainStatusBar.ForeColor = UI.System.EdgeColour;
            mainStatusBar.BackColor = UI.System.InfoBarColour;

            mainMenubar.BackColor = UI.Menu.BackgroundColour;
            mainMenubar.ForeColor = UI.Menu.ForegroundColour;

            _toolbar.BackColor = UI.System.ToolbarColour;

            _shortcutBar.BackColor = UI.Menu.BackgroundColour;

            foreach (CRLabel menuLabel in _labels)
            {
                menuLabel.ForeColor = UI.Menu.ForegroundColour;
                menuLabel.InactiveColour = UI.Menu.InactiveColour;
                menuLabel.CountColour = UI.System.CountColour;
                menuLabel.CountTextColour = UI.System.CountTextColour;
                menuLabel.Font = titleFont;
            }
            RefreshShortcutBar();

            _logonName.Font = logonNameFont;
            _logonName.ForeColor = UI.Menu.ForegroundColour;
            _logonName.InactiveColour = UI.Menu.ForegroundColour;
        }

        /// <summary>
        /// Refresh the shortcut bar by recomputing the X position of the
        /// labels after the count or font changes.
        /// </summary>
        private void RefreshShortcutBar()
        {
            int xPosition = _shortcutBar.Controls[0].Right + 4;
            foreach (CRLabel label in _labels)
            {
                label.Location = new Point(xPosition, label.Location.Y);
                xPosition += label.Size.Width;
            }
            _shortcutBar.Update();
        }

        /// <summary>
        /// Handle an authentication failure event from CIX and prompt for new credentials.
        /// We also force the system offline while this happens to avoid getting recurring
        /// authentication events.
        /// </summary>
        private void OnAuthenticationFailed(object sender, EventArgs args)
        {
            Platform.UIThread(this, delegate
                {
                    ChangeOnlineState(false);

                    Authenticate loginDialog = new Authenticate
                        {
                            Username = CIX.Username
                        };

                    // Stay offline if they cancel.
                    if (loginDialog.ShowDialog() == DialogResult.OK)
                    {
                        CIX.Username = loginDialog.Username;
                        CIX.Password = loginDialog.Password;

                        ChangeOnlineState(true);
                    }
                });
        }

        /// <summary>
        /// Update the logon mugshot if this changes globally.
        /// </summary>
        private void OnMugshotUpdated(Mugshot mugshot)
        {
            Platform.UIThread(this, delegate
                {
                    if (mugshot.Username == CIX.Username)
                    {
                        _logonImage.Image = mugshot.RealImage;
                    }
                });
        }

        /// <summary>
        /// This event is triggered when the network availability on the host system changes. For example,
        /// going offline or coming back online perhaps in response to WiFi connections. We use this to
        /// avoid implicit updates when no connection is available but we should not block the users
        /// ability to request an explicit update just in case some form of connection is available in
        /// ways this doesn't trigger.
        /// </summary>
        /// <param name="sender">The network monitor, which CAN be on a different thread</param>
        /// <param name="e">Network availability event information</param>
        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Platform.UIThread(this, delegate
                {
                    ChangeOnlineState(NetworkInterface.GetIsNetworkAvailable());

                    LogFile.WriteLine("Network state change detected: Online state is {0}", CIX.Online);
                });
        }

        /// <summary>
        /// Triggered mainly on Mono systems when the underlying IP address changes
        /// or some similar event.
        /// </summary>
        private void OnNetworkAddressChanged(object sender, EventArgs args)
        {
            LogFile.WriteLine("Network address change detected");
        }

        /// <summary>
        /// Handle the Offline menu command.
        /// </summary>
        private void mainOffline_Click(object sender, EventArgs e)
        {
            Action(ActionID.Offline);
        }

        /// <summary>
        /// Go to the CIX Support site.
        /// </summary>
        private void mainSupport_Click(object sender, EventArgs e)
        {
            LaunchURL(Constants.SupportURL);
        }

        /// <summary>
        /// Change the online state and update the caption bar.
        /// </summary>
        /// <param name="newState">The requested new state</param>
        private void ChangeOnlineState(bool newState)
        {
            CIX.Online = newState;
            UpdateCaption();
        }

        /// <summary>
        /// Update the main form caption bar. In addition to getting the application title from
        /// the centralised resource, we also indicate if we're in offline mode.
        /// </summary>
        private void UpdateCaption()
        {
            StringBuilder captionBar = new StringBuilder(Resources.AppTitle);
            if (UserTitleString != null)
            {
                captionBar.AppendFormat(" - {0}", UserTitleString);
            }
            if (!CIX.Online)
            {
                captionBar.AppendFormat(" ({0})", Resources.Offline);
                mainOffline.Checked = true;
            }
            else
            {
                mainOffline.Checked = false;
            }
            Text = captionBar.ToString();
        }

        /// <summary>
        /// Handle the main form being resized. We use this to resize the current
        /// displayed view as it currently has no anchors applied to it.
        /// </summary>
        /// <param name="sender">The object that sent this event</param>
        /// <param name="e">The event arguments</param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            _foldersTree.Size = _viewPanel.Size;
            _toolbar.Width = _viewPanel.Width;
        }

        /// <summary>
        /// Handle the main form being closed. This is our trigger to close the
        /// application and save state. We need to invoke the Close method on each view
        /// to give it a chance to kill any active threads or otherwise we won't shut
        /// down.
        /// </summary>
        /// <param name="sender">The object that sent this event</param>
        /// <param name="e">The event arguments</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!HandleFormClosure(false))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Common code to prepare to close the main form
        /// </summary>
        private bool HandleFormClosure(bool quick)
        {
            if (!MessageEditorCollection.Close())
            {
                return false;
            }

            // Do a final sync of reads
            if (!quick)
            {
                CIX.FolderCollection.SynchronizeReads();
            }

            Preferences.StandardPreferences.LastAddress = _foldersTree.Address;

            _foldersTree.Close();

            this.SaveToSettings();

            return true;
        }

        /// <summary>
        /// Handle the main window being closed by closing the underlying client.
        /// </summary>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _toolbar.Close();
            CIX.Close();
        }

        /// <summary>
        /// Display the Diagnostics window.
        /// </summary>
        private void mainDiagnostics_Click(object sender, EventArgs e)
        {
            Diagnostics diagForm = new Diagnostics();
            diagForm.ShowDialog();
        }

        /// <summary>
        /// Display the Settings dialog.
        /// </summary>
        private void mainSettings_Click(object sender, EventArgs e)
        {
            SettingsDialog settingsDialog = new SettingsDialog();
            settingsDialog.ShowDialog();
        }

        /// <summary>
        /// Handle the Install Script command.
        /// </summary>
        private void mainInstallScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Filter = Resources.ZipFileExtension
                };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _scriptManager.InstallScriptPackage(ofd.FileName);
            }
        }

        /// <summary>
        /// Top-level check whether the specified action ID can be carried out.
        /// </summary>
        private bool CanAction(ActionID id)
        {
            switch (id)
            {
                case ActionID.Offline:
                case ActionID.Search:
                    return true;

                case ActionID.BackTrack:
                    return CanGoBack();

                case ActionID.GoForward:
                    return CanGoForward();

                case ActionID.NextUnread:
                    return CIX.TotalUnread > 0;

                case ActionID.NextPriorityUnread:
                    return CIX.TotalUnreadPriority > 0;

                case ActionID.JoinForum:
                    return true;
            }
            return _foldersTree.CanAction(id);
        }

        /// <summary>
        /// Enable or disable menu items.
        /// </summary>
        private void mainMenubar_MenuActivate(object sender, EventArgs e)
        {
            menuFileOffline.Checked = !CIX.Online;
            menuFileReplyByMail.Enabled = CanAction(ActionID.ReplyByMail);

            menuViewGroupByConv.Checked = Preferences.StandardPreferences.GroupByConv;
            menuViewCollapseConv.Checked = Preferences.StandardPreferences.CollapseConv;
            menuViewAllTopics.Checked = Preferences.StandardPreferences.ShowAllTopics;
            menuViewRecentTopics.Checked = !Preferences.StandardPreferences.ShowAllTopics;
            menuViewShowPlainText.Checked = Preferences.StandardPreferences.IgnoreMarkup;
            menuViewShowInlineImages.Checked = Preferences.StandardPreferences.DownloadInlineImages;
            menuViewShowIgnoredMessages.Checked = Preferences.StandardPreferences.ShowIgnored;
            menuViewCustomiseToolbar.Visible = _toolbar.CanCustomise;

            ContextMenuStrip sortMenu = _foldersTree.SortMenu;
            if (sortMenu != null)
            {
                menuViewSortBy.Enabled = true;
                menuViewSortBy.DropDown = _foldersTree.SortMenu;
            }
            else
            {
                menuViewSortBy.Enabled = false;
            }

            menuViewSortByAuthor.Checked = _foldersTree.SortOrder == CRSortOrder.SortOrder.Author;
            menuViewSortBySubject.Checked = _foldersTree.SortOrder == CRSortOrder.SortOrder.Subject;
            menuViewSortByDate.Checked = _foldersTree.SortOrder == CRSortOrder.SortOrder.Date;
            menuViewSortByPopularity.Checked = _foldersTree.SortOrder == CRSortOrder.SortOrder.Popularity;
            menuViewSortBySubCategory.Checked = _foldersTree.SortOrder == CRSortOrder.SortOrder.SubCategory;
            menuViewSortByTitle.Checked = _foldersTree.SortOrder == CRSortOrder.SortOrder.Title;
            menuViewSortByName.Checked = _foldersTree.SortOrder == CRSortOrder.SortOrder.Name;
            menuViewSortAscendingForTopic.Checked = _foldersTree.Ascending;
            menuViewSortDescendingForTopic.Checked = !_foldersTree.Ascending;
            menuViewSortAscendingForDirectory.Checked = _foldersTree.Ascending;
            menuViewSortDescendingForDirectory.Checked = !_foldersTree.Ascending;

            // Enable/disable
            menuFileReply.Enabled = CanAction(ActionID.Reply);
            menuFilePrint.Enabled = CanAction(ActionID.Print);
            menuViewProfile.Enabled = CanAction(ActionID.Profile);
            menuViewPreviousMessage.Enabled = CanAction(ActionID.BackTrack);
            menuViewNextUnread.Enabled = CanAction(ActionID.NextUnread);
            menuViewNextUnreadPriority.Enabled = CanAction(ActionID.NextPriorityUnread);
            menuViewNextRoot.Enabled = CanAction(ActionID.NextRoot);
            menuViewPreviousRoot.Enabled = CanAction(ActionID.Root);
            menuViewSortBy.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortAscendingForTopic.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortDescendingForTopic.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortAscendingForDirectory.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortDescendingForDirectory.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortByAuthor.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortBySubject.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortByDate.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortByPopularity.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortBySubCategory.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortByName.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuViewSortByTitle.Enabled = _foldersTree.SortOrder != CRSortOrder.SortOrder.None;
            menuMessageNewMessage.Enabled = CanAction(ActionID.NewMessage);
            menuMessageEdit.Enabled = CanAction(ActionID.Edit);
            menuMessageReply.Enabled = CanAction(ActionID.Reply);
            menuMessageMarkThreadRead.Enabled = CanAction(ActionID.MarkThreadRead);
            menuMessageMarkUnread.Enabled = CanAction(ActionID.Read);
            menuMessageMarkIgnored.Enabled = CanAction(ActionID.Ignore);
            menuMessageMarkPriority.Enabled = CanAction(ActionID.Priority);
            menuMessageMarkReadLock.Enabled = CanAction(ActionID.ReadLock);
            menuMessageMarkUnread.Enabled = CanAction(ActionID.Read);
            menuMessageMarkStarred.Enabled = CanAction(ActionID.Star);
            menuMessageGoTo.Enabled = CanAction(ActionID.GoTo);
            menuMessageGoToOriginal.Enabled = CanAction(ActionID.Original);
            menuMessageWithdraw.Enabled = CanAction(ActionID.Withdraw);
            menuMessageBlock.Enabled = CanAction(ActionID.Block);
            menuFolderJoinForum.Enabled = CanAction(ActionID.JoinForum);
            menuFolderRefresh.Enabled = CanAction(ActionID.Refresh);
            menuFolderResign.Enabled = CanAction(ActionID.ResignForum);
            menuFolderMarkAllRead.Enabled = CanAction(ActionID.MarkTopicRead);
            menuFolderDelete.Enabled = CanAction(ActionID.Delete);
            menuFolderParticipants.Enabled = CanAction(ActionID.Participants);
            menuFolderManage.Enabled = CanAction(ActionID.ManageForum);
            menuEditCopy.Enabled = CanAction(ActionID.Copy);
            menuEditCopyLink.Enabled = CanAction(ActionID.Link);
            menuEditSelectAll.Enabled = CanAction(ActionID.SelectAll);
            menuEditQuote.Enabled = CanAction(ActionID.Quote);

            // Menu titles
            menuViewShowStatusBar.Text = Preferences.StandardPreferences.ViewStatusBar ? Resources.HideStatusBar : Resources.ShowStatusBar;
            menuViewShowToolBar.Text = Preferences.StandardPreferences.ShowToolBar ? Resources.HideToolBar : Resources.ShowToolBar;
            if (menuMessageMarkReadLock.Enabled)
            {
                menuMessageMarkReadLock.Text = _foldersTree.TitleForAction(ActionID.ReadLock);
            }
            if (menuMessageMarkStarred.Enabled)
            {
                menuMessageMarkStarred.Text = _foldersTree.TitleForAction(ActionID.Star);
            }
            if (menuMessageMarkIgnored.Enabled)
            {
                menuMessageMarkIgnored.Text = _foldersTree.TitleForAction(ActionID.Ignore);
            }
            if (menuMessageMarkPriority.Enabled)
            {
                menuMessageMarkPriority.Text = _foldersTree.TitleForAction(ActionID.Priority);
            }
            if (menuMessageMarkUnread.Enabled)
            {
                menuMessageMarkUnread.Text = _foldersTree.TitleForAction(ActionID.Read);
            }
            if (menuMessageWithdraw.Enabled)
            {
                menuMessageWithdraw.Text = _foldersTree.TitleForAction(ActionID.Withdraw);
            }
        }

        /// <summary>
        /// Start a new private message.
        /// </summary>
        private void menuFileNewPrivateMessage_Click(object sender, EventArgs e)
        {
            Address = "cixmail:";
        }

        /// <summary>
        /// Reply to the current selected message.
        /// </summary>
        private void menuFileReply_Click(object sender, EventArgs e)
        {
            Action(ActionID.Reply);
        }

        /// <summary>
        /// Display the profile for the current user.
        /// </summary>
        private void menuViewProfile_Click(object sender, EventArgs e)
        {
            Action(ActionID.Profile);
        }

        /// <summary>
        /// Toggle displaying the current message as plain text.
        /// </summary>
        private void menuViewShowPlainText_Click(object sender, EventArgs e)
        {
            Action(ActionID.Markdown);
        }

        /// <summary>
        /// Toggle showing inline images.
        /// </summary>
        private void menuViewShowInlineImages_Click(object sender, EventArgs e)
        {
            bool downloadImages = Preferences.StandardPreferences.DownloadInlineImages;
            Preferences.StandardPreferences.DownloadInlineImages = !downloadImages;
        }

        /// <summary>
        /// Toggle showing ignored messages.
        /// </summary>
        private void menuViewShowIgnoredMessages_Click(object sender, EventArgs e)
        {
            bool showIgnored = Preferences.StandardPreferences.ShowIgnored;
            Preferences.StandardPreferences.ShowIgnored = !showIgnored;
        }

        /// <summary>
        /// Go back to the previous message.
        /// </summary>
        private void menuViewPreviousMessage_Click(object sender, EventArgs e)
        {
            Action(ActionID.BackTrack);
        }

        /// <summary>
        /// Display the next unread message.
        /// </summary>
        private void menuViewNextUnread_Click(object sender, EventArgs e)
        {
            Action(ActionID.NextUnread);
        }

        /// <summary>
        /// Display the next unread priority message.
        /// </summary>
        private void menuViewNextUnreadPriority_Click(object sender, EventArgs e)
        {
            Action(ActionID.NextPriorityUnread);
        }

        /// <summary>
        /// Display the previous root message.
        /// </summary>
        private void menuViewPreviousRoot_Click(object sender, EventArgs e)
        {
            Action(ActionID.Root);
        }

        /// <summary>
        /// Display the next root message.
        /// </summary>
        private void menuViewNextRoot_Click(object sender, EventArgs e)
        {
            Action(ActionID.NextRoot);
        }

        /// <summary>
        /// Display all topics in the folder list.
        /// </summary>
        private void menuViewAllTopics_Click(object sender, EventArgs e)
        {
            Preferences.StandardPreferences.ShowAllTopics = true;
        }

        /// <summary>
        /// Display only recent topics in the folder list.
        /// </summary>
        private void menuViewRecentTopics_Click(object sender, EventArgs e)
        {
            Preferences.StandardPreferences.ShowAllTopics = false;
        }

        /// <summary>
        /// Toggle status bar on or off.
        /// </summary>
        private void menuViewShowStatusBar_Click(object sender, EventArgs e)
        {
            bool viewStatusBar = Preferences.StandardPreferences.ViewStatusBar;
            Preferences.StandardPreferences.ViewStatusBar = !viewStatusBar;
        }

        /// <summary>
        /// Toggle the toolbar on or off.
        /// </summary>
        private void menuViewShowToolBar_Click(object sender, EventArgs e)
        {
            bool viewToolBar = Preferences.StandardPreferences.ShowToolBar;
            Preferences.StandardPreferences.ShowToolBar = !viewToolBar;
        }

        /// <summary>
        /// Compose a new forum message.
        /// </summary>
        private void menuMessageNewMessage_Click(object sender, EventArgs e)
        {
            Action(ActionID.NewMessage);
        }

        /// <summary>
        /// Compose a reply to the current forum message.
        /// </summary>
        private void menuMessageReply_Click(object sender, EventArgs e)
        {
            Action(ActionID.Reply);
        }

        /// <summary>
        /// Edit the draft message in the forum.
        /// </summary>
        private void menuMessageEdit_Click(object sender, EventArgs e)
        {
            Action(ActionID.Edit);
        }

        /// <summary>
        /// Mark the current thread as read.
        /// </summary>
        private void menuMessageMarkThreadRead_Click(object sender, EventArgs e)
        {
            Action(ActionID.MarkThreadRead);
        }

        /// <summary>
        /// Mark current message unread.
        /// </summary>
        private void menuMessageMarkUnread_Click(object sender, EventArgs e)
        {
            Action(ActionID.Read);
        }

        /// <summary>
        /// Mark current thread priority.
        /// </summary>
        private void menuMessageMarkPriority_Click(object sender, EventArgs e)
        {
            Action(ActionID.Priority);
        }

        /// <summary>
        /// Mark current thread as ignored.
        /// </summary>
        private void menuMessageMarkIgnored_Click(object sender, EventArgs e)
        {
            Action(ActionID.Ignore);
        }

        /// <summary>
        /// Set read lock on current message.
        /// </summary>
        private void menuMessageMarkReadLock_Click(object sender, EventArgs e)
        {
            Action(ActionID.ReadLock);
        }

        /// <summary>
        /// Set Star on current message.
        /// </summary>
        private void menuMessageMarkStarred_Click(object sender, EventArgs e)
        {
            Action(ActionID.Star);
        }

        /// <summary>
        /// Go To a message.
        /// </summary>
        private void menuMessageGoTo_Click(object sender, EventArgs e)
        {
            Action(ActionID.GoTo);
        }

        /// <summary>
        /// Go to the parent of the selected message.
        /// </summary>
        private void menuMessageGoToOriginal_Click(object sender, EventArgs e)
        {
            Action(ActionID.Original);
        }

        /// <summary>
        /// Withdraw or delete the selected message.
        /// </summary>
        private void menuMessageWithdraw_Click(object sender, EventArgs e)
        {
            Action(ActionID.Withdraw);
        }

        /// <summary>
        /// Display the Join Forum dialog.
        /// </summary>
        private void menuFolderJoinForum_Click(object sender, EventArgs e)
        {
            Action(ActionID.JoinForum);
        }

        /// <summary>
        /// Resign from the current folder.
        /// </summary>
        private void menuFolderResign_Click(object sender, EventArgs e)
        {
            Action(ActionID.ResignForum);
        }

        /// <summary>
        /// Mark all messages in the current topic as read.
        /// </summary>
        private void menuFolderMarkAllRead_Click(object sender, EventArgs e)
        {
            Action(ActionID.MarkTopicRead);
        }

        /// <summary>
        /// Refresh the current folder.
        /// </summary>
        private void menuFolderRefresh_Click(object sender, EventArgs e)
        {
            Action(ActionID.Refresh);
        }

        /// <summary>
        /// Delete the selected folder.
        /// </summary>
        private void menuFolderDelete_Click(object sender, EventArgs e)
        {
            Action(ActionID.Delete);
        }

        /// <summary>
        /// Bring up the Page Setup dialog.
        /// </summary>
        private void menuFilePageSetup_Click(object sender, EventArgs e)
        {
            PageSetupDialog setupDlg = new PageSetupDialog
                {
                    Document = PrintDocument,
                    AllowMargins = true,
                    AllowOrientation = true,
                    AllowPaper = true,
                    AllowPrinter = true
                };

            if (setupDlg.ShowDialog() == DialogResult.OK)
            {
                PrintDocument.DefaultPageSettings = setupDlg.PageSettings;
                PrintDocument.PrinterSettings = setupDlg.PrinterSettings;
            }
        }

        /// <summary>
        /// Bring up the Print dialog.
        /// </summary>
        private void menuFilePrint_Click(object sender, EventArgs e)
        {
            Action(ActionID.Print);
        }

        /// <summary>
        /// Handle the Copy command
        /// </summary>
        private void menuEditCopy_Click(object sender, EventArgs e)
        {
            Action(ActionID.Copy);
        }

        /// <summary>
        /// Handle the Copy Link command.
        /// </summary>
        private void menuEditCopyLink_Click(object sender, EventArgs e)
        {
            Action(ActionID.Link);
        }

        /// <summary>
        /// Handle the Select All command.
        /// </summary>
        private void menuEditSelectAll_Click(object sender, EventArgs e)
        {
            Action(ActionID.SelectAll);
        }

        /// <summary>
        /// Handle the Quote command.
        /// </summary>
        private void menuEditQuote_Click(object sender, EventArgs e)
        {
            Action(ActionID.Quote);
        }

        /// <summary>
        /// Display the Account dialog
        /// </summary>
        private void menuCIXReaderAccount_Click(object sender, EventArgs e)
        {
            Account accountDialog = new Account();
            accountDialog.ShowDialog();
        }

        /// <summary>
        /// Display the forum participants list
        /// </summary>
        private void menuFolderParticipants_Click(object sender, EventArgs e)
        {
            Action(ActionID.Participants);
        }

        /// <summary>
        /// Sort the list in the current view by date
        /// </summary>
        private void viewSortByDate_Click(object sender, EventArgs e)
        {
            _foldersTree.SortOrder = CRSortOrder.SortOrder.Date;
        }

        /// <summary>
        /// Sort the list in the current view by author
        /// </summary>
        private void viewSortByAuthor_Click(object sender, EventArgs e)
        {
            _foldersTree.SortOrder = CRSortOrder.SortOrder.Author;
        }

        /// <summary>
        /// Sort the list in the current view by subject
        /// </summary>
        private void viewSortBySubject_Click(object sender, EventArgs e)
        {
            _foldersTree.SortOrder = CRSortOrder.SortOrder.Subject;
        }

        /// <summary>
        /// Sort the list in the current view by name.
        /// </summary>
        private void menuViewSortByName_Click(object sender, EventArgs e)
        {
            _foldersTree.SortOrder = CRSortOrder.SortOrder.Name;
        }

        /// <summary>
        /// Sort the list in the current view by title.
        /// </summary>
        private void menuViewSortByTitle_Click(object sender, EventArgs e)
        {
            _foldersTree.SortOrder = CRSortOrder.SortOrder.Title;
        }

        /// <summary>
        /// Sort the list in the current view by popularity.
        /// </summary>
        private void menuViewSortByPopularity_Click(object sender, EventArgs e)
        {
            _foldersTree.SortOrder = CRSortOrder.SortOrder.Popularity;
        }

        /// <summary>
        /// Sort the list in the current view by subcategory.
        /// </summary>
        private void menuViewSortBySubCategory_Click(object sender, EventArgs e)
        {
            _foldersTree.SortOrder = CRSortOrder.SortOrder.SubCategory;
        }

        /// <summary>
        /// Sort the list in the current view in ascending order
        /// </summary>
        private void viewSortAscending_Click(object sender, EventArgs e)
        {
            _foldersTree.Ascending = true;
        }

        /// <summary>
        /// Sort the list in the current view in descending order
        /// </summary>
        private void viewSortDescending_Click(object sender, EventArgs e)
        {
            _foldersTree.Ascending = false;
        }

        /// <summary>
        /// Moderate the current forum.
        /// </summary>
        private void menuFolderManage_Click(object sender, EventArgs e)
        {
            Action(ActionID.ManageForum);
        }

        /// <summary>
        /// Display the change log.
        /// </summary>
        private void menuHelpViewChangeLog_Click(object sender, EventArgs e)
        {
            Action(ActionID.ViewChangeLog);
        }

        /// <summary>
        /// Reply to the selected message by private mail.
        /// </summary>
        private void menuFileReplyByMail_Click(object sender, EventArgs e)
        {
            Action(ActionID.ReplyByMail);
        }

        /// <summary>
        /// Group the thread display by conversations.
        /// </summary>
        private void menuViewGroupByConv_Click(object sender, EventArgs e)
        {
            bool groupByConv = Preferences.StandardPreferences.GroupByConv;
            Preferences.StandardPreferences.GroupByConv = !groupByConv;
        }

        /// <summary>
        /// Collapse all conversations by default.
        /// </summary>
        private void menuViewCollapseConv_Click(object sender, EventArgs e)
        {
            bool collapseConv = Preferences.StandardPreferences.CollapseConv;
            Preferences.StandardPreferences.CollapseConv = !collapseConv;
        }

        /// <summary>
        /// Invoke the toolbar customisation UI.
        /// </summary>
        private void menuViewCustomiseToolbar_Click(object sender, EventArgs e)
        {
            _toolbar.CustomiseToolbar();
        }

        /// <summary>
        /// Block messages from the selected author.
        /// </summary>
        private void menuMessageBlock_Click(object sender, EventArgs e)
        {
            Action(ActionID.Block);
        }
    }
}