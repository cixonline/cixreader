// *****************************************************
// CIXReader
// InboxView.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/06/2015 19:22
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
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
    public sealed partial class InboxView : ViewBaseView
    {
        private Font _font;
        private Font _bodyFont;
        private int _rowHeight;
        private bool _showFullDate;
        private string _currentFilterString;
        private FolderBase _currentFolder;
        private ListViewItem[] _itemCache;
        private List<InboxConversation> _conversations; 

        public InboxView(FoldersTree foldersTree) : base("Inbox")
        {
            InitializeComponent();

            FoldersTree = foldersTree;
            SortOrderBase.DefaultSortOrder = CRSortOrder.SortOrder.Date;
        }

        /// <summary>
        /// Display the directory for the specified CategoryFolder
        /// </summary>
        public override bool ViewFromFolder(FolderBase folder, Address address, FolderOptions options)
        {
            if (folder != _currentFolder)
            {
                _currentFolder = folder;
                SortConversations();
            }

            // If an address is specified then it refers to a conversation ID that
            // needs to be selected. If it is not found, the first message is selected
            // instead.
            if (address != null && address.Scheme == "cixmailbox")
            {
                int selectedID;

                Int32.TryParse(address.Data, out selectedID);
            
                int selectedIndex;
                for (selectedIndex = 0; selectedIndex < _conversations.Count; ++selectedIndex)
                {
                    InboxConversation conversation = _conversations[selectedIndex];
                    if (conversation.RemoteID == selectedID)
                        break;
                }
                if (selectedIndex == _conversations.Count)
                {
                    selectedIndex = 0;
                }
                SelectedRow = selectedIndex;
                if (address.Unread)
                {
                    SelectedMessage.MarkUnread();
                }
                return true;
            }

            // If options are specified then search for the next unread
            // in the list otherwise set the initial selection to something
            // useful.
            if (options == 0 && SelectedRow == -1)
            {
                SetInitialSelection();
            }
            else
            {
                int row = inboxConversations.SearchRow;
                if (row < 0 || options.HasFlag(FolderOptions.Reset))
                {
                    row = 0;
                }
                else if (_conversations.Count > 0)
                {
                    InboxConversation conversation = _conversations[row];
                    if (conversation.UnreadCount > 0)
                    {
                        conversation.MarkRead();
                    }
                }
                if (!FirstUnreadAfterRow(row, options))
                {
                    inboxConversations.SearchRow = 0;
                    return false;
                }
            }

            FoldersTree.SetTopicName(_currentFolder.FullName);

            ActiveControl = inboxConversations;
            inboxConversations.Focus();
            return true;
        }

        /// <summary>
        /// The associated folders tree view
        /// </summary>
        public FoldersTree FoldersTree { get; set; }

        /// <summary>
        /// Return whether the specified action can be carried out.
        /// </summary>
        /// <param name="id">An action ID</param>
        public override bool CanAction(ActionID id)
        {
            switch (id)
            {
                case ActionID.NewMessage:
                    return true;

                case ActionID.Reply:
                case ActionID.Edit:
                case ActionID.Withdraw:
                case ActionID.Profile:
                case ActionID.Read:
                case ActionID.Print:
                case ActionID.SelectAll:
                    return SelectedMessage != null;
            }
            return false;
        }

        /// <summary>
        /// Return the text string for the specified action.
        /// </summary>
        public override string TitleForAction(ActionID id)
        {
            if (SelectedMessage != null)
            {
                InboxConversation conversation = SelectedMessage;
                switch (id)
                {
                    case ActionID.Read:
                        return conversation.UnreadCount > 0 ? Resources.AsRead : Resources.AsUnread;

                    case ActionID.Withdraw:
                        return Resources.Delete;
                }
            }
            return null;
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
        /// Override to return the URL of the view being displayed.
        /// </summary>
        public override string Address
        {
            get
            {
                int currentSelectedRow = SelectedRow;
                int selectedID = 0;

                if (currentSelectedRow > -1)
                {
                    InboxConversation message = _conversations[currentSelectedRow];
                    selectedID = message.RemoteID;
                }
                return string.Format("cixmailbox:inbox:{0}", selectedID);
            }
        }

        /// <summary>
        /// Return the sort menu for this view.
        /// </summary>
        public override ContextMenuStrip SortMenu
        {
            get { return FoldersTree.MainForm.TopicSortMenu; }
        }

        /// <summary>
        /// Display a directory of forums filtered by the given search string. If the search string is
        /// empty then all forums are displayed.
        /// </summary>
        /// <param name="searchString">A search string</param>
        public override void FilterViewByString(string searchString)
        {
            _currentFilterString = searchString.Trim().ToLower();
            InboxConversation selectedMessage = SelectedMessage;

            _conversations = new List<InboxConversation>(CIX.ConversationCollection.AllConversations);

            bool _isFiltering = !string.IsNullOrEmpty(_currentFilterString);
            if (_isFiltering)
            {
                _conversations = _conversations.Where(msg => msg.Author.Contains(_currentFilterString) || msg.Subject.Contains(_currentFilterString)).ToList();
            }

            InitialiseList();
            RefreshList();

            RestoreSelection(selectedMessage);
        }

        /// <summary>
        /// Update our copy of the font and font metrics when the theme changes.
        /// </summary>
        private void RefreshTheme()
        {
            _font = UI.GetFont(UI.Forums.Font, UI.Forums.FontSize);
            _bodyFont = UI.GetFont(UI.Forums.MessageFont, UI.Forums.MessageFontSize);
            _rowHeight = (_font.Height + 4) * 2;

            inboxConversations.Font = _font;
            inboxConversations.SetHeight(_rowHeight);

            inboxSplitview.BackColor = UI.System.SplitterBarColour;
        }

        /// <summary>
        /// Put the focus on the first message in the folder, if there is one.
        /// </summary>
        private void SetInitialSelection()
        {
            if (!FirstUnreadAfterRow(0, FolderOptions.NextUnread))
            {
                SelectedRow = SortOrderBase.Ascending ? _conversations.Count - 1 : 0;
            }
        }

        /// <summary>
        /// Locate and select the first unread mail message after the given row.
        /// </summary>
        private bool FirstUnreadAfterRow(int row, FolderOptions options)
        {
            while (row < _conversations.Count)
            {
                InboxConversation conversation = _conversations[row];
                if ((options & FolderOptions.NextUnread) == FolderOptions.NextUnread && conversation.UnreadCount > 0)
                {
                    SelectedRow = row;
                    break;
                }
                row++;
            }
            return (row != _conversations.Count);
        }

        /// <summary>
        /// Reload the mail folder list
        /// </summary>
        private void RefreshList()
        {
            if (_conversations.Count > 0)
            {
                InboxConversation savedMessage = SelectedMessage;
                inboxConversations.RedrawItems(0, _conversations.Count - 1, false);
                RestoreSelection(savedMessage);
            }
        }

        /// <summary>
        /// Return the current selected message item.
        /// </summary>
        private InboxConversation SelectedMessage
        {
            get
            {
                int selectedRow = SelectedRow;
                return selectedRow >= 0 && selectedRow < _conversations.Count ? _conversations[selectedRow] : null;
            }
        }

        /// <summary>
        /// Put the selection back on the specified conversation.
        /// </summary>
        /// <param name="conversation">The message to select</param>
        private void RestoreSelection(InboxConversation conversation)
        {
            if (_conversations.Count == 0)
            {
                ShowEmptyMessage();
            }
            else if (conversation != null)
            {
                SelectedRow = _conversations.IndexOf(conversation);
            }
        }

        /// <summary>
        /// Get or set the index of the selected row.
        /// </summary>
        private int SelectedRow
        {
            get
            {
                ListView.SelectedIndexCollection selectedItems = inboxConversations.SelectedIndices;
                if (selectedItems.Count == 1)
                {
                    return selectedItems[0];
                }
                return -1;
            }
            set
            {
                if (value >= 0 && value < _conversations.Count)
                {
                    if (inboxConversations.Items[value].Selected)
                    {
                        ShowMessage(_conversations[value]);
                    }
                    else
                    {
                        inboxConversations.Items[value].Selected = true;
                        inboxConversations.Items[value].Focused = true;
                        inboxConversations.EnsureVisible(value);
                        CentreSelection();
                    }
                }
                inboxConversations.SearchRow = value;
                inboxConversations.Select();
            }
        }

        /// <summary>
        /// Action the specified ID with the given conversation.
        /// </summary>
        /// <param name="id">An action ID</param>
        /// <param name="conversation">A selected conversation, or null</param>
        private void Action(ActionID id, InboxConversation conversation)
        {
            switch (id)
            {
                case ActionID.NewMessage:
                    FoldersTree.MainForm.Address = "cixmail:";
                    break;

                case ActionID.Print:
                    Print(conversation);
                    break;

                case ActionID.Profile:
                case ActionID.AuthorImage:
                    FoldersTree.MainForm.Address = string.Format("cixuser:{0}", conversation.Author);
                    break;

                case ActionID.Withdraw:
                    conversation.MarkDelete();
                    break;

                case ActionID.PageMessage:
                case ActionID.NextUnread:
                    GoToNextUnread(conversation);
                    break;

                case ActionID.NextPriorityUnread:
                    GoToNextPriorityUnread(conversation);
                    break;

                case ActionID.Edit:
                case ActionID.Reply:
                    {
                        InboxMessageEditor newMessageWnd = new InboxMessageEditor(conversation);
                        newMessageWnd.Show();
                        break;
                    }

                case ActionID.Read:
                    MarkConversationAsRead(conversation);
                    break;

                case ActionID.SelectAll:
                    SelectAll();
                    break;
            }
        }

        /// <summary>
        /// Go to the next unread message.
        /// </summary>
        private void GoToNextUnread(InboxConversation conversation)
        {
            if (conversation != null && conversation.UnreadCount > 0)
            {
                conversation.MarkRead();
            }
            FoldersTree.NextUnread(FolderOptions.NextUnread);
        }

        /// <summary>
        /// Go to the next priority unread message.
        /// </summary>
        private void GoToNextPriorityUnread(InboxConversation conversation)
        {
            if (conversation != null && conversation.UnreadCount > 0)
            {
                conversation.MarkRead();
            }
            FoldersTree.NextUnread(FolderOptions.NextUnread | FolderOptions.Priority);
        }

        /// <summary>
        /// Draw the full thread for the selected conversation. The users current scroll position is not altered
        /// unless resetScrollPosition is true in which case we scroll so that the last message is visible.
        /// 
        /// Drawing the thread also optionally marks the conversation as read and triggers an update on the server.
        /// </summary>
        /// <param name="conversation">The conversation to draw</param>
        private void ShowMessage(InboxConversation conversation)
        {
            inboxMessagePane.BeginUpdate();
            inboxMessagePane.Items.Clear();
            if (conversation != null)
            {
                InboxItem lastItem = null;

                foreach (InboxMessage message in conversation.Messages)
                {
                    InboxItem item = new InboxItem(inboxMessagePane, lastItem != null)
                    {
                        ID = message.RemoteID,
                        Image = Mugshot.MugshotForUser(message.Author, true).RealImage,
                        ItemColour = SystemColors.Window,
                        FullDateString = _showFullDate,
                        Message = message,
                        Font = _bodyFont
                    };
                    inboxMessagePane.Items.Add(item);
                    lastItem = item;
                }
                inboxMessagePane.SelectedItem = lastItem;
            }
            inboxMessagePane.EndUpdate(null);
        }

        /// <summary>
        /// Mark a conversation as read if we previously has unread messages. This also triggers
        /// a server update and redraws the inbox list item to remove the unread count badge.
        /// </summary>
        /// <param name="conversation">Conversation to mark read</param>
        private static void MarkConversationAsRead(InboxConversation conversation)
        {
            if (conversation.UnreadCount > 0)
            {
                conversation.MarkRead();
            }
            else
            {
                conversation.MarkUnread();
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
                int itemsOnPage = inboxConversations.ClientRectangle.Height / _rowHeight;
                int topItem = Math.Max(0, selectedRow - itemsOnPage / 2);
                inboxConversations.TopItem = inboxConversations.Items[topItem];
            }
        }

        /// <summary>
        /// Print the current conversation.
        /// </summary>
        private void Print(InboxConversation conversation)
        {
            Font printFont = new Font("Arial", 10);

            PrintDocument printDoc = new PrintDocument
            {
                PrinterSettings = FoldersTree.MainForm.PrintDocument.PrinterSettings,
                DefaultPageSettings = FoldersTree.MainForm.PrintDocument.DefaultPageSettings
            };

            PrintDialog pdi = new PrintDialog
            {
                Document = printDoc,
                UseEXDialog = true
            };
            if (pdi.ShowDialog() == DialogResult.OK)
            {
                StringBuilder convText = new StringBuilder();
                List<InboxMessage> thread = conversation.Messages.ToList();

                convText.AppendFormat(Resources.PrintMailSubject, conversation.Subject);
                convText.AppendLine();
                convText.AppendFormat(Resources.PrintMailDate, conversation.Date);
                convText.AppendLine();
                convText.AppendLine();
                foreach (InboxMessage message in thread)
                {
                    convText.AppendFormat(Resources.PrintMailFrom, message.Author);
                    convText.AppendLine();
                    convText.AppendLine();
                    convText.AppendLine(message.Body);
                }

                string[] lines = convText.ToString().Split('\n');
                int lineIndex = 0;
                try
                {
                    printDoc.PrintPage += (sender, ev) =>
                    {
                        float leftMargin = ev.MarginBounds.Left;
                        float topMargin = ev.MarginBounds.Top;

                        // Print each line of the file.
                        float yPos = topMargin;
                        while (lineIndex < lines.Length)
                        {
                            string line = lines[lineIndex];
                            SizeF sf = ev.Graphics.MeasureString(line, printFont, ev.MarginBounds.Width);
                            if (yPos + sf.Height > ev.MarginBounds.Bottom)
                            {
                                break;
                            }
                            using (Brush textBrush = new SolidBrush(SystemColors.ControlText))
                            {
                                ev.Graphics.DrawString(line, printFont, textBrush, new RectangleF(new PointF(leftMargin, yPos), sf), new StringFormat());
                            }
                            yPos += (sf.Height > 0) ? sf.Height : printFont.GetHeight(ev.Graphics);
                            ++lineIndex;
                        }

                        // If more lines exist, print another page. 
                        ev.HasMorePages = lineIndex < lines.Length;
                    };
                    printDoc.Print();
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format(Resources.PrintError, e.Message), Resources.Error, MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// Called on first load of the inbox view.
        /// </summary>
        private void InboxView_Load(object sender, EventArgs e)
        {
            RefreshTheme();

            _showFullDate = Preferences.StandardPreferences.ShowFullDate;

            CIX.ConversationCollection.ConversationAdded += OnConversationAdded;
            CIX.ConversationCollection.ConversationChanged += OnConversationChanged;
            CIX.ConversationCollection.ConversationDeleted += OnConversationDeleted;

            CIX.MugshotUpdated += OnMugshotUpdated;

            SortOrderBase.OrderingChanged += OnOrderingChanged;

            inboxMessagePane.LinkClicked += OnLinkClicked;

            inboxMessagePane.ExpandInlineImages = Preferences.StandardPreferences.DownloadInlineImages;
            inboxMessagePane.DisableMarkup = Preferences.StandardPreferences.IgnoreMarkup;

            // Get notified of preferences changes
            Preferences.PreferencesChanged += OnPreferencesChanged;

            // Get notified of theme changes.
            UI.ThemeChanged += OnThemeChanged;
        }

        /// <summary>
        /// Respond to links being clicked by sending them to the main form to be processed.
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
        /// Respond to changes in the sort order.
        /// </summary>
        private void OnOrderingChanged(object sender, EventArgs args)
        {
            SortConversations();
        }

        /// <summary>
        /// Initial load of the thread of all messages currently in the database for the given
        /// folder, up to a maximum of _maxRootsPerPage root messages.
        /// </summary>
        private void SortConversations()
        {
            switch (SortOrderBase.Order)
            {
                case CRSortOrder.SortOrder.Author:
                    _conversations = SortOrderBase.Ascending
                        ? CIX.ConversationCollection.AllConversations.OrderBy(rt => rt.Author).ToList()
                        : CIX.ConversationCollection.AllConversations.OrderByDescending(rt => rt.Author).ToList();
                    break;

                case CRSortOrder.SortOrder.Subject:
                    _conversations = SortOrderBase.Ascending
                        ? CIX.ConversationCollection.AllConversations.OrderBy(rt => rt.Subject).ToList()
                        : CIX.ConversationCollection.AllConversations.OrderByDescending(rt => rt.Subject).ToList();
                    break;

                case CRSortOrder.SortOrder.Date:
                    _conversations = SortOrderBase.Ascending
                        ? CIX.ConversationCollection.AllConversations.OrderBy(rt => rt.Date).ToList()
                        : CIX.ConversationCollection.AllConversations.OrderByDescending(rt => rt.Date).ToList();
                    break;
            }
            InitialiseList();
            RefreshList();
        }

        /// <summary>
        /// This notification is triggered if a mugshot is updated from the server. We use this
        /// to refresh the images shown in the root message list.
        /// </summary>
        /// <param name="mugshot">The mugshot task</param>
        private void OnMugshotUpdated(Mugshot mugshot)
        {
            Platform.UIThread(this, delegate
            {
                // Update in the thread list.
                foreach (InboxItem item in inboxMessagePane.Items.Cast<InboxItem>())
                {
                    InboxMessage message = item.Message;
                    if (message.Author == mugshot.Username)
                    {
                        item.Image = mugshot.RealImage;
                        item.Invalidate();
                    }
                }
            });
        }

        /// <summary>
        /// Handle a change to a conversation.
        /// </summary>
        private void OnConversationChanged(object sender, InboxEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                InboxConversation selectedMessage = SelectedMessage;
                RefreshList();
                RestoreSelection(selectedMessage);
            });
        }


        /// <summary>
        /// This event is fired when a new conversation is added to the list of conversations.
        /// </summary>
        private void OnConversationAdded(object sender, InboxEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                InboxConversation selectedMessage = SelectedMessage;
                SortConversations();
                RestoreSelection(selectedMessage);
            });
        }

        /// <summary>
        /// Handle the conversation being deleted from the client.
        /// </summary>
        private void OnConversationDeleted(object sender, InboxEventArgs args)
        {
            Platform.UIThread(this, delegate
            {
                InboxConversation conversation = args.Conversation;
                int selectedRow = SelectedRow;
                bool deletingCurrent = (selectedRow != -1) && (conversation == _conversations[selectedRow]);

                _conversations.Remove(conversation);

                InitialiseList();
                RefreshList();

                if (deletingCurrent)
                {
                    if (selectedRow == _conversations.Count)
                    {
                        --selectedRow;
                    }
                    if (selectedRow < 0)
                    {
                        ShowEmptyMessage();
                    }
                    else
                    {
                        SelectedRow = selectedRow;
                    }
                }
            });
        }

        /// <summary>
        /// Display a blank message pane and a button to create a new PMessage
        /// </summary>
        private void ShowEmptyMessage()
        {
            ShowMessage(null);
        }

        /// <summary>
        /// Handle preferences change events.
        /// </summary>
        private void OnPreferencesChanged(object sender, PreferencesChangedEventArgs args)
        {
            if (args.Name == Preferences.MAPref_DownloadInlineImages)
            {
                inboxMessagePane.ExpandInlineImages = Preferences.StandardPreferences.DownloadInlineImages;
                ShowMessage(SelectedMessage);
            }
            if (args.Name == Preferences.MAPref_IgnoreMarkup)
            {
                inboxMessagePane.DisableMarkup = Preferences.StandardPreferences.IgnoreMarkup;
                ShowMessage(SelectedMessage);
            }
            if (args.Name == Preferences.MAPref_ShowFullDate)
            {
                _showFullDate = Preferences.StandardPreferences.ShowFullDate;
                RefreshList();
            }
            if (args.Name == Preferences.MAPref_DefaultSearchEngine)
            {
                RefreshList();
            }
        }

        /// <summary>
        /// Handle theme changed events to change the forums theme.
        /// </summary>
        private void OnThemeChanged(object sender, EventArgs args)
        {
            RefreshTheme();
            RefreshList();
        }

        /// <summary>
        /// Initialise the virtual list view records with the capacity from the
        /// _messages list.
        /// </summary>
        private void InitialiseList()
        {
            _itemCache = new ListViewItem[_conversations.Count];
            inboxConversations.VirtualListSize = _conversations.Count;
        }

        /// <summary>
        /// Fill a ListViewItem with an entry for the conversation at the given index.
        /// </summary>
        private void RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs args)
        {
            ListViewItem itm = _itemCache[args.ItemIndex];
            if (itm == null)
            {
                itm = new ListViewItem("") { Tag = _conversations[args.ItemIndex] };
                _itemCache[args.ItemIndex] = itm;
            }
            args.Item = itm;
        }

        /// <summary>
        /// Compute the display rectangles for the various field elements of the given message
        /// </summary>
        private DrawRectElements ComputeDrawRectElements(Graphics graphics, InboxConversation message, Rectangle bounds)
        {
            DrawRectElements drawRectElements = new DrawRectElements();
            Rectangle drawRect = bounds;
            drawRect.Inflate(-2, -2);

            // The rectangle for the selection and focus
            drawRectElements.BoundaryRect = drawRect;

            drawRect.Y = drawRect.Y + (drawRect.Height - _font.Height) / 2;
            drawRect.X += 4;
            drawRect.Width -= 4;

            // Compute Read image rectangle
            Image readImage = ReadImageForMessage(message);
            Rectangle imageRect = new Rectangle(drawRect.X, bounds.Y + (bounds.Height - readImage.Height) / 2, readImage.Width, readImage.Height);

            drawRectElements.ReadRect = imageRect;

            drawRect.X += readImage.Width + 4;
            drawRect.Width -= readImage.Width + 4;

            // Compute author name rectangle
            SizeF idSize = graphics.MeasureString(message.Author, _font);

            drawRectElements.AuthorRect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute first separator rectangle
            const string separatorChar = "•";
            idSize = graphics.MeasureString(separatorChar, _font);

            drawRectElements.Separator1Rect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute date field rectangle
            string dateString = (_showFullDate)
                ? message.Date.ToString("d MMM yyyy") + " " + message.Date.ToShortTimeString()
                : message.Date.FriendlyString(true);
            idSize = graphics.MeasureString(dateString, _font);

            drawRectElements.DateRect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute ID field rectangle
            string idString = (message.IsDraft)
                ? "Draft"
                : message.RemoteID.ToString(CultureInfo.InvariantCulture);
            idSize = graphics.MeasureString(idString, _font);

            drawRectElements.IDRect = drawRect;

            drawRect.X += (int)idSize.Width + 4;
            drawRect.Width -= (int)idSize.Width + 4;

            // Compute second separator rectangle
            idSize = graphics.MeasureString(separatorChar, _font);
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
        private void DrawSubItem(object sender, DrawListViewSubItemEventArgs args)
        {
            InboxConversation message = args.Item.Tag as InboxConversation;
            if (message != null)
            {
                DrawRectElements elements = ComputeDrawRectElements(args.Graphics, message, args.Bounds);

                Color textColor = UI.Forums.HeaderFooterColour;
                if (args.Item.Selected)
                {
                    args.DrawFocusRectangle(elements.BoundaryRect);
                    textColor = UI.Forums.SelectionTextColour;
                }

                // Draw the selection around the control.
                using (Pen edgePen = new Pen(args.Item.Selected ? UI.Forums.SelectionColour : BackColor))
                {
                    Color fillColour = args.Item.Selected ? UI.Forums.SelectionColour : UI.Forums.RootColour;
                    using (Brush backBrush = new SolidBrush(fillColour))
                    {
                        args.Graphics.FillRoundedRectangle(edgePen, backBrush, elements.BoundaryRect);
                    }
                }

                using (Brush textBrush = new SolidBrush(textColor))
                {
                    // Draw read image
                    Image readImage = ReadImageForMessage(message);
                    args.Graphics.DrawImage(readImage, elements.ReadRect);

                    // Draw author name
                    args.Graphics.DrawString(message.Author, _font, textBrush, elements.AuthorRect);

                    // Draw separator
                    const string separatorChar = "•";
                    args.Graphics.DrawString(separatorChar, _font, textBrush, elements.Separator1Rect);

                    // Draw date
                    string dateString = (_showFullDate)
                        ? message.Date.ToString("d MMM yyyy") + " " + message.Date.ToShortTimeString()
                        : message.Date.FriendlyString(true);
                    args.Graphics.DrawString(dateString, _font, textBrush, elements.DateRect);

                    // Draw ID field
                    string idString = (message.IsDraft)
                        ? "Draft"
                        : message.RemoteID.ToString(CultureInfo.InvariantCulture);
                    args.Graphics.DrawString(idString, _font, textBrush, elements.IDRect);

                    // Another separator
                    args.Graphics.DrawString(separatorChar, _font, textBrush, elements.Separator2Rect);

                    // Draw subject line
                    string subjectLine = message.Subject;
                    args.Graphics.DrawString(subjectLine, _font, textBrush, elements.SubjectRect, new StringFormat(StringFormatFlags.NoWrap));
                }
            }
        }


        /// <summary>
        /// Return the appropriate read icon for this message based on its unread and priority state
        /// </summary>
        private static Image ReadImageForMessage(InboxConversation conversation)
        {
            if (conversation.LastError)
            {
                return Resources.Error1;
            }
            return (conversation.UnreadCount > 0) ? Resources.UnreadMessage : Resources.ReadMessage;
        }

        /// <summary>
        /// Handle click on read icons on an item to change the read state of
        /// the conversation.
        /// </summary>
        private void OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            ListViewItem item = inboxConversations.GetItemAt(mouseEventArgs.Location.X, mouseEventArgs.Location.Y);
            if (item != null)
            {
                if (item.Index >= 0 && item.Index < _conversations.Count)
                {
                    InboxConversation message = _conversations[item.Index];
                    DrawRectElements elements = ComputeDrawRectElements(inboxConversations.CreateGraphics(), message, item.Bounds);

                    if (elements.ReadRect.Contains(mouseEventArgs.Location))
                    {
                        Action(ActionID.Read, message);
                    }
                }
            }
        }

        /// <summary>
        /// The selection has changed so draw the new selection.
        /// </summary>
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (SelectedRow >= 0)
            {
                DisplaySelectedRow(true);
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

                InboxConversation message = _conversations[newSelection];
                string messageAddress = String.Format("cixmailbox:inbox:{0}", message.RemoteID);
                FoldersTree.MainForm.AddBacktrack(messageAddress, message.UnreadCount > 0);

                ShowMessage(message);
                FoldersTree.MainForm.RunEvent(EventID.MessageSelected, message);
                inboxConversations.Focus();
            }
        }

        /// <summary>
        /// Invoked when the split container is resized.
        /// </summary>
        private void inboxSplitContainer_SizeChanged(object sender, EventArgs e)
        {
            ResizeMessagePanel();
        }

        /// <summary>
        /// Resize the conversation list pane to fill the 1st SplitView panel area.
        /// There seems to be some issues with autolayout resizing so this needs to be done manually.
        /// </summary>
        private void ResizeMessagePanel()
        {
            inboxConversations.Height = inboxSplitview.Panel1.Height;
            inboxConversations.Width = inboxSplitview.Panel1.Width;
            inboxConversations.Columns[0].Width = inboxConversations.ClientRectangle.Width;
        }

        /// <summary>
        /// Invoked when the inbox view is resized.
        /// </summary>
        private void InboxView_Resize(object sender, EventArgs e)
        {
            ResizeMessagePanel();
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
    }
}